using Logitude.Server.Tools;
namespace Unifreight.BL.EntityPMs
{
    public partial class CCUTAXPM : EntityPM
    {
        public long? ADDEFINEDTAX { get; set; }

        public long? ADDIMPORT { get; set; }

        public long? ADDTAXRATE { get; set; }

        public long? DEFINEDTAX { get; set; }

        public int FILENO { get; set; }

        public int? GOODSNO { get; set; }

        public int LINENO { get; set; }

        public long? POSTPONEDTAX { get; set; }

        public string PRATMEHES { get; set; }

        public long? TAXAMOUNT { get; set; }

        public long? TAXBASIS { get; set; }

        public System.Nullable<short> TAXCALCCODE { get; set; }

        public long? TAXRATE { get; set; }

        public long? TAXTOPAY { get; set; }

        public string TAXTYPE { get; set; }

        public string PRATMEHESN { get; set; }

        public string TAXTYPEN { get; set; }
    }
}
