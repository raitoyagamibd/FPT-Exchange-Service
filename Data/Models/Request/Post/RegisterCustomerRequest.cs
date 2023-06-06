namespace Data.Models.Request.Post
{
    public class RegisterCustomerRequest
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

    }
}
