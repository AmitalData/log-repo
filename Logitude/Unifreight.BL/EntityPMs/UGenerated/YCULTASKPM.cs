using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class YCULTASKPM : EntityPM
    {

        public string TASKID { get; set; }

        public string ENTNAME { get; set; }

        public string PRIMARYNUM { get; set; }

        public string TYPE { get; set; }

        public DateTime? LOGTIME { get; set; }

        public short? PRIORITY { get; set; }

        public DateTime? PROCESSSTARTTIME { get; set; }

        public DateTime? PROCESSENDTIME { get; set; }

        public string ARCHIVE { get; set; }

        public string STATUS { get; set; }

        public string REQUESTDATA { get; set; }

        public string RESPONSE { get; set; }

        public string USRCODE { get; set; }

        public string CLIENTID { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCHRONIZED { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
 }
