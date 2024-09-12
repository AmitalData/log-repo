using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class CCUFILEMDataMapping : IMapping<CCUFILEMPM, CCUFILEM>
    {
        public void PMToPOCO(CCUFILEMPM entityPM, CCUFILEM entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.ACCEPTEDPRICE = entityPM.ACCEPTEDPRICE;
            entityPOCO.APARTMENTNO = entityPM.APARTMENTNO;
            entityPOCO.AUTONOMY = entityPM.AUTONOMY;
            entityPOCO.BRANCHID = entityPM.BRANCHID;
            entityPOCO.CHANGE = entityPM.CHANGE;
            entityPOCO.CHANGINGVALUE = entityPM.CHANGINGVALUE;
            entityPOCO.CHARGESYSPRINT = entityPM.CHARGESYSPRINT;
            entityPOCO.CIFVALUE = entityPM.CIFVALUE;
            entityPOCO.CITYSYMBOL = entityPM.CITYSYMBOL;
            entityPOCO.CLOSUREVALUE = entityPM.CLOSUREVALUE;
            entityPOCO.COINID = entityPM.COINID;
            entityPOCO.CURRENCYRATE = entityPM.CURRENCYRATE;
            entityPOCO.CUSTOMAGENT = entityPM.CUSTOMAGENT;
            entityPOCO.CUSTOMAGENTPRINT = entityPM.CUSTOMAGENTPRINT;
            entityPOCO.CUSTOMERID = entityPM.CUSTOMERID;
            entityPOCO.CUSTOMFILENO = entityPM.CUSTOMFILENO;
            entityPOCO.CUSTOMPRINT = entityPM.CUSTOMPRINT;
            entityPOCO.CUSTOMSBRANCH = entityPM.CUSTOMSBRANCH;
            entityPOCO.DEPARTID = entityPM.DEPARTID;
            entityPOCO.DRAFTDATE = entityPM.DRAFTDATE;
            entityPOCO.DRAWNO = entityPM.DRAWNO;
            entityPOCO.ENTRANCE = entityPM.ENTRANCE;
            entityPOCO.EXPENSEVALUE = entityPM.EXPENSEVALUE;
            entityPOCO.FAMILYNAME = entityPM.FAMILYNAME;
            entityPOCO.FEECARRIER = entityPM.FEECARRIER;
            entityPOCO.FEEPLATFORM = entityPM.FEEPLATFORM;
            entityPOCO.FILECLOSE = entityPM.FILECLOSE;
            entityPOCO.FIRSTNAME = entityPM.FIRSTNAME;
            entityPOCO.FOLUPDATE = entityPM.FOLUPDATE;
            entityPOCO.FROMIIG = entityPM.FROMIIG;
            entityPOCO.GOODSVALUE = entityPM.GOODSVALUE;
            entityPOCO.GRANTDATE = entityPM.GRANTDATE;
            entityPOCO.GRANTTIME = entityPM.GRANTTIME;
            entityPOCO.GRNTTYPEID = entityPM.GRNTTYPEID;
            entityPOCO.GUARANTEEAMNT = entityPM.GUARANTEEAMNT;
            entityPOCO.HOUSE = entityPM.HOUSE;
            entityPOCO.IMPORTERID = entityPM.IMPORTERID;
            entityPOCO.IMPORTERSTS = entityPM.IMPORTERSTS;
            entityPOCO.IMPORTTYPE = entityPM.IMPORTTYPE;
            entityPOCO.INDEXVALUE = entityPM.INDEXVALUE;
            entityPOCO.INDICATORS = entityPM.INDICATORS;
            entityPOCO.INSURANCEAMNT = entityPM.INSURANCEAMNT;
            entityPOCO.INSURANCECURR = entityPM.INSURANCECURR;
            entityPOCO.INSURANCEPERCENT = entityPM.INSURANCEPERCENT;
            entityPOCO.INSURANCEVALUE = entityPM.INSURANCEVALUE;
            entityPOCO.MEHESDRAFTSTATUS = entityPM.MEHESDRAFTSTATUS;
            entityPOCO.OPENBYUSER = entityPM.OPENBYUSER;
            entityPOCO.OPENDATE = entityPM.OPENDATE;
            entityPOCO.PASSPCTRY = entityPM.PASSPCTRY;
            entityPOCO.PASSPORTNO = entityPM.PASSPORTNO;
            entityPOCO.PAYDATE = entityPM.PAYDATE;
            entityPOCO.PAYTIME = entityPM.PAYTIME;
            entityPOCO.PRICEINDEX = entityPM.PRICEINDEX;
            entityPOCO.REGIONVALUE = entityPM.REGIONVALUE;
            entityPOCO.RESHIMONNO = entityPM.RESHIMONNO;
            entityPOCO.RESHIMONTYPE = entityPM.RESHIMONTYPE;
            entityPOCO.RESHMDATE = entityPM.RESHMDATE;
            entityPOCO.RIGHTOWNID = entityPM.RIGHTOWNID;
            entityPOCO.SELLCONDITIONID = entityPM.SELLCONDITIONID;
            entityPOCO.SERVICEVALUE = entityPM.SERVICEVALUE;
            entityPOCO.STORAGEREQUEST = entityPM.STORAGEREQUEST;
            entityPOCO.STREET = entityPM.STREET;
            entityPOCO.TOTALTAX = entityPM.TOTALTAX;
            entityPOCO.TRANCURRENCY = entityPM.TRANCURRENCY;
            entityPOCO.TRANSIMPORTID = entityPM.TRANSIMPORTID;
            entityPOCO.TRANSPVALFC = entityPM.TRANSPVALFC;
            entityPOCO.TRANSPVALUE = entityPM.TRANSPVALUE;
            entityPOCO.TSHUMOTTAXPRINT = entityPM.TSHUMOTTAXPRINT;
            entityPOCO.WARNINGDATE = entityPM.WARNINGDATE;
            entityPOCO.ZIPCODE = entityPM.ZIPCODE;
            entityPOCO.RESHIMONTYPEN = entityPM.RESHIMONTYPEN;
            entityPOCO.RESHIMONNON = entityPM.RESHIMONNON;
            entityPOCO.COINIDN = entityPM.COINIDN;
            entityPOCO.TRANCURRENCYN = entityPM.TRANCURRENCYN;
            entityPOCO.INSURANCECURRN = entityPM.INSURANCECURRN;
            entityPOCO.CANCELLED = entityPM.CANCELLED;
            entityPOCO.LOANAMOUNT = entityPM.LOANAMOUNT;
            entityPOCO.CURRENCYRATENEW = entityPM.CURRENCYRATENEW;
            entityPOCO.DRAWNON = entityPM.DRAWNON;
            entityPOCO.PRATMEHESLIST = entityPM.PRATMEHESLIST;
            entityPOCO.ALLPRATMEHESLIST = entityPM.ALLPRATMEHESLIST;
            entityPOCO.NOOFINVOICES = entityPM.NOOFINVOICES;
            entityPOCO.TOTALINVOICELINESNO = entityPM.TOTALINVOICELINESNO;
            entityPOCO.TENANT = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;

        }

        public void POCOToPM(CCUFILEMPM entityPM, CCUFILEM entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.ACCEPTEDPRICE = entityPOCO.ACCEPTEDPRICE;
            entityPM.APARTMENTNO = entityPOCO.APARTMENTNO;
            entityPM.AUTONOMY = entityPOCO.AUTONOMY;
            entityPM.BRANCHID = entityPOCO.BRANCHID;
            entityPM.CHANGE = entityPOCO.CHANGE;
            entityPM.CHANGINGVALUE = entityPOCO.CHANGINGVALUE;
            entityPM.CHARGESYSPRINT = entityPOCO.CHARGESYSPRINT;
            entityPM.CIFVALUE = entityPOCO.CIFVALUE;
            entityPM.CITYSYMBOL = entityPOCO.CITYSYMBOL;
            entityPM.CLOSUREVALUE = entityPOCO.CLOSUREVALUE;
            entityPM.COINID = entityPOCO.COINID;
            entityPM.CURRENCYRATE = entityPOCO.CURRENCYRATE;
            entityPM.CUSTOMAGENT = entityPOCO.CUSTOMAGENT;
            entityPM.CUSTOMAGENTPRINT = entityPOCO.CUSTOMAGENTPRINT;
            entityPM.CUSTOMERID = entityPOCO.CUSTOMERID;
            entityPM.CUSTOMFILENO = entityPOCO.CUSTOMFILENO;
            entityPM.CUSTOMPRINT = entityPOCO.CUSTOMPRINT;
            entityPM.CUSTOMSBRANCH = entityPOCO.CUSTOMSBRANCH;
            entityPM.DEPARTID = entityPOCO.DEPARTID;
            entityPM.DRAFTDATE = entityPOCO.DRAFTDATE;
            entityPM.DRAWNO = entityPOCO.DRAWNO;
            entityPM.ENTRANCE = entityPOCO.ENTRANCE;
            entityPM.EXPENSEVALUE = entityPOCO.EXPENSEVALUE;
            entityPM.FAMILYNAME = entityPOCO.FAMILYNAME;
            entityPM.FEECARRIER = entityPOCO.FEECARRIER;
            entityPM.FEEPLATFORM = entityPOCO.FEEPLATFORM;
            entityPM.FILECLOSE = entityPOCO.FILECLOSE;
            entityPM.FIRSTNAME = entityPOCO.FIRSTNAME;
            entityPM.FOLUPDATE = entityPOCO.FOLUPDATE;
            entityPM.FROMIIG = entityPOCO.FROMIIG;
            entityPM.GOODSVALUE = entityPOCO.GOODSVALUE;
            entityPM.GRANTDATE = entityPOCO.GRANTDATE;
            entityPM.GRANTTIME = entityPOCO.GRANTTIME;
            entityPM.GRNTTYPEID = entityPOCO.GRNTTYPEID;
            entityPM.GUARANTEEAMNT = entityPOCO.GUARANTEEAMNT;
            entityPM.HOUSE = entityPOCO.HOUSE;
            entityPM.IMPORTERID = entityPOCO.IMPORTERID;
            entityPM.IMPORTERSTS = entityPOCO.IMPORTERSTS;
            entityPM.IMPORTTYPE = entityPOCO.IMPORTTYPE;
            entityPM.INDEXVALUE = entityPOCO.INDEXVALUE;
            entityPM.INDICATORS = entityPOCO.INDICATORS;
            entityPM.INSURANCEAMNT = entityPOCO.INSURANCEAMNT;
            entityPM.INSURANCECURR = entityPOCO.INSURANCECURR;
            entityPM.INSURANCEPERCENT = entityPOCO.INSURANCEPERCENT;
            entityPM.INSURANCEVALUE = entityPOCO.INSURANCEVALUE;
            entityPM.MEHESDRAFTSTATUS = entityPOCO.MEHESDRAFTSTATUS;
            entityPM.OPENBYUSER = entityPOCO.OPENBYUSER;
            entityPM.OPENDATE = entityPOCO.OPENDATE;
            entityPM.PASSPCTRY = entityPOCO.PASSPCTRY;
            entityPM.PASSPORTNO = entityPOCO.PASSPORTNO;
            entityPM.PAYDATE = entityPOCO.PAYDATE;
            entityPM.PAYTIME = entityPOCO.PAYTIME;
            entityPM.PRICEINDEX = entityPOCO.PRICEINDEX;
            entityPM.REGIONVALUE = entityPOCO.REGIONVALUE;
            entityPM.RESHIMONNO = entityPOCO.RESHIMONNO;
            entityPM.RESHIMONTYPE = entityPOCO.RESHIMONTYPE;
            entityPM.RESHMDATE = entityPOCO.RESHMDATE;
            entityPM.RIGHTOWNID = entityPOCO.RIGHTOWNID;
            entityPM.SELLCONDITIONID = entityPOCO.SELLCONDITIONID;
            entityPM.SERVICEVALUE = entityPOCO.SERVICEVALUE;
            entityPM.STORAGEREQUEST = entityPOCO.STORAGEREQUEST;
            entityPM.STREET = entityPOCO.STREET;
            entityPM.TOTALTAX = entityPOCO.TOTALTAX;
            entityPM.TRANCURRENCY = entityPOCO.TRANCURRENCY;
            entityPM.TRANSIMPORTID = entityPOCO.TRANSIMPORTID;
            entityPM.TRANSPVALFC = entityPOCO.TRANSPVALFC;
            entityPM.TRANSPVALUE = entityPOCO.TRANSPVALUE;
            entityPM.TSHUMOTTAXPRINT = entityPOCO.TSHUMOTTAXPRINT;
            entityPM.WARNINGDATE = entityPOCO.WARNINGDATE;
            entityPM.ZIPCODE = entityPOCO.ZIPCODE;
            entityPM.RESHIMONTYPEN = entityPOCO.RESHIMONTYPEN;
            entityPM.RESHIMONNON = entityPOCO.RESHIMONNON;
            entityPM.COINIDN = entityPOCO.COINIDN;
            entityPM.TRANCURRENCYN = entityPOCO.TRANCURRENCYN;
            entityPM.INSURANCECURRN = entityPOCO.INSURANCECURRN;
            entityPM.CANCELLED = entityPOCO.CANCELLED;
            entityPM.LOANAMOUNT = entityPOCO.LOANAMOUNT;
            entityPM.CURRENCYRATENEW = entityPOCO.CURRENCYRATENEW;
            entityPM.DRAWNON = entityPOCO.DRAWNON;
            entityPM.PRATMEHESLIST = entityPOCO.PRATMEHESLIST;
            entityPM.ALLPRATMEHESLIST = entityPOCO.ALLPRATMEHESLIST;
            entityPM.NOOFINVOICES = entityPOCO.NOOFINVOICES;
            entityPM.TOTALINVOICELINESNO = entityPOCO.TOTALINVOICELINESNO;
            entityPM.Tenant = entityPOCO.TENANT != null ? (int)entityPOCO.TENANT : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT !=null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(CCUFILEMPM entityPM, CCUFILEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUFILEMPM entityPM, CCUFILEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUFILEMPM entityPM, CCUFILEMPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
