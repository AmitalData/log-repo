using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Collateral;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_DCAInDF_8211_CollateralRequestMsgMessagingServices
    {
        private ResponseContentHeader _responseContentHeader;
        private CollateralRequestDetails[] _collateralRequestDetails;
        internal COLT_NG_8211_MSG10040_CollateralRequestMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            SetRespondContentHeader();
            SetCollateralRequestDetails(requestParamsData);
            COLT_NG_8211_MSG10040_CollateralRequestMsg fake = new COLT_NG_8211_MSG10040_CollateralRequestMsg
            {
                ResponseContentHeader = _responseContentHeader,
                CollateralRequestDetails = _collateralRequestDetails
            };
            return fake;
        }
        public void SetRespondContentHeader()
        {
            _responseContentHeader = new ResponseContentHeader
            {
                TransmitionDateTime = DateTime.Now
            };
        }
        public void SetCollateralRequestDetails(GenericRequestParams requestParamsData)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            DeclarationPM _decParent=new DeclarationPM();
            if( _dec.AmendmentOriginalDeclartation!= null && _dec.DeclarationNumber!= null)
               _decParent = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);

            _collateralRequestDetails = new CollateralRequestDetails[1];
            _collateralRequestDetails[0] = new CollateralRequestDetails
            {
                collateralRequestNumber = 6,
                collateralValidityDate = DateTime.Now.AddDays(200),
                requestValidityDate = DateTime.Now.AddYears(2),
                collateralRequestStatus = 2,
                requestedCollateralType = 3,
                IncludingThirdPartyGuarantee = true
            };
            CollateralRequestDetailsCollateralConditioning[] _collateralConditioning = new CollateralRequestDetailsCollateralConditioning[1];
            _collateralConditioning[0] = new CollateralRequestDetailsCollateralConditioning
            {
                conditionCode = 2,
                requestedAmount = 999
            };
            _collateralRequestDetails[0].CollateralConditioning = _collateralConditioning;
            ConnectedEntity[] _relatedEntity = new ConnectedEntity[1];

            var dec_number = _decParent.DeclarationNumber!=null ? _decParent.DeclarationNumber : _dec.DeclarationNumber;
            _relatedEntity[0] = new ConnectedEntity
            {
                entityIdKey1 = dec_number,
                entityType=11185
            };


            _collateralRequestDetails[0].RelatedEntity = _relatedEntity;
            _collateralRequestDetails[0].Worker = new Worker
            {
                organizationUnitType = 15,
                customsHouse = 3,
                workerName = "מערכת שרת"
            };
            _collateralRequestDetails[0].remarks = "";
             

        }
    }
}
