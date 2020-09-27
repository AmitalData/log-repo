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

    public partial class SupplierInvoicePaymentListQueryService
    {
	    private IQueryable<SupplierInvoicePaymentList> GetIqueryableList(IQueryable<SupplierInvoicePayment> iQueryable)
        {
		IQueryable<SupplierInvoicePaymentList> query = (from a in iQueryable
                                            select new SupplierInvoicePaymentList()
											{
                     
					                          SequenceNumeric = a.SequenceNumeric,
					
					                          PaymentTypeCode = a.PaymentTypeCode,
					
					                          PaymentAmount = a.PaymentAmount,
					
		                    	            });
            return query;
		}

		private IQueryable<SupplierInvoicePayment> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SupplierInvoicePayment> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	