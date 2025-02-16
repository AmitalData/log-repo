
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
   
   public partial class Aur_TimesheetDataMapping: IMapping<Aur_TimesheetPM, Aur_Timesheet>,IMappingEncodeBase64NVARCHARFields<Aur_TimesheetPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Line, 
	         Name, 
	         UserReport, 
	         ExecutionDate, 
	         RelatedProject, 
	         ProjectNumber, 
	         ApprovesRelatedWork, 
	         CRMRelatedWork, 
	         CRMContactperson, 
	         ConfirmRequestCRM, 
	         RelatedTask, 
	         WorkType, 
	         CompletedEffort, 
	         BillableHours, 
	         EmployeeType, 
	         HourlyRate, 
	         PaymentId, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Line, 
	         Name, 
	         UserReport, 
	         ExecutionDate, 
	         RelatedProject, 
	         ProjectNumber, 
	         ApprovesRelatedWork, 
	         CRMRelatedWork, 
	         CRMContactperson, 
	         ConfirmRequestCRM, 
	         RelatedTask, 
	         WorkType, 
	         CompletedEffort, 
	         BillableHours, 
	         EmployeeType, 
	         HourlyRate, 
	         PaymentId, 
	         Tenant,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(Aur_TimesheetPM entityPM, Aur_Timesheet entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserReport))
            {
				entityPOCO.UserReport = entityPM.UserReport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExecutionDate))
            {
				entityPOCO.ExecutionDate = entityPM.ExecutionDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RelatedProject))
            {
				entityPOCO.RelatedProject = entityPM.RelatedProject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProjectNumber))
            {
				entityPOCO.ProjectNumber = entityPM.ProjectNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovesRelatedWork))
            {
				entityPOCO.ApprovesRelatedWork = entityPM.ApprovesRelatedWork;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CRMRelatedWork))
            {
				entityPOCO.CRMRelatedWork = entityPM.CRMRelatedWork;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CRMContactperson))
            {
				entityPOCO.CRMContactperson = entityPM.CRMContactperson;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConfirmRequestCRM))
            {
				entityPOCO.ConfirmRequestCRM = entityPM.ConfirmRequestCRM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RelatedTask))
            {
				entityPOCO.RelatedTask = entityPM.RelatedTask;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkType))
            {
				entityPOCO.WorkType = entityPM.WorkType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompletedEffort))
            {
				entityPOCO.CompletedEffort = entityPM.CompletedEffort;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BillableHours))
            {
				entityPOCO.BillableHours = entityPM.BillableHours;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeType))
            {
				entityPOCO.EmployeeType = entityPM.EmployeeType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HourlyRate))
            {
				entityPOCO.HourlyRate = entityPM.HourlyRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(Aur_TimesheetPM entityPM, Aur_Timesheet entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Line))
            {
					entityPM.Line = entityPOCO.Line;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserReport))
            {
					entityPM.UserReport = entityPOCO.UserReport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExecutionDate))
            {
					entityPM.ExecutionDate = entityPOCO.ExecutionDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RelatedProject))
            {
					entityPM.RelatedProject = entityPOCO.RelatedProject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProjectNumber))
            {
					entityPM.ProjectNumber = entityPOCO.ProjectNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ApprovesRelatedWork))
            {
					entityPM.ApprovesRelatedWork = entityPOCO.ApprovesRelatedWork;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CRMRelatedWork))
            {
					entityPM.CRMRelatedWork = entityPOCO.CRMRelatedWork;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CRMContactperson))
            {
					entityPM.CRMContactperson = entityPOCO.CRMContactperson;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConfirmRequestCRM))
            {
					entityPM.ConfirmRequestCRM = entityPOCO.ConfirmRequestCRM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RelatedTask))
            {
					entityPM.RelatedTask = entityPOCO.RelatedTask;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WorkType))
            {
					entityPM.WorkType = entityPOCO.WorkType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CompletedEffort))
            {
					entityPM.CompletedEffort = entityPOCO.CompletedEffort;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BillableHours))
            {
					entityPM.BillableHours = entityPOCO.BillableHours;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmployeeType))
            {
					entityPM.EmployeeType = entityPOCO.EmployeeType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HourlyRate))
            {
					entityPM.HourlyRate = entityPOCO.HourlyRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentId))
            {
					entityPM.PaymentId = entityPOCO.PaymentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(Aur_TimesheetPM entityPM, Aur_TimesheetPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserReport))
            {
                oldEntityPM.UserReport = entityPM.UserReport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExecutionDate))
            {
                oldEntityPM.ExecutionDate = entityPM.ExecutionDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RelatedProject))
            {
                oldEntityPM.RelatedProject = entityPM.RelatedProject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProjectNumber))
            {
                oldEntityPM.ProjectNumber = entityPM.ProjectNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ApprovesRelatedWork))
            {
                oldEntityPM.ApprovesRelatedWork = entityPM.ApprovesRelatedWork;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CRMRelatedWork))
            {
                oldEntityPM.CRMRelatedWork = entityPM.CRMRelatedWork;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CRMContactperson))
            {
                oldEntityPM.CRMContactperson = entityPM.CRMContactperson;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConfirmRequestCRM))
            {
                oldEntityPM.ConfirmRequestCRM = entityPM.ConfirmRequestCRM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RelatedTask))
            {
                oldEntityPM.RelatedTask = entityPM.RelatedTask;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WorkType))
            {
                oldEntityPM.WorkType = entityPM.WorkType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CompletedEffort))
            {
                oldEntityPM.CompletedEffort = entityPM.CompletedEffort;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BillableHours))
            {
                oldEntityPM.BillableHours = entityPM.BillableHours;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeType))
            {
                oldEntityPM.EmployeeType = entityPM.EmployeeType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HourlyRate))
            {
                oldEntityPM.HourlyRate = entityPM.HourlyRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(Aur_TimesheetPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.UserReport)) //T4 find type == nText 
            {
                entityPM.UserReport = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.UserReport));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.RelatedProject)) //T4 find type == nText 
            {
                entityPM.RelatedProject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.RelatedProject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ApprovesRelatedWork)) //T4 find type == nText 
            {
                entityPM.ApprovesRelatedWork = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ApprovesRelatedWork));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CRMContactperson)) //T4 find type == nText 
            {
                entityPM.CRMContactperson = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CRMContactperson));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ConfirmRequestCRM)) //T4 find type == nText 
            {
                entityPM.ConfirmRequestCRM = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ConfirmRequestCRM));
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
	 