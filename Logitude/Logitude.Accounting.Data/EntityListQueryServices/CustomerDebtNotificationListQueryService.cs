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

    public partial class CustomerDebtNotificationListQueryService
    {
	    private IQueryable<CustomerDebtNotificationList> GetIqueryableList(IQueryable<CustomerDebtNotification> iQueryable)
        {
		IQueryable<CustomerDebtNotificationList> query = (from a in iQueryable
                                            select new CustomerDebtNotificationList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          InActive = a.InActive,
					
					                          TypesDebts = a.TypesDebts,
					
					                          DebtLevel = a.DebtLevel,
					
					                          DebtLevelAmount = a.DebtLevelAmount,
					
					                          TasksSchedulerId = a.TasksSchedulerId,
					
					                          PaymentNotes = a.PaymentNotes,
					
		                    	            });
            return query;
		}

		private IQueryable<CustomerDebtNotification> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomerDebtNotification> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CustomerDebtNotification> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CustomerDebtNotification> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	