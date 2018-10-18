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

    public partial class JournalActionTypeListQueryService : Logitude.Accounting.Data.EntityListQueryServices.IJournalActionTypeListQueryService
    {
	    private IQueryable<JournalActionTypeList> GetIqueryableList(IQueryable<JournalActionType> iQueryable)
        {
            IQueryable<JournalActionTypeList> query = (from a in iQueryable
                                                       select new JournalActionTypeList()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           Code = a.Code,
                                                           LocalName = a.LocalName,
                                                           EnglishName = a.EnglishName,
                                                           SearchFields = a.SearchFields,
                                                           Inactive = a.Inactive,
                                                       });
            return query;//.Where(d => d.Inactive != true);
		}

		private IQueryable<JournalActionType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<JournalActionType> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<JournalActionType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<JournalActionType> iQueryable,int tenant)
        {
			return iQueryable;
		}


        public JournalActionTypeList GetByCode(string code, int tenant)
        {
            IQueryable<JournalActionType> actionQuery = 
                (from a in context.JournalActionTypes
                where a.Tenant == tenant && a.Code == code
                 select a);

            IQueryable<JournalActionTypeList> actionListQuery = this.GetIqueryableList(actionQuery);
            List<JournalActionTypeList> actionList = actionListQuery.ToList();
            JournalActionTypeList rvList = actionList.FirstOrDefault();
            return rvList;
        }

	}


}
	