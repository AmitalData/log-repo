using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        AWBDescriptionOfGoodsRepository awbDescriptionOfGoodsRepository;
        AWBDescriptionOfGoodsQuery awbDescriptionOfGoodsQuery;

        public AWBDescriptionOfGoodsList GetSingleAWBDescriptionOfGoodsList(string id, int tenant)
        {
            awbDescriptionOfGoodsRepository = new AWBDescriptionOfGoodsRepository(tenant);
            AWBDescriptionOfGoodsList entityList = null;

            AWBDescriptionOfGoods entityPoco = awbDescriptionOfGoodsRepository.GetSingleAWBDescriptionOfGoods(id);

            if (entityPoco != null)
            {
                List<AWBDescriptionOfGoods> singleEntityList = new List<AWBDescriptionOfGoods>();
                singleEntityList.Add(entityPoco);

                awbDescriptionOfGoodsQuery = new AWBDescriptionOfGoodsQuery(awbDescriptionOfGoodsRepository);
                IQueryable<AWBDescriptionOfGoods> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AWBDescriptionOfGoodsList> iQueryableEntityList = awbDescriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<AWBDescriptionOfGoodsList> GetAWBDescriptionOfGoodsLists(int tenant)
        {
            awbDescriptionOfGoodsRepository = new AWBDescriptionOfGoodsRepository(tenant);
            awbDescriptionOfGoodsQuery = new AWBDescriptionOfGoodsQuery(awbDescriptionOfGoodsRepository);

            IQueryable<AWBDescriptionOfGoods> iQueryable = awbDescriptionOfGoodsRepository.GetAWBDescriptionOfGoods();
            IQueryable<AWBDescriptionOfGoodsList> query2 = awbDescriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AWBDescriptionOfGoodsList> GetAWBDescriptionOfGoodsFilters(byte[] xmlFilters, int tenant)
        {
            awbDescriptionOfGoodsRepository = new AWBDescriptionOfGoodsRepository(tenant);
            awbDescriptionOfGoodsQuery = new AWBDescriptionOfGoodsQuery(awbDescriptionOfGoodsRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AWBDescriptionOfGoods> iQueryable = awbDescriptionOfGoodsRepository.GetAWBDescriptionOfGoods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AWBDescriptionOfGoodsCustomFilter customfilters = new AWBDescriptionOfGoodsCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filter.GetFilteredQuery<AWBDescriptionOfGoods>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AWBDescriptionOfGoodsList> query2 = awbDescriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AWBDescriptionOfGoodsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(PackageList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AWBDescriptionOfGoods", tenant).ToList();

                ObjectField objectField = (from a in entityObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AWBDescriptionOfGoodsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AWBDescriptionOfGoodsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AWBDescriptionOfGoodsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AWBDescriptionOfGoodsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AWBDescriptionOfGoodsList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAWBDescriptionOfGoodsFiltersCount(byte[] xmlFilters, int tenant)
        {
            awbDescriptionOfGoodsRepository = new AWBDescriptionOfGoodsRepository(tenant);
            awbDescriptionOfGoodsQuery = new AWBDescriptionOfGoodsQuery(awbDescriptionOfGoodsRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<AWBDescriptionOfGoods> iQueryable = awbDescriptionOfGoodsRepository.GetAWBDescriptionOfGoods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AWBDescriptionOfGoods>(nonListQueryOperation, iQueryable);

            IQueryable<AWBDescriptionOfGoodsList> query2 = awbDescriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AWBDescriptionOfGoodsList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void LoadAWBDescriptionOfGoodsFromFile(string stringOfLines)
        {
            awbDescriptionOfGoodsRepository = new AWBDescriptionOfGoodsRepository(0);

            int count = 0;
            string[] stringLineArray = stringOfLines.Split('\n');
            string[] readData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    stringLineArray[i] = stringLineArray[i].Replace("\"", "");

                    readData = stringLineArray[i].Split('\t');

                    AWBDescriptionOfGoods newEntity = new AWBDescriptionOfGoods();
                    newEntity.Id = IdCounter.GetNumber("AWBDescriptionOfGoods", 0).ToString();

                    //Name
                    if (readData[0].Trim().Count() >= 60)
                    {
                        newEntity.Name = readData[0].Trim().Substring(0, 60);
                    }
                    else
                    {
                        newEntity.Name = readData[0].Trim();
                    }

                    //ShortDescriptionOfGoods
                    if (readData[1].Trim().Count() >= 100)
                    {
                        newEntity.ShortDescriptionOfGoods = readData[1].Trim().Substring(0, 100);
                    }
                    else
                    {
                        newEntity.ShortDescriptionOfGoods = readData[1].Trim();
                    }

                    //Service
                    if (readData[2].Trim().Count() >= 40)
                    {
                        newEntity.Service = readData[2].Trim().Substring(0, 40);
                    }
                    else
                    {
                        newEntity.Service = readData[2].Trim();
                    }

                    //ProductCode
                    if (readData[3].Trim().Count() >= 4)
                    {
                        newEntity.ProductCode = readData[3].Trim().Substring(0, 4);
                    }
                    else
                    {
                        newEntity.ProductCode = readData[3].Trim();
                    }

                    newEntity.AirlineCode = "LH";
                    newEntity.SearchFields = newEntity.Name + "," + newEntity.ShortDescriptionOfGoods + "," + newEntity.AirlineCode + "," + newEntity.ProductCode;

                    awbDescriptionOfGoodsRepository.Add(newEntity);

                    count++;
                }

                if (count == 100)
                {
                    count = 0;
                    awbDescriptionOfGoodsRepository.SubmitChanges();
                }
            }

            awbDescriptionOfGoodsRepository.SubmitChanges();
        }

        public void LoadCurrenciesFromFile(string stringOfLines)
        {
            int tenant = 0;
            CurrencyRepository curreciesRepository = new CurrencyRepository(tenant);

            int count = 0;
            string[] stringLineArray = stringOfLines.Split('\n');
            string[] readData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    stringLineArray[i] = stringLineArray[i].Replace("\"", "");

                    readData = stringLineArray[i].Split('\t');

                    Currency newEntity = new Currency();
                    newEntity.Id = IdCounter.GetNumber("Currency", tenant).ToString();

                    //Code
                    if (readData[0].Trim().Count() >= 3)
                    {
                        newEntity.Code = readData[0].Trim().Substring(0, 3);
                    }
                    else
                    {
                        newEntity.Code = readData[0].Trim();
                    }

                    //EnglishName
                    if (readData[1].Trim().Count() >= 40)
                    {
                        newEntity.EnglishName = readData[1].Trim().Substring(0, 40);
                    }
                    else
                    {
                        newEntity.EnglishName = readData[1].Trim();
                    }

                    //Service
                    if (readData[2].Trim().Count() >= 40)
                    {
                        newEntity.LocalName = readData[2].Trim().Substring(0, 40);
                    }
                    else
                    {
                        newEntity.LocalName = readData[2].Trim();
                    }

                    //ProductCode
                    if (readData[3].Trim().Count() >= 1000)
                    {
                        newEntity.SearchFields = readData[3].Trim().Substring(0, 1000);
                    }
                    else
                    {
                        newEntity.SearchFields = readData[3].Trim();
                    }

                    //check if exisit 
                    Currency c = curreciesRepository.GetSingleCurrencyByCode(newEntity.Code, newEntity.Tenant);
                    if (c == null)
                    {
                        curreciesRepository.Add(newEntity);
                        count++;
                    }
                }

                if (count == 100)
                {
                    count = 0;
                    curreciesRepository.SubmitChanges();
                }
            }

            curreciesRepository.SubmitChanges();
        }
    }
}