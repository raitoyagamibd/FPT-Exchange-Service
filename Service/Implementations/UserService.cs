using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Data.Entities;
using Data.Models.Filters;
using Data.Models.Request.Post;
using Data.Models.Request.Put;
using Data.Models.View;
using Data.Repositories.Interfaces;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Utility.Constants;

namespace Service.Implementations
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userRepository = unitOfWork.User;
        }


        public async Task<IActionResult> GetUser(Guid id)
        {
            var customer = await _userRepository.GetMany(customer => customer.Id.Equals(id))
                                                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                                                .FirstOrDefaultAsync();
            if (customer != null)
            {
                return new JsonResult(customer);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetUsers(UserFilterModel filter)
        {
            var query = _userRepository.GetAll();
            if(filter.Name != null)
            {
                query = query.Where(user => user.Name.Contains(filter.Name));
            }
            if(filter.Email != null)
            {
                query = query.Where(user => user.Email.Contains(filter.Email));
            }
            var users = await query.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(users);
        }


        public async Task<IActionResult> RegisterCustomer(RegisterCustomerRequest request)
        {
            if(_userRepository.Any(user => user.Email.Equals(request.Email)))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                RoleId = Guid.Parse("61b99726-8e73-4a0c-be55-c7855bb0a0c3"),
                Status = UserStatus.Active.ToString(),
            };

            _userRepository.Add(user);
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetUser(user.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        public async Task<IActionResult> AddAvatar(Guid id, IFormFile file)
        {
            var user = await _userRepository.GetMany(user => user.Id.Equals(id)).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Avatar = await UploadProductImageToFirebase(file);
                _userRepository.Update(user);
                var result = await _unitOfWork.SaveChanges();
                if (result > 0)
                {
                    return await GetUser(user.Id);
                }
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerRequest request)
        {
            var customer = await _userRepository.GetMany(customer => customer.Id.Equals(id)).FirstOrDefaultAsync();
            if(customer == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            customer.Name = request.Name ?? customer.Name;
            customer.Password = request.Password ?? customer.Password;
            customer.Status = request.Status ?? customer.Status;

            _userRepository.Update(customer);

            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return await GetUser(customer.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _userRepository.GetMany(customer => customer.Id.Equals(id)).FirstOrDefaultAsync();
            if (customer == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            customer.Status = UserStatus.DeActive.ToString();
            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        private async Task<string?> UploadProductImageToFirebase(IFormFile file)
        {
            var storage = new FirebaseStorage("e-gift-6276a.appspot.com");
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var imageUrl = await storage.Child("images")
                                        .Child(imageName)
                                        .PutAsync(file.OpenReadStream());
            return imageUrl;
        }
    }
}
