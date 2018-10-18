using System;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GTBDOCPM : EntityPM
    {
        public string DOCID { get; set; }

        public string NAMEHEB { get; set; }
 
        public string NAMEENG { get; set; }

        public string BLOCKRECORD { get; set; }

        public string USERDOC { get; set; }

        public string SEARCHENG { get; set; }

        public string DISCLIENT { get; set; }

        public string DISFOREIGNCL { get; set; }

        public string DISAGENT { get; set; }

        public string COPYDESC { get; set; }

        public string FOLDERCODE { get; set; }

        public string DECLARATIONMAPPING { get; set; }

        public string OCR { get; set; }

    }
}

