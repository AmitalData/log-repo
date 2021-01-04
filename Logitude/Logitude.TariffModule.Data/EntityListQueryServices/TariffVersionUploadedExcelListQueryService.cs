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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{ 

    public partial class TariffVersionUploadedExcelListQueryService
    {
	    private IQueryable<TariffVersionUploadedExcelList> GetIqueryableList(IQueryable<TariffVersionUploadedExcel> iQueryable)
        {
		IQueryable<TariffVersionUploadedExcelList> query = (from a in iQueryable
                                            select new TariffVersionUploadedExcelList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
		                    	            });
            return query;
		}

		private IQueryable<TariffVersionUploadedExcel> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TariffVersionUploadedExcel> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TariffVersionUploadedExcel> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TariffVersionUploadedExcel> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	