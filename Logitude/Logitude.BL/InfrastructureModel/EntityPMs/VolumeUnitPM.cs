using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class VolumeUnitPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string PrintAs { get; set; }
        public string SearchFields { get; set; }
    }
}