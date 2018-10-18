using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.EntityLists
{
    public partial class TicketList
    {
        //[DataMember]
        //public DateTime? LastActivityDate { get; set; }

        //[DataMember]
        //public string LastActivityTypeName { get; set; }

        //[DataMember]
        //public string LastActivityByUserName { get; set; }

        [DataMember]
        public byte[] LastModified { get; set; }

        //[DataMember]
        //public string LastStageName { get; set; }

        //[DataMember]
        //public string CompanyTableName { get; set; }

        //[DataMember]
        //public string RankName { get; set; }

        //[DataMember]
        //public bool IsResolveDue { get; set; }

        //[DataMember]
        //public string ResolveColor { get; set; }

        //[DataMember]
        //public bool IsResolveExamination { get; set; }

        //[DataMember]
        //public bool IsResponseDue { get; set; }

        //[DataMember]
        //public string ResponseColor { get; set; }

        //[DataMember]
        //public bool IsResponseExamination { get; set; }        
    }
}
