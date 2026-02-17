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

    public partial class CallTypeListQueryService
    {
	    private IQueryable<CallTypeList> GetIqueryableList(IQueryable<CallType> iQueryable)
        {
            IQueryable<CallTypeList> query = (from a in iQueryable
                                              select new CallTypeList()
                                                         {
                                                             Code = a.Code,
                                                             Name = a.Name,
                                                             SearchFields = a.SearchFields,
                                                         });
            return query;
		}

        private IQueryable<CallType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CallType> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<CallType> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CallType> iQueryable)
        {
            return iQueryable;
        }
	}


}
	