namespace Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}