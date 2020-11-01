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

    public partial class AmendmentFieldStatusTypeListQueryService
    {
	    private IQueryable<AmendmentFieldStatusTypeList> GetIqueryableList(IQueryable<AmendmentFieldStatusType> iQueryable)
        {
		IQueryable<AmendmentFieldStatusTypeList> query = (from a in iQueryable
                                            select new AmendmentFieldStatusTypeList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<AmendmentFieldStatusType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AmendmentFieldStatusType> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	