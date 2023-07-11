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

    public partial class ClientIndicationListQueryService
    {
	    private IQueryable<ClientIndicationList> GetIqueryableList(IQueryable<ClientIndication> iQueryable)
        {
		  IQueryable<ClientIndicationList> query = (from a in iQueryable
                                            select new ClientIndicationList()
											{
                     
					                          IndicationId = a.IndicationId,
					
					                          Tenant = a.Tenant,
					
					                          ClientId = a.ClientId,
					
					                          CustomerIndicationTypeID = a.CustomerIndicationTypeID,
					
					                          IsActive = a.IsActive,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          CreateDate = a.CreateDate,

										      CustomerIndicationTypeName = a.CustomerIndicationType.LocalName,

											});
            return query;
		}

		private IQueryable<ClientIndication> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientIndication> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	