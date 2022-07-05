using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class LFIFILEMPM : EntityPM
    {
        public int DELIVERYNO { get; set; }
        public string ENTNAME { get; set; }
        public string PRIMARYNUM { get; set; }
        public string DELIVERYTYPE { get; set; }
        public string CLIENTID { get; set; }
        public string CARRIERID { get; set; }
        public string CONTACTNAME { get; set; }
        public string PHONENO { get; set; }
        public string EMAIL { get; set; }
        public string CLIENTDEBIT { get; set; }
        public string WAREHOUSEFROM { get; set; }
        public string WAREHOUSETO { get; set; }
        public string ZONEFROM { get; set; }
        public string ADDRESSFROM { get; set; }
        public string ADDRESSTO { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string PACKTYPEID { get; set; }
        public long? QUANTITY { get; set; }
        public double? WEIGHT { get; set; }
        public double? VOLUME { get; set; }
        public double? CHARGWT { get; set; }
        public string DRIVERID { get; set; }
        public string RATIO { get; set; }
        public bool? FILECLOSE { get; set; }
        public string TRUCKID { get; set; }
        public string TRUCKTYPE { get; set; }
        public string STATUSID { get; set; }
        public bool? FUCLOSE { get; set; }
        public bool? ACCOUNTINGCLOSE { get; set; }
        public DateTime? STATUSDATE { get; set; }
        public string LSTSTATUSID { get; set; }
        public DateTime? LSTSTATUSDATE { get; set; }
        public DateTime? FOLUPDATE { get; set; }
        public string OPENBYUSER { get; set; }
        public string BRANCHID { get; set; }
        public DateTime? OPENDATE { get; set; }
        public string DEPARTID { get; set; }
        public DateTime? LASTTIME { get; set; }
        public DateTime? LOADDATEFROM { get; set; }
        public DateTime? LOADDATETO { get; set; }
        public DateTime? LOADTIMEFROM { get; set; }
        public DateTime? LOADTIMETO { get; set; }
        public DateTime? DESTDATEFROM { get; set; }
        public DateTime? DESTDATETO { get; set; }
        public DateTime? DESTTIMEFROM { get; set; }
        public DateTime? DESTTIMETO { get; set; }
        public string CITYFROM { get; set; }
        public string CITYTO { get; set; }
        public string CHARACTERS { get; set; }
        public double? CHARGWTCAR { get; set; }
        public string RATIOCAR { get; set; }
        public DateTime? PODDATE { get; set; }
        public DateTime? PODTIME { get; set; }
        public string REMARKS { get; set; }
        public string RECEIVERNAME { get; set; }
        public string CARGOTYPE { get; set; }
        public string COMMODITYID { get; set; }
        public string CLASSNO { get; set; }
        public string UNNO { get; set; }
        public string PCKGROUP { get; set; }
        public string DELIVERYREF { get; set; }
        public string CUSTOMSREF { get; set; }
        public string TRASNPORTBY { get; set; }
        public string FROMAREA { get; set; }
        public string FROMITUR { get; set; }
        public string FORWARDREF { get; set; }
        public string DLVRYINPRGRS { get; set; }
        public string CUSTOMSAGNTCODE { get; set; }
        public string GATEPASSNO { get; set; }
        public string ORIGINALMODEOFTRANSP { get; set; }
        public string DRIVERDOC { get; set; }
        public string TRACKNO { get; set; }
        public string TRACKADDNO { get; set; }
        public DateTime? ARRDATE { get; set; }
        public DateTime? ARRTIME { get; set; }
        public string INCTRANSPDET { get; set; }
        public string WTVALCODE { get; set; }
        public string SENDPORT { get; set; }
        public string QUOTE { get; set; }


    }
}
