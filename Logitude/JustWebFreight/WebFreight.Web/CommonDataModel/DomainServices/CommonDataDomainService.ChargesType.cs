using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateChargesTypeList(ChargesTypeList currentEntity)
        {
        }

        public bool DoesChargesTypeCodeExist(string code, int tenant)
        {
            chargesTypeQuery = new ChargesTypeQuery(tenant);
            return (chargesTypeQuery.GetChargesTypeByCodeOrName(code, null, tenant).Any());
        }

        public IQueryable<ChargesTypePM> GetChargesTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            chargesTypeQuery = new ChargesTypeQuery(tenant);
            return chargesTypeQuery.GetChargesTypePMsByTenant(tenant);
        }

        public ChargesTypePM GetSingleChargesType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            chargesTypeQuery = new ChargesTypeQuery(tenant);
            return chargesTypeQuery.GetSinglePM(id, tenant);
        }

        public ChargesTypeList GetSingleChargesTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            chargesTypeQuery = new ChargesTypeQuery(tenant);
            return chargesTypeQuery.GetSingleChargesType(id, tenant);
        }

        public IQueryable<ChargesTypeList> GetChargesTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            chargesTypeRepository = new ChargesTypeRepository(tenant);
            chargesTypeQuery = new ChargesTypeQuery(chargesTypeRepository);

            IQueryable<ChargesType> chargesTypes = chargesTypeRepository.GetChargesTypes(tenant);
            IQueryable<ChargesTypeList> query2 = chargesTypeQuery.GetIQueryableEntityList(chargesTypes);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ChargesTypeList> GetChargesTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            chargesTypeRepository = new ChargesTypeRepository(tenant);
            chargesTypeQuery = new ChargesTypeQuery(chargesTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ChargesType> chargesTypes = chargesTypeRepository.GetChargesTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            chargesTypes = filter.GetFilteredQuery<ChargesType>(nonListQueryOperation, chargesTypes);

            int skippedChargesTypes = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<ChargesTypeList> query2 = chargesTypeQuery.GetIQueryableEntityList(chargesTypes);
            query2 = filter.GetFilteredQuery<ChargesTypeList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ChargesType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, bool>(queryOperations, query2);
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
            
            query2 = query2.Skip(skippedChargesTypes);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetChargesTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            chargesTypeRepository = new ChargesTypeRepository(tenant);
            chargesTypeQuery = new ChargesTypeQuery(chargesTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ChargesType> chargesTypes = chargesTypeRepository.GetChargesTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            chargesTypes = filter.GetFilteredQuery<ChargesType>(nonListQueryOperation, chargesTypes);

            int skippedChargesTypes = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<ChargesTypeList> query2 = chargesTypeQuery.GetIQueryableEntityList(chargesTypes);
            query2 = filter.GetFilteredQuery<ChargesTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ChargesType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesTypeList, bool>(queryOperations, query2);
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

            //query2 = query2.Skip(skippedChargesTypes);
            //query2 = query2.Take(queryOperations.PageSize);
            return query2.Count();           
        }

        public void InsertChargesType(ChargesTypePM entity)
        {
            SecurityUtility.CheckContactFeature("ChargesType", "NEW", entity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            ChargesTypeService service = new ChargesTypeService(objectContext, entity.Tenant);
            service.Create(entity);
        }

        public void UpdateChargesType(ChargesTypePM entityPM)
        {
            SecurityUtility.CheckContactFeature("ChargesType", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            string entityName = "ChargesType" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "ChargesTypePM" + entityPM.Id + entityPM.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            List<ChargeTypeAccountingPM> chargeTypeAccountingChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ChargeTypeAccountings).Cast<ChargeTypeAccountingPM>().ToList();
            foreach (ChargeTypeAccountingPM item in chargeTypeAccountingChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(item))
                {
                    case ChangeOperation.Insert: { item.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { item.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { item.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { item.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            ChargesTypeService service = new ChargesTypeService(objectContext, entityPM.Tenant);
            service.SetChangeSet(chargeTypeAccountingChangeSet);
            service.Update(entityPM);
        }

        public void DeleteChargesType(ChargesTypePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            ChargesType chargesType = ChargesTypeRepository.GetSingleChargesType(entityPM.Id, entityPM.Tenant, false);
            
            if (chargesType != null)
            {
                ChargeTypeAccountingRepository chargeTypeAccountingRepository = new ChargeTypeAccountingRepository(entityPM.Tenant);
                foreach (ChargeTypeAccountingPM itemPM in entityPM.ChargeTypeAccountings)
                {
                    ChargeTypeAccounting itemPoco = chargeTypeAccountingRepository.GetSingleChargeTypeAccountings(itemPM.Id,entityPM.Tenant);
                    
                    if (itemPoco != null)
                    {
                        chargeTypeAccountingRepository.Remove(itemPoco);
                    }
                }

                chargesTypeRepository.Remove(chargesType);
            }
        }

        public ChargeTypeAccountingList GetSingleChargeTypeAccountingList(string chargeTypeId, string vatTypeId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ChargesType", "READ", tenant);

            ChargeTypeAccountingQuery query = new ChargeTypeAccountingQuery(tenant);
            return query.GetSingleChargeTypeAccountingList(chargeTypeId, vatTypeId, tenant);
        }

        [Invoke]
        public void InvokeUpdateAutoDisplay(string myChargeTypeId, string myPropertyTypeCode, bool isAutoDisplay, int tenant)
        {
            SecurityUtility.CheckContactFeature("ChargesType", "UPDATE", tenant);

            chargesTypeRepository = new ChargesTypeRepository(tenant);
            ChargesType myChargeType = chargesTypeRepository.GetSingleChargesType(myChargeTypeId, tenant);

            if (myChargeType != null)
            {
                switch (myPropertyTypeCode)
                {
                    case "S":
                        {
                            myChargeType.IsAutoDisplayInShipment = isAutoDisplay;
                            break;
                        }

                    case "C":
                        {
                            myChargeType.IsAutoDisplayInConsolidation = isAutoDisplay;
                            break;
                        }
                }

                chargesTypeRepository.Update(myChargeType);
                chargesTypeRepository.SubmitChanges();
            }
        }
    }
}