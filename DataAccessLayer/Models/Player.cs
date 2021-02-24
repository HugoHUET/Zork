using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Xp { get; set; }
        public List<Object> UsedObjects { get; set; }
        public List<Object> Inventory { get; set; }
        public List<Weapon> Weapons { get; set; }

        public Player(string name, int hp, int xp)
        {
            Name = name;
            Hp = hp;
            Xp = xp;
            UsedObjects = new List<Object>();
            Inventory = new List<Object>();
            Weapons = new List<Weapon>();
        }
        public double getTotalDefenseBoost()
        {
            double totalDefenseBoost = 0;
            this.UsedObjects.ForEach(delegate (Object obj)
            {
                totalDefenseBoost += obj.DefenseBoost;
            });
            totalDefenseBoost /= 20;
            return totalDefenseBoost > 0.5 ? 0.5 : totalDefenseBoost;
        }
        public double getTotalAttackBoost()
        {
            double totalAttackBoost = 0;
            this.UsedObjects.ForEach(delegate (Object obj)
            {
                totalAttackBoost += obj.AttackStrengthBoost;
            });
            totalAttackBoost /= 20;
            return totalAttackBoost > 0.5 ? 0.5 : totalAttackBoost;
        }
    }
}
