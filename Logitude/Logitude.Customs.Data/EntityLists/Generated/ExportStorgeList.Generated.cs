using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class ExportStorgeList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string ExportFileNo  { get; set; }
       [DataMember]
       public string OrderNo  { get; set; }
       [DataMember]
       public string CustomFileNo  { get; set; }
       [DataMember]
       public string FclLcl  { get; set; }
       [DataMember]
       public string FclLclName  { get; set; }
       [DataMember]
       public string Direction  { get; set; }
       [DataMember]
       public string TransportModeName  { get; set; }
       [DataMember]
       public int StorageNo  { get; set; }
       [DataMember]
       public double VoyageNo  { get; set; }
       [DataMember]
       public DateTime StorageDate  { get; set; }
       [DataMember]
       public string StorageStatus  { get; set; }
       [DataMember]
       public bool IsOpenStoarge  { get; set; }
       [DataMember]
       public string OperationCode  { get; set; }
       [DataMember]
       public string SenderCodeID  { get; set; }
       [DataMember]
       public int MessageFromForm  { get; set; }
       [DataMember]
       public double ReplyPhoneNumeric  { get; set; }
       [DataMember]
       public double OperatorID  { get; set; }
       [DataMember]
       public string InformedParty  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public int DeclarationsInContainer  { get; set; }
       [DataMember]
       public int ExportManifestNumber  { get; set; }
       [DataMember]
       public string ReceivingSite  { get; set; }
       [DataMember]
       public string StuffingSiteType  { get; set; }
       [DataMember]
       public string LoadingSite  { get; set; }
       [DataMember]
       public string ForwarderReference  { get; set; }
       [DataMember]
       public decimal TransactionQuantity  { get; set; }
       [DataMember]
       public string ExportDocument  { get; set; }
       [DataMember]
       public string MessageContent  { get; set; }
       [DataMember]
       public string BookingNumber  { get; set; }
       [DataMember]
       public string LogisticDeliveryTypeID  { get; set; }
       [DataMember]
       public decimal CargoRows  { get; set; }
       [DataMember]
       public string ExporterIdentificationType  { get; set; }
       [DataMember]
       public string FinalDestinationInternatID  { get; set; }
       [DataMember]
       public string FirstDestinationInternatID  { get; set; }
       [DataMember]
       public string OriginAbroadSite  { get; set; }
       [DataMember]
       public string ExpectedPortArrivalDate  { get; set; }
       [DataMember]
       public string DraggedOrSupportedNumber  { get; set; }
       [DataMember]
       public string TruckOrTrainNumber  { get; set; }
       [DataMember]
       public string DriverId  { get; set; }
       [DataMember]
       public string ShipCode  { get; set; }
       [DataMember]
       public string ShipName  { get; set; }
       [DataMember]
       public string TransportCompany  { get; set; }
       [DataMember]
       public string ShipAgent  { get; set; }
       [DataMember]
       public string ShippingCompanyCode  { get; set; }
       [DataMember]
       public string SecurityClearence  { get; set; }
       [DataMember]
       public string TransferCargoMethodType  { get; set; }
       [DataMember]
       public string StorageOrDockID  { get; set; }
       [DataMember]
       public string ExporterName  { get; set; }
       [DataMember]
       public string VesselAndVoyage  { get; set; }
       [DataMember]
       public DateTime OpenDate  { get; set; }
       [DataMember]
       public string DriverName  { get; set; }
       [DataMember]
       public string Customs  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public string Manifest  { get; set; }
       [DataMember]
       public string SecondCargoID  { get; set; }
       [DataMember]
       public string ThirdCargoID  { get; set; }
       [DataMember]
       public string CargoDescription  { get; set; }
       [DataMember]
       public string CargoType  { get; set; }
       [DataMember]
       public string HandlingCode  { get; set; }
       [DataMember]
       public decimal DangerousGoodsIndication  { get; set; }
       [DataMember]
       public decimal CodeBreaksIndication  { get; set; }
       [DataMember]
       public decimal DamageCode  { get; set; }
       [DataMember]
       public string ForeignCurrencyType  { get; set; }
       [DataMember]
       public decimal ForeignCurrencyAmoun  { get; set; }
       [DataMember]
       public decimal GoodsValueNIS  { get; set; }
       [DataMember]
       public string PackageType  { get; set; }
       [DataMember]
       public decimal Quantity  { get; set; }
       [DataMember]
       public string MarksNumbers  { get; set; }
       [DataMember]
       public decimal WeightInPortMandatory  { get; set; }
       [DataMember]
       public decimal Weight  { get; set; }
       [DataMember]
       public decimal VolumeSize  { get; set; }
       [DataMember]
       public string LicensePlateNumber  { get; set; }
       [DataMember]
       public string ContainerNumber  { get; set; }
       [DataMember]
       public string ExporterRef  { get; set; }
       [DataMember]
       public string Hatara  { get; set; }
       [DataMember]
       public string CustomStatus  { get; set; }
       [DataMember]
       public string ShipAgentName  { get; set; }
       [DataMember]
       public string LoadingSiteCode  { get; set; }
       [DataMember]
       public string FinalDestinationCode  { get; set; }
       [DataMember]
       public string Routing  { get; set; }
   }

}
	 