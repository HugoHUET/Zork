using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Models;
using Zork.Models;

namespace Zork
{
    public class Gameplay
    {
        private string gameName;

        public Gameplay()
        {
        }

        public void create()
        {
            string gameName;
            do
            {
                Console.WriteLine("Veuillez saisir le nom de la partie :");
                gameName = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(gameName));

            this.gameName = gameName;

            Game game = new Game();
            Player player = new Player();
            Weapon rustedToothPick = new Weapon();
            Weapon nerfGun = new Weapon();

            DataAccessLayer.Models.Object painDeMie = new DataAccessLayer.Models.Object();
            DataAccessLayer.Models.Object huileDeFoieDeMorue = new DataAccessLayer.Models.Object();
            DataAccessLayer.Models.Object bananaSkin = new DataAccessLayer.Models.Object();

            Monster kwoakGaming = new Monster();
            Monster cSharpDev = new Monster();

            game.Monsters.Add(kwoakGaming);
            game.Monsters.Add(cSharpDev);

            game.player = player;

            game.Weapons.Add(rustedToothPick);
            game.Weapons.Add(nerfGun);

            game.Objects.Add(painDeMie);
            game.Objects.Add(huileDeFoieDeMorue);
            game.Objects.Add(bananaSkin);

            game.name = this.gameName;

            this.run();
        }

        public void load()
        {

        }

        private void run()
        {
            Console.WriteLine(this.context.Games.First().Id);
            var options = new List<Option>
            {
                new Option("Afficher l’inventaire", () => {
                    //TODO
                }),
                new Option("Afficher les stats", () => {
                    //TODO
                }),
                new Option("Se déplacer", () => {
                    //TODO
                })
            };

            //Menu.DisplayMenu(options);
        }
    }
}
