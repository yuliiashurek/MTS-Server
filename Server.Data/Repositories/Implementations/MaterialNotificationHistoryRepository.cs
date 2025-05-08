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
    public class MaterialNotificationHistoryRepository : Repository<MaterialNotificationHistory>, IRepository<MaterialNotificationHistory>
    {
        public MaterialNotificationHistoryRepository(AppDbContext context) : base(context)
        {
        }
    }

}
