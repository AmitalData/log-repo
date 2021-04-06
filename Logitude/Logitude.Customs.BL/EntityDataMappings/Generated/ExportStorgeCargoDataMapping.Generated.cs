
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
   
   public partial class ExportStorgeCargoDataMapping: IMapping<ExportStorgeCargoPM, ExportStorgeCargo>,IMappingEncodeBase64NVARCHARFields<ExportStorgeCargoPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         StorageID, 
	         CargoTypeCode, 
	         Manifest, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         CargoDescription, 
	         CargoType, 
	         HandlingCode, 
	         DangerousGoodsIndication, 
	         CodeBreaksIndication, 
	         DamageCode, 
	         ForeignCurrencyType, 
	         ForeignCurrencyAmoun, 
	         GoodsValueNIS, 
	         PackageType, 
	         Quantity, 
	         MarksNumbers, 
	         WeightInPortMandatory, 
	         Weight, 
	         VolumeSize, 
	         LicensePlateNumber, 
	         CustomsItem, 
	         RiskLevel, 
	         DangerousSubstancename, 
	         WeightVerificationNumber, 
	         ExporterReportedWeightID, 
	         ExporterReportedWeightName, 
	         ContainerNumber, 
	         CoolingActivated, 
	         RequiredTemperature, 
	         PharmaGroceryIndication, 
	         LeftException, 
	         RightException, 
	         FrontException, 
	         BackException, 
	         HeightException, 
	         ContainerLineCode, 
	         VentValue, 
	         HumidityPercentage, 
	         Co2Percentage, 
	         O2Percentage, 
	         SealNumber, 
	         SealType, 
	         CoolingReportingMethod, 
	         FullnessCode, 
	         OwnershipCode, 
	         ContainerTypeWCO, 
	         UNNumber, 
	         RiskGroup,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SearchFields, 
	         StorageID, 
	         CargoTypeCode, 
	         Manifest, 
	         SecondCargoID, 
	         ThirdCargoID, 
	         CargoDescription, 
	         CargoType, 
	         HandlingCode, 
	         DangerousGoodsIndication, 
	         CodeBreaksIndication, 
	         DamageCode, 
	         ForeignCurrencyType, 
	         ForeignCurrencyAmoun, 
	         GoodsValueNIS, 
	         PackageType, 
	         Quantity, 
	         MarksNumbers, 
	         WeightInPortMandatory, 
	         Weight, 
	         VolumeSize, 
	         LicensePlateNumber, 
	         CustomsItem, 
	         RiskLevel, 
	         DangerousSubstancename, 
	         WeightVerificationNumber, 
	         ExporterReportedWeightID, 
	         ExporterReportedWeightName, 
	         ContainerNumber, 
	         CoolingActivated, 
	         RequiredTemperature, 
	         PharmaGroceryIndication, 
	         LeftException, 
	         RightException, 
	         FrontException, 
	         BackException, 
	         HeightException, 
	         ContainerLineCode, 
	         VentValue, 
	         HumidityPercentage, 
	         Co2Percentage, 
	         O2Percentage, 
	         SealNumber, 
	         SealType, 
	         CoolingReportingMethod, 
	         FullnessCode, 
	         OwnershipCode, 
	         ContainerTypeWCO, 
	         UNNumber, 
	         RiskGroup,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ExportStorgeCargoPM entityPM, ExportStorgeCargo entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageID))
            {
				entityPOCO.StorageID = entityPM.StorageID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
				entityPOCO.CargoTypeCode = entityPM.CargoTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Manifest))
            {
				entityPOCO.Manifest = entityPM.Manifest;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
				entityPOCO.SecondCargoID = entityPM.SecondCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
				entityPOCO.ThirdCargoID = entityPM.ThirdCargoID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoDescription))
            {
				entityPOCO.CargoDescription = entityPM.CargoDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoType))
            {
				entityPOCO.CargoType = entityPM.CargoType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HandlingCode))
            {
				entityPOCO.HandlingCode = entityPM.HandlingCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousGoodsIndication))
            {
				entityPOCO.DangerousGoodsIndication = entityPM.DangerousGoodsIndication;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CodeBreaksIndication))
            {
				entityPOCO.CodeBreaksIndication = entityPM.CodeBreaksIndication;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DamageCode))
            {
				entityPOCO.DamageCode = entityPM.DamageCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignCurrencyType))
            {
				entityPOCO.ForeignCurrencyType = entityPM.ForeignCurrencyType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignCurrencyAmoun))
            {
				entityPOCO.ForeignCurrencyAmoun = entityPM.ForeignCurrencyAmoun;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsValueNIS))
            {
				entityPOCO.GoodsValueNIS = entityPM.GoodsValueNIS;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType))
            {
				entityPOCO.PackageType = entityPM.PackageType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarksNumbers))
            {
				entityPOCO.MarksNumbers = entityPM.MarksNumbers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightInPortMandatory))
            {
				entityPOCO.WeightInPortMandatory = entityPM.WeightInPortMandatory;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
				entityPOCO.Weight = entityPM.Weight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeSize))
            {
				entityPOCO.VolumeSize = entityPM.VolumeSize;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LicensePlateNumber))
            {
				entityPOCO.LicensePlateNumber = entityPM.LicensePlateNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItem))
            {
				entityPOCO.CustomsItem = entityPM.CustomsItem;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RiskLevel))
            {
				entityPOCO.RiskLevel = entityPM.RiskLevel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousSubstancename))
            {
				entityPOCO.DangerousSubstancename = entityPM.DangerousSubstancename;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightVerificationNumber))
            {
				entityPOCO.WeightVerificationNumber = entityPM.WeightVerificationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterReportedWeightID))
            {
				entityPOCO.ExporterReportedWeightID = entityPM.ExporterReportedWeightID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterReportedWeightName))
            {
				entityPOCO.ExporterReportedWeightName = entityPM.ExporterReportedWeightName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNumber))
            {
				entityPOCO.ContainerNumber = entityPM.ContainerNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CoolingActivated))
            {
				entityPOCO.CoolingActivated = entityPM.CoolingActivated;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequiredTemperature))
            {
				entityPOCO.RequiredTemperature = entityPM.RequiredTemperature;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PharmaGroceryIndication))
            {
				entityPOCO.PharmaGroceryIndication = entityPM.PharmaGroceryIndication;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeftException))
            {
				entityPOCO.LeftException = entityPM.LeftException;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RightException))
            {
				entityPOCO.RightException = entityPM.RightException;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FrontException))
            {
				entityPOCO.FrontException = entityPM.FrontException;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BackException))
            {
				entityPOCO.BackException = entityPM.BackException;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HeightException))
            {
				entityPOCO.HeightException = entityPM.HeightException;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerLineCode))
            {
				entityPOCO.ContainerLineCode = entityPM.ContainerLineCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VentValue))
            {
				entityPOCO.VentValue = entityPM.VentValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HumidityPercentage))
            {
				entityPOCO.HumidityPercentage = entityPM.HumidityPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Co2Percentage))
            {
				entityPOCO.Co2Percentage = entityPM.Co2Percentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.O2Percentage))
            {
				entityPOCO.O2Percentage = entityPM.O2Percentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealNumber))
            {
				entityPOCO.SealNumber = entityPM.SealNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealType))
            {
				entityPOCO.SealType = entityPM.SealType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CoolingReportingMethod))
            {
				entityPOCO.CoolingReportingMethod = entityPM.CoolingReportingMethod;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullnessCode))
            {
				entityPOCO.FullnessCode = entityPM.FullnessCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnershipCode))
            {
				entityPOCO.OwnershipCode = entityPM.OwnershipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerTypeWCO))
            {
				entityPOCO.ContainerTypeWCO = entityPM.ContainerTypeWCO;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UNNumber))
            {
				entityPOCO.UNNumber = entityPM.UNNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RiskGroup))
            {
				entityPOCO.RiskGroup = entityPM.RiskGroup;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ExportStorgeCargoPM entityPM, ExportStorgeCargo entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageID))
            {
					entityPM.StorageID = entityPOCO.StorageID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoTypeCode))
            {
					entityPM.CargoTypeCode = entityPOCO.CargoTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Manifest))
            {
					entityPM.Manifest = entityPOCO.Manifest;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecondCargoID))
            {
					entityPM.SecondCargoID = entityPOCO.SecondCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ThirdCargoID))
            {
					entityPM.ThirdCargoID = entityPOCO.ThirdCargoID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoDescription))
            {
					entityPM.CargoDescription = entityPOCO.CargoDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoType))
            {
					entityPM.CargoType = entityPOCO.CargoType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HandlingCode))
            {
					entityPM.HandlingCode = entityPOCO.HandlingCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousGoodsIndication))
            {
					entityPM.DangerousGoodsIndication = entityPOCO.DangerousGoodsIndication;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CodeBreaksIndication))
            {
					entityPM.CodeBreaksIndication = entityPOCO.CodeBreaksIndication;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DamageCode))
            {
					entityPM.DamageCode = entityPOCO.DamageCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignCurrencyType))
            {
					entityPM.ForeignCurrencyType = entityPOCO.ForeignCurrencyType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForeignCurrencyAmoun))
            {
					entityPM.ForeignCurrencyAmoun = entityPOCO.ForeignCurrencyAmoun;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsValueNIS))
            {
					entityPM.GoodsValueNIS = entityPOCO.GoodsValueNIS;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType))
            {
					entityPM.PackageType = entityPOCO.PackageType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MarksNumbers))
            {
					entityPM.MarksNumbers = entityPOCO.MarksNumbers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WeightInPortMandatory))
            {
					entityPM.WeightInPortMandatory = entityPOCO.WeightInPortMandatory;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Weight))
            {
					entityPM.Weight = entityPOCO.Weight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumeSize))
            {
					entityPM.VolumeSize = entityPOCO.VolumeSize;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LicensePlateNumber))
            {
					entityPM.LicensePlateNumber = entityPOCO.LicensePlateNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItem))
            {
					entityPM.CustomsItem = entityPOCO.CustomsItem;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RiskLevel))
            {
					entityPM.RiskLevel = entityPOCO.RiskLevel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousSubstancename))
            {
					entityPM.DangerousSubstancename = entityPOCO.DangerousSubstancename;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WeightVerificationNumber))
            {
					entityPM.WeightVerificationNumber = entityPOCO.WeightVerificationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterReportedWeightID))
            {
					entityPM.ExporterReportedWeightID = entityPOCO.ExporterReportedWeightID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExporterReportedWeightName))
            {
					entityPM.ExporterReportedWeightName = entityPOCO.ExporterReportedWeightName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerNumber))
            {
					entityPM.ContainerNumber = entityPOCO.ContainerNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CoolingActivated))
            {
					entityPM.CoolingActivated = entityPOCO.CoolingActivated;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequiredTemperature))
            {
					entityPM.RequiredTemperature = entityPOCO.RequiredTemperature;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PharmaGroceryIndication))
            {
					entityPM.PharmaGroceryIndication = entityPOCO.PharmaGroceryIndication;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeftException))
            {
					entityPM.LeftException = entityPOCO.LeftException;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RightException))
            {
					entityPM.RightException = entityPOCO.RightException;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FrontException))
            {
					entityPM.FrontException = entityPOCO.FrontException;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BackException))
            {
					entityPM.BackException = entityPOCO.BackException;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HeightException))
            {
					entityPM.HeightException = entityPOCO.HeightException;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerLineCode))
            {
					entityPM.ContainerLineCode = entityPOCO.ContainerLineCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VentValue))
            {
					entityPM.VentValue = entityPOCO.VentValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HumidityPercentage))
            {
					entityPM.HumidityPercentage = entityPOCO.HumidityPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Co2Percentage))
            {
					entityPM.Co2Percentage = entityPOCO.Co2Percentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.O2Percentage))
            {
					entityPM.O2Percentage = entityPOCO.O2Percentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SealNumber))
            {
					entityPM.SealNumber = entityPOCO.SealNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SealType))
            {
					entityPM.SealType = entityPOCO.SealType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CoolingReportingMethod))
            {
					entityPM.CoolingReportingMethod = entityPOCO.CoolingReportingMethod;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FullnessCode))
            {
					entityPM.FullnessCode = entityPOCO.FullnessCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnershipCode))
            {
					entityPM.OwnershipCode = entityPOCO.OwnershipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerTypeWCO))
            {
					entityPM.ContainerTypeWCO = entityPOCO.ContainerTypeWCO;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UNNumber))
            {
					entityPM.UNNumber = entityPOCO.UNNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RiskGroup))
            {
					entityPM.RiskGroup = entityPOCO.RiskGroup;
            }

		}

		public void PMToOldPM(ExportStorgeCargoPM entityPM, ExportStorgeCargoPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageID))
            {
                oldEntityPM.StorageID = entityPM.StorageID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoTypeCode))
            {
                oldEntityPM.CargoTypeCode = entityPM.CargoTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Manifest))
            {
                oldEntityPM.Manifest = entityPM.Manifest;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecondCargoID))
            {
                oldEntityPM.SecondCargoID = entityPM.SecondCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ThirdCargoID))
            {
                oldEntityPM.ThirdCargoID = entityPM.ThirdCargoID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoDescription))
            {
                oldEntityPM.CargoDescription = entityPM.CargoDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoType))
            {
                oldEntityPM.CargoType = entityPM.CargoType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HandlingCode))
            {
                oldEntityPM.HandlingCode = entityPM.HandlingCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousGoodsIndication))
            {
                oldEntityPM.DangerousGoodsIndication = entityPM.DangerousGoodsIndication;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CodeBreaksIndication))
            {
                oldEntityPM.CodeBreaksIndication = entityPM.CodeBreaksIndication;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DamageCode))
            {
                oldEntityPM.DamageCode = entityPM.DamageCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignCurrencyType))
            {
                oldEntityPM.ForeignCurrencyType = entityPM.ForeignCurrencyType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForeignCurrencyAmoun))
            {
                oldEntityPM.ForeignCurrencyAmoun = entityPM.ForeignCurrencyAmoun;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsValueNIS))
            {
                oldEntityPM.GoodsValueNIS = entityPM.GoodsValueNIS;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType))
            {
                oldEntityPM.PackageType = entityPM.PackageType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MarksNumbers))
            {
                oldEntityPM.MarksNumbers = entityPM.MarksNumbers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightInPortMandatory))
            {
                oldEntityPM.WeightInPortMandatory = entityPM.WeightInPortMandatory;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Weight))
            {
                oldEntityPM.Weight = entityPM.Weight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeSize))
            {
                oldEntityPM.VolumeSize = entityPM.VolumeSize;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LicensePlateNumber))
            {
                oldEntityPM.LicensePlateNumber = entityPM.LicensePlateNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItem))
            {
                oldEntityPM.CustomsItem = entityPM.CustomsItem;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RiskLevel))
            {
                oldEntityPM.RiskLevel = entityPM.RiskLevel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousSubstancename))
            {
                oldEntityPM.DangerousSubstancename = entityPM.DangerousSubstancename;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightVerificationNumber))
            {
                oldEntityPM.WeightVerificationNumber = entityPM.WeightVerificationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterReportedWeightID))
            {
                oldEntityPM.ExporterReportedWeightID = entityPM.ExporterReportedWeightID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExporterReportedWeightName))
            {
                oldEntityPM.ExporterReportedWeightName = entityPM.ExporterReportedWeightName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerNumber))
            {
                oldEntityPM.ContainerNumber = entityPM.ContainerNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CoolingActivated))
            {
                oldEntityPM.CoolingActivated = entityPM.CoolingActivated;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequiredTemperature))
            {
                oldEntityPM.RequiredTemperature = entityPM.RequiredTemperature;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PharmaGroceryIndication))
            {
                oldEntityPM.PharmaGroceryIndication = entityPM.PharmaGroceryIndication;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeftException))
            {
                oldEntityPM.LeftException = entityPM.LeftException;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RightException))
            {
                oldEntityPM.RightException = entityPM.RightException;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FrontException))
            {
                oldEntityPM.FrontException = entityPM.FrontException;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BackException))
            {
                oldEntityPM.BackException = entityPM.BackException;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HeightException))
            {
                oldEntityPM.HeightException = entityPM.HeightException;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerLineCode))
            {
                oldEntityPM.ContainerLineCode = entityPM.ContainerLineCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VentValue))
            {
                oldEntityPM.VentValue = entityPM.VentValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HumidityPercentage))
            {
                oldEntityPM.HumidityPercentage = entityPM.HumidityPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Co2Percentage))
            {
                oldEntityPM.Co2Percentage = entityPM.Co2Percentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.O2Percentage))
            {
                oldEntityPM.O2Percentage = entityPM.O2Percentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealNumber))
            {
                oldEntityPM.SealNumber = entityPM.SealNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealType))
            {
                oldEntityPM.SealType = entityPM.SealType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CoolingReportingMethod))
            {
                oldEntityPM.CoolingReportingMethod = entityPM.CoolingReportingMethod;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullnessCode))
            {
                oldEntityPM.FullnessCode = entityPM.FullnessCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnershipCode))
            {
                oldEntityPM.OwnershipCode = entityPM.OwnershipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerTypeWCO))
            {
                oldEntityPM.ContainerTypeWCO = entityPM.ContainerTypeWCO;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UNNumber))
            {
                oldEntityPM.UNNumber = entityPM.UNNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RiskGroup))
            {
                oldEntityPM.RiskGroup = entityPM.RiskGroup;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ExportStorgeCargoPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CargoDescription)) //T4 find type == nText 
            {
                entityPM.CargoDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CargoDescription));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.MarksNumbers)) //T4 find type == nText 
            {
                entityPM.MarksNumbers = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.MarksNumbers));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CustomsItem)) //T4 find type == nText 
            {
                entityPM.CustomsItem = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CustomsItem));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DangerousSubstancename)) //T4 find type == nText 
            {
                entityPM.DangerousSubstancename = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DangerousSubstancename));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.WeightVerificationNumber)) //T4 find type == nText 
            {
                entityPM.WeightVerificationNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.WeightVerificationNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExporterReportedWeightName)) //T4 find type == nText 
            {
                entityPM.ExporterReportedWeightName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExporterReportedWeightName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ContainerNumber)) //T4 find type == nText 
            {
                entityPM.ContainerNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ContainerNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ContainerLineCode)) //T4 find type == nText 
            {
                entityPM.ContainerLineCode = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ContainerLineCode));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SealNumber)) //T4 find type == nText 
            {
                entityPM.SealNumber = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SealNumber));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RiskGroup)) //T4 find type == nText 
            {
                entityPM.RiskGroup = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RiskGroup));
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
		
		private void BuildSearchFieldsGenerated(ExportStorgeCargoPM entityPM, ExportStorgeCargo entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 