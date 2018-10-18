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

    public partial class TaxReportListQueryService
    {
	    private IQueryable<TaxReportList> GetIqueryableList(IQueryable<TaxReport> iQueryable)
        {
            IQueryable<TaxReportList> query = (from a in iQueryable.Include("VatReportStatus")
                                               select new TaxReportList()
                                               {

                                                   Id = a.Id,

                                                   Tenant = a.Tenant,

                                                   CreateDate = a.CreateDate,

                                                   CreatedByUserId = a.CreatedByUserId,

                                                   LastUpdateDate = a.LastUpdateDate,

                                                   UpdatedByUserId = a.UpdatedByUserId,

                                                   SearchFields = a.SearchFields,

                                                   TaxReportMonth = a.TaxReportMonth,

                                                   TaxReportNumber = a.TaxReportNumber,

                                                   VatNumber = a.VatNumber,

                                                   TaxReportTypeCode = a.TaxReportTypeCode,

                                                   IsCancelled = a.IsCancelled,

                                                   TaxableOutputAmount = a.TaxableOutputAmount,

                                                   OutputTaxAmount = a.OutputTaxAmount,

                                                   TaxableOutputsWithDiffPercent = a.TaxableOutputsWithDiffPercent,

                                                   OutputTaxAmountWithDiffPercent = a.OutputTaxAmountWithDiffPercent,

                                                   ExemptTaxableOutput = a.ExemptTaxableOutput,

                                                   OutputLinesCount = a.OutputLinesCount,

                                                   OtherInputsTaxAmount = a.OtherInputsTaxAmount,

                                                   EquipmentInputsTaxAmount = a.EquipmentInputsTaxAmount,

                                                   InputLinesCount = a.InputLinesCount,

                                                   AmountForPayRefund = a.AmountForPayRefund,

                                                   StatusCode = a.StatusCode,
                                                   StatusLocalName = a.VatReportStatus != null ? a.VatReportStatus.LocalName : null,
                                                   StatusEnglishName = a.VatReportStatus != null ? a.VatReportStatus.EnglishName : null,

                                                   ProcessStartDate = a.ProcessStartDate,

                                                   ProcessEndDate = a.ProcessEndDate,

                                                   ProcessProgress = a.ProcessProgress,
                                                   CreatedByUserName = a.CreatedByUser != null? a.CreatedByUser.Contact.LocalName : null,
					
		                    	            });
            return query;
		}

		private IQueryable<TaxReport> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TaxReport> iQueryable, int tenant)
        {
            return iQueryable;

        }
				private IQueryable<TaxReport> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TaxReport> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	