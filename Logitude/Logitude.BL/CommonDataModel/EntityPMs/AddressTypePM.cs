using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class AddressTypePM
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
