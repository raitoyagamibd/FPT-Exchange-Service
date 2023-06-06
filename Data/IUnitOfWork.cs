using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }


        Task<int> SaveChanges();
        IDbContextTransaction Transaction();
    }
}
