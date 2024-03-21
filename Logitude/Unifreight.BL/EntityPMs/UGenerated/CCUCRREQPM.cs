using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUCRREQPM : EntityPM
    {
        public string ENTNAME { get; set; }

        public int FILENO { get; set; }

        public int ACCLINENO { get; set; }

        public int ITEMLINE { get; set; }

        public int LINENO { get; set; }

        public string APPROVCODE { get; set; }

        public string APPROVTYPE { get; set; }

        public string CERTIFICATENO { get; set; }

        public string FENTNAME { get; set; }

        public bool? GCRCRTFCLOSE { get; set; }

        public int? GCRCRTFID { get; set; }

        public string ITEMNO { get; set; }

        public string PRATMEHES { get; set; }

        public string REMARKS { get; set; }

        public string REQCERTID { get; set; }

        public string SINUMBER { get; set; }

        public string SUPPLIERCOUNTRY { get; set; }

        public string SUPPLIERID { get; set; }

        public string REQUESTNO { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}