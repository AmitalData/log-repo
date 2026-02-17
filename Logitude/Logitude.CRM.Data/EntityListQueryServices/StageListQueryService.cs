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

    public partial class StageListQueryService
    {
	    private IQueryable<StageList> GetIqueryableList(IQueryable<Stage> iQueryable)
        {
            IQueryable<StageList> query = (from a in iQueryable
                                           select new StageList()
                                            {
                                                Id = a.Id,
                                                Code = a.Code,
                                                Name = a.Name,
                                                Probability = a.Probability,
                                                SearchFields = a.SearchFields,
                                                IsSelectable = a.IsSelectable,
                                                MaxDays = a.MaxDays,
                                                InActive = a.InActive,
                                                Tenant = a.Tenant,
                                            });
            return query;
		}

        private IQueryable<Stage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Stage> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<Stage> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Stage> iQueryable, int tenant)
        {
            return iQueryable;
        }


	}


}
	