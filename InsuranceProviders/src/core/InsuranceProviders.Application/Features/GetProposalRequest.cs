using InsuranceProviders.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Application.Features
{
    public class GetProposalRequest : IRequest<ServiceResponse<GetProposalResult>>
    {
        public int ProductCode { get; set; }
        public int InstallmentCount { get; set; }
    }
}
