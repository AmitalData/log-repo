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
    public class CCUACCSUPDataMapping : IMapping<SupplierInvoicePM, CCUACCSUP>
    {
        public void PMToPOCO(SupplierInvoicePM entityPM, CCUACCSUP entityPOCO)
        {
            entityPOCO.ACCOUNTTYPE = entityPM.ACCOUNTTYPE;
            entityPOCO.CHANGINGVALUE = entityPM.CHANGINGVALUE;
            entityPOCO.COMMISSION = entityPM.COMMISSION;
            entityPOCO.COUNTRYID = entityPM.COUNTRYID;
            entityPOCO.CURRENCYID = entityPM.CURRENCYID;
            entityPOCO.DECLARATIONNO = entityPM.DECLARATIONNO;
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.INCOTERMID = entityPM.INCOTERMID;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.MAINACCOUNT = entityPM.MAINACCOUNT;
            entityPOCO.SUPPLIERACCOUNT = entityPM.SUPPLIERACCOUNT;
            entityPOCO.SUPPLIERID = entityPM.SUPPLIERID;
            entityPOCO.VALUE = entityPM.VALUE;
            entityPOCO.SUPPLIERACCOUNTN = entityPM.SUPPLIERACCOUNTN;
            entityPOCO.COMMISSIONPERCENT = entityPM.COMMISSIONPERCENT;
            entityPOCO.INCOTERMIDN = entityPM.INCOTERMIDN;
            entityPOCO.COUNTRYIDN = entityPM.COUNTRYIDN;
            entityPOCO.CURRENCYIDN = entityPM.CURRENCYIDN;
            entityPOCO.TENANT = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(SupplierInvoicePM entityPM, CCUACCSUP entityPOCO)
        {
            entityPM.ACCOUNTTYPE = entityPOCO.ACCOUNTTYPE;
            entityPM.CHANGINGVALUE = entityPOCO.CHANGINGVALUE;
            entityPM.COMMISSION = entityPOCO.COMMISSION;
            entityPM.COUNTRYID = entityPOCO.COUNTRYID;
            entityPM.CURRENCYID = entityPOCO.CURRENCYID;
            entityPM.DECLARATIONNO = entityPOCO.DECLARATIONNO;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.INCOTERMID = entityPOCO.INCOTERMID;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.MAINACCOUNT = entityPOCO.MAINACCOUNT;
            entityPM.SUPPLIERACCOUNT = entityPOCO.SUPPLIERACCOUNT;
            entityPM.SUPPLIERID = entityPOCO.SUPPLIERID;
            entityPM.VALUE = entityPOCO.VALUE;
            entityPM.SUPPLIERACCOUNTN = entityPOCO.SUPPLIERACCOUNTN;
            entityPM.COMMISSIONPERCENT = entityPOCO.COMMISSIONPERCENT;
            entityPM.INCOTERMIDN = entityPOCO.INCOTERMIDN;
            entityPM.COUNTRYIDN = entityPOCO.COUNTRYIDN;
            entityPM.CURRENCYIDN = entityPOCO.CURRENCYIDN;
            entityPM.Tenant = entityPOCO.TENANT != null ? (int)entityPOCO.TENANT : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(SupplierInvoicePM entityPM, CCUACCSUP entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(SupplierInvoicePM entityPM, CCUACCSUP entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(SupplierInvoicePM entityPM, SupplierInvoicePM oldEntityPM)
        {
        //    throw new NotImplementedException();
        }
    }
}
