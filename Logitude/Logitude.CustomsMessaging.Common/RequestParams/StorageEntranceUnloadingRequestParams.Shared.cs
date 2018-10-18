using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class StorageEntranceUnloadingRequestParams : RequestParamsBase
    {
        public string DeclarationId { get; set; }
        public string DeclarationNumber { get; set; }
        public string ConsignmentNumber { get; set; }
        public string ManifestNumber { get; set; }
        public string LineNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public string Quantity { get; set; }
        public string GrossWeight { get; set; }
        public string PackageTypeCode { get; set; }
    }
}