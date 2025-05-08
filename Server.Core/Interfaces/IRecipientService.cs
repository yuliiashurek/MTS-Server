using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{
    public interface IRecipientService
    {
        Task<RecipientDto?> GetByNameAsync(string name);
        Task<RecipientDto> CreateAsync(RecipientDto dto);
        Task<List<RecipientDto>> GetAllAsync();

        Task<RecipientDto> GetByIdAsync(Guid id);

    }

}
