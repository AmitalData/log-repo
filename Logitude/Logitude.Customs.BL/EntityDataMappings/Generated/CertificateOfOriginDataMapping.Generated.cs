
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
   
   public partial class CertificateOfOriginDataMapping: IMapping<CertificateOfOriginPM, CertificateOfOrigin>,IMappingEncodeBase64NVARCHARFields<CertificateOfOriginPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         Counter, 
	         CooTypeCode, 
	         RequestReasonCode, 
	         COONumber, 
	         COONumberToCancel, 
	         ReplacementReason, 
	         DeclarationId, 
	         ExporterVat, 
	         ExporterName, 
	         ExporterAddress, 
	         ExporterCountry, 
	         TradeAgreementCountry1, 
	         TradeAgreementCountry2, 
	         TradeAgreementGroupOfCountries, 
	         ConsigneeName, 
	         ConsigneeAddress, 
	         ConsigneeCountry, 
	         ConsigneeRemarks, 
	         IsConsigneeForPrint, 
	         OriginCountry, 
	         OriginGroupOfCountry, 
	         DestinationCountry, 
	         DestinationGroupOfCountries, 
	         Transport, 
	         PortOfShipment, 
	         IsCumulation, 
	         CumulationCountry, 
	         CumulationGroupOfCountries, 
	         PlaceOfManufacture, 
	         ZipCodeOfManufacture, 
	         Observations, 
	         IsExportDecForPrint, 
	         IsUnitedInvoices, 
	         CustomsHouse, 
	         IssuingCountry, 
	         CityOfDeclaration, 
	         CountryOfDeclaration, 
	         DateOfDeclaration, 
	         IsDeclaredByManufacture, 
	         IsDeclaredByExporter, 
	         IsAttachedList, 
	         InsufficentWorkingInd, 
	         InsufficentWorkingText, 
	         NonExportDate, 
	         NonExportCountry, 
	         NonImportBillOfLadingNum, 
	         NonExportPort, 
	         NonImportDate, 
	         NonExportBillOfLadingNum, 
	         NonTransirCountry, 
	         NonPortOfEntrance, 
	         NonExpectedExitDate, 
	         NonExitPort, 
	         NonGoodsDescription, 
	         NonDeclaringCompany, 
	         NonDeclaringPerson, 
	         NonDeclaringPosition, 
	         NonManifestNum, 
	         ErrXml, 
	         CooStatusCode, 
	         FeedbackRemark, 
	         RejectCancelReason, 
	         IssueDateIfReleased, 
	         QueryUrl, 
	         CooPdf, 
	         CoodPdf1, 
	         OpenByUser, 
	         IsSubmitted, 
	         UpdateDeclaration,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         Counter, 
	         CooTypeCode, 
	         RequestReasonCode, 
	         COONumber, 
	         COONumberToCancel, 
	         ReplacementReason, 
	         DeclarationId, 
	         ExporterVat, 
	         ExporterName, 
	         ExporterAddress, 
	         ExporterCountry, 
	         TradeAgreementCountry1, 
	         TradeAgreementCountry2, 
	         TradeAgreementGroupOfCountries, 
	         ConsigneeName, 
	         ConsigneeAddress, 
	         ConsigneeCountry, 
	         ConsigneeRemarks, 
	         IsConsigneeForPrint, 
	         OriginCountry, 
	         OriginGroupOfCountry, 
	         DestinationCountry, 
	         DestinationGroupOfCountries, 
	         Transport, 
	         PortOfShipment, 
	         IsCumulation, 
	         CumulationCountry, 
	         CumulationGroupOfCountries, 
	         PlaceOfManufacture, 
	         ZipCodeOfManufacture, 
	         Observations, 
	         IsExportDecForPrint, 
	         IsUnitedInvoices, 
	         CustomsHouse, 
	         IssuingCountry, 
	         CityOfDeclaration, 
	         CountryOfDeclaration, 
	         DateOfDeclaration, 
	         IsDeclaredByManufacture, 
	         IsDeclaredByExporter, 
	         IsAttachedList, 
	         InsufficentWorkingInd, 
	         InsufficentWorkingText, 
	         NonExportDate, 
	         NonExportCountry, 
	         NonImportBillOfLadingNum, 
	         NonExportPort, 
	         NonImportDate, 
	         NonExportBillOfLadingNum, 
	         NonTransirCountry, 
	         NonPortOfEntrance, 
	         NonExpectedExitDate, 
	         NonExitPort, 
	         NonGoodsDescription, 
	         NonDeclaringCompany, 
	         NonDeclaringPerson, 
	         NonDeclaringPosition, 
	         NonManifestNum, 
	         ErrXml, 
	         CooStatusCode, 
	         FeedbackRemark, 
	         RejectCancelReason, 
	         IssueDateIfReleased, 
	         QueryUrl, 
	         CooPdf, 
	         CoodPdf1, 
	         OpenByUser, 
	         IsSubmitted, 
	         OpenByUserName, 
	         CooTypeCodeName, 
	         RequestReasonCodeName, 
	         CooStatusCodeName, 
	         ListCounter, 
	         UpdateDeclaration,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CertificateOfOriginPM entityPM, CertificateOfOrigin entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Counter))
            {
				entityPOCO.Counter = entityPM.Counter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooTypeCode))
            {
				entityPOCO.CooTypeCode = entityPM.CooTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestReasonCode))
            {
				entityPOCO.RequestReasonCode = entityPM.RequestReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.COONumber))
            {
				entityPOCO.COONumber = entityPM.COONumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.COONumberToCancel))
            {
				entityPOCO.COONumberToCancel = entityPM.COONumberToCancel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReplacementReason))
            {
				entityPOCO.ReplacementReason = entityPM.ReplacementReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
				entityPOCO.DeclarationId = entityPM.DeclarationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterVat))
            {
				entityPOCO.ExporterVat = entityPM.ExporterVat;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterName))
            {
				entityPOCO.ExporterName = entityPM.ExporterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterAddress))
            {
				entityPOCO.ExporterAddress = entityPM.ExporterAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterCountry))
            {
				entityPOCO.ExporterCountry = entityPM.ExporterCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementCountry1))
            {
				entityPOCO.TradeAgreementCountry1 = entityPM.TradeAgreementCountry1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementCountry2))
            {
				entityPOCO.TradeAgreementCountry2 = entityPM.TradeAgreementCountry2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementGroupOfCountries))
            {
				entityPOCO.TradeAgreementGroupOfCountries = entityPM.TradeAgreementGroupOfCountries;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
				entityPOCO.ConsigneeName = entityPM.ConsigneeName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeAddress))
            {
				entityPOCO.ConsigneeAddress = entityPM.ConsigneeAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeCountry))
            {
				entityPOCO.ConsigneeCountry = entityPM.ConsigneeCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeRemarks))
            {
				entityPOCO.ConsigneeRemarks = entityPM.ConsigneeRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsigneeForPrint))
            {
				entityPOCO.IsConsigneeForPrint = entityPM.IsConsigneeForPrint;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountry))
            {
				entityPOCO.OriginCountry = entityPM.OriginCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginGroupOfCountry))
            {
				entityPOCO.OriginGroupOfCountry = entityPM.OriginGroupOfCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationCountry))
            {
				entityPOCO.DestinationCountry = entityPM.DestinationCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationGroupOfCountries))
            {
				entityPOCO.DestinationGroupOfCountries = entityPM.DestinationGroupOfCountries;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transport))
            {
				entityPOCO.Transport = entityPM.Transport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PortOfShipment))
            {
				entityPOCO.PortOfShipment = entityPM.PortOfShipment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCumulation))
            {
				entityPOCO.IsCumulation = entityPM.IsCumulation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CumulationCountry))
            {
				entityPOCO.CumulationCountry = entityPM.CumulationCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CumulationGroupOfCountries))
            {
				entityPOCO.CumulationGroupOfCountries = entityPM.CumulationGroupOfCountries;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PlaceOfManufacture))
            {
				entityPOCO.PlaceOfManufacture = entityPM.PlaceOfManufacture;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ZipCodeOfManufacture))
            {
				entityPOCO.ZipCodeOfManufacture = entityPM.ZipCodeOfManufacture;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Observations))
            {
				entityPOCO.Observations = entityPM.Observations;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExportDecForPrint))
            {
				entityPOCO.IsExportDecForPrint = entityPM.IsExportDecForPrint;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsUnitedInvoices))
            {
				entityPOCO.IsUnitedInvoices = entityPM.IsUnitedInvoices;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsHouse))
            {
				entityPOCO.CustomsHouse = entityPM.CustomsHouse;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCountry))
            {
				entityPOCO.IssuingCountry = entityPM.IssuingCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CityOfDeclaration))
            {
				entityPOCO.CityOfDeclaration = entityPM.CityOfDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryOfDeclaration))
            {
				entityPOCO.CountryOfDeclaration = entityPM.CountryOfDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DateOfDeclaration))
            {
				entityPOCO.DateOfDeclaration = entityPM.DateOfDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDeclaredByManufacture))
            {
				entityPOCO.IsDeclaredByManufacture = entityPM.IsDeclaredByManufacture;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDeclaredByExporter))
            {
				entityPOCO.IsDeclaredByExporter = entityPM.IsDeclaredByExporter;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAttachedList))
            {
				entityPOCO.IsAttachedList = entityPM.IsAttachedList;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsufficentWorkingInd))
            {
				entityPOCO.InsufficentWorkingInd = entityPM.InsufficentWorkingInd;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsufficentWorkingText))
            {
				entityPOCO.InsufficentWorkingText = entityPM.InsufficentWorkingText;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportDate))
            {
				entityPOCO.NonExportDate = entityPM.NonExportDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportCountry))
            {
				entityPOCO.NonExportCountry = entityPM.NonExportCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonImportBillOfLadingNum))
            {
				entityPOCO.NonImportBillOfLadingNum = entityPM.NonImportBillOfLadingNum;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportPort))
            {
				entityPOCO.NonExportPort = entityPM.NonExportPort;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonImportDate))
            {
				entityPOCO.NonImportDate = entityPM.NonImportDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportBillOfLadingNum))
            {
				entityPOCO.NonExportBillOfLadingNum = entityPM.NonExportBillOfLadingNum;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonTransirCountry))
            {
				entityPOCO.NonTransirCountry = entityPM.NonTransirCountry;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonPortOfEntrance))
            {
				entityPOCO.NonPortOfEntrance = entityPM.NonPortOfEntrance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExpectedExitDate))
            {
				entityPOCO.NonExpectedExitDate = entityPM.NonExpectedExitDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExitPort))
            {
				entityPOCO.NonExitPort = entityPM.NonExitPort;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonGoodsDescription))
            {
				entityPOCO.NonGoodsDescription = entityPM.NonGoodsDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonDeclaringCompany))
            {
				entityPOCO.NonDeclaringCompany = entityPM.NonDeclaringCompany;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonDeclaringPerson))
            {
				entityPOCO.NonDeclaringPerson = entityPM.NonDeclaringPerson;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonDeclaringPosition))
            {
				entityPOCO.NonDeclaringPosition = entityPM.NonDeclaringPosition;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonManifestNum))
            {
				entityPOCO.NonManifestNum = entityPM.NonManifestNum;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrXml))
            {
				entityPOCO.ErrXml = entityPM.ErrXml;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooStatusCode))
            {
				entityPOCO.CooStatusCode = entityPM.CooStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FeedbackRemark))
            {
				entityPOCO.FeedbackRemark = entityPM.FeedbackRemark;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RejectCancelReason))
            {
				entityPOCO.RejectCancelReason = entityPM.RejectCancelReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssueDateIfReleased))
            {
				entityPOCO.IssueDateIfReleased = entityPM.IssueDateIfReleased;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueryUrl))
            {
				entityPOCO.QueryUrl = entityPM.QueryUrl;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooPdf))
            {
				entityPOCO.CooPdf = entityPM.CooPdf;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CoodPdf1))
            {
				entityPOCO.CoodPdf1 = entityPM.CoodPdf1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenByUser))
            {
				entityPOCO.OpenByUser = entityPM.OpenByUser;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSubmitted))
            {
				entityPOCO.IsSubmitted = entityPM.IsSubmitted;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDeclaration))
            {
				entityPOCO.UpdateDeclaration = entityPM.UpdateDeclaration;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CertificateOfOriginPM entityPM, CertificateOfOrigin entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Counter))
            {
					entityPM.Counter = entityPOCO.Counter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CooTypeCode))
            {
					entityPM.CooTypeCode = entityPOCO.CooTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestReasonCode))
            {
					entityPM.RequestReasonCode = entityPOCO.RequestReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.COONumber))
            {
					entityPM.COONumber = entityPOCO.COONumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.COONumberToCancel))
            {
					entityPM.COONumberToCancel = entityPOCO.COONumberToCancel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReplacementReason))
            {
					entityPM.ReplacementReason = entityPOCO.ReplacementReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterVat))
            {
					entityPM.ExporterVat = entityPOCO.ExporterVat;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterName))
            {
					entityPM.ExporterName = entityPOCO.ExporterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterAddress))
            {
					entityPM.ExporterAddress = entityPOCO.ExporterAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterCountry))
            {
					entityPM.ExporterCountry = entityPOCO.ExporterCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementCountry1))
            {
					entityPM.TradeAgreementCountry1 = entityPOCO.TradeAgreementCountry1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementCountry2))
            {
					entityPM.TradeAgreementCountry2 = entityPOCO.TradeAgreementCountry2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementGroupOfCountries))
            {
					entityPM.TradeAgreementGroupOfCountries = entityPOCO.TradeAgreementGroupOfCountries;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeName))
            {
					entityPM.ConsigneeName = entityPOCO.ConsigneeName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeAddress))
            {
					entityPM.ConsigneeAddress = entityPOCO.ConsigneeAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeCountry))
            {
					entityPM.ConsigneeCountry = entityPOCO.ConsigneeCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeRemarks))
            {
					entityPM.ConsigneeRemarks = entityPOCO.ConsigneeRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConsigneeForPrint))
            {
					entityPM.IsConsigneeForPrint = entityPOCO.IsConsigneeForPrint;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginCountry))
            {
					entityPM.OriginCountry = entityPOCO.OriginCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginGroupOfCountry))
            {
					entityPM.OriginGroupOfCountry = entityPOCO.OriginGroupOfCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationCountry))
            {
					entityPM.DestinationCountry = entityPOCO.DestinationCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationGroupOfCountries))
            {
					entityPM.DestinationGroupOfCountries = entityPOCO.DestinationGroupOfCountries;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transport))
            {
					entityPM.Transport = entityPOCO.Transport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PortOfShipment))
            {
					entityPM.PortOfShipment = entityPOCO.PortOfShipment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCumulation))
            {
					entityPM.IsCumulation = entityPOCO.IsCumulation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CumulationCountry))
            {
					entityPM.CumulationCountry = entityPOCO.CumulationCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CumulationGroupOfCountries))
            {
					entityPM.CumulationGroupOfCountries = entityPOCO.CumulationGroupOfCountries;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PlaceOfManufacture))
            {
					entityPM.PlaceOfManufacture = entityPOCO.PlaceOfManufacture;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ZipCodeOfManufacture))
            {
					entityPM.ZipCodeOfManufacture = entityPOCO.ZipCodeOfManufacture;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Observations))
            {
					entityPM.Observations = entityPOCO.Observations;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExportDecForPrint))
            {
					entityPM.IsExportDecForPrint = entityPOCO.IsExportDecForPrint;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsUnitedInvoices))
            {
					entityPM.IsUnitedInvoices = entityPOCO.IsUnitedInvoices;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsHouse))
            {
					entityPM.CustomsHouse = entityPOCO.CustomsHouse;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssuingCountry))
            {
					entityPM.IssuingCountry = entityPOCO.IssuingCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CityOfDeclaration))
            {
					entityPM.CityOfDeclaration = entityPOCO.CityOfDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryOfDeclaration))
            {
					entityPM.CountryOfDeclaration = entityPOCO.CountryOfDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DateOfDeclaration))
            {
					entityPM.DateOfDeclaration = entityPOCO.DateOfDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDeclaredByManufacture))
            {
					entityPM.IsDeclaredByManufacture = entityPOCO.IsDeclaredByManufacture;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDeclaredByExporter))
            {
					entityPM.IsDeclaredByExporter = entityPOCO.IsDeclaredByExporter;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAttachedList))
            {
					entityPM.IsAttachedList = entityPOCO.IsAttachedList;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InsufficentWorkingInd))
            {
					entityPM.InsufficentWorkingInd = entityPOCO.InsufficentWorkingInd;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InsufficentWorkingText))
            {
					entityPM.InsufficentWorkingText = entityPOCO.InsufficentWorkingText;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonExportDate))
            {
					entityPM.NonExportDate = entityPOCO.NonExportDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonExportCountry))
            {
					entityPM.NonExportCountry = entityPOCO.NonExportCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonImportBillOfLadingNum))
            {
					entityPM.NonImportBillOfLadingNum = entityPOCO.NonImportBillOfLadingNum;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonExportPort))
            {
					entityPM.NonExportPort = entityPOCO.NonExportPort;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonImportDate))
            {
					entityPM.NonImportDate = entityPOCO.NonImportDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonExportBillOfLadingNum))
            {
					entityPM.NonExportBillOfLadingNum = entityPOCO.NonExportBillOfLadingNum;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonTransirCountry))
            {
					entityPM.NonTransirCountry = entityPOCO.NonTransirCountry;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonPortOfEntrance))
            {
					entityPM.NonPortOfEntrance = entityPOCO.NonPortOfEntrance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonExpectedExitDate))
            {
					entityPM.NonExpectedExitDate = entityPOCO.NonExpectedExitDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonExitPort))
            {
					entityPM.NonExitPort = entityPOCO.NonExitPort;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonGoodsDescription))
            {
					entityPM.NonGoodsDescription = entityPOCO.NonGoodsDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonDeclaringCompany))
            {
					entityPM.NonDeclaringCompany = entityPOCO.NonDeclaringCompany;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonDeclaringPerson))
            {
					entityPM.NonDeclaringPerson = entityPOCO.NonDeclaringPerson;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonDeclaringPosition))
            {
					entityPM.NonDeclaringPosition = entityPOCO.NonDeclaringPosition;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NonManifestNum))
            {
					entityPM.NonManifestNum = entityPOCO.NonManifestNum;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrXml))
            {
					entityPM.ErrXml = entityPOCO.ErrXml;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CooStatusCode))
            {
					entityPM.CooStatusCode = entityPOCO.CooStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FeedbackRemark))
            {
					entityPM.FeedbackRemark = entityPOCO.FeedbackRemark;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RejectCancelReason))
            {
					entityPM.RejectCancelReason = entityPOCO.RejectCancelReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssueDateIfReleased))
            {
					entityPM.IssueDateIfReleased = entityPOCO.IssueDateIfReleased;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QueryUrl))
            {
					entityPM.QueryUrl = entityPOCO.QueryUrl;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CooPdf))
            {
					entityPM.CooPdf = entityPOCO.CooPdf;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CoodPdf1))
            {
					entityPM.CoodPdf1 = entityPOCO.CoodPdf1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenByUser))
            {
					entityPM.OpenByUser = entityPOCO.OpenByUser;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSubmitted))
            {
					entityPM.IsSubmitted = entityPOCO.IsSubmitted;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDeclaration))
            {
					entityPM.UpdateDeclaration = entityPOCO.UpdateDeclaration;
            }

		}

		public void PMToOldPM(CertificateOfOriginPM entityPM, CertificateOfOriginPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Counter))
            {
                oldEntityPM.Counter = entityPM.Counter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooTypeCode))
            {
                oldEntityPM.CooTypeCode = entityPM.CooTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestReasonCode))
            {
                oldEntityPM.RequestReasonCode = entityPM.RequestReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.COONumber))
            {
                oldEntityPM.COONumber = entityPM.COONumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.COONumberToCancel))
            {
                oldEntityPM.COONumberToCancel = entityPM.COONumberToCancel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReplacementReason))
            {
                oldEntityPM.ReplacementReason = entityPM.ReplacementReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationId))
            {
                oldEntityPM.DeclarationId = entityPM.DeclarationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterVat))
            {
                oldEntityPM.ExporterVat = entityPM.ExporterVat;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterName))
            {
                oldEntityPM.ExporterName = entityPM.ExporterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterAddress))
            {
                oldEntityPM.ExporterAddress = entityPM.ExporterAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterCountry))
            {
                oldEntityPM.ExporterCountry = entityPM.ExporterCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementCountry1))
            {
                oldEntityPM.TradeAgreementCountry1 = entityPM.TradeAgreementCountry1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementCountry2))
            {
                oldEntityPM.TradeAgreementCountry2 = entityPM.TradeAgreementCountry2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementGroupOfCountries))
            {
                oldEntityPM.TradeAgreementGroupOfCountries = entityPM.TradeAgreementGroupOfCountries;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
                oldEntityPM.ConsigneeName = entityPM.ConsigneeName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeAddress))
            {
                oldEntityPM.ConsigneeAddress = entityPM.ConsigneeAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeCountry))
            {
                oldEntityPM.ConsigneeCountry = entityPM.ConsigneeCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeRemarks))
            {
                oldEntityPM.ConsigneeRemarks = entityPM.ConsigneeRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConsigneeForPrint))
            {
                oldEntityPM.IsConsigneeForPrint = entityPM.IsConsigneeForPrint;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginCountry))
            {
                oldEntityPM.OriginCountry = entityPM.OriginCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginGroupOfCountry))
            {
                oldEntityPM.OriginGroupOfCountry = entityPM.OriginGroupOfCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationCountry))
            {
                oldEntityPM.DestinationCountry = entityPM.DestinationCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationGroupOfCountries))
            {
                oldEntityPM.DestinationGroupOfCountries = entityPM.DestinationGroupOfCountries;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transport))
            {
                oldEntityPM.Transport = entityPM.Transport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PortOfShipment))
            {
                oldEntityPM.PortOfShipment = entityPM.PortOfShipment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCumulation))
            {
                oldEntityPM.IsCumulation = entityPM.IsCumulation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CumulationCountry))
            {
                oldEntityPM.CumulationCountry = entityPM.CumulationCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CumulationGroupOfCountries))
            {
                oldEntityPM.CumulationGroupOfCountries = entityPM.CumulationGroupOfCountries;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PlaceOfManufacture))
            {
                oldEntityPM.PlaceOfManufacture = entityPM.PlaceOfManufacture;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ZipCodeOfManufacture))
            {
                oldEntityPM.ZipCodeOfManufacture = entityPM.ZipCodeOfManufacture;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Observations))
            {
                oldEntityPM.Observations = entityPM.Observations;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExportDecForPrint))
            {
                oldEntityPM.IsExportDecForPrint = entityPM.IsExportDecForPrint;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsUnitedInvoices))
            {
                oldEntityPM.IsUnitedInvoices = entityPM.IsUnitedInvoices;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsHouse))
            {
                oldEntityPM.CustomsHouse = entityPM.CustomsHouse;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCountry))
            {
                oldEntityPM.IssuingCountry = entityPM.IssuingCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CityOfDeclaration))
            {
                oldEntityPM.CityOfDeclaration = entityPM.CityOfDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryOfDeclaration))
            {
                oldEntityPM.CountryOfDeclaration = entityPM.CountryOfDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DateOfDeclaration))
            {
                oldEntityPM.DateOfDeclaration = entityPM.DateOfDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDeclaredByManufacture))
            {
                oldEntityPM.IsDeclaredByManufacture = entityPM.IsDeclaredByManufacture;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDeclaredByExporter))
            {
                oldEntityPM.IsDeclaredByExporter = entityPM.IsDeclaredByExporter;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAttachedList))
            {
                oldEntityPM.IsAttachedList = entityPM.IsAttachedList;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsufficentWorkingInd))
            {
                oldEntityPM.InsufficentWorkingInd = entityPM.InsufficentWorkingInd;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InsufficentWorkingText))
            {
                oldEntityPM.InsufficentWorkingText = entityPM.InsufficentWorkingText;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportDate))
            {
                oldEntityPM.NonExportDate = entityPM.NonExportDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportCountry))
            {
                oldEntityPM.NonExportCountry = entityPM.NonExportCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonImportBillOfLadingNum))
            {
                oldEntityPM.NonImportBillOfLadingNum = entityPM.NonImportBillOfLadingNum;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportPort))
            {
                oldEntityPM.NonExportPort = entityPM.NonExportPort;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonImportDate))
            {
                oldEntityPM.NonImportDate = entityPM.NonImportDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExportBillOfLadingNum))
            {
                oldEntityPM.NonExportBillOfLadingNum = entityPM.NonExportBillOfLadingNum;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonTransirCountry))
            {
                oldEntityPM.NonTransirCountry = entityPM.NonTransirCountry;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonPortOfEntrance))
            {
                oldEntityPM.NonPortOfEntrance = entityPM.NonPortOfEntrance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExpectedExitDate))
            {
                oldEntityPM.NonExpectedExitDate = entityPM.NonExpectedExitDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonExitPort))
            {
                oldEntityPM.NonExitPort = entityPM.NonExitPort;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonGoodsDescription))
            {
                oldEntityPM.NonGoodsDescription = entityPM.NonGoodsDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonDeclaringCompany))
            {
                oldEntityPM.NonDeclaringCompany = entityPM.NonDeclaringCompany;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonDeclaringPerson))
            {
                oldEntityPM.NonDeclaringPerson = entityPM.NonDeclaringPerson;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonDeclaringPosition))
            {
                oldEntityPM.NonDeclaringPosition = entityPM.NonDeclaringPosition;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NonManifestNum))
            {
                oldEntityPM.NonManifestNum = entityPM.NonManifestNum;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrXml))
            {
                oldEntityPM.ErrXml = entityPM.ErrXml;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooStatusCode))
            {
                oldEntityPM.CooStatusCode = entityPM.CooStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FeedbackRemark))
            {
                oldEntityPM.FeedbackRemark = entityPM.FeedbackRemark;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RejectCancelReason))
            {
                oldEntityPM.RejectCancelReason = entityPM.RejectCancelReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssueDateIfReleased))
            {
                oldEntityPM.IssueDateIfReleased = entityPM.IssueDateIfReleased;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QueryUrl))
            {
                oldEntityPM.QueryUrl = entityPM.QueryUrl;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CooPdf))
            {
                oldEntityPM.CooPdf = entityPM.CooPdf;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CoodPdf1))
            {
                oldEntityPM.CoodPdf1 = entityPM.CoodPdf1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenByUser))
            {
                oldEntityPM.OpenByUser = entityPM.OpenByUser;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSubmitted))
            {
                oldEntityPM.IsSubmitted = entityPM.IsSubmitted;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDeclaration))
            {
                oldEntityPM.UpdateDeclaration = entityPM.UpdateDeclaration;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CertificateOfOriginPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Counter)) //T4 find type == nText 
            {
                entityPM.Counter = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Counter));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.COONumber)) //T4 find type == nText 
            {
                entityPM.COONumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.COONumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.COONumberToCancel)) //T4 find type == nText 
            {
                entityPM.COONumberToCancel = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.COONumberToCancel));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ReplacementReason)) //T4 find type == nText 
            {
                entityPM.ReplacementReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ReplacementReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExporterVat)) //T4 find type == nText 
            {
                entityPM.ExporterVat = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExporterVat));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExporterName)) //T4 find type == nText 
            {
                entityPM.ExporterName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExporterName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExporterAddress)) //T4 find type == nText 
            {
                entityPM.ExporterAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExporterAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ConsigneeName)) //T4 find type == nText 
            {
                entityPM.ConsigneeName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ConsigneeName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ConsigneeAddress)) //T4 find type == nText 
            {
                entityPM.ConsigneeAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ConsigneeAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ConsigneeRemarks)) //T4 find type == nText 
            {
                entityPM.ConsigneeRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ConsigneeRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Transport)) //T4 find type == nText 
            {
                entityPM.Transport = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Transport));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ZipCodeOfManufacture)) //T4 find type == nText 
            {
                entityPM.ZipCodeOfManufacture = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ZipCodeOfManufacture));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Observations)) //T4 find type == nText 
            {
                entityPM.Observations = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Observations));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.InsufficentWorkingText)) //T4 find type == nText 
            {
                entityPM.InsufficentWorkingText = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.InsufficentWorkingText));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonImportBillOfLadingNum)) //T4 find type == nText 
            {
                entityPM.NonImportBillOfLadingNum = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonImportBillOfLadingNum));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonExportBillOfLadingNum)) //T4 find type == nText 
            {
                entityPM.NonExportBillOfLadingNum = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonExportBillOfLadingNum));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonGoodsDescription)) //T4 find type == nText 
            {
                entityPM.NonGoodsDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonGoodsDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonDeclaringCompany)) //T4 find type == nText 
            {
                entityPM.NonDeclaringCompany = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonDeclaringCompany));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonDeclaringPerson)) //T4 find type == nText 
            {
                entityPM.NonDeclaringPerson = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonDeclaringPerson));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonDeclaringPosition)) //T4 find type == nText 
            {
                entityPM.NonDeclaringPosition = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonDeclaringPosition));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NonManifestNum)) //T4 find type == nText 
            {
                entityPM.NonManifestNum = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NonManifestNum));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ErrXml)) //T4 find type == nText 
            {
                entityPM.ErrXml = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ErrXml));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FeedbackRemark)) //T4 find type == nText 
            {
                entityPM.FeedbackRemark = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FeedbackRemark));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RejectCancelReason)) //T4 find type == nText 
            {
                entityPM.RejectCancelReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RejectCancelReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.QueryUrl)) //T4 find type == nText 
            {
                entityPM.QueryUrl = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.QueryUrl));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CooPdf)) //T4 find type == nText 
            {
                entityPM.CooPdf = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CooPdf));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CoodPdf1)) //T4 find type == nText 
            {
                entityPM.CoodPdf1 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CoodPdf1));
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
		
		private void BuildSearchFieldsGenerated(CertificateOfOriginPM entityPM, CertificateOfOrigin entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 