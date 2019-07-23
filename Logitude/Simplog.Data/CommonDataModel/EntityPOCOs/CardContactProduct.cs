using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CardContactProduct
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardContactId { get; set; }        
        public string ProductTypeCode { get; set; }
        
        public virtual CardContact CardContact { get; set; }        
        public virtual ProductType ProductType { get; set; }
    }
}
