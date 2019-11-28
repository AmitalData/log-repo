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
            BranchId = HybridData.BranchCode,
            DepartmentId = HybridData.DepartmentCode,
            ConsigneeId = HybridData.AgentCode,
            FromPortId = HybridData.FromPortCode,
            ToPortId = HybridData.ToPortCode,
            MainCarriageFromPortId = HybridData.FromPortCode,
            MainCarriageToPortId = HybridData.ToPortCode,
            CreateDateTime = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            AgentContactId = HybridData.AgentCode,
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
