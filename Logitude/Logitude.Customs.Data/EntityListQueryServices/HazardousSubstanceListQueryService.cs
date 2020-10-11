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

    public partial class HazardousSubstanceListQueryService
    {
	    private IQueryable<HazardousSubstanceList> GetIqueryableList(IQueryable<HazardousSubstance> iQueryable)
        {
		IQueryable<HazardousSubstanceList> query = (from a in iQueryable
                                            select new HazardousSubstanceList()
											{

                                                Code = a.Code,

                                                // EnglishName = a.EnglishName,
                                                LocalName = a.LocalName,
                                                Inactive = a.Inactive,
                                                SearchFields = a.SearchFields,

                                            });
            return query;
		}

		private IQueryable<HazardousSubstance> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<HazardousSubstance> iQueryable)
        {
            return iQueryable;

 		}
			}


}
	