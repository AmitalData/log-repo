using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImporterDeclarationsDetailServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class VE_8326_ImporterDeclarationRequestService : RequestServiceBase
        <VE_NG_8326_Web01_ImporterDeclarationsParam, ImporterDeclarationRequestParams>
    {
        public override VE_NG_8326_Web01_ImporterDeclarationsParam GetRequest(ImporterDeclarationRequestParams requestParams)
        {
            //Build request Importer Declarations(8326)
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var vendorQueryService = new CustomsVendorQueryService(dbContext);

            var myImporterDeclarationsRequest = new VE_NG_8326_Web01_ImporterDeclarationsParam();
            myImporterDeclarationsRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            int importerNumber = 0;
            int.TryParse(requestParams.ImporterNumber, out importerNumber);
            myImporterDeclarationsRequest.Customer = importerNumber;

            if (requestParams.IsByExpireDate == true)
            {
                int numberOfDays = (int)((DateTime)requestParams.DeclarationExpire - DateTime.Today).TotalDays;
                myImporterDeclarationsRequest.ExpiresSoon = numberOfDays;
                myImporterDeclarationsRequest.ExpiresSoonSpecified = true;
            }
            else
            {
                myImporterDeclarationsRequest.ExpiresSoonSpecified = false;
                myImporterDeclarationsRequest.DeclarationList = new VE_NG_8326_Web01_ImporterDeclarationsParamDeclarationList();
                int relatedTo = 0;
                string code = requestParams.Code;
                switch (requestParams.DeclarationConect)
                {
                    case "0": // "ALL"
                        relatedTo = 1;
                        break;
                    case "1": // "ImportDeclaration"
                        relatedTo = 2;
                        break;
                    case "2": // "Vendor"
                        relatedTo = 3;
                        var customsVendorPM = vendorQueryService.GetSingle(requestParams.Code, true, false);
                        code = customsVendorPM.VendorNumber;
                        break;
                    case "4": // "Declaration"
                        relatedTo = 4;
                        break;
                }
                myImporterDeclarationsRequest.DeclarationList.relatedTo = relatedTo;
                myImporterDeclarationsRequest.DeclarationList.id = code;
                myImporterDeclarationsRequest.DeclarationList.fromDate = (DateTime)requestParams.FromDate;
                myImporterDeclarationsRequest.DeclarationList.toDate = (DateTime)requestParams.ToDate;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Client");
            this.MyRequestSheetParam.EntityId1 = requestParams.ImporterNumber;
            this.MyRequestSheetParam.RequestDescription = "שאילתא לתצהיר יבואן " + requestParams.ImporterNumber;

            return myImporterDeclarationsRequest;
        }
    }
}
