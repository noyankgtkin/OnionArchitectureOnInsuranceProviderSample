using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Application.Behaviors
{
    public class LoggingRequestBehavior<TRequest>
        : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;
        public LoggingRequestBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation($"Talep geldi {request}", requestName);
        }
    }
}
