using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DecCourierStatusesView
    {
        [Key]
        public string CourierMasterId { get; set; }

        public int Tenant { get; set; }   
        public int QuantityNoDocuments { get; set; }
        public int QuantityNoClassification { get; set; }
        public int QuantityNoManifest { get; set; }
        public int QuantityNoDeclaration { get; set; }

        public string DocumentStatusCode { get; set; }
        public string CourierPaymentStatusCode { get; set; }
        public string CourierDeclarationStatusCode { get; set; }
        public string CourierManifestStatusCode { get; set; }
        public string IsCourierMissingClassification { get; set; }


    }
}
