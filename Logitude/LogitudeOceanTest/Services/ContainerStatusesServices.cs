using Logitude.OceanTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.OceanTest.Services
{
    public class ContainerStatusesServices
    {
        internal ShipmentContainerSimulator CreateShipmentContainerSimulator(string xMLFile)
        {
            throw new NotImplementedException();
        }

        internal void ValidateShipmentContainerSimulator(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            if (shipmentContainerSimulator.Errors.Any())
            {
                throw new InvalidOperationException("Failed Run Shipment Container Simulator", new Exception(string.Join(", ", shipmentContainerSimulator.Errors)));
            }

        }

        internal bool CheckWorkerQuewe(ShipmentContainerSimulator shipmentContainerSimulator)
        {

            return false;
        }
    }
}
