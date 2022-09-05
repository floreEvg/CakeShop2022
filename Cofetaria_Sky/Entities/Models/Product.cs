namespace Cofetaria_Sky.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Category { get; set; }
    
        public string Name { get; set; }

        public string Image { get; set; }

        public string Info { get; set; }

        public float Price { get; set; }

        public bool Stock {get; set;}
    }
}
