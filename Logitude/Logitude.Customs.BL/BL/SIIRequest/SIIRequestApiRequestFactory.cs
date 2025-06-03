using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Interfaces;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.RestRequestExecutor;
using Logitude.Server.Tools.Utils;
using Microsoft.Azure.Management.Sql.Fluent.Models;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SIIRequestApiRequestFactory
    {
        private readonly int _tenant;
        private readonly CustomsPartnerFtpDetails _ftpDetailsHelper;
        private readonly CustomsPartnerFtpQueryService _ftpQueryService;

        public const string MissingCommDef = "Customs.SIIRequest.O.MissingCommDef";
        public const string MissingServiceUrl = "Customs.SIIRequest.O.MissingServiceUrl";
        public const string MissingUsername = "Customs.SIIRequest.O.MissingUsername";
        public const string MissingPassword = "Customs.SIIRequest.O.MissingPassword";

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

            Validate(dto, def.Name, _tenant);

            return new CredentialsDto
            {
                userId = dto.User,
                customerUniqueCode = dto.CustomerUniqueCode,
                hashPassword = dto.Password
            };
        }
        public SIIRequestWebApiEndpointConfig GetEndpointConfig(string interfaceName, string partnerCode)
        {
            var def = GetInterfaceDefinition(interfaceName);
            var pm = GetPartnerFtpRow(interfaceName, partnerCode, def);
            var dto = ProxyUtil.JsonConvertDeserializeTyped<WebApiDefinitionDTO>(pm.CommunicationDetails);

            Validate(dto, def.Name, _tenant);

            return new SIIRequestWebApiEndpointConfig
            {
                Url = dto.WEBAPIURL
            };
        }
        private string Translate(string textCode, int tenant)
        {
            return TranslateMyTextCode(textCode, tenant);
        }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);
            return loggedcontact;
        }
        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        private string _JLineNumberTExt;


        private bool ToUseLocalText(int tenant)
        {
            bool useLocal = true;
            var user = GetLoggedContact(tenant);
            if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);
            return useLocal;
        }
        private string TranslateMyTextCode(string textCodeCode, int tenant)
        {
            string trans = "";
            if (OverrideITextCodeTranslator != null)
            {
                trans = OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            else
            {

                bool useLocal = ToUseLocalText(tenant);
                trans = TranslateTextsClass.Translate(textCodeCode, tenant, useLocal);
                if (string.IsNullOrWhiteSpace(trans))
                {
                    trans = "$Text(" + textCodeCode + ")";
                }
                trans += " " + _JLineNumberTExt; 
            }
            if (string.IsNullOrWhiteSpace(trans))
            {
                trans = "$Text(" + textCodeCode + ")";
            }
            return trans;
        }
        private void Require(string value, string textCode,
                             int tenant,
                             string msgName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception(FormatMessage(textCode, tenant, msgName));
        }
        private string FormatMessage(string textCode, int tenant, string msgName)
        {
            return Translate(textCode, tenant).Replace("%name", msgName);
        }
        public ApiRequest<TData> Create<TData>(
            string interfaceName,
            string partnerCode,
            TData data)
        {
            var config = GetEndpointConfig(interfaceName, partnerCode);
            return ApiRequestBuilder.Build(_tenant, config, data);
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
                throw new MasofException(FormatMessage(MissingCommDef, _tenant, defDefault.Name));

            return pm;
        }

        private void Validate(WebApiDefinitionDTO dto, string name, int tenant)
        {
            Require(dto.WEBAPIURL, MissingServiceUrl, tenant, name);
            Require(dto.User, MissingUsername, tenant, name);
            Require(dto.Password, MissingPassword, tenant, name);
        }
}
}