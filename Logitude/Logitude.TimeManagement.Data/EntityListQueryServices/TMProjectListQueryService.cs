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

using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityLists;
using Logitude.TimeManagement.Data.CustomFilters;

namespace Logitude.TimeManagement.Data.EntityListQueryServices
{

    public partial class TMProjectListQueryService
    {
        private IQueryable<TMProjectList> GetIqueryableList(IQueryable<TMProject> iQueryable)
        {
            IQueryable<TMProjectList> query = (from a in iQueryable
                                               select new TMProjectList()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   Name = a.Name,
                                                   Description = a.Description,
                                                   CustomerId = a.CustomerId,
                                                   CustomerName = a.Customer != null ? a.Customer.EnglishName : null,
                                                   OwnerName = a.Owner == null ? "" : a.Owner.Contact.EnglishName,
                                                   CreatedByUserId = a.CreatedByUserId,
                                                   CreateDate = a.CreateDate,
                                                   UpdateDate = a.UpdateDate,
                                                   UpdatedByUserId = a.UpdatedByUserId,
                                                   Inactive = a.Inactive,
                                                   OwnerId = a.OwnerId,
                                                   ProjectNumber = a.ProjectNumber,
                                                   SearchFields = a.SearchFields,
                                                   IsInnerProject=a.IsInnerProject,
                                                   ExternalProjectNumber = a.ExternalProjectNumber,
                                                   BudgetId = a.BudgetId,
                                                   CategoryId = a.CategoryId,
                                                   IsProrated = a.IsProrated, 
                                                   CategoryName=a.TMProjectCategory!=null?a.TMProjectCategory.Name:null,
                                                   DayOffTypeCode = a.DayOffTypeCode,
                                               });
            return query;
        }

        private IQueryable<TMProject> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TMProject> iQueryable, int tenant)
        {
            return TMProjectCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);

        }
        private IQueryable<TMProject> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TMProject> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }

}
	