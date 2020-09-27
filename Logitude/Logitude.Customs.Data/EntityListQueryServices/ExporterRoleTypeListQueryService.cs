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

    public partial class ExporterRoleTypeListQueryService
    {
	    private IQueryable<ExporterRoleTypeList> GetIqueryableList(IQueryable<ExporterRoleType> iQueryable)
        {
		IQueryable<ExporterRoleTypeList> query = (from a in iQueryable
                                            select new ExporterRoleTypeList()
											{
                     
					                          SearchFields = a.SearchFields,
					
					                          Inactive = a.Inactive,
                                              Code= a.Code,
                                              EnglishName= a.EnglishName,
                                              LocalName = a.LocalName,
                                              
					
		                    	            });
            return query;
		}

		private IQueryable<ExporterRoleType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExporterRoleType> iQueryable)
        {
            return iQueryable;
        }
			}


}
	