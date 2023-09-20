using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Domain.Enums
{
    public class Enums
    {
        public enum RetCode
        {
            [EnumMember]
            Success = 1,
            [EnumMember]
            Fail,
            [EnumMember]
            RecordNotFound,
            [EnumMember]
            DuplicateRecord,
        }
    }
}
