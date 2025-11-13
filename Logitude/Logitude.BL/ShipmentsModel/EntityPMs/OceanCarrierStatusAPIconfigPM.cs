using System.ComponentModel.DataAnnotations;


namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class OceanCarrierStatusAPIconfigPM
    {
        [Key]
        public string Id { get; set; }        
        public int Tenant { get; set; }
        public string SCACCode { get; set; }   
        public string URL { get; set; }     
        public string ResponseFormat { get; set; }
        public int Frequency { get; set; }
        public int RateLimit { get; set; }
        public bool Inactive { get; set; }
        public int PeriodJourneyEnd { get; set; }
    }
}
