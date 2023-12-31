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

    public partial class CertificateOfOriginInvoiceListQueryService
    {
	    private IQueryable<CertificateOfOriginInvoiceList> GetIqueryableList(IQueryable<CertificateOfOriginInvoice> iQueryable)
        {
		IQueryable<CertificateOfOriginInvoiceList> query = (from a in iQueryable
                                            select new CertificateOfOriginInvoiceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          CertificateOfOriginId = a.CertificateOfOriginId,
					
					                          InvoicesIdUry = a.InvoicesIdUry,
					
					                          InvoiceNumber = a.InvoiceNumber,
					
					                          InvoiceDate = a.InvoiceDate,
					
					                          InvoiceSum = a.InvoiceSum,

                                              CurrencyTypeCode = a.CurrencyTypeCode,
					
					                          DescriptionOfInvoice = a.DescriptionOfInvoice,
					
					                          IsInvoicesForPrint = a.IsInvoicesForPrint,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificateOfOriginInvoice> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOriginInvoice> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
	