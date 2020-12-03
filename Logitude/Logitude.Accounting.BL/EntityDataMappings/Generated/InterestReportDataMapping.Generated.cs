
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
   
   public partial class InterestReportDataMapping: IMapping<InterestReportPM, InterestReport>,IMappingEncodeBase64NVARCHARFields<InterestReportPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDateTime, 
	         CreatedByUserId, 
	         UpdateDateTime, 
	         UpdatedByUserId, 
	         GLAccountId, 
	         ReportNumber, 
	         InterestCalculationDate, 
	         TotalAmount, 
	         OpenBalance, 
	         CloseBalance, 
	         ARinvoiceId, 
	         InvoiceAmount, 
	         GLAccountInterestCreditLimit, 
	         InterestReportStatusCode, 
	         SearchFields, 
	         CustomerId, 
	         InvoiceFailureReason, 
	         CreditAllotmentPercentage, 
	         CalCreditAllotmentCommission,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDateTime, 
	         CreatedByUserId, 
	         UpdateDateTime, 
	         UpdatedByUserId, 
	         GLAccountId, 
	         ReportNumber, 
	         InterestCalculationDate, 
	         TotalAmount, 
	         OpenBalance, 
	         CloseBalance, 
	         ARinvoiceId, 
	         InvoiceAmount, 
	         GLAccountInterestCreditLimit, 
	         InterestReportStatusCode, 
	         CreatedByLocalName, 
	         GLAccountDisplayNumber, 
	         GLAccountLocalName, 
	         ARInvoiceNumber, 
	         UpdatedByLocalName, 
	         InterestReportStatusName, 
	         SearchFields, 
	         InterestReportStatusLocalName, 
	         CustomerId, 
	         CustomerName, 
	         GLAccountMinimumInterest, 
	         CustomerLocalName, 
	         EnableInvoiceing, 
	         IsFirstReport, 
	         InvoiceFailureReason, 
	         IsCreatedFromBatch, 
	         BatchReportUserEmail, 
	         RecalculateData, 
	         IsUpdatedFromBatch, 
	         CanRecalculate, 
	         InvoiceDate, 
	         IsNewReport, 
	         CreditAllotmentPercentage, 
	         CalCreditAllotmentCommission,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(InterestReportPM entityPM, InterestReport entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
				entityPOCO.CreateDateTime = entityPM.CreateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
				entityPOCO.UpdateDateTime = entityPM.UpdateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
				entityPOCO.GLAccountId = entityPM.GLAccountId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportNumber))
            {
				entityPOCO.ReportNumber = entityPM.ReportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestCalculationDate))
            {
				entityPOCO.InterestCalculationDate = entityPM.InterestCalculationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalAmount))
            {
				entityPOCO.TotalAmount = entityPM.TotalAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenBalance))
            {
				entityPOCO.OpenBalance = entityPM.OpenBalance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CloseBalance))
            {
				entityPOCO.CloseBalance = entityPM.CloseBalance;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ARinvoiceId))
            {
				entityPOCO.ARinvoiceId = entityPM.ARinvoiceId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceAmount))
            {
				entityPOCO.InvoiceAmount = entityPM.InvoiceAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountInterestCreditLimit))
            {
				entityPOCO.GLAccountInterestCreditLimit = entityPM.GLAccountInterestCreditLimit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestReportStatusCode))
            {
				entityPOCO.InterestReportStatusCode = entityPM.InterestReportStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceFailureReason))
            {
				entityPOCO.InvoiceFailureReason = entityPM.InvoiceFailureReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditAllotmentPercentage))
            {
				entityPOCO.CreditAllotmentPercentage = entityPM.CreditAllotmentPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalCreditAllotmentCommission))
            {
				entityPOCO.CalCreditAllotmentCommission = entityPM.CalCreditAllotmentCommission;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(InterestReportPM entityPM, InterestReport entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDateTime))
            {
					entityPM.CreateDateTime = entityPOCO.CreateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDateTime))
            {
					entityPM.UpdateDateTime = entityPOCO.UpdateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountId))
            {
					entityPM.GLAccountId = entityPOCO.GLAccountId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReportNumber))
            {
					entityPM.ReportNumber = entityPOCO.ReportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestCalculationDate))
            {
					entityPM.InterestCalculationDate = entityPOCO.InterestCalculationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalAmount))
            {
					entityPM.TotalAmount = entityPOCO.TotalAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenBalance))
            {
					entityPM.OpenBalance = entityPOCO.OpenBalance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CloseBalance))
            {
					entityPM.CloseBalance = entityPOCO.CloseBalance;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ARinvoiceId))
            {
					entityPM.ARinvoiceId = entityPOCO.ARinvoiceId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceAmount))
            {
					entityPM.InvoiceAmount = entityPOCO.InvoiceAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GLAccountInterestCreditLimit))
            {
					entityPM.GLAccountInterestCreditLimit = entityPOCO.GLAccountInterestCreditLimit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestReportStatusCode))
            {
					entityPM.InterestReportStatusCode = entityPOCO.InterestReportStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InvoiceFailureReason))
            {
					entityPM.InvoiceFailureReason = entityPOCO.InvoiceFailureReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditAllotmentPercentage))
            {
					entityPM.CreditAllotmentPercentage = entityPOCO.CreditAllotmentPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CalCreditAllotmentCommission))
            {
					entityPM.CalCreditAllotmentCommission = entityPOCO.CalCreditAllotmentCommission;
            }

		}

		public void PMToOldPM(InterestReportPM entityPM, InterestReportPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
                oldEntityPM.CreateDateTime = entityPM.CreateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
                oldEntityPM.UpdateDateTime = entityPM.UpdateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountId))
            {
                oldEntityPM.GLAccountId = entityPM.GLAccountId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportNumber))
            {
                oldEntityPM.ReportNumber = entityPM.ReportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestCalculationDate))
            {
                oldEntityPM.InterestCalculationDate = entityPM.InterestCalculationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalAmount))
            {
                oldEntityPM.TotalAmount = entityPM.TotalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenBalance))
            {
                oldEntityPM.OpenBalance = entityPM.OpenBalance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CloseBalance))
            {
                oldEntityPM.CloseBalance = entityPM.CloseBalance;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ARinvoiceId))
            {
                oldEntityPM.ARinvoiceId = entityPM.ARinvoiceId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceAmount))
            {
                oldEntityPM.InvoiceAmount = entityPM.InvoiceAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GLAccountInterestCreditLimit))
            {
                oldEntityPM.GLAccountInterestCreditLimit = entityPM.GLAccountInterestCreditLimit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestReportStatusCode))
            {
                oldEntityPM.InterestReportStatusCode = entityPM.InterestReportStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InvoiceFailureReason))
            {
                oldEntityPM.InvoiceFailureReason = entityPM.InvoiceFailureReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditAllotmentPercentage))
            {
                oldEntityPM.CreditAllotmentPercentage = entityPM.CreditAllotmentPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalCreditAllotmentCommission))
            {
                oldEntityPM.CalCreditAllotmentCommission = entityPM.CalCreditAllotmentCommission;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(InterestReportPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.InvoiceFailureReason)) //T4 find type == nText 
            {
                entityPM.InvoiceFailureReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.InvoiceFailureReason));
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
		
		private void BuildSearchFieldsGenerated(InterestReportPM entityPM, InterestReport entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 