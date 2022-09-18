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

using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityLists;
using Logitude.DashboardModule.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;

namespace Logitude.DashboardModule.Data.EntityListQueryServices
{

	public partial class DashboardListQueryService
	{
		private IQueryable<DashboardList> GetIqueryableList(IQueryable<Dashboard> iQueryable)
		{
			IQueryable<DashboardList> query = (from a in iQueryable
											   select new DashboardList()
											   {
												   Id = a.Id,
												   Tenant = a.Tenant,
												   CreateDate = a.CreateDate,
												   CreatedByUserId = a.CreatedByUserId,
												   UpdateDate = a.UpdateDate,
												   UpdatedByUserId = a.UpdatedByUserId,
												   SearchFields = a.SearchFields,
												   Name = a.Name,
												   Description = a.Description,
											   });
			return query;
		}
        private IQueryable<Dashboard> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Dashboard> iQueryable, int tenant)
		{
			DashboardSharedUserRepository dashboardSharedUserRepository = new DashboardSharedUserRepository(context);
			IQueryable<string> dashboardIds = iQueryable.Select(s => s.Id);
			IQueryable<DashboardSharedUser> users = dashboardSharedUserRepository.GetDashboardSharedUsersByDashboardsIds(dashboardIds, tenant);

			string loggedContactId = this.GetLoggedContactId(tenant);

			return (from d in iQueryable
					where
					(d.PermissionLevelCode == "ONM" && d.CreatedByUserId == loggedContactId)
					|| (d.PermissionLevelCode == "SPF" && users.Select(s => s.UserId).Contains(loggedContactId))
					|| (d.PermissionLevelCode == "ALL")
					select d);
		}
		private string GetLoggedContactId(int tenant)
		{
			string email = HttpContext.Current.User.Identity.Name;
			ContactRepository contactRepository = new ContactRepository(tenant);
			return contactRepository.GetConactIdByemail(email, tenant);
		}

		private IQueryable<Dashboard> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Dashboard> iQueryable, int tenant)
		{
			return iQueryable;
		}
	}
}
	