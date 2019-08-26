using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ContinuousRequestOnClaimFileServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CLAIM_5013_ContinuousResponseOnClaimFileResponseService : ResponseServiceBase<ContinuousResponseOnClaimFileResponseData, CLAIM_MSG13_ContinuousResponseOnClaimFile, ContinuousRequestOnClaimFileRequestParams>
    {
        ClaimPM _ClaimPM;

        public override ContinuousResponseOnClaimFileResponseData GetResponse(CLAIM_MSG13_ContinuousResponseOnClaimFile customResponse, ContinuousRequestOnClaimFileRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(CLAIM_MSG13_ContinuousResponseOnClaimFile customResponse, ContinuousRequestOnClaimFileRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myClaimQueryService = new ClaimQueryService(context);
            var myClaimUpdateService = new ClaimUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            string userMessage = "ניתוח מסר ביטול/ערר תביעה";

            this.MyResponseData = new ContinuousResponseOnClaimFileResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = userMessage;

            if(customResponse.ResponseContentHeader != null && customResponse.ResponseContentHeader.Exception != null)
            {
                LogMessagingUtil.Instance.AppendLine("Exception :" + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                LogMessagingUtil.Instance.AppendLine("Can not find claim: " + requestParams.AppicationId);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not find claim: " + requestParams.AppicationId;
                return;
            }

            if (customResponse == null || customResponse.SystemAnswer == null)
            {
                LogMessagingUtil.Instance.AppendLine("No Data - customResponse.SystemAnswer is empty");
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "No Data - customResponse.SystemAnswer is empty";
                return;
            }

            this._ClaimPM = myClaimQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (this._ClaimPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not find claim" + requestParams.AppicationId);
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "Can not find claim" + requestParams.AppicationId;
                return;
            }

            MyResponseData.ContinuousMessagesTypeName = customResponse.SystemAnswer.FirstOrDefault().continuousMessagesTypecode.ToString();
            MyResponseData.ClaimRequestNumber = customResponse.SystemAnswer.FirstOrDefault().requestNumber.ToString();
            MyResponseData.Note = customResponse.SystemAnswer.FirstOrDefault().note;

            if (_ClaimPM != null && _ClaimPM.ClaimsRelatedEntities != null && _ClaimPM.ClaimsRelatedEntities.Count > 0)
            {
                ClaimsRelatedEntityPM myClaimsRelatedEntityPM = _ClaimPM.ClaimsRelatedEntities.Where(r => r.EntityCounterKey.ToString() == requestParams.ClaimRelatedEntityCounterKey).FirstOrDefault();
                if (myClaimsRelatedEntityPM != null)
                {
                    this._ClaimPM.ChangeSetOp = ChangeSetOperation.Update;
                    myClaimsRelatedEntityPM.ChangeSetOp = ChangeSetOperation.Update;
                    myClaimsRelatedEntityPM.ContinuousMessagesTypeCode = customResponse.SystemAnswer.FirstOrDefault().continuousMessagesTypecode.ToString();
                    myClaimsRelatedEntityPM.Note = customResponse.SystemAnswer.FirstOrDefault().note;
                    this.MyResponseData.UserMessage = "ניתוח מסר ביטול/ערר תביעה. מספר בקשה: " + customResponse.SystemAnswer.FirstOrDefault().requestNumber;

                    if(customResponse.SystemAnswer.FirstOrDefault().Exceptions != null)
                    {
                        string exeptionDescriptions = "";
                        foreach (var item in customResponse.SystemAnswer.FirstOrDefault().Exceptions)
                        {
                            exeptionDescriptions += string.Concat(item.ExeptionType, ": ", item.ExeptionDescription, "\n");
                        }
                        this.MyResponseData.UserMessage = string.Concat(this.MyResponseData.UserMessage, "\n", exeptionDescriptions);
                    }
                }
            }

            myClaimUpdateService.Update(this._ClaimPM, true);
        }

    }
}
