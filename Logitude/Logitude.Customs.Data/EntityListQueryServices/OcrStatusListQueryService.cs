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

    public partial class OcrStatusListQueryService
    {
	    private IQueryable<OcrStatusList> GetIqueryableList(IQueryable<OcrStatus> iQueryable)
        {
		IQueryable<OcrStatusList> query = (from a in iQueryable
                                            select new OcrStatusList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
		                    	            });
            return query;
		}

		private IQueryable<OcrStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OcrStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	