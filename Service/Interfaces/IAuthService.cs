using Data.Models.Internal;
using Data.Models.Request.Post;
using Data.Models.View;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel?> GetUserById(Guid id);
        Task<AuthViewModel> AuthenticatedUser(AuthRequest auth);
    }
}
