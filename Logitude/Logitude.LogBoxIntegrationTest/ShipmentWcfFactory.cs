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
            ShipmentNumber = CloudVariables.DirectShipmentCode,
            BranchId = CloudVariables.BranchCodeHBRA,
            DepartmentId = CloudVariables.DepartmentCodeHDEP,
            ConsigneeId = CloudVariables.AgentCodeHAgent,
            FromPortId = CloudVariables.PortCodeLON,
            ToPortId = CloudVariables.PortCodeMAN,
            MainCarriageFromPortId = CloudVariables.PortCodeLON,
            MainCarriageToPortId = CloudVariables.PortCodeMAN,
            CreateDateTime = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            AgentContactId = CloudVariables.AgentCodeHAgent,
            Tenant = EnvironmentParams.CloudTenant,
            TransportModeId = "A",//A:Air, O:Occean, I:Inland
            DirectionId = "C",//I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
            FreightPrepaidCollectId = "P",//P:Prepaid, C:Collect, B:Both
            OtherPrepaidCollectId = "P",//P:Prepaid, C:Collect, B:Both
            ShipmentLevelCode = "D",//D:Direct, H: House, C:Consol, A:Customs
        };

        public static ShipmentPM GetShipmentPM()
        {
            return shipmentPM;
        }
        public static ShipmentPM GetShipmentPMWithNewNumber()
        {
            shipmentPM.ShipmentNumber = Guid.NewGuid().ToString().Substring(0, 6) + Guid.NewGuid().ToString().Substring(0, 6); //TableCounter.GetNumber(EnvironmentGlobalParams.MainTenant, "SHIP", shipmentPM.DirectionId, shipmentPM.TransportModeId);
            return shipmentPM;
        }
    }
}
