using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.EntityLists
{
    public partial class ActivityList
    {
       // [DataMember]
       // public string UpdatedByUserName { get; set; }

        //[DataMember]
        //public byte[] LastModified { get; set; }

        //[DataMember]
        //public string Background { get; set; }

        //[DataMember]
        //public string ActivityTypePathCode { get; set; }

        //[DataMember]
        //public DateTime? SortByDate { get; set; }

        [DataMember]
        public DateTime? OverViewSortingDate { get; set; }

        //[DataMember]
        //public DateTime? UpcomingDate { get; set; }

        //[DataMember]
        //public bool PostToFollowers { get; set; }

        
       // [DataMember]
       // public string RecipientsEmails { get; set; }


        

    }
}
