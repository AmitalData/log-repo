using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class FakeDCA_MSG2030_DepositRequestCreatedInfoMessagingServices
    {
        private RequestContentHeader _responseContentHeader;
        private DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsgDepositConditions[] _depositConditions;
        private DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsgDepositRequestInfo _depositRequestInfo;
        internal DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            SetRequestContentHeader();
            SetDepositConditions();
            SetDepositRequestInfo(requestParamsData);
            DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg fake = new DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg()
            {
                RequestContentHeader = _responseContentHeader,
                DepositConditions = _depositConditions,
                DepositRequestInfo = _depositRequestInfo
            };
            return fake;
        }
        public void SetRequestContentHeader()
        {
            _responseContentHeader = new RequestContentHeader()
            {
                TransmitionDateTime = DateTime.Now,
            };
        }
        public void SetDepositRequestInfo(GenericRequestParams requestParamsData)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            _depositRequestInfo = new DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsgDepositRequestInfo();
            _depositRequestInfo.entityType = 1055;
            _depositRequestInfo.entityNumber = _dec.DeclarationNumber;
            if (_dec.AmendmentOriginalDeclartation != null && _dec.DeclarationNumber != null)
                _depositRequestInfo.entityNumber = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false).DeclarationNumber;
            int agentID = 0;int filenumeral=1;
            dynamic params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            int.TryParse(Convert.ToString(params1.numeral), out filenumeral);
            int.TryParse(_dec.AgentId, out agentID);
            _depositRequestInfo.agentNumber = agentID;
            _depositRequestInfo.customsHouse = 1;
            _depositRequestInfo.requestValidity = DateTime.Now.AddDays(30);
            _depositRequestInfo.professionalUnitID = 11;
            _depositRequestInfo.DepositAmount = 2500;
            _depositRequestInfo.DepositEssenceType = 1;
            _depositRequestInfo.PaymentDetails = new PaymentDetails();
            _depositRequestInfo.DepositRequestIdentifier = new TPGIdentifier
            {
                fileNumber = _dec.DeclarationNumber,
                numeral = filenumeral
            };
        }
        public void SetDepositConditions()
        {
            _depositConditions = new DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsgDepositConditions[1];
            _depositConditions[0] = new DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsgDepositConditions()
            {
                DepositAmount = 2500,
                DepositCondition = 48
            };
        }
    }
}
