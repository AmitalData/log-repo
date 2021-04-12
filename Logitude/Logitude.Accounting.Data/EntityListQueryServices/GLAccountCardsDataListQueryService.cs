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

    public partial class GLAccountCardsDataListQueryService
    {
	    private IQueryable<GLAccountCardsDataList> GetIqueryableList(IQueryable<GLAccountCardsData> iQueryable)
        {
		IQueryable<GLAccountCardsDataList> query = (from a in iQueryable
                                            select new GLAccountCardsDataList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SalesmanUserId = a.SalesmanUserId,
					
					                          CreditLimit = a.CreditLimit,
					
					                          PaymentTermId = a.PaymentTermId,
					
					                          CollectorUserId = a.CollectorUserId,
					
					                          Phone = a.Phone,
					
					                          VatNumber = a.VatNumber,
					
					                          TotalOpenShipments = a.TotalOpenShipments,
					
		                    	            });
            return query;
		}

		private IQueryable<GLAccountCardsData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountCardsData> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<GLAccountCardsData> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountCardsData> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	