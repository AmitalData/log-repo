using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIMSVDOCPM : EntityPM
    {
        public long FILENO { get; set; }

        public string COMID { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public string CREATEBY { get; set; }

        public string UPDATEBY { get; set; }

        public short? STATUS { get; set; }

        public string REMARK { get; set; }

        public int? TOTALPAGES { get; set; }

        public string CUSTOMERID { get; set; }

        public short? HASCHANGED { get; set; }

        public string QUETYPE { get; set; }

        public string GSTRING1 { get; set; }

        public string GSTRING2 { get; set; }

        public string GSTRING3 { get; set; }

        public DateTime? INVOICEDATE { get; set; }

        public decimal? INVOICEAMOUNT { get; set; }

        public string CURRENCYID { get; set; }

        public string CUSTOMSSUPPLIERID { get; set; }

        public string INCOTERMS { get; set; }

        public decimal? TAX { get; set; }

        public decimal? DISCOUNT { get; set; }

        public string ORIGINID { get; set; }

        public string MORE { get; set; }

        public decimal? ADDITIONAL { get; set; }

        public string INVOICENO { get; set; }

    }
}
