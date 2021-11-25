using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.CommonDataModel.APIDataContract.QueryService
{
    public class UnassignedDataQueryService
	{
		public bool HasUnassignedData ;
		public Dictionary<string,string> ReceivedCodes;
		private CardQuery query = null;
		private int tenant;
		private string computingPartnerName = null;
		private ComputingPartnerTranslationHelper computingPartnerTranslationHelper = null;
		private ObjectTableRepository objectTableRepository;
		private UnassignedEntityQuery unassignedEntityQuery;

		public UnassignedDataQueryService(int tenant,string computingPartnerName)
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

			shipment.Shipper = this.HandleUnassignedShipper(shipment.Shipper);
			shipment.Consignee =  this.HandleUnassignedConsignee(shipment.Consignee);
			return shipment;
		}

		public House HandleUnassignedHouseShipmentData(House shipment)
		{
			if (shipment == null)
				return shipment;

			shipment.Shipper = this.HandleUnassignedShipper(shipment.Shipper);
			shipment.Consignee = this.HandleUnassignedConsignee(shipment.Consignee);
			return shipment;
		}

		public Master HandleUnassignedMasterShipmentData(Master shipment)
		{
			if (shipment == null)
				return shipment;

			shipment.Shipper = this.HandleUnassignedShipper(shipment.Shipper);
			shipment.Consignee = this.HandleUnassignedConsignee(shipment.Consignee);
			return shipment;
		}

		public ShipmentPM AddDirectShipmentUnassignedAddress(Direct shipment,ShipmentPM shipmentPM)
		{
			shipmentPM.ShipmentUnassignedFields = new List<ShipmentUnassignedFieldPM>();
			this.HandleShipmentShipperUnassignedAddress(shipment.UnassignedShipperAddress,shipmentPM);
			this.HandleShipmentConsigneeUnassignedAddress(shipment.UnassignedConsigneeAddress,shipmentPM);

			return shipmentPM;
		}

		private Card HandleUnassignedShipper(Card shipper)
        {
			if (shipper == null)
				return null;

			if(!shipper.AllowUnassignedEntry)
				return shipper;

			if (this.IsCardExsist(shipper,"Shipper"))
				return shipper;

			shipper.Code = GetUnassignedCardCode("Customer");
			this.HasUnassignedData = shipper.Code != null ? true : this.HasUnassignedData;
			return shipper;
		}

		private Card HandleUnassignedConsignee(Card consignee)
		{
			if (consignee == null)
				return null;

			if (!consignee.AllowUnassignedEntry)
				return consignee;

			if (this.IsCardExsist(consignee, "Consignee"))
				return consignee;

			consignee.Code = GetUnassignedCardCode("Customer");
			this.HasUnassignedData = consignee.Code != null ? true : this.HasUnassignedData;
			return consignee;
		}

		private bool IsCardExsist(Card card,string cardType)
		{
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

		private void HandleShipmentShipperUnassignedAddress(Address shipperAddress, ShipmentPM shipmentPM)
        {
			if (shipperAddress == null)
				return;

			if (!this.HasUnassignedData)
				return;

			shipmentPM.ShipmentUnassignedFields.Add(this.GetShipmentUnassignedField(shipperAddress, "UnassignedShipperAddress"));
		}

		private void HandleShipmentConsigneeUnassignedAddress(Address consigneeAddress, ShipmentPM shipmentPM)
		{
			if (consigneeAddress == null)
				return;
			if (!this.HasUnassignedData)
				return;
			shipmentPM.ShipmentUnassignedFields.Add(this.GetShipmentUnassignedField(consigneeAddress, "UnassignedConsigneeAddress"));
		}

		private ShipmentUnassignedFieldPM GetShipmentUnassignedField(Address address,string fieldName)
        {
			ShipmentUnassignedFieldPM shipmentUnassignedFieldPM = new ShipmentUnassignedFieldPM();
			shipmentUnassignedFieldPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
			shipmentUnassignedFieldPM.FieldName = fieldName;
			shipmentUnassignedFieldPM.ReceivedData = this.ConvertAddressToXML(address);
			shipmentUnassignedFieldPM.ReceivedCode = "";
			return shipmentUnassignedFieldPM;

		}

		private string ConvertAddressToXML(Address address)
		{
			using (var stringWriter = new System.IO.StringWriter())
			{
				var serializer = new XmlSerializer(address.GetType());
				serializer.Serialize(stringWriter, this);
				return stringWriter.ToString();
			}
		}



	}
}
