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
using UnifreightIIG.Common.MessageLib.DeclarationCancel;
 using UnifreightIIG.Common.MessageLib.Ransom;
using Exception = UnifreightIIG.Common.MessageLib.ID.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
   public class Fake_VAL_NG_8227_MSG_520_RequiredDocumentMessage
    {
        //Declaration dec;
        //ResponseContentHeader _header;

        public Fake_VAL_NG_8227_MSG_520_RequiredDocumentMessage(GenericRequestParams requestParams)
            
            {

        }

        public VAL_NG_8227_MSG_520_RequiredDocumentMessage GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {


            dynamic data = JObject.Parse(requestParamsData.TestCase.Param1);

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);



            var dec = declarationQueryService.GetSingleDeclarationById(requestParamsData.LoggingEntityId, requestParamsData.Tenant);


            string requestNumber = data.RequestNumber;
            VAL_NG_8227_MSG_520_RequiredDocumentMessage response = new VAL_NG_8227_MSG_520_RequiredDocumentMessage() {
                RequestContentHeader = new RequestContentHeader()
                {
                    TransmitionDateTime = DateTime.Now
                }
            };

            response.RequiredDocumentDetails = new UnifreightIIG.Common.MessageLib.Ransom.RequiredDocumentDetails();
            // CustomsDocumentsTicketQueryService customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(requestParamsData.Tenant);
          

            if (data.requiredDocumentMessageType=="1")
            {
                response.RequiredDocumentDetails.documentID =    int.Parse(DateTime.Now.ToString("MMddhhmm"));
            }

            else
            {
                response.RequiredDocumentDetails.documentID = data.documentId;
            }

            response.RequiredDocumentDetails.remarks = "FAKE FAKE";
            response.RequiredDocumentDetails.requiredDocumentMessageType = data.requiredDocumentMessageType;
            response.RequiredDocumentDetails.typeID = data.typeId;



            response.Worker = new UnifreightIIG.Common.MessageLib.Ransom.Worker() { customsHouse = Convert.ToInt32(dec.DeclarationOfficeCode), organizationUnitType = 42, workerName = "FAKE" };

            response.RelatedEntity = new UnifreightIIG.Common.MessageLib.Ransom.ConnectedEntity[1];

            response.RelatedEntity[0] = new UnifreightIIG.Common.MessageLib.Ransom.ConnectedEntity() {
                entityType = data.entityType,
                entityIdKey1 = dec.DeclarationNumber

        };


          
            if ( data.entityIdKey2!= null)
            {
                response.RelatedEntity[0].entityIdKey2 = data.entityIdKey2;

                if (data.entityIdKey3 != null)
                {
                    response.RelatedEntity[0].entityIdKey3 = data.entityIdKey3;

                }

            }

            return response;
                        }


                    }
}
