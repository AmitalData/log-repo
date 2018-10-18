using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Participant
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int ForwarderTenant { get; set; }
        public string TTY { get; set; }
        public bool Registered { get; set; }
        public bool RegistrationRequested { get; set; }
        public string RegistrationUpdatedBy { get; set; }
        public bool IsDirect { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string FWBNotifyContacts { get; set; }
        public string FHLNotifyContacts { get; set; }
        public string FFRNotifyContacts { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }

        public virtual Card Card { get; set; }

        [ForeignKey("ForwarderTenant")]
        public virtual Tenant Forwarder { get; set; }
    }
}
