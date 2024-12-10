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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class InterestReportsConnectInvoiceListQueryService
    {
	    private IQueryable<InterestReportsConnectInvoiceList> GetIqueryableList(IQueryable<InterestReportsConnectInvoice> iQueryable)
        {
		IQueryable<InterestReportsConnectInvoiceList> query = (from a in iQueryable
                                            select new InterestReportsConnectInvoiceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          ReportId = a.ReportId,
					
					                          InvoiceId = a.InvoiceId,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestReportsConnectInvoice> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestReportsConnectInvoice> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<InterestReportsConnectInvoice> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestReportsConnectInvoice> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	