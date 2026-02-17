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

    public partial class GLAccountWithholdingTaxListQueryService
    {
	    private IQueryable<GLAccountWithholdingTaxList> GetIqueryableList(IQueryable<GLAccountWithholdingTax> iQueryable)
        {
		IQueryable<GLAccountWithholdingTaxList> query = (from a in iQueryable
                                            select new GLAccountWithholdingTaxList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          GLAccountId = a.GLAccountId,
					
					                          FromDate = a.FromDate,
					
					                          ToDate = a.ToDate,
					
					                          Percentage = a.Percentage,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<GLAccountWithholdingTax> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountWithholdingTax> iQueryable, int tenant)
        {
            return iQueryable;
		}
				private IQueryable<GLAccountWithholdingTax> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountWithholdingTax> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	