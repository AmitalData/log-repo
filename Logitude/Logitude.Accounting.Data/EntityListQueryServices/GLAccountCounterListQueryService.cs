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

    public partial class GLAccountCounterListQueryService
    {
	    private IQueryable<GLAccountCounterList> GetIqueryableList(IQueryable<GLAccountCounter> iQueryable)
        {
		IQueryable<GLAccountCounterList> query = (from a in iQueryable
                                            select new GLAccountCounterList()
											{
                     
					                          Prefix = a.Prefix,
					
					                          StartNumber = a.StartNumber,
					
					                          CurrentNumber = a.CurrentNumber,
					
		                    	            });
            return query;
		}

		private IQueryable<GLAccountCounter> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountCounter> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<GLAccountCounter> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountCounter> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	