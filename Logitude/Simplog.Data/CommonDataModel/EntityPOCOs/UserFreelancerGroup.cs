using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class UserFreelancerGroup
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual FreelancerGroupType FreelancerGroupType { get; set; }



    }
}
