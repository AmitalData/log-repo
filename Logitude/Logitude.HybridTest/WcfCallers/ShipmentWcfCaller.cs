using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class ShipmentWcfCaller
    {
        public static Response PrepareDirectShipment()
        {
            if (HybridData.DirectShipmentId == null)
            {
                return CallDirectShipmentUpsert();
            }
            return new Response();
        }
        public static Response CallDirectShipmentUpsert()
        {
            PrepareShipmentVars();
            ShipmentPM entityPM = new ShipmentPM()
            {
                ShipmentNumber = HybridData.DirectShipmentCode,
                TransportModeId = "A", //A:Air, O:Occean, I:Inland
                DirectionId = "E", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                FreightPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                OtherPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                BranchId = HybridData.BranchCode,
                DepartmentId = HybridData.DepartmentCode,
                ConsigneeId = HybridData.AgentCode,
                ShipmentLevelCode = "D", //D:Direct, H: House, C:Consol, A:Customs
                FromPortId = HybridData.FromPortCode,
                ToPortId = HybridData.ToPortCode,
                MainCarriageFromPortId = HybridData.FromPortCode,
                MainCarriageToPortId = HybridData.ToPortCode,
                CreateDateTime = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                AgentContactId = HybridData.AgentCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "Upsert",
                ServiceType = typeof(ShipmentPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if (!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.DirectShipmentId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareHouseShipment()
        {
            if (HybridData.HouseShipmentId == null)
            {
                return CallHouseShipmentUpsert();
            }
            return new Response();
        }
        public static Response CallHouseShipmentUpsert()
        {
            PrepareShipmentVars();
            ShipmentPM entityPM = new ShipmentPM()
            {
                ShipmentNumber = HybridData.HouseShipmentCode,
                TransportModeId = "A", //A:Air, O:Occean, I:Inland
                DirectionId = "E", //I:Import, E:Export, D:Drop, R:Drop, C:Customs Import
                FreightPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                OtherPrepaidCollectId = "P", //P:Prepaid, C:Collect, B:Both
                BranchId = HybridData.BranchCode,
                DepartmentId = HybridData.DepartmentCode,
                ConsigneeId = HybridData.AgentCode,
                ShipmentLevelCode = "H", //D:Direct, H: House, C:Consol, A:Customs
                FromPortId = HybridData.FromPortCode,
                ToPortId = HybridData.ToPortCode,
                MainCarriageFromPortId = HybridData.FromPortCode,
                MainCarriageToPortId = HybridData.ToPortCode,
                CreateDateTime = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                AgentContactId = HybridData.AgentCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Shipment",
                ServiceOperation = "Upsert",
                ServiceType = typeof(ShipmentPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if (!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.HouseShipmentId = serviceResponse.Result;
            return serviceResponse;
        }        
        private static void PrepareShipmentVars()
        {
            BranchWcfCaller.PrepareBranch();
            DepartmentWcfCaller.PrepareDepartment();
            PortWcfCaller.PrepareFromPort();
            PortWcfCaller.PrepareToPort();
            AgentWcfCaller.PrepareAgent();
            UserWcfCaller.PrepareUser();
        }
    }
}
