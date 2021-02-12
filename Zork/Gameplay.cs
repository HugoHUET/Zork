using System;
using System.Collections.Generic;
using System.Linq;
using Zork.Models;

namespace Zork
{
    public class Gameplay
    {
        private static int TAILLE_PLATEAU = 10;
        private List<List<int>> plateau;
        private string gameName;

        public Gameplay(string gameName)
        {
            this.gameName = gameName;
        }

        public static void create()
        {
            string gn;
            do
            {
                Console.WriteLine("Veuillez saisir le nom de la partie :");
                gn = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(gn));

            Random rnd = new Random();
            Gameplay g = new Gameplay(gn);
            int id = 0;
            for (int i = 0; i < TAILLE_PLATEAU; i++)
            {
                for (int j = 0; j < TAILLE_PLATEAU; j++)
                {
                    int type = rnd.Next(0, 10);
                    if(type >= 5 && type <= 7)
                    {
                        //loot
                    }
                    else if(type < 7)
                    {
                        //monster
                    }
                    g.plateau[i][j] = id;
                    Console.Write($" {id}({i},{j}) ");
                    id++;
                }
                Console.WriteLine();
            }

            int playerPosX = rnd.Next(0, TAILLE_PLATEAU);
            int playerPosY = rnd.Next(0, TAILLE_PLATEAU);
            g.run();
        }

        public static void load()
        {

        }

        private void run()
        {
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

            Menu.DisplayMenu(options);
        }

		private void getAvailableDeplacement(int playerPosX, int playerPosY) {
            bool[] availableDeplacement = { false, false, false, false };
            try
            {
                //go top
                if(this.plateau.ElementAt(playerPosX).ElementAt(playerPosY + 1) != null)
                {
                    availableDeplacement[0] = true;
                }

                //go right
                if(this.plateau.ElementAt(playerPosX + 1).ElementAt(playerPosY) != null)
                {
                    availableDeplacement[1] = true;
                }

                //go bottom
                if(this.plateau.ElementAt(playerPosX).ElementAt(playerPosY - 1) != null)
                {
                    availableDeplacement[2] = true;
                }

                //go left
                if(this.plateau.ElementAt(playerPosX - 1).ElementAt(playerPosY) != null)
                {
                    availableDeplacement[3] = true;
                }
            }
            catch { }
		}
    }
}
