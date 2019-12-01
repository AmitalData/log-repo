using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfFactory
{
    class ShipmentWcfFactory
    {
        readonly private static ShipmentPM shipmentPM = new ShipmentPM()
        {
            ShipmentNumber = HybridData.DirectShipmentCode,
            BranchId = HybridData.BranchCodeHBRA,
            DepartmentId = HybridData.DepartmentCodeHDEP,
            ConsigneeId = HybridData.AgentCodeHAgent,
            FromPortId = HybridData.PortCodeLON,
            ToPortId = HybridData.PortCodeMAN,
            MainCarriageFromPortId = HybridData.PortCodeLON,
            MainCarriageToPortId = HybridData.PortCodeMAN,
            CreateDateTime = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            AgentContactId = HybridData.AgentCodeHAgent,
            Tenant = TestEnvironmentGlobalParameters.Tenant,
            TransportModeId = "A",//A:Air, O:Occean, I:Inland
            DirectionId = "E",//I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
            FreightPrepaidCollectId = "P",//P:Prepaid, C:Collect, B:Both
            OtherPrepaidCollectId = "P",//P:Prepaid, C:Collect, B:Both
            ShipmentLevelCode = "D",//D:Direct, H: House, C:Consol, A:Customs
        };
        public static ShipmentPM GetShipmentPM() { return shipmentPM; }
    }
}
