using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIMSVLINEPM : EntityPM
    {
        public long FILENO { get; set; }

        public string COMID { get; set; }

        public int PAGENUM { get; set; }

        public int LINENUM { get; set; }

        public string PRATMEHES { get; set; }

        public string TARIFFCODE { get; set; }

        public int? TOP { get; set; }

        public int? HEIGHT { get; set; }

        public int? GROUPNUM { get; set; }

        public string YEVU { get; set; }

        public string REMARK { get; set; }

        public string SUGGESTM { get; set; }

        public string SUGGESTI { get; set; }

        public string SUGGESTDET { get; set; }

        public int? STATUS { get; set; }

        public string TAXEXEMPT { get; set; }

        public double? INVOICEQUANTITY { get; set; }

        public string INVOICEQUANTITYTYPE { get; set; }

        public string QUETYPE { get; set; }

        public string CATALOGID { get; set; }

        public string CATALOGNAME { get; set; }

        public double? AMOUNT { get; set; }

        public double? STATAMOUNT { get; set; }

        public string STATTYPE { get; set; }

        public string ORIGINID { get; set; }

        public decimal? ITEMPRICE { get; set; }

        public double? OCRQUANTITY { get; set; }

        public string OCRQUANTITYTYPE { get; set; }

        public int? LINECOUNTER { get; set; }

    }
}
