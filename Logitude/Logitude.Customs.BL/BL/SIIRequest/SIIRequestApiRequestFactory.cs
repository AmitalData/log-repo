using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.RestRequestExecutor;
using Logitude.Server.Tools.Utils;
using System;
using System.Linq;
using System.Net.Http;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiRequestFactory
    {
        private readonly int _tenant;
        private readonly CustomsPartnerFtpDetails _ftpDetailsHelper;
        private readonly CustomsPartnerFtpQueryService _ftpQueryService;

        public SIIRequestApiRequestFactory(int tenant)
        {
            _tenant = tenant;
            _ftpDetailsHelper = new CustomsPartnerFtpDetails();
            _ftpQueryService = new CustomsPartnerFtpQueryService(tenant);
        }

        public CredentialsDto BuildCredentials(string interfaceName, string partnerCode)
        {
            var def = GetInterfaceDefinition(interfaceName);
            var pm = GetPartnerFtpRow(interfaceName, partnerCode, def);
            var dto = ProxyUtil.JsonConvertDeserializeTyped<WebApiDefinitionDTO>(pm.CommunicationDetails);

            Validate(dto, def.Name);

            return new CredentialsDto
            {
                userId = dto.User,
                customerUniqueCode = dto.CustomerUniqueCode,
                hashPassword = dto.Password
            };
        }

        public ApiRequest<TData> Create<TData>(
            string interfaceName,
            string partnerCode,
            TData data)
        {
            var def = GetInterfaceDefinition(interfaceName);
            var pm = GetPartnerFtpRow(interfaceName, partnerCode, def);
            var dto = ProxyUtil.JsonConvertDeserializeTyped<WebApiDefinitionDTO>(pm.CommunicationDetails);

            Validate(dto, def.Name);

            return new ApiRequest<TData>
            {
                Tenant = _tenant,
                Url = dto.WEBAPIURL,
                Header = new ApiRequestHeader
                {
                    Method = HttpMethod.Post,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Timeout = 20_000
                },
                Data = data
            };
        }
        private InterfaceDetails GetInterfaceDefinition(string interfaceName)
        {
            var defJson = _ftpDetailsHelper
                          .GetAllInterfaceName()
                          .First(r => r.Key == interfaceName)
                          .Value;

            return ProxyUtil.JsonConvertDeserializeTyped<InterfaceDetails>(defJson);
        }

        private CustomsPartnerFtpPM GetPartnerFtpRow(
            string interfaceName,
            string partnerCode,
            InterfaceDetails defDefault)
        {
            var pm = _ftpQueryService.GetBy(
                         _tenant,
                         interfaceName,
                         partnerCode,
                         CustomsPartnerFtpDetails.TypeCode_Out);

            if (string.IsNullOrWhiteSpace(pm.CommunicationDetails))
                throw new MasofException($"מסר {defDefault.Name} - לא נמצא הגדרת תקשורת");

            return pm;
        }

        private static void Validate(WebApiDefinitionDTO dto, string name)
        {
            if (string.IsNullOrWhiteSpace(dto.WEBAPIURL))
                throw new Exception($"הינו שדה חובה מסר {name} - כתובת השירות");
            if (string.IsNullOrWhiteSpace(dto.User))
                throw new Exception($"הינו שדה חובה {name} - שם משתמש");
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception($"הינו שדה חובה {name} - סיסמא");
        }
    }
}