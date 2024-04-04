
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
   
   public partial class CB_QuotaDetailsHistoryDataMapping: IMapping<CB_QuotaDetailsHistoryPM, CB_QuotaDetailsHistory>,IMappingEncodeBase64NVARCHARFields<CB_QuotaDetailsHistoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         IsImportLicenseRequired, 
	         MeasurementUnitID, 
	         Quantity, 
	         QuotaComputationBasisID, 
	         QuotaIncrementID, 
	         RenewalMethodID, 
	         RenewalUntilDate, 
	         QuotaID, 
	         ChangeRequestTypePriority, 
	         QuotaValueIncrement, 
	         PerYearFrequency, 
	         CurrencyTypeID, 
	         CB_ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         IsImportLicenseRequired, 
	         MeasurementUnitID, 
	         Quantity, 
	         QuotaComputationBasisID, 
	         QuotaIncrementID, 
	         RenewalMethodID, 
	         RenewalUntilDate, 
	         QuotaID, 
	         ChangeRequestTypePriority, 
	         QuotaValueIncrement, 
	         PerYearFrequency, 
	         CurrencyTypeID, 
	         CB_ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_QuotaDetailsHistoryPM entityPM, CB_QuotaDetailsHistory entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImportLicenseRequired))
            {
				entityPOCO.IsImportLicenseRequired = entityPM.IsImportLicenseRequired;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasurementUnitID))
            {
				entityPOCO.MeasurementUnitID = entityPM.MeasurementUnitID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaComputationBasisID))
            {
				entityPOCO.QuotaComputationBasisID = entityPM.QuotaComputationBasisID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaIncrementID))
            {
				entityPOCO.QuotaIncrementID = entityPM.QuotaIncrementID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RenewalMethodID))
            {
				entityPOCO.RenewalMethodID = entityPM.RenewalMethodID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RenewalUntilDate))
            {
				entityPOCO.RenewalUntilDate = entityPM.RenewalUntilDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaID))
            {
				entityPOCO.QuotaID = entityPM.QuotaID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
				entityPOCO.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaValueIncrement))
            {
				entityPOCO.QuotaValueIncrement = entityPM.QuotaValueIncrement;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PerYearFrequency))
            {
				entityPOCO.PerYearFrequency = entityPM.PerYearFrequency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyTypeID))
            {
				entityPOCO.CurrencyTypeID = entityPM.CurrencyTypeID;
			}
			}

		public void POCOToPM(CB_QuotaDetailsHistoryPM entityPM, CB_QuotaDetailsHistory entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsImportLicenseRequired))
            {
					entityPM.IsImportLicenseRequired = entityPOCO.IsImportLicenseRequired;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MeasurementUnitID))
            {
					entityPM.MeasurementUnitID = entityPOCO.MeasurementUnitID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuotaComputationBasisID))
            {
					entityPM.QuotaComputationBasisID = entityPOCO.QuotaComputationBasisID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuotaIncrementID))
            {
					entityPM.QuotaIncrementID = entityPOCO.QuotaIncrementID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RenewalMethodID))
            {
					entityPM.RenewalMethodID = entityPOCO.RenewalMethodID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RenewalUntilDate))
            {
					entityPM.RenewalUntilDate = entityPOCO.RenewalUntilDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuotaID))
            {
					entityPM.QuotaID = entityPOCO.QuotaID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeRequestTypePriority))
            {
					entityPM.ChangeRequestTypePriority = entityPOCO.ChangeRequestTypePriority;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuotaValueIncrement))
            {
					entityPM.QuotaValueIncrement = entityPOCO.QuotaValueIncrement;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PerYearFrequency))
            {
					entityPM.PerYearFrequency = entityPOCO.PerYearFrequency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyTypeID))
            {
					entityPM.CurrencyTypeID = entityPOCO.CurrencyTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

		}

		public void PMToOldPM(CB_QuotaDetailsHistoryPM entityPM, CB_QuotaDetailsHistoryPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImportLicenseRequired))
            {
                oldEntityPM.IsImportLicenseRequired = entityPM.IsImportLicenseRequired;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MeasurementUnitID))
            {
                oldEntityPM.MeasurementUnitID = entityPM.MeasurementUnitID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaComputationBasisID))
            {
                oldEntityPM.QuotaComputationBasisID = entityPM.QuotaComputationBasisID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaIncrementID))
            {
                oldEntityPM.QuotaIncrementID = entityPM.QuotaIncrementID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RenewalMethodID))
            {
                oldEntityPM.RenewalMethodID = entityPM.RenewalMethodID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RenewalUntilDate))
            {
                oldEntityPM.RenewalUntilDate = entityPM.RenewalUntilDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaID))
            {
                oldEntityPM.QuotaID = entityPM.QuotaID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
                oldEntityPM.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotaValueIncrement))
            {
                oldEntityPM.QuotaValueIncrement = entityPM.QuotaValueIncrement;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PerYearFrequency))
            {
                oldEntityPM.PerYearFrequency = entityPM.PerYearFrequency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyTypeID))
            {
                oldEntityPM.CurrencyTypeID = entityPM.CurrencyTypeID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_QuotaDetailsHistoryPM entityPM)
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
	 