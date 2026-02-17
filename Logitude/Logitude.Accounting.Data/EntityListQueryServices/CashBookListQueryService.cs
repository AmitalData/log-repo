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
using Simplog.Data.CommonDataModel;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class CashBookListQueryService
    {
        private IQueryable<CashBookList> GetIqueryableList(IQueryable<CashBook> iQueryable)
        {
            IQueryable<CashBookList> query = (from a in iQueryable
                                              select new CashBookList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  CreateDate = a.CreateDate,
                                                  CreatedByUserId = a.CreatedByUserId,
                                                  UpdateDate = a.UpdateDate,
                                                  UpdatedByUserId = a.UpdatedByUserId,
                                                  SearchFields = a.SearchFields,
                                                  EnglishName = a.EnglishName,
                                                  LocalName = a.LocalName,
                                                  CashBookTypeCode = a.CashBookTypeCode,
                                                  TotalAmount = a.TotalAmount,
                                                  CashBookTypeName = a.CashBookType != null ? a.CashBookType.EnglishName : null,
                                                  AccountName = a.Account != null ? a.Account.EnglishName : null,
                                                  AccountNumber = a.Account != null ? a.Account.DisplayNumber : null,
                                                  CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                  UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                  CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                  CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                  AccountId = a.AccountId,
                                                  CurrencyId = a.CurrencyId,
                                                  Inactive = a.Inactive,
                                                  BranchName = a.Branch.LocalName,
                                              });
            return query;
        }

        private IQueryable<CashBook> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CashBook> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<CashBook> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CashBook> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<CashBookStatusChart> GetChartDataData(int tenant)
        {

            ICashBookStatusChartDataViewContext ctx = CashBookStatusChartDataViewContext.GetContext(tenant);
            DateTime today = DateTime.Now.Date;
            //IAccountingContext contextA = AccountingContext.GetContext(tenant);
            //IQueryable<CashBook> cashbooks = (from a in contextA.CashBooks
            //                                  where a.Tenant == tenant
            //                                  select a);

            //var ccccc = (from a in cashbooks
            //             group a by new { a.CurrencyId,a.CashBookTypeCode } into gr
            //             select new { currency = gr.Key.CurrencyId,type=gr.Key.CashBookTypeCode });

            //var hahahaha = ccccc.ToList();
           
            IQueryable<CashBookStatusChartDataView> data = (from a in ctx.CashBookStatusChartDataViews
                                                            where a.Tenant == tenant
                                                            select a);



            IQueryable<CashBookStatusChart> groupedData = (from x in data
                                                           group x by new
                                                           {
                                                               x.Id,                                                              
                                                               x.CashBookTypeCode,
                                                               x.CurrencyId,
                                                               x.CurrencyCode,
                                                               x.TotalAmount,
                                                           } into g
                                                           orderby g.Key.TotalAmount descending
                                                           select new CashBookStatusChart()
                                                           {
                                                               Id = g.Key.Id,
                                                               CashBookTypeCode = g.Key.CashBookTypeCode,
                                                               CashTotal = g.Where(d => d.ChequeValueDate <= today).Sum(f => f.ChequeAmount.Value),
                                                               PostdatedTotal = g.Where(d => d.ChequeValueDate > today).Sum(f => f.ChequeAmount.Value),
                                                               CurrencyCode = g.Key.CurrencyCode,
                                                               CurrencyId = g.Key.CurrencyId,
                                                               Count=g.Count(),
                                                               TotalAmount = g.Key.TotalAmount.Value,
                                                           });

            return groupedData.ToList();          
                                                
        }

    }
    public class CashBookStatusChart
    {
        private static int counter = 0;
        public CashBookStatusChart()
        {
            linePrimary = ++counter;
        }

        [Key]
        public int linePrimary { get; set; }
        public string Id { get; set; }
        public string CashBookTypeCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        public decimal? PostdatedTotal { get; set; }
        public decimal? CashTotal { get; set; }

        public int Count { get; set; }
    }


}
	