using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CollateralAnswerMsgServiceReference;
using UnifreightIIG.Common.MessageLib.Collateral;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class COLT_NG_8212_MSG10041_CollateralAnswerMsgResponseService:
        ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, CollateralRequestParams>
    {
        public override void Update(INF_MSG_Generic customResponse, CollateralRequestParams requestParams)
        {
            bool succeeded = false;
            string applicationId = null;
            bool hasException = false;
            string exceptionMessage = null;

            if (customResponse.ResponseContentHeader.Exception == null)
            {
                succeeded = true;
                applicationId = customResponse.ResponseContentHeader.ApplicationID.ToString();
                hasException = false;
                exceptionMessage = "מענה לבטוחה נשלח בהצלחה";

                ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                var customsCollateralQueryService = new CustomsCollateralQueryService(dbContext);
                var customsCollateralUpdateService = new CustomsCollateralUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);

                CustomsCollateralPM CustomsCollateralPM = customsCollateralQueryService.GetSingle(requestParams.CustomCollateralId, true, false);
                CustomsCollateralPM.ChangeSetOp = ChangeSetOperation.Update;
                foreach (var collateralAnswerItem in CustomsCollateralPM.CustomsCollateralsAnswers)
                {
                    collateralAnswerItem.ChangeSetOp = ChangeSetOperation.Update;
                    collateralAnswerItem.IsClosed = true;
                }
                customsCollateralUpdateService.Update(CustomsCollateralPM, true);
            }
            else
            {
                string ExceptionDescription = "";
                var ExeptionDescription = "";
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    foreach (var rec in customResponse.ResponseContentHeader.Exception)
                    {

                        if (!String.IsNullOrWhiteSpace(ExeptionDescription))
                        {
                            ExeptionDescription += Environment.NewLine;
                        }
                        ExeptionDescription += rec.ExeptionDescription;

                    }
                    ExceptionDescription = ExeptionDescription;
                    hasException = true;
                    exceptionMessage = ExceptionDescription;
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                Succeeded = succeeded,
                ApplicationID = applicationId,
                HasException = hasException,
                UserMessage = exceptionMessage,
            };
        }

        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, CollateralRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
