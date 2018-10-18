using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class PrepaidCollectList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool DisplayInLOV { get; set; }
        public string SearchFields { get; set; }
    }
}