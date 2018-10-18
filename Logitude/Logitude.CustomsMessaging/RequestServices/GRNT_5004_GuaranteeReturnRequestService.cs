
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GuaranteeReturnRequestApprovalInfoServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class GRNT_5004_GuaranteeReturnRequestService : RequestServiceBase
        <GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequest, GuaranteeReturnRequesRequestParams>
    {
        public override GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequest GetRequest(GuaranteeReturnRequesRequestParams requestParams)
        {
            //Build request GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequest(5004)- Guarantee Return Request

            var my_GuaranteeReturnRequest = new GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequest();
            decimal guaranteeAmount;
            int guaranteeType;

            my_GuaranteeReturnRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };

            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails = new GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequestGuaranteeCertificateReturnGuaranteedRequestDetails();
            decimal.TryParse(requestParams.GuaranteeAmountToReturn, out guaranteeAmount);
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.guaranteeAmountToReturn = guaranteeAmount;
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.guaranteeExternalCertificateNumber = requestParams.GuaranteeExternalCertificateNumber;
            int guarantorID;
            int.TryParse(requestParams.GuarantorID, out guarantorID);
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.GuarantorID = guarantorID;
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.GuarantorIDSpecified = requestParams.GuarantorID != null ? true : false;
            int.TryParse(requestParams.GuaranteeCertificateType, out guaranteeType);
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.guaranteeCertificateType = guaranteeType;
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.reason = requestParams.Reason;
            if (requestParams.FileNumber != null)
            {
                int numeral = 0;
                my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.TPGIdentifier = new TPGIdentifier();
                my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.TPGIdentifier.fileNumber = requestParams.FileNumber;
                int.TryParse(requestParams.Numeral,out numeral);
                my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestDetails.TPGIdentifier.numeral = numeral;
            }

            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestSubmitter = new GRNT_MSG16_GuaranteeCertificateReturnGuaranteedRequestGuaranteeCertificateReturnGuaranteedRequestSubmitter();
            int messageId = 0;
            int.TryParse(requestParams.MsgId,out messageId);
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestSubmitter.msgID = messageId;
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestSubmitter.msgIDSpecified = requestParams.MsgId != null ? true : false;
            int guaranteedID = 0;
            int.TryParse(requestParams.GuaranteedID, out guaranteedID);
            my_GuaranteeReturnRequest.GuaranteeCertificateReturnGuaranteedRequestSubmitter.guaranteedID = guaranteedID;

            var conditionsList = new List<int?>();
            foreach (var conditionItem in requestParams.ConditionCodeList)
            {
                int condition = 0;
                int.TryParse(conditionItem,out condition);
                conditionsList.Add(condition);
            }
            my_GuaranteeReturnRequest.conditions = conditionsList.ToArray();

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Guarantee");
            this.MyRequestSheetParam.RequestDescription = "בקשה להחזרת ערבות";

            return my_GuaranteeReturnRequest;
        }
    }
}
