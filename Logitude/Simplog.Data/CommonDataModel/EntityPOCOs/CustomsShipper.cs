
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{

    public class CustomsShipper
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomsShipperCode { get; set; }
        public string ValidDepositionNumber { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public bool FutureDepositionExist { get; set; }
        //public virtual Card Card { get; set; }

    }




}
