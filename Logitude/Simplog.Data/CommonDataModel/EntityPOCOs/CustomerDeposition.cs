using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   
    public class CustomerDeposition
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomsShipperId { get; set; }
        public string DepositionNumber { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("CustomsShipperId")]
        public virtual CustomsShipper CustomsShipper { get; set; }




    }






}
