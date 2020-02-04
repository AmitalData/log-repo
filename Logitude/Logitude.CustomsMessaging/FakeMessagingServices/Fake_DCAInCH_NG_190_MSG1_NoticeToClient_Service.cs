using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.PhysicalCheck190;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_DCAInCH_NG_190_MSG1_NoticeToClient_Service
    {
        internal CH_NG_190_MSG1_NoticeToClient GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            return new CH_NG_190_MSG1_NoticeToClient()
            {
                RequestContentHeader = new RequestContentHeader()
                {
                    TransmitionDateTime = DateTime.Now
                },
                NoticeToClient = new CH_NG_190_MSG1_NoticeToClientNoticeToClient()
                {
                    operationCode = 1,
                    statusMessage = 2,
                    checkId = 2369229,
                    entityType = 5,
                    customsAgent = 1111,
                    importerNumber = 111,
                    storageSiteNumber = "ILMMN",
                    checkSiteNumber = "10470",
                    openDate = DateTime.Now,
                    CheckType = 1,
                    declarationID = requestParamsData.AppicationId,///change to number 


                },
                CheckEntity = new CH_NG_190_MSG1_NoticeToClientCheckEntity()
                {
                    cargoIdentifier = new cargoIdentifier()
                    {
                        cargoIdentifierKey1 = "22",
                        cargoIdentifierType = 1
                    }

                },
                SplitCargoIdentifier = new CH_NG_190_MSG1_NoticeToClientSplitCargoIdentifier[]{
                      new CH_NG_190_MSG1_NoticeToClientSplitCargoIdentifier()
                  {
                       cargoIdentifier= new cargoIdentifier()
                       {
                            cargoIdentifierType= 27 ,
                             cargoIdentifierKey1= "50497355"
                       }
                  }
                  }

            };
        }
    }
}
