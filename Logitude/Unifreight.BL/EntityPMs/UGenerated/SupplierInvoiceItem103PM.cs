using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs.UGenerated;

namespace Unifreight.BL.EntityPMs
{
    public partial  class SupplierInvoiceItem103PM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public int ACCLINENO { get; set; }
      
        public int? ABSAMOUNT { get; set; }
        
        public int? ACAMOUNT { get; set; }
        
        public double? AGNTPAYBITHA { get; set; }
        
        public double? AGNTPAYCUST { get; set; }
        
        public double? AGNTPAYTAX { get; set; }
        
        public int? AIRBAGSAMOUNT { get; set; }
        
        public string AUTONOMYBOOK { get; set; }
        
        public string BITHATAXITEM { get; set; }
        
        public string CURRENCYCODE { get; set; }
        
        public string DISCOUNTCODE { get; set; }
        
        public string ESSENTIALITEM { get; set; }
        
        public string EXEMPTIONCODE { get; set; }
        
        public string EXPPRAT { get; set; }
        
        public string EXPRESHIMONNO { get; set; }
        
        public double? EXTRAQNTY { get; set; }
        
        public decimal? FOREIGNCURRVAL { get; set; }
        
        public string GOODSDESC { get; set; }
        
        public string GUARANPERCENT { get; set; }
        
        public string GUARANTEENO { get; set; }
        
        public string GUARANTEETYPE { get; set; }
        
        public double? IMPORTADDITION { get; set; }
        
        public int? ITEMLINENO { get; set; }
        
        public string ITEMNO { get; set; }
        
        public string KATALOGNO { get; set; }
        
        public string LICENSENO { get; set; }
        
        public double? NIDHEMASPCNT { get; set; }
        
        public double? NIDHEMEHESPCNT { get; set; }
        
        public double? NISVALUE { get; set; }
        
        public string ORIGINCOUNTRY { get; set; }
        
        public decimal? ORIGINVALUE { get; set; }
        
        public string PRATMEHES { get; set; }
        
        public string PRATMEHESCAN { get; set; }
        
        public string PRIVATEIMPCURR { get; set; }
        
        public string PURCHCOUNTRY { get; set; }
        
        public double? QUANTITY { get; set; }
        
        public double? RAISEPERCENT { get; set; }
        
        public double? RAISEVALUE { get; set; }
        
        public string STANDARDNO { get; set; }
        
        public string TARIFFCODE { get; set; }
        
        public bool? TSVIRA { get; set; }
        
        public string UNITID { get; set; }
        
        public string VEHICLECODE { get; set; }
        
        public decimal? WHOLESALEPRICE { get; set; }

        public string PRATMEHESN { get; set; }
  
        public string ORIGINCOUNTRYN { get; set; }

        public string PURCHCOUNTRYN { get; set; }

        public double? STSQNTY { get; set; }


        private List<SupplierInvoiceItem105PM> _DeletedSupplierInvoiceItems105;

        public List<SupplierInvoiceItem105PM> DeletedSupplierInvoiceItems105
        {
            get { return _DeletedSupplierInvoiceItems105 = _DeletedSupplierInvoiceItems105 ?? new List<SupplierInvoiceItem105PM>(); }
            set { _DeletedSupplierInvoiceItems105 = value; }
        }

        List<SupplierInvoiceItem105PM> _SupplierInvoiceItems105;

        public List<SupplierInvoiceItem105PM> SupplierInvoiceItems105
        {
            get { return _SupplierInvoiceItems105 = _SupplierInvoiceItems105 ?? new List<SupplierInvoiceItem105PM>(); }
            set { _SupplierInvoiceItems105 = value; }
        }

        List<CCUCRREQPM> _CCUCRREQPM;
        public List<CCUCRREQPM> CCUCRREQPM
        {
            get { return _CCUCRREQPM = _CCUCRREQPM ?? new List<CCUCRREQPM>(); }
            set { _CCUCRREQPM = value; }
        }

        public int CCUCRREQPMLastLine { get; set; }

        private List<CCUCRREQPM> _DeletedCCUCRREQPM;

        public List<CCUCRREQPM> DeletedCCUCRREQPM
        {
            get { return _DeletedCCUCRREQPM = _DeletedCCUCRREQPM ?? new List<CCUCRREQPM>(); }
            set { _DeletedCCUCRREQPM = value; }
        }

        public LastLine105PM LastLine105PM { get; set; }


        CCUSUPITEMSIPM _CCUSUPITEMSIPM;
        public CCUSUPITEMSIPM CCUSUPITEMSIPM
        {
            get { return _CCUSUPITEMSIPM = _CCUSUPITEMSIPM ?? new CCUSUPITEMSIPM(); }
            set { _CCUSUPITEMSIPM = value; }
        }
        private CCUSUPITEMSIPM _DeletedCCUSUPITEMSIPM;
        public CCUSUPITEMSIPM DeletedCCUSUPITEMSIPM
        {
            get { return _DeletedCCUSUPITEMSIPM = _DeletedCCUSUPITEMSIPM ?? new CCUSUPITEMSIPM(); }
            set { _DeletedCCUSUPITEMSIPM = value; }
        }
    }
}
