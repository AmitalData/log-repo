	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class OcrDocumentListQueryService
    {
	    private IQueryable<OcrDocumentList> GetIqueryableList(IQueryable<OcrDocument> iQueryable)
        {
		IQueryable<OcrDocumentList> query = (from a in iQueryable
                                            select new OcrDocumentList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          Process = a.Process,
					
					                          JsonData = a.JsonData,
					
					                          Score = a.Score,
					
					                          JsonTif = a.JsonTif,
					
					                          ErrorMsg = a.ErrorMsg,
					
					                          StatusCode = a.StatusCode,
					
		                    	            });
            return query;
		}

		private IQueryable<OcrDocument> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OcrDocument> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	