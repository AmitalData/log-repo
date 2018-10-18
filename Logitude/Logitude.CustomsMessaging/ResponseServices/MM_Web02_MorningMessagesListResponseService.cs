using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnifreightIIG.Common.MorningMessagesListServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class MM_Web02_MorningMessagesListResponseService : ResponseServiceBase
        <MorningMessageResponseData, MM_Web02_MorningMessagesList, MorningMessageRequestParams>
    {
        public override void Update(MM_Web02_MorningMessagesList customResponse, MorningMessageRequestParams requestParams)
        {
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData = new MorningMessageResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            List<MorningMessageResult> morningMessageResultList = new List<MorningMessageResult>();
            foreach (var morningMessageItem in customResponse.MorningMessage)
            {
                MorningMessageResult morningMessageResult = new MorningMessageResult()
                {
                    MessageID = morningMessageItem.MessageID.ToString(),
                    Category = morningMessageItem.Category.ToString(),
                    CategoryName = morningMessageItem.CategoryName,
                    Subject = morningMessageItem.Subject,
                    //Content = morningMessageItem.Content,
                    Content = StripHTML(morningMessageItem.Content),
                    MessageDate = morningMessageItem.MessageDate.Date.ToString("dd/MM/yyyy"),
                };
                morningMessageResultList.Add(morningMessageResult);
            }

            this.MyResponseData = new MorningMessageResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.UserMessage = "";
            this.MyResponseData.MorningMessageList = morningMessageResultList;
        }

        public override MorningMessageResponseData GetResponse(MM_Web02_MorningMessagesList customResponse, MorningMessageRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public static string StripHTML(string HTMLText)
        {
            if (string.IsNullOrWhiteSpace(HTMLText)) return "";

            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
        }
    }
}
