using InsuranceProviders.Application.Features;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceProviders.Application.Behaviors
{
    public class LoggingResponseBehavior<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingResponseBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {

            var responseName = typeof(TResponse).Name;

            _logger.LogInformation($"Sonuc Döndü {request}", responseName);
            return Task.CompletedTask;
        }
    }
}
