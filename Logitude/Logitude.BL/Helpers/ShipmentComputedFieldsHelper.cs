using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.Helpers
{
    public class ShipmentComputedFieldsHelper
    {

        public void UpdateShipmentComputedFields(ShipmentComputedFields shipmentComputedFields)
        {
            if (shipmentComputedFields != null)
            {
                bool isSaveShipmentComputedFields = false;
                if (EntityChangeHelper.IsShowLogBoxAutomationFields())
                {
                    ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(shipmentComputedFields.Tenant);
                    ShipmentComputedFields oldShipmentCompField = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(shipmentComputedFields.Id, shipmentComputedFields.Tenant);
                    if (CheckIfShipmentComputedFieldsChange(oldShipmentCompField, shipmentComputedFields))
                    {
                        ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentComputedFields.Tenant);
                        ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(shipmentComputedFields.Id, shipmentComputedFields.Tenant);
                        if (shipmentPM != null)
                        {
                            shipmentPM.IsDepositionRequired = shipmentComputedFields.IsDepositionRequired;
                            shipmentPM.IsRequestedDocuments = shipmentComputedFields.IsRequestedDocuments;
                            shipmentPM.IsDigitalSignRequired = shipmentComputedFields.IsDigitalSignRequired;
                            shipmentPM.IsMissingDocuments = shipmentComputedFields.IsMissingDocuments;
                            shipmentPM.DocumentsSearchFields = shipmentComputedFields.DocumentsSearchFields;
                            shipmentPM.MissingDocumentsCount = shipmentComputedFields.MissingDocumentsCount;
                            shipmentPM.MissingDocumentsNames = shipmentComputedFields.MissingDocumentsNames;
                            shipmentPM.RequestedDocumentsCount = shipmentComputedFields.RequestedDocumentsCount;
                            shipmentPM.NumberOfHouses = shipmentComputedFields.NumberOfHouses;
                            shipmentPM.ImporterDepositionRequestDetails = shipmentComputedFields.ImporterDepositionRequestDetails;
                            shipmentPM.LastDocumentDateTime = shipmentComputedFields.LastDocumentDateTime;
                            shipmentPM.ShipperReference1 = shipmentPM.CustomerReference1;
                            shipmentPM.ConsigneeReference1 = shipmentPM.CustomerReference1;
                            shipmentPM.ShipperReference2 = shipmentPM.CustomerReference2;
                            shipmentPM.ConsigneeReference2 = shipmentPM.CustomerReference2;
                            //shipmentPM.ShipperId = shipmentPM.CustomerId;
                            shipmentPM.IsShipmentComputedFieldChange = true;
                            shipmentPM.IsImporterShipment = true;

                            string email = "system@tenant" + shipmentComputedFields.Tenant.ToString() + ".com"; //SecurityUtility.GetAuthenticatedUser(shipmentComputedFields.Tenant);
                            IShipmentsContext objectContext = ShipmentsContext.GetContext(shipmentPM.Tenant);
                            ShipmentService shipmentService = new ShipmentService(objectContext, shipmentPM, email);
                            shipmentService.entityComputedFields = shipmentComputedFields;
                            shipmentService.Update();
                            isSaveShipmentComputedFields = true;
                        }
                    }
                }

                if (!isSaveShipmentComputedFields)
                {
                    ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(shipmentComputedFields.Tenant);
                    shipmentComputedFieldsRepository.Update(shipmentComputedFields);
                    shipmentComputedFieldsRepository.SubmitChanges();
                    isSaveShipmentComputedFields = true;
                }
            }
        }


        public bool CheckIfShipmentComputedFieldsChange(ShipmentComputedFields oldShipmentComputedFields, ShipmentComputedFields newShipmentComputedFields)
        {
            bool result = false;
            if (oldShipmentComputedFields != null && newShipmentComputedFields != null)
            {
                if (oldShipmentComputedFields.IsMissingDocuments != newShipmentComputedFields.IsMissingDocuments) return true;
                //if (oldShipmentComputedFields.DocumentsSearchFields != newShipmentComputedFields.DocumentsSearchFields) return true;
                if (oldShipmentComputedFields.MissingDocumentsCount != newShipmentComputedFields.MissingDocumentsCount) return true;
                if (oldShipmentComputedFields.MissingDocumentsNames != newShipmentComputedFields.MissingDocumentsNames) return true;
                if (oldShipmentComputedFields.IsRequestedDocuments != newShipmentComputedFields.IsRequestedDocuments) return true;
                if (oldShipmentComputedFields.RequestedDocumentsCount != newShipmentComputedFields.RequestedDocumentsCount) return true;
                if (oldShipmentComputedFields.NumberOfHouses != newShipmentComputedFields.NumberOfHouses) return true;
                if (oldShipmentComputedFields.IsDigitalSignRequired != newShipmentComputedFields.IsDigitalSignRequired) return true;
                if (oldShipmentComputedFields.IsDepositionRequired != newShipmentComputedFields.IsDepositionRequired) return true;
                if (oldShipmentComputedFields.ImporterDepositionRequestDetails != newShipmentComputedFields.ImporterDepositionRequestDetails) return true;
                //if (oldShipmentComputedFields.LastDocumentDateTime != newShipmentComputedFields.LastDocumentDateTime) return true;
            }
            return result;
        }




    }
}
