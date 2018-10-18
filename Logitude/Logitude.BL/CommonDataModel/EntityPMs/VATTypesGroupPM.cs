using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class VATTypesGroupPM
    {
        [Key]
        public string GroupVATTypeId { get; set; }
        [Key]
        public string SingleVATTypeId { get; set; }

        public int Tenant { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        public string SingleVATTypeName { get; set; }
        public double? SingleVATTypePercentage { get; set; }
    }
}
