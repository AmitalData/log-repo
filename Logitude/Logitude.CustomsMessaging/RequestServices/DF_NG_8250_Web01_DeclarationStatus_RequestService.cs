using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_NG_8250_Web01_DeclarationStatus_RequestService
        : RequestServiceBase<DF_NG_8250_Web01_DeclarationStatus_Request, DeclarationStatusRequestParams>
    {
        public override DF_NG_8250_Web01_DeclarationStatus_Request GetRequest(DeclarationStatusRequestParams requestParams)
        {
            var req = new DF_NG_8250_Web01_DeclarationStatus_Request();
            if (requestParams.LoggingObjectTableId == ObjectTableRepository.GetObjectTableByName("Customs.Containerization"))
            {
                ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);

                var connectedDeclarations = requestParams.LoggingEntityId2.Split(',').ToList();
                var pms = declarationQueryService.GetDeclarationsByIds(connectedDeclarations, requestParams.Tenant);
                var queryDetails = new List<DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails>();
                foreach (var item in pms)
                {
                    if (string.IsNullOrWhiteSpace(item.DeclarationNumber)) continue;
                    requestParams.DeclarationNumber = item.DeclarationNumber;
                    queryDetails.AddRange(QueryDetails(requestParams, item));
                }
                int sequence = 0;
                queryDetails.ForEach(x => x.SequenceNumber = sequence++);
                req.QueryDetails = queryDetails.ToArray();
            }
            else
            {
                req.QueryDetails = QueryDetails(requestParams);
            }
            return req;
        }

        private DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails[] QueryDetails(DeclarationStatusRequestParams requestParams, DeclarationPM declarationPM = null)
        {
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
            string declarationId = "";
            string requestDescription = "";
            var declarationStatus_RequestQueryDetailsList = new List<DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails>();
            var declarationStatus_RequestQueryDetails = new DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails();
            //DeclarationPM declarationPM = null;

            //Query by Declaration
            if (!String.IsNullOrWhiteSpace(requestParams.DeclarationNumber))
            {
                if (declarationPM == null)
                    declarationPM = declarationQueryService.GetSingleDeclarationByNumber(requestParams.DeclarationNumber, requestParams.Tenant);
                declarationStatus_RequestQueryDetails.QueryByDeclaration = new DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetailsQueryByDeclaration()
                {
                    DeclarationID = requestParams.DeclarationNumber,
                    DeclarationType = (declarationPM != null && !String.IsNullOrWhiteSpace(declarationPM.DeclarationTypeCode)) ? int.Parse(declarationPM.DeclarationTypeCode)  : 1 ,
                };
                //declarationId = declarationQueryService.GetIdByDeclarationNumber(requestParams.DeclarationNumber, requestParams.Tenant);
                
                if(declarationPM != null)
                {
                    declarationId = declarationPM.Id;
                    if(declarationPM.IsCourierDeclaration)
                    {
                        declarationStatus_RequestQueryDetails.QueryByDeclaration.DeclarationType = -1;
                    }
                }
                requestDescription = requestParams.DeclarationNumber;
            }
            //Query by Old Reshimon Number
            else if (!String.IsNullOrWhiteSpace(requestParams.OldReshimonNumber))
            {
                //int oldReshimonNumber;
                //int.TryParse(requestParams.OldReshimonNumber,out oldReshimonNumber);
                declarationStatus_RequestQueryDetails.QueryByDeclaration = new DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetailsQueryByDeclaration()
                {
                    ReshimonNumber = requestParams.OldReshimonNumber, //oldReshimonNumber,
                    //ReshimonNumberSpecified = true,
                    DeclarationType = (declarationPM != null && !String.IsNullOrWhiteSpace(declarationPM.DeclarationTypeCode)) ? int.Parse(declarationPM.DeclarationTypeCode) : 1,
                };
                requestDescription = requestParams.OldReshimonNumber;
            }
            //Query by Cargo
            else
            {
                declarationStatus_RequestQueryDetails.QueryByCargo = new DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetailsQueryByCargo()
                {
                    CargoIdentifierType = requestParams.CargoTypeCode,
                    CargoIdentifierKey1 = requestParams.ManifestNumber,
                    CargoIdentifierKey2 = requestParams.SecondCargoID,
                    CargoIdentifierKey3 = requestParams.ThirdCargoID
                };
                requestDescription = requestParams.ManifestNumber;
           }

            declarationStatus_RequestQueryDetailsList.Add(declarationStatus_RequestQueryDetails);

            this.MyRequestSheetParam = new RequestSheetParam();
            if (requestParams.LoggingObjectTableId == ObjectTableRepository.GetObjectTableByName("Customs.Containerization"))
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Containerization");
                this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
                this.MyRequestSheetParam.RequestDescription = "שאילתא לסטטוס הצהרות בהמכלה";
            }
            else
            {
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = declarationId;
                this.MyRequestSheetParam.RequestDescription = "שאילתא לסטטוס הצהרה " + requestDescription;
            }

            return declarationStatus_RequestQueryDetailsList.ToArray();
        }
    }
}
