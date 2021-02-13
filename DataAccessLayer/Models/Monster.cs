using System;
namespace DataAccessLayer.Models
{
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damages { get; set; }
        public double MissRate { get; set; }
        public int Hp { get; set; }

        public Monster(string name, int damages, double missRate, int hp)
        {
            Name = name;
            Damages = damages;
            MissRate = missRate;
            Hp = hp;
        }

        public Monster()
        {
        }
    }
}
