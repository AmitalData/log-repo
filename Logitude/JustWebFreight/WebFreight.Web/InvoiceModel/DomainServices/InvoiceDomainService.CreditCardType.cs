using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.Tools.EntityService;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public void UpdateCreditCardTypeList(CreditCardTypeList list)
        {

        }

        public IQueryable<CreditCardType> GetCreditCardTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            creditCardTypeRepository = new CreditCardTypeRepository(tenant);
            return creditCardTypeRepository.GetCreditCardTypes(0);
        }

        public IQueryable<CreditCardTypePM> GetCreditCardTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            creditCardTypeQuery = new CreditCardTypeQuery(tenant);
            return creditCardTypeQuery.GetCreditCardTypePMs(tenant).Where(d => d.Tenant == tenant);
        }

        public bool DoesCreditCardTypeExist(string code, int tenant)
        {
            creditCardTypeRepository = new CreditCardTypeRepository(tenant);
            return (creditCardTypeRepository.GetCreditCardTypes(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<CreditCardTypeList> GetCreditCardTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            creditCardTypeRepository = new CreditCardTypeRepository(tenant);
            creditCardTypeQuery = new CreditCardTypeQuery(creditCardTypeRepository);

            IQueryable<CreditCardType> iQueryable = creditCardTypeRepository.GetCreditCardTypes(tenant);
            IQueryable<CreditCardTypeList> query2 = creditCardTypeQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CreditCardTypeList> GetCreditCardTypeFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            creditCardTypeRepository = new CreditCardTypeRepository(tenant);
            creditCardTypeQuery = new CreditCardTypeQuery(creditCardTypeRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CreditCardType> iQueryable = creditCardTypeRepository.GetCreditCardTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CreditCardType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CreditCardTypeList> query2 = creditCardTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CreditCardTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CreditCardTypeList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<CreditCardTypeList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<CreditCardTypeList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<CreditCardTypeList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<CreditCardTypeList, int>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Code);
                            break;
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

        public int GetCreditCardTypeCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            creditCardTypeRepository = new CreditCardTypeRepository(tenant);
            creditCardTypeQuery = new CreditCardTypeQuery(creditCardTypeRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CreditCardType> iQueryable = creditCardTypeRepository.GetCreditCardTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CreditCardType>(nonListQueryOperation, iQueryable);

            IQueryable<CreditCardTypeList> query2 = creditCardTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CreditCardTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public CreditCardTypeList GetSingleCreditCardTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            CreditCardTypeList entityList = null;
            creditCardTypeRepository = new CreditCardTypeRepository(tenant);
            creditCardTypeQuery = new CreditCardTypeQuery(creditCardTypeRepository);
            CreditCardType entity = creditCardTypeRepository.GetSingleCreditCardType(id, tenant);

            if (entity != null)
            {
                List<CreditCardType> singleEntityList = new List<CreditCardType>();
                singleEntityList.Add(entity);

                IQueryable<CreditCardType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CreditCardTypeList> iQueryableEntityList = creditCardTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public CreditCardTypePM GetSingleCreditCardType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CreditCardType", "READ", tenant);

            creditCardTypeQuery = new CreditCardTypeQuery(tenant);
            return creditCardTypeQuery.GetSinglePM(id, tenant);
        }

        public void MapCreditCardCreditCardPM(CreditCardTypePM entityPM, CreditCardType entity)
        {
            entity.Tenant = entityPM.Tenant;
            entity.Code = entityPM.Code;
            entity.Name = entityPM.Name;
            entity.InActive = entityPM.InActive;
            entity.SearchFields = entityPM.Code + "," + entityPM.Name;
        }

        public void InsertCreditCardType(CreditCardTypePM entityPM)
        {
            SecurityUtility.CheckContactFeature("CreditCardType", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }
            creditCardTypeRepository = new CreditCardTypeRepository(objectContext);

            entityPM.Id = IdCounter.GetNumber("CreditCardType", entityPM.Tenant).ToString();

            bool exist = (from a in creditCardTypeRepository.GetCreditCardTypes(entityPM.Tenant)
                          where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                          select a).Any();
            if (!exist)
            {
                CreditCardTypeService service = new CreditCardTypeService(objectContext, entityPM.Tenant);
                service.Create(entityPM);

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CreditCardType");
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "CreditCardType");
                throw new Exception(msg);
            }         
        }

        public void UpdateCreditCardType(CreditCardTypePM entityPM)
        {
            SecurityUtility.CheckContactFeature("CreditCardType", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }
            creditCardTypeRepository = new CreditCardTypeRepository(objectContext);

            string entityName = "CreditCardType" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "CreditCardTypePM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            bool exist = (from a in creditCardTypeRepository.GetCreditCardTypes(entityPM.Tenant)
                          where a.Code == entityPM.Code && a.Id != entityPM.Id && a.Tenant == entityPM.Tenant
                          select a).Any();

            if (!exist)
            {
                CreditCardTypeService service = new CreditCardTypeService(objectContext, entityPM.Tenant);
                service.Update(entityPM);

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CreditCardType");
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "CreditCardType");
                throw new Exception(msg);

            }
        }

        public void DeleteCreditCardType(CreditCardTypePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }
            creditCardTypeRepository = new CreditCardTypeRepository(objectContext);

            CreditCardType entity = creditCardTypeRepository.GetSingleCreditCardType(entityPM.Id, entityPM.Tenant);
            creditCardTypeRepository.Remove(entity);
        }
    }
}