using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    [DataContract]
    public class EntityStatusPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ObjectTableId { get; set; }
        [DataMember]
        public int StatusWeight { get; set; }
        [DataMember]
        public string ObjectTableName { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public bool InActive { get; set; }
        //public int IndexOrder { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public bool IsHybrid { get; set; }
        public string DisplayName { get; set; }
    }
}