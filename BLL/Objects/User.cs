namespace BLL.Objects
{
    public class User : BaseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
    }
}