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

    public partial class SLAHeaderListQueryService
    {
        private IQueryable<SLAHeaderList> GetIqueryableList(IQueryable<SLAHeader> iQueryable)
        {
            IQueryable<SLAHeaderList> query = (from a in iQueryable
                                               select new SLAHeaderList()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   CreateDate = a.CreateDate,
                                                   CreatedByUserId = a.CreatedByUserId,
                                                   UpdateDate = a.UpdateDate,
                                                   UpdatedByUserId = a.UpdatedByUserId,
                                                   Name = a.Name,
                                                   Description = a.Description,
                                                   Inactive = a.Inactive,
                                                   UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                   CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                               });
            return query;
        }

        private IQueryable<SLAHeader> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<SLAHeader> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<SLAHeader> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<SLAHeader> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
