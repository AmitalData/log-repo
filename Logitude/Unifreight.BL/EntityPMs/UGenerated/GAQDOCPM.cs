using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GAQDOCPM : EntityPM
    {
        public string APPQID  { get; set; }

        public string FOLDERCODE  { get; set; }

        public string DOCID { get; set; }
    }
}
