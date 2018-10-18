using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class DeficitFilesDetailResponseData : ResponseDataBase
    {
        public string FileStatus { get; set; }
        public string StatusName { get; set; }
        public string ExternalID { get; set; }
        public string ExternalName { get; set; }
        public string CustomOfficeNumber { get; set; }
        public string CustomOfficeName { get; set; }
        public string FilingNumber { get; set; }
        public string OpenFileCounter { get; set; }
        public string CloseFileCounter { get; set; }
        public string AgentExternalID { get; set; }
        public string AgentName { get; set; }
        public List<ExternalFilesDetailsResult> OpenFilesList { get; set; }
        public List<ExternalFilesDetailsResult> CloseFileList { get; set; }
        public List<ExternalPaymentOrderResult> PaymentOrderList { get; set; }
        public List<RequireDocumentsResult> RequireDocumentsList { get; set; }
    }

    public class ExternalFilesDetailsResult
    {
        public string ExternalID { get; set; }
        public string ExternalName { get; set; }
        public string FileNumber { get; set; }
        public string Numeral { get; set; }
        public string DisplayFileNumber { get; set; }
        public string DeficitEntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string DeficitEntityID { get; set; }
        public string ProductionDate { get; set; }
        public decimal UnpaidBalance { get; set; }
        public decimal EstimatedBalance { get; set; }
        public string EstimatedDate { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public decimal TotalComponentAmount { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public string CloseDate { get; set; }
        public string SecondaryStatus { get; set; }
    }

    public class ExternalPaymentOrderResult
    {
        public string ExternalID { get; set; }
        public string ExternalName { get; set; }
        public string PaymentID { get; set; }
        public string PaymentProcessType { get; set; }
        public string PaymentProcessName { get; set; }
        public decimal AmountSum { get; set; }
        public string ValidityDateTo { get; set; }
        public string CreateDate { get; set; }
        public string PaymentOrderPayDate { get; set; }
        public string PaymentOrderStatus { get; set; }
        public string PaymentOrderStatusName { get; set; }
    }
    public class RequireDocumentsResult
    {
        public string DocumentCode { get; set; }
        public string DocumentTypeName { get; set; }
        public string FileNumber { get; set; }
        public string Numeral { get; set; }
        public string DisplayFileNumber { get; set; }
        public string DocumentID { get; set; }
    }
}
