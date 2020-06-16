using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.LogboxIntegrationTest
{
    class ShipmentWcfFactory
    {
        readonly private static ShipmentPM shipmentPM = new ShipmentPM()
        {
            //ShipmentNumber = "1005",
            BranchId = CloudVariables.BranchCodeMain,
            DepartmentId = CloudVariables.DepartmentCodeMana,
            Tenant = EnvironmentParams.CloudTenant,
            ShipperId = CloudVariables.CustomerCodeIntegration,
            ConsigneeId = CloudVariables.CustomerCodeIntegration,
            CustomerId = CloudVariables.CustomerCodeIntegration,
            FromPortId = CloudVariables.FromPortCodeUSBOS,
            ToPortId = CloudVariables.ToPortCodeAUAAB,
            MainCarriageFromPortId = CloudVariables.FromPortCodeUSBOS,
            MainCarriageToPortId = CloudVariables.ToPortCodeAUAAB,
            IncotermId = CloudVariables.IncotermCodeFOB,
            CreateDateTime = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ShipmentLevelCode = "D",
            TransportModeId = "A",
            DirectionId = "C",
            FreightPrepaidCollectId = "C",
            OtherPrepaidCollectId = "C",
            ChargeableWeightUnitCode = "KG",
            GrossWeightUnitCode = "KG",
            ShipperReference1  = "SR1",
            ShipperReference2 = "SR2",
            ConsigneeReference1 = "CR1",
            ConsigneeReference2 = "CR2",

        };

        public static ShipmentPM GetShipmentPM()
        {
            return shipmentPM;
        }
        public static ShipmentPM GetShipmentPMWithNewNumber()
        {
            shipmentPM.ShipmentNumber = Guid.NewGuid().ToString().Substring(0, 6) + Guid.NewGuid().ToString().Substring(0, 6);
            return shipmentPM;
        }
    }
}
