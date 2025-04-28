using Server.Data.Db;
using Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Repositories.Implementations
{
    public class MaterialItemRepository : Repository<MaterialItem>
    {
        public MaterialItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
