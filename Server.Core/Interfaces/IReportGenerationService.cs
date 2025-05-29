using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Interfaces
{
    public interface IReportGenerationService
    {
        Task<string> GenerateAcceptanceActHtml(AcceptanceActRequestDto request);
        Task<string> GenerateTransferActHtml(TransferActRequestDto request);
    }
}
