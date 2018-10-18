using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ChargesExternalAccountsByProductList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PayablesGLAccount { get; set; }
        public string PayablesCostCenter { get; set; }
        public string ReceivablesGLAccount { get; set; }
        public string ReceivablesCostCenter { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ChargesTypeId { get; set; }
        public string ProductTypeCode { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
