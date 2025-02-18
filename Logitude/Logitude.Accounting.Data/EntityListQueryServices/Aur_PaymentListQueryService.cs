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

    public partial class Aur_PaymentListQueryService
    {
	    private IQueryable<Aur_PaymentList> GetIqueryableList(IQueryable<Aur_Payment> iQueryable)
        {
		IQueryable<Aur_PaymentList> query = (from a in iQueryable
                                            select new Aur_PaymentList()
											{
                     
					                          Id = a.Id,
					
					                          CreateDate = a.CreateDate,
					
					                          DraftNumber = a.DraftNumber,
					
					                          ForMonth = a.ForMonth,
					
					                          SaleOrder = a.SaleOrder,
					
					                          Customer = a.Customer,
					
					                          InvoiceType = a.InvoiceType,
					
					                          PaymentRequestStatus = a.PaymentRequestStatus,
					
					                          ErrorMessage = a.ErrorMessage,
					
		                    	            });
            return query;
		}

		private IQueryable<Aur_Payment> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Aur_Payment> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<Aur_Payment> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Aur_Payment> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	