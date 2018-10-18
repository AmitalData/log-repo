using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class IATACodeList
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public string MeasurementCode { get; set; }
        public string DueTypeCode { get; set; }
        public bool IsIATA { get; set; }
        public bool InActive { get; set; }
        public string AirlineId { get; set; }
    }
}