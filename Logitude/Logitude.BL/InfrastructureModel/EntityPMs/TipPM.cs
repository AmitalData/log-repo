using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class TipPM
    {
        [Key]
        public string Code { get; set; }


        public int Tenant { get; set; }
        public bool VisibilityDefaultValue { get; set; }
        public string ShortTextCodeCode { get; set; }
        public string ShortTextCodeId { get; set; }
        public string ObjectTableId { get; set; }
    }
}