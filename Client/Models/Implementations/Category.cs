using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client
{
    public class Category : INamedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public bool IsPersisted { get; set; }

        [NotMapped]
        public string? PreviousName { get; set; }
    }
}
