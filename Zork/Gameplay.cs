using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using Zork.Models;

namespace Zork
{
    public class Gameplay
    {
        private Game game;
        private readonly ZorkDbContext context;

        public Gameplay()
        {
            DbContextFactory dbFactory = new DbContextFactory();
            this.context = dbFactory.CreateDbContext(null);
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

            // TODO : Sélecteur pour le pseudo et fichier de conf pour les hp de base
            Player player = new Player("torti", 20, 0);
            Weapon rustedToothPick = new Weapon("rustedToothPick", 5, 0.1);
            Weapon nerfGun = new Weapon("nerfGun", 1, 0.5);

            DataAccessLayer.Models.Object painDeMie = new DataAccessLayer.Models.Object("painDeMie", 2, 1, 1);

            // Easter egg : L'huile de foie de morue est deg donc elle enlève des HP mais elle renforce a mort
            DataAccessLayer.Models.Object huileDeFoieDeMorue = new DataAccessLayer.Models.Object("huileDeFoieDeMorue", -3, 3, 5);
            DataAccessLayer.Models.Object bananaSkin = new DataAccessLayer.Models.Object("bananaSkin", 2, 1, 1);

            Monster kwoakGaming = new Monster("kwoakGaming",3 , 0.2, 20, 12);
            Monster cSharpDev = new Monster("cSharpDev", 5, 0.1, 40, 40);

            game.Monsters.Add(kwoakGaming);
            game.Monsters.Add(cSharpDev);

            game.player = player;

            game.Weapons.Add(rustedToothPick);
            game.Weapons.Add(nerfGun);

            game.UsedObjects.Add(painDeMie);
            game.UsedObjects.Add(huileDeFoieDeMorue);
            game.UsedObjects.Add(bananaSkin);

            game = context.Games.Add(game).Entity;
            this.context.SaveChanges();

            this.run();
        }

        public void load()
        {
            game = context.Games.First();
        }

        private void run()
        {
            var options = new List<Option>
            {
                new Option("Afficher l’inventaire", () => {
                    this.displayInventory();
                }),
                new Option("Afficher les stats", () => {
                    this.displayStats();
                }),
                new Option("Se déplacer", () => {
                    this.selectDirection();
                })
            };

            Menu.DisplayMenu(options);
        }

        private void displayInventory()
        {
            if(game.Inventory.Count > 0)
            {
                game.Inventory.ForEach((objet) =>
                {
                    Console.WriteLine($"{objet.Name} : \n Attack : {objet.AttackStrengthBoost}\n Defense : {objet.DefenseBoost}\n HP : {objet.HPRestoreValue}");
                });
            }
            else
            {
                Console.WriteLine("There is no item in your inventory");
            }

            Console.WriteLine("\nPress any key to return");
            Console.ReadKey();
            this.run();
        }

        private void displayStats()
        {
            Console.WriteLine($"Name : {game.player.Name}\nHP : {game.player.Hp}\nExperience : {game.player.Xp}");

            Console.WriteLine("\nPress any key to return");
            Console.ReadKey();
            this.run();
        }

        private void selectDirection()
        {
            var options = new List<Option>
            {
                new Option("North", () => {
                    //TODO
                }),
                new Option("South", () => {
                    //TODO
                }),
                new Option("West", () => {
                    //TODO
                }),
                new Option("East", () => {
                    //TODO
                })
            };

            Menu.DisplayMenu(options);
        }

        private void useItem(DataAccessLayer.Models.Object item)
        {
            if (game.Inventory.Contains(item))
            {
                game.player.Hp += item.HPRestoreValue;
                game.Inventory.Remove(item);
                game.UsedObjects.Add(item);

                this.updateGame();
            }
            else
            {
                Console.WriteLine("Vous ne possédez pas cet item");
            }
            
        }

        private void attack(Monster monster, Weapon weapon)
        {
            Random random = new Random();
            if(random.NextDouble() > weapon.MissRate)
            {
                monster.Hp -= Convert.ToInt32(weapon.Damages * (1.0 + game.getTotalAttackBoost()));
                if (monster.Hp <= 0)
                {
                    game.Monsters.Remove(monster);
                    this.updateGame();
                    Console.WriteLine(monster.Name + " est mort");
                }
                else
                {
                    context.Monsters.Update(monster);
                    context.SaveChanges();
                }
            }
            else
            {
                Console.WriteLine("Oops ! Echec critique");
            }
        }

        // Si le monstre est plus haut lvl on gagne beaucoup d'XP
        // Si notre lvl est le double de celui du monstre on ne gagne pas xp
        private void getXp(Monster monster)
        {
            Random random = new Random();
            double randomFactor = 1 + random.NextDouble();
            int lvlDiff = monster.Level - (game.player.Xp / 1000);
            int xpToGet = lvlDiff + monster.Level;

            if (xpToGet < 0)
            {
                xpToGet = 0;
            }

            game.player.Xp += Convert.ToInt32(xpToGet * randomFactor);
            updateGame();
        }

        //TODO : Récupérer éventuellement un loot en fonction de la différence de niveau
        private void getLoot(Monster monster)
        {

        }
        private void getAttacked(Monster monster)
        {
            Random random = new Random();
            if (random.NextDouble() > monster.MissRate)
            {
                game.player.Hp -= Convert.ToInt32(monster.Damages * (1.0 - game.getTotalDefenseBoost()));
                updateGame();
            }
            else
            {
                Console.WriteLine("Coup de bol ! " + monster.Name + " a fait un échec critique");
            }
        }

        // Verifie si la game doit être terminée et met un message en conséquence
        private Boolean shouldEndGame()
        {
            if (game.player.Hp <= 0)
            {
                Console.WriteLine("Vous avez perdu !");
                return true;
            }

            if(game.Monsters.Count() == 0)
            {
                Console.WriteLine("Vous avez battu tous les monstres félicitations !");
                return true;
            }
            return false;
        }

        // Plus le monstre et le joueur ont un niveau proche, moins la chance de s'échapper est grande.
        // On estime que si le monstre est trop haut niveau le joueur peux s'enfuir pour éviter de mourir
        // A l'inverse si le joueur peut éclater le monstre facilement, il peut également esquiver le combat pour gagner du temps
        private Boolean canRunAway(Monster monster)
        {
            double runAwayRate = 0.5 + Math.Abs((game.player.Xp / 1000 - monster.Level) / 100);
            Random random = new Random();
            return random.NextDouble() < runAwayRate;
        }

        // Met à jour la game dans la bdd
        private void updateGame()
        {
            context.Games.Update(game);
            context.SaveChanges();
        }

    }
}
