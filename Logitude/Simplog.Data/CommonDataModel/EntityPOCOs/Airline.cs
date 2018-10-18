using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Airline
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool AddedManually { get; set; }
        public string Prefix { get; set; }
        public string AWBAccount { get; set; }
        public bool CheckDigit { get; set; }
        public bool LimitedLength { get; set; }
        public string AccountNumber { get; set; }
        public string TTY { get; set; }
        public bool IsChampRegistered { get; set; }
        public bool ChampNeedsRegistration { get; set; }                
        public string GLSHKPIMA { get; set; }
        public bool IsGLSHKRegistered { get; set; }
        public bool GLSHKNeedsRegistration { get; set; }
        public bool ChampFWB { get; set; }
        public bool ChampFHL { get; set; }
        public bool ChampFSU { get; set; }
        public bool ChampFSRFSA { get; set; }
        public bool ChampFVRFVA { get; set; }
        public bool ChampFFRFFA { get; set; }
        public bool GLSHKFWB { get; set; }
        public bool GLSHKFHL { get; set; }
        public bool GLSHKFSU { get; set; }
        public bool GLSHKFSRFSA { get; set; }
        public bool GLSHKFVRFVA { get; set; }
        public bool GLSHKFFRFFA { get; set; }
        public bool IsAllowedInAirlinesRestriction { get; set; }
        public string RegistrationNotes { get; set; }
        public bool ChampRegistrationRequested { get; set; }
        public bool GLSHKRegistrationRequested { get; set; }
        public bool HasAdaptations { get; set; }
        public string ICAO { get; set; }
        public string RegistrationUpdatedBy { get; set; }
        public bool IsManagingProduct { get; set; }
        public bool IsProductMandatory { get; set; }
        public bool IsDescriptionOfGoodsFromList { get; set; }
        public int? ScheduleDays { get; set; }
        public bool NoAvailabilityInFVAMessages { get; set; }
        public bool IsDeclined { get; set; }
        public string DeclineNotes { get; set; }
        public string OldTTY { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public virtual Card Card { get; set; }
    }
}
