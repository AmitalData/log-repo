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

    public partial class ReferantExceptionListQueryService
    {
	    private IQueryable<ReferantExceptionList> GetIqueryableList(IQueryable<ReferantException> iQueryable)
        {
		IQueryable<ReferantExceptionList> query = (from a in iQueryable
                                            select new ReferantExceptionList()
											{
                     
					                          DeclarationId = a.DeclarationId,
					
					                          ExceptionReasonsCode = a.ExceptionReasonsCode,
					
					                          ExceptionRemarks = a.ExceptionRemarks,
					
					                          Status = a.Status,
					
		                    	            });
            return query;
		}

		private IQueryable<ReferantException> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReferantException> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	