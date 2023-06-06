namespace Data.Models.Internal
{
    public class AuthModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
