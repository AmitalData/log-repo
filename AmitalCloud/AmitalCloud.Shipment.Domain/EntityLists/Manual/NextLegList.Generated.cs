using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class NextLegList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}