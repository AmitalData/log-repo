using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools.RestRequestExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiSender
    {
        private const string InterfaceName = CustomsPartnerFtpDetails.InterfaceName_SIISendRequest;
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

        public async Task<ApiResponse<ReleaseRequestApiResponseDto>> SendAsync(string siiRequestId, List<SupplierInvoiceItemsReqListKeys> selectedRows)
        {
            var credentials = _factory.BuildCredentials(InterfaceName, PartnerCode);
            var config = _factory.GetEndpointConfig(InterfaceName, PartnerCode);

            var dto = _mapper.Build(credentials, siiRequestId, selectedRows);

            var apiReq = ApiRequestBuilder.Build(_tenant, config, dto);
            var executor = new RestRequestExecutor();
            return await executor.ExecuteAsync<ReleaseRequestApiDto, ReleaseRequestApiResponseDto>(apiReq);
        }
    }

}
