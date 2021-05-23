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

            req.QueryDetails = QueryDetails(requestParams);
            return req;
        }

        private DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails[] QueryDetails(DeclarationStatusRequestParams requestParams)
        {
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
            string declarationId = "";
            string requestDescription = "";
            var declarationStatus_RequestQueryDetailsList = new List<DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails>();
            var declarationStatus_RequestQueryDetails = new DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetails();
            DeclarationPM declarationPM = null;

            //Query by Declaration
            if (!String.IsNullOrWhiteSpace(requestParams.DeclarationNumber))
            {
                declarationStatus_RequestQueryDetails.QueryByDeclaration = new DF_NG_8250_Web01_DeclarationStatus_RequestQueryDetailsQueryByDeclaration()
                {
                    DeclarationID = requestParams.DeclarationNumber,
                    DeclarationType = (declarationPM != null && !String.IsNullOrWhiteSpace(declarationPM.DeclarationDocumentTypeCode)) ? int.Parse(declarationPM.DeclarationDocumentTypeCode)  : 1 ,
                };
                //declarationId = declarationQueryService.GetIdByDeclarationNumber(requestParams.DeclarationNumber, requestParams.Tenant);
                declarationPM = declarationQueryService.GetSingleDeclarationByNumber(requestParams.DeclarationNumber, requestParams.Tenant);
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
                    DeclarationType = (declarationPM != null && !String.IsNullOrWhiteSpace(declarationPM.DeclarationDocumentTypeCode)) ? int.Parse(declarationPM.DeclarationDocumentTypeCode) : 1,
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
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = declarationId;
            this.MyRequestSheetParam.RequestDescription = "שאילתא לסטטוס הצהרה " + requestDescription;

            return declarationStatus_RequestQueryDetailsList.ToArray();
        }
    }
}
