using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string name { get; set; }
        public Player player { get; set; }
        public List<Monster> Monsters { get; set; }
        public List<Object> Loots { get; set; }
        public List<Weapon> Weapons { get; set; }

        public Game()
        {
        }

        public Game(string name)
        {
            this.name = name;
            Monsters = new List<Monster>();
            Loots = new List<Object>();
            Weapons = new List<Weapon>();
        }
    }
}
