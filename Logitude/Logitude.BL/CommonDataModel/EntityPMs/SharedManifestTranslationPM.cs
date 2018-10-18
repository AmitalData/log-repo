using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class SharedManifestTranslationPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableName { get; set; }
        public string MyCode { get; set; }
        public string AgentCode { get; set; }
        public string AgentId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

    }
}
