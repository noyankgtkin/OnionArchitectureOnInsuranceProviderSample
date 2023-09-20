using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InsuranceProviders.Domain.Enums.Enums;

namespace InsuranceProviders.Application.Wrappers
{
    public class BaseResponse
    {
        public RetCode ReturnCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
