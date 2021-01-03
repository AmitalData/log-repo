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
using UnifreightIIG.Common.MessageLib.ID;
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;
using Exception = UnifreightIIG.Common.MessageLib.ID.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
   public class Fake_DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg 
    {
        //Declaration dec;
        //ResponseContentHeader _header;

        public Fake_DF_NG_5118_MSG14004_ImportDeclarationCancellationReplyMsg(GenericRequestParams requestParams)
            
            {

        }

        public DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {

            
             dynamic data = JObject.Parse(requestParamsData.TestCase.Param1);

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);


          var dec=   declarationQueryService.GetSingleDeclarationById(requestParamsData.LoggingEntityId, requestParamsData.Tenant);


            string requestNumber = data.RequestNumber;
            DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg response = new DF_NG_5118_MSG14004_DeclarationCancellationReplyMsg();



            response.CancellationResponse = new DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgCancellationResponse()
            {
                DeclarationID = dec.DeclarationNumber,
                DeclarationStatusID = data.DeclarationStatusID,
        

            };

            if(!string.IsNullOrEmpty(requestNumber))
            {

                response.CancellationResponse.FunctionalReferenceID = Convert.ToInt32(requestNumber);
                response.CancellationResponse.FunctionalReferenceIDSpecified = true;
            }

            List<DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgAdditionalInformation> AdditionalInformation = new List<DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgAdditionalInformation>();

            if (!string.IsNullOrEmpty(data.Content33.ToString()))
            {
                AdditionalInformation.Add(new DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgAdditionalInformation
                {
                    StatementTypeCode = 33,
                    Content = data.Content33
                }
            );
            }
                if (!string.IsNullOrEmpty(data.Content22.ToString()))
                {
                    AdditionalInformation.Add(new DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgAdditionalInformation
                    {
                        StatementTypeCode = 22,
                        Content = data.Content22
                    }
                );

                }
                    if (!string.IsNullOrEmpty(data.Content36.ToString()))
                    {
                        AdditionalInformation.Add(new DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgAdditionalInformation
                        {
                            StatementTypeCode = 36,
                            Content = data.Content36
                        }
                    );
                    }
                        if (!string.IsNullOrEmpty(data.Content37.ToString()))
                        {
                            AdditionalInformation.Add(new DF_NG_5118_MSG14004_DeclarationCancellationReplyMsgAdditionalInformation
                            {
                                StatementTypeCode = 37,
                                Content = data.Content37.ToString()
                            }
                        );

                        }

            response.AdditionalInformation = AdditionalInformation.ToArray();
            response.CanceledDocumentDetails = null;
            response.ProceduralFaultMsg = new ProceduralFaultDetails[1];
            response.ProceduralFaultMsg[0]= new ProceduralFaultDetails
            {
                createDate= DateTime.Now,
                inputType=1,
                inspectionType= 2,
                proceduralFaultCode=12,
                proceduralFaultInputProcess=3,
                proceduralFaultNumber= 642511,
                proceduralFaultStatus=3,
                ransomViolationSum= 0,
                remarks="FAKE",
                isCustomerResponsibility=false,
                isCustomerProceduralFaultCountable= false,
                isAgentResponsibility= true,
                isAgentProceduralFaultCountable=true,
                leadingDocumentVersion="1.0",
                ConnectedEntity= new UnifreightIIG.Common.MessageLib.ID.ConnectedEntity[1]
                { new UnifreightIIG.Common.MessageLib.ID.ConnectedEntity
                      {
                    entityIdKey1 = dec.DeclarationNumber,
                    entityType= 1055,
                    entityPath= "הצהרה",
                    entityIdExternalReferenceID= "642511"


                }
                },
                FaultUpdateWorker= new Worker()
                {
                    organizationUnitType=38,
                    customsHouse=4,
                    workerName="FAKE"
                }
                ,
                FaultDiscovererWorker= new Worker()
                {
                    organizationUnitType = 38,
                    customsHouse = 4,
                    workerName = "FAKE"
                }



            };
            response.ResponseContentHeader = new ResponseContentHeader()
            {
                ApplicationID = 0,
                Exception = null,
                Remark = "",
                TransmitionDateTime = DateTime.Now
        };


            return response;
                        }


                    }
}
