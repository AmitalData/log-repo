using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
  public  class CardSearchFilter
    {
        public IQueryable<EntityLists.CardList> GetFilteredQuery(CardSearchFilterArgs cardSearchFilterArgs)
        {
            IQueryable<EntityLists.CardList> entityLists = cardSearchFilterArgs.EntityLists;
            if (!string.IsNullOrEmpty(cardSearchFilterArgs.SeachText))
            {
                List<string> partnerTypeCodeLists = new List<string>();
                bool inactive = false;
                var partnerTypeFilter = cardSearchFilterArgs.QueryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerTypeId").FirstOrDefault();
                if (partnerTypeFilter != null && partnerTypeFilter.FieldValue != null && !string.IsNullOrEmpty(partnerTypeFilter.FieldValue.ToString())) partnerTypeCodeLists = partnerTypeFilter.FieldValue.ToString().Split(',').ToList();
                var inactiveFilter = cardSearchFilterArgs.QueryOperations.QueryFilterItems.Where(d => d.FieldName == "InActive").FirstOrDefault();
                if (inactiveFilter != null && inactiveFilter.FieldValue != null && !string.IsNullOrEmpty(inactiveFilter.FieldValue.ToString())) inactive = bool.Parse(inactiveFilter.FieldValue.ToString());

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(cardSearchFilterArgs.Tenant);
                List<string> cardIds = (from a in commonDataContext.CardSearchs
                                        where a.Tenant == cardSearchFilterArgs.Tenant && partnerTypeCodeLists.Contains(a.PartnerTypeId) && a.InActive == inactive && a.Keyword.StartsWith(cardSearchFilterArgs.SeachText)
                                        select a).OrderByDescending(d => d.RecordDate).GroupBy(d => d.CardId).Select(d => d.FirstOrDefault()).Take(cardSearchFilterArgs.QueryOperations.PageSize).Select(d => d.CardId).ToList();
                   return entityLists.Where(d => cardIds.Contains(d.Id));

                //string searchFilterAsWhere = string.Empty;
                //if (cardIds.Count() != 0)
                //{
                //    var values = new StringBuilder();
                //    values.AppendFormat("{0}", "'" + cardIds[0] + "'");
                //    for (int i = 1; i < cardIds.Count; i++)
                //        values.AppendFormat(", {0}", "'" + cardIds[i] + "'");

                //    searchFilterAsWhere = string.Format(
                //        "[Extent1].Id IN ({0})",
                //        values);


                //}


                //TraceStringValues MySql;
                //MySql = IQueryableExtensions.ToTraceString<CardList>(entityLists);
                //List<SqlParameter> parameters = new List<SqlParameter>();

                //if (!string.IsNullOrEmpty(searchFilterAsWhere))
                //{
                //    var regex = new Regex(Regex.Escape("WHERE"), RegexOptions.IgnoreCase);
                //    MySql.TSQL = regex.Replace(MySql.TSQL, "WHERE " + searchFilterAsWhere + " AND ", 1);
                //}

                //foreach (var item in MySql.TSQLParams)
                //{
                //    parameters.Add(new SqlParameter(item.Name, item.Value));
                //}

                //CommonDataContext activeContext = commonDataContext.GetActiveDbContext() as CommonDataContext;
        
                //return activeContext.Database.SqlQuery<CardList>(MySql.TSQL, parameters.ToArray()).ToList().AsQueryable();


            }
            else return GetDefultQuery(cardSearchFilterArgs);
        }



        private static IQueryable<CardList> GetDefultQuery(CardSearchFilterArgs cardSearchFilterArgs)
        {
           var results = cardSearchFilterArgs.EntityLists.Take(cardSearchFilterArgs.QueryOperations.PageSize);
            cardSearchFilterArgs.QueryOperations.SortByColumnName = "LocalName";
            results = QuerySortClass.GetSortedQuery(cardSearchFilterArgs.QueryOperations, results, "Card", cardSearchFilterArgs.Tenant);
            return results;
        }



        public IQueryable<EntityLists.CardList> GetFilteredQuery2(CardSearchFilterArgs cardSearchFilterArgs)
        {
            IQueryable<EntityLists.CardList> entityLists = cardSearchFilterArgs.EntityLists;
            if (!string.IsNullOrEmpty(cardSearchFilterArgs.SeachText))
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(cardSearchFilterArgs.Tenant);
                List<string> cardIds = new List<string>();
                List<CardList> cardLists = new List<CardList>();

                int skip = 0;
                var isfirsttime = true;
                while ((cardLists.Count() < cardSearchFilterArgs.QueryOperations.PageSize && cardIds.Count() == cardSearchFilterArgs.QueryOperations.PageSize) || isfirsttime)
                {
                    isfirsttime = false;
                    cardIds = (from a in commonDataContext.CardSearchs where a.Tenant == cardSearchFilterArgs.Tenant && a.Keyword.StartsWith(cardSearchFilterArgs.SeachText) select a).GroupBy(d => d.CardId).Select(d => d.FirstOrDefault()).OrderByDescending(d => d.RecordDate).Skip(skip).Take(cardSearchFilterArgs.QueryOperations.PageSize).Select(d => d.CardId).ToList();
                    foreach (CardList cardList in entityLists.Where(d => cardIds.Contains(d.Id)))
                    {
                        if (cardLists.Count() < 10)
                        {
                            cardLists.Add(cardList);
                        }
                        else break;
                    }
                    skip += cardSearchFilterArgs.QueryOperations.PageSize;
                }
                return cardLists.AsQueryable();

            }
            else return entityLists;
  
        }




    }


    public class CardSearchFilterArgs
    {
        public string SeachText { get; set; }
        public int Tenant { get; set; }
        public QueryOperations QueryOperations { get; set; }
        public IQueryable<EntityLists.CardList> EntityLists { get; set; }
        public GenericFilter Filter { get; set; }

         
    }



}
