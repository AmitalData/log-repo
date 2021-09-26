using System;


namespace Logitude.FullAccounting.Test.Models
{

    public class AccountingIntegrityCheckPM
    {

        public string Id { get; set; }

        public int Tenant { get; set; }
        public DateTime CreateDateTimeUTC { get; set; }
        public string StatusCode { get; set; }
        public string ParametersXML { get; set; }
        public string ResultXML { get; set; }
        public bool HasException { get; set; }
        public DateTime? DoneDateTimeUTC { get; set; }
        public string StatusName { get; set; }
        public DateTime FromMonthInclusive { get; set; }
        public DateTime ToMonthInclusive { get; set; }
        public string SearchFields { get; set; }
        public bool ShouldFix { get; set; }
    }

}
