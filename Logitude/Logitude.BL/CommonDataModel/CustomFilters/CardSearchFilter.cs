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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CardSearchFilter
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

                IQueryable<CardSearch> cardSearches = (from a in commonDataContext.CardSearches where a.Tenant == cardSearchFilterArgs.Tenant && a.InActive == inactive select a);
                if (partnerTypeCodeLists.Count > 0) cardSearches = cardSearches.Where(d => partnerTypeCodeLists.Contains(d.PartnerTypeId));

                var cardIds = (from a in cardSearches
                               where a.Keyword.StartsWith(cardSearchFilterArgs.SeachText)
                               select a).GroupBy(d => d.CardId).Select(d => d.FirstOrDefault()).OrderByDescending(d => d.Weight).Take(cardSearchFilterArgs.QueryOperations.PageSize).Select(d => d.CardId).ToList();

                entityLists = entityLists.Where(d => cardIds.Contains(d.Id));
                if (cardSearchFilterArgs.SortList) entityLists = SortDataLists(cardSearchFilterArgs, entityLists);
                return entityLists;

            }
            else return GetDefultQuery(cardSearchFilterArgs);
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



}
