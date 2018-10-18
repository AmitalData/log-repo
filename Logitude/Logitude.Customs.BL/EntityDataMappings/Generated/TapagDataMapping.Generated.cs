
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
   
   public partial class TapagDataMapping: IMapping<TapagPM, Tapag>,IMappingEncodeBase64NVARCHARFields<TapagPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TapagNumber, 
	         LeadingFileNumber, 
	         TapagTypeCode, 
	         CustomerId, 
	         ImporterId, 
	         CustomsBranchCode, 
	         ProfessionUnitTypeCode, 
	         SpecializationTypeCode, 
	         CreateDate, 
	         FollowDate, 
	         ValidityDate, 
	         IsClosed, 
	         ReferantId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TapagNumber, 
	         LeadingFileNumber, 
	         TapagTypeCode, 
	         TapagTypeName, 
	         CustomerId, 
	         CustomerName, 
	         ImporterId, 
	         ImporterName, 
	         CustomsBranchCode, 
	         CustomsBranchName, 
	         ProfessionUnitTypeCode, 
	         ProfessionUnitTypeName, 
	         SpecializationTypeCode, 
	         SpecializationTypeName, 
	         CreateDate, 
	         FollowDate, 
	         ValidityDate, 
	         IsClosed, 
	         CustomsTapagFile, 
	         CustomsNumeral, 
	         RequestFileNumber, 
	         ReferantId, 
	         ReferantName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TapagPM entityPM, Tapag entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagNumber))
            {
				entityPOCO.TapagNumber = entityPM.TapagNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadingFileNumber))
            {
				entityPOCO.LeadingFileNumber = entityPM.LeadingFileNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagTypeCode))
            {
				entityPOCO.TapagTypeCode = entityPM.TapagTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterId))
            {
				entityPOCO.ImporterId = entityPM.ImporterId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBranchCode))
            {
				entityPOCO.CustomsBranchCode = entityPM.CustomsBranchCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfessionUnitTypeCode))
            {
				entityPOCO.ProfessionUnitTypeCode = entityPM.ProfessionUnitTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecializationTypeCode))
            {
				entityPOCO.SpecializationTypeCode = entityPM.SpecializationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FollowDate))
            {
				entityPOCO.FollowDate = entityPM.FollowDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValidityDate))
            {
				entityPOCO.ValidityDate = entityPM.ValidityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferantId))
            {
				entityPOCO.ReferantId = entityPM.ReferantId;
			}
			}

		public void POCOToPM(TapagPM entityPM, Tapag entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TapagNumber))
            {
					entityPM.TapagNumber = entityPOCO.TapagNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LeadingFileNumber))
            {
					entityPM.LeadingFileNumber = entityPOCO.LeadingFileNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TapagTypeCode))
            {
					entityPM.TapagTypeCode = entityPOCO.TapagTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterId))
            {
					entityPM.ImporterId = entityPOCO.ImporterId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBranchCode))
            {
					entityPM.CustomsBranchCode = entityPOCO.CustomsBranchCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfessionUnitTypeCode))
            {
					entityPM.ProfessionUnitTypeCode = entityPOCO.ProfessionUnitTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecializationTypeCode))
            {
					entityPM.SpecializationTypeCode = entityPOCO.SpecializationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FollowDate))
            {
					entityPM.FollowDate = entityPOCO.FollowDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValidityDate))
            {
					entityPM.ValidityDate = entityPOCO.ValidityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReferantId))
            {
					entityPM.ReferantId = entityPOCO.ReferantId;
            }

		}

		public void PMToOldPM(TapagPM entityPM, TapagPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagNumber))
            {
                oldEntityPM.TapagNumber = entityPM.TapagNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LeadingFileNumber))
            {
                oldEntityPM.LeadingFileNumber = entityPM.LeadingFileNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TapagTypeCode))
            {
                oldEntityPM.TapagTypeCode = entityPM.TapagTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterId))
            {
                oldEntityPM.ImporterId = entityPM.ImporterId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBranchCode))
            {
                oldEntityPM.CustomsBranchCode = entityPM.CustomsBranchCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfessionUnitTypeCode))
            {
                oldEntityPM.ProfessionUnitTypeCode = entityPM.ProfessionUnitTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecializationTypeCode))
            {
                oldEntityPM.SpecializationTypeCode = entityPM.SpecializationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FollowDate))
            {
                oldEntityPM.FollowDate = entityPM.FollowDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValidityDate))
            {
                oldEntityPM.ValidityDate = entityPM.ValidityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferantId))
            {
                oldEntityPM.ReferantId = entityPM.ReferantId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TapagPM entityPM)
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
	 