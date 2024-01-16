namespace MyBank.API.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string PasswordSalt { get; set; } = Guid.NewGuid().ToString("D");
    }
}
