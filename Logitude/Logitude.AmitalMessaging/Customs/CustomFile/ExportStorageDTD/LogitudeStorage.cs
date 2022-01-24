using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.AmitalMessaging.Customs.CustomFile.ExportStorageDTD
{
    

	// using System.Xml.Serialization;
	// XmlSerializer serializer = new XmlSerializer(typeof(LOGICUSTFILE));
	// using (StringReader reader = new StringReader(xml))
	// {
	//    var test = (LOGICUSTFILE)serializer.Deserialize(reader);
	// }

	[XmlRoot(ElementName = "General")]
	public class General
	{

		[XmlElement(ElementName = "OperationCode")]
		public string OperationCode { get; set; }

		[XmlElement(ElementName = "SenderCodeID")]
		public string SenderCodeID { get; set; }

		[XmlElement(ElementName = "MessageFromForm")]
		public bool MessageFromForm { get; set; }

		[XmlElement(ElementName = "ReplyPhoneNumeric")]
		public string ReplyPhoneNumeric { get; set; }

		[XmlElement(ElementName = "OperatorID")]
		public string OperatorID { get; set; }

		[XmlElement(ElementName = "InformedParty")]
		public string InformedParty { get; set; }

		[XmlElement(ElementName = "DeclarationsInContainer")]
		public string DeclarationsInContainer { get; set; }

		[XmlElement(ElementName = "ExportManifestNumber")]
		public string ExportManifestNumber { get; set; }

		[XmlElement(ElementName = "ForwarderReference")]
		public string ForwarderReference { get; set; }

		[XmlElement(ElementName = "ExportDealIdentification")]
		public string ExportDealIdentification { get; set; }

		[XmlElement(ElementName = "TransactionQuantity")]
		public string TransactionQuantity { get; set; }

		[XmlElement(ElementName = "MessageContent")]
		public string MessageContent { get; set; }

		[XmlElement(ElementName = "BookingNumber")]
		public string BookingNumber { get; set; }

		[XmlElement(ElementName = "LogisticDeliveryTypeID")]
		public string LogisticDeliveryTypeID { get; set; }

		[XmlElement(ElementName = "CargoRows")]
		public string CargoRows { get; set; }

		[XmlElement(ElementName = "ExporterIdentificationType")]
		public string ExporterIdentificationType { get; set; }

		[XmlElement(ElementName = "ExporterNumber")]
		public string ExporterNumber { get; set; }

		[XmlElement(ElementName = "ExporterFileNumber")]
		public string ExporterFileNumber { get; set; }

		[XmlElement(ElementName = "ReceivingSite")]
		public string ReceivingSite { get; set; }

		[XmlElement(ElementName = "StuffingSiteType")]
		public string StuffingSiteType { get; set; }

		[XmlElement(ElementName = "LoadingSite")]
		public string LoadingSite { get; set; }

		[XmlElement(ElementName = "FinalDestinationInternationalSiteID")]
		public string FinalDestinationInternationalSiteID { get; set; }

		[XmlElement(ElementName = "FirstDestinationInternationalSiteID")]
		public string FirstDestinationInternationalSiteID { get; set; }

		[XmlElement(ElementName = "ShipCode")]
		public string ShipCode { get; set; }

		[XmlElement(ElementName = "ShipAgent")]
		public string ShipAgent { get; set; }

		[XmlElement(ElementName = "ShippingCompanyCode")]
		public string ShippingCompanyCode { get; set; }

		[XmlElement(ElementName = "SecurityClearence")]
		public string SecurityClearence { get; set; }
	}

	[XmlRoot(ElementName = "cargoIdentifier")]
	public class CargoIdentifier
	{

		[XmlElement(ElementName = "cargoIdentifierType")]
		public string CargoIdentifierType { get; set; }

		[XmlElement(ElementName = "cargoIdentifierKey1")]
		public string CargoIdentifierKey1 { get; set; }

		[XmlElement(ElementName = "cargoIdentifierKey2")]
		public string CargoIdentifierKey2 { get; set; }

		[XmlElement(ElementName = "cargoIdentifierKey3")]
		public string CargoIdentifierKey3 { get; set; }
	}

	[XmlRoot(ElementName = "CustomsItem")]
	public class CustomsItemClass
	{

		[XmlElement(ElementName = "CustomsItem")]
		public string CustomsItem { get; set; }
	}

	[XmlRoot(ElementName = "DangerousSubstances")]
	public class DangerousSubstances
	{

		[XmlElement(ElementName = "RiskLevel")]
		public string RiskLevel { get; set; }

		[XmlElement(ElementName = "RiskGroup")]
		public string RiskGroup { get; set; }

		[XmlElement(ElementName = "UNNumber")]
		public string UNNumber { get; set; }

		[XmlElement(ElementName = "DangerousSubstancename")]
		public string DangerousSubstancename { get; set; }
	}

	[XmlRoot(ElementName = "CargoDetails")]
	public class CargoDetails
	{

		[XmlElement(ElementName = "CargoDescription")]
		public string CargoDescription { get; set; }

		[XmlElement(ElementName = "CargoType")]
		public string CargoType { get; set; }

		[XmlElement(ElementName = "HandingCode")]
		public string HandingCode { get; set; }

		[XmlElement(ElementName = "DangerousGoodsIndication")]
		public bool DangerousGoodsIndication { get; set; }

		[XmlElement(ElementName = "CodeBreaksIndication")]
		public bool CodeBreaksIndication { get; set; }

		[XmlElement(ElementName = "DamageCode")]
		public bool DamageCode { get; set; }

		[XmlElement(ElementName = "ForeignCurrencyType")]
		public string ForeignCurrencyType { get; set; }

		[XmlElement(ElementName = "ForeignCurrencyAmount")]
		public string ForeignCurrencyAmount { get; set; }

		[XmlElement(ElementName = "GoodsValueNIS")]
		public string GoodsValueNIS { get; set; }

		[XmlElement(ElementName = "MarksNumbers")]
		public string MarksNumbers { get; set; }

		[XmlElement(ElementName = "PackageType")]
		public string PackageType { get; set; }

		[XmlElement(ElementName = "Quantity")]
		public string Quantity { get; set; }

		[XmlElement(ElementName = "WeightInPortMandatory")]
		public bool WeightInPortMandatory { get; set; }

		[XmlElement(ElementName = "Weight")]
		public string Weight { get; set; }

		[XmlElement(ElementName = "VolumeSize")]
		public string VolumeSize { get; set; }

		[XmlElement(ElementName = "LicensePlateNumber")]
		public string LicensePlateNumber { get; set; }

		[XmlElement(ElementName = "CustomsItem")]
		public CustomsItemClass CustomsItem { get; set; }

		[XmlElement(ElementName = "DangerousSubstances")]
		public DangerousSubstances DangerousSubstances { get; set; }
	}

	[XmlRoot(ElementName = "Seal")]
	public class Seal
	{

		[XmlElement(ElementName = "sealType")]
		public string SealType { get; set; }

		[XmlElement(ElementName = "sealNumber")]
		public string SealNumber { get; set; }
	}

	[XmlRoot(ElementName = "ContainerDetails")]
	public class ContainerDetails
	{

		[XmlElement(ElementName = "ContainerTypeWCO")]
		public string ContainerTypeWCO { get; set; }

		[XmlElement(ElementName = "ContainerNumber")]
		public string ContainerNumber { get; set; }

		[XmlElement(ElementName = "OwnershipCode")]
		public string OwnershipCode { get; set; }

		[XmlElement(ElementName = "CoolingActivated")]
		public bool CoolingActivated { get; set; }

		[XmlElement(ElementName = "RequiredTemperature")]
		public string RequiredTemperature { get; set; }

		[XmlElement(ElementName = "PharmaGroceryIndication")]
		public bool PharmaGroceryIndication { get; set; }

		[XmlElement(ElementName = "LeftException")]
		public string LeftException { get; set; }

		[XmlElement(ElementName = "RightException")]
		public string RightException { get; set; }

		[XmlElement(ElementName = "FrontException")]
		public string FrontException { get; set; }

		[XmlElement(ElementName = "BackException")]
		public string BackException { get; set; }

		[XmlElement(ElementName = "FullnessCode")]
		public string FullnessCode { get; set; }

		[XmlElement(ElementName = "CoolingReportingMethod")]
		public string CoolingReportingMethod { get; set; }

		[XmlElement(ElementName = "VentValue")]
		public string VentValue { get; set; }

		[XmlElement(ElementName = "HumidityPercentage")]
		public string HumidityPercentage { get; set; }

		[XmlElement(ElementName = "Co2Percentage")]
		public string Co2Percentage { get; set; }

		[XmlElement(ElementName = "O2Percentage")]
		public string O2Percentage { get; set; }

		[XmlElement(ElementName = "Seal")]
		public Seal Seal { get; set; }
	}

	[XmlRoot(ElementName = "LogitudeStorage")]
	public class LogitudeStorage
	{

		[XmlElement(ElementName = "StorageNo")]
		public string StorageNo { get; set; }

		[XmlElement(ElementName = "Id")]
		public string Id { get; set; }

		[XmlElement(ElementName = "Tenant")]
		public string Tenant { get; set; }

		[XmlElement(ElementName = "OpenDate")]
		public string OpenDate { get; set; }

		[XmlElement(ElementName = "DeclarationID")]
		public string DeclarationID { get; set; }
        public string DeclarationId { get; set; }
        [XmlElement(ElementName = "CustomStatus")]
		public string CustomStatus { get; set; }

		[XmlElement(ElementName = "General")]
		public General General { get; set; }

		[XmlElement(ElementName = "cargoIdentifier")]
		public CargoIdentifier CargoIdentifier { get; set; }

		[XmlElement(ElementName = "CargoDetails")]
		public CargoDetails CargoDetails { get; set; }

		[XmlElement(ElementName = "ContainerDetails")]
		public ContainerDetails ContainerDetails { get; set; }
        public string ExportFileNo { get; set; }
    }

	//[XmlRoot(ElementName = "LOGICUSTFILE")]
	//public class LOGICUSTFILE
	//{

	//	[XmlElement(ElementName = "LogitudeStorage")]
	//	public LogitudeStorage LogitudeStorage { get; set; }
	//}


}
