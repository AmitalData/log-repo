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

    public partial class CourierDeclarationListQueryService
    {
	    private IQueryable<CourierDeclarationList> GetIqueryableList(IQueryable<CourierDeclaration> iQueryable)
        {
		IQueryable<CourierDeclarationList> query = (from a in iQueryable
                                            select new CourierDeclarationList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          //CasualSupplierName = a.CasualSupplierName,
					
					                          //CasualSupplierAddress = a.CasualSupplierAddress,
					
		                    	            });
            return query;
		}

		private IQueryable<CourierDeclaration> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CourierDeclaration> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	