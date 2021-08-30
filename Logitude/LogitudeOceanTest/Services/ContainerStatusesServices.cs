using Logitude.OceanTest.Models;
using Logitude.OceanTest.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
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
            _oceanContext.ShipmentContainerSimulator = APICaller.CallPost<ShipmentContainerSimulator>(_oceanContext.ShipmentContainerSimulator, Urls.ShipmentContainersWebServiceController, UserTenant.Token)?.Data;

            return false;
        }
        
    }
}
