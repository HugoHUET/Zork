using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Zork.Models;
using Object = DataAccessLayer.Models.Object;
using System.Configuration;

namespace Zork
{
    public class Gameplay
    {
        private Game game;
        private readonly ZorkDbContext context;
        private double MonsterRate;
        private int PlayerHP;

        Random random;

        public Gameplay()
        {
            string DbServer = ConfigurationManager.AppSettings.Get("DbServer");
            string DbUsername = ConfigurationManager.AppSettings.Get("DbUsername");
            string DbPassword = ConfigurationManager.AppSettings.Get("DbPassword");
            string DbName = ConfigurationManager.AppSettings.Get("DbName");

            string[] args = { DbServer, DbName, DbUsername, DbPassword };

            DbContextFactory dbFactory = new DbContextFactory();
            this.context = dbFactory.CreateDbContext(args);
            random = new Random();

            MonsterRate = Convert.ToDouble(ConfigurationManager.AppSettings.Get("MonsterRate"));
            PlayerHP = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PlayerHP"));
        }

        public void create()
        {
            string gameName;
            do
            {
                Console.WriteLine("Veuillez saisir le nom de la partie :");
                gameName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(gameName));


            game = new Game(gameName);

            Player player = new Player(null, PlayerHP, 0);
            Weapon rustedToothPick = new Weapon("rustedToothPick", 5, 0.1);
            Weapon nerfGun = new Weapon("nerfGun", 1, 0.5);

            player.Weapons.Add(rustedToothPick);

            Object painDeMie = new Object("painDeMie", 2, 1, 1);

            // Easter egg : L'huile de foie de morue est deg donc elle enlève des HP mais elle renforce a mort
            Object huileDeFoieDeMorue = new Object("huileDeFoieDeMorue", -3, 3, 5);
            Object bananaSkin = new Object("bananaSkin", 2, 1, 1);

            initMonsters();

            game.player = player;

            game.Weapons.Add(rustedToothPick);
            game.Weapons.Add(nerfGun);

            game.Loots.Add(painDeMie);
            game.Loots.Add(huileDeFoieDeMorue);
            game.Loots.Add(bananaSkin);

            game = context.Games.Add(game).Entity;
            this.context.SaveChanges();

            this.run();
        }

        //Chargement d'une partie
        public void load()
        {
            var options = new List<Option>();
            context.Games.Include(g => g.player).Include(g => g.Loots).Include(g => g.Monsters).Include(g => g.Weapons).ToList().ForEach((game) =>
            {
                options.Add(new Option(game.name, () =>
                {
                    this.game = game;
                    run();
                }));
            });

            if (options.Count == 0)
            {
                Menu.lastMoveDescription = "Vous n'avez pas de partie de sauvegardée";
            }

            options.Add(new Option("Retour", () => { }));

            Menu.DisplayMenu(options);
        }

        //Boucle de la partie
        private void run()
        {
            bool wantToexit = false;
            while (wantToexit == false && shouldEndGame() != true)
            {
                var options = new List<Option>
                {
                    new Option("Afficher l’inventaire", () => listAndUseItems()),
                    new Option("Afficher les stats", () => displayStats()),
                    new Option("Se déplacer", () => selectDirection()),
                    new Option("Sauvegarder et quitter", () => {
                        updateGame();
                        Menu.lastMoveDescription = "Partie sauvegardée avec succès";
                        wantToexit= true;
                    })
                };

                Menu.DisplayMenu(options);

            } ;
        }

        //Affichage des statistiques du joueurs
        private void displayStats()
        {
            var options = new List<Option>{ new Option("Retour", () => { }) };

            Menu.lastMoveDescription = $"HP : {game.player.Hp}\nExperience : {game.player.Xp}\n";

            Menu.DisplayMenu(options);
        }

        //Sélecteur de direction
        private void selectDirection()
        {
            var options = new List<Option>
            {
                new Option("Nord", () => move()),
                new Option("Sud", () => move()),
                new Option("Est", () => move()),
                new Option("Ouest", () => move()),
                new Option("Retour", () => { }),
            };

            Menu.DisplayMenu(options);
        }

        //Arrivé sur une nouvelle case
        private void move()
        {
            Menu.lastMoveDescription = $"{getLocationDescription()}.\n";
            if (random.NextDouble() < MonsterRate) // x% de chance de tomber sur un monstre
            {
                int index = random.Next(game.Monsters.Count());
                Monster monster = game.Monsters.ToList()[index];
                Menu.lastMoveDescription += $"Attention, un(e) {monster.Name} sauvage apparait !\n";

                fight(monster);
            }
            else
            {
                int index = random.Next(context.Objects.Count());
                Object objet = context.Objects.ToList()[index];
                game.player.Inventory.Add(objet);
                updateGame();

                Menu.lastMoveDescription += $"Vous avez trouvé {objet.Name}\n";
            }
        }

        //Boucle de combat
        private void fight(Monster monster)
        {
            bool runAway = false;
            bool canChooseRunAway = true;
            do
            {
                string tmp = Menu.lastMoveDescription;
                var options = new List<Option>
                {
                    new Option("Utiliser un objet", () => {
                        bool isReturn = listAndUseItems();
                        if(isReturn)
                        {
                            Menu.lastMoveDescription = tmp;
                        }
                    }),
                    new Option($"Attaquer {monster.Name}", () => {
                        listAnsUseWeapons(monster);
                        canChooseRunAway = true;
                    })
                };

                if(canChooseRunAway)
                {
                    options.Add(new Option("Prendre la fuite", () =>
                        {
                            if (canRunAway(monster))
                            {
                                Menu.lastMoveDescription = "Vous avez pris la fuite...\n";
                                runAway = true;
                            }
                            else
                            {
                                Menu.lastMoveDescription = "Impossible de s'enfuir, le monstre vous a rattrapé !\n";
                                canChooseRunAway = false;
                            }
                        }));
                }

                Menu.DisplayMenu(options);

            } while (game.player.Hp > 0 && monster.Hp > 0 && runAway == false);
        }

        //Sélecteur d'items
        private bool listAndUseItems()
        {
            bool isReturn = false;
            var options = new List<Option>();
            game.player.Inventory.ForEach((objet) =>
            {
                options.Add(new Option($"{objet.Name} : + {objet.HPRestoreValue} HP, + {objet.AttackStrengthBoost} de boost d'attaque, + {objet.DefenseBoost} de boost de défense", () => useItem(objet)));
            });

            if (options.Count == 0)
            {
                Menu.lastMoveDescription = "Vous n'avez pas d'objet dans votre inventaire.";
            }

            options.Add(new Option("Retour", () => {
                isReturn = true;
            }));

            Menu.DisplayMenu(options);

            return isReturn;
        }

        //Sélecteur d'armes
        private void listAnsUseWeapons(Monster monster)
        {
            var options = new List<Option>();
            game.player.Weapons.ForEach((weapon) =>
            {
                options.Add(new Option(weapon.name, () => attack(monster, weapon)));
            });

            if (options.Count > 0)
            {
                Weapon specialWeapon = new Weapon("KAMEAMEMA", 1000, 0);
                Menu.DisplayMenu(options, new Option(specialWeapon.name, () => attack(monster, specialWeapon)));
            }
        }

        //Système de consomation d'un item
        private void useItem(Object item)
        {
            if (game.player.Inventory.Contains(item))
            {
                game.player.Hp += item.HPRestoreValue;
                game.player.Inventory.Remove(item);
                game.player.UsedObjects.Add(item);

                Menu.lastMoveDescription = $"Vous utilisez {item.Name}\n";
                Menu.lastMoveDescription += $"+ {item.HPRestoreValue} HP\n";
                Menu.lastMoveDescription += $"+ {item.AttackStrengthBoost} de boost d'attaque\n";
                Menu.lastMoveDescription += $"+ {item.DefenseBoost} de boost de défense\n";

                this.updateGame();
            }
            else
            {
                Menu.lastMoveDescription = "Vous ne possédez pas cet item\n";
            }
        }

        // Système d'attaque d'un monstre avec une arme
        private void attack(Monster monster, Weapon weapon)
        {
            if (random.NextDouble() > weapon.MissRate)
            {
                monster.Hp -= Convert.ToInt32(weapon.Damages * (1.0 + game.player.getTotalAttackBoost()));
                Menu.lastMoveDescription = $"Vous infligez { Convert.ToInt32(weapon.Damages * (1.0 + game.player.getTotalAttackBoost())) } point(s) de dégats. ";

                if (monster.Hp <= 0)
                {
                    game.Monsters.Remove(monster);
                    updateGame();

                    Menu.lastMoveDescription += $"{monster.Name} est mort\n";

                    //On récupère l'XP et les loots à la fin du combat
                    getXp(monster);
                    getLoot(monster);
                }
                else
                {
                    Menu.lastMoveDescription += $"Il reste {monster.Hp} HP à {monster.Name}\n";
                    context.Monsters.Update(monster);
                    context.SaveChanges();

                    getAttacked(monster);
                }
            }
            else
            {
                Menu.lastMoveDescription = "Oops ! Echec critique\n";

                getAttacked(monster);
            }
        }

        // Si le monstre est plus haut lvl on gagne beaucoup d'XP
        // Si notre lvl est le double de celui du monstre on ne gagne pas xp
        private void getXp(Monster monster)
        {
            double randomFactor = 1 + random.NextDouble();
            int lvlDiff = monster.Level - (game.player.Xp / 1000);
            int xpToGet = lvlDiff + monster.Level;

            if (xpToGet < 0)
            {
                xpToGet = 0;
            }

            game.player.Xp += Convert.ToInt32(xpToGet * randomFactor);

            updateGame();

            Menu.lastMoveDescription += $"Vous avez gagné { Convert.ToInt32(xpToGet * randomFactor) } XP !\n";
        }

        //Récupère un loot aléatoire. Le %drop est augmenté si le monstre a un lvl important par rapport au joueur
        // Si le monstre a un lvl trop faible par rapport au joueur, le drop est de 0%
        private void getLoot(Monster monster)
        {
            double lootRate = 0.3 + (monster.Level - game.player.Xp / 1000) / 100;
            if (random.NextDouble() < lootRate)
            {
                int index = random.Next(game.Weapons.Count);
                Menu.lastMoveDescription = $"Vous avez trouvé { game.Weapons[index].name }\n";
                if (game.player.Weapons.Contains(game.Weapons[index]))
                {
                    Menu.lastMoveDescription += "Pas de chance vous possédez déjà cette arme\n";
                }
                else
                {
                    game.player.Weapons.Add(game.Weapons[index]);
                    updateGame();
                }
            }
            else
            {
                Menu.lastMoveDescription += $"Pas de chance, {monster.Name} n'a pas d'arme sur lui\n";
            }

        }

        //Le monstre attaque le joueur
        private void getAttacked(Monster monster)
        {
            if(monster.Hp > 0)
            {
                Menu.lastMoveDescription += $"Attention {monster.Name} vous attaque :\n";
                if (random.NextDouble() > monster.MissRate)
                {
                    game.player.Hp -= Convert.ToInt32(monster.Damages * (1.0 - game.player.getTotalDefenseBoost()));
                    Menu.lastMoveDescription += $" {monster.Name} vous inflige { Convert.ToInt32(monster.Damages * (1.0 - game.player.getTotalDefenseBoost())) } point(s) de dégats\n";
                    updateGame();
                }
                else
                {
                    Menu.lastMoveDescription += $" Coup de bol ! { monster.Name } a fait un échec critique\n";
                }
                Menu.lastMoveDescription += $" Il vous reste {game.player.Hp} HP\n";
            }
        }

        // Verifie si la game doit être terminée et met un message en conséquence
        private bool shouldEndGame()
        {
            if (game.player.Hp > 0)
            {
                if (game.Monsters.Count == 0)
                {
                    Menu.lastMoveDescription = "Victoire ! Vous avez battu tous les monstres félicitations !\n";
                    context.Games.Remove(game);
                    context.SaveChanges();
                    return true;
                }

                Menu.lastMoveDescription += $"Vous avez { game.player.Hp } HP\nIl reste { game.Monsters.Count } monstres à vaincre sur la carte\n";

                return false;
            }
            Menu.lastMoveDescription = "Défaite... Vous aurez plus de chance la prochaine fois !\n";

            context.Games.Remove(game);
            context.SaveChanges();
            return true;
        }

        // Plus le monstre et le joueur ont un niveau proche, moins la chance de s'échapper est grande.
        // On estime que si le monstre est trop haut niveau le joueur peux s'enfuir pour éviter de mourir
        // A l'inverse si le joueur peut éclater le monstre facilement, il peut également esquiver le combat pour gagner du temps
        private bool canRunAway(Monster monster)
        {
            double runAwayRate = 0.5 + Math.Abs((game.player.Xp / 1000 - monster.Level) / 100);
            return random.NextDouble() < runAwayRate;
        }

        // Met à jour la game dans la bdd
        private void updateGame()
        {
            context.Games.Update(game);
            context.SaveChanges();
        }

        // Retourne la description du lieu
        private String getLocationDescription()
        {
            string[] InFormOfStringArray = ConfigurationManager.AppSettings["LocationsDescription"].Split(',').Select(s => s.Trim()).ToArray();
            List<string> list = new List<string>(InFormOfStringArray);
            return list[random.Next(list.Count)];
        }

        //Initialise les monstres aléatoirement
        private void initMonsters()
        {
            Monster kwoakGaming = new Monster("kwoakGaming", 3, 0.2, 20, 12);
            Monster cSharpDev = new Monster("cSharpDev", 5, 0.1, 40, 40);

            int MonsterNb = Convert.ToInt32(ConfigurationManager.AppSettings["MonsterNb"]);

            for (int i = 0; i < MonsterNb; i++)
            {
                if (random.NextDouble() > 0.5)
                {
                    game.Monsters.Add(kwoakGaming);
                }
                else
                {
                    game.Monsters.Add(cSharpDev);
                }
            }
            
        }

    }
}
