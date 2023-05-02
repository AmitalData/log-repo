using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public class EventRemark
        {
            string dbms;

            [Key]
            [Column("Id")]
            public string Id { get; set; }
            [Column("Tenant")]
            public int Tenant { get; set; }
            [Column("CreateDate")]
            public DateTime CreateDate { get; set; }
            [ForeignKey("CreatedByUser")]
            [Column("CreatedByUserId")]
            public string CreatedByUserId { get; set; }

            public virtual User CreatedByUser { get; set; }
            [Column("SearchFields")]
            public string SearchFields { get; set; }
            [ForeignKey("EventType")]
            [Column("EventTypeId")]
            public string EventTypeId { get; set; }

            public virtual EventType EventType { get; set; }
            [ForeignKey("PartnerType")]
            [Column("PartnerTypeId")]
            public string PartnerTypeId { get; set; }

            public virtual PartnerType PartnerType { get; set; }
            [Column("IsChoose")]
            public bool IsChoose { get; set; }
        
    }

}
