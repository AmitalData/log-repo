using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class GuaranteeResponseData : ResponseDataBase
    {
        public string DisplayFileNumber { get; set; }
        public string StatusName { get; set; }
        public string CreditLimit { get; set; }
        public string EntityTypeName { get; set; }
        public string CustomOfficeNumber { get; set; }
        public string CustomOfficeName { get; set; }
        public string CreditBalance { get; set; }
        public string GuaranteedName { get; set; }
        public string EntityNumber { get; set; }
        public string AgentExternalID { get; set; }
        public string AgentName { get; set; }
        public string GuaranteeExecutedAmountAdjusted { get; set; }
        public string GuaranteeAmount { get; set; }
        public string Validity { get; set; }

        public List<ExternalGuaranteeLettersResult> GuaranteeLettersList { get; set; }
        public List<ExternalCreditTransactionsResult> CreditTransactionsList { get; set; }
        public List<RequiredDocumentsResult> RequireDocumentsList { get; set; }
    }

    public class ExternalGuaranteeLettersResult
    {
        public string GuaranteeTypeName { get; set; }
        public string CertificateID { get; set; }
        public string GuaranteeExternalCertificateNumebr { get; set; }
        public string GuaranatorName { get; set; }
        public string GuaranteeValidityDate { get; set; }
        public string CertificateAmount { get; set; }
        public string CertificateAllocation { get; set; }
        public string AvaliableCertificateAmount { get; set; }
        public string GuaranteeStatusName { get; set; }
    }

    public class ExternalCreditTransactionsResult
    {
        public string CreditTransactionDate { get; set; }
        public string CreditTransactionName { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityNumber { get; set; }
        public string CreditTransactionAmount { get; set; }
    }
    public class RequiredDocumentsResult
    {
        public string DocumentCode { get; set; }
        public string DocumentTypeName { get; set; }
        public string FileNumber { get; set; }
        public string Numeral { get; set; }
        public string DisplayFileNumber { get; set; }
        public string DocumentID { get; set; }
    }

    
}
