using System;
using System.Collections.Generic;
using Zork.Models;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Gameplay gameplay = new Gameplay();
            var options = new List<Option>
            {
                new Option("Create new game", () => {
                    gameplay.create();
                }),
                new Option("Load saved game", () => {
                    gameplay.load();
                }),
                new Option("About", () => {
                    Console.Clear();
                    Console.WriteLine("Paul Lereverend et Hugo Huet");
                    Console.WriteLine("\nAppuyer sur n'importe quelle touche pour retourner au menu précédent");
                    Console.ReadKey();
                }),
                new Option("Exit", () => Environment.Exit(0)),
            };

            while (true)
            {
                Menu.DisplayMenu(options);
            }
            

        }
    }
}
