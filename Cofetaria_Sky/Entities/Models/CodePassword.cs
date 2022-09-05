namespace Cofetaria_Sky.Entities.Models
{
    public class CodePassword
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public string Code { get; set; }
    }
}
