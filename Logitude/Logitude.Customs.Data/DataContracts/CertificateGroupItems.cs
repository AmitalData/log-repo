using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts
{
    public class CertificateGroupItems
    {
        [Key]
        public Guid Id { get; set; }
        public string ClassificationCode { get; set; }
        public string ItemCode { get; set; }
        public string OriginCountryCode { get; set; }
        public string VendorNumber { get; set; }
        public List<SupplierInvoiceItemKeys> SupplierInvoiceItems { get; set; }
    }
}
