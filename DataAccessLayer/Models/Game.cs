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
        public List<Object> Inventory { get; set; }

        public List<Weapon> Weapons { get; set; }
        public List<Object> UsedObjects { get; set; }

        public Game()
        {
        }
        public Game(string name)
        {
            this.name = name;
            this.Monsters = new List<Monster>();
            this.Inventory = new List<Object>();
            this.Weapons = new List<Weapon>();
            this.UsedObjects = new List<Object>();
        }

        public double getTotalDefenseBoost()
        {
            double totalDefenseBoost = 0;
            this.UsedObjects.ForEach(delegate (Object obj)
            {
                totalDefenseBoost += obj.DefenseBoost;
            });
            return totalDefenseBoost > 0.5 ? 0.5 : totalDefenseBoost;
        }
        public double getTotalAttackBoost()
        {
            double totalAttackBoost = 0;
            this.UsedObjects.ForEach(delegate (Object obj)
            {
                totalAttackBoost += obj.AttackStrengthBoost;
            });
            return totalAttackBoost > 0.5 ? 0.5 : totalAttackBoost;
        }

    }
}
