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

    public partial class ActivityNoteListQueryService
    {
	    private IQueryable<ActivityNoteList> GetIqueryableList(IQueryable<ActivityNote> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<ActivityNote> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ActivityNote> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        private IQueryable<ActivityNote> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ActivityNote> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	