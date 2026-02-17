using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data; 
using Logitude.TimeManagement.Data.EntityMapping;

namespace Logitude.TimeManagement.Data
{

    public interface ITimeManagementContext : IContext
    {
   
       	 IDbSet<Sprint> Sprints { get; }
		 IDbSet<TMBudget> TMBudgets { get; }
		 IDbSet<TMEmployeeTime> TMEmployeeTimes { get; }
		 IDbSet<TMLocation> TMLocations { get; }
		 IDbSet<TMOfficeHour> TMOfficeHours { get; }
		 IDbSet<TMProject> TMProjects { get; }
		 IDbSet<TMProjectCategory> TMProjectCategories { get; }
		 IDbSet<TMRelease> TMReleases { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}