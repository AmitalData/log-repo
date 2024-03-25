using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
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
    public class CCUMSHGRDataMapping : IMapping<CCUMSHGRPM, CCUMSHGR>
    {
        public void PMToPOCO(CCUMSHGRPM entityPM, CCUMSHGR entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.ADDQUANTITY = entityPM.ADDQUANTITY;
            entityPOCO.CARNETNUMBER = entityPM.CARNETNUMBER;
            entityPOCO.CARRIERID = entityPM.CARRIERID;
            entityPOCO.DESCOFGOODS1 = entityPM.DESCOFGOODS1;
            entityPOCO.DESCOFGOODS2 = entityPM.DESCOFGOODS2;
            entityPOCO.DESCOFGOODS3 = entityPM.DESCOFGOODS3;
            entityPOCO.EXPORTLAND = entityPM.EXPORTLAND;
            entityPOCO.HAWB = entityPM.HAWB;
            entityPOCO.HAWBDATE = entityPM.HAWBDATE;
            entityPOCO.IDENTIFIERNO = entityPM.IDENTIFIERNO;
            entityPOCO.IDENTIFIERTYPE = entityPM.IDENTIFIERTYPE;
            entityPOCO.LOADPORTID = entityPM.LOADPORTID;
            entityPOCO.MANIFESTNO = entityPM.MANIFESTNO;
            entityPOCO.MISHGORNO = entityPM.MISHGORNO;
            entityPOCO.MISHGORTYPE = entityPM.MISHGORTYPE;
            entityPOCO.PACKDET = entityPM.PACKDET;
            entityPOCO.PACKTYPEID = entityPM.PACKTYPEID;
            entityPOCO.PARTIALITYID = entityPM.PARTIALITYID;
            FieldDbVaildUtil.Validate(entityPM.QUANTITY, 8, "CCUMSHGR.QUANTITY");
            entityPOCO.QUANTITY = entityPM.QUANTITY;
            entityPOCO.SEALQTY = entityPM.SEALQTY;
            entityPOCO.STORAGESITE = entityPM.STORAGESITE;
            entityPOCO.TRANSPTYPE = entityPM.TRANSPTYPE;
            entityPOCO.UNLOADDATE = entityPM.UNLOADDATE;
            entityPOCO.UNLOADPORTID = entityPM.UNLOADPORTID;
            entityPOCO.WAREHOUSEID = entityPM.WAREHOUSEID;
            entityPOCO.WAREHOUSEREC = entityPM.WAREHOUSEREC;
            entityPOCO.WEIGHT = entityPM.WEIGHT;
            entityPOCO.WAREHOUSEIDN = entityPM.WAREHOUSEIDN;
            entityPOCO.WAREHOUSERECN = entityPM.WAREHOUSERECN;
            entityPOCO.EXPORTLANDN = entityPM.EXPORTLANDN;
            entityPOCO.PACKTYPEIDN = entityPM.PACKTYPEIDN;
            entityPOCO.HAWBN = entityPM.HAWBN;
            entityPOCO.IDENTIFIERTYPEN = entityPM.IDENTIFIERTYPEN;
            entityPOCO.FIRSTCARGOID = entityPM.FIRSTCARGOID;
            entityPOCO.SECONDCARGOID = entityPM.SECONDCARGOID;
            entityPOCO.tenant = entityPM.Tenant;
            entityPOCO.IS_SYNCH = entityPM.IS_SYNCH;
            entityPOCO.LAST_UPDATE_DT = entityPM.LAST_UPDATE_DT;
        }

        public void POCOToPM(CCUMSHGRPM entityPM, CCUMSHGR entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.ADDQUANTITY = entityPOCO.ADDQUANTITY;
            entityPM.CARNETNUMBER = entityPOCO.CARNETNUMBER;
            entityPM.CARRIERID = entityPOCO.CARRIERID;
            entityPM.DESCOFGOODS1 = entityPOCO.DESCOFGOODS1;
            entityPM.DESCOFGOODS2 = entityPOCO.DESCOFGOODS2;
            entityPM.DESCOFGOODS3 = entityPOCO.DESCOFGOODS3;
            entityPM.EXPORTLAND = entityPOCO.EXPORTLAND;
            entityPM.HAWB = entityPOCO.HAWB;
            entityPM.HAWBDATE = entityPOCO.HAWBDATE;
            entityPM.IDENTIFIERNO = entityPOCO.IDENTIFIERNO;
            entityPM.IDENTIFIERTYPE = entityPOCO.IDENTIFIERTYPE;
            entityPM.LOADPORTID = entityPOCO.LOADPORTID;
            entityPM.MANIFESTNO = entityPOCO.MANIFESTNO;
            entityPM.MISHGORNO = entityPOCO.MISHGORNO;
            entityPM.MISHGORTYPE = entityPOCO.MISHGORTYPE;
            entityPM.PACKDET = entityPOCO.PACKDET;
            entityPM.PACKTYPEID = entityPOCO.PACKTYPEID;
            entityPM.PARTIALITYID = entityPOCO.PARTIALITYID;
            entityPM.QUANTITY = entityPOCO.QUANTITY;
            entityPM.SEALQTY = entityPOCO.SEALQTY;
            entityPM.STORAGESITE = entityPOCO.STORAGESITE;
            entityPM.TRANSPTYPE = entityPOCO.TRANSPTYPE;
            entityPM.UNLOADDATE = entityPOCO.UNLOADDATE;
            entityPM.UNLOADPORTID = entityPOCO.UNLOADPORTID;
            entityPM.WAREHOUSEID = entityPOCO.WAREHOUSEID;
            entityPM.WAREHOUSEREC = entityPOCO.WAREHOUSEREC;
            entityPM.WEIGHT = entityPOCO.WEIGHT;
            entityPM.WAREHOUSEIDN = entityPOCO.WAREHOUSEIDN;
            entityPM.WAREHOUSERECN = entityPOCO.WAREHOUSERECN;
            entityPM.EXPORTLANDN = entityPOCO.EXPORTLANDN;
            entityPM.PACKTYPEIDN = entityPOCO.PACKTYPEIDN;
            entityPM.HAWBN = entityPOCO.HAWBN;
            entityPM.IDENTIFIERTYPEN = entityPOCO.IDENTIFIERTYPEN;
            entityPM.FIRSTCARGOID = entityPOCO.FIRSTCARGOID;
            entityPM.SECONDCARGOID = entityPOCO.SECONDCARGOID;
            entityPM.Tenant = entityPOCO.tenant != null ? (int)entityPOCO.tenant : 0;
            entityPM.IS_SYNCH = entityPOCO.IS_SYNCH != null ? (bool)entityPOCO.IS_SYNCH : false;
            entityPM.LAST_UPDATE_DT = entityPOCO.LAST_UPDATE_DT != null ? entityPOCO.LAST_UPDATE_DT : DateTime.Now;
        }

        public void CustomPMToPOCO(CCUMSHGRPM entityPM, CCUMSHGR entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUMSHGRPM entityPM, CCUMSHGR entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUMSHGRPM entityPM, CCUMSHGRPM oldEntityPM)
        {
        //    throw new NotImplementedException();
        }
    }
}
