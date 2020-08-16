using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CardCompactFilter
    {
        public static IQueryable<EntityLists.CardList> GetFilteredQuery(object compactSeachvalue, QueryOperations queryOperations, GenericFilter filter, IQueryable<EntityLists.CardList> entityLists, int tenant)
        {
            if (compactSeachvalue != null)
            {
               // entityLists = GetCardCompactSearchResults(compactSeachvalue, queryOperations, filter, entityLists, tenant);
                entityLists = GetCardSearchResults(compactSeachvalue,  tenant , entityLists , queryOperations);

            }

            return entityLists;
        }

        private static IQueryable<CardList> GetCardSearchResults(object compactSeachvalue, int tenant ,IQueryable<CardList> entityLists , QueryOperations queryOperations)
        {
            string seachvalue = compactSeachvalue != null ? compactSeachvalue.ToString() : null;
            List<CardList> results = new List<CardList>();
            if (!string.IsNullOrEmpty(seachvalue))
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                 List<string> cardIds = (from a in commonDataContext.CardSearchs where a.Tenant == tenant && a.Keyword.StartsWith(seachvalue) select a).GroupBy(d => d.CardId).Select(d => d.FirstOrDefault().CardId).ToList();
                 entityLists = entityLists.Where(d=> cardIds.Contains(d.Id)).Take(queryOperations.PageSize);
                if (string.IsNullOrEmpty(queryOperations.SortByColumnName)) queryOperations.SortByColumnName = "LocalName";
                if (string.IsNullOrEmpty(queryOperations.SortDirectin)) queryOperations.SortDirectin = "Ascending";
                entityLists = QuerySortClass.GetSortedQuery(queryOperations, entityLists, "Card", tenant);


                return entityLists;

                //int skip = 0;
                //List<string> cardIds = new List<string>();
                //ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                //var isfirsttime = true;
                //while ((results.Count() < queryOperations.PageSize && cardIds.Count == queryOperations.PageSize) || isfirsttime)
                //{
                //    isfirsttime = false;
                //    cardIds = (from a in commonDataContext.CardSearchs where a.Tenant == tenant && a.Keyword.StartsWith(seachvalue) select a).GroupBy(d => d.CardId).Select(d => d.FirstOrDefault()).OrderByDescending(d => d.RecordDate).Skip(skip).Take(queryOperations.PageSize).Select(d => d.CardId).ToList();
                //    foreach (CardList cardList in entityLists.Where(d => cardIds.Contains(d.Id)))
                //    {
                //        results.Add(cardList);
                //    }
                //    skip += queryOperations.PageSize;
                //}

                //return results.AsQueryable();


            }
            else
            {
                if (string.IsNullOrEmpty(queryOperations.SortByColumnName)) queryOperations.SortByColumnName = "LocalName";
                if (string.IsNullOrEmpty(queryOperations.SortDirectin)) queryOperations.SortDirectin = "Ascending";
                entityLists = QuerySortClass.GetSortedQuery(queryOperations, entityLists, "Card", tenant);
                entityLists.Take(queryOperations.PageSize);
                return entityLists;

            }
        }

        private static IQueryable<CardList> GetCardCompactSearchResults(object compactSeachvalue, QueryOperations queryOperations, GenericFilter filter, IQueryable<CardList> entityLists, int tenant)
        {
            List<CardList> resultList;
            IQueryable<CardList> nameQueryResult = null;
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            queryOperations.SortDirectin = "Ascending";
            bool isTextAlphaNumeric = System.Text.RegularExpressions.Regex.IsMatch(compactSeachvalue.ToString(), @"^[a-zA-Z0-9]+$");
            if (!isTextAlphaNumeric)
            {
                listQueryOperation.SetFilter("LocalName", compactSeachvalue, false, "StartsWith", null, false);
                nameQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, entityLists).Take(queryOperations.PageSize);

                queryOperations.SortByColumnName = "LocalName";
                nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "Card", tenant);
                resultList = nameQueryResult.ToList();
            }
            else if (LogitudeSettings.WorkEnvironment == "customs")
            {
                listQueryOperation.SetFilter("LocalName", compactSeachvalue, false, "StartsWith", null, false);
                nameQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, entityLists).Take(queryOperations.PageSize);

                queryOperations.SortByColumnName = "LocalName";

                nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "Card", tenant);
                resultList = nameQueryResult.ToList();
                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("LocalName", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("EnglishName", compactSeachvalue, false, "StartsWith", null, false);
                    IQueryable<CardList> engNameQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, entityLists).Take(queryOperations.PageSize);
                    foreach (CardList card in engNameQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == card.Code).Any())
                        {
                            resultList.Add(card);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }
                }

            }
            else
            {
                listQueryOperation.SetFilter("EnglishName", compactSeachvalue, false, "StartsWith", null, false);
                nameQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, entityLists).Take(queryOperations.PageSize);

                queryOperations.SortByColumnName = "EnglishName";
                nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "Card", tenant);
                resultList = nameQueryResult.ToList();
            }





            if (resultList.Count() < queryOperations.PageSize)
            {
                listQueryOperation.SetFilter("LocalName", null, false, "StartsWith", null, false);
                listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                listQueryOperation.SetFilter("Code", compactSeachvalue, false, "StartsWith", null, false);

                IQueryable<CardList> coedQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, entityLists);

                foreach (CardList card in coedQueryResult)
                {
                    if (!resultList.Where(p => p.Code == card.Code).Any())
                    {
                        resultList.Add(card);

                    }
                    if (resultList.Count == queryOperations.PageSize)
                    {
                        break;
                    }
                }

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("LocalName", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("SearchFields", compactSeachvalue, false, "Contains", null, false);

                    IQueryable<CardList> searchFieldQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, entityLists);


                    foreach (CardList card in searchFieldQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == card.Code).Any())
                        {
                            resultList.Add(card);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }
                }
            }

            entityLists = resultList.AsQueryable();
            return entityLists;
        }
    }
}

