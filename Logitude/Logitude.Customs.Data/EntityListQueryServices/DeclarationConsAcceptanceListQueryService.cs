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

    public partial class DeclarationConsAcceptanceListQueryService
    {
	    private IQueryable<DeclarationConsAcceptanceList> GetIqueryableList(IQueryable<DeclarationConsAcceptance> iQueryable)
        {
		IQueryable<DeclarationConsAcceptanceList> query = (from a in iQueryable
                                            select new DeclarationConsAcceptanceList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          DeclarationId = a.DeclarationId,
					
					                          ConsignmentNumber = a.ConsignmentNumber,
					
					                          LineNumber = a.LineNumber,
					
					                          EntryDate = a.EntryDate,
					
					                          PackageTypeCode = a.PackageTypeCode,
					
					                          Quantity = a.Quantity,
					
					                          GrossWeight = a.GrossWeight,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationConsAcceptance> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationConsAcceptance> iQueryable, int tenant)
        {
            return iQueryable;

        }
			}


}
	