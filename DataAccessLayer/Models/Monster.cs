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

        public Monster()
        {
        }
    }
}
