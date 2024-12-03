using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateChargesGroupList(ChargesGroupList currentEntity)
        {
        }

        public IQueryable<ChargesGroup> GetChargesGroups(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            return chargesGroupsRepository.GetChargesGroups(tenant);
        }

        public IQueryable<ChargesGroupPM> GetChargesGroupsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            return chargesGroupQuery.GetChargesGroupPMs();
        }

        public IQueryable<ChargesGroup> GetFirstChargesGroups(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            input = input.ToUpper();
            return chargesGroupsRepository.GetChargesGroups(tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.Name.ToUpper().StartsWith(input));
        }

        public ChargesGroupList GetSingleChargreGroupList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            ChargesGroupList chargesGroupList = null;
            ChargesGroup chargesGroup = chargesGroupsRepository.GetSingleChargesGroup(id, tenant);

            if (chargesGroup != null)
            {
                List<ChargesGroup> singleEntityList = new List<ChargesGroup>();
                singleEntityList.Add(chargesGroup);

                IQueryable<ChargesGroup> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ChargesGroupList> iQueryableEntityList = chargesGroupQuery.GetIQueryableEntityList(iQueryable);
                chargesGroupList = iQueryableEntityList.FirstOrDefault();
            }
            return chargesGroupList;
        }

        public ChargesGroupPM GetSingleChargeGroup(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            chargesGroupQuery = new ChargesGroupQuery(tenant);

            return chargesGroupQuery.GetSingleChargesGroupPM(id , tenant);
        }

        public ChargesGroupPM GetSingleChargeGroupByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            chargesGroupQuery = new ChargesGroupQuery(tenant);

            return chargesGroupQuery.GetSingleChargesGroupPMByCode(code, tenant);
        }


        public IQueryable<ChargesGroupList> GetChargeGroupLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            IQueryable<ChargesGroup> iQueryable = chargesGroupsRepository.GetChargesGroups(tenant);
            IQueryable<ChargesGroupList> query2 = chargesGroupQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        public IQueryable<ChargesGroup> GetChargesGroupsByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {
                if (byCode)
                {
                    return chargesGroupsRepository.GetChargesGroups(tenant).Where(d => d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return chargesGroupsRepository.GetChargesGroups(tenant).Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return chargesGroupsRepository.GetChargesGroups(tenant);
            }
        }

        public IQueryable<ChargesGroup> GetSingleChargesGroupByTenantInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            if (byCode)
            {
                return chargesGroupsRepository.GetChargesGroups(tenant).Where(d => d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return chargesGroupsRepository.GetChargesGroups(tenant).Where(d => d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ChargesGroupList> GetChargeGroupFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ChargesGroup> chargreGroups = chargesGroupsRepository.GetChargesGroups(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            chargreGroups = filter.GetFilteredQuery<ChargesGroup>(nonListQueryOperation, chargreGroups);

            int skippedChargregroups = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            IQueryable<ChargesGroupList> query2 = chargesGroupQuery.GetIQueryableEntityList(chargreGroups);
            query2 = filter.GetFilteredQuery<ChargesGroupList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ChargesGroupList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ChargesGroup", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ChargesGroupList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedChargregroups);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetChargeGroupFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            chargesGroupsRepository = new ChargesGroupRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ChargesGroup> chargreGroups = chargesGroupsRepository.GetChargesGroups(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            chargreGroups = filter.GetFilteredQuery<ChargesGroup>(nonListQueryOperation, chargreGroups);

            int skippedVatTypes = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            chargesGroupQuery = new ChargesGroupQuery(chargesGroupsRepository);
            IQueryable<ChargesGroupList> query2 = chargesGroupQuery.GetIQueryableEntityList(chargreGroups);
            query2 = filter.GetFilteredQuery<ChargesGroupList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertChargesGroup(ChargesGroupPM entity)
        {
            if (objectContext == null) objectContext = WebFreightContext.GetContext(entity.Tenant);
     
            ChargesGroupService ChargesGroupService = new ChargesGroupService(objectContext, entity.Tenant);
            ChargesGroupService.Create(entity);

        }

        public void UpdateChargesGroup(ChargesGroupPM currentEntity)
        {
            if (objectContext == null) objectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            ChargesGroupService ChargesGroupService = new ChargesGroupService(objectContext, currentEntity.Tenant);
            ChargesGroupService.Update(currentEntity);
           

           
        }

        public void DeleteChargesGroup(ChargesGroup entity)
        {
            chargesGroupsRepository = new ChargesGroupRepository(entity.Tenant);
            chargesGroupsRepository.Remove(entity);
            chargesGroupsRepository.SubmitChanges();
        }
    }
}