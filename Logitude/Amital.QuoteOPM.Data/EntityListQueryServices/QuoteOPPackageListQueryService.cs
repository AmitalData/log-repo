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

    public partial class QuoteopPackageListQueryService
    {
	    private IQueryable<QuoteopPackageList> GetIqueryableList(IQueryable<QuoteopPackage> iQueryable)
        {
		IQueryable<QuoteopPackageList> query = (from a in iQueryable
                                            select new QuoteopPackageList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          QuoteOPId = a.QuoteOPId,
					
					                          PackageTypeId = a.PackageTypeId,
					
					                          Quantity = a.Quantity,
					
					                          GrossWeight = a.GrossWeight,
					
					                          Volume = a.Volume,
					
					                          Height = a.Height,
					
					                          Width = a.Width,
					
					                          Length = a.Length,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteopPackage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteopPackage> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteopPackage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteopPackage> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	