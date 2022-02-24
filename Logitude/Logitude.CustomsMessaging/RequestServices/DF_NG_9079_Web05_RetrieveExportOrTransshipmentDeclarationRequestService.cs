
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CargoQueryMessageServiceReference;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;
using UnifreightIIG.Common.RetrieveExportOrTransshipmentDeclarationServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclarationRequestService
        : RequestServiceBase<DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclaration_Request, DeclarationRestoreRequestParams>
    {
        //        public override DF_NG_8373_Web05_RetrieveImportDeclaration_Request GetRequest(CargoQueryRequestParams requestParams)
        public override DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclaration_Request GetRequest(DeclarationRestoreRequestParams requestParams)
        {
            var myDF_NG_8373_Web05_RetrieveImportDeclaration_Request = new DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclaration_Request();

            myDF_NG_8373_Web05_RetrieveImportDeclaration_Request.QueryDetails = new DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclaration_RequestQueryDetails()
            {
                DeclarationID = requestParams.DeclarationNumber
            };

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.DeclarationId;
            this.MyRequestSheetParam.CustomFileNo = requestParams.CustomsFile;
            this.MyRequestSheetParam.RequestDescription = "שאילתא לשחזור נתוני הצהרה";

            return myDF_NG_8373_Web05_RetrieveImportDeclaration_Request;

        }


    }
}
