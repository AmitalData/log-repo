using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class FBLStockPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int Number { get; set; }
        public DateTime InsertionDate { get; set; }
        public string Notes { get; set; }
        public bool IsUsed { get; set; }
    }
}
