using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ObjectTableHelperControlPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ControlPath { get; set; }
        public string Code { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }
        public string FeatureId { get; set; }
    }
}