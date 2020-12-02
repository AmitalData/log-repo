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

        private  IQueryable<CardList> GetCardSearchResults(CardSearchFilterArgs cardSearchFilterArgs, IQueryable<EntityLists.CardList> entityLists)
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
            CardSearchResultArgs cardSearchResultArgs = GetCardSearchResultArgs(cardSearchFilterArgs);
            List<CardSearchResult> cardSearchResultLists = GetCardSearchDataResults(cardSearchResultArgs, cardSearches);

            var cardIds = cardSearchResultLists.Select(c => c.CardId).ToList();
            var filteredEntityLists = entityLists.Where(d => cardIds.Contains(d.Id)).ToList();
            foreach (var list in filteredEntityLists)
            {
                list.SearchWeight = cardSearchResultLists.First(c => c.CardId == list.Id).Weight;
            }

            return filteredEntityLists.AsQueryable();
        }

        private CardSearchResultArgs GetCardSearchResultArgs(CardSearchFilterArgs cardSearchFilterArgs)
        {
            return new CardSearchResultArgs()
            {
                SeachText = cardSearchFilterArgs.SeachText,
                Take = cardSearchFilterArgs.QueryOperations.PageSize,
                Skip = 0,
                Tenant = cardSearchFilterArgs.Tenant,
            };
        }

        public  List<CardSearchResult> GetCardSearchDataResults(CardSearchResultArgs cardSearchResultArgs, IQueryable<CardSearch> cardSearches)
        {
            List<CardSearchResult> cardSearchResultLists = new List<CardSearchResult>();
            int take = (int)(cardSearchResultArgs.Take * 1.5);
            bool isFirstTime = true;
            int selectedDataCount = 0;
            while ((selectedDataCount == take && cardSearchResultLists.Count() < cardSearchResultArgs.Take) || isFirstTime)
            {
                var cardSearchResultSelectedLists = (from a in cardSearches
                                               where a.Keyword.StartsWith(cardSearchResultArgs.SeachText)
                                               select new CardSearchResult()
                                               {
                                                   CardId = a.CardId,
                                                   Weight = a.Weight,
                                               }).OrderByDescending(d => d.Weight).Skip(cardSearchResultArgs.Skip).Take(take).ToList();
                selectedDataCount = cardSearchResultSelectedLists.Count();
                isFirstTime = false;
                cardSearchResultArgs.Skip += selectedDataCount;
                cardSearchResultSelectedLists = cardSearchResultSelectedLists.GroupBy(d => d.CardId).Select(d => d.FirstOrDefault()).OrderByDescending(d => d.Weight).Take((cardSearchResultArgs.Take - cardSearchResultLists.Count)).ToList();
                cardSearchResultLists = cardSearchResultLists.Concat(cardSearchResultSelectedLists).ToList();
            }
         
            return cardSearchResultLists;
        }

        private IQueryable<CardList> SortDataListByWeight(CardSearchFilterArgs cardSearchFilterArgs, IQueryable<CardList> entityLists)
        {
            var sortedList = entityLists;
            var sortByField = cardSearchFilterArgs.QueryOperations.SortByColumnName;
            if (string.IsNullOrEmpty(sortByField) || cardSearchFilterArgs.SortList)
            {
                sortByField = sortByField ?? GetDefaultSearchColumnName(cardSearchFilterArgs);
                cardSearchFilterArgs.QueryOperations.SortByColumnName = "SearchWeight";
                Expression<Func<CardList, string>> sortExpression = GetSortByExpression(GetDefaultSearchColumnName(cardSearchFilterArgs));
                sortedList = sortedList.OrderByDescending(c => c.SearchWeight).ThenBy(sortExpression);
            }

            return sortedList;
        }

        private  Expression<Func<CardList, string>> GetSortByExpression(string sortByColumnName)
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
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public bool SortList { get; set; }
    }

    public class CardSearchResult
    {
        public string CardId { get; set; }
        public int Weight { get; set; }
    }

    public class CardSearchResultArgs
    {
        public string SeachText { get; set; }
        public int Tenant { get; set; }
        public int Take { get; set; }
        public  int Skip { get; set; }

       

    }
}
