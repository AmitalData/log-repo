using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SaveCLAIMServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TPG_NG_8245_ClaimFilesDetailResponseService : ResponseServiceBase
        <TPG_NG_8245_ClaimFilesDetailResponseData, TPG_NG_8245_Web02_ClaimFilesDetail, TPG_NG_8244_ClaimFileFilterRequestParams>
    {
        public override void Update(TPG_NG_8245_Web02_ClaimFilesDetail customResponse, TPG_NG_8244_ClaimFileFilterRequestParams requestParams)
        {
            this.MyResponseData = new TPG_NG_8245_ClaimFilesDetailResponseData();
            MyResponseData.Succeeded = true;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לתביעות " + requestParams.FileNumber + " - " + requestParams.Numeral;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                MyResponseData.HasException = true;
                MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription; ;
                return;
            }

            #region General Details
            if (customResponse.GeneralDetails != null)
            {
                MyResponseData.GeneralDataDetails = new TPG_NG_8245_ClaimFilesDetailResponseData.GeneralDetails();
                MyResponseData.GeneralDataDetails.FileStatus = customResponse.GeneralDetails.fileStatus.ToString();
                MyResponseData.GeneralDataDetails.StatusName = customResponse.GeneralDetails.statusName;
                MyResponseData.GeneralDataDetails.ExternalID = customResponse.GeneralDetails.externalID.ToString();
                MyResponseData.GeneralDataDetails.ExternalName = customResponse.GeneralDetails.name;
                MyResponseData.GeneralDataDetails.CustomOfficeNumber = customResponse.GeneralDetails.customOfficeNumber;
                MyResponseData.GeneralDataDetails.CustomOfficeName = customResponse.GeneralDetails.customOfficeName;
                MyResponseData.GeneralDataDetails.FilingNumber = customResponse.GeneralDetails.filingNumber;
                MyResponseData.GeneralDataDetails.OpenFileCounter = String.Format("{0:N2}", customResponse.GeneralDetails.openFileCounter);
                MyResponseData.GeneralDataDetails.CloseFileCounter = String.Format("{0:N2}", customResponse.GeneralDetails.closeFileCounter);
                MyResponseData.GeneralDataDetails.AgentExternalID = customResponse.GeneralDetails.AgentExternalID.ToString();
                MyResponseData.GeneralDataDetails.AgentName = customResponse.GeneralDetails.AgentName;
            }
            #endregion General Details

            #region Open Files Details
            if (customResponse.OpenFiles != null)
            {
                MyResponseData.OpenFilesCollapsList = new List<TPG_NG_8245_ClaimFilesDetailResponseData.FilesDetails>();
                foreach (var openFileCollapsItem in customResponse.OpenFiles)
                {
                    foreach (var openFileDetailsItem in openFileCollapsItem.OpenFileList)
                    {
                        var openFileDetails = new TPG_NG_8245_ClaimFilesDetailResponseData.FilesDetails();
                        openFileDetails.ExternalID = openFileCollapsItem.externalID.ToString();
                        openFileDetails.ExternalName = openFileCollapsItem.name;
                        openFileDetails.FileNumber = openFileDetailsItem.fileNumber;
                        openFileDetails.Numeral = openFileDetailsItem.Numeral.ToString();
                        openFileDetails.DisplayFileNumber = openFileDetailsItem.displayFileNumber;
                        openFileDetails.ClaimEntityType = openFileDetailsItem.claimEntityType.ToString();
                        openFileDetails.EntityTypeName = openFileDetailsItem.entityTypeName;
                        openFileDetails.ClaimEntityID = openFileDetailsItem.claimEntityID;
                        openFileDetails.CreateDate = openFileDetailsItem.createDate.Date.ToString("dd/MM/yyyy");
                        openFileDetails.ClaimAmount = String.Format("{0:N2}", openFileDetailsItem.claimAmount);
                        openFileDetails.Status = openFileDetailsItem.status;
                        openFileDetails.StatusName = openFileDetailsItem.statusName;

                        MyResponseData.OpenFilesCollapsList.Add(openFileDetails);
                    }
                }
            }
            #endregion Open Files Details

            #region Close Files Details
            if (customResponse.CloseFiles != null && customResponse.CloseFiles.Collaps != null)
            {
                MyResponseData.CloseFilesCollapsList = new List<TPG_NG_8245_ClaimFilesDetailResponseData.FilesDetails>();
                foreach (var closeFileCollapsItem in customResponse.CloseFiles.Collaps)
                {
                    foreach (var closeFileDetailsItem in closeFileCollapsItem.CloseFileList)
                    {
                        var closeFileDetails = new TPG_NG_8245_ClaimFilesDetailResponseData.FilesDetails();
                        closeFileDetails.ExternalID = closeFileCollapsItem.externalID.ToString();
                        closeFileDetails.ExternalName = closeFileCollapsItem.name;
                        closeFileDetails.FileNumber = closeFileDetailsItem.fileNumber;
                        closeFileDetails.Numeral = closeFileDetailsItem.Numeral.ToString();
                        closeFileDetails.DisplayFileNumber = closeFileDetailsItem.displayFileNumber;
                        closeFileDetails.ClaimEntityType = closeFileDetailsItem.claimEntityType.ToString();
                        closeFileDetails.EntityTypeName = closeFileDetailsItem.entityTypeName;
                        closeFileDetails.ClaimEntityID = closeFileDetailsItem.claimEntityID;
                        closeFileDetails.TotalComponentAmount = String.Format("{0:N2}", closeFileDetailsItem.totalComponentAmount);
                        closeFileDetails.TotalRefundAmount = String.Format("{0:N2}", closeFileDetailsItem.totalRefundAmount);
                        closeFileDetails.CloseDate = closeFileDetailsItem.closeDate.Date.ToString("dd/MM/yyyy");
                        closeFileDetails.Status = closeFileDetailsItem.secondaryStatus;
                        closeFileDetails.StatusName = closeFileDetailsItem.secondaryStatus;

                        MyResponseData.CloseFilesCollapsList.Add(closeFileDetails);
                    }
                }
            }
            #endregion Close Files Details

            #region Refund Order Details
            if (customResponse.RefundOrder != null)
            {
                MyResponseData.RefundOrderList = new List<TPG_NG_8245_ClaimFilesDetailResponseData.RefundOrder>();
                foreach (var refundOrderItem in customResponse.RefundOrder)
                {
                    foreach (var refundOrderDetailsItem in refundOrderItem.RefundList)
                    {
                        var refundOrderDetails = new TPG_NG_8245_ClaimFilesDetailResponseData.RefundOrder();
                        refundOrderDetails.ExternalID = refundOrderItem.externalID.ToString();
                        refundOrderDetails.ExternalName = refundOrderItem.name;
                        refundOrderDetails.PaymentOrderID = refundOrderDetailsItem.paymentOrderID.ToString();
                        refundOrderDetails.PaymentProcessType = refundOrderDetailsItem.paymentProcessType.ToString();
                        refundOrderDetails.PaymentProcessName = refundOrderDetailsItem.paymentProcessName;
                        refundOrderDetails.AmountSum = String.Format("{0:N2}", refundOrderDetailsItem.amountSum);
                        refundOrderDetails.CreateDate = refundOrderDetailsItem.createDate.Date.ToString("dd/MM/yyyy");
                        refundOrderDetails.ValidityDateTo = refundOrderDetailsItem.validityDateTo.Date.ToString("dd/MM/yyyy");
                        refundOrderDetails.PaymentOrderStatus = refundOrderDetailsItem.paymentOrderStatus.ToString();
                        refundOrderDetails.PaymentOrderStatusName = refundOrderDetailsItem.paymentOrderStatusName;
                        MyResponseData.RefundOrderList.Add(refundOrderDetails);
                    }
                }
            }
            #endregion Refund Order Details

            #region Require Documents Details
            if (customResponse.RequireDocuments != null)
            {
                MyResponseData.RequireDocumentsList = new List<TPG_NG_8245_ClaimFilesDetailResponseData.RequireDocuments>();
                foreach (var requireDocumentItem in customResponse.RequireDocuments)
                {
                    var requireDocumentDetails = new TPG_NG_8245_ClaimFilesDetailResponseData.RequireDocuments();
                    requireDocumentDetails.DocumentCode = requireDocumentItem.documentCode.ToString();
                    requireDocumentDetails.TypeName = requireDocumentItem.typeName;
                    requireDocumentDetails.FileNumber = requireDocumentItem.fileNumber;
                    requireDocumentDetails.Numeral = requireDocumentItem.numeral.ToString();
                    requireDocumentDetails.DisplayFileNumber = requireDocumentItem.displayFileNumber;
                    requireDocumentDetails.DocumentID = requireDocumentItem.documentID;

                    MyResponseData.RequireDocumentsList.Add(requireDocumentDetails);

                }
            }
            #endregion Require Documents Details

            MyResponseData.HasException = false;
            MyResponseData.UserMessage = "שאילתת תביעות  " + requestParams.FileNumber + " - " + requestParams.Numeral;
        }

        public override TPG_NG_8245_ClaimFilesDetailResponseData GetResponse(TPG_NG_8245_Web02_ClaimFilesDetail customResponse, TPG_NG_8244_ClaimFileFilterRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
