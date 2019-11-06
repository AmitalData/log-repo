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

    public partial class DecDangersContactListQueryService
    {
	    private IQueryable<DecDangersContactList> GetIqueryableList(IQueryable<DecDangersContact> iQueryable)
        {
		IQueryable<DecDangersContactList> query = (from a in iQueryable
                                            select new DecDangersContactList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          CompanyName = a.CompanyName,
					
					                          CompanyCommNumber = a.CompanyCommNumber,
					
					                          CompanyCommTypeCode = a.CompanyCommTypeCode,
					
					                          ContactName = a.ContactName,
					
					                          ContactCommNumber = a.ContactCommNumber,
					
					                          ContactCommTypeCode = a.ContactCommTypeCode,
					
					                          ContactId = a.ContactId,
					
		                    	            });
            return query;
		}

		private IQueryable<DecDangersContact> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DecDangersContact> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	