using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MorningMessagesListServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class MM_Web01_MorningMessagesListRequestService : RequestServiceBase
        <MM_Web01_MorningMessagesListFilter, MorningMessageRequestParams>
    {
        public override MM_Web01_MorningMessagesListFilter GetRequest(MorningMessageRequestParams requestParams)
        {
            //Build request MM_Web01_MorningMessagesListFilter- Filter Morning Messages
            var myMM_Web01_MorningMessagesListFilter = new MM_Web01_MorningMessagesListFilter();
            int recepientTypeInt = 0;
            int categoryInt = 0;

            myMM_Web01_MorningMessagesListFilter.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter = new MM_Web01_MorningMessagesListFilterMorningMessagesFilter();
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.FromDate = requestParams.FromDate;
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.FromDateSpecified = requestParams.FromDate == null ? false : true;
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.ToDate = requestParams.ToDate;
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.ToDateSpecified = requestParams.ToDate == null ? false : true;
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.SubjectText = requestParams.SubjectText;
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.ContentText = requestParams.ContentText;
            if (requestParams.RecepientType != null)
            {
                int.TryParse(requestParams.RecepientType, out recepientTypeInt);
                myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.RecepientType = recepientTypeInt;
            }
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.RecepientTypeSpecified = recepientTypeInt > 0 ? true : false;
            if (requestParams.Category != null)
            {
                int.TryParse(requestParams.Category, out categoryInt);
                myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.Category = categoryInt;
            }
            myMM_Web01_MorningMessagesListFilter.MorningMessagesFilter.CategorySpecified = categoryInt > 0 ? true : false;

            this.MyRequestSheetParam = new RequestSheetParam();
            //this.MyRequestSheetParam.ObjectTableId1 = ObjectTabelRepository.GetObjectTableByName("Customs.Guarante");
            this.MyRequestSheetParam.RequestDescription = "בקשה להודעות בוקר";

            return myMM_Web01_MorningMessagesListFilter;
        }
    }
}
