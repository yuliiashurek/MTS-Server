using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Warehouse: INamedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string? PreviousName { get; set; }    
    }

}
