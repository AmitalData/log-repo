using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract
{
    public class GeneralContainerStatusSimulatorArgs
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
        public bool IsSimulator { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string Master { get; internal set; }

        public GeneralContainerStatusSimulatorArgs()
        {
            Success = true;
            Errors = new List<string>();
        }

    }

    public class UnsubscribeArgs
    {
        public bool IsFromContainer { get; set; }
        public string ContainerId { get; set; }
        public string ShipmentId { get; set; }
        public string SourceCode { get; set; }
        public bool IsSimulate { get; set; }

    }
}
