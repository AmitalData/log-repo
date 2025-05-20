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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class MagayaCommunicationLogListQueryService
    {
	    private IQueryable<MagayaCommunicationLogList> GetIqueryableList(IQueryable<MagayaCommunicationLog> iQueryable)
        {
		IQueryable<MagayaCommunicationLogList> query = (from a in iQueryable
                                            select new MagayaCommunicationLogList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          SearchFields = a.SearchFields,
					
					                          CommunicationId = a.CommunicationId,
					
					                          Step = a.Step,
					
					                          StatusCode = a.StatusCode,
					
					                          Exception = a.Exception,
					
		                    	            });
            return query;
		}

		private IQueryable<MagayaCommunicationLog> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<MagayaCommunicationLog> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<MagayaCommunicationLog> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<MagayaCommunicationLog> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	