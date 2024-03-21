
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
   
   public partial class CB_PropertiesDetailsHistoryDataMapping: IMapping<CB_PropertiesDetailsHistoryPM, CB_PropertiesDetailsHistory>,IMappingEncodeBase64NVARCHARFields<CB_PropertiesDetailsHistoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         CustomsItemID, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         ChangeRequestTypePriority, 
	         IsCarItem, 
	         IsConditionalExemptionItem, 
	         IsCustomsItemDiscount, 
	         IsEntitlementDiscount, 
	         IsGreenIndex, 
	         IsHybridCar, 
	         IsImporterDiscount, 
	         IsIndexedLinked, 
	         IsNotAutonomiaUpdate, 
	         IsRawMaterial, 
	         IsWholesalePrice, 
	         VatDiscountReason, 
	         MaxSupervisionPeriod, 
	         MeasurementUnitID, 
	         ConditionalExemptionTypeID, 
	         FuelTypeID, 
	         IsElectronic, 
	         CarEngineVolumeID, 
	         CarWeightID, 
	         VatDiscountRate, 
	         Discount_CustomsItemGroupTypeID, 
	         IsCarDiscount, 
	         DiscountRegularityRequirementType, 
	         CB_ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         CustomsItemID, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         ChangeRequestTypePriority, 
	         IsCarItem, 
	         IsConditionalExemptionItem, 
	         IsCustomsItemDiscount, 
	         IsEntitlementDiscount, 
	         IsGreenIndex, 
	         IsHybridCar, 
	         IsImporterDiscount, 
	         IsIndexedLinked, 
	         IsNotAutonomiaUpdate, 
	         IsRawMaterial, 
	         IsWholesalePrice, 
	         VatDiscountReason, 
	         MaxSupervisionPeriod, 
	         MeasurementUnitID, 
	         ConditionalExemptionTypeID, 
	         FuelTypeID, 
	         IsElectronic, 
	         CarEngineVolumeID, 
	         CarWeightID, 
	         VatDiscountRate, 
	         Discount_CustomsItemGroupTypeID, 
	         IsCarDiscount, 
	         DiscountRegularityRequirementType, 
	         CB_ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_PropertiesDetailsHistoryPM entityPM, CB_PropertiesDetailsHistory entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
				entityPOCO.ID = entityPM.ID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
				entityPOCO.CustomsItemID = entityPM.CustomsItemID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
				entityPOCO.EntityStatusID = entityPM.EntityStatusID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
				entityPOCO.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCarItem))
            {
				entityPOCO.IsCarItem = entityPM.IsCarItem;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConditionalExemptionItem))
            {
				entityPOCO.IsConditionalExemptionItem = entityPM.IsConditionalExemptionItem;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomsItemDiscount))
            {
				entityPOCO.IsCustomsItemDiscount = entityPM.IsCustomsItemDiscount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEntitlementDiscount))
            {
				entityPOCO.IsEntitlementDiscount = entityPM.IsEntitlementDiscount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsGreenIndex))
            {
				entityPOCO.IsGreenIndex = entityPM.IsGreenIndex;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsHybridCar))
            {
				entityPOCO.IsHybridCar = entityPM.IsHybridCar;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImporterDiscount))
            {
				entityPOCO.IsImporterDiscount = entityPM.IsImporterDiscount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsIndexedLinked))
            {
				entityPOCO.IsIndexedLinked = entityPM.IsIndexedLinked;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsNotAutonomiaUpdate))
            {
				entityPOCO.IsNotAutonomiaUpdate = entityPM.IsNotAutonomiaUpdate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRawMaterial))
            {
				entityPOCO.IsRawMaterial = entityPM.IsRawMaterial;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsWholesalePrice))
            {
				entityPOCO.IsWholesalePrice = entityPM.IsWholesalePrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatDiscountReason))
            {
				entityPOCO.VatDiscountReason = entityPM.VatDiscountReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaxSupervisionPeriod))
            {
				entityPOCO.MaxSupervisionPeriod = entityPM.MaxSupervisionPeriod;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasurementUnitID))
            {
				entityPOCO.MeasurementUnitID = entityPM.MeasurementUnitID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConditionalExemptionTypeID))
            {
				entityPOCO.ConditionalExemptionTypeID = entityPM.ConditionalExemptionTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FuelTypeID))
            {
				entityPOCO.FuelTypeID = entityPM.FuelTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsElectronic))
            {
				entityPOCO.IsElectronic = entityPM.IsElectronic;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarEngineVolumeID))
            {
				entityPOCO.CarEngineVolumeID = entityPM.CarEngineVolumeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarWeightID))
            {
				entityPOCO.CarWeightID = entityPM.CarWeightID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatDiscountRate))
            {
				entityPOCO.VatDiscountRate = entityPM.VatDiscountRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Discount_CustomsItemGroupTypeID))
            {
				entityPOCO.Discount_CustomsItemGroupTypeID = entityPM.Discount_CustomsItemGroupTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCarDiscount))
            {
				entityPOCO.IsCarDiscount = entityPM.IsCarDiscount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DiscountRegularityRequirementType))
            {
				entityPOCO.DiscountRegularityRequirementType = entityPM.DiscountRegularityRequirementType;
			}
			}

		public void POCOToPM(CB_PropertiesDetailsHistoryPM entityPM, CB_PropertiesDetailsHistory entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemID))
            {
					entityPM.CustomsItemID = entityPOCO.CustomsItemID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityStatusID))
            {
					entityPM.EntityStatusID = entityPOCO.EntityStatusID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeRequestTypePriority))
            {
					entityPM.ChangeRequestTypePriority = entityPOCO.ChangeRequestTypePriority;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCarItem))
            {
					entityPM.IsCarItem = entityPOCO.IsCarItem;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConditionalExemptionItem))
            {
					entityPM.IsConditionalExemptionItem = entityPOCO.IsConditionalExemptionItem;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCustomsItemDiscount))
            {
					entityPM.IsCustomsItemDiscount = entityPOCO.IsCustomsItemDiscount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsEntitlementDiscount))
            {
					entityPM.IsEntitlementDiscount = entityPOCO.IsEntitlementDiscount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsGreenIndex))
            {
					entityPM.IsGreenIndex = entityPOCO.IsGreenIndex;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsHybridCar))
            {
					entityPM.IsHybridCar = entityPOCO.IsHybridCar;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsImporterDiscount))
            {
					entityPM.IsImporterDiscount = entityPOCO.IsImporterDiscount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsIndexedLinked))
            {
					entityPM.IsIndexedLinked = entityPOCO.IsIndexedLinked;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsNotAutonomiaUpdate))
            {
					entityPM.IsNotAutonomiaUpdate = entityPOCO.IsNotAutonomiaUpdate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRawMaterial))
            {
					entityPM.IsRawMaterial = entityPOCO.IsRawMaterial;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsWholesalePrice))
            {
					entityPM.IsWholesalePrice = entityPOCO.IsWholesalePrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatDiscountReason))
            {
					entityPM.VatDiscountReason = entityPOCO.VatDiscountReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MaxSupervisionPeriod))
            {
					entityPM.MaxSupervisionPeriod = entityPOCO.MaxSupervisionPeriod;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeasurementUnitID))
            {
					entityPM.MeasurementUnitID = entityPOCO.MeasurementUnitID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConditionalExemptionTypeID))
            {
					entityPM.ConditionalExemptionTypeID = entityPOCO.ConditionalExemptionTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FuelTypeID))
            {
					entityPM.FuelTypeID = entityPOCO.FuelTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsElectronic))
            {
					entityPM.IsElectronic = entityPOCO.IsElectronic;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CarEngineVolumeID))
            {
					entityPM.CarEngineVolumeID = entityPOCO.CarEngineVolumeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CarWeightID))
            {
					entityPM.CarWeightID = entityPOCO.CarWeightID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatDiscountRate))
            {
					entityPM.VatDiscountRate = entityPOCO.VatDiscountRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Discount_CustomsItemGroupTypeID))
            {
					entityPM.Discount_CustomsItemGroupTypeID = entityPOCO.Discount_CustomsItemGroupTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCarDiscount))
            {
					entityPM.IsCarDiscount = entityPOCO.IsCarDiscount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DiscountRegularityRequirementType))
            {
					entityPM.DiscountRegularityRequirementType = entityPOCO.DiscountRegularityRequirementType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

		}

		public void PMToOldPM(CB_PropertiesDetailsHistoryPM entityPM, CB_PropertiesDetailsHistoryPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
                oldEntityPM.ID = entityPM.ID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
                oldEntityPM.CustomsItemID = entityPM.CustomsItemID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
                oldEntityPM.EntityStatusID = entityPM.EntityStatusID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
                oldEntityPM.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCarItem))
            {
                oldEntityPM.IsCarItem = entityPM.IsCarItem;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConditionalExemptionItem))
            {
                oldEntityPM.IsConditionalExemptionItem = entityPM.IsConditionalExemptionItem;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCustomsItemDiscount))
            {
                oldEntityPM.IsCustomsItemDiscount = entityPM.IsCustomsItemDiscount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEntitlementDiscount))
            {
                oldEntityPM.IsEntitlementDiscount = entityPM.IsEntitlementDiscount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsGreenIndex))
            {
                oldEntityPM.IsGreenIndex = entityPM.IsGreenIndex;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsHybridCar))
            {
                oldEntityPM.IsHybridCar = entityPM.IsHybridCar;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImporterDiscount))
            {
                oldEntityPM.IsImporterDiscount = entityPM.IsImporterDiscount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsIndexedLinked))
            {
                oldEntityPM.IsIndexedLinked = entityPM.IsIndexedLinked;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsNotAutonomiaUpdate))
            {
                oldEntityPM.IsNotAutonomiaUpdate = entityPM.IsNotAutonomiaUpdate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRawMaterial))
            {
                oldEntityPM.IsRawMaterial = entityPM.IsRawMaterial;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsWholesalePrice))
            {
                oldEntityPM.IsWholesalePrice = entityPM.IsWholesalePrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatDiscountReason))
            {
                oldEntityPM.VatDiscountReason = entityPM.VatDiscountReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaxSupervisionPeriod))
            {
                oldEntityPM.MaxSupervisionPeriod = entityPM.MaxSupervisionPeriod;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasurementUnitID))
            {
                oldEntityPM.MeasurementUnitID = entityPM.MeasurementUnitID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConditionalExemptionTypeID))
            {
                oldEntityPM.ConditionalExemptionTypeID = entityPM.ConditionalExemptionTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FuelTypeID))
            {
                oldEntityPM.FuelTypeID = entityPM.FuelTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsElectronic))
            {
                oldEntityPM.IsElectronic = entityPM.IsElectronic;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarEngineVolumeID))
            {
                oldEntityPM.CarEngineVolumeID = entityPM.CarEngineVolumeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarWeightID))
            {
                oldEntityPM.CarWeightID = entityPM.CarWeightID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatDiscountRate))
            {
                oldEntityPM.VatDiscountRate = entityPM.VatDiscountRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Discount_CustomsItemGroupTypeID))
            {
                oldEntityPM.Discount_CustomsItemGroupTypeID = entityPM.Discount_CustomsItemGroupTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCarDiscount))
            {
                oldEntityPM.IsCarDiscount = entityPM.IsCarDiscount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DiscountRegularityRequirementType))
            {
                oldEntityPM.DiscountRegularityRequirementType = entityPM.DiscountRegularityRequirementType;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_PropertiesDetailsHistoryPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
			  
   }
}
	 