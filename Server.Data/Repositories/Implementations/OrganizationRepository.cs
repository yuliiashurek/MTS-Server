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
    public class OrganizationRepository : Repository<Organization>, IRepository<Organization>
    {
        public OrganizationRepository(AppDbContext context) : base(context) { }
    }

}
