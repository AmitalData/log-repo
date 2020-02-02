using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class CustomsTransferHeader
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TransferNumber { get; set; }
        public DateTime? TransferDate { get; set; }
        public string FileName { get; set; }
        public string CreatedByUserId { get; set; }
        public string CustomsTransferTypeCode { get; set; }
        public string SearchFields { get; set; }        
        public string Notes { get; set; }
        public string ShipmentNumber { get; set; }
        public virtual CustomsTransferType CustomsTransferType { get; set; }        
        public virtual User CreatedByUser { get; set; }
    }
}
