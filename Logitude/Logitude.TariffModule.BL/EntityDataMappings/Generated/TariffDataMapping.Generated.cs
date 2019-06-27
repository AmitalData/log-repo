
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffDataMapping: IMapping<TariffPM, Tariff>,IMappingEncodeBase64NVARCHARFields<TariffPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         SearchFields, 
	         StartDate, 
	         ExpirationDate, 
	         Name, 
	         InActive, 
	         Description, 
	         SellerId, 
	         CurrencyId, 
	         CreateDate, 
	         UpdateDate, 
	         LastExpirationDate, 
	         PriceSteps, 
	         TypeCode, 
	         LastStartDate, 
	         LastVersion, 
	         ContractNumber, 
	         TariffNumber, 
	         Surcharge1Id, 
	         Surcharge2Id, 
	         Surcharge3Id, 
	         Surcharge4Id, 
	         Surcharge5Id, 
	         Surcharge6Id, 
	         Surcharge7Id, 
	         Surcharge8Id, 
	         Surcharge9Id, 
	         Surcharge10Id, 
	         Surcharge1UOM, 
	         Surcharge2UOM, 
	         Surcharge3UOM, 
	         Surcharge4UOM, 
	         Surcharge5UOM, 
	         Surcharge6UOM, 
	         Surcharge7UOM, 
	         Surcharge8UOM, 
	         Surcharge9UOM, 
	         Surcharge10UOM, 
	         ConcurrencyGUID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         SearchFields, 
	         StartDate, 
	         ExpirationDate, 
	         Name, 
	         InActive, 
	         Description, 
	         SellerId, 
	         CurrencyId, 
	         CreateDate, 
	         UpdateDate, 
	         LastExpirationDate, 
	         PriceSteps, 
	         TypeCode, 
	         TypeName, 
	         LastStartDate, 
	         LastVersion, 
	         ContractNumber, 
	         SetAsInActive, 
	         SetAsReActive, 
	         TariffNumber, 
	         TariffLinesAdded, 
	         Surcharge1Id, 
	         Surcharge2Id, 
	         Surcharge3Id, 
	         Surcharge4Id, 
	         Surcharge5Id, 
	         Surcharge6Id, 
	         Surcharge7Id, 
	         Surcharge8Id, 
	         Surcharge9Id, 
	         Surcharge10Id, 
	         Surcharge1UOM, 
	         Surcharge2UOM, 
	         Surcharge3UOM, 
	         Surcharge4UOM, 
	         Surcharge5UOM, 
	         Surcharge6UOM, 
	         Surcharge7UOM, 
	         Surcharge8UOM, 
	         Surcharge9UOM, 
	         Surcharge10UOM, 
	         TariffLinesAddedNumbers, 
	         TariffLinesAddedFromExcel, 
	         FileUploadedName, 
	         ConcurrencyGUID, 
	         NewConcurrencyGUID, 
	         IsApprovingDraftVersion, 
	         IsSurchargeUpdate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffPM entityPM, Tariff entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
				entityPOCO.ExpirationDate = entityPM.ExpirationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
				entityPOCO.InActive = entityPM.InActive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
				entityPOCO.Description = entityPM.Description;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SellerId))
            {
				entityPOCO.SellerId = entityPM.SellerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
				entityPOCO.CurrencyId = entityPM.CurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastExpirationDate))
            {
				entityPOCO.LastExpirationDate = entityPM.LastExpirationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PriceSteps))
            {
				entityPOCO.PriceSteps = entityPM.PriceSteps;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
				entityPOCO.TypeCode = entityPM.TypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStartDate))
            {
				entityPOCO.LastStartDate = entityPM.LastStartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastVersion))
            {
				entityPOCO.LastVersion = entityPM.LastVersion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContractNumber))
            {
				entityPOCO.ContractNumber = entityPM.ContractNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffNumber))
            {
				entityPOCO.TariffNumber = entityPM.TariffNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1Id))
            {
				entityPOCO.Surcharge1Id = entityPM.Surcharge1Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2Id))
            {
				entityPOCO.Surcharge2Id = entityPM.Surcharge2Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3Id))
            {
				entityPOCO.Surcharge3Id = entityPM.Surcharge3Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4Id))
            {
				entityPOCO.Surcharge4Id = entityPM.Surcharge4Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5Id))
            {
				entityPOCO.Surcharge5Id = entityPM.Surcharge5Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6Id))
            {
				entityPOCO.Surcharge6Id = entityPM.Surcharge6Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7Id))
            {
				entityPOCO.Surcharge7Id = entityPM.Surcharge7Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8Id))
            {
				entityPOCO.Surcharge8Id = entityPM.Surcharge8Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9Id))
            {
				entityPOCO.Surcharge9Id = entityPM.Surcharge9Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10Id))
            {
				entityPOCO.Surcharge10Id = entityPM.Surcharge10Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1UOM))
            {
				entityPOCO.Surcharge1UOM = entityPM.Surcharge1UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2UOM))
            {
				entityPOCO.Surcharge2UOM = entityPM.Surcharge2UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3UOM))
            {
				entityPOCO.Surcharge3UOM = entityPM.Surcharge3UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4UOM))
            {
				entityPOCO.Surcharge4UOM = entityPM.Surcharge4UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5UOM))
            {
				entityPOCO.Surcharge5UOM = entityPM.Surcharge5UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6UOM))
            {
				entityPOCO.Surcharge6UOM = entityPM.Surcharge6UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7UOM))
            {
				entityPOCO.Surcharge7UOM = entityPM.Surcharge7UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8UOM))
            {
				entityPOCO.Surcharge8UOM = entityPM.Surcharge8UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9UOM))
            {
				entityPOCO.Surcharge9UOM = entityPM.Surcharge9UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10UOM))
            {
				entityPOCO.Surcharge10UOM = entityPM.Surcharge10UOM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TariffPM entityPM, Tariff entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpirationDate))
            {
					entityPM.ExpirationDate = entityPOCO.ExpirationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InActive))
            {
					entityPM.InActive = entityPOCO.InActive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Description))
            {
					entityPM.Description = entityPOCO.Description;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SellerId))
            {
					entityPM.SellerId = entityPOCO.SellerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrencyId))
            {
					entityPM.CurrencyId = entityPOCO.CurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastExpirationDate))
            {
					entityPM.LastExpirationDate = entityPOCO.LastExpirationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PriceSteps))
            {
					entityPM.PriceSteps = entityPOCO.PriceSteps;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TypeCode))
            {
					entityPM.TypeCode = entityPOCO.TypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStartDate))
            {
					entityPM.LastStartDate = entityPOCO.LastStartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastVersion))
            {
					entityPM.LastVersion = entityPOCO.LastVersion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContractNumber))
            {
					entityPM.ContractNumber = entityPOCO.ContractNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffNumber))
            {
					entityPM.TariffNumber = entityPOCO.TariffNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge1Id))
            {
					entityPM.Surcharge1Id = entityPOCO.Surcharge1Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge2Id))
            {
					entityPM.Surcharge2Id = entityPOCO.Surcharge2Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge3Id))
            {
					entityPM.Surcharge3Id = entityPOCO.Surcharge3Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge4Id))
            {
					entityPM.Surcharge4Id = entityPOCO.Surcharge4Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge5Id))
            {
					entityPM.Surcharge5Id = entityPOCO.Surcharge5Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge6Id))
            {
					entityPM.Surcharge6Id = entityPOCO.Surcharge6Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge7Id))
            {
					entityPM.Surcharge7Id = entityPOCO.Surcharge7Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge8Id))
            {
					entityPM.Surcharge8Id = entityPOCO.Surcharge8Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge9Id))
            {
					entityPM.Surcharge9Id = entityPOCO.Surcharge9Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge10Id))
            {
					entityPM.Surcharge10Id = entityPOCO.Surcharge10Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge1UOM))
            {
					entityPM.Surcharge1UOM = entityPOCO.Surcharge1UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge2UOM))
            {
					entityPM.Surcharge2UOM = entityPOCO.Surcharge2UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge3UOM))
            {
					entityPM.Surcharge3UOM = entityPOCO.Surcharge3UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge4UOM))
            {
					entityPM.Surcharge4UOM = entityPOCO.Surcharge4UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge5UOM))
            {
					entityPM.Surcharge5UOM = entityPOCO.Surcharge5UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge6UOM))
            {
					entityPM.Surcharge6UOM = entityPOCO.Surcharge6UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge7UOM))
            {
					entityPM.Surcharge7UOM = entityPOCO.Surcharge7UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge8UOM))
            {
					entityPM.Surcharge8UOM = entityPOCO.Surcharge8UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge9UOM))
            {
					entityPM.Surcharge9UOM = entityPOCO.Surcharge9UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Surcharge10UOM))
            {
					entityPM.Surcharge10UOM = entityPOCO.Surcharge10UOM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

		}

		public void PMToOldPM(TariffPM entityPM, TariffPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
                oldEntityPM.ExpirationDate = entityPM.ExpirationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
                oldEntityPM.InActive = entityPM.InActive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Description))
            {
                oldEntityPM.Description = entityPM.Description;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SellerId))
            {
                oldEntityPM.SellerId = entityPM.SellerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrencyId))
            {
                oldEntityPM.CurrencyId = entityPM.CurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastExpirationDate))
            {
                oldEntityPM.LastExpirationDate = entityPM.LastExpirationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PriceSteps))
            {
                oldEntityPM.PriceSteps = entityPM.PriceSteps;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TypeCode))
            {
                oldEntityPM.TypeCode = entityPM.TypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStartDate))
            {
                oldEntityPM.LastStartDate = entityPM.LastStartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastVersion))
            {
                oldEntityPM.LastVersion = entityPM.LastVersion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContractNumber))
            {
                oldEntityPM.ContractNumber = entityPM.ContractNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffNumber))
            {
                oldEntityPM.TariffNumber = entityPM.TariffNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1Id))
            {
                oldEntityPM.Surcharge1Id = entityPM.Surcharge1Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2Id))
            {
                oldEntityPM.Surcharge2Id = entityPM.Surcharge2Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3Id))
            {
                oldEntityPM.Surcharge3Id = entityPM.Surcharge3Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4Id))
            {
                oldEntityPM.Surcharge4Id = entityPM.Surcharge4Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5Id))
            {
                oldEntityPM.Surcharge5Id = entityPM.Surcharge5Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6Id))
            {
                oldEntityPM.Surcharge6Id = entityPM.Surcharge6Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7Id))
            {
                oldEntityPM.Surcharge7Id = entityPM.Surcharge7Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8Id))
            {
                oldEntityPM.Surcharge8Id = entityPM.Surcharge8Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9Id))
            {
                oldEntityPM.Surcharge9Id = entityPM.Surcharge9Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10Id))
            {
                oldEntityPM.Surcharge10Id = entityPM.Surcharge10Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge1UOM))
            {
                oldEntityPM.Surcharge1UOM = entityPM.Surcharge1UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge2UOM))
            {
                oldEntityPM.Surcharge2UOM = entityPM.Surcharge2UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge3UOM))
            {
                oldEntityPM.Surcharge3UOM = entityPM.Surcharge3UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge4UOM))
            {
                oldEntityPM.Surcharge4UOM = entityPM.Surcharge4UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge5UOM))
            {
                oldEntityPM.Surcharge5UOM = entityPM.Surcharge5UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge6UOM))
            {
                oldEntityPM.Surcharge6UOM = entityPM.Surcharge6UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge7UOM))
            {
                oldEntityPM.Surcharge7UOM = entityPM.Surcharge7UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge8UOM))
            {
                oldEntityPM.Surcharge8UOM = entityPM.Surcharge8UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge9UOM))
            {
                oldEntityPM.Surcharge9UOM = entityPM.Surcharge9UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Surcharge10UOM))
            {
                oldEntityPM.Surcharge10UOM = entityPM.Surcharge10UOM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Description)) //T4 find type == nText 
            {
                entityPM.Description = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Description));
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
		
		private void BuildSearchFieldsGenerated(TariffPM entityPM, Tariff entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 