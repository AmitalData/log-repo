
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.DeclarationPrintServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{// moran 8.1.15 - Task 10004
    public class DF_NG_8302_Web03_DeclarationPrintRequestService
    
        : RequestServiceBase
        <DF_NG_8302_Web03_DeclarationPrint_Request, DF_NG_8302_Web03_DeclarationPrintRequestParams>
    {
        public override DF_NG_8302_Web03_DeclarationPrint_Request GetRequest(DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            var myDF_NG_8302_Web03_DeclarationPrint_Request = new DF_NG_8302_Web03_DeclarationPrint_Request();

            myDF_NG_8302_Web03_DeclarationPrint_Request.QueryDetails = QueryDetails(requestParams);
            return myDF_NG_8302_Web03_DeclarationPrint_Request;

        }

        private DF_NG_8302_Web03_DeclarationPrint_RequestQueryDetails[] QueryDetails(DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams)
        {
            const string DeclarationTypeExport = "2";
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
            var declarationStatus_RequestQueryDetailsList = new List<DF_NG_8302_Web03_DeclarationPrint_RequestQueryDetails>();
            string declarationId = "";
            string declarationNumber = "";
            string direction = "";
            //Query by Declaration
            int count = 0;
            if (requestParams.DeclarationNumber.Count() > 0)
            {
                declarationNumber = requestParams.DeclarationNumber.FirstOrDefault();


                //declarationId = declarationQueryService.GetIdByDeclarationNumber(declarationNumber, requestParams.Tenant);
                (declarationId,direction) = declarationQueryService.GetMinDeclarationByDeclarationNumber(declarationNumber, requestParams.Tenant);
                
                foreach (var declarationNumberItem in requestParams.DeclarationNumber)
                {
                    var declarationStatus_RequestQueryDetails = new DF_NG_8302_Web03_DeclarationPrint_RequestQueryDetails();
                    declarationStatus_RequestQueryDetails.QueryByDeclaration = new DF_NG_8302_Web03_DeclarationPrint_RequestQueryDetailsQueryByDeclaration()
                    {
                        DeclarationID = declarationNumberItem,
                        DeclarationType = direction != "E" ? "1" : DeclarationTypeExport
                    };
                    declarationStatus_RequestQueryDetails.SequenceNumber = ++count;
                    declarationStatus_RequestQueryDetailsList.Add(declarationStatus_RequestQueryDetails);
                }
            }
            //Query by Cargo
            else
            {
                var declarationStatus_RequestQueryDetails = new DF_NG_8302_Web03_DeclarationPrint_RequestQueryDetails();
                declarationStatus_RequestQueryDetails.QueryByCargo = new DF_NG_8302_Web03_DeclarationPrint_RequestQueryDetailsQueryByCargo()
                {
                    CargoIdentifierType = requestParams.CargoTypeCode,
                    CargoIdentifierKey1 = requestParams.ManifestNumber,
                    CargoIdentifierKey2 = requestParams.SecondCargoID,
                    CargoIdentifierKey3 = requestParams.ThirdCargoID,
                };
                declarationStatus_RequestQueryDetails.SequenceNumber = ++count;
                declarationStatus_RequestQueryDetailsList.Add(declarationStatus_RequestQueryDetails);
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = declarationId;
            this.MyRequestSheetParam.RequestDescription = "בקשה לטופס הצהרה " + declarationNumber; 

            return declarationStatus_RequestQueryDetailsList.ToArray();
        }

    }
}
