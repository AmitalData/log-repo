using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure;
using System.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsVendorPM GetSingleVendorPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            vendorQuery = new CustomsVendorQueryService(customContext);
            CustomsVendorPM vendor = vendorQuery.GetSingle(id,true, false);
            return vendor;
        }

        public CustomsVendorList GetSingleVendorList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsVendorListQueryService listService = new CustomsVendorListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsVendorList> GetVendorLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsVendorListQueryService listService = new CustomsVendorListQueryService(customContext);
            return listService.GetList(tenant);
        }

        [Query(HasSideEffects = true)]
        public List<CustomsVendorList> GetCustomsVendorCompactFilters(byte[] xmlFilters, int tenant)
        {


            customContext = CustomContext.GetContext(tenant);
            CustomsVendorListQueryService listService = new CustomsVendorListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);



            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);
            object seachvalue = item != null ? item.FieldValue : null;
            List<CustomsVendorList> resultList = null;


            if (seachvalue != null)
            {
                int count = listService.GetListCount(queryOperations,tenant);

                queryOperations.SetFilter("VendorNumber", seachvalue, false, "StartsWith", null, false);
                queryOperations.SortByColumnName = "VendorNumber";
                queryOperations.SortDirectin = "Ascending";



                List<CustomsVendorList> codeQueryResult = listService.GetList(queryOperations, tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize && count > resultList.Count())
                {
                    queryOperations.SetFilter("VendorNumber", null, false, "StartsWith", null, false);
                    queryOperations.SetFilter("VendorName", seachvalue, false, "StartsWith", null, false);
                    queryOperations.SortByColumnName = "VendorName";

                    List<CustomsVendorList> nameQueryResult = listService.GetList(queryOperations, tenant);



                    foreach (CustomsVendorList site in nameQueryResult)
                    {
                        if (!resultList.Where(p => p.VendorNumber == site.VendorNumber).Any())
                        {
                            resultList.Add(site);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        queryOperations.SetFilter("VendorNumber", null, false, "StartsWith", null, false);
                        queryOperations.SetFilter("VendorName", null, false, "StartsWith", null, false);
                        queryOperations.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);
                        queryOperations.SortByColumnName = "VendorNumber";

                        List<CustomsVendorList> searchFieldQueryResult = listService.GetList(queryOperations, tenant);

                        foreach (CustomsVendorList vendor in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.VendorNumber == vendor.VendorNumber).Any())
                            {
                                resultList.Add(vendor);

                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }

            }
            else
            {
                resultList = listService.GetList(queryOperations, tenant);
            }

            return resultList;
        }


        public List<CustomsVendorList> GetCustomsVendorFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsVendorListQueryService listService = new CustomsVendorListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
           
        }

        public int GetCustomsVendorFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsVendorListQueryService queryService = new CustomsVendorListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertVendor(CustomsVendorPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsVendorUpdateService service = new CustomsVendorUpdateService(customContext,new Dictionary<string,IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (VendorCommunicationPM vendorcomm in entityPm.VendorCommunications)
            {
                vendorcomm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPm,true);

        }

        public void UpdateVendor(CustomsVendorPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsVendorUpdateService service = new CustomsVendorUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetVendorCommunicationChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetVendorCommunicationChangeSet(CustomsVendorPM currententityPm)
        {
            List<VendorCommunicationPM> vendorCommunicationchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.VendorCommunications).Cast<VendorCommunicationPM>().ToList();
            foreach (VendorCommunicationPM itemPM in vendorCommunicationchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            VendorCommunicationPM currentItemPM = currententityPm.VendorCommunications.Where(d => d.VendorId == itemPM.VendorId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            VendorCommunicationPM currentItemPM = currententityPm.VendorCommunications.Where(d => d.VendorId == itemPM.VendorId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            VendorCommunicationPM currentItemPM = new VendorCommunicationPM() { ChangeSetOp = ChangeSetOperation.Delete, VendorId = itemPM.VendorId, LineNumber = itemPM.LineNumber };
                            currententityPm.DeletedVendorCommunications.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            VendorCommunicationPM currentItemPM = currententityPm.VendorCommunications.Where(d => d.VendorId == itemPM.VendorId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdateVendorList(CustomsVendorList list)
        {

        }

        public void DeleteVendor(CustomsVendorPM entityPM)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsVendor", "UPDATE", entityPM.Tenant);
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPM.Tenant);
            }
            List<VendorCommunicationPM> vendorCommunicationchangeset = ChangeSet.GetAssociatedChanges(entityPM, d => d.VendorCommunications).Cast<VendorCommunicationPM>().ToList();
            CustomsVendorUpdateService service = new CustomsVendorUpdateService(customContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            foreach (VendorCommunicationPM communication in vendorCommunicationchangeset)
            {
                VendorCommunicationPM deletedComm = new VendorCommunicationPM()
                {
                    ChangeSetOp = ChangeSetOperation.Delete,
                    VendorId = communication.VendorId,
                    Tenant = communication.Tenant,
                    CommunicationAddress = communication.CommunicationAddress,
                    CommunicationTypeCode = communication.CommunicationTypeCode,
                    LineNumber = communication.LineNumber,
                };
                entityPM.DeletedVendorCommunications.Add(deletedComm);
            }
           
           
            service.Update(entityPM, true);
        }

        public CustomsVendorPM GetVendorByVendorNumber(string vendorNumber, int tenant)
        {
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsVendorQueryService queryService = new CustomsVendorQueryService(customContext);
            return queryService.GetVendorByNumber(vendorNumber, tenant);
        }

   
    }
}