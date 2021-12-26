using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Logitude.BL.CommonDataModel.APIDataContract.QueryService
{
    public class APIUnassignedDataHandler
	{
		public bool HasUnassignedData ;
		public Dictionary<string,string> ReceivedCodes;
		private CardQuery query = null;
		private int tenant;
		private string computingPartnerName = null;
		private ComputingPartnerTranslationHelper computingPartnerTranslationHelper = null;
		private ObjectTableRepository objectTableRepository;
		private UnassignedEntityQuery unassignedEntityQuery;
		private string customerObjectTableName  = "Customer";
		private string CardTypeSameAsCustomerCode = "";
		public APIUnassignedDataHandler(int tenant,string computingPartnerName)
		{
			this.tenant = tenant;
			this.query = new CardQuery(tenant);
			this.computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(this.tenant);
			this.objectTableRepository = new ObjectTableRepository(this.tenant);
			this.unassignedEntityQuery = new UnassignedEntityQuery(this.tenant);
			this.computingPartnerName = computingPartnerName;
			this.HasUnassignedData = false;
			this.ReceivedCodes = new Dictionary<string, string>();
		}

		public Direct HandleUnassignedDirectShipmentData(Direct shipment) 
		{
			if (shipment == null)
				return shipment;

			shipment.Shipper = this.HandleUnassignedCard(shipment.Shipper, CardsTypes.Shipper.ToString() , customerObjectTableName);
			shipment.Consignee = this.HandleUnassignedCard(shipment.Consignee, CardsTypes.Consignee.ToString(), customerObjectTableName);
			shipment.Customer = this.HandleCustomerUnassignedCard(shipment.Customer, shipment);
		
			return shipment;
		}

		public House HandleUnassignedHouseShipmentData(House shipment)
		{
			if (shipment == null)
				return shipment;

			shipment.Shipper = this.HandleUnassignedCard(shipment.Shipper, CardsTypes.Shipper.ToString(), customerObjectTableName);
			shipment.Consignee = this.HandleUnassignedCard(shipment.Consignee, CardsTypes.Consignee.ToString(), customerObjectTableName);
			shipment.Customer = this.HandleCustomerUnassignedCard(shipment.Customer, shipment);

			return shipment;
		}

		public Master HandleUnassignedMasterShipmentData(Master shipment)
		{
			if (shipment == null)
				return shipment;

			shipment.Shipper = this.HandleUnassignedCard(shipment.Shipper, CardsTypes.Shipper.ToString(), customerObjectTableName);
			shipment.Consignee = this.HandleUnassignedCard(shipment.Consignee, CardsTypes.Consignee.ToString(), customerObjectTableName);

			return shipment;
		}

		public ShipmentPM AddDirectShipmentUnassignedData(Direct shipment,ShipmentPM shipmentPM)
		{
			shipmentPM.ShipmentUnassignedFields = new List<ShipmentUnassignedFieldPM>();

			this.HandleShipmentUnassignedData(shipment.UnassignedShipperAddress, shipmentPM, CardsTypes.Shipper.ToString());
			this.HandleShipmentUnassignedData(shipment.UnassignedConsigneeAddress,shipmentPM, CardsTypes.Consignee.ToString());
			this.HandleUnassignedCustomerType(shipmentPM);

			return shipmentPM;
		}

		public ShipmentPM AddHouseShipmentUnassignedData(House shipment, ShipmentPM shipmentPM)
		{
			shipmentPM.ShipmentUnassignedFields = new List<ShipmentUnassignedFieldPM>();
			this.HandleShipmentUnassignedData(shipment.UnassignedShipperAddress, shipmentPM, CardsTypes.Shipper.ToString());
			this.HandleShipmentUnassignedData(shipment.UnassignedConsigneeAddress, shipmentPM, CardsTypes.Consignee.ToString());
		    this.HandleUnassignedCustomerType(shipmentPM);

			return shipmentPM;
		}

		public ShipmentPM AddMasterShipmentUnassignedData(Master shipment, ShipmentPM shipmentPM)
		{
			shipmentPM.ShipmentUnassignedFields = new List<ShipmentUnassignedFieldPM>();
			this.HandleShipmentUnassignedData(shipment.UnassignedShipperAddress, shipmentPM, CardsTypes.Shipper.ToString());
			this.HandleShipmentUnassignedData(shipment.UnassignedConsigneeAddress, shipmentPM, CardsTypes.Consignee.ToString());

			return shipmentPM;
		}

		private Card HandleUnassignedCard(Card card, string cardType, string cardTypeObjectTableName)
		{
			if (card == null)
				return null;

			if (!card.AllowUnassignedEntry)
				return card;

			if (this.IsCardExsist(card, cardType))
				return card;

			string cardReceivedCode = card.Code;
			ReceivedCodes.Add(cardType, cardReceivedCode);
			card.Code = GetUnassignedCardCode(cardTypeObjectTableName);
			this.HasUnassignedData = card.Code != null ? true : this.HasUnassignedData;

			return card;
		}

		private Card HandleCustomerUnassignedCard(Card card, dynamic shipment)
		{
			if (card == null)
				return null;

			if (!ReceivedCodes.ContainsValue(card.Code))
				return card;

			this.CardTypeSameAsCustomerCode = ReceivedCodes.FirstOrDefault(x => x.Value == card.Code).Key;

			if (this.CardTypeSameAsCustomerCode == CardsTypes.Shipper.ToString())
				card.Code = shipment?.Shipper?.Code;
			else if (this.CardTypeSameAsCustomerCode == CardsTypes.Consignee.ToString())
				card.Code = shipment?.Consignee?.Code;

			return card;
		}

		private bool IsCardExsist(Card card,string cardType)
		{
			this.ValidateCardPartnerCode(card, cardType);
			if (!string.IsNullOrEmpty(card.Id))
			{
				return query.IsCardExisitByCardId(card.Id, this.tenant);
			}
			if (!string.IsNullOrEmpty(card.Code))
			{
				return query.IsCardExisitByCardCode(card.Code, this.tenant);
			}
			if (!string.IsNullOrEmpty(card.PartnerCode) && !string.IsNullOrEmpty(computingPartnerName))
			{
				var cardCode = this.computingPartnerTranslationHelper.GetLogitudeCodeTranslation(card.PartnerCode, computingPartnerName, "Card");
				return query.IsCardExisitByCardCode(cardCode, this.tenant);
			}
			return false;
		}

		private void ValidateCardPartnerCode(Card card, string cardType)
        {
			if (!string.IsNullOrEmpty(card.Id))
				return;

			if (!string.IsNullOrEmpty(card.Code))
				return;

			if (!string.IsNullOrEmpty(card.PartnerCode))
				return;

			throw new ApplicationException(cardType + " PartnerCode is required");
		}

		private string GetUnassignedCardCode(string objectTableName)
        {
			UnassignedEntityPM unassignedEntityPM = this.unassignedEntityQuery.GetSinglePMByObjectTableId(GetObjectTableId(objectTableName),this.tenant);
			string unassignedCardCode = unassignedEntityPM?.UnassignedCode;

			return unassignedCardCode;
		}

		private string GetObjectTableId(string objectTableName)
        {
			ObjectTable objectTable = this.objectTableRepository.GetObjectTableByName(objectTableName, this.tenant, true);
			string objectTableId = objectTable?.Id;

			return objectTableId;
		}

		private void HandleShipmentUnassignedData(Address shipperAddress, ShipmentPM shipmentPM,string cardtype)
        {
			if (!this.HasUnassignedData)
				return;

			if (!ReceivedCodes.ContainsKey(cardtype))
				return;

			if (string.IsNullOrEmpty(ReceivedCodes[cardtype]))
				return;

			shipmentPM.ShipmentUnassignedFields.Add(this.GetShipmentUnassignedField(shipperAddress, cardtype));
		}

		private ShipmentUnassignedFieldPM GetShipmentUnassignedField(Address address,string cardtype)
        {
			ShipmentUnassignedFieldPM shipmentUnassignedFieldPM = new ShipmentUnassignedFieldPM();
			shipmentUnassignedFieldPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
			shipmentUnassignedFieldPM.FieldName = cardtype;
			shipmentUnassignedFieldPM.ReceivedData = this.ConvertAddressToXML(address);
			shipmentUnassignedFieldPM.ReceivedCode = ReceivedCodes[cardtype];

			return shipmentUnassignedFieldPM;
		}

		private string ConvertAddressToXML(Address address)
		{
			if (address == null)
				return "";

			using (var stringWriter = new System.IO.StringWriter())
			{
				var serializer = new XmlSerializer(address.GetType());
				serializer.Serialize(stringWriter, address);
				return stringWriter.ToString();
			}
		}

		private void HandleUnassignedCustomerType(ShipmentPM shipmentPM)
        {
			if (string.IsNullOrEmpty(this.CardTypeSameAsCustomerCode))
				return;

			if(this.CardTypeSameAsCustomerCode == CardsTypes.Shipper.ToString())
            {
				shipmentPM.ShipmentCustomerTypeCode = "SHI";
			}
			else if (this.CardTypeSameAsCustomerCode == CardsTypes.Consignee.ToString())
			{
				shipmentPM.ShipmentCustomerTypeCode = "CON";
			}
		}
	}

    public enum CardsTypes {
		Shipper ,
		Consignee
	}

}
