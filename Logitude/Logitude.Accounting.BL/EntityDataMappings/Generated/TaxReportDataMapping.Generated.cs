
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
   
   public partial class TaxReportDataMapping: IMapping<TaxReportPM, TaxReport>,IMappingEncodeBase64NVARCHARFields<TaxReportPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         LastUpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         TaxReportMonth, 
	         TaxReportNumber, 
	         VatNumber, 
	         TaxReportTypeCode, 
	         IsCancelled, 
	         TaxableOutputAmount, 
	         OutputTaxAmount, 
	         TaxableOutputsWithDiffPercent, 
	         OutputTaxAmountWithDiffPercent, 
	         ExemptTaxableOutput, 
	         OutputLinesCount, 
	         OtherInputsTaxAmount, 
	         EquipmentInputsTaxAmount, 
	         InputLinesCount, 
	         AmountForPayRefund, 
	         StatusCode, 
	         ProcessStartDate, 
	         ProcessEndDate, 
	         ProcessProgress, 
	         NeedsRebulid, 
	         CreatedInTwoMonthsLogic, 
	         OutputTaxAmountRound, 
	         InputsTaxAmountRound,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         LastUpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         TaxReportMonth, 
	         TaxReportNumber, 
	         VatNumber, 
	         TaxReportTypeCode, 
	         IsCancelled, 
	         TaxableOutputAmount, 
	         OutputTaxAmount, 
	         TaxableOutputsWithDiffPercent, 
	         OutputTaxAmountWithDiffPercent, 
	         ExemptTaxableOutput, 
	         OutputLinesCount, 
	         OtherInputsTaxAmount, 
	         EquipmentInputsTaxAmount, 
	         InputLinesCount, 
	         AmountForPayRefund, 
	         StatusCode, 
	         ProcessStartDate, 
	         ProcessEndDate, 
	         ProcessProgress, 
	         StatusLocalName, 
	         CreatedByUserName, 
	         Year, 
	         StatusEnglishName, 
	         TaxReportLineLastLine, 
	         NeedsRebulid, 
	         IsNew, 
	         UpdatedByUserName, 
	         CreatedInTwoMonthsLogic, 
	         OutputTaxAmountRound, 
	         InputsTaxAmountRound,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TaxReportPM entityPM, TaxReport entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
				entityPOCO.LastUpdateDate = entityPM.LastUpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportMonth))
            {
				entityPOCO.TaxReportMonth = entityPM.TaxReportMonth;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportNumber))
            {
				entityPOCO.TaxReportNumber = entityPM.TaxReportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatNumber))
            {
				entityPOCO.VatNumber = entityPM.VatNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportTypeCode))
            {
				entityPOCO.TaxReportTypeCode = entityPM.TaxReportTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxableOutputAmount))
            {
				entityPOCO.TaxableOutputAmount = entityPM.TaxableOutputAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputTaxAmount))
            {
				entityPOCO.OutputTaxAmount = entityPM.OutputTaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxableOutputsWithDiffPercent))
            {
				entityPOCO.TaxableOutputsWithDiffPercent = entityPM.TaxableOutputsWithDiffPercent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputTaxAmountWithDiffPercent))
            {
				entityPOCO.OutputTaxAmountWithDiffPercent = entityPM.OutputTaxAmountWithDiffPercent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExemptTaxableOutput))
            {
				entityPOCO.ExemptTaxableOutput = entityPM.ExemptTaxableOutput;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputLinesCount))
            {
				entityPOCO.OutputLinesCount = entityPM.OutputLinesCount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OtherInputsTaxAmount))
            {
				entityPOCO.OtherInputsTaxAmount = entityPM.OtherInputsTaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EquipmentInputsTaxAmount))
            {
				entityPOCO.EquipmentInputsTaxAmount = entityPM.EquipmentInputsTaxAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InputLinesCount))
            {
				entityPOCO.InputLinesCount = entityPM.InputLinesCount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmountForPayRefund))
            {
				entityPOCO.AmountForPayRefund = entityPM.AmountForPayRefund;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessStartDate))
            {
				entityPOCO.ProcessStartDate = entityPM.ProcessStartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessEndDate))
            {
				entityPOCO.ProcessEndDate = entityPM.ProcessEndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessProgress))
            {
				entityPOCO.ProcessProgress = entityPM.ProcessProgress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedsRebulid))
            {
				entityPOCO.NeedsRebulid = entityPM.NeedsRebulid;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedInTwoMonthsLogic))
            {
				entityPOCO.CreatedInTwoMonthsLogic = entityPM.CreatedInTwoMonthsLogic;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputTaxAmountRound))
            {
				entityPOCO.OutputTaxAmountRound = entityPM.OutputTaxAmountRound;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InputsTaxAmountRound))
            {
				entityPOCO.InputsTaxAmountRound = entityPM.InputsTaxAmountRound;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TaxReportPM entityPM, TaxReport entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdateDate))
            {
					entityPM.LastUpdateDate = entityPOCO.LastUpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportMonth))
            {
					entityPM.TaxReportMonth = entityPOCO.TaxReportMonth;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportNumber))
            {
					entityPM.TaxReportNumber = entityPOCO.TaxReportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatNumber))
            {
					entityPM.VatNumber = entityPOCO.VatNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportTypeCode))
            {
					entityPM.TaxReportTypeCode = entityPOCO.TaxReportTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxableOutputAmount))
            {
					entityPM.TaxableOutputAmount = entityPOCO.TaxableOutputAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutputTaxAmount))
            {
					entityPM.OutputTaxAmount = entityPOCO.OutputTaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxableOutputsWithDiffPercent))
            {
					entityPM.TaxableOutputsWithDiffPercent = entityPOCO.TaxableOutputsWithDiffPercent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutputTaxAmountWithDiffPercent))
            {
					entityPM.OutputTaxAmountWithDiffPercent = entityPOCO.OutputTaxAmountWithDiffPercent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExemptTaxableOutput))
            {
					entityPM.ExemptTaxableOutput = entityPOCO.ExemptTaxableOutput;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutputLinesCount))
            {
					entityPM.OutputLinesCount = entityPOCO.OutputLinesCount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OtherInputsTaxAmount))
            {
					entityPM.OtherInputsTaxAmount = entityPOCO.OtherInputsTaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EquipmentInputsTaxAmount))
            {
					entityPM.EquipmentInputsTaxAmount = entityPOCO.EquipmentInputsTaxAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InputLinesCount))
            {
					entityPM.InputLinesCount = entityPOCO.InputLinesCount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmountForPayRefund))
            {
					entityPM.AmountForPayRefund = entityPOCO.AmountForPayRefund;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProcessStartDate))
            {
					entityPM.ProcessStartDate = entityPOCO.ProcessStartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProcessEndDate))
            {
					entityPM.ProcessEndDate = entityPOCO.ProcessEndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProcessProgress))
            {
					entityPM.ProcessProgress = entityPOCO.ProcessProgress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NeedsRebulid))
            {
					entityPM.NeedsRebulid = entityPOCO.NeedsRebulid;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedInTwoMonthsLogic))
            {
					entityPM.CreatedInTwoMonthsLogic = entityPOCO.CreatedInTwoMonthsLogic;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutputTaxAmountRound))
            {
					entityPM.OutputTaxAmountRound = entityPOCO.OutputTaxAmountRound;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InputsTaxAmountRound))
            {
					entityPM.InputsTaxAmountRound = entityPOCO.InputsTaxAmountRound;
            }

		}

		public void PMToOldPM(TaxReportPM entityPM, TaxReportPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
                oldEntityPM.LastUpdateDate = entityPM.LastUpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportMonth))
            {
                oldEntityPM.TaxReportMonth = entityPM.TaxReportMonth;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportNumber))
            {
                oldEntityPM.TaxReportNumber = entityPM.TaxReportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatNumber))
            {
                oldEntityPM.VatNumber = entityPM.VatNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportTypeCode))
            {
                oldEntityPM.TaxReportTypeCode = entityPM.TaxReportTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxableOutputAmount))
            {
                oldEntityPM.TaxableOutputAmount = entityPM.TaxableOutputAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputTaxAmount))
            {
                oldEntityPM.OutputTaxAmount = entityPM.OutputTaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxableOutputsWithDiffPercent))
            {
                oldEntityPM.TaxableOutputsWithDiffPercent = entityPM.TaxableOutputsWithDiffPercent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputTaxAmountWithDiffPercent))
            {
                oldEntityPM.OutputTaxAmountWithDiffPercent = entityPM.OutputTaxAmountWithDiffPercent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExemptTaxableOutput))
            {
                oldEntityPM.ExemptTaxableOutput = entityPM.ExemptTaxableOutput;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputLinesCount))
            {
                oldEntityPM.OutputLinesCount = entityPM.OutputLinesCount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OtherInputsTaxAmount))
            {
                oldEntityPM.OtherInputsTaxAmount = entityPM.OtherInputsTaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EquipmentInputsTaxAmount))
            {
                oldEntityPM.EquipmentInputsTaxAmount = entityPM.EquipmentInputsTaxAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InputLinesCount))
            {
                oldEntityPM.InputLinesCount = entityPM.InputLinesCount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmountForPayRefund))
            {
                oldEntityPM.AmountForPayRefund = entityPM.AmountForPayRefund;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessStartDate))
            {
                oldEntityPM.ProcessStartDate = entityPM.ProcessStartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessEndDate))
            {
                oldEntityPM.ProcessEndDate = entityPM.ProcessEndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcessProgress))
            {
                oldEntityPM.ProcessProgress = entityPM.ProcessProgress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedsRebulid))
            {
                oldEntityPM.NeedsRebulid = entityPM.NeedsRebulid;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedInTwoMonthsLogic))
            {
                oldEntityPM.CreatedInTwoMonthsLogic = entityPM.CreatedInTwoMonthsLogic;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputTaxAmountRound))
            {
                oldEntityPM.OutputTaxAmountRound = entityPM.OutputTaxAmountRound;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InputsTaxAmountRound))
            {
                oldEntityPM.InputsTaxAmountRound = entityPM.InputsTaxAmountRound;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TaxReportPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(TaxReportPM entityPM, TaxReport entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 