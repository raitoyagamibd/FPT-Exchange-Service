using Data.Models.Filters;
using Data.Models.Request.Post;
using Data.Models.Request.Put;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> GetUser(Guid id);
        Task<IActionResult> GetUsers(UserFilterModel filter);
        Task<IActionResult> RegisterCustomer(RegisterCustomerRequest request);
        Task<IActionResult> AddAvatar(Guid id, IFormFile file);
        Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerRequest request);
        Task<IActionResult> DeleteCustomer(Guid id);
    }
}
