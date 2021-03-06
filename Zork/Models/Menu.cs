﻿using System;
using System.Collections.Generic;

namespace Zork.Models
{
    public static class Menu
    {
        private static List<Option> options;

        public static string lastMoveDescription;

        public static void DisplayMenu(List<Option> optionsList, Option specialAttack = null)
        {
            options = optionsList;

            // Set the default index of the selected item to be the first
            int index = 0;

            // Write the menu out
            WriteMenu(options, options[index]);

            // Store key info in here
            ConsoleKeyInfo keyinfo;

            int countArrowDown = 0;
            do
            {
                keyinfo = Console.ReadKey();

                // Handle each key input (down arrow will write the menu again with a different selected item)
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (specialAttack != null)
                    {
                        countArrowDown++;

                        if(countArrowDown == 5)
                        {
                            optionsList.Add(specialAttack);
                        }
                    }
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                // Handle different action for the option
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    lastMoveDescription = null;
                    options[index].Selected.Invoke();
                    break;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

        }

        static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            if (!string.IsNullOrWhiteSpace(lastMoveDescription))
            {
                Console.WriteLine(lastMoveDescription);
            }

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }
    }
}
