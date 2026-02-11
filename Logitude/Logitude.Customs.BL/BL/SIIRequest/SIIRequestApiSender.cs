using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.EntityKeys.Extended;
using Logitude.Server.Tools.RestRequestExecutor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiSender
    {
        private const string InterfaceName = CustomsPartnerFtpDetails.InterfaceName_SIISendRequest;
        private const string InterfaceName_Response = CustomsPartnerFtpDetails.InterfaceName_SIISendRequest_Response;
        private const string PartnerCode = CustomsPartnerFtpDetails.PartnerCode_SII;

        private readonly int _tenant;
        private readonly SIIRequestApiRequestFactory _factory;
        private readonly SIIRequestApiDataMapper _mapper;

        public SIIRequestApiSender(int tenant)
        {
            _tenant = tenant;
            _factory = new SIIRequestApiRequestFactory(tenant);
            _mapper = new SIIRequestApiDataMapper(tenant);
        }

        public async Task<(ApiResponse<ReleaseRequestApiResponseDto> Response, ReleaseRequestApiDto Dto)> SendAsync(string siiRequestId, string declarationId, SiiSendRequestBodyDto body)
        {
            var credentials = _factory.BuildCredentials(InterfaceName, PartnerCode);
            var config = _factory.GetEndpointConfig(InterfaceName, PartnerCode);

            var commRequest = _factory.BuildCommunicationsDto(InterfaceName, PartnerCode, declarationId);
            var commResponse = _factory.BuildCommunicationsDto(InterfaceName_Response, PartnerCode, declarationId);

            var dto = _mapper.Build(credentials, siiRequestId, body);
            var apiReq = ApiRequestBuilder.Build(_tenant, config, dto, commRequest, commResponse);

            var executor = new RestRequestExecutor();
            var response = await executor.ExecuteAsync<ReleaseRequestApiDto, ReleaseRequestApiResponseDto>(apiReq);

            return (response, dto); 
        }

    }

}
