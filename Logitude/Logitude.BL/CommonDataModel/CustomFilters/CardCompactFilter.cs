using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
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
                List<CardList> resultList;
                IQueryable<CardList> nameQueryResult = null;
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
                queryOperations.SortDirectin = "Ascending";
                if (LogitudeSettings.WorkEnvironment == "customs")
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
                }

                return entityLists;
            }
        }
    }

