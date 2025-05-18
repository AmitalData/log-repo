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

    public partial class MagayaStepListQueryService
    {
	    private IQueryable<MagayaStepList> GetIqueryableList(IQueryable<MagayaStep> iQueryable)
        {
		IQueryable<MagayaStepList> query = (from a in iQueryable
                                            select new MagayaStepList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
					                          IsAllowResend = a.IsAllowResend,
					
		                    	            });
            return query;
		}

		private IQueryable<MagayaStep> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<MagayaStep> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<MagayaStep> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<MagayaStep> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	