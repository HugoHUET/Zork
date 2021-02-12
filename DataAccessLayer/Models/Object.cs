using System;
namespace DataAccessLayer.Models
{
    public class Object
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HPRestoreValue { get; set; }
        public int AttackStrengthBoost { get; set; }
        public int DefenseBoost { get; set; }

        public Object()
        {
        }
    }
}
