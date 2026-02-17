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

    public partial class BankCodeListQueryService
    {
	    private IQueryable<BankCodeList> GetIqueryableList(IQueryable<BankCode> iQueryable)
        {
		IQueryable<BankCodeList> query = (from a in iQueryable
                                            select new BankCodeList()
											{
                                                Id = a.Id,

					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,

                                              Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<BankCode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BankCode> iQueryable, int tenant)
        {
            return iQueryable;
		}
				private IQueryable<BankCode> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BankCode> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	