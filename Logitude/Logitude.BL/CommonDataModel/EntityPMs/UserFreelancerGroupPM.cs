using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class UserFreelancerGroupPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        [DataMember]
        public string UserId { get; set; }
        public string GroupID { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }




    }
}
