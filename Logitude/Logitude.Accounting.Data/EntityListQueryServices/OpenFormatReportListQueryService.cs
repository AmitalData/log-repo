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

    public partial class OpenFormatReportListQueryService
    {
	    private IQueryable<OpenFormatReportList> GetIqueryableList(IQueryable<OpenFormatReport> iQueryable)
        {
		IQueryable<OpenFormatReportList> query = (from a in iQueryable
                                            select new OpenFormatReportList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                     
					
					                          SearchFields = a.SearchFields,
					
					                          ReportNumber = a.ReportNumber,

                                                FromDate = a.FromDate,

                                                ToDate = a.ToDate,

                                              //  DateTypeCode = a.DateTypeCode,
					
					                          StatusTypeCode = a.StatusTypeCode,
					
					                          ErrorMessage = a.ErrorMessage,
                                              CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                                              Status= a.OpenFormatReportStatus != null? a.OpenFormatReportStatus.EnglishName:null,
                                              UserLocalName = a.CreatedByUser.Contact.LocalName,
                                              StatusLocalName = a.OpenFormatReportStatus != null ? a.OpenFormatReportStatus.LocalName:null,
                                              PDFRerportXML = a.PDFRerportXML,
                                          //    DateTypeName = a.OpenFormatDateType != null? a.OpenFormatDateType.LocalName : null
					
		                    	            });
            return query;
		}

		private IQueryable<OpenFormatReport> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OpenFormatReport> iQueryable, int tenant)
        {
            return iQueryable;

        }
				private IQueryable<OpenFormatReport> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<OpenFormatReport> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	