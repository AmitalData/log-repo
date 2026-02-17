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

    public partial class ClaimImporterDeclarsPage3ListQueryService
    {
	    private IQueryable<ClaimImporterDeclarsPage3List> GetIqueryableList(IQueryable<ClaimImporterDeclarsPage3> iQueryable)
        {
		    IQueryable<ClaimImporterDeclarsPage3List> query = (from a in iQueryable
                                                select new ClaimImporterDeclarsPage3List()
											    {
                                                    ClaimId = a.ClaimId,
                                                    Tenant = a.Tenant,
                                                    LineNo = a.LineNo,
		                    	                });
            return query;
		}

        private IQueryable<ClaimImporterDeclarsPage3> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ClaimImporterDeclarsPage3> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	