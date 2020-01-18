using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateVatTypeList(VatTypeList currentEntity)
        {

        }

        public List<VatTypePercentagePM> GetVatTypePercentagePMByDate(int tenant, DateTime? date)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            vatTypePercentageRepository = new VatTypePercentageRepository(objectContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);

            List<VatTypePercentagePM> myResult = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, date);

            return myResult;
        }

        public IQueryable<VatType> GetVatTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeRepository = new VatTypeRepository(tenant);
            return vatTypeRepository.GetVatTypes(0);
        }

        public IQueryable<VatTypePM> GetVatTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Tenant == tenant);
        }

        public IQueryable<VatTypePM> GetFirstVatTypes(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            input = input.ToUpper();
            return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input));
        }

        public VatTypePM GetSingleVatType(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            return vatTypeQuery.GetSinglePM(id, tenant);

           
        }

        public VatTypePM GetSingleVatTypeByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            return vatTypeQuery.GetSinglePMByCode(code, tenant);
        }

        public VatTypeList GetSingleVatTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            vatTypeRepository = new VatTypeRepository(objectContext);

            VatType entityPoco = VatTypeRepository.GetSingleVatType(id, tenant, false);

            VatTypeList vatTypeList = null;

            if (entityPoco != null)
            {
                vatTypeList = new VatTypeList()
                {
                    AddedManually = entityPoco.AddedManually,
                    Id = entityPoco.Id,
                    InActive = entityPoco.InActive,
                    LocalName = entityPoco.LocalName,
                    EnglishName = entityPoco.EnglishName,
                    Code = entityPoco.Code,
                    SearchFields = entityPoco.SearchFields,
                    Tenant = entityPoco.Tenant,
                    Description = entityPoco.Description,
                    LocalDescription = entityPoco.LocalDescription,
                    ReceivablesExternalId = entityPoco.ReceivablesExternalId,
                    ExternalTAXItemId = entityPoco.ExternalTAXItemId

                };

                //VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
                //List<VatTypePercentagePM> percentages = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, id).ToList();

                //if (percentages.Count > 0)
                //{
                //    vatTypeList.Percentage = percentages.OrderByDescending(d => d.FromDate).FirstOrDefault().Percentage;
                //}
            }

            return vatTypeList;
        }

        public List<VatTypeList> GetVatTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeRepository = new VatTypeRepository(tenant);
            vatTypeQuery = new VatTypeQuery(vatTypeRepository);

            IQueryable<VatType> iQueryable = vatTypeRepository.GetVatTypes(tenant);

            iQueryable = iQueryable.Where(d => d.IsMultiPercentage == false);

            List<VatTypeList> myResult = vatTypeQuery.GetIQueryableEntityList(iQueryable).ToList();

            //if (myResult.Count > 0)
            //{
            //    VatTypePercentageQuery vatTypePepercentageQuery = new VatTypePercentageQuery(tenant);

            //    foreach (VatTypeList item in myResult)
            //    {
            //        IQueryable<VatTypePercentagePM> vatTypePercentages = vatTypePepercentageQuery.GetVatTypePercentagesForVatType(tenant, item.Id);

            //        if (vatTypePercentages.Count() > 0)
            //        {
            //            item.Percentage = vatTypePercentages.OrderByDescending(d => d.FromDate).FirstOrDefault().Percentage;
            //        }
            //    }
            //}

            return myResult;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<VatTypeList> GetVatTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeRepository = new VatTypeRepository(tenant);
            vatTypeQuery = new VatTypeQuery(vatTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<VatType> vatTypes = vatTypeRepository.GetVatTypes(tenant);

            vatTypes = vatTypes.Where(d => d.IsMultiPercentage == false);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            vatTypes = filter.GetFilteredQuery<VatType>(nonListQueryOperation, vatTypes);

            int skippedVatTypes = queryOperations.PageIndex;

            IQueryable<VatTypeList> query2 = vatTypeQuery.GetIQueryableEntityList(vatTypes);
            query2 = filter.GetFilteredQuery<VatTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VatTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("VatType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VatTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VatTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VatTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VatTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VatTypeList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedVatTypes);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;

            //List<VatTypeList> myResult = query2.ToList();

            //if (myResult.Count > 0)
            //{
            //    VatTypePercentageQuery vatTypePepercentageQuery = new VatTypePercentageQuery(tenant);

            //    foreach (VatTypeList item in myResult)
            //    {
            //        IQueryable<VatTypePercentagePM> vatTypePercentages = vatTypePepercentageQuery.GetVatTypePercentagesForVatType(tenant, item.Id);

            //        if (vatTypePercentages.Count() > 0)
            //        {
            //            item.Percentage = vatTypePercentages.OrderByDescending(d => d.FromDate).FirstOrDefault().Percentage;
            //        }
            //    }
            //}

            //return myResult.AsQueryable();
        }

        public int GetVatTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeRepository = new VatTypeRepository(tenant);
            vatTypeQuery = new VatTypeQuery(vatTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<VatType> vatTypes = vatTypeRepository.GetVatTypes(tenant);

            vatTypes = vatTypes.Where(d => d.IsMultiPercentage == false);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            vatTypes = filter.GetFilteredQuery<VatType>(nonListQueryOperation, vatTypes);

            int skippedVatTypes = queryOperations.PageIndex;

            IQueryable<VatTypeList> query2 = vatTypeQuery.GetIQueryableEntityList(vatTypes);

            query2 = filter.GetFilteredQuery<VatTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<VatTypePM> GetVatTypesSearch(string name, string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            string nameNew = "";
            string codeNew = "";

            if (name != null)
            {
                nameNew = name;
            }
            if (code != null)
            {
                codeNew = code;
            }

            IQueryable<VatTypePM> q2 = null;
            IQueryable<VatTypePM> q = null;
            if (codeNew != null)
            {
                q = vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Code.StartsWith(codeNew)).Where(d => d.Tenant == tenant);
            }
            if (nameNew != null)
            {
                q2 = vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.EnglishName.StartsWith(nameNew)).Where(d => d.Tenant == tenant);
            }

            if (codeNew == "" && nameNew == "")
            {
                return q;
            }
            else if (codeNew == "")
            {
                return q2;
            }
            else if (nameNew == "")
            {
                return q;
            }
            else
            {
                return q.Concat(q2);
            }
        }

        public IQueryable<VatTypePM> GetVatTypesByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<VatTypePM> GetSingleVatTypeByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

            vatTypeQuery = new VatTypeQuery(tenant);
            if (byCode)
            {
                return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return vatTypeQuery.GetVatTypePMsByTenant(tenant).Where(d => d.Tenant == tenant && d.EnglishName.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public void InsertVatType(VatTypePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("VatType", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            VatTypeService service = new VatTypeService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "VatType");
        }

        public void UpdateVatType(VatTypePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("VatType", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            List<VatTypePercentagePM> vatTypePercentageChangesSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.VatTypePercentages).Cast<VatTypePercentagePM>().ToList();
           
            foreach (VatTypePercentagePM r in vatTypePercentageChangesSet)
            {
                ChangeOperation op = ChangeSet.GetChangeOperation(r);
                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert; break;                          
                        }
                    case ChangeOperation.Update:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Update; break;                            
                        }
                    case ChangeOperation.Delete:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Delete; break;                           
                        }
                    case ChangeOperation.None:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            VatTypeService service = new VatTypeService(objectContext, entityPM.Tenant);
            service.SetChangeSet(vatTypePercentageChangesSet);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "VatType");
        }

        public void DeleteVatType(VatTypePM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            VatType entity = VatTypeRepository.GetSingleVatType(entityPm.Id, entityPm.Tenant, false);
            if (entity != null)
            {
                vatTypeRepository.Remove(entity);
            }
        }
    }
}
