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

    public partial class DecConsAcceptanceListQueryService
    {
	    private IQueryable<DecConsAcceptanceList> GetIqueryableList(IQueryable<DecConsAcceptance> iQueryable)
        {
		IQueryable<DecConsAcceptanceList> query = (from a in iQueryable
                                            select new DecConsAcceptanceList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          DeclarationId = a.DeclarationId,
					
					                          ConsignmentNumber = a.ConsignmentNumber,
					
					                          LineNumber = a.LineNumber,
					
					                          LoadDate = a.LoadDate,
					
					                          PackageTypeCode = a.PackageTypeCode,
					
					                          PackageQuantity = a.PackageQuantity,
					
					                          GrossMassMeasure = a.GrossMassMeasure,
					
		                    	            });
            return query;
		}

		private IQueryable<DecConsAcceptance> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DecConsAcceptance> iQueryable, int tenant)
        {
            return iQueryable;

        }
			}


}
	