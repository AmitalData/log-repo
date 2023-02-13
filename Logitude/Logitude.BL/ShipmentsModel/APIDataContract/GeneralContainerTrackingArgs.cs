using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract
{
    public class GeneralContainerTrackingArgs
    {
        public string Data { get; set; }
        public bool IsFromContainer { get; set; }
        public string ContainerId { get; set; }
        public string ShipmentId { get; set; }
        public string ContainerStatusSourceCode { get; set; }
        public int Tenant { get; set; }
        public string ContainerNumber { get; set; }
        public string CarrierId { get; set; }
        public string CarrierCode { get; set; }
        public string ScacCode { get; set; }
        public bool IsSimulator { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string Master { get; internal set; }
        public string SourceCode { get; set; }
        public string DirectionId { get; set; }
        public bool IsUpdatedFromRequest { get; set; }
        public DateTime? ShipmentCreateDateTime { get; set; }

        public GeneralContainerTrackingArgs()
        {
            Success = true;
            Errors = new List<string>();
        }

    }

    public class UnsubscribeArgs00
    {
        public bool IsFromContainer { get; set; }
        public string ContainerId { get; set; }
        public string ShipmentId { get; set; }
        public string SourceCode { get; set; }
        public bool IsSimulate { get; set; }

    }
}
