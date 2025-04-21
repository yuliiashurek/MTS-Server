using Server.Data.Db;
using Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data.Repositories.Interfaces;


namespace Server.Data.Repositories.Implementations
{
    public class WarehouseRepository : Repository<Warehouse>, IRepository<Warehouse>
    {
        public WarehouseRepository(AppDbContext context) : base(context)
        {
        }
    }

}
