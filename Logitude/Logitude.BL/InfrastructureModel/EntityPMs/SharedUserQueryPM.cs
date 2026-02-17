using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class SharedUserQueryPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }        
        public string UserId { get; set; }
        public string QueryId { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
