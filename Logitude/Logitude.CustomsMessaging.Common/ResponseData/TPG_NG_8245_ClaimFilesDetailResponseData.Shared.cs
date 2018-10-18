using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class TPG_NG_8245_ClaimFilesDetailResponseData : ResponseDataBase
    {
        public GeneralDetails GeneralDataDetails { get; set; }
        public List<FilesDetails> OpenFilesCollapsList { get; set; }
        public List<FilesDetails> CloseFilesCollapsList { get; set; }
        public List<RefundOrder> RefundOrderList { get; set; }
        public List<RequireDocuments> RequireDocumentsList { get; set; }

        public class GeneralDetails
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
        }

        public class FilesDetails
        {
            public string ExternalID { get; set; }
            public string ExternalName { get; set; }
            public string FileNumber { get; set; }
            public string Numeral { get; set; }
            public string DisplayFileNumber { get; set; }
            public string ClaimEntityType { get; set; }
            public string EntityTypeName { get; set; }
            public string ClaimEntityID { get; set; }
            public string CreateDate { get; set; }
            public string CloseDate { get; set; }
            public string ClaimAmount { get; set; }
            public string TotalComponentAmount { get; set; }
            public string TotalRefundAmount { get; set; }
            public string Status { get; set; }
            public string StatusName { get; set; }
        }

        public class RefundOrder
        {
            public string ExternalID { get; set; }
            public string ExternalName { get; set; }
            public string PaymentOrderID { get; set; }
            public string PaymentProcessType { get; set; }
            public string PaymentProcessName { get; set; }
            public string AmountSum { get; set; }
            public string CreateDate { get; set; }
            public string ValidityDateTo { get; set; }
            public string PaymentOrderStatus { get; set; }
            public string PaymentOrderStatusName { get; set; }
        }

        public class RequireDocuments
        {
            public string DocumentCode { get; set; }
            public string TypeName { get; set; }
            public string FileNumber { get; set; }
            public string Numeral { get; set; }
            public string DisplayFileNumber { get; set; }
            public string DocumentID { get; set; }
        }

    }
}
