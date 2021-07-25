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

    public partial class CustomerClassificationTypeListQueryService
    {
	    private IQueryable<CustomerClassificationTypeList> GetIqueryableList(IQueryable<CustomerClassificationType> iQueryable)
        {
		IQueryable<CustomerClassificationTypeList> query = (from a in iQueryable
                                            select new CustomerClassificationTypeList()
											{
                     
					                          Code = a.Code,
					                          LocalName = a.LocalName,
											  EnglishName =a.EnglishName,
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<CustomerClassificationType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomerClassificationType> iQueryable)
        {
			return iQueryable;
		}
	}


}
	