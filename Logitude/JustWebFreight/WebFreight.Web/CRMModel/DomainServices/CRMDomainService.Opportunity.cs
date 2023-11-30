using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.DataContracts;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Data.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public OpportunityPM GetSingleOpportunityPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            opportunityQuery = new OpportunityQueryService(crmContext);
            OpportunityPM entityPM = opportunityQuery.GetSingle(id, true, false);
            return entityPM;
        }

        public OpportunityList GetSingleOpportunityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);
            OpportunityList myResult = listService.GetSingle(id);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Opportunity", tenant, new List<OpportunityList> { myResult }.Cast<object>().ToList());

            return myResult;
        }

        public void UpdateOpportunityList(OpportunityList list)
        {

        }

        public List<OpportunityList> GetOpportunityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);
            List<OpportunityList> myResult = listService.GetList(tenant);       
    
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Opportunity", tenant, myResult.Cast<object>().ToList());

            return myResult;
        }
        
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OpportunityList> GetOpportunityFilters(byte[] xmlFilters, int tenant)
        {
            
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<OpportunityList> myResult = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Opportunity", tenant, myResult.Cast<object>().ToList());

            return myResult;

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OpportunityList> GetOpportunityFiltersHybrid(byte[] xmlFilters, int tenant)
        {

            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<OpportunityList> myResult = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Opportunity", tenant, myResult.Cast<object>().ToList());

            return myResult;

        }

        public int GetOpportunityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityListQueryService queryService = new OpportunityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public List<OpportunityList> GetRecentOpportunities(string ownerId, string businessUnitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Opportunity", 0, true);

            OpportunityListQueryService queryService = new OpportunityListQueryService(crmContext);
            IQueryable<OpportunityList> myResult = queryService.GetRecentEntityLists(ownerId, businessUnitId, tenant, contact.Id, objectTable.Id).AsQueryable();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Opportunity", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        public void InsertOpportunity(OpportunityPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckFeatureAccessLevelPermission("Opportunity", "NEW", entityPM.OwnerId, entityPM.BusinessUnitId, entityPM.Tenant);
           
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;

            
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                if (loggedContact != null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }
            

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            OpportunityUpdateService service = new OpportunityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Opportunity", 0, true);
            ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", entityPM.UpdatedByUserId);
        }

        public void UpdateOpportunity(OpportunityPM entityPM)
        {            
            opportunityRepository = new OpportunityRepository(entityPM.Tenant);
            OpportunityKeys keys = new OpportunityKeys() { Id = entityPM.Id };
            Opportunity entity_Poco = opportunityRepository.GetSingle(keys);
              
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckFeatureAccessLevelPermission("Opportunity", "UPDATE", entity_Poco.OwnerId, entity_Poco.BusinessUnitId, entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdateDate = todayDate;

            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            OpportunityUpdateService service = new OpportunityUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);

            SetOpportunityProductsChangeSet(entityPM);
            SetOpportunityCompetitorsChangeSet(entityPM);
            SetOpportunityAdditionalServicesChangeSet(entityPM);

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Opportunity", 0, true);
            ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.UpdatedByUserId);
        }

        private void SetOpportunityProductsChangeSet(OpportunityPM entityPM)
        {
            List<OpportunityProductPM> opportunityProductsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.OpportunityProducts).Cast<OpportunityProductPM>().ToList();

            foreach (OpportunityProductPM itemPM in opportunityProductsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            OpportunityProductPM currentItemPM = entityPM.OpportunityProducts.Where(d => d.OpportunityId == itemPM.OpportunityId && d.OpportunityProductTypeCode == itemPM.OpportunityProductTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetOpportunityProductLocationsChangeSet(currentItemPM);
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            OpportunityProductPM currentItemPM = entityPM.OpportunityProducts.Where(d => d.OpportunityId == itemPM.OpportunityId && d.OpportunityProductTypeCode == itemPM.OpportunityProductTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetOpportunityProductLocationsChangeSet(currentItemPM);
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            OpportunityProductPM currentItemPM = new OpportunityProductPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                OpportunityId = itemPM.OpportunityId,
                                OpportunityProductTypeCode = itemPM.OpportunityProductTypeCode,
                                OpportunityProductTypeName = itemPM.OpportunityProductTypeName
                            };

                            entityPM.DeletedOpportunityProducts.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            OpportunityProductPM currentItemPM = entityPM.OpportunityProducts.Where(d => d.OpportunityId == itemPM.OpportunityId && d.OpportunityProductTypeCode == itemPM.OpportunityProductTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetOpportunityProductLocationsChangeSet(OpportunityProductPM entityPM)
        {
            List<OpportunityProductLocationPM> entityChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.OpportunityProductLocations).Cast<OpportunityProductLocationPM>().ToList();
            foreach (OpportunityProductLocationPM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            OpportunityProductLocationPM currentItemPM = entityPM.OpportunityProductLocations.Where(d => d.OpportunityId == itemPM.OpportunityId && d.OpportunityProductTypeCode == itemPM.OpportunityProductTypeCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            OpportunityProductLocationPM currentItemPM = entityPM.OpportunityProductLocations.Where(d => d.OpportunityId == itemPM.OpportunityId && d.OpportunityProductTypeCode == itemPM.OpportunityProductTypeCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            OpportunityProductLocationPM currentItemPM = new OpportunityProductLocationPM() { ChangeSetOp = ChangeSetOperation.Delete, OpportunityId = itemPM.OpportunityId, OpportunityProductTypeCode = itemPM.OpportunityProductTypeCode, LineNumber = itemPM.LineNumber };
                            entityPM.DeletedOpportunityProductLocations.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            OpportunityProductLocationPM currentItemPM = entityPM.OpportunityProductLocations.Where(d => d.OpportunityId == itemPM.OpportunityId && d.OpportunityProductTypeCode == itemPM.OpportunityProductTypeCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetOpportunityCompetitorsChangeSet(OpportunityPM entityPM)
        {
            List<OpportunityCompetitorPM> dataChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.OpportunityCompetitors).Cast<OpportunityCompetitorPM>().ToList();

            foreach (OpportunityCompetitorPM itemPM in dataChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            OpportunityCompetitorPM currentItemPM = entityPM.OpportunityCompetitors.Where(d => d.OpportunityId == itemPM.OpportunityId && d.CompetitorId == itemPM.CompetitorId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            OpportunityCompetitorPM currentItemPM = entityPM.OpportunityCompetitors.Where(d => d.OpportunityId == itemPM.OpportunityId && d.CompetitorId == itemPM.CompetitorId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            OpportunityCompetitorPM currentItemPM = new OpportunityCompetitorPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                OpportunityId = itemPM.OpportunityId,
                                CompetitorId = itemPM.CompetitorId,
                                EnglishName = itemPM.EnglishName,
                            };

                            entityPM.DeletedOpportunityCompetitors.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            OpportunityCompetitorPM currentItemPM = entityPM.OpportunityCompetitors.Where(d => d.OpportunityId == itemPM.OpportunityId && d.CompetitorId == itemPM.CompetitorId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetOpportunityAdditionalServicesChangeSet(OpportunityPM entityPM)
        {
            List<OpportunityAdditionalServicePM> dataChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.OpportunityAdditionalServices).Cast<OpportunityAdditionalServicePM>().ToList();

            foreach (OpportunityAdditionalServicePM itemPM in dataChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            OpportunityAdditionalServicePM currentItemPM = entityPM.OpportunityAdditionalServices.Where(d => d.OpportunityId == itemPM.OpportunityId && d.AdditionalServiceId == itemPM.AdditionalServiceId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            OpportunityAdditionalServicePM currentItemPM = entityPM.OpportunityAdditionalServices.Where(d => d.OpportunityId == itemPM.OpportunityId && d.AdditionalServiceId == itemPM.AdditionalServiceId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            OpportunityAdditionalServicePM currentItemPM = new OpportunityAdditionalServicePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                OpportunityId = itemPM.OpportunityId,
                                AdditionalServiceId = itemPM.AdditionalServiceId,
                                EnglishName = itemPM.EnglishName,
                            };

                            entityPM.DeletedOpportunityAdditionalServices.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            OpportunityAdditionalServicePM currentItemPM = entityPM.OpportunityAdditionalServices.Where(d => d.OpportunityId == itemPM.OpportunityId && d.AdditionalServiceId == itemPM.AdditionalServiceId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void TraceOpportunity(OpportunityPM entity)
        {
            WebFreightDomainService freightService = new WebFreightDomainService();
            ContactRepository contactsRepository = new ContactRepository(entity.Tenant);
            ContactQuery contactQuery = new ContactQuery(contactsRepository);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), 0, true);

            //EventTracer.CreateTraceEvent(new TraceEvent(), "UPMG", 0, contact.Id, entity.Id.ToString(), null, "TenantManagement", null, null, false);

        }

        public List<OpportunityList> GetOpportunitiesByCustomerId(string customerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OpportunityListQueryService listService = new OpportunityListQueryService(crmContext);
            List<OpportunityList> myResult = listService.GetOpportunitiesByCustomerId(customerId, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("Opportunity", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        public List<ChartingDataClass> GetStageFunnelData(string ownerId, string businessUnitId, string filterCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            opportunityQuery = new OpportunityQueryService(crmContext);

            List<CRMChartingClass> data = opportunityQuery.GetStageFunnelData(ownerId, businessUnitId, filterCode, tenant, null);

            List<ChartingDataClass> result = new List<ChartingDataClass>();

            foreach (CRMChartingClass item in data)
            {
                result.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    LabelProperty = item.LabelProperty,
                    DecimalProperty = item.DecimalProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                    GroupedId = item.GroupedId,
                    DataTypeCode = filterCode,
                });
            }

            return result.OrderBy(d => d.IntegerProperty).ToList();
        }
        public List<ChartingDataClass> GetOpportunitiesChartDataCustom(DateTime?FromDate,DateTime?ToDate, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            opportunityQuery = new OpportunityQueryService(crmContext);

            List<CRMChartingClass> data = opportunityQuery.GetOpportunitiesChartDataCustom(FromDate,ToDate, ownerId, businessUnitId, chartCode, tenant);            
            foreach (CRMChartingClass item in data)
            {
                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    GroupedId = item.GroupedId,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    TypeIndex = item.TypeIndex,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpportunitiesChartData(string code, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            opportunityQuery = new OpportunityQueryService(crmContext);

            List<CRMChartingClass> data = opportunityQuery.GetOpportunitiesChartData(code, ownerId, businessUnitId, chartCode, tenant);

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            int days = Convert.ToInt32(str);

            foreach (CRMChartingClass item in data)
            {
                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    GroupedId = item.GroupedId,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    TypeIndex = item.TypeIndex,
                    Day = days,
                    OwnerId = ownerId,
                    BusinessUnitId = businessUnitId,
                    Code = code,
                });
            }

            return myResult;
        }

        public List<ChartingDataClass> GetOpportunitiesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            opportunityQuery = new OpportunityQueryService(crmContext);

            List<CRMChartingClass> data = opportunityQuery.GetOpportunitiesGroupBySalesman(code, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

            string str = code;
            if (str.Contains("_1"))
            {
                str = str.Replace("_1", "");
            }

            int days = Convert.ToInt32(str);

            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    Day = days,
                    OwnerId = item.OwnerId,
                    BusinessUnitId = businessUnitId,
                    ShortLabelProperty = myShortLabelProperty,
                    Code = code,
                });
            }

            return myResult;
        }
        public List<ChartingDataClass> GetOpportunitiesGroupBySalesmanCustom(DateTime? FromDate,DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            crmContext = CRMContext.GetContext(tenant);
            opportunityQuery = new OpportunityQueryService(crmContext);

            List<CRMChartingClass> data = opportunityQuery.GetOpportunitiesGroupBySalesmanCustom(FromDate,ToDate, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            List<ChartingDataClass> myResult = new List<ChartingDataClass>();

         


            foreach (CRMChartingClass item in data)
            {
                string myShortLabelProperty = "";
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    myShortLabelProperty = nameString[0];
                }

                myResult.Add(new ChartingDataClass()
                {
                    Id = item.Id,
                    DataTypeCode = item.DataTypeCode,
                    StringProperty = item.StringProperty,
                    IntegerProperty = item.IntegerProperty,
                    OwnerId = item.OwnerId,
                    BusinessUnitId = businessUnitId,
                    ShortLabelProperty = myShortLabelProperty,
                });
            }

            return myResult;
        }

        public List<DummyPostClass> GetPosts(int tenant)
        {
            List<DummyPostClass> result = new List<DummyPostClass>();

            List<OpportunityList> list = this.GetOpportunityLists(tenant);

            int i = 0;
            foreach (OpportunityList item in list)
            {
                i += 1;
                result.Add(new DummyPostClass()
                {
                    Id = i,
                    //OwnerId = item.OwnerId,
                    OwnerName = item.OwnerName,
                    OpportunityTopic = item.Subject,
                    PostDate = item.CreateDate,
                    Post = "Post ...",
                });
            }

            return result;
        }

        [Invoke]
        public void UpdateAccount(string opportunityId, string customerId, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            opportunityQuery = new OpportunityQueryService(crmContext);

            opportunityQuery.UpdateAccount(opportunityId, customerId, tenant);
        }

        public List<EntityPartner> GetEntityPartners(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Opportunity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            List<EntityPartner> list = new List<EntityPartner>();
                        
            opportunityQuery = new OpportunityQueryService(crmContext);
            OpportunityPM entityPM = opportunityQuery.GetSingle(entityId, false, false);

            if (entityPM != null)
            {
                int idCounter = 0;

                if (!string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    list.Add(new EntityPartner()
                    {
                        Id = idCounter++,
                        PartnerId = entityPM.CustomerId,
                        PartnerType = "Customer",
                        PartnerContactId = entityPM.ContactId,
                    });
                }

                // Ayman: please ask me if you wanted to re-open this code
                //if (!string.IsNullOrEmpty(entityPM.OwnerId))
                //{
                //    UserRepository userRepository = new UserRepository(tenant);
                //    User user = userRepository.GetSingleUser(entityPM.OwnerId, tenant, false);
                //    if (user != null)
                //    {
                //        if (user.Contact != null)
                //        {
                //            list.Add(new EntityPartner()
                //            {
                //                Id = idCounter++,
                //                PartnerId = entityPM.OwnerId,
                //                PartnerType = "Owner",
                //                PartnerContactId = user.Contact.Id,
                //                IsUser = true,
                //                PartnerContactName = user.Contact.EnglishName,
                //                PartnerContactMail = user.Contact.Email,
                //            });
                //        }
                //    }
                //}
            }
             
            return list;
        }

        [Invoke]
        public DateTime InsertOpportunityTraceEvent(OpportunityPM entityPM, DateTime? eventDate, string note, string eventTypeId, string objectTableId, string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            string entityId = entityPM.Id;       

            //opportunityRepository = new OpportunityRepository(tenant);           
            //EventTypeRepository eventTypeRep = new EventTypeRepository(tenant);
            //EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
            //WebFreightDomainService webFreightService = new WebFreightDomainService();
            //Opportunity opportunity = opportunityRepository.GetSingle(opportunityPM.Id, opportunityPM.Tenant);
            
            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = note;
            newTraceEvent.ObjectTableId = objectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = userId;
            newTraceEvent.EventTypeId = eventTypeId;
            newTraceEvent.EventDateTime = eventDate != null ? eventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.EntityId = entityId;
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = true;

            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            traceEventRep.Add(newTraceEvent);
            traceEventRep.SubmitChanges();

            return newTraceEvent.LogDateTime;
        }

        [Invoke]
        public void DeleteOpportunityTraceEvent(OpportunityPM entityPM, string traceEventId, int tenant, bool external)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
        }

        [Invoke]
        public bool OpportunityHasOpenActivities(string myOpportunityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Activity", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            
            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            bool myResult = listService.OpportunityHasOpenActivities(myOpportunityId, tenant);

            return myResult;
        }

    }
}