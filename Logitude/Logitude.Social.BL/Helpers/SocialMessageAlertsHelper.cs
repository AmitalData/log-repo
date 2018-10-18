using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Social.BL.Helpers
{
    public class SocialMessageAlertsHelper
    {



       
        public void SendEmailAlert(ConversationHeaderMessagePM entityPM)
        {

            EmailAlertSettingRepository emailAlertSettingRepository = new EmailAlertSettingRepository(entityPM.Tenant);
            EmailAlertSetting alertSettings = emailAlertSettingRepository.GetSingleEmailAlertSettingByCodeAndTenant("OMER", entityPM.Tenant);

            if (alertSettings != null && alertSettings.InActive == false)
            {
                ISocialContext socialContext = SocialContext.GetContext(entityPM.Tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
                ContactRepository contactRepository = new ContactRepository(commonContext);
                ConversationHeaderParticipantRepository conversationHeaderParticipantRepository = new ConversationHeaderParticipantRepository(socialContext);
                List<ConversationHeaderParticipantList> ParticipantLists = conversationHeaderParticipantRepository.GetAllParticipantForHeader(entityPM.ConversationHeaderId, entityPM.Tenant).ToList();

                ConversationHeaderRepository conversationHeaderRepository = new ConversationHeaderRepository(socialContext);
      
            

                ConversationHeader conversationHeader = conversationHeaderRepository.GetConversationHeaderbyId(entityPM.ConversationHeaderId);

                string body = "";
                if (!string.IsNullOrEmpty(conversationHeader.EntityId))
                {
                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(conversationHeader.Tenant);
                    ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(conversationHeader.ObjectTableId, conversationHeader.Tenant, true);

                    if (objectTable != null)
                    {
                        if (objectTable.Name == "Opportunity")
                        {
                            OpportunityRepository opportunityRepository = new OpportunityRepository(entityPM.Tenant);
                            Opportunity opportunity = opportunityRepository.GetSingle(conversationHeader.EntityId, entityPM.Tenant);
                            if (opportunity != null)
                            {
                                CustomerRepository customerRepository = new CustomerRepository(entityPM.Tenant);
                                Customer customer = customerRepository.GetSingleCustomer(opportunity.CustomerId, opportunity.Tenant, true);

                                body = "Opportunity: " + opportunity.Subject + " for " + (customer != null ? (customer.Card != null ? customer.Card.EnglishName : "") : "");
                            }

                        }
                        else if (objectTable.Name == "Customer")
                        {

                            CustomerRepository customerRepository = new CustomerRepository(entityPM.Tenant);
                            Customer customer = customerRepository.GetSingleCustomer(conversationHeader.EntityId, conversationHeader.Tenant, true);
                            body = "Customer: " + customer != null ? customer.Card != null ? customer.Card.EnglishName : "" : "";
                        }

                        else if (objectTable.Name == "Quote")
                        {
                            QuoteRepository quoteRepository = new QuoteRepository(entityPM.Tenant);
                            Quote quote = quoteRepository.GetSingleQuote(conversationHeader.EntityId, conversationHeader.Tenant);

                            if (quote != null)
                            {
                                body = "Quote: " + quote.Subject + " [ " + quote.QuoteNumber + " ]";
                            }

                        }
                    }

                }


                foreach (ConversationHeaderParticipantList Participant in ParticipantLists)
                {
                    EmailAlersManager emailAlertManager = new EmailAlersManager();
                    if (Participant.ParticipantUserId != entityPM.CreatedByUserId)
                    {
                        string email = "";
                        string englishName = "";
                        Tenant tenantpm = TenantRepository.GetSingleTenant(entityPM.Tenant, true);
                        Contact contact = contactRepository.GetSingleContact(Participant.ParticipantUserId , entityPM.Tenant);


                        if (contact != null)
                        {
                            email = contact.Email;
                            englishName = contact.EnglishName;
                        }
                        string subject = "New Message Received From " + entityPM.UserName + " ( " + tenantpm.Company +" " +tenantpm.Id + " )";
                      
                        string message = entityPM.MessageBody;
                  
                        emailAlertManager.HtmlTemplate.Append(" A new message was received from " + entityPM.UserName + " at " + entityPM.CreateDate);
                        emailAlertManager.HtmlTemplate.Append("<br />");
                        emailAlertManager.HtmlTemplate.Append("regarding:");
                        emailAlertManager.HtmlTemplate.Append("<br /><br />");
                 
                        //if (!string.IsNullOrEmpty(entityPM.RegardingEntity))
                        //{
                        //    emailAlertManager.HtmlTemplate.Append(entityPM.RegardingEntity);
                        //    emailAlertManager.HtmlTemplate.Append("<br /><br />");
                        //}

                        emailAlertManager.HtmlTemplate.Append(body);
                        emailAlertManager.HtmlTemplate.Append("<br /><br />");

                        emailAlertManager.HtmlTemplate.Append(message);


                        if (!string.IsNullOrEmpty(email))
                        {
                            emailAlertManager.SendEmailAlert(entityPM.Tenant, email, subject,"For more details please login");
                        }

                    }

                }
            }
        }
    }
}
