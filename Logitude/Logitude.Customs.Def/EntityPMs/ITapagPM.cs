using System;
namespace Logitude.Customs.Def.EntityPMs
{
    public interface ITapagPM
    {

        string TapagID { get; set; }
        //DateTime? CreateDate { get; set; }
        //string CustomerId { get; set; }
        //string CustomerName { get; set; }
        string CustomsBranchCode { get; set; }
        string CustomsBranchName { get; set; }
        //int? CustomsNumeral { get; set; }
        //string CustomsTapagFile { get; set; }
        DateTime? FollowDate { get; set; }
        //string ImporterId { get; set; }
        //string ImporterName { get; set; }
        bool IsClosed { get; set; }
        string LeadingFileNumber { get; set; }
        //string ProfessionUnitTypeCode { get; set; }
        //string ProfessionUnitTypeName { get; set; }
        //string RequestFileNumber { get; set; }
        //string SpecializationTypeCode { get; set; }
        //string SpecializationTypeName { get; set; }
        ///string TapagNumber { get; set; }
        string TapagTypeCode { get; set; }
        string TapagTypeName { get; set; }
        int Tenant { get; set; }
        DateTime? ValidityDate { get; set; }

        
    }
}
