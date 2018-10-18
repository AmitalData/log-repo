using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.DCA
{
    public class LastAccessFileInDCADirM
    {
        public LastAccessFileInDCADirM(int Tenant)
        {
            this.Tenant = Tenant;
            ObjectCreatedAt = DateTime.Now;
            LastAllXmlFileInMyBranch = new List<string>();
        }
        public int Tenant { get; private set; }
        public DateTime ObjectCreatedAt { get; private set; }

        public bool ErrorOccurred { get; set; }
        public DateTime? LastAccessFileInDCADir { get; set; }

        public DateTime? Dir1stChangedAt { get; set; }

        

        public List<string> LastAllXmlFileInMyBranch { get; set; }

        public int NothingChangeCount { get; set; }
    }
}
