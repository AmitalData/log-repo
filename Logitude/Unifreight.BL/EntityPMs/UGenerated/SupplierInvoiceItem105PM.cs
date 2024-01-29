using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial  class SupplierInvoiceItem105PM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }
        
        public int? ABSAMOUNT { get; set; }
        
        public int? ACAMOUNT { get; set; }
        
        public long? AGNTPAYBITHA { get; set; }
        
        public long? AGNTPAYCUST { get; set; }
        
        public long? AGNTPAYTAX { get; set; }
        
        public int? AIRBAGSAMOUNT { get; set; }
        
        public string AUTONOMYBOOK { get; set; }
        
        public string BITHATAXITEM { get; set; }
        
        public string CURRENCYCODE { get; set; }
        
        public string DISCOUNTCODE { get; set; }
        
        public string ESSENTIALITEM { get; set; }
        
        public string EXEMPTIONCODE { get; set; }
        
        public string EXPPRAT { get; set; }
        
        public string EXPRESHIMONNO { get; set; }
        
        public long? EXTRAQNTY { get; set; }
        
        public decimal? FOREIGNCURRVAL { get; set; }
        
        public string GOODSDESC { get; set; }
        
        public string GUARANPERCENT { get; set; }
        
        public string GUARANTEENO { get; set; }
        
        public string GUARANTEETYPE { get; set; }
        
        public long? IMPORTADDITION { get; set; }
        
        public string KATALOGNO { get; set; }
        
        public string LICENSENO { get; set; }
     
        public long? NIDHEMASPCNT { get; set; }
        
        public long? NIDHEMEHESPCNT { get; set; }
        
        public long? NISVALUE { get; set; }
        
        public int? ORDERLINE { get; set; }
        
        public string ORIGINCOUNTRY { get; set; }
        
        public string PRATMEHES { get; set; }
        
        public string PRATMEHESCAN { get; set; }
        
        public string PRIVATEIMPCURR { get; set; }
        
        public string PURCHCOUNTRY { get; set; }
        
        public long? QUANTITY { get; set; }
        
        public long? RAISEPERCENT { get; set; }
        
        public long? RAISEVALUE { get; set; }
        
        public string STANDARDNO { get; set; }
        
        public string SUPPLIERACCOUNT { get; set; }
        
        public string TARIFFCODE { get; set; }
        
        public bool? TSVIRA { get; set; }
        
        public string UNITID { get; set; }
        
        public string VEHICLECODE { get; set; }
        
        public decimal? WHOLESALEPRICE { get; set; }

        public global::System.Nullable<long> STSQNTY { get; set; }

        public string PRATMEHESN { get; set; }

        public string ORIGINCOUNTRYN { get; set; }
 
        public string PURCHCOUNTRYN { get; set; }

        public LastLine105PM LastLine105PM { get; set; }

        List<CCUCARLPM> _CCUCARLs;

        public List<CCUCARLPM> CCUCARLs
        {
            get { return _CCUCARLs = _CCUCARLs ?? new List<CCUCARLPM>(); }
            set
            {
                _CCUCARLs = value;
            }
        }

        private List<CCUCARLPM> _DeletedCCUCARLPM;

        public List<CCUCARLPM> DeletedCCUCARLPM
        {
            get { return _DeletedCCUCARLPM = _DeletedCCUCARLPM ?? new List<CCUCARLPM>(); }
            set { _DeletedCCUCARLPM = value; }
        }

        List<CCUCARPM> _CCUCARs;

        public List<CCUCARPM> CCUCARs
        {
            get { return _CCUCARs = _CCUCARs ?? new List<CCUCARPM>(); }
            set
            {
                _CCUCARs = value;
            }
        }

        private List<CCUCARPM> _DeletedCCUCARPM;

        public List<CCUCARPM> DeletedCCUCARPM
        {
            get { return _DeletedCCUCARPM = _DeletedCCUCARPM ?? new List<CCUCARPM>(); }
            set { _DeletedCCUCARPM = value; }
        }
        // moran 9.3.16 - AMI-55747 -->
        List<CCUCARSCPM> _CCUCARSCs;

        public List<CCUCARSCPM> CCUCARSCs
        {
            get { return _CCUCARSCs = _CCUCARSCs ?? new List<CCUCARSCPM>(); }
            set
            {
                _CCUCARSCs = value;
            }
        }

        private List<CCUCARSCPM> _DeletedCCUCARSCPM;

        public List<CCUCARSCPM> DeletedCCUCARSCPM
        {
            get { return _DeletedCCUCARSCPM = _DeletedCCUCARSCPM ?? new List<CCUCARSCPM>(); }
            set { _DeletedCCUCARSCPM = value; }
        }
        // moran 9.3.16 - AMI-55747 <--
    }
}
