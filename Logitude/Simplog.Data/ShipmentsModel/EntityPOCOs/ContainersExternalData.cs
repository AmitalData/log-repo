using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Linq;
using System.Text;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ContainersExternalData
    {
        [Key, ForeignKey("Container")]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime? GateIn { get; set; }
        public DateTime? GateOut { get; set; }
        public Container Container { get; set; }
    }
}
