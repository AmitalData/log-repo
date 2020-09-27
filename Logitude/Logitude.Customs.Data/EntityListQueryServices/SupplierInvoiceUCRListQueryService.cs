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

    public partial class SupplierInvoiceUCRListQueryService
    {
	    private IQueryable<SupplierInvoiceUCRList> GetIqueryableList(IQueryable<SupplierInvoiceUCR> iQueryable)
        {
		IQueryable<SupplierInvoiceUCRList> query = (from a in iQueryable
                                            select new SupplierInvoiceUCRList()
											{
                     
					                          SequenceNumeric = a.SequenceNumeric,
					
					                          SupplierChargeID = a.SupplierChargeID,
					
					                          AgentChargeID = a.AgentChargeID,
					
		                    	            });
            return query;
		}

		private IQueryable<SupplierInvoiceUCR> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SupplierInvoiceUCR> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	