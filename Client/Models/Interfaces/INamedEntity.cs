using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface INamedEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string? PreviousName { get; set; }
    }
}
