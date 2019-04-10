using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
   public class ShipmentComputedFieldsHelper
    {

        public void UpdateShipmentComputedFields(ShipmentComputedFields shipmentComputedFields)
        {
            if (shipmentComputedFields != null)
            {
                if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1")
                {

                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentComputedFields.Tenant);
                    ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(shipmentComputedFields.Id, shipmentComputedFields.Tenant);
                    if (shipmentPM != null)
                    {
                        shipmentPM.ShipmentComputedFields = shipmentComputedFields;
                        shipmentPM.IsDepositionRequired = shipmentComputedFields.IsDepositionRequired;
                        shipmentPM.IsRequestedDocuments = shipmentComputedFields.IsRequestedDocuments;
                        shipmentPM.IsDigitalSignRequired = shipmentComputedFields.IsDigitalSignRequired;
                        shipmentPM.IsShipmentComputedFieldChange = true;

                        IShipmentsContext objectContext = ShipmentsContext.GetContext(shipmentPM.Tenant);
                        ShipmentService shipmentService = new ShipmentService(objectContext, shipmentPM, SecurityUtility.GetAuthenticatedUser());
                        shipmentService.Update();
                    }

                }
                else
                {
                    ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(shipmentComputedFields.Tenant);
                    shipmentComputedFieldsRepository.Update(shipmentComputedFields);
                    shipmentComputedFieldsRepository.SubmitChanges();
                }
            }

        }
    }
}
