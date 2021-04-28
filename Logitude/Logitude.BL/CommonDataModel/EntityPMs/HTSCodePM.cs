using System.ComponentModel.DataAnnotations;


namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class HTSCodePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ItemId { get; set; }
        public string DestinationCountryId { get; set; }
        public string Code { get; set; }
        public string ApprovedByCustomer { get; set; }
        public bool InActive { get; set; }
    }
}
