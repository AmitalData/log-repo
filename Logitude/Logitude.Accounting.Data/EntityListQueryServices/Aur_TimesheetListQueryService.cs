	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class Aur_TimesheetListQueryService
    {
	    private IQueryable<Aur_TimesheetList> GetIqueryableList(IQueryable<Aur_Timesheet> iQueryable)
        {
		IQueryable<Aur_TimesheetList> query = (from a in iQueryable
                                            select new Aur_TimesheetList()
											{
                     
					                          Line = a.Line,
					
					                          Name = a.Name,
					
					                          UserReport = a.UserReport,
					
					                          ExecutionDate = a.ExecutionDate,
					
					                          RelatedProject = a.RelatedProject,
					
					                          ProjectNumber = a.ProjectNumber,
					
					                          ApprovesRelatedWork = a.ApprovesRelatedWork,
					
					                          CRMRelatedWork = a.CRMRelatedWork,
					
					                          CRMContactperson = a.CRMContactperson,
					
					                          ConfirmRequestCRM = a.ConfirmRequestCRM,
					
					                          RelatedTask = a.RelatedTask,
					
					                          WorkType = a.WorkType,
					
					                          CompletedEffort = a.CompletedEffort,
					
					                          BillableHours = a.BillableHours,
					
					                          EmployeeType = a.EmployeeType,
					
					                          HourlyRate = a.HourlyRate,
					
		                    	            });
            return query;
		}

		private IQueryable<Aur_Timesheet> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Aur_Timesheet> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<Aur_Timesheet> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Aur_Timesheet> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	