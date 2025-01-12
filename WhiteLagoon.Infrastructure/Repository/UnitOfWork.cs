using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IVillaRepository Villas { get; private set; }
        public IVillaNumberRepository VillaNumbers { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villas = new VillaRepository(_db);
            VillaNumbers = new VillaNumberRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
