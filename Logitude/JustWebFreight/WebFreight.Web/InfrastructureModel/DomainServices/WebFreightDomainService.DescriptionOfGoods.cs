using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateDescriptionOfGoodsList(DescriptionOfGoodsList currentEntity)
        {
        }

        public IQueryable<DescriptionOfGoods> GetDescriptionOfGoods(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(tenant);
            return descriptionOfGoodsRepository.GetDescriptionOfGoodsByTenant(0);
        }

        public DescriptionOfGoodsPM GetSingleDescriptionOfGoods(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(tenant);
            descriptionOfGoodsQuery =new DescriptionOfGoodsQuery(descriptionOfGoodsRepository);
            return descriptionOfGoodsQuery.GetSingleDescriptionOfGoodsPM(id);
        }

        public DescriptionOfGoodsList GetSingleDescriptionOfGoodslList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(tenant);
            DescriptionOfGoodsList descriptionOfGoodsList = null;
            DescriptionOfGoods descriptionOfGoods = descriptionOfGoodsRepository.GetSingleDescriptionOfGoods(id);

            if (descriptionOfGoods != null)
            {
                List<DescriptionOfGoods> singleEntityList = new List<DescriptionOfGoods>();
                singleEntityList.Add(descriptionOfGoods);

                IQueryable<DescriptionOfGoods> iQueryable = singleEntityList.AsQueryable();
                descriptionOfGoodsQuery = new DescriptionOfGoodsQuery(descriptionOfGoodsRepository);
                IQueryable<DescriptionOfGoodsList> iQueryableEntityList = descriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);
                descriptionOfGoodsList = iQueryableEntityList.FirstOrDefault();
            }
            return descriptionOfGoodsList;
        }

        public IQueryable<DescriptionOfGoodsList> GetDescriptionOfGoodsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(tenant);
            IQueryable<DescriptionOfGoods> iQueryable = descriptionOfGoodsRepository.GetDescriptionOfGoodsByTenant(tenant);
            descriptionOfGoodsQuery = new DescriptionOfGoodsQuery(descriptionOfGoodsRepository);
            IQueryable<DescriptionOfGoodsList> query2 = descriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DescriptionOfGoodsList> GetDescriptionOfGoodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<DescriptionOfGoods> iQueryable = descriptionOfGoodsRepository.GetDescriptionOfGoodsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DescriptionOfGoods>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            descriptionOfGoodsQuery = new DescriptionOfGoodsQuery(descriptionOfGoodsRepository);
            IQueryable<DescriptionOfGoodsList> query2 = descriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DescriptionOfGoodsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DescriptionOfGoodsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DescriptionOfGood", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DescriptionOfGoodsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DescriptionOfGoodsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DescriptionOfGoodsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DescriptionOfGoodsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DescriptionOfGoodsList, bool>(queryOperations, query2);
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

        public int GetDescriptionOfGoodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<DescriptionOfGoods> iQueryable = descriptionOfGoodsRepository.GetDescriptionOfGoodsByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DescriptionOfGoods>(nonListQueryOperation, iQueryable);
            descriptionOfGoodsQuery = new DescriptionOfGoodsQuery(descriptionOfGoodsRepository);
            IQueryable<DescriptionOfGoodsList> query2 = descriptionOfGoodsQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DescriptionOfGoodsList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapDescriptionOfGoodsPMDescriptionOfGoods(DescriptionOfGoodsPM descriptionOfGoodsPM, DescriptionOfGoods descriptionOfGoods)
        //{
        //    descriptionOfGoods.AddedManually = descriptionOfGoodsPM.AddedManually;
        //    descriptionOfGoods.Name = descriptionOfGoodsPM.Name;
        //    descriptionOfGoods.InActive = descriptionOfGoodsPM.InActive;
        //    descriptionOfGoods.DescriptionOfGood = descriptionOfGoodsPM.DescriptionOfGood;
        //    descriptionOfGoods.Tenant = descriptionOfGoodsPM.Tenant;
        //}

        public void InsertDescriptionOfGoods(DescriptionOfGoodsPM entity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            DescriptionOfGoodsService service = new DescriptionOfGoodsService(objectContext , entity.Tenant);
            service.Create(entity);

            //descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(objectContext);
            //DescriptionOfGoods newEntity = new DescriptionOfGoods();
            //entity.Id = IdCounter.GetNumber("DescriptionOfGoods", entity.Tenant).ToString();
            //newEntity.Id = entity.Id;
            //MapDescriptionOfGoodsPMDescriptionOfGoods(entity, newEntity);
            //descriptionOfGoodsRepository.Add(newEntity);
        }

        public void UpdateDescriptionOfGoods(DescriptionOfGoodsPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            DescriptionOfGoodsService service = new DescriptionOfGoodsService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(objectContext);
            //DescriptionOfGoods entity = descriptionOfGoodsRepository.GetSingleDescriptionOfGoods(currentEntity.Id);
            //MapDescriptionOfGoodsPMDescriptionOfGoods(currentEntity, entity);
            //descriptionOfGoodsRepository.Update(entity);
        }

        public void DeleteDescriptionOfGoods(DescriptionOfGoodsPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(entityPM.Tenant);
            }
            descriptionOfGoodsRepository = new DescriptionOfGoodsRepository(objectContext);
            DescriptionOfGoods entity = descriptionOfGoodsRepository.GetSingleDescriptionOfGoods(entityPM.Id);
            descriptionOfGoodsRepository.Remove(entity);
        }     
    }
}