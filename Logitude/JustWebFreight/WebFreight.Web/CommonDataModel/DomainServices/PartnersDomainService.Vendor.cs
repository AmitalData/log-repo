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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateVendorList(VendorList currentEntity)
        {
        }

        public bool DoesVendorCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorRepository = new VendorRepository(tenant);
            return (vendorRepository.GetVendors(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<Vendor> GetVendors(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorRepository = new VendorRepository(tenant);
            return vendorRepository.GetVendors(0);
        }

        public IQueryable<VendorPM> GetVendorsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorQuery = new VendorQuery(tenant);
            return vendorQuery.GetVendorPMsByTenant(tenant);
        }

        public IQueryable<VendorPM> GetVendorsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorQuery = new VendorQuery(tenant);
            IQueryable<VendorPM> q = vendorQuery.GetVendorsByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public VendorPM GetVendorById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorQuery = new VendorQuery(tenant);
            VendorPM vendor = vendorQuery.GetSingleVendorPM(tenant, id);
            return vendor;
        }

        public VendorList GetSingleVendorList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorRepository = new VendorRepository(tenant);
            VendorList vendorList = null;
            Vendor vendor = vendorRepository.GetSingleVendor(tenant, id);

            if (vendor != null)
            {
                List<Vendor> singleEntityList = new List<Vendor>();
                singleEntityList.Add(vendor);

                vendorQuery = new VendorQuery(vendorRepository);
                IQueryable<Vendor> iQueryable = singleEntityList.AsQueryable();
                IQueryable<VendorList> iQueryableEntityList = vendorQuery.GetIQueryableEntityList(iQueryable);
                vendorList = iQueryableEntityList.FirstOrDefault();
            }
            return vendorList;
        }

        public IQueryable<VendorList> GetVendorLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorRepository = new VendorRepository(tenant);
            vendorQuery = new VendorQuery(vendorRepository);

            IQueryable<Vendor> vendors = vendorRepository.GetVendors(tenant);
            IQueryable<VendorList> query2 = vendorQuery.GetIQueryableEntityList(vendors);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<VendorList> GetVendorFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorRepository = new VendorRepository(tenant);
            vendorQuery = new VendorQuery(vendorRepository);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Vendor> vendors = vendorRepository.GetVendors(tenant);
            
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            VendorCustomFilter customfilters = new VendorCustomFilter(tenant);
            vendors = customfilters.GetFilteredQuery(queryOperations, vendors);

            vendors = filter.GetFilteredQuery<Vendor>(nonListQueryOperation, vendors);
            int skippedPorts = queryOperations.PageIndex;//queryOperations.PageSize * (queryOperations.PageIndex - 1);

            IQueryable<VendorList> query2 = vendorQuery.GetIQueryableEntityList(vendors);
            query2 = filter.GetFilteredQuery<VendorList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VendorList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Vendor", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VendorList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VendorList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VendorList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VendorList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<VendorList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VendorList, bool>(queryOperations, query2);
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

        public int GetVendorFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            vendorRepository = new VendorRepository(tenant);
            vendorQuery = new VendorQuery(vendorRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Vendor> vendors = vendorRepository.GetVendors(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            VendorCustomFilter customfilters = new VendorCustomFilter(tenant);
            vendors = customfilters.GetFilteredQuery(queryOperations, vendors);

            vendors = filter.GetFilteredQuery<Vendor>(nonListQueryOperation, vendors);

            IQueryable<VendorList> query2 = vendorQuery.GetIQueryableEntityList(vendors);
            query2 = filter.GetFilteredQuery<VendorList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertVendor(VendorPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            SecurityUtility.CheckContactFeature("Vendor", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            VendorService service = new VendorService(objectContext, entityPm.Tenant);
            service.Create(entityPm);
        }

        public void UpdateVendor(VendorPM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("Vendor", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

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


            VendorService service = new VendorService(objectContext, currentEntity.Tenant);
            service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
            service.Update(currentEntity);
        }

        public void DeleteVendor(VendorPM vendor)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(vendor.Tenant);
            }
            vendorRepository = new VendorRepository(objectContext);
            Vendor entity = vendorRepository.GetSingleVendor(vendor.Tenant, vendor.Id);
            vendorRepository.Remove(entity);
        }
    }
}
