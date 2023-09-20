using InsuranceProviders.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Application.Features
{
    public class GetProposalResult : BaseResponse
    {
        public string InsuranceFirmName { get; set; }

        public decimal PremiumAmount { get; set; }

        public string Err { get; set; }
    }
}
