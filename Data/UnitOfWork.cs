using Data.Entities;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FptExchangeDbContext _context;

        private IUserRepository _user = null!;

        public UnitOfWork(FptExchangeDbContext context)
        {
            _context = context;
        }

        public IUserRepository User
        {
            get { return _user ??= new UserRepository(_context); } 
        }
        

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
