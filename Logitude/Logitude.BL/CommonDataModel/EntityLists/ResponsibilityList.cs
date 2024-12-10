using Logitude.BL.InfrastructureModel.EntityLists;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ResponsibilityList
    {
        [Key]
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string LocalName { get; set; }
        [DataMember]
        public string EnglishName { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public bool Inactive { get; set; }

    }
}