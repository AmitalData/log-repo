
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Logitude.Server.Tools;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.ShipmentOrderModule.BL.EntityDataMappings
{

    public partial class ShipmentOrderDataMapping : IMapping<ShipmentOrderPM, ShipmentOrder>
    {

        public void CustomPMToPOCO(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            if (!string.IsNullOrEmpty(entityPOCO.TransportModeId))
            {
                TransportModeQuery transportModeQuery = new TransportModeQuery(entityPM.Tenant);
                TransportModePM transportModePM = transportModeQuery.GetSinglePM(entityPM.TransportModeId, entityPM.Tenant);
                entityPM.TransportModeName = transportModePM.Name;
            }
            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
            {
                ShipmentTypeQuery shipmentTypeQuery = new ShipmentTypeQuery(entityPM.Tenant);
                ShipmentTypePM shipmentTypePM = shipmentTypeQuery.GetSinglePM(entityPM.ShipmentTypeId, entityPM.Tenant);
                entityPM.ShipmentTypeName = shipmentTypePM.Name;
            }
        }

        private void BuildSearchFields(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.OrderNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReferences);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PONumber);

            mySearchFields = AddCardNameToSearchField(entityPM.ConsigneeId, entityPM.Tenant, mySearchFields);
            mySearchFields = AddCardNameToSearchField(entityPM.ShipperId, entityPM.Tenant, mySearchFields);
            mySearchFields = AddCardNameToSearchField(entityPM.AgentId, entityPM.Tenant, mySearchFields);
            mySearchFields = AddCardNameToSearchField(entityPM.ForwarderId, entityPM.Tenant, mySearchFields);

            entityPM.SearchFields = mySearchFields;
        }

        private string AddCardNameToSearchField(string cardId, int tenant, string mySearchFields)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                Card myCard = CardRepository.GetSingleCard(cardId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            return mySearchFields;
        }



    }


}
