
using System;
using System.Collections.Generic;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class LFIFILEMDataMapping : IMapping<LFIFILEMPM, LFIFILEM>
    {
        public void PMToPOCO(LFIFILEMPM entityPM, LFIFILEM entityPOCO)
        {
            entityPOCO.DELIVERYNO = entityPM.DELIVERYNO;
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.PRIMARYNUM = entityPM.PRIMARYNUM;
            entityPOCO.DELIVERYTYPE = entityPM.DELIVERYTYPE;
            entityPOCO.CLIENTID = entityPM.CLIENTID;
            entityPOCO.CARRIERID = entityPM.CARRIERID;
            entityPOCO.CONTACTNAME = entityPM.CONTACTNAME;
            entityPOCO.PHONENO = entityPM.PHONENO;
            entityPOCO.EMAIL = entityPM.EMAIL;
            entityPOCO.CLIENTDEBIT = entityPM.CLIENTDEBIT;
            entityPOCO.WAREHOUSEFROM = entityPM.WAREHOUSEFROM;
            entityPOCO.WAREHOUSETO = entityPM.WAREHOUSETO;
            entityPOCO.ZONEFROM = entityPM.ZONEFROM;
            entityPOCO.ADDRESSFROM = entityPM.ADDRESSFROM;
            entityPOCO.ADDRESSTO = entityPM.ADDRESSTO;
            entityPOCO.LASTDATE = entityPM.LASTDATE;
            entityPOCO.PACKTYPEID = entityPM.PACKTYPEID;
            entityPOCO.QUANTITY = entityPM.QUANTITY;
            entityPOCO.WEIGHT = entityPM.WEIGHT;
            entityPOCO.VOLUME = entityPM.VOLUME;
            entityPOCO.CHARGWT = entityPM.CHARGWT;
            entityPOCO.DRIVERID = entityPM.DRIVERID;
            entityPOCO.RATIO = entityPM.RATIO;
            entityPOCO.FILECLOSE = entityPM.FILECLOSE;
            entityPOCO.TRUCKID = entityPM.TRUCKID;
            entityPOCO.TRUCKTYPE = entityPM.TRUCKTYPE;
            entityPOCO.STATUSID = entityPM.STATUSID;
            entityPOCO.FUCLOSE = entityPM.FUCLOSE;
            entityPOCO.ACCOUNTINGCLOSE = entityPM.ACCOUNTINGCLOSE;
            entityPOCO.STATUSDATE = entityPM.STATUSDATE;
            entityPOCO.LSTSTATUSID = entityPM.LSTSTATUSID;
            entityPOCO.LSTSTATUSDATE = entityPM.LSTSTATUSDATE;
            entityPOCO.FOLUPDATE = entityPM.FOLUPDATE;
            entityPOCO.OPENBYUSER = entityPM.OPENBYUSER;
            entityPOCO.BRANCHID = entityPM.BRANCHID;
            entityPOCO.OPENDATE = entityPM.OPENDATE;
            entityPOCO.DEPARTID = entityPM.DEPARTID;
            entityPOCO.LASTTIME = entityPM.LASTTIME;
            entityPOCO.LOADDATEFROM = entityPM.LOADDATEFROM;
            entityPOCO.LOADDATETO = entityPM.LOADDATETO;
            entityPOCO.LOADTIMEFROM = entityPM.LOADTIMEFROM;
            entityPOCO.LOADTIMETO = entityPM.LOADTIMETO;
            entityPOCO.DESTDATEFROM = entityPM.DESTDATEFROM;
            entityPOCO.DESTDATETO = entityPM.DESTDATETO;
            entityPOCO.DESTTIMEFROM = entityPM.DESTTIMEFROM;
            entityPOCO.DESTTIMETO = entityPM.DESTTIMETO;
            entityPOCO.CITYFROM = entityPM.CITYFROM;
            entityPOCO.CITYTO = entityPM.CITYTO;
            entityPOCO.CHARACTERS = entityPM.CHARACTERS;
            entityPOCO.CHARGWTCAR = entityPM.CHARGWTCAR;
            entityPOCO.RATIOCAR = entityPM.RATIOCAR;
            entityPOCO.PODDATE = entityPM.PODDATE;
            entityPOCO.PODTIME = entityPM.PODTIME;
            entityPOCO.REMARKS = entityPM.REMARKS;
            entityPOCO.RECEIVERNAME = entityPM.RECEIVERNAME;
            entityPOCO.CARGOTYPE = entityPM.CARGOTYPE;
            entityPOCO.COMMODITYID = entityPM.COMMODITYID;
            entityPOCO.CLASSNO = entityPM.CLASSNO;
            entityPOCO.UNNO = entityPM.UNNO;
            entityPOCO.PCKGROUP = entityPM.PCKGROUP;
            entityPOCO.DELIVERYREF = entityPM.DELIVERYREF;
            entityPOCO.CUSTOMSREF = entityPM.CUSTOMSREF;
            entityPOCO.TRASNPORTBY = entityPM.TRASNPORTBY;
            entityPOCO.FROMAREA = entityPM.FROMAREA;
            entityPOCO.FROMITUR = entityPM.FROMITUR;
            entityPOCO.FORWARDREF = entityPM.FORWARDREF;
            entityPOCO.DLVRYINPRGRS = entityPM.DLVRYINPRGRS;
            entityPOCO.CUSTOMSAGNTCODE = entityPM.CUSTOMSAGNTCODE;
            entityPOCO.GATEPASSNO = entityPM.GATEPASSNO;
            entityPOCO.ORIGINALMODEOFTRANSP = entityPM.ORIGINALMODEOFTRANSP;
            entityPOCO.DRIVERDOC = entityPM.DRIVERDOC;
            entityPOCO.TRACKNO = entityPM.TRACKNO;
            entityPOCO.TRACKADDNO = entityPM.TRACKADDNO;
            entityPOCO.ARRDATE = entityPM.ARRDATE;
            entityPOCO.ARRTIME = entityPM.ARRTIME;
            entityPOCO.INCTRANSPDET = entityPM.INCTRANSPDET;
            entityPOCO.WTVALCODE = entityPM.WTVALCODE;
            entityPOCO.SENDPORT = entityPM.SENDPORT;
            entityPOCO.QUOTE = entityPM.QUOTE;

        }

        public void POCOToPM(LFIFILEMPM entityPM, LFIFILEM entityPOCO)
        {
            entityPM.DELIVERYNO = entityPOCO.DELIVERYNO;
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.PRIMARYNUM = entityPOCO.PRIMARYNUM;
            entityPM.DELIVERYTYPE = entityPOCO.DELIVERYTYPE;
            entityPM.CLIENTID = entityPOCO.CLIENTID;
            entityPM.CARRIERID = entityPOCO.CARRIERID;
            entityPM.CONTACTNAME = entityPOCO.CONTACTNAME;
            entityPM.PHONENO = entityPOCO.PHONENO;
            entityPM.EMAIL = entityPOCO.EMAIL;
            entityPM.CLIENTDEBIT = entityPOCO.CLIENTDEBIT;
            entityPM.WAREHOUSEFROM = entityPOCO.WAREHOUSEFROM;
            entityPM.WAREHOUSETO = entityPOCO.WAREHOUSETO;
            entityPM.ZONEFROM = entityPOCO.ZONEFROM;
            entityPM.ADDRESSFROM = entityPOCO.ADDRESSFROM;
            entityPM.ADDRESSTO = entityPOCO.ADDRESSTO;
            entityPM.LASTDATE = entityPOCO.LASTDATE;
            entityPM.PACKTYPEID = entityPOCO.PACKTYPEID;
            entityPM.QUANTITY = entityPOCO.QUANTITY;
            entityPM.WEIGHT = entityPOCO.WEIGHT;
            entityPM.VOLUME = entityPOCO.VOLUME;
            entityPM.CHARGWT = entityPOCO.CHARGWT;
            entityPM.DRIVERID = entityPOCO.DRIVERID;
            entityPM.RATIO = entityPOCO.RATIO;
            entityPM.FILECLOSE = entityPOCO.FILECLOSE;
            entityPM.TRUCKID = entityPOCO.TRUCKID;
            entityPM.TRUCKTYPE = entityPOCO.TRUCKTYPE;
            entityPM.STATUSID = entityPOCO.STATUSID;
            entityPM.FUCLOSE = entityPOCO.FUCLOSE;
            entityPM.ACCOUNTINGCLOSE = entityPOCO.ACCOUNTINGCLOSE;
            entityPM.STATUSDATE = entityPOCO.STATUSDATE;
            entityPM.LSTSTATUSID = entityPOCO.LSTSTATUSID;
            entityPM.LSTSTATUSDATE = entityPOCO.LSTSTATUSDATE;
            entityPM.FOLUPDATE = entityPOCO.FOLUPDATE;
            entityPM.OPENBYUSER = entityPOCO.OPENBYUSER;
            entityPM.BRANCHID = entityPOCO.BRANCHID;
            entityPM.OPENDATE = entityPOCO.OPENDATE;
            entityPM.DEPARTID = entityPOCO.DEPARTID;
            entityPM.LASTTIME = entityPOCO.LASTTIME;
            entityPM.LOADDATEFROM = entityPOCO.LOADDATEFROM;
            entityPM.LOADDATETO = entityPOCO.LOADDATETO;
            entityPM.LOADTIMEFROM = entityPOCO.LOADTIMEFROM;
            entityPM.LOADTIMETO = entityPOCO.LOADTIMETO;
            entityPM.DESTDATEFROM = entityPOCO.DESTDATEFROM;
            entityPM.DESTDATETO = entityPOCO.DESTDATETO;
            entityPM.DESTTIMEFROM = entityPOCO.DESTTIMEFROM;
            entityPM.DESTTIMETO = entityPOCO.DESTTIMETO;
            entityPM.CITYFROM = entityPOCO.CITYFROM;
            entityPM.CITYTO = entityPOCO.CITYTO;
            entityPM.CHARACTERS = entityPOCO.CHARACTERS;
            entityPM.CHARGWTCAR = entityPOCO.CHARGWTCAR;
            entityPM.RATIOCAR = entityPOCO.RATIOCAR;
            entityPM.PODDATE = entityPOCO.PODDATE;
            entityPM.PODTIME = entityPOCO.PODTIME;
            entityPM.REMARKS = entityPOCO.REMARKS;
            entityPM.RECEIVERNAME = entityPOCO.RECEIVERNAME;
            entityPM.CARGOTYPE = entityPOCO.CARGOTYPE;
            entityPM.COMMODITYID = entityPOCO.COMMODITYID;
            entityPM.CLASSNO = entityPOCO.CLASSNO;
            entityPM.UNNO = entityPOCO.UNNO;
            entityPM.PCKGROUP = entityPOCO.PCKGROUP;
            entityPM.CUSTOMSREF = entityPOCO.CUSTOMSREF;
            entityPM.TRASNPORTBY = entityPOCO.TRASNPORTBY;
            entityPM.FROMAREA = entityPOCO.FROMAREA;
            entityPM.FROMITUR = entityPOCO.FROMITUR;
            entityPM.FORWARDREF = entityPOCO.FORWARDREF;
            entityPM.DLVRYINPRGRS = entityPOCO.DLVRYINPRGRS;
            entityPM.CUSTOMSAGNTCODE = entityPOCO.CUSTOMSAGNTCODE;
            entityPM.GATEPASSNO = entityPOCO.GATEPASSNO;
            entityPM.ORIGINALMODEOFTRANSP = entityPOCO.ORIGINALMODEOFTRANSP;
            entityPM.DRIVERDOC = entityPOCO.DRIVERDOC;
            entityPM.TRACKNO = entityPOCO.TRACKNO;
            entityPM.TRACKADDNO = entityPOCO.TRACKADDNO;
            entityPM.ARRDATE = entityPOCO.ARRDATE;
            entityPM.ARRTIME = entityPOCO.ARRTIME;
            entityPM.INCTRANSPDET = entityPOCO.INCTRANSPDET;
            entityPM.WTVALCODE = entityPOCO.WTVALCODE;
            entityPM.SENDPORT = entityPOCO.SENDPORT;
            entityPM.QUOTE = entityPOCO.QUOTE;

        }

        public void CustomPMToPOCO(LFIFILEMPM entityPM, LFIFILEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(LFIFILEMPM entityPM, LFIFILEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(LFIFILEMPM entityPM, LFIFILEMPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
