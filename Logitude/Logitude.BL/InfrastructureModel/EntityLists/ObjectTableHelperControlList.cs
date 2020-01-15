using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class ObjectTableHelperControlList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }

        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }
        public string FeatureUniqeCode { get; set; }

    }
}