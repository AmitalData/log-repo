using Logitude.BL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.Test.Shipments
{
    [TestClass]
    public class ShipmentComputedFieldsHelperTests
    {
        private readonly ShipmentComputedFieldsHelper helper = new ShipmentComputedFieldsHelper();

        [TestMethod]
        public void CheckIfShipmentComputedFieldsChange_WithIdenticalSnapshots_ReturnsFalse()
        {
            var original = CreateBaseline();
            var current = CreateBaseline();

            var result = helper.CheckIfShipmentComputedFieldsChange(original, current);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckIfShipmentComputedFieldsChange_WhenMissingDocumentsFlagChanges_ReturnsTrue()
        {
            var original = CreateBaseline();
            var current = CreateBaseline();
            current.IsMissingDocuments = !original.IsMissingDocuments;

            var result = helper.CheckIfShipmentComputedFieldsChange(original, current);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfShipmentComputedFieldsChange_WhenCountsDiffer_ReturnsTrue()
        {
            var original = CreateBaseline();
            var current = CreateBaseline();
            current.RequestedDocumentsCount = original.RequestedDocumentsCount + 1;

            var result = helper.CheckIfShipmentComputedFieldsChange(original, current);

            Assert.IsTrue(result);
        }

        private static ShipmentComputedFields CreateBaseline()
        {
            return new ShipmentComputedFields
            {
                Id = "SHIP-001",
                Tenant = 1,
                IsMissingDocuments = false,
                MissingDocumentsCount = 0,
                MissingDocumentsNames = string.Empty,
                IsRequestedDocuments = false,
                RequestedDocumentsCount = 0,
                NumberOfHouses = 0,
                IsDigitalSignRequired = false,
                IsDepositionRequired = false,
                ImporterDepositionRequestDetails = string.Empty,
                CreatedFromDigital = false
            };
        }
    }
}

