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

    public partial class ExportReferenceListQueryService
    {
	    private IQueryable<ExportReferenceList> GetIqueryableList(IQueryable<ExportReference> iQueryable)
        {
		IQueryable<ExportReferenceList> query = (from a in iQueryable
                                            select new ExportReferenceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          StorageNo = a.StorageNo,
					
					                          RefType = a.RefType,
					
					                          RefValue = a.RefValue,
					
		                    	            });
            return query;
		}

		private IQueryable<ExportReference> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExportReference> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	