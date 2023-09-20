using InsuranceProviders.Application.Dtos;
using InsuranceProviders.Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace InsuranceProviders.SompoJapanWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly IMediator mediator;

        public OfferController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetOffer([FromQuery] ProposalParametersDto proposalParametersDto)
        {
            var query = new GetProposalRequest()
            {
                InstallmentCount = proposalParametersDto.InstallmentCount,
                ProductCode = proposalParametersDto.ProductCode,
            };

            return Ok(await mediator.Send(query));
        }
    }
}