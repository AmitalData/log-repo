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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.CustomFilters;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public IQueryable<Address> GetAddresses(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            AddressRepository = new AddressRepository(tenant);
            return this.AddressRepository.GetAddresses(0);
        }

        public AddressPM GetSingleAddress(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetSinglePM(id, tenant);
        }

        public AddressList GetSingleAddressList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AddressRepository = new AddressRepository(tenant);
            AddressList addressList = null;
            Address address = AddressRepository.GetSingleAddress(id, tenant);

            if (address != null)
            {
                List<Address> singleEntityList = new List<Address>();
                singleEntityList.Add(address);

                addressQuery = new AddressQuery(AddressRepository);
                IQueryable<Address> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AddressList> iQueryableEntityList = addressQuery.GetIQueryableEntityList(iQueryable);
                addressList = iQueryableEntityList.FirstOrDefault();
            }
            return addressList;
        }

        public void UpdateAddressList(AddressList currentEntity)
        {

        }

        public IQueryable<AddressList> GetAddressLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AddressRepository = new AddressRepository(tenant);
            addressQuery = new AddressQuery(AddressRepository);
            IQueryable<Address> iQueryable = AddressRepository.GetAddresses(tenant);
            IQueryable<AddressList> query2 = addressQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AddressList> GetAddressFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AddressRepository = new AddressRepository(tenant);
            addressQuery = new AddressQuery(AddressRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Address> iQueryable = AddressRepository.GetAddresses(tenant);

            PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Address>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<AddressList> query2 = addressQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<AddressList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AddressList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Address", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AddressList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AddressList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AddressList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AddressList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<AddressList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AddressList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Address1);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Address1);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAddressFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AddressRepository = new AddressRepository(tenant);
            addressQuery = new AddressQuery(AddressRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Address> iQueryable = AddressRepository.GetAddresses(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Address>(nonListQueryOperation, iQueryable);

            IQueryable<AddressList> query2 = addressQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<AddressList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public AddressPM GetMainAddressByCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetAddressPMByTypeAndCard(cardId, "M", tenant);
        }

        public AddressPM GetBillingAddressByCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetAddressPMByTypeAndCard(cardId, "B", tenant);
        }

        public AddressPM GetPickupDeliveryAddressByCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetAddressPMByTypeAndCard(cardId, "P", tenant);
        }

        public AddressList GetMainAddressListByCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetAddressListByTypeAndCard(cardId, "M", tenant);
        }

        public AddressList GetBillingAddressListByCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetAddressListByTypeAndCard(cardId, "B", tenant);
        }

        public AddressList GetPickupDeliveryAddressListByCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return addressQuery.GetAddressListByTypeAndCard(cardId, "P", tenant);
        }

        public IQueryable<AddressPM> GetAddressesByTenant(int tenant)
        {
            addressQuery = new AddressQuery(tenant);
            return this.addressQuery.GetAddressePMsByTenant(tenant);
        }

        public IQueryable<AddressPM> GetAllAddressesbyCardID(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return this.addressQuery.GetAddressePMsByTenant(tenant).Where(a => a.CardId == cardId);
        }

        public IQueryable<AddressPM> GetOtherAddressesbyCardID(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressQuery = new AddressQuery(tenant);
            return this.addressQuery.GetAddressePMsByTenant(tenant).Where(a => a.CardId == cardId && a.AddressTypeId == "O");
        }

        public void InsertAddress(AddressPM entityPm)
        {
            if (!entityPm.IsCreatedWithPartner)
            {
                if (!string.IsNullOrEmpty(entityPm.CardId))
                {
                    if (objectContext == null)
                    {
                        objectContext = CommonDataContext.GetContext(entityPm.Tenant);
                    }

                    AddressService service = new AddressService(objectContext, entityPm.Tenant);
                    service.Create(entityPm);
                }
            }
        }

        public void UpdateAddress(AddressPM currententityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currententityPm.Tenant);
            }

            AddressService service = new AddressService(objectContext, currententityPm.Tenant);
            service.Update(currententityPm);
        }

        public void DeleteAddress(AddressPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }
            AddressRepository = new AddressRepository(objectContext);
            Address removedEntity = AddressRepository.GetSingleAddress(entityPm.Id, entityPm.Tenant);
            this.AddressRepository.Remove(removedEntity);
        }
    }
}