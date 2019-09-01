using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial  class GGGQPM : EntityPM
    {
        private List<YCULTASKPM> tasks;

#if false
        #region Properties

        /// <summary>
        /// There are no comments for QUEID in the schema.
        /// </summary>

        public string QUEID { get; set; }

        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public global::System.DateTime EXECDATE { get; set; }
        public global::System.Nullable<System.DateTime> STARTDATE { get; set; }

        public string ORIGINQUE { get; set; }

        public string USERID { get; set; }

        public string REF { get; set; }
        public string DEPENDENCYREF { get; set; }
        public string DONEOPERATION { get; set; }
        public string STATUS { get; set; }
        public string DESC { get; set; }
        public global::System.Nullable<double> EXPTASKTIME { get; set; }
        public global::System.Nullable<int> TRY { get; set; }
        public global::System.Nullable<int> PRIORITY { get; set; }

        public string ENTNAME { get; set; }

        public string PRIMARYNUM { get; set; }
        public string LOGLEVEL { get; set; }

        public string SECNUMBER { get; set; }
        public global::System.Nullable<long> PROCESSID { get; set; }

        public string FORMID { get; set; }

        public string COMPUTERID { get; set; }
        public string GSTRING1 { get; set; }

        public string GSTRING2 { get; set; }
        public string GSTRING3 { get; set; }
        public string GSTRING4 { get; set; }
        public global::System.Nullable<bool> QUEUEMANAGEMENT { get; set; }
        public global::System.Nullable<bool> OTHERASNFILE { get; set; }
        public global::System.Nullable<bool> STOPPEDBYSM { get; set; }
        public string GSTRING5 { get; set; }

        public string DEBUG { get; set; }
        public string WEAKREF { get; set; }
        public string HUGERECORD { get; set; }
        public string POSTFAILED { get; set; }
        public string GSTRING6 { get; set; }
        public string POSTSUCCESS { get; set; }
        public global::System.Nullable<int> FAILED { get; set; }


        
        public virtual List<YCULTASKPM> Tasks
        {
            get
            {
                if (tasks == null)
                {
                    tasks = new List<YCULTASKPM>();
                }
                return tasks;
            }
            set { tasks = value; }
        }

        #endregion


        
#endif

        public string QUEID { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime EXECDATE { get; set; }

        public string ORIGINQUE { get; set; }

        public string STATUS { get; set; }

        public double? EXPTASKTIME { get; set; }

        public int? TRY { get; set; }

        public int? PRIORITY { get; set; }

        public string ENTNAME { get; set; }

        public string PRIMARYNUM { get; set; }

        public string FORMID { get; set; }

        public string DEBUG { get; set; }

        public string DONEOPERATION { get; set; }

        public string GSTRING1 { get; set; }
        public string GSTRING2 { get; set; }
        public string GSTRING3 { get; set; }
        public bool? QUEUEMANAGEMENT { get; set; }

        
    }
}
