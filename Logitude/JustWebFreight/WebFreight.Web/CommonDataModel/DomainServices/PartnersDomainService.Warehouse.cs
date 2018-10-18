using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateWarehouseList(WarehouseList currentEntity)
        {
        }

        public IQueryable<Warehouse> GetWarehouses(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseRepository = new WarehouseRepository(tenant);
            return warehouseRepository.GetWarehousesByTenant(0);
        }

        public WarehousePM GetSingleWarehouse(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseQuery = new WarehouseQuery(tenant);
            return warehouseQuery.GetSingleWarehousePM(id, tenant);
        }

        public WarehousePM GetWarehouseByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseQuery = new WarehouseQuery(tenant);
            return warehouseQuery.GetSinglePMByCode(code, tenant);
        }

        public WarehouseList GetSingleWarehouseList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseRepository = new WarehouseRepository(tenant);
            Warehouse warehouse = warehouseRepository.GetSingleWarehouse(id);
            WarehouseList warehouseList = null;

            if (warehouse != null)
            {
                List<Warehouse> singleEntityList = new List<Warehouse>();
                singleEntityList.Add(warehouse);

                warehouseQuery = new WarehouseQuery(warehouseRepository);
                IQueryable<Warehouse> iQueryable = singleEntityList.AsQueryable();
                IQueryable<WarehouseList> iQueryableEntityList = warehouseQuery.GetIQueryableEntityList(iQueryable);
                warehouseList = iQueryableEntityList.FirstOrDefault();
            }
            return warehouseList;
        }

        public IQueryable<WarehouseList> GetWarehouseLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseRepository = new WarehouseRepository(tenant);
            warehouseQuery = new WarehouseQuery(warehouseRepository);

            IQueryable<Warehouse> iQueryable = warehouseRepository.GetWarehousesByTenant(tenant);
            IQueryable<WarehouseList> query2 = warehouseQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<WarehouseList> GetWarehouseFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseRepository = new WarehouseRepository(tenant);
            warehouseQuery = new WarehouseQuery(warehouseRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Warehouse> iQueryable = warehouseRepository.GetWarehousesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Warehouse>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<WarehouseList> query2 = warehouseQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WarehouseList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(WarehouseList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Warehouse", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<WarehouseList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetWarehouseFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "READ", tenant);

            warehouseRepository = new WarehouseRepository(tenant);
            warehouseQuery = new WarehouseQuery(warehouseRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Warehouse> iQueryable = warehouseRepository.GetWarehousesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Warehouse>(nonListQueryOperation, iQueryable);

            IQueryable<WarehouseList> query2 = warehouseQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<WarehouseList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertWarehouse(WarehousePM entity)
        {
            SecurityUtility.AuthenticationOnTenant(entity.Tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "NEW", entity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
             
            warehouseRepository = new WarehouseRepository(objectContext);
            AddressRepository = new AddressRepository(objectContext);
            CardRepository = new CardRepository(objectContext);
            ContactRepository = new ContactRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);

            bool exist = (from a in warehouseRepository.GetWarehousesByTenant(entity.Tenant)
                          where a.Card.Code == entity.Code && a.Tenant == entity.Tenant
                          select a).Any();
            if (!exist)
            {
                WarehouseService service = new WarehouseService(objectContext, entity.Tenant);
                service.Create(entity);
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entity.Tenant);
                msg = msg.Replace("%Entity", "Warehouse");
                throw new Exception(msg);
            }
        }

        public void UpdateWarehouse(WarehousePM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("Warehouse", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
                    

            warehouseRepository = new WarehouseRepository(objectContext);
            CardRepository = new CardRepository(objectContext);
            cardQuery = new CardQuery(CardRepository);
            ContactRepository = new ContactRepository(objectContext);

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
            foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrenciesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }

            }
            CardPM c = cardQuery.GetSinglePM(currentEntity.Id, currentEntity.Tenant);

            bool exist = (from a in warehouseRepository.GetWarehousesByTenant(currentEntity.Tenant)
                          where a.Card.Code == c.Code
                          && a.Id != currentEntity.Id && a.Tenant == currentEntity.Tenant
                          select a).Any();

            if (!exist)
            {
                WarehouseService service = new WarehouseService(objectContext, currentEntity.Tenant);
                service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
                service.Update(currentEntity);
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentEntity.Tenant);
                msg = msg.Replace("%Entity", "Warehouse");
                throw new Exception(msg);
            }

        }

        public void DeleteWarehouse(WarehousePM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            warehouseRepository = new WarehouseRepository(objectContext);
            Warehouse entityStatus = warehouseRepository.GetSingleWarehouse(entity.Id);
            warehouseRepository.Remove(entityStatus);
        }
    }
}
