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

    public partial class InvoiceApiCommunicationLogListQueryService
    {
	    private IQueryable<InvoiceApiCommunicationLogList> GetIqueryableList(IQueryable<InvoiceApiCommunicationLog> iQueryable)
        {
            HashSet<string> invoiceStatusCodes = new HashSet<string>();
            invoiceStatusCodes.Add("DR");
            invoiceStatusCodes.Add("LL");
            invoiceStatusCodes.Add("PR");

            IQueryable<InvoiceApiCommunicationLogList> query = (from a in iQueryable
                                            select new InvoiceApiCommunicationLogList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          SearchFields = a.SearchFields,
					
					                          DocumentId = a.DocumentId,
					
					                          Step = a.Step,
					
					                          StatusCode = a.StatusCode,
					
					                          Exception = a.Exception,

                                              StatusName = a.InvoiceApiStatus != null ? a.InvoiceApiStatus.StatusName : null,

                                              StepName = a.InvoiceApiStep != null ? a.InvoiceApiStep.EnglishName?? a.InvoiceApiStep.LocalName : null ,
											  ExternalID = a.ExternalID,
											  ARInvoiceId = a.ARInvoiceId,
											  InvoiceNumber = a.ARInvoice != null ?( invoiceStatusCodes.Contains(a.ARInvoice.StatusCode) ? a.ARInvoice.DraftNumber : a.ARInvoice.InvoiceNumber) : null,


                                            });
            return query;
		}

		private IQueryable<InvoiceApiCommunicationLog> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InvoiceApiCommunicationLog> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<InvoiceApiCommunicationLog> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InvoiceApiCommunicationLog> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
	}


}
	