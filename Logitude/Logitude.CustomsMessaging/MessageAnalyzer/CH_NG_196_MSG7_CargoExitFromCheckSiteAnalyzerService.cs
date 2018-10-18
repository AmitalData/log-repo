using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    class CH_NG_196_MSG7_CargoExitFromCheckSiteAnalyzerService
         : MessageAnalyzerServiceBase<
        GenericRequestParams, INF_MSG_GenericResponseData, 
        CH_NG_196_MSG7_CargoExitFromCheckSite, 
        CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService>, IMessageAnalyzerService
    {
        public CH_NG_196_MSG7_CargoExitFromCheckSiteAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_196_MSG7_CargoExitFromCheckSite.xsd")
        {

        }

        public override string GetObjectTableName()
        {
            return "Customs.PhysicalCheck";
        }

        


        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(CH_NG_196_MSG7_CargoExitFromCheckSite));
            CH_NG_196_MSG7_CargoExitFromCheckSite physicalCheckMessage = null;///(CH_NG_196_MSG7_CargoExitFromCheckSite)serializer.Deserialize(memstream);
            physicalCheckMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService customResponseService = new CH_NG_196_MSG7_CargoExitFromCheckSiteResponseService();
            customResponseService.Update(physicalCheckMessage, requestParams);
            if (customResponseService.MyResponseData == null)
            {
                //??
                return null;
            }
            reData.ExceptionMessage = customResponseService.MyResponseData.ExceptionMessage;
            reData.ApplicationID = customResponseService.MyResponseData.ApplicationID;
            reData.HasException = customResponseService.MyResponseData.HasException;
            reData.Succeeded = customResponseService.MyResponseData.Succeeded;
            return reData;
        }

        private int CustomsAgentToTenant(int? CustomsAgent)
        {
            //  TODO: CustomsAgentToTenant- yaron how to know tenent     <customsAgent>510120041</customsAgent>
            return 1;
        }




        //public override string GetMessageXSD()
        //{
        //    return "UnifreightIIG.Common.MessageLib.PhysicalCheck.CH_NG_196_MSG7_CargoExitFromCheckSite.xsd";
        //}

        public override int ResolveTenant()
        {
            return CustomsAgentToTenant(_CustomResponse.generalDetails.customsAgent);
        }

        public override string GetLoggingEntityReference()
        {
            return _CustomResponse.generalDetails.checkId.ToString();
        }
    }
}
