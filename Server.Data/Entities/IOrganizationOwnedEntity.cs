﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Entities
{
    public interface IOrganizationOwnedEntity
    {
        Guid OrganizationId { get; set; }
    }
}
