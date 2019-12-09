namespace Zwaj.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Passwordhash { get; set; }
        public byte[] PasswordsSalt { get; set; }
    }
}