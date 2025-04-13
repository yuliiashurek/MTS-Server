using Server.Data.Entities;
using Server.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Supplier> Suppliers { get; }
        Task<int> SaveChangesAsync();

    }

}
