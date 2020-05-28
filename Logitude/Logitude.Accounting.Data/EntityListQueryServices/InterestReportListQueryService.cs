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

    public partial class InterestReportListQueryService
    {
	    private IQueryable<InterestReportList> GetIqueryableList(IQueryable<InterestReport> iQueryable)
        {


        IQueryable<InterestReportList> query = (from a in iQueryable
                                            select new InterestReportList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDateTime = a.CreateDateTime,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDateTime = a.UpdateDateTime,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          GLAccountId = a.GLAccountId,
					
					                          ReportNumber = a.ReportNumber,
					
					                          InterestCalculationDate = a.InterestCalculationDate,
					
					                          TotalAmount = a.TotalAmount,
					
					                          OpenBalance = a.OpenBalance,
					
					                          CloseBalance = a.CloseBalance,
					
					                          ARinvoiceId = a.ARinvoiceId,
					
					                          InvoiceAmount = a.InvoiceAmount,
					
					                          GLAccountInterestCreditLimit = a.GLAccountInterestCreditLimit,
					
					                          InterestReportStatusCode = a.InterestReportStatusCode,

                                              CreatedByLocalName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,

                                              UpdatedByLocalName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.LocalName : null,

                                              InterestReportStatusName = a.InterestReportStatuse == null ? null : a.InterestReportStatuse.EnglishName,

                                              InterestReportStatusLocalName = a.InterestReportStatuse==null ? null :a.InterestReportStatuse.LocalName,

                                              GLAccountDisplayNumber = a.GLAccount == null ? null : a.GLAccount.DisplayNumber,

                                              ARInvoiceNumber = a.ARInvoice == null ? null : a.ARInvoice.InvoiceNumber,

                                              GLAccountLocalName = a.GLAccount == null ? null : a.GLAccount.LocalName

                                            });
            return query;
		}

		private IQueryable<InterestReport> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestReport> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<InterestReport> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestReport> iQueryable, int tenant)
        {
			return iQueryable;
		}




    }


}
	