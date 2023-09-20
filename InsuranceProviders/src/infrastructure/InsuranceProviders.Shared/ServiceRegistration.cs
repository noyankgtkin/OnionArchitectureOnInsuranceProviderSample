using InsuranceProviders.Application.Interfaces;
using InsuranceProviders.Shared.Services.CascoServices.SompoJapan;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedRegistration(this IServiceCollection services)
        {
            services.AddTransient<ISompoJapan, SompoJapan>();
        }
    }
}
