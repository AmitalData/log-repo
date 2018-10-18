using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ContactsUnseenEntitie
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContactId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
    }
}
