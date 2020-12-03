using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.MessagingServices
{
     class Fake_DCA_NG_5110_DepositRequestFulfillednfoMsgMessagingService
    {
        private ResponseContentHeader _responseContentHeader;
        private DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsgDepositRequestFulfillednfoMsg _depositRequestFulfillednfoMsg;
        private TPGIdentifier[] _depositRequests;
        private TPGIdentifier _tapagIdentifier;

        internal DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            SetResponseContentHeader();
            SetDepositRequestFulfillednfoMsg();
            SetDepositRequests(requestParamsData);
            SetTapagIdentifier();
            DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg fake = new DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsg()
            {
                ResponseContentHeader = _responseContentHeader,
                DepositRequestFulfillednfoMsg = _depositRequestFulfillednfoMsg,
                DepositRequests = _depositRequests,
                TapagIdentifier = _tapagIdentifier
            };
            return fake;
        }
        public void SetResponseContentHeader()
        {
            _responseContentHeader = new ResponseContentHeader()
            {
                TransmitionDateTime = DateTime.Now
            };
        }
        public void SetDepositRequestFulfillednfoMsg()
        {
            _depositRequestFulfillednfoMsg = new DEPO_NG_5110_MSG5_DepositRequestFulfillednfoMsgDepositRequestFulfillednfoMsg
            {
                depositeEntityType = 1055,
                depositeEssenceType = 1,
                isDepositFileNew = true,
                validityDay = DateTime.Now.AddDays(30),
                customUnit = 1,
                depositAmount = 2500
            };
        }
        public void SetDepositRequests(GenericRequestParams requestParamsData)
        {
            int filenumeral = 1;
            dynamic params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            int.TryParse(Convert.ToString(params1.numeral), out filenumeral);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            _depositRequests = new TPGIdentifier[1];
            _depositRequests[0] = new TPGIdentifier
            {
                fileNumber = _dec.DeclarationNumber,
                numeral = filenumeral
            };


        }
        public void SetTapagIdentifier()
        {
            _tapagIdentifier = new TPGIdentifier();
            {
               // fileNumber = "91320051",
                //numeral = 2
            };
        }
    }
}