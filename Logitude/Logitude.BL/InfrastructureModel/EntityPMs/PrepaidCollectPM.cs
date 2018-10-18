using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class PrepaidCollectPM
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool DisplayInLOV { get; set; }
        public string SearchFields { get; set; }
    }
}