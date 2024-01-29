using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUFILEMPM : EntityPM
    {
        public int FILENO { get; set; }

        public long? ACCEPTEDPRICE { get; set; }

        public string APARTMENTNO { get; set; }

        public string AUTONOMY { get; set; }

        public string BRANCHID { get; set; }

        public string CHANGE { get; set; }

        public long? CHANGINGVALUE { get; set; }

        public DateTime? CHARGESYSPRINT { get; set; }

        public long? CIFVALUE { get; set; }

        public string CITYSYMBOL { get; set; }

        public long? CLOSUREVALUE { get; set; }

        public string COINID { get; set; }

        public long? CURRENCYRATE { get; set; }

        public string CUSTOMAGENT { get; set; }

        public DateTime? CUSTOMAGENTPRINT { get; set; }

        public string CUSTOMERID { get; set; }

        public long? CUSTOMFILENO { get; set; }

        public DateTime? CUSTOMPRINT { get; set; }

        public string CUSTOMSBRANCH { get; set; }

        public string DEPARTID { get; set; }

        public DateTime? DRAFTDATE { get; set; }

        public string DRAWNO { get; set; }

        public string ENTRANCE { get; set; }

        public long? EXPENSEVALUE { get; set; }

        public string FAMILYNAME { get; set; }

        public long? FEECARRIER { get; set; }

        public long? FEEPLATFORM { get; set; }

        public short? FILECLOSE { get; set; }

        public string FIRSTNAME { get; set; }

        public DateTime? FOLUPDATE { get; set; }

        public string FROMIIG { get; set; }

        public long? GOODSVALUE { get; set; }

        public DateTime? GRANTDATE { get; set; }

        public DateTime? GRANTTIME { get; set; }

        public string GRNTTYPEID { get; set; }

        public long? GUARANTEEAMNT { get; set; }

        public string HOUSE { get; set; }

        public string IMPORTERID { get; set; }

        public string IMPORTERSTS { get; set; }

        public string IMPORTTYPE { get; set; }

        public long? INDEXVALUE { get; set; }

        public string INDICATORS { get; set; }

        public long? INSURANCEAMNT { get; set; }

        public string INSURANCECURR { get; set; }

        public long? INSURANCEPERCENT { get; set; }

        public long? INSURANCEVALUE { get; set; }

        public int? MEHESDRAFTSTATUS { get; set; }

        public string OPENBYUSER { get; set; }

        public DateTime? OPENDATE { get; set; }

        public string PASSPCTRY { get; set; }

        public string PASSPORTNO { get; set; }

        public DateTime? PAYDATE { get; set; }

        public DateTime? PAYTIME { get; set; }

        public long? PRICEINDEX { get; set; }

        public long? REGIONVALUE { get; set; }

        public string RESHIMONNO { get; set; }

        public string RESHIMONTYPE { get; set; }

        public DateTime? RESHMDATE { get; set; }

        public short? RIGHTOWNID { get; set; }

        public string SELLCONDITIONID { get; set; }

        public long? SERVICEVALUE { get; set; }

        public string STORAGEREQUEST { get; set; }

        public string STREET { get; set; }

        public long? TOTALTAX { get; set; }

        public string TRANCURRENCY { get; set; }

        public string TRANSIMPORTID { get; set; }

        public long? TRANSPVALFC { get; set; }

        public long? TRANSPVALUE { get; set; }

        public DateTime? TSHUMOTTAXPRINT { get; set; }

        public DateTime? WARNINGDATE { get; set; }

        public string ZIPCODE { get; set; }

        public string RESHIMONTYPEN { get; set; }

        public string RESHIMONNON { get; set; }

        public string COINIDN { get; set; }

        public string TRANCURRENCYN { get; set; }

        public string INSURANCECURRN { get; set; }

        public string CANCELLED { get; set; }

        public decimal? LOANAMOUNT { get; set; }

        public decimal? CURRENCYRATENEW { get; set; }

        public string DRAWNON { get; set; }

        public string PRATMEHESLIST { get; set; }

        public string ALLPRATMEHESLIST { get; set; }

        public int? NOOFINVOICES { get; set; }

        public int? TOTALINVOICELINESNO { get; set; }
        public int Tenant { get; set; }

    }
}
