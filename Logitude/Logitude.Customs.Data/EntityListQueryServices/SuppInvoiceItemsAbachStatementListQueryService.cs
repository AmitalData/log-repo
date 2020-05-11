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

    public partial class SuppInvoiceItemsAbachStatementListQueryService
    {
	    private IQueryable<SuppInvoiceItemsAbachStatementList> GetIqueryableList(IQueryable<SuppInvoiceItemsAbachStatement> iQueryable)
        {
		IQueryable<SuppInvoiceItemsAbachStatementList> query = (from a in iQueryable
                                            select new SuppInvoiceItemsAbachStatementList()
											{
                     
					                          DeclarationId = a.DeclarationId,
					
					                          InvoiceCounterKey = a.InvoiceCounterKey,
					
					                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
					
					                          SequenceNumeric = a.SequenceNumeric,
					
					                          Tenant = a.Tenant,
					
					                          StatementType = a.StatementType,
					
					                          StatementInd = a.StatementInd,
					
		                    	            });
            return query;
		}

		private IQueryable<SuppInvoiceItemsAbachStatement> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SuppInvoiceItemsAbachStatement> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	