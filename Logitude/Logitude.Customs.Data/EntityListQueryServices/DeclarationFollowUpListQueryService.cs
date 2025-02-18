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

    public partial class DeclarationFollowUpListQueryService
    {
	    private IQueryable<DeclarationFollowUpList> GetIqueryableList(IQueryable<DeclarationFollowUp> iQueryable)
        {
		IQueryable<DeclarationFollowUpList> query = (from a in iQueryable
                                            select new DeclarationFollowUpList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          CreateBy = a.CreateBy,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationFollowUp> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationFollowUp> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	