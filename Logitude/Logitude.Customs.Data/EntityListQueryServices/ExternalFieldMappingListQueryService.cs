using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class ExternalFieldMappingListQueryService
    {
        private IQueryable<ExternalFieldMappingList> GetIqueryableList(IQueryable<ExternalFieldMapping> iQueryable)
        {
            IQueryable<ExternalFieldMappingList> query = (from a in iQueryable
                                                          select new ExternalFieldMappingList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              SearchFields = a.SearchFields,
                                                              StatusFieldType = a.StatusFieldType,
                                                              StatusCode = a.StatusCode,
                                                              Field = a.Field,
                                                              InActive= a.InActive,
                                                              StatusName= a.StatusName,
                                                          });
            return query;
        }

        private IQueryable<ExternalFieldMapping> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExternalFieldMapping> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
