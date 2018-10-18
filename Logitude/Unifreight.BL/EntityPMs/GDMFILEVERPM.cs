using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{

    public partial class GDMFILEVERPM : EntityPM
    {
        public string COMID { get; set; }

        public int VERSION { get; set; }

        public string MD5HASH { get; set; }

        public string EXTENSION { get; set; }

    }
}
