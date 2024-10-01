using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class TransportModeList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        public string LocalName { get; set; }
    }
}