using InsuranceProviders.Application.Dtos;
using InsuranceProviders.Application.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Application.Interfaces
{
    public interface ISompoJapan
    {
        Task<GetProposalResult> GetProposal(GetProposalRequest request);
    }
}
