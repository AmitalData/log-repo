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

    public partial class ARPaymentChequeListQueryService
    {
        private IQueryable<ARPaymentChequeList> GetIqueryableList(IQueryable<ARPaymentCheque> iQueryable)
        {
            IQueryable<ARPaymentChequeList> query = (from a in iQueryable
                                                     select new ARPaymentChequeList()
                                                     {

                                                         Id = a.Id,

                                                         Tenant = a.Tenant,

                                                         CurrencyCode = a.Currency.Code,

                                                         SearchFields = a.SearchFields,

                                                         LineNumber = a.LineNumber,

                                                         ChequeNumber = a.ChequeNumber,

                                                         ValueDate = a.ValueDate,

                                                         LocalAmount = a.LocalAmount,

                                                         ForeignAmount = a.ForeignAmount,

                                                         BankId = a.BankId,

                                                         BankBranch = a.BankBranch,

                                                         BankAccount = a.BankAccount,

                                                         PaymentNumber = a.PaymentId != null ? a.Payment.PaymentNo : null,

                                                         StatusCode = a.StatusCode,

                                                        StatusName =  a.ARPaymentChequeStatus != null ? a.ARPaymentChequeStatus.LocalName : null ,

                                                     });
            return query;
        }

        private IQueryable<ARPaymentCheque> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ARPaymentCheque> iQueryable, int tenant)
        {
            ARPaymentChequeListCustomFilter customFilter = new ARPaymentChequeListCustomFilter(tenant);
            QueryOperations customizedQueryOperation = new QueryOperations();
            customizedQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.IsCustom == true).ToList();
            iQueryable = customFilter.GetFilteredQuery(customizedQueryOperation, iQueryable);
            return iQueryable;

        }
        private IQueryable<ARPaymentCheque> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ARPaymentCheque> iQueryable, int tenant)
        {
            return iQueryable;
        }


        public List<ARPaymentChequeList> GetPayablePostDatedARPaymentChequeList(int tenant)
        {
            IQueryable<ARPaymentCheque> aRPaymentChequeQuery = (from a in context.ARPaymentCheques
                                                                where a.Tenant == tenant && ((a.StatusCode == "2") && (a.ValueDate != null && a.ValueDate <= DateTime.Today))
                                                                select a);

            IQueryable<ARPaymentChequeList> aRPaymentChequeListQuery = this.GetIqueryableList(aRPaymentChequeQuery);
            List<ARPaymentChequeList> aRPaymentChequeList = aRPaymentChequeListQuery.ToList();

            return aRPaymentChequeList;
        }

        public class ARPaymentChequeListCustomFilter
        {
            public int Tenant { get; set; }
            public ARPaymentChequeListCustomFilter(int tenant)
            {
                this.Tenant = tenant;
            }

            public IQueryable<ARPaymentCheque> GetFilteredQuery(QueryOperations operations, IQueryable<ARPaymentCheque> queryableData)
            {
                List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

                foreach (QueryFilterItem item in queryFilters)
                {
                    if (item.FieldName == "IsOpen")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode == "1" && d.ValueDate.Date <= DateTime.Today.Date);
                    }

                }
                return queryableData;
            }

        }


        private IQueryable<ARPaymentChequeList> GetAFakeIqueryableList(IQueryable<ARPaymentCheque> iQueryable)
        {
            IQueryable<ARPaymentChequeList> query = (from a in iQueryable//.Include("JournalLine").Include("Currency").Include("Journal")
                                                     select new ARPaymentChequeList()
                                                     {
                                                         Id = a.Id,

                                                         Tenant = a.Tenant,

                                                         CurrencyCode = a.Currency.Code,

                                                         SearchFields = a.SearchFields,

                                                         LineNumber = a.LineNumber,

                                                         ChequeNumber = a.ChequeNumber,

                                                         ValueDate = a.ValueDate,

                                                         LocalAmount = a.LocalAmount,

                                                         ForeignAmount = a.ForeignAmount,

                                                         BankId = a.BankId,

                                                         BankBranch = a.BankBranch,

                                                         BankAccount = a.BankAccount,

                                                         PaymentNumber = a.PaymentId != null ? a.Payment.PaymentNo : null,

                                                     });
            return query;
        }
        public List<ARPaymentChequeList> GetARPaymentChequeListForceOrderByValueDateAndId(
            IQueryable<ARPaymentCheque> ARPaymentChequeQuery, int pageSize, int pageStartAtRecordIndex)
        {
            //var skip = pageSize * curPageZeroBase;
            var skip = pageStartAtRecordIndex;
            var q = ARPaymentChequeQuery.Skip(skip).Take(pageSize);
            IQueryable<ARPaymentChequeList> aRPaymentChequeListQuery = null;
            if (this.context.ToString().StartsWith("Fake"))
            {
                aRPaymentChequeListQuery = GetAFakeIqueryableList(q);
            }
            else
            {
                aRPaymentChequeListQuery = GetIqueryableList(q);
            }

            aRPaymentChequeListQuery = aRPaymentChequeListQuery.OrderBy(rec => rec.ValueDate).ThenBy(rec => rec.Id);


            return aRPaymentChequeListQuery.ToList();
        }


        public class ARPaymentChequeFilter
        {
            public int Tenant { get; set; }
            public string Id { get; set; }
            public string CurrencyId { get; set; }
            public string ChequeNumber { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }


            public int PageSize { get; set; }
            public int PageStartAtRecordIndex { get; set; }

            public string StatusCode { get; set; }



            public string SearchFields { get; set; }

            public ARPaymentChequeFilterCallBack CallBack { get; set; }

        }

    }

    public class ARPaymentChequeResponse : ARPaymentChequeFilterCallBack
    {
        //[XmlIgnore]
        public List<ARPaymentChequeList> MyARPaymentChequeList { get; set; }



    }

  


    public class ARPaymentChequeFilterCallBack : ARPaymentChequeFilterCallBackCanBeNull
    {

        public string SearchFields { get; set; }
        //must not be null!
        public bool OmitAllCheque { get; set; }
        public int? TotalRowCount { get; set; }
        public List<string> AllIdCheques { get; set; }

    }

    public class ARPaymentChequeFilterCallBackCanBeNull
    {

    }
}
	