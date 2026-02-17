using Logitude.Server.Tools;
namespace Unifreight.BL.EntityPMs
{
    public partial class CCUTAXPM : EntityPM
    {
        public double? ADDEFINEDTAX { get; set; }

        public double? ADDIMPORT { get; set; }

        public double? ADDTAXRATE { get; set; }

        public double? DEFINEDTAX { get; set; }

        public int FILENO { get; set; }

        public int? GOODSNO { get; set; }

        public int LINENO { get; set; }

        public double? POSTPONEDTAX { get; set; }

        public string PRATMEHES { get; set; }

        public double? TAXAMOUNT { get; set; }

        public double? TAXBASIS { get; set; }

        public System.Nullable<short> TAXCALCCODE { get; set; }

        public double? TAXRATE { get; set; }

        public double? TAXTOPAY { get; set; }

        public string TAXTYPE { get; set; }

        public string PRATMEHESN { get; set; }

        public string TAXTYPEN { get; set; }
    }
}
