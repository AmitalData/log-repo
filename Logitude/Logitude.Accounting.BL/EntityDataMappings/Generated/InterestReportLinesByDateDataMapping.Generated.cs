
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
   
   public partial class InterestReportLinesByDateDataMapping: IMapping<InterestReportLinesByDatePM, InterestReportLinesByDate>,IMappingEncodeBase64NVARCHARFields<InterestReportLinesByDatePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         InterestReportId, 
	         LineNumber, 
	         FromDate, 
	         ToDate, 
	         TotalInterestDays, 
	         TotalAmount, 
	         AccumulatedAmount, 
	         StandardInterestPercentage, 
	         ExceptionalInterestPercentage, 
	         CreditInterestPercentage, 
	         StandardInterestAmount, 
	         ExceptionalInterestAmount, 
	         CreditInterestAmount, 
	         CalculatedStandInterestAmount, 
	         CalculatedExcepInterestAmount, 
	         CalculatedCreditInterestAmount, 
	         CalculationDetails,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         InterestReportId, 
	         LineNumber, 
	         FromDate, 
	         ToDate, 
	         TotalInterestDays, 
	         TotalAmount, 
	         AccumulatedAmount, 
	         StandardInterestPercentage, 
	         ExceptionalInterestPercentage, 
	         CreditInterestPercentage, 
	         StandardInterestAmount, 
	         ExceptionalInterestAmount, 
	         CreditInterestAmount, 
	         CalculatedStandInterestAmount, 
	         CalculatedExcepInterestAmount, 
	         CalculatedCreditInterestAmount, 
	         CalculationDetails,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(InterestReportLinesByDatePM entityPM, InterestReportLinesByDate entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestReportId))
            {
				entityPOCO.InterestReportId = entityPM.InterestReportId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
				entityPOCO.LineNumber = entityPM.LineNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
				entityPOCO.FromDate = entityPM.FromDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
				entityPOCO.ToDate = entityPM.ToDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalInterestDays))
            {
				entityPOCO.TotalInterestDays = entityPM.TotalInterestDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalAmount))
            {
				entityPOCO.TotalAmount = entityPM.TotalAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccumulatedAmount))
            {
				entityPOCO.AccumulatedAmount = entityPM.AccumulatedAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardInterestPercentage))
            {
				entityPOCO.StandardInterestPercentage = entityPM.StandardInterestPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalInterestPercentage))
            {
				entityPOCO.ExceptionalInterestPercentage = entityPM.ExceptionalInterestPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditInterestPercentage))
            {
				entityPOCO.CreditInterestPercentage = entityPM.CreditInterestPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardInterestAmount))
            {
				entityPOCO.StandardInterestAmount = entityPM.StandardInterestAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalInterestAmount))
            {
				entityPOCO.ExceptionalInterestAmount = entityPM.ExceptionalInterestAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditInterestAmount))
            {
				entityPOCO.CreditInterestAmount = entityPM.CreditInterestAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedStandInterestAmount))
            {
				entityPOCO.CalculatedStandInterestAmount = entityPM.CalculatedStandInterestAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedExcepInterestAmount))
            {
				entityPOCO.CalculatedExcepInterestAmount = entityPM.CalculatedExcepInterestAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedCreditInterestAmount))
            {
				entityPOCO.CalculatedCreditInterestAmount = entityPM.CalculatedCreditInterestAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculationDetails))
            {
				entityPOCO.CalculationDetails = entityPM.CalculationDetails;
			}
			}

		public void POCOToPM(InterestReportLinesByDatePM entityPM, InterestReportLinesByDate entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterestReportId))
            {
					entityPM.InterestReportId = entityPOCO.InterestReportId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromDate))
            {
					entityPM.FromDate = entityPOCO.FromDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToDate))
            {
					entityPM.ToDate = entityPOCO.ToDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalInterestDays))
            {
					entityPM.TotalInterestDays = entityPOCO.TotalInterestDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalAmount))
            {
					entityPM.TotalAmount = entityPOCO.TotalAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccumulatedAmount))
            {
					entityPM.AccumulatedAmount = entityPOCO.AccumulatedAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StandardInterestPercentage))
            {
					entityPM.StandardInterestPercentage = entityPOCO.StandardInterestPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExceptionalInterestPercentage))
            {
					entityPM.ExceptionalInterestPercentage = entityPOCO.ExceptionalInterestPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditInterestPercentage))
            {
					entityPM.CreditInterestPercentage = entityPOCO.CreditInterestPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StandardInterestAmount))
            {
					entityPM.StandardInterestAmount = entityPOCO.StandardInterestAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExceptionalInterestAmount))
            {
					entityPM.ExceptionalInterestAmount = entityPOCO.ExceptionalInterestAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreditInterestAmount))
            {
					entityPM.CreditInterestAmount = entityPOCO.CreditInterestAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CalculatedStandInterestAmount))
            {
					entityPM.CalculatedStandInterestAmount = entityPOCO.CalculatedStandInterestAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CalculatedExcepInterestAmount))
            {
					entityPM.CalculatedExcepInterestAmount = entityPOCO.CalculatedExcepInterestAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CalculatedCreditInterestAmount))
            {
					entityPM.CalculatedCreditInterestAmount = entityPOCO.CalculatedCreditInterestAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CalculationDetails))
            {
					entityPM.CalculationDetails = entityPOCO.CalculationDetails;
            }

		}

		public void PMToOldPM(InterestReportLinesByDatePM entityPM, InterestReportLinesByDatePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterestReportId))
            {
                oldEntityPM.InterestReportId = entityPM.InterestReportId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineNumber))
            {
                oldEntityPM.LineNumber = entityPM.LineNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
                oldEntityPM.FromDate = entityPM.FromDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
                oldEntityPM.ToDate = entityPM.ToDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalInterestDays))
            {
                oldEntityPM.TotalInterestDays = entityPM.TotalInterestDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalAmount))
            {
                oldEntityPM.TotalAmount = entityPM.TotalAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccumulatedAmount))
            {
                oldEntityPM.AccumulatedAmount = entityPM.AccumulatedAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardInterestPercentage))
            {
                oldEntityPM.StandardInterestPercentage = entityPM.StandardInterestPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalInterestPercentage))
            {
                oldEntityPM.ExceptionalInterestPercentage = entityPM.ExceptionalInterestPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditInterestPercentage))
            {
                oldEntityPM.CreditInterestPercentage = entityPM.CreditInterestPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StandardInterestAmount))
            {
                oldEntityPM.StandardInterestAmount = entityPM.StandardInterestAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionalInterestAmount))
            {
                oldEntityPM.ExceptionalInterestAmount = entityPM.ExceptionalInterestAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreditInterestAmount))
            {
                oldEntityPM.CreditInterestAmount = entityPM.CreditInterestAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedStandInterestAmount))
            {
                oldEntityPM.CalculatedStandInterestAmount = entityPM.CalculatedStandInterestAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedExcepInterestAmount))
            {
                oldEntityPM.CalculatedExcepInterestAmount = entityPM.CalculatedExcepInterestAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculatedCreditInterestAmount))
            {
                oldEntityPM.CalculatedCreditInterestAmount = entityPM.CalculatedCreditInterestAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CalculationDetails))
            {
                oldEntityPM.CalculationDetails = entityPM.CalculationDetails;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(InterestReportLinesByDatePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.CalculationDetails)) //T4 find type == nText 
            {
                entityPM.CalculationDetails = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CalculationDetails));
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
	 