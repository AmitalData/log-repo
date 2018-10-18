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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{
    public partial class EmployeeGroupListQueryService
    {
        private IQueryable<EmployeeGroupList> GetIqueryableList(IQueryable<EmployeeGroup> iQueryable)
        {
            IQueryable<EmployeeGroupList> query = (from a in iQueryable
                                                   select new EmployeeGroupList()
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
                                                       Inactive = a.Inactive,
                                                       ManagerUserId = a.ManagerUserId,
                                                       EscalationNotify = a.EscalationNotify, 
                                                   });
            return query;
        }

        private IQueryable<EmployeeGroup> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<EmployeeGroup> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<EmployeeGroup> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<EmployeeGroup> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
	