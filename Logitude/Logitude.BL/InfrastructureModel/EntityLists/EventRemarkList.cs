using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityLists
{

        [DataContract]
        public partial class EventRemarkList
        {

            [Key]
            [DataMember]
            public string Id { get; set; }
            [DataMember]
            public int Tenant { get; set; }
            [DataMember]
            public DateTime CreateDate { get; set; }
            [DataMember]
            public string CreatedByUserId { get; set; }
            [DataMember]
            public string SearchFields { get; set; }
            [DataMember]
            public string EventTypeId { get; set; }
            [DataMember]
            public string PartnerTypeId { get; set; }
            [DataMember]
            public bool IsChoose { get; set; }
        }


}
