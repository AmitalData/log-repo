using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Web;
using UnifreightIIG.Common.BLClient;
using UnifreightIIG.Common.ChangingTimeServiceReference;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using UnifreightIIG.Common.Utils;
using Logitude.Customs.BL.DummyData;
namespace Logitude.CustomsMessaging.Helpers
{
    class ChangingTimeManager : ManagerProxyBase
    {

        public ChangingTimeManager(IErrorHandler errorHandler = null)
            : base(errorHandler ?? ErrorHandlerUtil.CreateNew())
        {

        }
        public ResponseHeader ChangingTime(
          string ExternalId,
            CH_NG_191_MSG2_ChangingTimeRequestChangingTimeRequest changingTimeRequest,
            out  CH_NG_192_MSG3_ApproveChangeTimeRequest response192,
            MoreParams moreParams = null)
        {

            ResponseHeader responseHeader = null;


            response192 = null;
            if (moreParams == null)
            {
                moreParams = new MoreParams();
                if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["TestMode"]))
                {
                    moreParams.MyOption = MoreParams.Options.TestMode;
                }
            }

            try
            {
                if (String.IsNullOrWhiteSpace(ExternalId))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("ExternalId");
                }

                if (changingTimeRequest == null)
                {
                    throw new ArgumentNullException("ChangingTimeRequest");
                }

                var myCH_NG_191_MSG2_ChangingTimeRequest = new CH_NG_191_MSG2_ChangingTimeRequest() { RequestContentHeader = new RequestContentHeader() };
                RequestContentHeaderUtil.GetDefault(myCH_NG_191_MSG2_ChangingTimeRequest.RequestContentHeader);
 
                myCH_NG_191_MSG2_ChangingTimeRequest.ChangingTimeRequest = changingTimeRequest;

                using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
                {

                    //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
                    //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
                    responseHeader = uifreightSdkGateway.GetChannel<IPhysicalCheck>().PhysicalCheckChangingTime(ExternalId,
                   base.CustomsSetting.CustomsAgentId,
                   myCH_NG_191_MSG2_ChangingTimeRequest,
                   ref moreParams,
                   out response192);
                }




                return responseHeader;
            }
            catch (System.Exception ex)
            {
                _ErrorHandler.ToFormattedMessage(ex);
                return null;
            }

        }
    }
}
