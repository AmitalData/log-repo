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

    public partial class ExportDeclarationClosingDataListQueryService
    {
	    private IQueryable<ExportDeclarationClosingDataList> GetIqueryableList(IQueryable<ExportDeclarationClosingData> iQueryable)
        {
		IQueryable<ExportDeclarationClosingDataList> query = (from a in iQueryable
                                            select new ExportDeclarationClosingDataList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          DeclarationId = a.DeclarationId,
					
					                          FinalCargoTypeCode = a.FinalCargoTypeCode,
					
					                          FinalLoadingSite = a.FinalLoadingSite,
					
		                    	            });
            return query;
		}

		private IQueryable<ExportDeclarationClosingData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExportDeclarationClosingData> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	