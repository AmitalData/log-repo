
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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

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
            entityPM.TransportModeName = GetTransportModeNameById(entityPOCO.TransportModeId, entityPM.Tenant);
            entityPM.ShipmentTypeName = GetShipmentTypeNameById(entityPOCO.ShipmentTypeId, entityPM.Tenant);
            entityPM.ConsigneeName = GetCardNameById(entityPOCO.ConsigneeId, entityPM.Tenant);
            entityPM.ShipperName = GetCardNameById(entityPOCO.ShipperId, entityPM.Tenant);
            entityPM.AgentName = GetCardNameById(entityPOCO.AgentId, entityPM.Tenant);
            entityPM.CustomsAgentName = GetCardNameById(entityPOCO.CustomsAgentId, entityPM.Tenant);
            entityPM.ForwarderName = GetCardNameById(entityPOCO.ForwarderId, entityPM.Tenant);
            entityPM.AccountManagerName = GetAccountManagerNameById(entityPOCO.AccountManagerId, entityPM.Tenant);
            entityPM.VesselName = GetVesselNameById(entityPOCO.VesselId, entityPM.Tenant);
            entityPM.SpecialServicesTypeName = GetSpecialServicesTypeNameById(entityPOCO.SpecialServicesTypeId, entityPM.Tenant);
            entityPM.IncotermCode = GetIncotermCodeById(entityPOCO.IncotermId, entityPM.Tenant);
        }

        private string GetTransportModeNameById(string transportModeId, int tenant)
        {
            if (!string.IsNullOrEmpty(transportModeId))
            {
                TransportModeQuery transportModeQuery = new TransportModeQuery(tenant);
                TransportModePM transportModePM = transportModeQuery.GetSinglePM(transportModeId, tenant);
                return transportModePM.Name;
            }
            return null;
        }

        private string GetShipmentTypeNameById(string shipmentTypeId, int tenant)
        {
            if (!string.IsNullOrEmpty(shipmentTypeId))
            {
                ShipmentTypeQuery shipmentTypeQuery = new ShipmentTypeQuery(tenant);
                ShipmentTypePM shipmentTypePM = shipmentTypeQuery.GetSinglePM(shipmentTypeId, tenant);
                return shipmentTypePM.Name;
            }
            return null;
        }

        private string GetCardNameById(string cardId, int tenant)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                CardQuery cardQuery = new CardQuery(tenant);
                CardPM cardPM = cardQuery.GetSinglePM(cardId, tenant);
                return cardPM?.EnglishName;
            }
            return null;
        }

        private string GetAccountManagerNameById(string accountManagerId, int tenant)
        {
            if (!string.IsNullOrEmpty(accountManagerId))
            {
                UserQuery userQuery = new UserQuery(tenant);
                UserPM userPM = userQuery.GetSinglePM(accountManagerId, tenant);
                return userPM.EnglishName;
            }
            return null;
        }

        private string GetVesselNameById(string vesselId, int tenant)
        {
            if (!string.IsNullOrEmpty(vesselId))
            {
                VesselQuery vesselQuery = new VesselQuery(tenant);
                VesselPM vesselPM = vesselQuery.GetSinglePM(vesselId, tenant);
                return vesselPM.EnglishName;
            }
            return null;
        }

        private string GetSpecialServicesTypeNameById(string specialServicesTypeId, int tenant)
        {
            if (!string.IsNullOrEmpty(specialServicesTypeId))
            {
                SpecialServicesTypeQuery specialServicesTypeQuery = new SpecialServicesTypeQuery(tenant);
                SpecialServicesTypePM specialServicesTypePM = specialServicesTypeQuery.GetSinglePM(specialServicesTypeId, tenant);
                return specialServicesTypePM.EnglishName;
            }
            return null;
        }

        private string GetIncotermCodeById(string incotermId, int tenant)
        {
            if (!string.IsNullOrEmpty(incotermId))
            {
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                IncotermPM incotermPM = incotermQuery.GetSinglePM(incotermId, tenant);
                return incotermPM.Code;
            }
            return null;
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
