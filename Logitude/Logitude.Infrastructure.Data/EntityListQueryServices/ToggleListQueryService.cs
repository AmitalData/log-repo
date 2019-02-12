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

    public partial class ToggleListQueryService
    {
        private IQueryable<ToggleList> GetIqueryableList(IQueryable<Toggle> iQueryable)
        {
            IQueryable<ToggleList> query = (from a in iQueryable
                                            select new ToggleList()
                                            {
                                                Code = a.Code,
                                                Name = a.Name,
                                                SearchFields = a.SearchFields,
                                            });
            return query;
        }

        private IQueryable<Toggle> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Toggle> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<Toggle> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Toggle> iQueryable)
        {
            return iQueryable;
        }
    }
}
	