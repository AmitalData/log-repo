using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;


namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   public class ScreenSection
    {

        [Key]
        public string Id { get; set; }
        public string ScreenCode { get; set; }

        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string RelatedScreenCode { get; set; }
        public string CreatedByUserId { get; set; }



        
        public int Number { get; set; }


        public int NumberOfRows { get; set; }

        //[ForeignKey("ScreenCode")]
        //public virtual Screen Screen { get; set; }

        public bool Inactive { get; set; }


        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

    }
}
