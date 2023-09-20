using AutoMapper;
using InsuranceProviders.Application.Interfaces;
using InsuranceProviders.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Application.Features
{
    public class GetProposalHandler : IRequestHandler<GetProposalRequest, ServiceResponse<GetProposalResult>>
    {
        private readonly IMapper mapper;
        private readonly ISompoJapan sompoJapan;

        public GetProposalHandler(ISompoJapan sompoJapan, IMapper mapper)
        {
            this.mapper = mapper;
            this.sompoJapan = sompoJapan;
        }

        public async Task<ServiceResponse<GetProposalResult>> Handle(GetProposalRequest request, CancellationToken cancellationToken)
        {
            GetProposalResult response = await sompoJapan.GetProposal(request);
            var dto = mapper.Map<GetProposalResult>(response);

            return new ServiceResponse<GetProposalResult>(dto);
        }
    }
}
