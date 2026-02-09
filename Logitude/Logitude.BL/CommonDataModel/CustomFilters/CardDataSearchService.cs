using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
    public class CardDataSearchService
    {

        public IQueryable<EntityLists.CardList> Run(CardSearchArgs cardSearchArgs)
        {
            IQueryable<CardList> entityLists = cardSearchArgs.EntityLists;
            if (!string.IsNullOrEmpty(cardSearchArgs.SearchText))
            {
                entityLists = GetCardSearchResults(cardSearchArgs, entityLists);
                entityLists = SortCardLists(cardSearchArgs, entityLists);

                return entityLists;
            }
            else return GetDefultQuery(cardSearchArgs);
        }

        private  IQueryable<CardList> GetCardSearchResults(CardSearchArgs cardSearchArgs, IQueryable<EntityLists.CardList> entityLists)
        {
            IQueryable<CardSearch> cardSearches = GetCardSearches(cardSearchArgs);
            var allowedCardIds = entityLists.Select(e => e.Id).ToList();

            CardSearchAdvanceArgs cardSearchAdvanceArgs = GetCardSearchAdvanceArgs(cardSearchArgs);
            List<CardSearchResult> cardSearchResultLists = GetCardSearchDataResults(cardSearchAdvanceArgs, cardSearches, allowedCardIds);
            if (!cardSearchResultLists.Any())
                return Enumerable.Empty<CardList>().AsQueryable();

            var cardIds = cardSearchResultLists.Select(c => c.CardId);
            var filteredEntityLists = entityLists.Where(d => cardIds.Contains(d.Id)).ToList();
            foreach (var list in filteredEntityLists)
            {
                list.SearchWeight = cardSearchResultLists.First(c => c.CardId == list.Id).Weight;
            }

            return filteredEntityLists.AsQueryable();
        }

        private  IQueryable<CardSearch> GetCardSearches(CardSearchArgs cardSearchArgs)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(cardSearchArgs.Tenant);
            IQueryable<CardSearch> cardSearches = (from a in commonDataContext.CardSearches where a.Tenant == cardSearchArgs.Tenant select a);
            if (cardSearchArgs.FilterItems != null && cardSearchArgs.FilterItems.Count()>0)
            {
                cardSearches = GetByPartnerTypeFilters(cardSearchArgs.FilterItems, cardSearches);
                cardSearches = GetByInActiveFilters(cardSearchArgs.FilterItems, cardSearches);
            }
            return cardSearches;
        }

        public  List<CardSearchResult> GetCardSearchDataResults(CardSearchAdvanceArgs cardSearchAdvanceArgs, IQueryable<CardSearch> cardSearches, List<string> allowedCardIds)
        {
            List<CardSearchResult> cardSearchResultLists = new List<CardSearchResult>();
            int take = (int)(cardSearchAdvanceArgs.Take * 1.5);
            bool isFirstTime = true;
            int selectedDataCount = 0;
            while ((selectedDataCount == take && cardSearchResultLists.Count() < cardSearchAdvanceArgs.Take) || isFirstTime)
            {
                var cardSearchResultSelectedLists = (from a in cardSearches
                                               where a.Keyword.StartsWith(cardSearchAdvanceArgs.SeachText) && (allowedCardIds.Contains(a.CardId)  || allowedCardIds.Count() == 0)
                                               select new CardSearchResult()
                                               {
                                                   CardId = a.CardId,
                                                   Weight = a.Weight,
                                               }).OrderByDescending(d => d.Weight).Skip(cardSearchAdvanceArgs.Skip).Take(take).ToList();
                selectedDataCount = cardSearchResultSelectedLists.Count();
                isFirstTime = false;
                cardSearchAdvanceArgs.Skip += selectedDataCount;
                cardSearchResultSelectedLists = cardSearchResultSelectedLists.GroupBy(d => d.CardId).Select(d => d.FirstOrDefault()).OrderByDescending(d => d.Weight).Take((cardSearchAdvanceArgs.Take - cardSearchResultLists.Count)).ToList();
                cardSearchResultLists = cardSearchResultLists.Concat(cardSearchResultSelectedLists).ToList();
            }
         
            return cardSearchResultLists;
        }

        private IQueryable<CardList> SortCardLists(CardSearchArgs cardSearchArgs, IQueryable<CardList> entityLists)
        {
            string sortByField = !string.IsNullOrEmpty(cardSearchArgs.SortByColumnName) ? cardSearchArgs.SortByColumnName : GetDefaultSearchColumnName(cardSearchArgs);
            Expression<Func<CardList, string>> sortExpression = GetSortByExpression(GetDefaultSearchColumnName(cardSearchArgs));
            var  sortedList = entityLists.OrderByDescending(c => c.SearchWeight).ThenBy(sortExpression);
           
            if (!string.IsNullOrEmpty(cardSearchArgs.SortByColumnName) && !string.IsNullOrEmpty(cardSearchArgs.SortDirectin))
            {
                var propertyInfo = typeof(CardList).GetProperty(cardSearchArgs.SortByColumnName);
                if (cardSearchArgs.SortDirectin.ToLower() == "ascending")
                {
                    sortedList = sortedList.OrderBy(c => propertyInfo.GetValue(c, null));
                }
                else
                {
                    sortedList = sortedList.OrderByDescending(c => propertyInfo.GetValue(c, null));
                }
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

        private  IQueryable<CardList> GetDefultQuery(CardSearchArgs cardSearchArgs)
        {
            var results = cardSearchArgs.EntityLists.Take(cardSearchArgs.PageSize);
            if (cardSearchArgs.IsSortList) results = SortCardLists(cardSearchArgs, results);

            return results;
        }

        private string GetDefaultSearchColumnName(CardSearchArgs cardSearchArgs)
        {
          
            string SortByColumnName = "LocalName";
            bool isTextAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(cardSearchArgs.SearchText.ToString(), @"^[a-zA-Z0-9]+$");
            if (isTextAlphaNumeric && LogitudeSettings.WorkEnvironment != "customs") SortByColumnName = "EnglishName";

            return SortByColumnName;
        }

        private IQueryable<CardSearch> GetByPartnerTypeFilters(List<QueryFilterItem> filterItems, IQueryable<CardSearch> cardSearches)
        {
            var result = cardSearches;
            string partnerTypefilterValue = GetFilterValue("PartnerTypeId", filterItems);
            if (!string.IsNullOrEmpty(partnerTypefilterValue))
            {
                List<string> partnerTypeLists = partnerTypefilterValue.Split(',').ToList();
                result = result.Where(d => partnerTypeLists.Contains(d.PartnerTypeId));
            }

            return result;
        }

        private IQueryable<CardSearch> GetByInActiveFilters(List<QueryFilterItem> filterItems,  IQueryable<CardSearch> cardSearches)
        {
            var result = cardSearches;
            string inActivefilterValue = GetFilterValue("InActive", filterItems);
            if (!string.IsNullOrEmpty(inActivefilterValue))
            {
                bool inactive = bool.Parse(inActivefilterValue);
                result = result.Where(d => d.InActive == inactive);
            }

            return result;
        }

        private string GetFilterValue(string fieldName, List<QueryFilterItem> filterItems)
        {
            string fieldValue = string.Empty;
            var filterItem = filterItems.Where(d => d.FieldName == fieldName).FirstOrDefault();
            if (filterItem != null && filterItem.FieldValue != null && !string.IsNullOrEmpty(filterItem.FieldValue.ToString()))
            {
                fieldValue = filterItem.FieldValue.ToString();
            }
            return fieldValue;
        }

        private CardSearchAdvanceArgs GetCardSearchAdvanceArgs(CardSearchArgs cardSearchArgs)
        {
            return new CardSearchAdvanceArgs()
            {
                SeachText = cardSearchArgs.SearchText,
                Take = cardSearchArgs.PageSize,
                Skip = 0,
                Tenant = cardSearchArgs.Tenant,
            };
        }


    }


    public class CardSearchArgs
    {
        public string SearchText { get; set; }
        public int Tenant { get; set; }
        public int PageSize { get; set; }
        public IQueryable<CardList> EntityLists { get; set; }
        public string SortByColumnName { get; set; }
        public string SortDirectin { get; set; }
        public List<QueryFilterItem> FilterItems { get; set; }
        public bool IsSortList { get; set; }

    }

    public class CardSearchResult
    {
        public string CardId { get; set; }
        public int Weight { get; set; }
    }

    public class CardSearchAdvanceArgs
    {
        public string SeachText { get; set; }
        public int Tenant { get; set; }
        public int Take { get; set; }
        public  int Skip { get; set; }

       

    }
}
