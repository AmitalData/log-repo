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

    public partial class CouriersVatListQueryService
    {
	    private IQueryable<CouriersVatList> GetIqueryableList(IQueryable<CouriersVat> iQueryable)
        {
		IQueryable<CouriersVatList> query = (from a in iQueryable
                                            select new CouriersVatList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          VatNumber = a.VatNumber,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
					                          InActive = a.InActive,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<CouriersVat> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CouriersVat> iQueryable, int tenant)
        {
            return iQueryable;
        
		}
			}


}
	