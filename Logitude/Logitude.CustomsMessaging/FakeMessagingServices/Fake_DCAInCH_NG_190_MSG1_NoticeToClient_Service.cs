using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
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
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            ConsignmentQueryService consignmentQueryService = new ConsignmentQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
                        ConsignmentPM _con = consignmentQueryService.GetSingle(_dec.Id,1, false, false);

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
                    entityType = 1,
                    checkId = int.Parse(DateTime.Now.ToString("MMddhhmm")),
                    customsAgent = 1111,
                    importerNumber = 111,
                    storageSiteNumber = _con.StorageSiteCode,
                    checkSiteNumber = "10470",
                    openDate = DateTime.Now.AddHours(-3),
                    limitDateSpecified=false,
                    //limitDate = DateTime.Now.AddDays(1),
                    CheckType = 3,
                    QueueTypeSpecified = true,
                    QueueType =3,
                    declarationID = _dec.DeclarationNumber, 
                },
                CheckEntity = new CH_NG_190_MSG1_NoticeToClientCheckEntity()
                {
                    cargoIdentifier = new cargoIdentifier()
                    {
                        cargoIdentifierType = int.Parse(_con.CargoTypeCode),
                        cargoIdentifierKey1 = _con.ManifestNumber,
                        cargoIdentifierKey2 = _con.SecondCargoID,
                        cargoIdentifierKey3 = _con.ThirdCargoID,
                    },
                    containerNumber= "CAIU8087570",

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
