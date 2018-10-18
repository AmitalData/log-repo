using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ExportDeclarationDataRequestServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_9070_ExportDeclarationDataRequestService : RequestServiceBase<DF_MSG_9070_ExportDeclarationDataRequst, ExportDeclarationDataRequestParams>
    {
        public override DF_MSG_9070_ExportDeclarationDataRequst GetRequest(ExportDeclarationDataRequestParams requestParams)
        {
            var myDF_MSG_9070_ExportDeclarationDataRequst = new DF_MSG_9070_ExportDeclarationDataRequst();
            myDF_MSG_9070_ExportDeclarationDataRequst.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myDF_MSG_9070_ExportDeclarationDataRequst.RequestParams = new DF_MSG_9070_ExportDeclarationDataRequstRequestParams();

            myDF_MSG_9070_ExportDeclarationDataRequst.RequestParams.ReshimonNubmer = requestParams.ReshimonNubmer;


            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = " שאילתא להצהרת יצוא" + requestParams.ReshimonNubmer;

            return myDF_MSG_9070_ExportDeclarationDataRequst;
        }
    }
}
