using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Models
{
    public static class DocumentData
    {
        public static string DocumentTypeAirManifestId { get; set; }
        public static string DocumentTypeGeneralMessageId { get; set; }
        public static string DocumentTypeCustomsId { get; set; }
        public static string ShipmentObjectTableId{ get; set; }
        public static string DocumentTypeAirManifestTemplateId { get; set; }
        public static string ShipmentId { get; internal set; }
    }
}
