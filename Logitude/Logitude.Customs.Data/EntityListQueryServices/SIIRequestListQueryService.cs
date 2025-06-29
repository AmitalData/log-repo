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

    public partial class SIIRequestListQueryService
    {
	    private IQueryable<SIIRequestList> GetIqueryableList(IQueryable<SIIRequest> iQueryable)
        {
		IQueryable<SIIRequestList> query = (from a in iQueryable
                                            select new SIIRequestList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          Status = a.Status,
					
					                          WareHouseAddress = a.WareHouseAddress,
					
					                          WareHouseCity = a.WareHouseCity,
					
					                          IsClosed = a.IsClosed,
											  Remarks = a.Remarks,
											  DeclarationId = a.DeclarationId,
                                                RequestNo = a.RequestNo,
												RequestDate = a.RequestDate,
                                            });
            return query;
		}

		private IQueryable<SIIRequest> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SIIRequest> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	