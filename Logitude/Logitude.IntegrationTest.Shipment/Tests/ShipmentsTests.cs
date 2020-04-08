using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.IntegrationTest.Shipment.Tests;


namespace Logitude.IntegrationTest.Shipment
{
    [TestClass]
    public class ShipmentsTests
    {

        ShipmentPM shipmentPM = new ShipmentPM();
        CreateShipment shipment = new CreateShipment();
        [TestMethod]
        public async Task TestDirectExportAirShipment()
        {

            //shipment.PostShipment
            await shipment.PostShipment("D", "E", "A");
            shipmentPM = await shipment.GetShipment(ShipmentVariables.ShipmentId);
            //ShipmentPM entityPM= UpdateShipmentAirExport(shipmentPM);
            //await TestReceivables(shipmentPM, quantity, unitPrice);
        }
        [TestMethod]
        public async Task CalculateShipmentTotalProfit()
        {
            int quantity = 1;
            int PayableUnitPrice = 50;
            int RecUnitPrice = 20;
            await shipment.PostShipment("D", "E", "A");

            shipmentPM = await shipment.GetShipment(ShipmentVariables.ShipmentId);
            ShipmentPM entityPM = shipment.UpdateReceivables(shipmentPM, quantity, RecUnitPrice);
            entityPM = shipment.UpdatePayables(entityPM, quantity, PayableUnitPrice);
            await shipment.PutShipment(entityPM);




            // shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);

        }

        /* [TestMethod]
         public async Task TestReceivables()
         {
             int quantity = 5;
             int unitPrice = 5;
             //await PostShipment("D", "E", "A");
             shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);
             ShipmentPM entityPM = UpdateReceivables(shipmentPM, quantity, unitPrice);
             await PutShipment(entityPM);
             shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);
             try
             {
                 Assert.AreEqual(60, shipmentPM.OpenReceivablesInLocalCurrency);

             }
             catch (Exception Ex)
             {
                 throw new Exception(Ex.Message);
             }
         }
         [TestMethod]
         public async Task TestPayables()
         {
             int quantity = 5;
             int unitPrice = 5;
             await PostShipment("D", "E", "A");
             shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);
             ShipmentPM entityPM = UpdatePayables(shipmentPM, quantity, unitPrice);
             await PutShipment(entityPM);
             shipmentPM = await GetShipment(ShipmentVariables.ShipmentId);
             try
             {
                 Assert.AreEqual("50", shipmentPM.OpenPayablesInLocalCurrency);

             }
             catch (Exception Ex)
             {
                 throw new Exception(Ex.Message);
             }
         }*/
    }
}
