using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class WorkerRoleNamePM
    {
        [Key]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int WaitingStatus { get; set; }       
    }
}
