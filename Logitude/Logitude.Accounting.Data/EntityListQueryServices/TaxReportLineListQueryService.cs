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
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class TaxReportLineListQueryService
    {
	    private IQueryable<TaxReportLineList> GetIqueryableList(IQueryable<TaxReportLine> iQueryable)
        {
            IQueryable<TaxReportLineList> query = (from a in iQueryable.Include("TaxReportLineStatus").Include("Journal")
                                                   select new TaxReportLineList()
                                                   {

                                                       Tenant = a.Tenant,

                                                       LastUpdateDateTime = a.LastUpdateDateTime,

                                                       UpdatedByUserId = a.UpdatedByUserId,

                                                       SearchFields = a.SearchFields,

                                                       TaxReportId = a.TaxReportId,

                                                       Line = a.Line,

                                                       OutputOrInput = a.OutputOrInput,

                                                       LineTypeCode = a.LineTypeCode,

                                                       VatNumber = a.VatNumber,

                                                       Reference = a.Reference,

                                                       ReferecneGroup = a.ReferecneGroup,

                                                       ReferenceDate = a.ReferenceDate,

                                                       VatAmount = a.VatAmount,

                                                       VatableInvoiceAmount = a.VatableInvoiceAmount,
                                                       IsExternalLine= a.IsExternalLine,
                                                       StatusCode = a.StatusCode,
                                                       StatusEnglishName = a.TaxReportLineStatus != null ? a.TaxReportLineStatus.EnglishName : null,
                                                       StatusLocalName = a.TaxReportLineStatus != null ? a.TaxReportLineStatus.LocalName : null,
                                                       JournalNumber = a.Journal != null ? a.Journal.JournalNumber : null,
                                                       UpdatedBUserName = a.UpdatedByUser.Contact.LocalName ==null? a.UpdatedByUser.Contact.EnglishName : a.UpdatedByUser.Contact.LocalName,




                                                       TotalInvoiceAmount = a.TotalInvoiceAmount,

                                                       OriginalReference =a.OriginalReference,
                                                       TransmitStatusCode = a.TransmitStatusCode,

                                                       JournalId = a.JournalId,

                                                       IsManuallyChanged = a.IsManuallyChanged,
                                                       

                                                       IsEquipment = a.IsEquipment,

                                                   });
            return query;
		}
        public IQueryable<TaxReportLineList> GetReportLines(string taxreportid, int tenant)
        {
            IQueryable<TaxReportLine> TaxReportLineQuery = (from a in context.TaxReportLines
                                                            where a.TaxReportId == taxreportid && a.Tenant == tenant
                                                            select a);


            IQueryable<TaxReportLineList> TaxReportLineListQuery = GetIqueryableList(TaxReportLineQuery);
            return TaxReportLineListQuery;

        }

       
        

        private IQueryable<TaxReportLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxReportLine> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<TaxReportLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxReportLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	