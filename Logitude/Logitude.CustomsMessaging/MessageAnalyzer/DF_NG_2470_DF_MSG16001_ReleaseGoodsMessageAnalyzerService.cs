using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.DeclarationDeal;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    class DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageAnalyzerService
         : MessageAnalyzerServiceBase<
        GenericRequestParams, INF_MSG_GenericResponseData, 
        DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage, 
        DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseService>, IMessageAnalyzerService
    {
        public DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageAnalyzerService()
            : base("UnifreightIIG.Common.MessageLib.DeclarationDeal.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage.xsd")
        {
        }

        public override string GetObjectTableName()
        {
            return "Customs.Declaration";
        }

        

        public override INF_MSG_GenericResponseData AnalyzeToOverride()
        {
            DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage releaseGoodsMessage = null;

            releaseGoodsMessage = _CustomResponse;
            var tenant = 1;
            var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

            var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams();
            requestParams.Tenant = ResolveTenant();
            ///InsertPhysicalCheck(physicalCheckMessage);
            DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseService customResponseService = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseService();
            customResponseService.Update(releaseGoodsMessage, requestParams);
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
            //return CustomsAgentToTenant(_CustomResponse.generalDetails.customsAgent);
            //return CustomsAgentToTenant(_CustomResponse.Customers.agentExternalID);
            return 1;
        }

        public override string GetLoggingEntityReference()
        {
            //return _CustomResponse.generalDetails.checkId.ToString();
            return "1";

        }
    }
}
