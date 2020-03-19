
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.CRM.BL.EntityDataMappings
{
    public partial class ActivityDataMapping : IMapping<ActivityPM, Activity>
    {
        public void CustomPMToPOCO(ActivityPM entityPM, Activity entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field1);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field2);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field3);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field4);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field5);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field6);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field7);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field8);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field9);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field10);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            //entityPOCO.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            //entityPOCO.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            //entityPOCO.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            //entityPOCO.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            //entityPOCO.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            //entityPOCO.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            //entityPOCO.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            //entityPOCO.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            //entityPOCO.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            //entityPOCO.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;

            entityPOCO.ConcurrencyGUID = Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);

            if (entityPM.IsHybrid)
            {
            //    this.CustomMappedPOCOProperties.Add(POCOPropertyNames.MeetingSummary);
            }
         
        }

        public void CustomPOCOToPM(ActivityPM entityPM, Activity entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ActivityTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ActivityStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OpportunitySubject);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.QuoteNumber);

            ActivityTypeRepository activityTypeRepository = new ActivityTypeRepository(entityPOCO.Tenant);
            ActivityTypeKeys activityTypeKeys = new ActivityTypeKeys() { Code = entityPOCO.ActivityTypeCode };
            ActivityType activityType = activityTypeRepository.GetSingle(activityTypeKeys);
            if (activityType != null)
            {
                entityPM.ActivityTypeName = activityType.Name;
            }

            ActivityStatusRepository activityStatusRepository = new ActivityStatusRepository(entityPOCO.Tenant);
            ActivityStatusKeys activityStatusKeys = new ActivityStatusKeys() { Code = entityPOCO.ActivityStatusCode };
            ActivityStatus activityStatus = activityStatusRepository.GetSingle(activityStatusKeys);
            if (activityStatus != null)
            {
                entityPM.ActivityStatusName = activityStatus.Name;
            }

            if (!string.IsNullOrEmpty(entityPOCO.OpportunityId))
            {
                OpportunityRepository oppRep = new OpportunityRepository(entityPOCO.Tenant);
                OpportunityKeys oppKeys = new OpportunityKeys() { Id = entityPOCO.OpportunityId };
                Opportunity opp = oppRep.GetSingle(oppKeys);

                if (opp != null)
                {
                    entityPM.OpportunitySubject = opp.Subject;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.QuoteId))
            {
                QuoteRepository quoteRepository = new QuoteRepository(entityPOCO.Tenant);
                Quote quote = quoteRepository.GetSingleQuote(entityPOCO.QuoteId, entityPOCO.Tenant);
                if (quote != null)
                {
                    entityPM.QuoteNumber = quote.QuoteNumber;
                }
            }

            ActivityPriorityRepository priorityRepository = new ActivityPriorityRepository(entityPOCO.Tenant);
            ActivityPriorityKeys priorityKeys = new ActivityPriorityKeys() { Code = entityPOCO.PriorityCode };
            ActivityPriority priority = priorityRepository.GetSingle(priorityKeys);
            if (priority != null)
            {
                entityPM.PriorityName = priority.Name;
            }

            if (!string.IsNullOrEmpty(entityPOCO.ActivityTypeCode))
            {
                entityPM.ActivityTypePathCode = (entityPOCO.ActivityTypeCode == "CL" && entityPOCO.IsLeftVoiceMail) ? "VM" : entityPOCO.ActivityTypeCode;
            }

            CustomerRepository customerRepository = new CustomerRepository(entityPOCO.Tenant);
            Customer customer = customerRepository.GetSingleCustomer(entityPOCO.CustomerId, entityPOCO.Tenant, false);
            if (customer != null)
            {
                entityPM.CustomerName = customer.Card.EnglishName;
                entityPM.IsCustomerBlockedBusinessUnit = this.GetIsBlockedBusinessUnit(customer);
            }

            //entityPM.Field1 = new CustomFieldClass("Field1", "Activity", entityPOCO.Field1);
            //entityPM.Field2 = new CustomFieldClass("Field2", "Activity", entityPOCO.Field2);
            //entityPM.Field3 = new CustomFieldClass("Field3", "Activity", entityPOCO.Field3);
            //entityPM.Field4 = new CustomFieldClass("Field4", "Activity", entityPOCO.Field4);
            //entityPM.Field5 = new CustomFieldClass("Field5", "Activity", entityPOCO.Field5);
            //entityPM.Field6 = new CustomFieldClass("Field6", "Activity", entityPOCO.Field6);
            //entityPM.Field7 = new CustomFieldClass("Field7", "Activity", entityPOCO.Field7);
            //entityPM.Field8 = new CustomFieldClass("Field8", "Activity", entityPOCO.Field8);
            //entityPM.Field9 = new CustomFieldClass("Field9", "Activity", entityPOCO.Field9);
            //entityPM.Field10 = new CustomFieldClass("Field10", "Activity", entityPOCO.Field10);

            if (!string.IsNullOrEmpty(entityPOCO.OwnerId))
            {
                UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
                User user = userRepository.GetSingleUser(entityPOCO.OwnerId, entityPOCO.Tenant, false);
                if (user != null)
                {
                    entityPM.OwnerName = user.Contact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.SenderContactId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPOCO.Tenant);
                Contact contact = contactRepository.GetSingleContact(entityPM.SenderContactId, entityPM.Tenant);
                if (contact != null)
                {
                    entityPM.SenderContactName = contact.EnglishName;
                }
            }

            DateTime? myResult = entityPM.UpdateDate;
            if (!entityPM.IsOpen)
            {
                if (entityPM.ActivityStatusCode == "C")
                {
                    myResult = entityPM.CompleteDate;
                }
            }

            entityPM.ArchiveDate = myResult;
        }

        private void BuildSearchFields(ActivityPM entityPM, Activity entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Description);

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }

        public bool GetIsBlockedBusinessUnit(Customer myCustomer)
        {
            bool isBlockedBusinessUnit = false;

            int myCurrentTenant = myCustomer.Tenant;

            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();

            UserRepository userRepository = new UserRepository(myCurrentTenant);
            User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, myCurrentTenant, true);

            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles(myCurrentTenant, loggedUserEmail, loggedUser.Id);

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        if (myCustomer.SalesmanUserId == null)
                        {
                            isBlockedBusinessUnit = false;
                        }

                        else if (myCustomer.SalesmanUserId != loggedUser.Id)
                        {
                            isBlockedBusinessUnit = true;
                        }
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                    {
                        if (myCustomer.SalesmanUserId == null)
                        {
                            isBlockedBusinessUnit = false;
                        }

                        else if (myCustomer.SalesmanUser.BusinessUnitId != loggedUser.BusinessUnitId)
                        {
                            isBlockedBusinessUnit = true;
                        }
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        if (myCustomer.SalesmanUserId == null)
                        {
                            isBlockedBusinessUnit = false;
                        }

                        if (!myCustomer.SalesmanUser.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId))
                        {
                            isBlockedBusinessUnit = true;
                        }
                    }
                }
            }

            return isBlockedBusinessUnit;
        }

        private List<RoleFeature> GetFeaturesRoles(int myCurrentTenant, string loggedUserEmail, string loggedUserId)
        {
            List<RoleFeature> myFeatureRoles = new List<RoleFeature>();

            if (loggedUserEmail != "admin@fnarsoft.com")
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myCurrentTenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Customer", 0, true);

                FeatureRepository featureRepository = new FeatureRepository(myCurrentTenant);
                Feature myFeature = featureRepository.GetSingleFeatureByCode(objectTable.Id, "READ", myCurrentTenant);
                if (myFeature != null)
                {
                    RoleRepository roleRepository = new RoleRepository(myCurrentTenant);
                    RoleFeatureRepository roleFeatureRepository = new RoleFeatureRepository(myCurrentTenant);
                    List<string> myRolesIds = roleRepository.GetUserRolesIds(loggedUserId, myCurrentTenant);

                    if (myFeature.IsBusinessUnitEnabled)
                    {
                        foreach (string myRoleId in myRolesIds)
                        {
                            RoleFeature myRoleFeature = roleFeatureRepository.GetBusinessUnitFilterRoleFeature(myRoleId, myFeature.Id, myFeature.Tenant);
                            if (myRoleFeature != null)
                            {
                                myFeatureRoles.Add(myRoleFeature);
                            }
                        }
                    }
                }
            }

            return myFeatureRoles;
        }
    }
}
   