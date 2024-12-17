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

    public partial class DeclarationCounterListQueryService
    {
	    private IQueryable<DeclarationCounterList> GetIqueryableList(IQueryable<DeclarationCounter> iQueryable)
        {
		IQueryable<DeclarationCounterList> query = (from a in iQueryable
                                            select new DeclarationCounterList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          DeclarationId = a.DeclarationId,
					
					                          CustomFileNo = a.CustomFileNo,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationCounter> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationCounter> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	