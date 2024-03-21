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
    public class CCUCUSTITEMDataMapping : IMapping<SupplierInvoiceItem105PM, CCUCUSTITEM>
    {
        public void PMToPOCO(SupplierInvoiceItem105PM entityPM, CCUCUSTITEM entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.ABSAMOUNT = entityPM.ABSAMOUNT;
            entityPOCO.ACAMOUNT = entityPM.ACAMOUNT;
            entityPOCO.AGNTPAYBITHA = entityPM.AGNTPAYBITHA;
            entityPOCO.AGNTPAYCUST = entityPM.AGNTPAYCUST;
            entityPOCO.AGNTPAYTAX = entityPM.AGNTPAYTAX;
            entityPOCO.AIRBAGSAMOUNT = entityPM.AIRBAGSAMOUNT;
            entityPOCO.AUTONOMYBOOK = entityPM.AUTONOMYBOOK;
            entityPOCO.BITHATAXITEM = entityPM.BITHATAXITEM;
            entityPOCO.CURRENCYCODE = entityPM.CURRENCYCODE;
            entityPOCO.DISCOUNTCODE = entityPM.DISCOUNTCODE;
            entityPOCO.ESSENTIALITEM = entityPM.ESSENTIALITEM;
            entityPOCO.EXEMPTIONCODE = entityPM.EXEMPTIONCODE;
            entityPOCO.EXPPRAT = entityPM.EXPPRAT;
            entityPOCO.EXPRESHIMONNO = entityPM.EXPRESHIMONNO;
            entityPOCO.EXTRAQNTY = entityPM.EXTRAQNTY;
            entityPOCO.FOREIGNCURRVAL = entityPM.FOREIGNCURRVAL;
            entityPOCO.GOODSDESC = entityPM.GOODSDESC;
            entityPOCO.GUARANPERCENT = entityPM.GUARANPERCENT;
            entityPOCO.GUARANTEENO = entityPM.GUARANTEENO;
            entityPOCO.GUARANTEETYPE = entityPM.GUARANTEETYPE;
            entityPOCO.IMPORTADDITION = entityPM.IMPORTADDITION;
            entityPOCO.KATALOGNO = entityPM.KATALOGNO;
            entityPOCO.LICENSENO = entityPM.LICENSENO;
            entityPOCO.NIDHEMASPCNT = entityPM.NIDHEMASPCNT;
            entityPOCO.NIDHEMEHESPCNT = entityPM.NIDHEMEHESPCNT;
            entityPOCO.NISVALUE = entityPM.NISVALUE;
            entityPOCO.ORDERLINE = entityPM.ORDERLINE;
            entityPOCO.ORIGINCOUNTRY = entityPM.ORIGINCOUNTRY;
            entityPOCO.PRATMEHES = entityPM.PRATMEHES;
            entityPOCO.PRATMEHESCAN = entityPM.PRATMEHESCAN;
            entityPOCO.PRIVATEIMPCURR = entityPM.PRIVATEIMPCURR;
            entityPOCO.PURCHCOUNTRY = entityPM.PURCHCOUNTRY;
            entityPOCO.QUANTITY = entityPM.QUANTITY;
            entityPOCO.RAISEPERCENT = entityPM.RAISEPERCENT;
            entityPOCO.RAISEVALUE = entityPM.RAISEVALUE;
            entityPOCO.STANDARDNO = entityPM.STANDARDNO;
            entityPOCO.SUPPLIERACCOUNT = entityPM.SUPPLIERACCOUNT;
            entityPOCO.TARIFFCODE = entityPM.TARIFFCODE;
            entityPOCO.TSVIRA = entityPM.TSVIRA;
            entityPOCO.UNITID = entityPOCO.UNITID;
            entityPOCO.VEHICLECODE = entityPM.VEHICLECODE;
            entityPOCO.WHOLESALEPRICE = entityPM.WHOLESALEPRICE;
            entityPOCO.STSQNTY = entityPM.STSQNTY;
            entityPOCO.PRATMEHESN = entityPM.PRATMEHESN;
            entityPOCO.ORIGINCOUNTRYN = entityPM.ORIGINCOUNTRYN;
            entityPOCO.PURCHCOUNTRYN = entityPM.PURCHCOUNTRYN;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCHRONIZED;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(SupplierInvoiceItem105PM entityPM, CCUCUSTITEM entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.ABSAMOUNT = entityPOCO.ABSAMOUNT;
            entityPM.ACAMOUNT = entityPOCO.ACAMOUNT;
            entityPM.AGNTPAYBITHA = entityPOCO.AGNTPAYBITHA;
            entityPM.AGNTPAYCUST = entityPOCO.AGNTPAYCUST;
            entityPM.AGNTPAYTAX = entityPOCO.AGNTPAYTAX;
            entityPM.AIRBAGSAMOUNT = entityPOCO.AIRBAGSAMOUNT;
            entityPM.AUTONOMYBOOK = entityPOCO.AUTONOMYBOOK;
            entityPM.BITHATAXITEM = entityPOCO.BITHATAXITEM;
            entityPM.CURRENCYCODE = entityPOCO.CURRENCYCODE;
            entityPM.DISCOUNTCODE = entityPOCO.DISCOUNTCODE;
            entityPM.ESSENTIALITEM = entityPOCO.ESSENTIALITEM;
            entityPM.EXEMPTIONCODE = entityPOCO.EXEMPTIONCODE;
            entityPM.EXPPRAT = entityPOCO.EXPPRAT;
            entityPM.EXPRESHIMONNO = entityPOCO.EXPRESHIMONNO;
            entityPM.EXTRAQNTY = entityPOCO.EXTRAQNTY;
            entityPM.FOREIGNCURRVAL = entityPOCO.FOREIGNCURRVAL;
            entityPM.GOODSDESC = entityPOCO.GOODSDESC;
            entityPM.GUARANPERCENT = entityPOCO.GUARANPERCENT;
            entityPM.GUARANTEENO = entityPOCO.GUARANTEENO;
            entityPM.GUARANTEETYPE = entityPOCO.GUARANTEETYPE;
            entityPM.IMPORTADDITION = entityPOCO.IMPORTADDITION;
            entityPM.KATALOGNO = entityPOCO.KATALOGNO;
            entityPM.LICENSENO = entityPOCO.LICENSENO;
            entityPM.NIDHEMASPCNT = entityPOCO.NIDHEMASPCNT;
            entityPM.NIDHEMEHESPCNT = entityPOCO.NIDHEMEHESPCNT;
            entityPM.NISVALUE = entityPOCO.NISVALUE;
            entityPM.ORDERLINE = entityPOCO.ORDERLINE;
            entityPM.ORIGINCOUNTRY = entityPOCO.ORIGINCOUNTRY;
            entityPM.PRATMEHES = entityPOCO.PRATMEHES;
            entityPM.PRATMEHESCAN = entityPOCO.PRATMEHESCAN;
            entityPM.PRIVATEIMPCURR = entityPOCO.PRIVATEIMPCURR;
            entityPM.PURCHCOUNTRY = entityPOCO.PURCHCOUNTRY;
            entityPM.QUANTITY = entityPOCO.QUANTITY;
            entityPM.RAISEPERCENT = entityPOCO.RAISEPERCENT;
            entityPM.RAISEVALUE = entityPOCO.RAISEVALUE;
            entityPM.STANDARDNO = entityPOCO.STANDARDNO;
            entityPM.SUPPLIERACCOUNT = entityPOCO.SUPPLIERACCOUNT;
            entityPM.TARIFFCODE = entityPOCO.TARIFFCODE;
            entityPM.TSVIRA = entityPOCO.TSVIRA;
            entityPM.UNITID = entityPOCO.UNITID;
            entityPM.VEHICLECODE = entityPOCO.VEHICLECODE;
            entityPM.WHOLESALEPRICE = entityPOCO.WHOLESALEPRICE;
            entityPM.STSQNTY = entityPOCO.STSQNTY;
            entityPM.PRATMEHESN = entityPOCO.PRATMEHESN;
            entityPM.ORIGINCOUNTRYN = entityPOCO.ORIGINCOUNTRYN;
            entityPM.PURCHCOUNTRYN = entityPOCO.PURCHCOUNTRYN;
            entityPM.Tenant = entityPOCO.tenant != null ? (int)entityPOCO.tenant : 0;
            entityPM.IS_SYNCHRONIZED = entityPOCO.IS_SYNCHRONIZED != null ? (bool)entityPOCO.IS_SYNCHRONIZED : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;

        }

        public void CustomPMToPOCO(SupplierInvoiceItem105PM entityPM, CCUCUSTITEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(SupplierInvoiceItem105PM entityPM, CCUCUSTITEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(SupplierInvoiceItem105PM entityPM, SupplierInvoiceItem105PM oldEntityPM)
        {
//            throw new NotImplementedException();
        }
    }
}
