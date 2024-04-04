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

    public partial class RemarksClassificationListQueryService
    {
	    private IQueryable<RemarksClassificationList> GetIqueryableList(IQueryable<RemarksClassification> iQueryable)
        {
		IQueryable<RemarksClassificationList> query = (from a in iQueryable
                                            select new RemarksClassificationList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CB_ID = a.CB_ID,
					
					                          CustomsItemsID = a.CustomsItemsID,
					
					                          RemarkDescription = a.RemarkDescription,
					
		                    	            });
            return query;
		}

		private IQueryable<RemarksClassification> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<RemarksClassification> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	