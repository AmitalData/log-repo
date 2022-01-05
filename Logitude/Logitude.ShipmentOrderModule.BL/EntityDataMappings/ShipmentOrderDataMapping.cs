
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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.Helpers;

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
            List<CardList> cards = GetCardList(entityPOCO.Tenant, entityPOCO);
            List<PortList> ports = GetPortList(entityPOCO.Tenant, entityPOCO);
            incoterm = GetIncotermById(entityPM.IncotermId, entityPM.Tenant);
            entityPM.ConsigneeName = cards.Where(d => d.Id == entityPOCO.ConsigneeId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.ShipperName = cards.Where(d => d.Id == entityPOCO.ShipperId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.AgentName = cards.Where(d => d.Id == entityPOCO.AgentId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.CustomsAgentName = cards.Where(d => d.Id == entityPOCO.CustomsAgentId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.ForwarderName = cards.Where(d => d.Id == entityPOCO.ForwarderId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.CarrierName = cards.Where(d => d.Id == entityPOCO.CarrierId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.CustomerName = cards.Where(d => d.Id == entityPOCO.CustomerId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.PlaceOfDeliveryName = cards.Where(d => d.Id == entityPOCO.PlaceOfDeliveryId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.OriginPortName = ports.Where(d => d.Id == entityPOCO.OriginPortId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.DestinationPortName = ports.Where(d => d.Id == entityPOCO.DestinationPortId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.GatewayName = ports.Where(d => d.Id == entityPOCO.GatewayId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.AccountManagerName = GetAccountManagerNameById(entityPOCO.AccountManagerId, entityPOCO.Tenant);
            entityPM.VesselName = GetVesselNameById(entityPOCO.VesselId, entityPOCO.Tenant);
            entityPM.SpecialServicesTypeName = GetSpecialServicesTypeNameById(entityPOCO.SpecialServicesTypeId, entityPOCO.Tenant);
            entityPM.IncotermCode = incoterm?.Code;
            entityPM.ShipmentLevelName = GetShipmentLevelNameByCode(entityPOCO.ShipmentLevelCode, entityPOCO.Tenant);
            entityPM.TransportModeName = GetTransportModeNameById(entityPOCO.TransportModeId, entityPOCO.Tenant);
            entityPM.DirectionName = GetDirectionNameById(entityPOCO.DirectionId, entityPOCO.Tenant);
            entityPM.OriginPortCode = ports.Where(d => d.Id == entityPOCO.OriginPortId).Select(d => d.Code).FirstOrDefault();
            entityPM.GatewayCode = ports.Where(d => d.Id == entityPOCO.GatewayId).Select(d => d.Code).FirstOrDefault();
            entityPM.DestinationPortCode = ports.Where(d => d.Id == entityPOCO.DestinationPortId).Select(d => d.Code).FirstOrDefault();
            entityPM.ShipmentTypeName = GetShipmentTypeNameById(entityPOCO.ShipmentTypeId, entityPOCO.Tenant);
            entityPM.PackageTypeName = GetPackageTypeNameById(entityPOCO.PackageTypeId, entityPOCO.Tenant);
            entityPM.IncotermName = incoterm?.Name;
        }
        public void CustomLocalPOCOToPM(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
            List<CardList> cards = GetCardList(entityPOCO.Tenant, entityPOCO);
            List<PortList> ports = GetPortList(entityPOCO.Tenant, entityPOCO);
            incoterm = GetIncotermById(entityPM.IncotermId, entityPM.Tenant);
            entityPM.ConsigneeName = cards.Where(d => d.Id == entityPOCO.ConsigneeId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.ShipperName = cards.Where(d => d.Id == entityPOCO.ShipperId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.AgentName = cards.Where(d => d.Id == entityPOCO.AgentId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.CustomsAgentName = cards.Where(d => d.Id == entityPOCO.CustomsAgentId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.ForwarderName = cards.Where(d => d.Id == entityPOCO.ForwarderId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.CarrierName = cards.Where(d => d.Id == entityPOCO.CarrierId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.CustomerName = cards.Where(d => d.Id == entityPOCO.CustomerId).Select(d => !string.IsNullOrEmpty(d.LocalName) ? d.LocalName : d.EnglishName).FirstOrDefault();
            entityPM.OriginPortName = ports.Where(d => d.Id == entityPOCO.OriginPortId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.DestinationPortName = ports.Where(d => d.Id == entityPOCO.DestinationPortId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.GatewayName = ports.Where(d => d.Id == entityPOCO.GatewayId).Select(d => d.EnglishName).FirstOrDefault();
            entityPM.AccountManagerName = GetAccountManagerNameById(entityPOCO.AccountManagerId, entityPOCO.Tenant);
            entityPM.VesselName = GetVesselNameById(entityPOCO.VesselId, entityPOCO.Tenant);
            entityPM.SpecialServicesTypeName = GetSpecialServicesTypeNameById(entityPOCO.SpecialServicesTypeId, entityPOCO.Tenant);
            entityPM.IncotermCode = incoterm?.Code;
            entityPM.ShipmentLevelName = GetShipmentLevelNameByCode(entityPOCO.ShipmentLevelCode, entityPOCO.Tenant);
            entityPM.TransportModeName = GetTransportModeNameById(entityPOCO.TransportModeId, entityPOCO.Tenant);
            entityPM.DirectionName = GetDirectionNameById(entityPOCO.DirectionId, entityPOCO.Tenant);
            entityPM.OriginPortCode = ports.Where(d => d.Id == entityPOCO.OriginPortId).Select(d => d.Code).FirstOrDefault();
            entityPM.GatewayCode = ports.Where(d => d.Id == entityPOCO.GatewayId).Select(d => d.Code).FirstOrDefault();
            entityPM.DestinationPortCode = ports.Where(d => d.Id == entityPOCO.DestinationPortId).Select(d => d.Code).FirstOrDefault();
            entityPM.IncotermName = incoterm?.Name;
        }

        #region Cards
        private List<CardList> GetCardList(int tenant, ShipmentOrder entityPOCO)
        {
            List<string> cardIds = BuildCardIds(entityPOCO);
            CardQuery cardQuery = new CardQuery(tenant);
            return cardQuery.GetCardListsByListIds(cardIds, tenant).ToList();
        }

        private List<string> BuildCardIds(ShipmentOrder entityPOCO)
        {
            return new List<string>
            {
                entityPOCO.ConsigneeId,
                entityPOCO.ShipperId,
                entityPOCO.AgentId,
                entityPOCO.CustomsAgentId,
                entityPOCO.ForwarderId,
                entityPOCO.CarrierId,
                entityPOCO.CustomerId,
                entityPOCO.PlaceOfDeliveryId
            };
        }
        #endregion

        #region Port
        private List<PortList> GetPortList(int tenant, ShipmentOrder entityPOCO)
        {

            List<string> portIds = new List<string> { entityPOCO.OriginPortId, entityPOCO.DestinationPortId, entityPOCO.GatewayId };
            PortQuery portQuery = new PortQuery(tenant);
            return portQuery.GetPortListsByListIds(portIds, tenant).ToList();
        }

        #endregion

        #region Others
        private string GetShipmentLevelNameByCode(string shipmentLevelCode, int tenant)
        {
            if (!string.IsNullOrEmpty(shipmentLevelCode))
            {
                ShipmentLevelQuery shipmentLevelQuery = new ShipmentLevelQuery(tenant);
                ShipmentLevelPM shipmentLevelPM = shipmentLevelQuery.GetSinglePM(shipmentLevelCode, tenant);
                return shipmentLevelPM.Name;
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
        IncotermPM incoterm;
        private IncotermPM GetIncotermById(string incotermId, int tenant)
        {
            if (!string.IsNullOrEmpty(incotermId))
            {
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                return incotermQuery.GetSinglePM(incotermId, tenant);

            }
            return null;
        }


        private string GetDirectionNameById(string directionId, int tenant)
        {
            if (!string.IsNullOrEmpty(directionId))
            {
                DirectionQuery directionQuery = new DirectionQuery(tenant);
                DirectionPM directionPM = directionQuery.GetSinglePM(directionId, tenant);
                return directionPM?.Name;
            }
            return null;
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
                ShipmentTypePM shipmentType = shipmentTypeQuery.GetSinglePM(shipmentTypeId, tenant);
                return shipmentType.Name;
            }
            return null;
        }

        private string GetPackageTypeNameById(string packageTypeId, int tenant)
        {
            if (!string.IsNullOrEmpty(packageTypeId))
            {
                PackageTypeQuery packageTypeQuery = new PackageTypeQuery(tenant);
                PackageTypePM packageType = packageTypeQuery.GetSinglePM(packageTypeId, tenant);
                return packageType.EnglishName;
            }
            return null;
        }




        #endregion

        #region  SearchFields
        private void BuildSearchFields(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.OrderNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReferences);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PONumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.House);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.BookingConfirmationNumber);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.OriginPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.DestinationPortId);
            QueryHelper.AddFullPortToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.GatewayId);
            QueryHelper.AddCardToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.ConsigneeId);
            QueryHelper.AddCardToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.ShipperId);
            QueryHelper.AddCardToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.AgentId);
            QueryHelper.AddCardToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.ForwarderId);
            QueryHelper.AddCardToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.CustomsAgentId);
            QueryHelper.AddCardToSearchFields(ref mySearchFields, entityPM.Tenant, entityPM.CarrierId);

            entityPM.SearchFields = mySearchFields;
        }

        #endregion

    }


}
