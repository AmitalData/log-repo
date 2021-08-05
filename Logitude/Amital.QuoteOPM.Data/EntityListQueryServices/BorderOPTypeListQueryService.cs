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

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class BorderOPTypeListQueryService
    {
	    private IQueryable<BorderOPTypeList> GetIqueryableList(IQueryable<BorderOPType> iQueryable)
        {
		IQueryable<BorderOPTypeList> query = (from a in iQueryable
                                            select new BorderOPTypeList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<BorderOPType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BorderOPType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<BorderOPType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BorderOPType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	