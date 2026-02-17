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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class JournalTypeListQueryService
    {
	    private IQueryable<JournalTypeList> GetIqueryableList(IQueryable<JournalType> iQueryable)
        {
            IQueryable<JournalTypeList> query = (from a in iQueryable
                                                       select new JournalTypeList()
                                                       {
                                                           JournalTypeID = a.JournalTypeID,
                                                           LocalName = a.LocalName,
                                                           EnglishName = a.EnglishName,
                                                           SearchFields = a.SearchFields,
                                                           Inactive = a.Inactive,
                                                       });
            return query.Where(d => d.Inactive != true);
        }

		private IQueryable<JournalType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<JournalType> iQueryable)//,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<JournalType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<JournalType> iQueryable)//,int tenant)
        {
			return iQueryable;
		}
	}


}
	