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

    public partial class GatepassRequestListQueryService
    {
	    private IQueryable<GatepassRequestList> GetIqueryableList(IQueryable<GatepassRequest> iQueryable)
        {
		IQueryable<GatepassRequestList> query = (from a in iQueryable
                                            select new GatepassRequestList()
											{
                     
					                          MasterCourierId = a.MasterCourierId,
					                          Tenant = a.Tenant,
					                          GatepassNumber = a.GatepassNumber,
					                            
					                          OriginSiteCode = a.OriginSiteCode,
					
					                          UpdateCode = a.UpdateCode,
					
					                          DesignateSiteCode = a.DesignateSiteCode,
					
					                          TransportationTypeCode = a.TransportationTypeCode,
					
					                          GatepassRequestStatus = a.GatepassRequestStatus,
					
					                          CustomsUpdateDateTime = a.CustomsUpdateDateTime,
					
		                    	            });
            return query;
		}

		private IQueryable<GatepassRequest> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GatepassRequest> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	