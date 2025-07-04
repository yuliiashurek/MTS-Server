﻿using Server.Data.Db;
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
    public class MeasurementUnitRepository : Repository<MeasurementUnit>, IRepository<MeasurementUnit>
    {
        public MeasurementUnitRepository(AppDbContext context) : base(context)
        {
        }
    }

}
