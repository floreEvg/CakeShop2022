using System;

namespace Cofetaria_Sky.Entities.Models
{
    public class Order
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public float Total { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public string Payment { get; set; }
    }
}
