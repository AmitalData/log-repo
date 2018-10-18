using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class ImporterDeclarationResponseData : ResponseDataBase
    {
        public List<PeriodDeclarationResult> PeriodDeclarationList { get; set; }
        public List<LoiDeclarationResult> LoiDeclarationList { get; set; }
        public List<SecurityDeclarationResult> SecurityDeclarationList { get; set; }
    }

    public class PeriodDeclarationResult
    {
        public string PeriodDeclarationID { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string CreateDate { get; set; }
        public string ValidityFrom { get; set; }
        public string ExpirationDate { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string DocumentID { get; set; }



        public string DBVendorID { get; set; }
        public string DBCountryCode { get; set; }
        
    }

    public class LoiDeclarationResult
    {
        public string DeclarationID { get; set; }
        public string LoiDeclarationID { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string CreateDate { get; set; }
        public string DocumentID { get; set; }
    }

    public class SecurityDeclarationResult
    {
        public string SecurityDeclarationType { get; set; }
        public string SecurityDeclarationName { get; set; }
        public string SecurityImporterDeclarationId { get; set; }
        public string DeclarationDate { get; set; }
        public string ExpirationDate { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string DocumentID { get; set; }
    }
}
