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

    public partial class DocumentTypeCustomsDataListQueryService
    {
	    private IQueryable<DocumentTypeCustomsDataList> GetIqueryableList(IQueryable<DocumentTypeCustomsData> iQueryable)
        {
		IQueryable<DocumentTypeCustomsDataList> query = (from a in iQueryable
                                            select new DocumentTypeCustomsDataList()
											{
                     
					                          DocumentTypeId = a.DocumentTypeId,
					
					                          Tenant = a.Tenant,
					
					                          CustomsDoucumentTypeCode = a.CustomsDoucumentTypeCode,
					
		                    	            });
            return query;
		}

		private IQueryable<DocumentTypeCustomsData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DocumentTypeCustomsData> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	