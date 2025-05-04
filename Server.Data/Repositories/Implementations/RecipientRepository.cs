using Microsoft.EntityFrameworkCore;
using Server.Data.Db;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Repositories.Implementations
{
    public class RecipientRepository : Repository<Recipient>, IRecipientRepository
    {
        public RecipientRepository(AppDbContext context) : base(context) { }

        public async Task<Recipient?> GetByNameAsync(Guid organizationId, string name)
        {
            return await _context.Recipients
                .FirstOrDefaultAsync(r => r.OrganizationId == organizationId && r.Name == name);
        }
    }

}
