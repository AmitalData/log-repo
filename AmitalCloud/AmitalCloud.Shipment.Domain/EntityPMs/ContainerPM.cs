
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ContainerPM : EntityPM
    {
        public DateTime? OIEventDate { get; set; }
        public string OIContainerStatus { get; set; }
        public bool IsShipmentBatchUpdate { get; set; }
        public bool IsEmptyReturnDatesChanged { get; set; }
    }
}
