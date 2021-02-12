using System;
using System.Collections.Generic;
using System.Threading;
using Zork.Models;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new List<Option>
            {
                new Option("Create new game", () => {
                    Game.create();
                }),
                new Option("Load saved game", () => {
                    Game.load();
                }),
                new Option("About", () => {
                    WriteTemporaryMessage("Paul Lereverend et Hugo Huet");
                }),
                new Option("Exit", () => Environment.Exit(0)),
            };

            Menu.DisplayMenu(options);
        }

        // Default action of all the options. You can create more methods
        static void WriteTemporaryMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
        }
    }
}
