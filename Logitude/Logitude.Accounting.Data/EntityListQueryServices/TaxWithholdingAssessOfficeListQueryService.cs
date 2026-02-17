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

    public partial class TaxWithholdingAssessOfficeListQueryService
    {
	    private IQueryable<TaxWithholdingAssessOfficeList> GetIqueryableList(IQueryable<TaxWithholdingAssessOffice> iQueryable)
        {
		IQueryable<TaxWithholdingAssessOfficeList> query = (from a in iQueryable
                                            select new TaxWithholdingAssessOfficeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxWithholdingAssessOffice> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxWithholdingAssessOffice> iQueryable, int tenant)
        {
            return iQueryable;

        }

        private IQueryable<TaxWithholdingAssessOffice> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxWithholdingAssessOffice> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	