
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class JournalDataMapping: IMapping<JournalPM, Journal>
   {

        public void CustomPMToPOCO(JournalPM entityPM, Journal entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ExternalNo);
            if (!String.IsNullOrWhiteSpace(entityPM.ExternalNo))
            {
                entityPOCO.ExternalNo = entityPM.ExternalNo = entityPM.ExternalNo.ToUpper();    
            }
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ExternalSystem);
            if (!String.IsNullOrWhiteSpace(entityPM.ExternalSystem))
            {
                entityPOCO.ExternalSystem = entityPM.ExternalSystem = entityPM.ExternalSystem.ToUpper();
            }
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

        }

        public void CustomPOCOToPM(JournalPM entityPM, Journal entityPOCO)
        {
            IAccountingContext accContext = null;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.JournalNumber);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusCode);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.TypeCode);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.AccountingDate);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.AccountingEntityCode);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreateDate);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);

            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);

            //CustomMappedPOCOProperties.Add(POCOPropertyNames.ApproveDate);
            //CustomMappedPOCOProperties.Add(POCOPropertyNames.ApprovedByUserId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ExternalNo);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.AccountingEntityId);

            

            if (entityPOCO.OriginalJournalId != null)
            {
                accContext= accContext ??AccountingContext.GetContext(entityPOCO.Tenant);
                JournalQueryService journalQueryService = new JournalQueryService(accContext);
                JournalPM parent = journalQueryService.GetSingle(entityPOCO.OriginalJournalId, false, false);
                entityPM.OriginalJournalName = parent.JournalNumber;
               
            }
                   

            if (entityPOCO.AccountingEntityCode != null)
            {
                accContext = accContext ?? AccountingContext.GetContext(entityPOCO.Tenant);
                AccountingEntityQueryService accountingEntityQueryService = new AccountingEntityQueryService(accContext);
                AccountingEntityPM parent = accountingEntityQueryService.GetSingle(entityPOCO.AccountingEntityCode, false, true);
                entityPM.AccountingEntityName = parent.EnglishName;
            }

            if (entityPOCO.TypeCode != null)
            {
                accContext = accContext ?? AccountingContext.GetContext(entityPOCO.Tenant);
                JournalTypeQueryService journalTypeQueryService = new JournalTypeQueryService(accContext);
                JournalTypePM type = journalTypeQueryService.GetSingle(entityPOCO.TypeCode, false, true);
                entityPM.TypeName = type.EnglishName;
            }
            if (entityPOCO.StatusCode != null)
            {
                accContext = accContext ?? AccountingContext.GetContext(entityPOCO.Tenant);
                JournalStatusTypeQueryService journalStatusTypeQueryService = new JournalStatusTypeQueryService(accContext);
                JournalStatusTypePM type = journalStatusTypeQueryService.GetSingle(entityPOCO.StatusCode, false, true);
                entityPM.StatusName = type.EnglishName;

                ContactPM user = GetLoggedContactData(GetLoggedContactEmail(entityPOCO.Tenant), entityPOCO.Tenant);

                if (user != null)
                {
                    entityPM.StatusLocalName = user.DontShowLocal ? type.EnglishName : type.LocalName;
                }
                else
                {
                    entityPM.StatusLocalName = type.EnglishName;
                }

            }

            
            if (entityPOCO.CreatedByUserId != null)
            {
                ContactPM contact = GetLoggedContact(entityPOCO.Tenant);
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
                contact = contact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM();
                if (userContact != null)
                {
                    entityPM.CreatedByUserName = contact.DontShowLocal ? userContact.EnglishName : userContact.LocalName;
                }
            }






            //if (entityPOCO.CreatedByUserId != null)
            //{
            //    UserQueryService chartOfAccountsTypeQueryService = new ChartOfAccountsTypeQueryService(entityPOCO.Tenant);
            //    ChartOfAccountsTypePM type = chartOfAccountsTypeQueryService.GetSingle(entityPOCO.TypeCode, false, true);
            //    entityPM.TypeName = type.EnglishName;
            //}

        }


        private ContactPM GetLoggedContactData(string userEmail, int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(userEmail, tenant);
            return contactPM;
        }

        private string GetLoggedContactEmail(int tenant)
        {
            string email = "";
            if (HttpContext.Current != null)
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                email = "system@tenant" + tenant.ToString() + ".com";
            }

            return email;
        }
        private static void BuildSearchFields(JournalPM entityPM, Journal poco, bool isNewEntity)
        {
            string result = "";

           
            if (!string.IsNullOrEmpty(entityPM.JournalNumber))
            {
                if (!(result.Split(',').Contains(entityPM.JournalNumber)))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.JournalNumber : result + "," + entityPM.JournalNumber;
                }
            }
            
            result = AddRef(result, entityPM.ExternalNo);
            foreach (JournalLinePM item in entityPM.JournalLines)
            {
                if (!string.IsNullOrEmpty(item.Reference1))
                {

                    if (!(result.Split(',').Contains(item.Reference1)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference1 : result + "," + item.Reference1;
                    }

                }

                if (!string.IsNullOrEmpty(item.Reference2))
                {
                    if (!(result.Split(',').Contains(item.Reference2)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference2 : result + "," + item.Reference2;
                    }
                }

                if (!string.IsNullOrEmpty(item.Reference3))
                {
                    if (!(result.Split(',').Contains(item.Reference3)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference3 : result + "," + item.Reference3;
                    }
                }
            }
            
            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }

        private static string AddRef(string result, string myRef)
        {
            if (!string.IsNullOrWhiteSpace(myRef))
            {
                if (!(result.Split(',').Contains(myRef)))
                {
                    result = string.IsNullOrEmpty(result) ? myRef : result + "," + myRef;
                }
            }
            return result;
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        private static ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }


        //if (!string.IsNullOrEmpty(entityPM.))
        //{
        //    result = string.IsNullOrEmpty(result) ? entityPM.CustomFileNo : result + "," + entityPM.CustomFileNo;
        //}

        //ClientRepository clientRepository = new ClientRepository(entityPM.Tenant);
        //ClientKeys clientKeys = new ClientKeys() { Id = entityPM.ImporterId };
        //Client client = clientRepository.GetSingle(clientKeys);
        //if (client != null)
        //{
        //    result = string.IsNullOrEmpty(result) ? client.FullName : result + "," + client.FullName;
        //}

        //if (isNewEntity)
        //{
        //    foreach (JournalLinePM item in entityPM.JournalLines)
        //    {
        //        if (!string.IsNullOrEmpty(item.Reference1))
        //        {
        //            result = string.IsNullOrEmpty(result) ? item.Reference1 : result + "," + item.Reference1;
        //        }

        //        if (!string.IsNullOrEmpty(item.Reference2))
        //        {
        //            result = string.IsNullOrEmpty(result) ? item.Reference2 : result + "," + item.Reference2;
        //        }

        //        if (!string.IsNullOrEmpty(item.Reference3))
        //        {
        //            result = string.IsNullOrEmpty(result) ? item.Reference3 : result + "," + item.Reference3;
        //        }
        //    }
        //}

        //else
        //{
        //JournalLineRepository journalLineRepository = new JournalLineRepository(entityPM.Tenant);
        //JournalKeys entityKeys = new JournalKeys() { Id = entityPM.Id };
        //List<JournalLine> journalLines = journalLineRepository.GetMulti(entityKeys);
        //foreach (JournalLine item in journalLines)
        //{
        //    if (!string.IsNullOrEmpty(item.Reference1))
        //    {
        //        result = string.IsNullOrEmpty(result) ? item.Reference1 : result + "," + item.Reference1;
        //    }

        //    if (!string.IsNullOrEmpty(item.Reference2))
        //    {
        //        result = string.IsNullOrEmpty(result) ? item.Reference2 : result + "," + item.Reference2;
        //    }

        //    if (!string.IsNullOrEmpty(item.Reference3))
        //    {
        //        result = string.IsNullOrEmpty(result) ? item.Reference3 : result + "," + item.Reference3;
        //    }
        //}


        //    foreach (JournalLinePM item in entityPM.JournalLines)
        //    {
        //        if (!string.IsNullOrEmpty(item.Reference1))
        //        {

        //            if  (!(result.Split(',').Contains(item.Reference1)))
        //            { 
        //                 result = string.IsNullOrEmpty(result) ? item.Reference1 : result + "," + item.Reference1;
        //            }

        //        }

        //        if (!string.IsNullOrEmpty(item.Reference2))
        //        {
        //            if (!(result.Split(',').Contains(item.Reference2)))
        //            {
        //                result = string.IsNullOrEmpty(result) ? item.Reference2 : result + "," + item.Reference2;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(item.Reference3))
        //        {
        //            if (!(result.Split(',').Contains(item.Reference3)))
        //            {
        //                result = string.IsNullOrEmpty(result) ? item.Reference3 : result + "," + item.Reference3;
        //            }
        //        }
        //    }


        //}

        //entityPM.SearchFields = result;
        //poco.SearchFields = result;



    }


}
   