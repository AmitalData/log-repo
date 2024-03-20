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
    public class CCUTAXDataMapping : IMapping<CCUTAXPM, CCUTAX>
    {
        public void PMToPOCO(CCUTAXPM entityPM, CCUTAX entityPOCO)
        {
            entityPOCO.ADDEFINEDTAX = entityPM.ADDEFINEDTAX;
            entityPOCO.ADDIMPORT = entityPM.ADDIMPORT;
            entityPOCO.ADDTAXRATE = entityPM.ADDTAXRATE;
            entityPOCO.DEFINEDTAX = entityPM.DEFINEDTAX;
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.GOODSNO = entityPM.GOODSNO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.POSTPONEDTAX = entityPM.POSTPONEDTAX;
            entityPOCO.PRATMEHES = entityPM.PRATMEHES;
            entityPOCO.TAXAMOUNT = entityPM.TAXAMOUNT;
            entityPOCO.TAXBASIS = entityPM.TAXBASIS;
            entityPOCO.TAXCALCCODE = entityPM.TAXCALCCODE;
            entityPOCO.TAXRATE = entityPM.TAXRATE;
            entityPOCO.TAXTOPAY = entityPM.TAXTOPAY;
            entityPOCO.TAXTYPE = entityPM.TAXTYPE;
            entityPOCO.PRATMEHESN = entityPM.PRATMEHESN;
            entityPOCO.TAXTYPEN = entityPM.TAXTYPEN;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCHRONIZED = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUTAXPM entityPM, CCUTAX entityPOCO)
        {
            entityPM.ADDEFINEDTAX = entityPOCO.ADDEFINEDTAX;
            entityPM.ADDIMPORT = entityPOCO.ADDIMPORT;
            entityPM.ADDTAXRATE = entityPOCO.ADDTAXRATE;
            entityPM.DEFINEDTAX = entityPOCO.DEFINEDTAX;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.GOODSNO = entityPOCO.GOODSNO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.POSTPONEDTAX = entityPOCO.POSTPONEDTAX;
            entityPM.PRATMEHES = entityPOCO.PRATMEHES;
            entityPM.TAXAMOUNT = entityPOCO.TAXAMOUNT;
            entityPM.TAXBASIS = entityPOCO.TAXBASIS;
            entityPM.TAXCALCCODE = entityPOCO.TAXCALCCODE;
            entityPM.TAXRATE = entityPOCO.TAXRATE;
            entityPM.TAXTOPAY = entityPOCO.TAXTOPAY;
            entityPM.TAXTYPE = entityPOCO.TAXTYPE;
            entityPM.PRATMEHESN = entityPOCO.PRATMEHESN;
            entityPM.TAXTYPEN = entityPOCO.TAXTYPEN;
            entityPM.Tenant = (int)entityPOCO.tenant;
            entityPM.IS_SYNCH = (bool)entityPOCO.IS_SYNCHRONIZED;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT;

        }

        public void CustomPMToPOCO(CCUTAXPM entityPM, CCUTAX entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUTAXPM entityPM, CCUTAX entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUTAXPM entityPM, CCUTAXPM oldEntityPM)
        {
            //throw new NotImplementedException();
        }
    }
}
