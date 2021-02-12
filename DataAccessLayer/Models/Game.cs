using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Game
    {
        public int Id { get; set; }

        public Player player { get; set; }
        public List<Monster> Monsters { get; set; }
        public List<Object> inventory { get; set; }

        public Weapon Weapon { get; set; }

        public Game()
        {
        }
    }
}
