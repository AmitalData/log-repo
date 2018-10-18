using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.RTGSInfoServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TSH_WEB8289_9060_RTGSInfoQueryResponseService : ResponseServiceBase<
         RTGSInfoQueryResponseData, TSH_NG_9060_Web07_RTGSInfo, CreditQueryRequestParams>
    {
        public override RTGSInfoQueryResponseData GetResponse(TSH_NG_9060_Web07_RTGSInfo customResponse, CreditQueryRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(TSH_NG_9060_Web07_RTGSInfo customResponse, CreditQueryRequestParams requestParams)
        {
            var context = CustomContext.GetContext(requestParams.Tenant);
            //DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this.MyResponseData = new RTGSInfoQueryResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;


            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null || customResponse.Exception != null)
            {
                if (customResponse.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.Exception.ExeptionDescription;
                }
                else
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                this.MyResponseData.HasException = true;
                return;
            }

            if(customResponse.RTGS_Header != null)
            {
                this.MyResponseData.ActiveInd = customResponse.RTGS_Header.ActiveInd;
                this.MyResponseData.CreationDate = customResponse.RTGS_Header.CreationDate.Value.ToString("dd/MM/yyyy");
                this.MyResponseData.GeneralOutputRemarks = customResponse.RTGS_Header.GeneralOutputRemarks;
                this.MyResponseData.QueryDate = customResponse.RTGS_Header.QueryDate.Date.ToString("dd/MM/yyyy");
                this.MyResponseData.QueryTime = customResponse.RTGS_Header.QueryTime.Date.ToString("dd/MM/yyyy");
                this.MyResponseData.RTGSCurrentBalance = String.Format("{0:N2}", customResponse.RTGS_Header.RTGSCurrentBalance);
                this.MyResponseData.RTGSDeposits = String.Format("{0:N2}", customResponse.RTGS_Header.RTGSDeposits);
                this.MyResponseData.RTGSRefund = String.Format("{0:N2}", customResponse.RTGS_Header.RTGSRefund);
                this.MyResponseData.RTGSUsed = String.Format("{0:N2}", customResponse.RTGS_Header.RTGSUsed);
            }

            if(customResponse.RTGS_Transactions != null)
            {
                MyResponseData.TransactionsList = new List<RTGSInfoQueryResponseData.TransactionResult>();

                foreach (var transactionItem in customResponse.RTGS_Transactions)
                {
                    RTGSInfoQueryResponseData.TransactionResult newTransactionResult = new RTGSInfoQueryResponseData.TransactionResult();
                    newTransactionResult.EntityID = transactionItem.EntityID;
                    newTransactionResult.EntityType = transactionItem.EntityType;
                    if (transactionItem.PaymentDate != null && transactionItem.PaymentDateSpecified == true)
                    {
                        newTransactionResult.PaymentDate = transactionItem.PaymentDate.Value.ToString("dd/MM/yyyy");
                    }
                    newTransactionResult.PaymentID = String.Format("{0:N2}", transactionItem.PaymentID);
                    newTransactionResult.PaymentStatus = transactionItem.PaymentStatus;
                    newTransactionResult.RTGSBalance = String.Format("{0:N2}", transactionItem.RTGSBalance);
                    if (transactionItem.TransactionAmount != null && transactionItem.TransactionAmountSpecified == true)
                    {
                        newTransactionResult.TransactionAmount = String.Format("{0:N2}", transactionItem.TransactionAmount);
                    }
                    newTransactionResult.TransactionType = transactionItem.TransactionType;
                    newTransactionResult.UpdateUser = transactionItem.UpdateUser;

                    MyResponseData.TransactionsList.Add(newTransactionResult);
                }
            }
        }
    }
}
