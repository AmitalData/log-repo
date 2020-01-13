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

    public partial class ConsignmentPackDangerListQueryService
    {
	    private IQueryable<ConsignmentPackDangerList> GetIqueryableList(IQueryable<ConsignmentPackDanger> iQueryable)
        {
		IQueryable<ConsignmentPackDangerList> query = (from a in iQueryable
                                            select new ConsignmentPackDangerList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<ConsignmentPackDanger> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ConsignmentPackDanger> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	