using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   
    public class WorkerRoleName
    {
        [Key]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int WaitingStatus { get; set; }
    }
}
