using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class CustomsTransferHeaderList
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
        public string CreatedByUserName { get; set; }
        public string CustomsTransferTypeName { get; set; }
    }
}
