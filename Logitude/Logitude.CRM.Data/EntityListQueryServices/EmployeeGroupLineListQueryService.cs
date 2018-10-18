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
    public partial class EmployeeGroupLineListQueryService
    {
        private IQueryable<EmployeeGroupLineList> GetIqueryableList(IQueryable<EmployeeGroupLine> iQueryable)
        {
            IQueryable<EmployeeGroupLineList> query = (from a in iQueryable
                                                       select new EmployeeGroupLineList()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           EmployeeGroupId = a.EmployeeGroupId,
                                                           UserId = a.UserId,
                                                           IsDefaultOwner = a.IsDefaultOwner,
                                                       });
            return query;
        }

        private IQueryable<EmployeeGroupLine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<EmployeeGroupLine> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<EmployeeGroupLine> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<EmployeeGroupLine> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
	