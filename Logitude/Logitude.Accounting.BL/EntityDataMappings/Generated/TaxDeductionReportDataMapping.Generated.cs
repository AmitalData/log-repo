
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxDeductionReportDataMapping: IMapping<TaxDeductionReportPM, TaxDeductionReport>,IMappingEncodeBase64NVARCHARFields<TaxDeductionReportPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         ReportNumber, 
	         StatusTypeCode, 
	         IsAdditionalReportExist, 
	         TaxYear, 
	         Email, 
	         ErrorMessage,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         ReportNumber, 
	         StatusTypeCode, 
	         IsAdditionalReportExist, 
	         TaxYear, 
	         Email, 
	         ErrorMessage, 
	         CreatedByUser, 
	         Status, 
	         StatusLocalName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TaxDeductionReportPM entityPM, TaxDeductionReport entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportNumber))
            {
				entityPOCO.ReportNumber = entityPM.ReportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusTypeCode))
            {
				entityPOCO.StatusTypeCode = entityPM.StatusTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAdditionalReportExist))
            {
				entityPOCO.IsAdditionalReportExist = entityPM.IsAdditionalReportExist;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxYear))
            {
				entityPOCO.TaxYear = entityPM.TaxYear;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Email))
            {
				entityPOCO.Email = entityPM.Email;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorMessage))
            {
				entityPOCO.ErrorMessage = entityPM.ErrorMessage;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TaxDeductionReportPM entityPM, TaxDeductionReport entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReportNumber))
            {
					entityPM.ReportNumber = entityPOCO.ReportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusTypeCode))
            {
					entityPM.StatusTypeCode = entityPOCO.StatusTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAdditionalReportExist))
            {
					entityPM.IsAdditionalReportExist = entityPOCO.IsAdditionalReportExist;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxYear))
            {
					entityPM.TaxYear = entityPOCO.TaxYear;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Email))
            {
					entityPM.Email = entityPOCO.Email;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrorMessage))
            {
					entityPM.ErrorMessage = entityPOCO.ErrorMessage;
            }

		}

		public void PMToOldPM(TaxDeductionReportPM entityPM, TaxDeductionReportPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportNumber))
            {
                oldEntityPM.ReportNumber = entityPM.ReportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusTypeCode))
            {
                oldEntityPM.StatusTypeCode = entityPM.StatusTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAdditionalReportExist))
            {
                oldEntityPM.IsAdditionalReportExist = entityPM.IsAdditionalReportExist;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxYear))
            {
                oldEntityPM.TaxYear = entityPM.TaxYear;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Email))
            {
                oldEntityPM.Email = entityPM.Email;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrorMessage))
            {
                oldEntityPM.ErrorMessage = entityPM.ErrorMessage;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TaxDeductionReportPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ErrorMessage)) //T4 find type == nText 
            {
                entityPM.ErrorMessage = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ErrorMessage));
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
		
		private void BuildSearchFieldsGenerated(TaxDeductionReportPM entityPM, TaxDeductionReport entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 