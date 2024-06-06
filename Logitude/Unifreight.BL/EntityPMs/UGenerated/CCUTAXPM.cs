using Logitude.Server.Tools;
using System;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUTAXPM : EntityPM
    {
        public decimal? ADDEFINEDTAX { get; set; }

        public decimal? ADDIMPORT { get; set; }

        public decimal? ADDTAXRATE { get; set; }

        public decimal? DEFINEDTAX { get; set; }

        public int FILENO { get; set; }

        public int? GOODSNO { get; set; }

        public int LINENO { get; set; }

        public decimal? POSTPONEDTAX { get; set; }

        public string PRATMEHES { get; set; }

        public decimal? TAXAMOUNT { get; set; }

        public decimal? TAXBASIS { get; set; }

        public System.Nullable<short> TAXCALCCODE { get; set; }

        public decimal? TAXRATE { get; set; }

        public decimal? TAXTOPAY { get; set; }

        public string TAXTYPE { get; set; }

        public string PRATMEHESN { get; set; }

        public string TAXTYPEN { get; set; }
        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
