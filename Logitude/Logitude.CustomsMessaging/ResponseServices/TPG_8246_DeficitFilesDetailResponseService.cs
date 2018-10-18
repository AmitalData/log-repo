using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.DeficitFileServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TPG_8246_DeficitFilesDetailResponseService
        : ResponseServiceBase<DeficitFilesDetailResponseData, TPG_NG_8246_Web04_DeficitFilesDetail, DeficitFileFilterRequestParams>
    {
        public override void Update(TPG_NG_8246_Web04_DeficitFilesDetail customResponse, DeficitFileFilterRequestParams requestParams)
        {
            //Analyze message 8246- Deficit Files Detail
            this.MyResponseData = new DeficitFilesDetailResponseData();
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            //Get General Details
            if (customResponse.GeneralDetails != null)
            {
                this.MyResponseData.FileStatus = customResponse.GeneralDetails.fileStatus.ToString();
                this.MyResponseData.StatusName = customResponse.GeneralDetails.statusName;
                this.MyResponseData.ExternalID = customResponse.GeneralDetails.externalID.ToString();
                this.MyResponseData.ExternalName = customResponse.GeneralDetails.name;
                this.MyResponseData.CustomOfficeNumber = customResponse.GeneralDetails.customOfficeNumber.ToString();
                this.MyResponseData.CustomOfficeName = customResponse.GeneralDetails.customOfficeName;
                this.MyResponseData.FilingNumber = customResponse.GeneralDetails.filingNumber;
                this.MyResponseData.OpenFileCounter = customResponse.GeneralDetails.openFileCounter.ToString();
                this.MyResponseData.CloseFileCounter = customResponse.GeneralDetails.closeFileCounter.ToString();
                this.MyResponseData.AgentExternalID = customResponse.GeneralDetails.agentExternalID.ToString();
                this.MyResponseData.AgentName = customResponse.GeneralDetails.agentName;
            }

            if (customResponse.OpenFiles != null)
            {
                List<ExternalFilesDetailsResult> openFilesList = new List<ExternalFilesDetailsResult>();
                foreach (var openFileItem in customResponse.OpenFiles)
                {
                    foreach (var fileDetailsItem in openFileItem.OpenFileList)
                    {
                        var openFileDetails = new ExternalFilesDetailsResult();
                        openFileDetails.ExternalID = openFileItem.externalID.ToString();
                        openFileDetails.ExternalName = openFileItem.name;
                        openFileDetails.FileNumber = fileDetailsItem.fileNumber;
                        openFileDetails.Numeral = fileDetailsItem.Numeral.ToString();
                        openFileDetails.DisplayFileNumber = fileDetailsItem.displayFileNumber;
                        openFileDetails.DeficitEntityType = fileDetailsItem.DeficitEntityType.ToString();
                        openFileDetails.EntityTypeName = fileDetailsItem.entityTypeName;
                        openFileDetails.DeficitEntityID = fileDetailsItem.DeficitEntityID;
                        if (fileDetailsItem.productionDate.HasValue)
                        {
                            openFileDetails.ProductionDate = fileDetailsItem.productionDate.Value.Date.ToString("dd/MM/yyyy");
                        }
                        //openFileDetails.UnpaidBalance = fileDetailsItem.unpaidBalance.ToString("N2");
                        openFileDetails.UnpaidBalance = fileDetailsItem.unpaidBalance;
                        openFileDetails.EstimatedBalance = fileDetailsItem.estimatedBalance;
                        openFileDetails.EstimatedDate = fileDetailsItem.estimatedDate.Date.ToString("dd/MM/yyyy");
                        openFileDetails.Status = fileDetailsItem.status.ToString();
                        openFileDetails.StatusName = fileDetailsItem.statusName;
                        openFilesList.Add(openFileDetails);
                    }
                }
                this.MyResponseData.OpenFilesList = openFilesList;
            }

            if (customResponse.CloseFiles != null)
            {
                List<ExternalFilesDetailsResult> closeFilesList = new List<ExternalFilesDetailsResult>();
                foreach (var closeFileItem in customResponse.CloseFiles)
                {
                    foreach (var fileDetailsItem in closeFileItem.CloseFileList)
                    {
                        var openFileDetails = new ExternalFilesDetailsResult();
                        openFileDetails.ExternalID = closeFileItem.externalID.ToString();
                        openFileDetails.ExternalName = closeFileItem.name;
                        openFileDetails.FileNumber = fileDetailsItem.fileNumber;
                        openFileDetails.Numeral = fileDetailsItem.Numeral.ToString();
                        openFileDetails.DisplayFileNumber = fileDetailsItem.displayFileNumber;
                        openFileDetails.DeficitEntityType = fileDetailsItem.DeficitEntityType.ToString();
                        openFileDetails.EntityTypeName = fileDetailsItem.entityTypeName;
                        openFileDetails.DeficitEntityID = fileDetailsItem.DeficitEntityID;
                        if(fileDetailsItem.totalComponentAmount != null)
                        {
                            //openFileDetails.TotalComponentAmount = String.Format("{0:N2}",fileDetailsItem.totalComponentAmount);
                            openFileDetails.TotalComponentAmount = (decimal)fileDetailsItem.totalComponentAmount;
                        }
                        if (fileDetailsItem.totalRefundAmount != null)
                        {
                            openFileDetails.TotalRefundAmount = (decimal)fileDetailsItem.totalRefundAmount;
                        }
                        openFileDetails.CloseDate = fileDetailsItem.closeDate.Date.ToString("dd/MM/yyyy");
                        openFileDetails.SecondaryStatus = fileDetailsItem.secondaryStatus;
                        closeFilesList.Add(openFileDetails);
                    }
                }
                this.MyResponseData.CloseFileList = closeFilesList;
            }

            if (customResponse.PaymentOrder != null)
            {
                List<ExternalPaymentOrderResult> paymentOrderList = new List<ExternalPaymentOrderResult>();
                foreach (var paymentOrderItem in customResponse.PaymentOrder)
                {
                    foreach (var paymentDetailsItem in paymentOrderItem.PaymentList)
                    {
                        var paymentDetails = new ExternalPaymentOrderResult();
                        paymentDetails.ExternalID = paymentOrderItem.externalID.ToString();
                        paymentDetails.ExternalName = paymentOrderItem.name;
                        paymentDetails.PaymentID = paymentDetailsItem.paymentID.ToString();
                        paymentDetails.PaymentProcessType = paymentDetailsItem.paymentProcessType.ToString();
                        paymentDetails.PaymentProcessName = paymentDetailsItem.paymentProcessName;
                        paymentDetails.AmountSum = paymentDetailsItem.amountSum;
                        paymentDetails.ValidityDateTo = paymentDetailsItem.validityDateTo.Date.ToString("dd/MM/yyyy");
                        paymentDetails.CreateDate = paymentDetailsItem.createDate.Date.ToString("dd/MM/yyyy");
                        if (paymentDetailsItem.paymentOrderPayDate.HasValue)
                        {
                            paymentDetails.PaymentOrderPayDate = paymentDetailsItem.paymentOrderPayDate.Value.Date.ToString("dd/MM/yyyy");
                        }
                        paymentDetails.PaymentOrderStatus = paymentDetailsItem.paymentOrderStatus.ToString();
                        paymentDetails.PaymentOrderStatusName = paymentDetailsItem.paymentOrderStatusName;
                        paymentOrderList.Add(paymentDetails);
                    }
                }
                this.MyResponseData.PaymentOrderList = paymentOrderList;
            }

            if (customResponse.RequireDocuments != null)
            {
                List<RequireDocumentsResult> requireDocumentList = new List<RequireDocumentsResult>();
                foreach (var requireDocumentItem in customResponse.RequireDocuments)
                {
                    RequireDocumentsResult requireDocumentsResult = new RequireDocumentsResult();
                    requireDocumentsResult.DocumentCode = requireDocumentItem.documentCode.ToString();
                    requireDocumentsResult.DocumentTypeName = requireDocumentItem.typeName;
                    requireDocumentsResult.FileNumber = requireDocumentItem.fileNumber;
                    requireDocumentsResult.Numeral = requireDocumentItem.numeral.ToString();
                    requireDocumentsResult.DisplayFileNumber = requireDocumentItem.displayFileNumber;
                    requireDocumentsResult.DocumentID = requireDocumentItem.documentID;
                    requireDocumentList.Add(requireDocumentsResult);
                }
                this.MyResponseData.RequireDocumentsList = requireDocumentList;
            }

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שליחת מסר שאילתא לגרעונות הצליחה";
        }

        public override DeficitFilesDetailResponseData GetResponse(TPG_NG_8246_Web04_DeficitFilesDetail customResponse, DeficitFileFilterRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
