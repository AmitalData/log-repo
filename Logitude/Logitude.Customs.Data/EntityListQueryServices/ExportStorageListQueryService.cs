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

    public partial class ExportStorageListQueryService
    {
	    private IQueryable<ExportStorageList> GetIqueryableList(IQueryable<ExportStorage> iQueryable)
        {
		IQueryable<ExportStorageList> query = (from a in iQueryable
                                            select new ExportStorageList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          DeclarationId = a.DeclarationId,
					
					                          ExportFileNo = a.ExportFileNo,
					
					                          StorageNo = a.StorageNo,
					
					                          StorageStatus = a.StorageStatus,
					
					                          CargoTypeCode = a.CargoTypeCode,
					
		                    	            });
            return query;
		}

		private IQueryable<ExportStorage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExportStorage> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	