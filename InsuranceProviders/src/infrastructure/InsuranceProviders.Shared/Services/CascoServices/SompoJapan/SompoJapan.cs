using AutoMapper;
using InsuranceProviders.Application.Dtos;
using InsuranceProviders.Application.Features;
using InsuranceProviders.Application.Interfaces;
using InsuranceProviders.Shared.Utilities;
using Microsoft.Extensions.Logging;
using SompoJapanCascoService;
using SompoJapanCommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static InsuranceProviders.Domain.Enums.Enums;

namespace InsuranceProviders.Shared.Services.CascoServices.SompoJapan
{
    public class SompoJapan : ISompoJapan
    {
        private readonly ILogger<SompoJapan> _logger;
        private readonly IMapper _mapper;

        private readonly CascoSoapClient _sompoJapanCascoServiceClient;
        private readonly CommonSoapClient _sompoJapanCommonServiceClient;

        public readonly string UserName = "VDF";
        public readonly string Password = "ApANHqAQxc";
        public readonly string ServiceUrl = "https://appstest.sompojapan.com.tr/SompoEndPoint/Casco.asmx";
        public readonly string ServicePrintUrl = "";
        public readonly string UserIp = "185.60.227.90";

        public const int SompoCompanyId = 1231850;
        public const string SompoCompanyName = "SOMPO JAPAN SİGORTA";




        public SompoJapan(IMapper mapper, ILogger<SompoJapan> logger)
        {
            _logger = logger;
            _mapper = mapper;


            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            _sompoJapanCascoServiceClient = new CascoSoapClient(CascoSoapClient.EndpointConfiguration.CascoSoap);
            _sompoJapanCascoServiceClient.Endpoint.EndpointBehaviors.Add(new InspectorBehavior(new ClientInspector(new SecurityHeaderNonCrypt(UserName, Password))));
            _sompoJapanCascoServiceClient.Endpoint.Address = new EndpointAddress(ServiceUrl);
            _sompoJapanCascoServiceClient.Endpoint.Binding = new BasicHttpBinding
            {
                BypassProxyOnLocal = false,
                UseDefaultWebProxy = true,
                MaxReceivedMessageSize = 2147483647,
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport,
                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None, ProxyCredentialType = HttpProxyCredentialType.None },
                    Message = new BasicHttpMessageSecurity { ClientCredentialType = BasicHttpMessageCredentialType.UserName }
                }
            };

            _sompoJapanCommonServiceClient = new CommonSoapClient(CommonSoapClient.EndpointConfiguration.CommonSoap);
            _sompoJapanCommonServiceClient.Endpoint.EndpointBehaviors.Add(new InspectorBehavior(new ClientInspector(new SecurityHeaderNonCrypt(UserName, Password))));
            _sompoJapanCommonServiceClient.Endpoint.Address = new EndpointAddress(ServiceUrl);
            _sompoJapanCommonServiceClient.Endpoint.Binding = new BasicHttpBinding
            {
                BypassProxyOnLocal = false,
                UseDefaultWebProxy = true,
                MaxReceivedMessageSize = 2147483647,
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.Transport,
                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None, ProxyCredentialType = HttpProxyCredentialType.None },
                    Message = new BasicHttpMessageSecurity { ClientCredentialType = BasicHttpMessageCredentialType.UserName }
                }
            };
        }

        public async Task<GetProposalResult> GetProposal(Application.Features.GetProposalRequest request)
        {

            var res = await _sompoJapanCascoServiceClient.GetProposalAsync(new SompoJapanCascoService.IdentityHeader
            {
                KullaniciAdi = UserName,
                KullaniciParola = Password,
                KullaniciIP = UserIp,
                KullaniciTipi = SompoJapanCascoService.ClientType.ACENTE
            },
            new ProposalParameters
            {

            });
            await _sompoJapanCascoServiceClient.CloseAsync();

            if (res.GetProposalResult.ResultCode != SompoJapanCascoService.ProposalResultCodes.Success)
            {

                //var dto1 = mapper.Map<GetProposalResult>(res.GetProposalResult.Description);
                //_mapper. CreateMap<GetProposalResult, dto1>();
                //.ForMember(dest => dest.Language
                //    , opt => opt.MapFrom(source => (int)source.Language)
                //    );


                return new GetProposalResult() { Err = res.GetProposalResult.Description };
            }
            return new GetProposalResult();
            //var dto = mapper.Map<GetProposalResult>(res);
            //return dto;




        }
    }
}
