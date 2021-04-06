
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ExportStorgeDataMapping: IMapping<ExportStorgePM, ExportStorge>,IMappingEncodeBase64NVARCHARFields<ExportStorgePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         DeclarationId, 
	         ExportFileNo, 
	         OrderNo, 
	         CustomFileNo, 
	         FclLcl, 
	         Direction, 
	         TransportModeId, 
	         StorageNo, 
	         VoyageNo, 
	         StorageDate, 
	         StorageStatus, 
	         IsOpenStoarge, 
	         IsConnectedToDeclaration, 
	         OperationCode, 
	         SenderCodeID, 
	         MessageFromForm, 
	         ReplyPhoneNumeric, 
	         OperatorID, 
	         InformedParty, 
	         DeclarationNumber, 
	         DeclarationsInContainer, 
	         ExportManifestNumber, 
	         ReceivingSite, 
	         StuffingSiteType, 
	         LoadingSite, 
	         ForwarderReference, 
	         TransactionQuantity, 
	         ExportDocument, 
	         MessageContent, 
	         BookingNumber, 
	         LogisticDeliveryTypeID, 
	         CargoRows, 
	         ExporterIdentificationType, 
	         FinalDestinationInternatID, 
	         FirstDestinationInternatID, 
	         OriginAbroadSite, 
	         ExpectedPortArrivalDate, 
	         DraggedOrSupportedNumber, 
	         TruckOrTrainNumber, 
	         DriverId, 
	         ShipCode, 
	         ShipName, 
	         TransportCompany, 
	         ShipAgent, 
	         ShippingCompanyCode, 
	         SecurityClearence, 
	         TransferCargoMethodType, 
	         StorageOrDockID, 
	         ExporterName, 
	         ExporterFileNumber, 
	         PassportCountry, 
	         ExporterNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         DeclarationId, 
	         ExportFileNo, 
	         OrderNo, 
	         CustomFileNo, 
	         FclLcl, 
	         FclLclName, 
	         Direction, 
	         TransportModeId, 
	         TransportModeName, 
	         StorageNo, 
	         VoyageNo, 
	         StorageDate, 
	         StorageStatus, 
	         IsOpenStoarge, 
	         IsConnectedToDeclaration, 
	         OperationCode, 
	         SenderCodeID, 
	         MessageFromForm, 
	         ReplyPhoneNumeric, 
	         OperatorID, 
	         InformedParty, 
	         DeclarationNumber, 
	         DeclarationsInContainer, 
	         ExportManifestNumber, 
	         ReceivingSite, 
	         StuffingSiteType, 
	         LoadingSite, 
	         ForwarderReference, 
	         TransactionQuantity, 
	         ExportDocument, 
	         MessageContent, 
	         BookingNumber, 
	         LogisticDeliveryTypeID, 
	         CargoRows, 
	         ExporterIdentificationType, 
	         FinalDestinationInternatID, 
	         FirstDestinationInternatID, 
	         OriginAbroadSite, 
	         ExpectedPortArrivalDate, 
	         DraggedOrSupportedNumber, 
	         TruckOrTrainNumber, 
	         DriverId, 
	         ShipCode, 
	         ShipName, 
	         TransportCompany, 
	         ShipAgent, 
	         ShippingCompanyCode, 
	         SecurityClearence, 
	         TransferCargoMethodType, 
	         StorageOrDockID, 
	         ExporterName, 
	         ExporterFileNumber, 
	         PassportCountry, 
	         ExporterNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ExportStorgePM entityPM, ExportStorge entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
				entityPOCO.DeclarationId = entityPM.DeclarationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportFileNo))
            {
				entityPOCO.ExportFileNo = entityPM.ExportFileNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNo))
            {
				entityPOCO.OrderNo = entityPM.OrderNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomFileNo))
            {
				entityPOCO.CustomFileNo = entityPM.CustomFileNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FclLcl))
            {
				entityPOCO.FclLcl = entityPM.FclLcl;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Direction))
            {
				entityPOCO.Direction = entityPM.Direction;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageNo))
            {
				entityPOCO.StorageNo = entityPM.StorageNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoyageNo))
            {
				entityPOCO.VoyageNo = entityPM.VoyageNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageDate))
            {
				entityPOCO.StorageDate = entityPM.StorageDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageStatus))
            {
				entityPOCO.StorageStatus = entityPM.StorageStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOpenStoarge))
            {
				entityPOCO.IsOpenStoarge = entityPM.IsOpenStoarge;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConnectedToDeclaration))
            {
				entityPOCO.IsConnectedToDeclaration = entityPM.IsConnectedToDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OperationCode))
            {
				entityPOCO.OperationCode = entityPM.OperationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SenderCodeID))
            {
				entityPOCO.SenderCodeID = entityPM.SenderCodeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MessageFromForm))
            {
				entityPOCO.MessageFromForm = entityPM.MessageFromForm;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReplyPhoneNumeric))
            {
				entityPOCO.ReplyPhoneNumeric = entityPM.ReplyPhoneNumeric;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OperatorID))
            {
				entityPOCO.OperatorID = entityPM.OperatorID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InformedParty))
            {
				entityPOCO.InformedParty = entityPM.InformedParty;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationNumber))
            {
				entityPOCO.DeclarationNumber = entityPM.DeclarationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationsInContainer))
            {
				entityPOCO.DeclarationsInContainer = entityPM.DeclarationsInContainer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportManifestNumber))
            {
				entityPOCO.ExportManifestNumber = entityPM.ExportManifestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReceivingSite))
            {
				entityPOCO.ReceivingSite = entityPM.ReceivingSite;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StuffingSiteType))
            {
				entityPOCO.StuffingSiteType = entityPM.StuffingSiteType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingSite))
            {
				entityPOCO.LoadingSite = entityPM.LoadingSite;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwarderReference))
            {
				entityPOCO.ForwarderReference = entityPM.ForwarderReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransactionQuantity))
            {
				entityPOCO.TransactionQuantity = entityPM.TransactionQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportDocument))
            {
				entityPOCO.ExportDocument = entityPM.ExportDocument;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MessageContent))
            {
				entityPOCO.MessageContent = entityPM.MessageContent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingNumber))
            {
				entityPOCO.BookingNumber = entityPM.BookingNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LogisticDeliveryTypeID))
            {
				entityPOCO.LogisticDeliveryTypeID = entityPM.LogisticDeliveryTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoRows))
            {
				entityPOCO.CargoRows = entityPM.CargoRows;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterIdentificationType))
            {
				entityPOCO.ExporterIdentificationType = entityPM.ExporterIdentificationType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDestinationInternatID))
            {
				entityPOCO.FinalDestinationInternatID = entityPM.FinalDestinationInternatID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstDestinationInternatID))
            {
				entityPOCO.FirstDestinationInternatID = entityPM.FirstDestinationInternatID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginAbroadSite))
            {
				entityPOCO.OriginAbroadSite = entityPM.OriginAbroadSite;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpectedPortArrivalDate))
            {
				entityPOCO.ExpectedPortArrivalDate = entityPM.ExpectedPortArrivalDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DraggedOrSupportedNumber))
            {
				entityPOCO.DraggedOrSupportedNumber = entityPM.DraggedOrSupportedNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TruckOrTrainNumber))
            {
				entityPOCO.TruckOrTrainNumber = entityPM.TruckOrTrainNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DriverId))
            {
				entityPOCO.DriverId = entityPM.DriverId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipCode))
            {
				entityPOCO.ShipCode = entityPM.ShipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipName))
            {
				entityPOCO.ShipName = entityPM.ShipName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportCompany))
            {
				entityPOCO.TransportCompany = entityPM.TransportCompany;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipAgent))
            {
				entityPOCO.ShipAgent = entityPM.ShipAgent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShippingCompanyCode))
            {
				entityPOCO.ShippingCompanyCode = entityPM.ShippingCompanyCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecurityClearence))
            {
				entityPOCO.SecurityClearence = entityPM.SecurityClearence;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferCargoMethodType))
            {
				entityPOCO.TransferCargoMethodType = entityPM.TransferCargoMethodType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageOrDockID))
            {
				entityPOCO.StorageOrDockID = entityPM.StorageOrDockID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterName))
            {
				entityPOCO.ExporterName = entityPM.ExporterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterFileNumber))
            {
				entityPOCO.ExporterFileNumber = entityPM.ExporterFileNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportCountry))
            {
				entityPOCO.PassportCountry = entityPM.PassportCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterNumber))
            {
				entityPOCO.ExporterNumber = entityPM.ExporterNumber;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ExportStorgePM entityPM, ExportStorge entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportFileNo))
            {
					entityPM.ExportFileNo = entityPOCO.ExportFileNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderNo))
            {
					entityPM.OrderNo = entityPOCO.OrderNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomFileNo))
            {
					entityPM.CustomFileNo = entityPOCO.CustomFileNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FclLcl))
            {
					entityPM.FclLcl = entityPOCO.FclLcl;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Direction))
            {
					entityPM.Direction = entityPOCO.Direction;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageNo))
            {
					entityPM.StorageNo = entityPOCO.StorageNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VoyageNo))
            {
					entityPM.VoyageNo = entityPOCO.VoyageNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageDate))
            {
					entityPM.StorageDate = entityPOCO.StorageDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageStatus))
            {
					entityPM.StorageStatus = entityPOCO.StorageStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsOpenStoarge))
            {
					entityPM.IsOpenStoarge = entityPOCO.IsOpenStoarge;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConnectedToDeclaration))
            {
					entityPM.IsConnectedToDeclaration = entityPOCO.IsConnectedToDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OperationCode))
            {
					entityPM.OperationCode = entityPOCO.OperationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SenderCodeID))
            {
					entityPM.SenderCodeID = entityPOCO.SenderCodeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MessageFromForm))
            {
					entityPM.MessageFromForm = entityPOCO.MessageFromForm;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReplyPhoneNumeric))
            {
					entityPM.ReplyPhoneNumeric = entityPOCO.ReplyPhoneNumeric;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OperatorID))
            {
					entityPM.OperatorID = entityPOCO.OperatorID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InformedParty))
            {
					entityPM.InformedParty = entityPOCO.InformedParty;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationNumber))
            {
					entityPM.DeclarationNumber = entityPOCO.DeclarationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationsInContainer))
            {
					entityPM.DeclarationsInContainer = entityPOCO.DeclarationsInContainer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportManifestNumber))
            {
					entityPM.ExportManifestNumber = entityPOCO.ExportManifestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReceivingSite))
            {
					entityPM.ReceivingSite = entityPOCO.ReceivingSite;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StuffingSiteType))
            {
					entityPM.StuffingSiteType = entityPOCO.StuffingSiteType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LoadingSite))
            {
					entityPM.LoadingSite = entityPOCO.LoadingSite;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwarderReference))
            {
					entityPM.ForwarderReference = entityPOCO.ForwarderReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransactionQuantity))
            {
					entityPM.TransactionQuantity = entityPOCO.TransactionQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportDocument))
            {
					entityPM.ExportDocument = entityPOCO.ExportDocument;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MessageContent))
            {
					entityPM.MessageContent = entityPOCO.MessageContent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingNumber))
            {
					entityPM.BookingNumber = entityPOCO.BookingNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LogisticDeliveryTypeID))
            {
					entityPM.LogisticDeliveryTypeID = entityPOCO.LogisticDeliveryTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoRows))
            {
					entityPM.CargoRows = entityPOCO.CargoRows;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterIdentificationType))
            {
					entityPM.ExporterIdentificationType = entityPOCO.ExporterIdentificationType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalDestinationInternatID))
            {
					entityPM.FinalDestinationInternatID = entityPOCO.FinalDestinationInternatID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstDestinationInternatID))
            {
					entityPM.FirstDestinationInternatID = entityPOCO.FirstDestinationInternatID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginAbroadSite))
            {
					entityPM.OriginAbroadSite = entityPOCO.OriginAbroadSite;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpectedPortArrivalDate))
            {
					entityPM.ExpectedPortArrivalDate = entityPOCO.ExpectedPortArrivalDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DraggedOrSupportedNumber))
            {
					entityPM.DraggedOrSupportedNumber = entityPOCO.DraggedOrSupportedNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TruckOrTrainNumber))
            {
					entityPM.TruckOrTrainNumber = entityPOCO.TruckOrTrainNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DriverId))
            {
					entityPM.DriverId = entityPOCO.DriverId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipCode))
            {
					entityPM.ShipCode = entityPOCO.ShipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipName))
            {
					entityPM.ShipName = entityPOCO.ShipName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportCompany))
            {
					entityPM.TransportCompany = entityPOCO.TransportCompany;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipAgent))
            {
					entityPM.ShipAgent = entityPOCO.ShipAgent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShippingCompanyCode))
            {
					entityPM.ShippingCompanyCode = entityPOCO.ShippingCompanyCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecurityClearence))
            {
					entityPM.SecurityClearence = entityPOCO.SecurityClearence;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferCargoMethodType))
            {
					entityPM.TransferCargoMethodType = entityPOCO.TransferCargoMethodType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageOrDockID))
            {
					entityPM.StorageOrDockID = entityPOCO.StorageOrDockID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterName))
            {
					entityPM.ExporterName = entityPOCO.ExporterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterFileNumber))
            {
					entityPM.ExporterFileNumber = entityPOCO.ExporterFileNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PassportCountry))
            {
					entityPM.PassportCountry = entityPOCO.PassportCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterNumber))
            {
					entityPM.ExporterNumber = entityPOCO.ExporterNumber;
            }

		}

		public void PMToOldPM(ExportStorgePM entityPM, ExportStorgePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
                oldEntityPM.DeclarationId = entityPM.DeclarationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportFileNo))
            {
                oldEntityPM.ExportFileNo = entityPM.ExportFileNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNo))
            {
                oldEntityPM.OrderNo = entityPM.OrderNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomFileNo))
            {
                oldEntityPM.CustomFileNo = entityPM.CustomFileNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FclLcl))
            {
                oldEntityPM.FclLcl = entityPM.FclLcl;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Direction))
            {
                oldEntityPM.Direction = entityPM.Direction;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageNo))
            {
                oldEntityPM.StorageNo = entityPM.StorageNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VoyageNo))
            {
                oldEntityPM.VoyageNo = entityPM.VoyageNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageDate))
            {
                oldEntityPM.StorageDate = entityPM.StorageDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageStatus))
            {
                oldEntityPM.StorageStatus = entityPM.StorageStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOpenStoarge))
            {
                oldEntityPM.IsOpenStoarge = entityPM.IsOpenStoarge;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConnectedToDeclaration))
            {
                oldEntityPM.IsConnectedToDeclaration = entityPM.IsConnectedToDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OperationCode))
            {
                oldEntityPM.OperationCode = entityPM.OperationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SenderCodeID))
            {
                oldEntityPM.SenderCodeID = entityPM.SenderCodeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MessageFromForm))
            {
                oldEntityPM.MessageFromForm = entityPM.MessageFromForm;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReplyPhoneNumeric))
            {
                oldEntityPM.ReplyPhoneNumeric = entityPM.ReplyPhoneNumeric;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OperatorID))
            {
                oldEntityPM.OperatorID = entityPM.OperatorID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InformedParty))
            {
                oldEntityPM.InformedParty = entityPM.InformedParty;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationNumber))
            {
                oldEntityPM.DeclarationNumber = entityPM.DeclarationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationsInContainer))
            {
                oldEntityPM.DeclarationsInContainer = entityPM.DeclarationsInContainer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportManifestNumber))
            {
                oldEntityPM.ExportManifestNumber = entityPM.ExportManifestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReceivingSite))
            {
                oldEntityPM.ReceivingSite = entityPM.ReceivingSite;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StuffingSiteType))
            {
                oldEntityPM.StuffingSiteType = entityPM.StuffingSiteType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingSite))
            {
                oldEntityPM.LoadingSite = entityPM.LoadingSite;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwarderReference))
            {
                oldEntityPM.ForwarderReference = entityPM.ForwarderReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransactionQuantity))
            {
                oldEntityPM.TransactionQuantity = entityPM.TransactionQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportDocument))
            {
                oldEntityPM.ExportDocument = entityPM.ExportDocument;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MessageContent))
            {
                oldEntityPM.MessageContent = entityPM.MessageContent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingNumber))
            {
                oldEntityPM.BookingNumber = entityPM.BookingNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LogisticDeliveryTypeID))
            {
                oldEntityPM.LogisticDeliveryTypeID = entityPM.LogisticDeliveryTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoRows))
            {
                oldEntityPM.CargoRows = entityPM.CargoRows;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterIdentificationType))
            {
                oldEntityPM.ExporterIdentificationType = entityPM.ExporterIdentificationType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalDestinationInternatID))
            {
                oldEntityPM.FinalDestinationInternatID = entityPM.FinalDestinationInternatID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstDestinationInternatID))
            {
                oldEntityPM.FirstDestinationInternatID = entityPM.FirstDestinationInternatID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginAbroadSite))
            {
                oldEntityPM.OriginAbroadSite = entityPM.OriginAbroadSite;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpectedPortArrivalDate))
            {
                oldEntityPM.ExpectedPortArrivalDate = entityPM.ExpectedPortArrivalDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DraggedOrSupportedNumber))
            {
                oldEntityPM.DraggedOrSupportedNumber = entityPM.DraggedOrSupportedNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TruckOrTrainNumber))
            {
                oldEntityPM.TruckOrTrainNumber = entityPM.TruckOrTrainNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DriverId))
            {
                oldEntityPM.DriverId = entityPM.DriverId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipCode))
            {
                oldEntityPM.ShipCode = entityPM.ShipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipName))
            {
                oldEntityPM.ShipName = entityPM.ShipName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportCompany))
            {
                oldEntityPM.TransportCompany = entityPM.TransportCompany;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipAgent))
            {
                oldEntityPM.ShipAgent = entityPM.ShipAgent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShippingCompanyCode))
            {
                oldEntityPM.ShippingCompanyCode = entityPM.ShippingCompanyCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecurityClearence))
            {
                oldEntityPM.SecurityClearence = entityPM.SecurityClearence;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferCargoMethodType))
            {
                oldEntityPM.TransferCargoMethodType = entityPM.TransferCargoMethodType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageOrDockID))
            {
                oldEntityPM.StorageOrDockID = entityPM.StorageOrDockID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterName))
            {
                oldEntityPM.ExporterName = entityPM.ExporterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterFileNumber))
            {
                oldEntityPM.ExporterFileNumber = entityPM.ExporterFileNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PassportCountry))
            {
                oldEntityPM.PassportCountry = entityPM.PassportCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterNumber))
            {
                oldEntityPM.ExporterNumber = entityPM.ExporterNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ExportStorgePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.MessageContent)) //T4 find type == nText 
            {
                entityPM.MessageContent = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.MessageContent));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExpectedPortArrivalDate)) //T4 find type == nText 
            {
                entityPM.ExpectedPortArrivalDate = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExpectedPortArrivalDate));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DraggedOrSupportedNumber)) //T4 find type == nText 
            {
                entityPM.DraggedOrSupportedNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DraggedOrSupportedNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ShipName)) //T4 find type == nText 
            {
                entityPM.ShipName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ShipName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.StorageOrDockID)) //T4 find type == nText 
            {
                entityPM.StorageOrDockID = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StorageOrDockID));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExporterName)) //T4 find type == nText 
            {
                entityPM.ExporterName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExporterName));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(ExportStorgePM entityPM, ExportStorge entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 