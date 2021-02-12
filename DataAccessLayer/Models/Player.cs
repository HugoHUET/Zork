using System;
namespace DataAccessLayer.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Xp { get; set; }

        public Player()
        {
        }
    }
}
