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

    public partial class InvoiceApiStepListQueryService
    {
	    private IQueryable<InvoiceApiStepList> GetIqueryableList(IQueryable<InvoiceApiStep> iQueryable)
        {
		IQueryable<InvoiceApiStepList> query = (from a in iQueryable
                                            select new InvoiceApiStepList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
					                          IsAllowResend = a.IsAllowResend,
					
		                    	            });
            return query;
		}

		private IQueryable<InvoiceApiStep> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InvoiceApiStep> iQueryable)
        {
            return iQueryable;
        }
        private IQueryable<InvoiceApiStep> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InvoiceApiStep> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	