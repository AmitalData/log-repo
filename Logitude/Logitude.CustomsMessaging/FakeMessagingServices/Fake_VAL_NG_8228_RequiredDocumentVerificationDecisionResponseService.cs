using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
  using UnifreightIIG.Common.MessageLib.Ransom;
using Exception = UnifreightIIG.Common.MessageLib.ID.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
   public class Fake_VAL_NG_8228_RequiredDocumentVerificationDecisionResponseService
    {
        //Declaration dec;
        //ResponseContentHeader _header;

        public Fake_VAL_NG_8228_RequiredDocumentVerificationDecisionResponseService(GenericRequestParams requestParams)
            
            {

        }

        public VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {


            dynamic data = JObject.Parse(requestParamsData.TestCase.Param1);

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);



            var dec = declarationQueryService.GetSingleDeclarationById(requestParamsData.LoggingEntityId, requestParamsData.Tenant);


            string requestNumber = data.RequestNumber;
            VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage response = new VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessage() {
                RequestContentHeader = new RequestContentHeader()
                {
                    TransmitionDateTime = DateTime.Now
                }
            };

            response.VerificationDecision = new VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessageVerificationDecision();

            if (!string.IsNullOrEmpty(data.rejectVerificationReason.ToString()))
            {
                response.VerificationDecision.rejectVerificationReason = data.rejectVerificationReason;
                response.VerificationDecision.rejectVerificationReasonSpecified = true;
            }

           

            response.VerificationDecision.rejectVerificationRemark = data.rejectVerificationRemark;
            response.VerificationDecision.verificationDecisionType = data.verificationDecisionType;

            if(response.VerificationDecision.verificationDecisionType== 4 )
            {
                response.VerificationDecision.replacingDocumentId = int.Parse(DateTime.Now.ToString("MMddhhmm"));
                response.VerificationDecision.replacingDocumentIdSpecified = true;

            }

            response.GeneralDetails = new VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessageGeneralDetails()
            {
                documentId = data.documentId,
                remarks ="FAKE"
            };


            response.Worker = new UnifreightIIG.Common.MessageLib.Ransom.Worker() { customsHouse = Convert.ToInt32(dec.DeclarationOfficeCode), organizationUnitType = 42, workerName = "FAKE" };

            response.ConnectedEntity = new UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessageConnectedEntity[1];

            response.ConnectedEntity[0] = new UnifreightIIG.Common.MessageLib.Ransom.VAL_NG_8228_MSG550_RequiredDocumentVerificationDecisionMessageConnectedEntity() {
                entityType = data.entityType,
                entityIdKey1 = dec.DeclarationNumber

        };


          
            if ( data.entityIdKey2!= null)
            {
                response.ConnectedEntity[0].entityIdKey2 = data.entityIdKey2;

                if (data.entityIdKey3 != null)
                {
                    response.ConnectedEntity[0].entityIdKey3 = data.entityIdKey3;

                }

            }

            return response;
                        }


                    }
}
