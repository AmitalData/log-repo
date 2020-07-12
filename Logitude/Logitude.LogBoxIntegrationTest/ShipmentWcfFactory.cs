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
            BranchId = "TLV",//CloudVariables.BranchCodeMain,
            DepartmentId = "WGRP",//CloudVariables.DepartmentCodeMana,
            Tenant = EnvironmentParams.CloudTenant,
            ShipperId = "10013949",//CloudVariables.CustomerCodeIntegration,
            ConsigneeId = "10013949",//CloudVariables.CustomerCodeIntegration,
            CustomerId = "10013949",//CloudVariables.CustomerCodeIntegration,
            FromPortId = "BOS",//CloudVariables.FromPortCodeUSBOS,
            ToPortId = "TLV",//CloudVariables.ToPortCodeAUAAB,
            MainCarriageFromPortId = "BOS",//CloudVariables.FromPortCodeUSBOS,
            MainCarriageToPortId = "TLV",//CloudVariables.ToPortCodeAUAAB,
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
