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

    public partial class ClaimImporterDeclarsPage3BListQueryService
    {
	    private IQueryable<ClaimImporterDeclarsPage3BList> GetIqueryableList(IQueryable<ClaimImporterDeclarsPage3B> iQueryable)
        {
		IQueryable<ClaimImporterDeclarsPage3BList> query = (from a in iQueryable
                                            select new ClaimImporterDeclarsPage3BList()
											{
                                                ClaimId = a.ClaimId,
                                                Tenant = a.Tenant,
                                                LineNo = a.LineNo,
					                            InventoryAmount = a.InventoryAmount,
		                    	            });
            return query;
		}

        private IQueryable<ClaimImporterDeclarsPage3B> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ClaimImporterDeclarsPage3B> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	