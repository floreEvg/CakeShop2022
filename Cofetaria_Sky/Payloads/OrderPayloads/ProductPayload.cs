using System.ComponentModel.DataAnnotations;


namespace Cofetaria_Sky.Payloads.OrderPayloads
{
    public class ProductPayload
    {
        [Required(ErrorMessage = "Categoria este obligatorie")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Imaginea este obligatorie")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Detaliile sunt obligatorii")]
        public string Info { get; set; }

        [Required(ErrorMessage = "Prețul este obligatoriu")]
        public float Price { get; set; }
    }
}
