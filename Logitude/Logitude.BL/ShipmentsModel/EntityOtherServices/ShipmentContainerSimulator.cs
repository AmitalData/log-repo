using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityOtherServices
{
    public class ShipmentContainerSimulator
    {
        public string XmlString { get; set; }
        public string AnalyzeQueueId { get; set; }
        public int FilesCount { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public bool IsFromContainer { get; set; }
        public string ShipmentId { get; set; }
        public string ContainerNumber { get; set; }
        public string CarrierId { get; set; }
        public ShipmentContainerSimulator()
        {
            this.Success = true;
            this.Errors = new List<string>();
        }

    }
}
