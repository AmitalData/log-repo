using Logitude.Customs.BL.BL;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using RtfPipe;
using System;
using UnifreightIIG.Common.ClassifGuidanceDetailsServiceReference;


namespace Logitude.CustomsMessaging.ResponseServices
{
    public class Get_CB_MSG_8323_ClassifGuidanceDetailsResponseService : ResponseServiceBase<GetClassifGuidanceDetailsResponseData, CB_NG_8323_ClassifGuidanceDetailsOut, GetClassifGuidanceDetailsRequestParams>
    {
        public override void Update(CB_NG_8323_ClassifGuidanceDetailsOut customResponse, GetClassifGuidanceDetailsRequestParams requestParams)
        {

            if (customResponse?.ClassifGuidanceDetailsOut == null)
            {
                this.MyResponseData = new GetClassifGuidanceDetailsResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = false;
                this.MyResponseData.UserMessage = "לא התקבלו פרטי הנחיות סיווג";
                return;
            }
            string rtf = customResponse.ClassifGuidanceDetailsOut?.ClassifGuidanceText;

            this.MyResponseData = new GetClassifGuidanceDetailsResponseData()
            {
                classificationGuidanceNumber = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.classificationGuidanceNumber,
                title = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.title,
                classificationGuidanceTypeName = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.classificationGuidanceTypeName,
                fullClassificationItem = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.fullClassificationItem,
                createDate = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.createDate,
                expirationDate = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut?.expirationDate,
                publicationDate = customResponse.ClassifGuidanceDetailsOut.GeneraClassifGuidanceDetailsOut.publicationDate,
                classificationGuidanceTextRTF = !string.IsNullOrEmpty(rtf) && IsValidRtf(rtf) ? ConvertRtfToHtml(rtf) : rtf
        };
            
            if(customResponse.ClassifGuidanceDetailsOut?.ClassifGuidanceAttachedCI != null)
            {
                foreach (var item in customResponse.ClassifGuidanceDetailsOut.ClassifGuidanceAttachedCI)
                {
                    MyResponseData.classifGuidanceAttached.Add(new ClassifGuidanceAttached()
                    {
                        fullClassification = item.fullClassification,
                        attachedCustomsItemID = item.attachedCustomsItemID
                    });
                }
            }
          

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = " פרטי הנחיות סיווג התקבלו בהצלחה";


        }


        public static bool IsValidRtf(string rtf)
        {
            // לבדוק אם הטקסט מתחיל ב-{rtf1 ומסתיים ב-} 
            return rtf.Trim().StartsWith(@"{\rtf1") && rtf.Trim().EndsWith("}");
        }
        public string ConvertRtfToHtml(string rtf)
        {
            string html = Rtf.ToHtml(rtf);

            return html;
        }


        public override GetClassifGuidanceDetailsResponseData GetResponse(CB_NG_8323_ClassifGuidanceDetailsOut customResponse, GetClassifGuidanceDetailsRequestParams requestParams)
        {
            return this.MyResponseData;
        }
       
    }
}
