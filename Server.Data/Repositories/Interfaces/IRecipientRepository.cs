using Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Repositories.Interfaces
{
    public interface IRecipientRepository : IRepository<Recipient>
    {
        Task<Recipient?> GetByNameAsync(Guid organizationId, string name);
    }

}
