// moran 5.7.15 - Task 13442
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.GuaranteeFileFilterParamServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TPG_NG_8247_Web06_GuaranteeFilesDetailResponseService
        : ResponseServiceBase<GuaranteeResponseData, TPG_NG_8247_Web06_GuaranteeFilesDetail, GuaranteeRequestParams>
    {
        public override void Update(TPG_NG_8247_Web06_GuaranteeFilesDetail customResponse, GuaranteeRequestParams requestParams)
        {
            //Analyze message 8246- Guarantee Files Detail
            this.MyResponseData = new GuaranteeResponseData();
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            //Get General Details
            //this.MyResponseData. = customResponse.GeneralDetails.fileStatus.ToString();
            this.MyResponseData.StatusName = customResponse.GeneralDetails.statusName;
            this.MyResponseData.CustomOfficeNumber = customResponse.GeneralDetails.customOfficeNumber.ToString();
            this.MyResponseData.CustomOfficeName = customResponse.GeneralDetails.customOfficeName;
            this.MyResponseData.DisplayFileNumber = customResponse.GeneralDetails.displayFileNumber;
            this.MyResponseData.AgentExternalID = customResponse.GeneralDetails.agentNumber.ToString();
            this.MyResponseData.AgentName = customResponse.GeneralDetails.agentName;

            if (customResponse.GeneralDetails.creditLimitSpecified == true)
            {
                this.MyResponseData.CreditLimit = customResponse.GeneralDetails.creditLimit.Value.ToString("N2");
            }
            this.MyResponseData.EntityTypeName = customResponse.GeneralDetails.entityTypeName;
            if (customResponse.GeneralDetails.creditBalanceSpecified == true)
            {
                this.MyResponseData.CreditBalance = customResponse.GeneralDetails.creditBalance.Value.ToString("N2");
            }
            this.MyResponseData.GuaranteedName = customResponse.GeneralDetails.guaranteedName;
            this.MyResponseData.EntityNumber = customResponse.GeneralDetails.entityNumber;
            if (customResponse.GeneralDetails.guaranteeExecutedAmountAdjustedSpecified == true)
            {
                this.MyResponseData.GuaranteeExecutedAmountAdjusted = customResponse.GeneralDetails.guaranteeExecutedAmountAdjusted.Value.ToString("N2");
            }
            if (customResponse.GeneralDetails.guaranteeAmountSpecified == true)
            {
                this.MyResponseData.GuaranteeAmount = customResponse.GeneralDetails.guaranteeAmount.Value.ToString("N2");
            }
            this.MyResponseData.Validity = customResponse.GeneralDetails.validity.Date.ToString("dd/MM/yyyy");

            if (customResponse.GuaranteeCertificate != null)
            {
                List<ExternalGuaranteeLettersResult> guaranteeLettersList = new List<ExternalGuaranteeLettersResult>();
                foreach (var guaranteeLettersItem in customResponse.GuaranteeCertificate)
                {
                    var guaranteeLettersDetails = new ExternalGuaranteeLettersResult();
                    guaranteeLettersDetails.AvaliableCertificateAmount = guaranteeLettersItem.avaliableCertificateAmount.ToString();
                    guaranteeLettersDetails.CertificateAllocation = guaranteeLettersItem.certificateAllocation.ToString("N2");
                    guaranteeLettersDetails.CertificateAmount = guaranteeLettersItem.certificateAmount.ToString("N2");
                    guaranteeLettersDetails.CertificateID = guaranteeLettersItem.certificateID.ToString();
                    guaranteeLettersDetails.GuaranatorName = guaranteeLettersItem.guaranatorName;
                    guaranteeLettersDetails.GuaranteeExternalCertificateNumebr = guaranteeLettersItem.guaranteeExternalCertificateNumebr.ToString();
                    guaranteeLettersDetails.GuaranteeStatusName = guaranteeLettersItem.guaranteeStatusName;
                    guaranteeLettersDetails.GuaranteeTypeName = guaranteeLettersItem.guaranteeTypeName;
                    guaranteeLettersDetails.GuaranteeValidityDate = guaranteeLettersItem.guaranteeValidityDate.Date.ToString("dd/MM/yyyy");
                    guaranteeLettersList.Add(guaranteeLettersDetails);
                }
                this.MyResponseData.GuaranteeLettersList = guaranteeLettersList;
            }

            if (customResponse.CreditUtilization != null)
            {
                List<ExternalCreditTransactionsResult> creditTransactionsList = new List<ExternalCreditTransactionsResult>();
                foreach (var creditTransactionsItem in customResponse.CreditUtilization)
                {
                    
                        var creditTransactionsDetails = new ExternalCreditTransactionsResult();
                        creditTransactionsDetails.CreditTransactionAmount = creditTransactionsItem.creaditTransactionAmount.ToString("N2");
                        creditTransactionsDetails.CreditTransactionDate = creditTransactionsItem.creaditTransactionDate.Date.ToString("dd/MM/yyyy");
                        creditTransactionsDetails.CreditTransactionName = creditTransactionsItem.creaditTransactionName;
                        creditTransactionsDetails.EntityNumber = creditTransactionsItem.entityNumber.ToString();
                        creditTransactionsDetails.EntityTypeName = creditTransactionsItem.entityTypeName.ToString();
                        creditTransactionsList.Add(creditTransactionsDetails);
                }
                
                this.MyResponseData.CreditTransactionsList = creditTransactionsList;
            }
            /*
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
            }*/

            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שליחת מסר שאילתא לערבויות הצליחה";
        }

        public override GuaranteeResponseData GetResponse(TPG_NG_8247_Web06_GuaranteeFilesDetail customResponse, GuaranteeRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
