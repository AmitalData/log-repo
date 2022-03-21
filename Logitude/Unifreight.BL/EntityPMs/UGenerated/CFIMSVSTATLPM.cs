using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIMSVSTATLPM : EntityPM
    {
        public string GUID { get; set; }
        public long FILENO { get; set; }
        public string COMID { get; set; }
        public DateTime CREATEDATE { get; set; }
        public int PAGENUM { get; set; }
        public int LINENUM { get; set; }
        public int? LINECOUNTER { get; set; }
        public string PRATMEHES { get; set; }
        public string TARIFFCODE { get; set; }
        public string CUSTOMSSUPPLIERID { get; set; }
        public string CUSTOMERID { get; set; }
        public string CATALOGID { get; set; }
        public string CATALOGNAME { get; set; }
        public decimal? ITEMPRICE { get; set; }
        public double? OCRQUANTITY { get; set; }
        public string OCRQUANTITYTYPE { get; set; }
        public double? AMOUNT { get; set; }
        public string ORIGINID { get; set; }

    }
}
