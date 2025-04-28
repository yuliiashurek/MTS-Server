using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.Models
{

        public partial class MaterialItem
        {
            public Guid Id { get; set; }
            
            public Guid OrganizationId {  get; set; }
            
            public string Name {  get; set; }
            
            public Guid MeasurementUnitId { get; set; }
            
            public decimal MinimumStock {  get; set; }
            
            public Guid CategoryId { get; set; }
            
            public Guid SupplierId { get; set; }
        }
}
