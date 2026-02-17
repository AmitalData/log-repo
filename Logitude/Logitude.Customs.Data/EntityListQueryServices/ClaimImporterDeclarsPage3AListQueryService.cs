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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ClaimImporterDeclarsPage3AListQueryService
    {
	    private IQueryable<ClaimImporterDeclarsPage3AList> GetIqueryableList(IQueryable<ClaimImporterDeclarsPage3A> iQueryable)
        {
		    IQueryable<ClaimImporterDeclarsPage3AList> query = (from a in iQueryable
                                                select new ClaimImporterDeclarsPage3AList()
											    {
                                                    ClaimId = a.ClaimId,
                                                    Tenant = a.Tenant,
                                                    LineNo = a.LineNo,
		                    	                });
            return query;
		}

		private IQueryable<ClaimImporterDeclarsPage3A> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimImporterDeclarsPage3A> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	