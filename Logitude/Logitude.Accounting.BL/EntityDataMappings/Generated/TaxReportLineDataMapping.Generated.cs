
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
   
   public partial class TaxReportLineDataMapping: IMapping<TaxReportLinePM, TaxReportLine>,IMappingEncodeBase64NVARCHARFields<TaxReportLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         LastUpdateDateTime, 
	         UpdatedByUserId, 
	         SearchFields, 
	         TaxReportId, 
	         Line, 
	         OutputOrInput, 
	         LineTypeCode, 
	         VatNumber, 
	         Reference, 
	         ReferecneGroup, 
	         ReferenceDate, 
	         VatAmount, 
	         VatableInvoiceAmount, 
	         StatusCode, 
	         TransmitStatusCode, 
	         JournalId, 
	         IsManuallyChanged, 
	         IsEquipment, 
	         TaxReportDate, 
	         IsExternalLine, 
	         TotalInvoiceAmount, 
	         OriginalReference,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         LastUpdateDateTime, 
	         UpdatedByUserId, 
	         SearchFields, 
	         TaxReportId, 
	         Line, 
	         OutputOrInput, 
	         LineTypeCode, 
	         VatNumber, 
	         Reference, 
	         ReferecneGroup, 
	         ReferenceDate, 
	         VatAmount, 
	         VatableInvoiceAmount, 
	         StatusCode, 
	         TransmitStatusCode, 
	         JournalId, 
	         IsManuallyChanged, 
	         IsEquipment, 
	         StatusLocalName, 
	         StatusEnglishName, 
	         JournalNumber, 
	         TaxReportDate, 
	         IsExternalLine, 
	         TotalInvoiceAmount, 
	         OriginalReference, 
	         UpdatedBUserName, 
	         JournalLineNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDateTime))
            {
				entityPOCO.LastUpdateDateTime = entityPM.LastUpdateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputOrInput))
            {
				entityPOCO.OutputOrInput = entityPM.OutputOrInput;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineTypeCode))
            {
				entityPOCO.LineTypeCode = entityPM.LineTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatNumber))
            {
				entityPOCO.VatNumber = entityPM.VatNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Reference))
            {
				entityPOCO.Reference = entityPM.Reference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferecneGroup))
            {
				entityPOCO.ReferecneGroup = entityPM.ReferecneGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferenceDate))
            {
				entityPOCO.ReferenceDate = entityPM.ReferenceDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatAmount))
            {
				entityPOCO.VatAmount = entityPM.VatAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatableInvoiceAmount))
            {
				entityPOCO.VatableInvoiceAmount = entityPM.VatableInvoiceAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransmitStatusCode))
            {
				entityPOCO.TransmitStatusCode = entityPM.TransmitStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JournalId))
            {
				entityPOCO.JournalId = entityPM.JournalId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsManuallyChanged))
            {
				entityPOCO.IsManuallyChanged = entityPM.IsManuallyChanged;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEquipment))
            {
				entityPOCO.IsEquipment = entityPM.IsEquipment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportDate))
            {
				entityPOCO.TaxReportDate = entityPM.TaxReportDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExternalLine))
            {
				entityPOCO.IsExternalLine = entityPM.IsExternalLine;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalInvoiceAmount))
            {
				entityPOCO.TotalInvoiceAmount = entityPM.TotalInvoiceAmount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalReference))
            {
				entityPOCO.OriginalReference = entityPM.OriginalReference;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdateDateTime))
            {
					entityPM.LastUpdateDateTime = entityPOCO.LastUpdateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportId))
            {
					entityPM.TaxReportId = entityPOCO.TaxReportId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OutputOrInput))
            {
					entityPM.OutputOrInput = entityPOCO.OutputOrInput;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineTypeCode))
            {
					entityPM.LineTypeCode = entityPOCO.LineTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatNumber))
            {
					entityPM.VatNumber = entityPOCO.VatNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Reference))
            {
					entityPM.Reference = entityPOCO.Reference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReferecneGroup))
            {
					entityPM.ReferecneGroup = entityPOCO.ReferecneGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReferenceDate))
            {
					entityPM.ReferenceDate = entityPOCO.ReferenceDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatAmount))
            {
					entityPM.VatAmount = entityPOCO.VatAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VatableInvoiceAmount))
            {
					entityPM.VatableInvoiceAmount = entityPOCO.VatableInvoiceAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransmitStatusCode))
            {
					entityPM.TransmitStatusCode = entityPOCO.TransmitStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.JournalId))
            {
					entityPM.JournalId = entityPOCO.JournalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsManuallyChanged))
            {
					entityPM.IsManuallyChanged = entityPOCO.IsManuallyChanged;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsEquipment))
            {
					entityPM.IsEquipment = entityPOCO.IsEquipment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxReportDate))
            {
					entityPM.TaxReportDate = entityPOCO.TaxReportDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExternalLine))
            {
					entityPM.IsExternalLine = entityPOCO.IsExternalLine;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalInvoiceAmount))
            {
					entityPM.TotalInvoiceAmount = entityPOCO.TotalInvoiceAmount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginalReference))
            {
					entityPM.OriginalReference = entityPOCO.OriginalReference;
            }

		}

		public void PMToOldPM(TaxReportLinePM entityPM, TaxReportLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDateTime))
            {
                oldEntityPM.LastUpdateDateTime = entityPM.LastUpdateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OutputOrInput))
            {
                oldEntityPM.OutputOrInput = entityPM.OutputOrInput;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LineTypeCode))
            {
                oldEntityPM.LineTypeCode = entityPM.LineTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatNumber))
            {
                oldEntityPM.VatNumber = entityPM.VatNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Reference))
            {
                oldEntityPM.Reference = entityPM.Reference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferecneGroup))
            {
                oldEntityPM.ReferecneGroup = entityPM.ReferecneGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferenceDate))
            {
                oldEntityPM.ReferenceDate = entityPM.ReferenceDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatAmount))
            {
                oldEntityPM.VatAmount = entityPM.VatAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VatableInvoiceAmount))
            {
                oldEntityPM.VatableInvoiceAmount = entityPM.VatableInvoiceAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransmitStatusCode))
            {
                oldEntityPM.TransmitStatusCode = entityPM.TransmitStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.JournalId))
            {
                oldEntityPM.JournalId = entityPM.JournalId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsManuallyChanged))
            {
                oldEntityPM.IsManuallyChanged = entityPM.IsManuallyChanged;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsEquipment))
            {
                oldEntityPM.IsEquipment = entityPM.IsEquipment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxReportDate))
            {
                oldEntityPM.TaxReportDate = entityPM.TaxReportDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExternalLine))
            {
                oldEntityPM.IsExternalLine = entityPM.IsExternalLine;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalInvoiceAmount))
            {
                oldEntityPM.TotalInvoiceAmount = entityPM.TotalInvoiceAmount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginalReference))
            {
                oldEntityPM.OriginalReference = entityPM.OriginalReference;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TaxReportLinePM entityPM)
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
		
		private void BuildSearchFieldsGenerated(TaxReportLinePM entityPM, TaxReportLine entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 