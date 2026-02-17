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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{

    public partial class BusinessProcessQueueListQueryService
    {
        private IQueryable<BusinessProcessQueueList> GetIqueryableList(IQueryable<BusinessProcessQueue> iQueryable)
        {
            IQueryable<BusinessProcessQueueList> query = (from a in iQueryable
                                                          select new BusinessProcessQueueList()
                                                          {

                                                              Id = a.Id,

                                                              Tenant = a.Tenant,

                                                              CreateDate = a.CreateDate,

                                                              CreatedByUserId = a.CreatedByUserId,

                                                              UpdateDate = a.UpdateDate,

                                                              UpdatedByUserId = a.UpdatedByUserId,

                                                              SearchFields = a.SearchFields,

                                                              Name = a.Name,

                                                              LocalName = a.LocalName,

                                                              InActive = a.InActive,

                                                              BusinessRoleId = a.BusinessRoleId,

                                                              ObjectTableId = a.ObjectTableId,
                                                              EntityName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                              BusinessRoleName = a.BusinessRole != null ? a.BusinessRole.Name : null,
                                                          });
            return query;
        }

        private IQueryable<BusinessProcessQueue> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BusinessProcessQueue> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<BusinessProcessQueue> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BusinessProcessQueue> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	