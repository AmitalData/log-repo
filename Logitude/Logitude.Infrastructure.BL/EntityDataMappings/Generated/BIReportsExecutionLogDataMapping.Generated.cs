
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class BIReportsExecutionLogDataMapping: IMapping<BIReportsExecutionLogPM, BIReportsExecutionLog>,IMappingEncodeBase64NVARCHARFields<BIReportsExecutionLogPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         StatusCode, 
	         ExceptionMessage, 
	         DoneDate, 
	         ReportFilterXML, 
	         BIReportId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         StatusCode, 
	         ExceptionMessage, 
	         DoneDate, 
	         ReportFilterXML, 
	         BIReportId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BIReportsExecutionLogPM entityPM, BIReportsExecutionLog entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
				entityPOCO.StatusCode = entityPM.StatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionMessage))
            {
				entityPOCO.ExceptionMessage = entityPM.ExceptionMessage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DoneDate))
            {
				entityPOCO.DoneDate = entityPM.DoneDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportFilterXML))
            {
				entityPOCO.ReportFilterXML = entityPM.ReportFilterXML;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BIReportId))
            {
				entityPOCO.BIReportId = entityPM.BIReportId;
			}
			}

		public void POCOToPM(BIReportsExecutionLogPM entityPM, BIReportsExecutionLog entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StatusCode))
            {
					entityPM.StatusCode = entityPOCO.StatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExceptionMessage))
            {
					entityPM.ExceptionMessage = entityPOCO.ExceptionMessage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DoneDate))
            {
					entityPM.DoneDate = entityPOCO.DoneDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReportFilterXML))
            {
					entityPM.ReportFilterXML = entityPOCO.ReportFilterXML;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BIReportId))
            {
					entityPM.BIReportId = entityPOCO.BIReportId;
            }

		}

		public void PMToOldPM(BIReportsExecutionLogPM entityPM, BIReportsExecutionLogPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                oldEntityPM.StatusCode = entityPM.StatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExceptionMessage))
            {
                oldEntityPM.ExceptionMessage = entityPM.ExceptionMessage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DoneDate))
            {
                oldEntityPM.DoneDate = entityPM.DoneDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReportFilterXML))
            {
                oldEntityPM.ReportFilterXML = entityPM.ReportFilterXML;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BIReportId))
            {
                oldEntityPM.BIReportId = entityPM.BIReportId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BIReportsExecutionLogPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.ExceptionMessage)) //T4 find type == nText 
            {
                entityPM.ExceptionMessage = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ExceptionMessage));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ReportFilterXML)) //T4 find type == nText 
            {
                entityPM.ReportFilterXML = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ReportFilterXML));
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
	 