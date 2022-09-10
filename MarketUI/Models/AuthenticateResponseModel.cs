namespace MarketUI.Models
{
    public class AuthenticateResponseModel
    {
        public AuthenticateResponseModel()
        {
        }

        public AuthenticateResponseModel(UserModel user, string token)
        {
            Name = user.Name;
            Surname = user.Surname;
            IsAdmin = user.IsAdmin;
            Token = token;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
    }
}