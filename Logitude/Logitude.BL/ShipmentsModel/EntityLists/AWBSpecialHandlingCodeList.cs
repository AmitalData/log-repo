using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class AWBSpecialHandlingCodeList
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsIATA { get; set; }
        public bool InActive { get; set; }
        public string AirlineId { get; set; }
        public string SearchFields { get; set; }
    }
}