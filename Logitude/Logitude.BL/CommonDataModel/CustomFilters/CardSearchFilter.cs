using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CardSearchFilter
    {
        public IQueryable<EntityLists.CardList> GetFilteredQuery(CardSearchFilterArgs cardSearchFilterArgs)
        {
            IQueryable<EntityLists.CardList> entityLists = cardSearchFilterArgs.EntityLists;
            if (!string.IsNullOrEmpty(cardSearchFilterArgs.SeachText))
            {
                entityLists = GetCardSearchResults(cardSearchFilterArgs, entityLists);
                entityLists = SortDataListByWeight(cardSearchFilterArgs, entityLists);
                //if (cardSearchFilterArgs.SortList) entityLists = SortDataLists(cardSearchFilterArgs, entityLists);

                return entityLists;
            }
            else return GetDefultQuery(cardSearchFilterArgs);
        }

        private static IQueryable<CardList> GetCardSearchResults(CardSearchFilterArgs cardSearchFilterArgs, IQueryable<EntityLists.CardList> entityLists)
        {
            List<string> partnerTypeCodeLists = new List<string>();
            bool inactive = false;
            var partnerTypeFilter = cardSearchFilterArgs.QueryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerTypeId").FirstOrDefault();
            if (partnerTypeFilter != null && partnerTypeFilter.FieldValue != null && !string.IsNullOrEmpty(partnerTypeFilter.FieldValue.ToString())) partnerTypeCodeLists = partnerTypeFilter.FieldValue.ToString().Split(',').ToList();
            var inactiveFilter = cardSearchFilterArgs.QueryOperations.QueryFilterItems.Where(d => d.FieldName == "InActive").FirstOrDefault();
            if (inactiveFilter != null && inactiveFilter.FieldValue != null && !string.IsNullOrEmpty(inactiveFilter.FieldValue.ToString())) inactive = bool.Parse(inactiveFilter.FieldValue.ToString());

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(cardSearchFilterArgs.Tenant);
            IQueryable<CardSearch> cardSearches = (from a in commonDataContext.CardSearches where a.Tenant == cardSearchFilterArgs.Tenant && a.InActive == inactive select a);
            if (partnerTypeCodeLists.Count > 0) cardSearches = cardSearches.Where(d => partnerTypeCodeLists.Contains(d.PartnerTypeId));

            List<CardSearchResult> cardResult = (from a in cardSearches
                                                 where a.Keyword.StartsWith(cardSearchFilterArgs.SeachText)
                                                 select a)
                           .GroupBy(d => d.CardId)
                           .Select(d => d.FirstOrDefault())
                           .OrderByDescending(d => d.Weight)
                           .Take(cardSearchFilterArgs.QueryOperations.PageSize)
                           .Select(d => new CardSearchResult()
                           {
                               CardId = d.CardId,
                               Weight = d.Weight
                           })
                           .ToList();

            var cardIds = cardResult.Select(c => c.CardId).ToList();
            var filteredEntityLists = entityLists.Where(d => cardIds.Contains(d.Id)).ToList();
            foreach (var list in filteredEntityLists)
            {
                list.SearchWeight = cardResult.First(c => c.CardId == list.Id).Weight;
            }

            return filteredEntityLists.AsQueryable();
        }

        private IQueryable<CardList> SortDataListByWeight(CardSearchFilterArgs cardSearchFilterArgs, IQueryable<CardList> entityLists)
        {
            var sortedList = entityLists;
            var sortByField = cardSearchFilterArgs.QueryOperations.SortByColumnName;
            if (string.IsNullOrEmpty(sortByField)
                || cardSearchFilterArgs.SortList)
            {
                sortByField = sortByField ?? GetDefaultSearchColumnName(cardSearchFilterArgs);
                cardSearchFilterArgs.QueryOperations.SortByColumnName = "SearchWeight";
                Expression<Func<CardList, string>> sortExpression = GetSortByExpression(GetDefaultSearchColumnName(cardSearchFilterArgs));
                sortedList = sortedList.OrderByDescending(c => c.SearchWeight).ThenBy(sortExpression);
            }

            return sortedList;
        }

        private static Expression<Func<CardList, string>> GetSortByExpression(string sortByColumnName)
        {
            var param = Expression.Parameter(typeof(CardList), "item");

            var sortExpression = Expression.Lambda<Func<CardList, string>>
                (Expression.Convert(Expression.Property(param, sortByColumnName), typeof(string)), param);
            return sortExpression;
        }

        private  IQueryable<CardList> GetDefultQuery(CardSearchFilterArgs cardSearchFilterArgs)
        {
            var results = cardSearchFilterArgs.EntityLists.Take(cardSearchFilterArgs.QueryOperations.PageSize);
            if (cardSearchFilterArgs.SortList) results = SortDataLists(cardSearchFilterArgs, results);

            return results;
        }

        private IQueryable<EntityLists.CardList> SortDataLists(CardSearchFilterArgs cardSearchFilterArgs, IQueryable<EntityLists.CardList> entityLists)
        {
            var result = entityLists;
            string SortByColumnName = "LocalName";
            bool isTextAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(cardSearchFilterArgs.SeachText.ToString(), @"^[a-zA-Z0-9]+$");
            if (isTextAlphaNumeric && LogitudeSettings.WorkEnvironment != "customs") SortByColumnName = "EnglishName";
            cardSearchFilterArgs.QueryOperations.SortByColumnName = SortByColumnName;
            result = QuerySortClass.GetSortedQuery(cardSearchFilterArgs.QueryOperations, result, "Card", cardSearchFilterArgs.Tenant);
            return result;

        }
        
        private string GetDefaultSearchColumnName(CardSearchFilterArgs cardSearchFilterArgs)
        {
          
            string SortByColumnName = "LocalName";
            bool isTextAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(cardSearchFilterArgs.SeachText.ToString(), @"^[a-zA-Z0-9]+$");
            if (isTextAlphaNumeric && LogitudeSettings.WorkEnvironment != "customs") SortByColumnName = "EnglishName";

            return SortByColumnName;
        }

    }


    public class CardSearchFilterArgs
    {
        public string SeachText { get; set; }
        public int Tenant { get; set; }
        public QueryOperations QueryOperations { get; set; }
        public IQueryable<EntityLists.CardList> EntityLists { get; set; }
        public GenericFilter Filter { get; set; }

        public bool SortList { get; set; }
    }

    public class CardSearchResult
    {
        public string CardId { get; set; }
        public int Weight { get; set; }
    }


}
