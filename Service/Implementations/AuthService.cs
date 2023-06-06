using AutoMapper;
using Data;
using Data.Entities;
using Data.Models.Internal;
using Data.Models.Request.Post;
using Data.Models.View;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.Settings;

namespace Service.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppSetting _appSettings;

        private readonly IUserRepository _userRepository;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = unitOfWork.User;
        }

        public async Task<AuthModel?> GetUserById(Guid id)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id)).Include(user => user.Role).FirstOrDefaultAsync();
            if(user != null)
            {
                return new AuthModel
                {
                    Id = user.Id,
                    Username = user.Name,
                    Role = user.Role.Name
                };
            }
            return null;
        }

        public async Task<AuthViewModel> AuthenticatedUser(AuthRequest auth)
        {
            var user = await _userRepository.GetMany(user => user.Email.Equals(auth.Email) && user.Password.Equals(auth.Password)).Include(user => user.Role).FirstOrDefaultAsync();
            if (user != null)
            {
                var token = GenerateJwtToken(new AuthModel
                {
                    Id = user.Id,
                    Username = user.Email,
                    Role = user.Role.Name
                });

                return new AuthViewModel
                {
                    AccessToken = token
                };
            }

            return null!;
        }

        private string GenerateJwtToken(AuthModel auth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", auth.Id.ToString()),

                    new Claim("role", auth.Role.ToString()),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
