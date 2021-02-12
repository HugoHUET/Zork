using System;
namespace DataAccessLayer.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int Damages { get; set; }
        public double MissRate { get; set; }

        public Weapon()
        {
        }
    }
}
