using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUMESSAGEPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public string GROUPNO { get; set; }

        public string GROUPKEY { get; set; }

        public string REFERENCE { get; set; }

        public string MESSAGENO { get; set; }

        public string APPROVCODEID { get; set; }

        public string APPROVTYPEID { get; set; }

        public string APPROVNO { get; set; }

        public string ADDITIONID { get; set; }

        public string GENERAL { get; set; }

        public string APPROVELEVEL { get; set; }

        public string MESSAGETXT { get; set; }
        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
