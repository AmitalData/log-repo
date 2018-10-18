using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GAQDOCKeys : EntityKeyFields
    {
        public string APPQID { get; set; }
        public string FOLDERCODE { get; set; }
        public string DOCID { get; set; }

        public override string GetFullKey()
        {
            return APPQID + "_" + FOLDERCODE + "_" + DOCID;
        }

        public override string GetEntityPMName()
        {
            return "GAQDOC";
        }

    }
}

