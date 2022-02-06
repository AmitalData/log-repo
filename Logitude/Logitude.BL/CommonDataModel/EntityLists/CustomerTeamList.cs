using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    [DataContract]
    public class CustomerTeamList
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LocalName { get; set; }

        [DataMember]
        public bool InActive { get; set; }

        [DataMember]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public string CreatedByUserId { get; set; }

        [DataMember]
        public string UpdatedByUserId { get; set; }

        [DataMember]
        public string SearchFields { get; set; }

    }
}
