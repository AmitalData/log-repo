
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.CRM.BL.Validators;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CRM.BL.EntityDataMappings
{

    public partial class OpportunityDataMapping : IMapping<OpportunityPM, Opportunity>
    {
        public void CustomPMToPOCO(OpportunityPM entityPM, Opportunity entityPOCO)
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
            GetIsClosedLostProperty(entityPM);
        }

        public void CustomPOCOToPM(OpportunityPM entityPM, Opportunity entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.RatingName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StageName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OwnerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StageProbability);
            this.CustomMappedPMProperties.Add(PMPropertyNames.RatingIndexOrder);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactPhone);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LeadSourceName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OpportunityTypeName);

            CustomerRepository customerRepository = new CustomerRepository(entityPOCO.Tenant);
            Customer customer = customerRepository.GetSingleCustomer(entityPOCO.CustomerId, entityPOCO.Tenant, false);
            if (customer != null)
            {
                entityPM.CustomerName = customer.Card.EnglishName;
                entityPM.CustomerExternalId = customer.Card.ReceivablesAccountingCard;
                entityPM.CountryName = customer.Card.CountryName;
                entityPM.IsCustomerBlockedBusinessUnit = this.GetIsBlockedBusinessUnit(customer);

                RankRepository rankRepository = new RankRepository(entityPOCO.Tenant);
                Rank rank = rankRepository.GetSingleRank(customer.RankId, entityPOCO.Tenant);
                if (rank != null)
                {
                    entityPM.CustomerRankCode = rank.Code;
                    entityPM.CustomerRankName = rank.Name;
                }
            }

            StageRepository stageRepository = new StageRepository(entityPOCO.Tenant);
            StageKeys stageKeys = new StageKeys() { Id = entityPOCO.StageId };
            Stage stage = stageRepository.GetSingle(stageKeys);
            if (stage != null)
            {
                entityPM.StageName = stage.Name;
                entityPM.StageMaxDays = stage.MaxDays;
                entityPM.StageProbability = stage.Probability;
            }

            RatingRepository ratingRepository = new RatingRepository(entityPOCO.Tenant);
            RatingKeys ratingKeys = new RatingKeys() { Code = entityPOCO.RatingCode };
            Rating rating = ratingRepository.GetSingle(ratingKeys);
            if (rating != null)
            {
                entityPM.RatingName = rating.Name;
                entityPM.RatingIndexOrder = rating.IndexOrder;
            }

            OpportunityTypeRepository opportunityTypeRepository = new OpportunityTypeRepository(entityPOCO.Tenant);
            OpportunityTypeKeys opportunityTypeKeys = new OpportunityTypeKeys() { Id = entityPOCO.OpportunityTypeId };
            OpportunityType opportunityType = opportunityTypeRepository.GetSingle(opportunityTypeKeys);
            if (opportunityType != null)
            {
                entityPM.OpportunityTypeName = opportunityType.Name;
            }

            UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
            User user = userRepository.GetSingleUser(entityPOCO.OwnerId, entityPOCO.Tenant, false);
            if (user != null)
            {
                entityPM.OwnerName = user.Contact.EnglishName;
            }

            if (!string.IsNullOrEmpty(entityPOCO.ContactId))
            {
                ContactRepository rep = new ContactRepository(entityPOCO.Tenant);
                Contact contact = rep.GetSingleContact(entityPOCO.ContactId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.ContactName = contact.EnglishName;
                    entityPM.ContactPhone = contact.BusinessPhone;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.LastCompletedActivityTypeCode))
            {
                ActivityTypeRepository typeRepository = new ActivityTypeRepository(entityPOCO.Tenant);
                ActivityType type = typeRepository.GetSingle(entityPOCO.LastCompletedActivityTypeCode);
                if (type != null)
                {
                    entityPM.LastCompletedActivityTypeName = type.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.ClosingReasonId))
            {
                OpportunityClosingReasonRepository closingRepository = new OpportunityClosingReasonRepository(entityPOCO.Tenant);
                OpportunityClosingReasonKeys closingKeys = new OpportunityClosingReasonKeys() { Id = entityPOCO.ClosingReasonId };
                OpportunityClosingReason reason = closingRepository.GetSingle(closingKeys);

                if (reason != null)
                {
                    entityPM.ClosingReasonCode = reason.Code;
                    entityPM.IsClosedLost = reason.IsClosedLost;
                }
            }

            LeadSourceRepository sourceRepository = new LeadSourceRepository(entityPOCO.Tenant);
            LeadSource source = sourceRepository.GetSingleLeadSource(entityPM.LeadSourceId, entityPM.Tenant);
            if (source != null)
            {
                entityPM.LeadSourceName = source.Name;
            }


            //entityPM.Field1 = new CustomFieldClass("Field1", "Opportunity", entityPOCO.Field1);
            //entityPM.Field2 = new CustomFieldClass("Field2", "Opportunity", entityPOCO.Field2);
            //entityPM.Field3 = new CustomFieldClass("Field3", "Opportunity", entityPOCO.Field3);
            //entityPM.Field4 = new CustomFieldClass("Field4", "Opportunity", entityPOCO.Field4);
            //entityPM.Field5 = new CustomFieldClass("Field5", "Opportunity", entityPOCO.Field5);
            //entityPM.Field6 = new CustomFieldClass("Field6", "Opportunity", entityPOCO.Field6);
            //entityPM.Field7 = new CustomFieldClass("Field7", "Opportunity", entityPOCO.Field7);
            //entityPM.Field8 = new CustomFieldClass("Field8", "Opportunity", entityPOCO.Field8);
            //entityPM.Field9 = new CustomFieldClass("Field9", "Opportunity", entityPOCO.Field9);
            //entityPM.Field10 = new CustomFieldClass("Field10", "Opportunity", entityPOCO.Field10);
        }

        private void BuildSearchFields(OpportunityPM entityPM, Opportunity entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ContactId))
            {
                Contact myContact = ContactRepository.GetSingleContact(entityPM.ContactId, entityPM.Tenant, true);
                if (myContact != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myContact.Email);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myContact.EnglishName);
                }
            }

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
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

        private void GetIsClosedLostProperty(OpportunityPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.ClosingReasonId))
            {
                OpportunityClosingReasonRepository closingRepository = new OpportunityClosingReasonRepository(entityPM.Tenant);
                OpportunityClosingReasonKeys closingKeys = new OpportunityClosingReasonKeys() { Id = entityPM.ClosingReasonId };
                OpportunityClosingReason reason = closingRepository.GetSingle(closingKeys);

                if (reason != null)
                {
                    entityPM.IsClosedLost = reason.IsClosedLost;
                }
            }
        }
    }
}
   