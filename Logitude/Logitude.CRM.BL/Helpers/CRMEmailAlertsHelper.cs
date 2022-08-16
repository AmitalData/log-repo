using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.CRM.BL.Helpers
{
    public class CRMEmailAlertsHelper
    {
        public void SendEmailAlert(object entityPM, object entityPoco, string objectTableName, int tenant, string alertCode, bool isNew)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ICRMContext crmContext = CRMContext.GetContext(tenant);

            ContactRepository contactRepository = new ContactRepository(commonContext);
            CustomerRepository customerRepository = new CustomerRepository(commonContext);
            EmailAlertSettingRepository emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
            ActivityTypeRepository activityTypeRepository = new ActivityTypeRepository(crmContext);
            OpportunityClosingReasonRepository closeReasonRepository = new OpportunityClosingReasonRepository(crmContext);
            OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
            CompetitorRepository competitorRepository = new CompetitorRepository(commonContext);
            EmailAlertSetting alertSettings = emailAlertSettingRepository.GetSingleEmailAlertSettingByCodeObjectTableName(alertCode, objectTableName, tenant);

            if (alertSettings != null)
            {
                if (alertSettings.InActive == false)
                {                     
                    OpportunityPM opportunityPM = null;
                    ActivityPM activityPM = null;
                    Opportunity opportunityPoco = null;
                    Activity activityPoco = null;
                    Customer customer = null;
                    ActivityType activityType = null;
                    Contact createdByContact = null;
                    Contact updatedByContact = null;
                    Opportunity activityOpportunity = null;
                    Competitor competitor = null;
                    OpportunityClosingReason closingReason = null;
                    
                    if (objectTableName == "Opportunity")
                    {
                        opportunityPM = entityPM as OpportunityPM;
                        opportunityPoco = entityPoco as Opportunity;

                        customer = customerRepository.GetSingleCustomer(opportunityPM.CustomerId, tenant, true);
                        createdByContact = contactRepository.GetSingleContact(opportunityPM.CreatedByUserId, tenant);
                        //if (createdByContact.Tenant == 0 && tenant != 0)
                        //{
                        //    createdByContact = contactRepository.GetContactsByEmail("system@tenant" + tenant + ".com").FirstOrDefault();
                        //}

                        updatedByContact = contactRepository.GetSingleContact(opportunityPM.UpdatedByUserId, tenant);


                        //if (updatedByContact.Tenant == 0 && tenant != 0)
                        //{
                        //    updatedByContact = contactRepository.GetContactsByEmail("system@tenant" + tenant + ".com").FirstOrDefault();
                        //}
                        if (!string.IsNullOrEmpty(opportunityPM.ClosedToCompetitorId))
                        {
                            competitor = competitorRepository.GetSingleCompetitor(opportunityPM.ClosedToCompetitorId, tenant);
                        }

                        if (!string.IsNullOrEmpty(opportunityPM.ClosingReasonId))
                        {
                            closingReason = closeReasonRepository.GetSingle(opportunityPM.ClosingReasonId, tenant);
                        }
                    }

                    else if (objectTableName == "Activity")
                    {
                        activityPM = entityPM as ActivityPM;
                        activityPoco = entityPoco as Activity;

                        createdByContact = contactRepository.GetSingleContact(activityPM.CreatedByUserId, tenant);
                        //if (createdByContact.Tenant == 0 && tenant != 0)
                        //{
                        //    createdByContact = contactRepository.GetContactsByEmail("system@tenant" + tenant + ".com").FirstOrDefault();
                        //}

                        updatedByContact = contactRepository.GetSingleContact(activityPM.UpdatedByUserId, tenant);

                        //if (updatedByContact.Tenant == 0 && tenant != 0)
                        //{
                        //    updatedByContact = contactRepository.GetContactsByEmail("system@tenant" + tenant + ".com").FirstOrDefault();
                        //}

                        customer = customerRepository.GetSingleCustomer(activityPM.CustomerId, tenant, true);
                        activityType = activityTypeRepository.GetSingle(activityPM.ActivityTypeCode);

                        if (!string.IsNullOrEmpty(activityPM.OpportunityId))
                        {
                            activityOpportunity = opportunityRepository.GetSingle(activityPM.OpportunityId, tenant);
                        }
                    }

                    string toContactId = (objectTableName == "Opportunity" ? opportunityPM.OwnerId : activityPM.OwnerId);
                    Contact ownerContact = contactRepository.GetSingleContact(toContactId, tenant);
                    //if (ownerContact.Tenant == 0 && tenant != 0)
                    //{
                    //    ownerContact = contactRepository.GetContactsByEmail("system@tenant" + tenant + ".com").FirstOrDefault();
                    //}

                    string subject = "";
                    string toEmails = "";
                
                    EmailAlersManager emailAlertManager = new EmailAlersManager();
                    switch (alertCode)
                    {
                        case "OOPA": // Opportunity Assign
                        case "GNOP":
                            {
                                subject = (isNew ? "New " : "") + "Opportunity for " + customer.Card.EnglishName + " assigned to " + (alertCode == "OOPA" ? "you" : ownerContact.EnglishName);

                                emailAlertManager.HtmlTemplate.Append((isNew ? "A New " : "An ") + "Opportunity was assigned to " + (alertCode == "OOPA" ? "you" : ownerContact.EnglishName) + " with the following details:");
                                emailAlertManager.HtmlTemplate.Append("<br /><br />");

                                emailAlertManager.TableRows.Add("Customer", customer.Card.EnglishName);
                                emailAlertManager.TableRows.Add("Subject", opportunityPM.Subject);
                                emailAlertManager.TableRows.Add("Notes", opportunityPM.Notes);
                                emailAlertManager.TableRows.Add("Est. Closing Date", (opportunityPM.EstimatedClosingDate != null ? opportunityPM.EstimatedClosingDate.Value.ToShortDateString() : ""));

                                if (isNew || alertCode == "GNOP")
                                {
                                    emailAlertManager.TableRows.Add("Created By", createdByContact?.EnglishName);
                                }

                                break;
                            }

                        case "OOPS": // owner Opportunity Stage Update
                        case "GOPS": // general 
                            {
                                StageQueryService stageQueryService = new StageQueryService(crmContext);
                                StagePM oldStage = stageQueryService.GetSingle(opportunityPoco.StageId, false, false);
                                StagePM newStage = stageQueryService.GetSingle(opportunityPM.StageId, false, false);

                                subject = "Opportunity Stage for " + customer.Card.EnglishName + " changed to " + newStage.Name;

                                emailAlertManager.HtmlTemplate.Append("Please be informed that Stage of Opportunity : " + opportunityPM.Subject + " for " + (alertCode == "OOPA" ? "you" : ownerContact.EnglishName) + " was updated  with the following details : ");
                                emailAlertManager.HtmlTemplate.Append("<br /><br />");

                                emailAlertManager.TableRows.Add("From Stage ", oldStage.Name);
                                emailAlertManager.TableRows.Add("To Stage ", newStage.Name);
                                emailAlertManager.TableRows.Add("Owner", ownerContact.EnglishName);
                                emailAlertManager.TableRows.Add("Due Date", (opportunityPM.StageDueDate != null ? opportunityPM.StageDueDate.ToString() : ""));
                                emailAlertManager.TableRows.Add("Updated By", updatedByContact.EnglishName);

                                break;
                            }

                        case "GOCW": // Opportunity Close won
                        case "GOCL":// Opportunity Close lost
                            {
                                subject = "Opportunity for " + customer.Card.EnglishName + " closed as " + (alertCode == "GOCW" ? "won" : "lost");//opportunityPM.Subject + " (" + customer.Card.Code + ")";

                                emailAlertManager.HtmlTemplate.Append("Opportunity for Customer " + customer.Card.EnglishName + " with the following details closed as " + (alertCode == "GOCW" ? "Won" : "Lost") + ":");
                                emailAlertManager.HtmlTemplate.Append("<br /><br />");

                                emailAlertManager.TableRows.Add("Subject", opportunityPM.Subject);
                                emailAlertManager.TableRows.Add("Owner", ownerContact.EnglishName);
                                emailAlertManager.TableRows.Add("Due Date", (opportunityPM.StageDueDate != null ? opportunityPM.StageDueDate.ToString() : ""));
                                emailAlertManager.TableRows.Add("Updated By", updatedByContact.EnglishName);



                                if (tenant == LogitudeSettings.LogitudeCRMTenantNumber)
                                    emailAlertManager.TableRows.Add("No of Users", (opportunityPoco.NumberOfShipments != null ? opportunityPoco.NumberOfShipments.Value.ToString() : ""));
                                else
                                    emailAlertManager.TableRows.Add("No of Shipments", (opportunityPoco.NumberOfShipments != null ? opportunityPoco.NumberOfShipments.Value.ToString() : ""));



                                break;
                            }

                        case "OACA": //activity assign
                        case "OACT":
                        case "OACP":
                        case "GANA": //new activity
                        case "GANT":
                        case "GANP":
                            {
                                subject = (isNew ? "New " : "") + activityType.Name + (customer != null ? " for " + customer.Card.EnglishName : "");
                                emailAlertManager.HtmlTemplate.Append((isNew ? "New " : "") + activityType.Name + " was opened by " + createdByContact?.EnglishName + " and assigned to " + ownerContact.EnglishName + " with the following details:");
                                emailAlertManager.HtmlTemplate.Append("<br /><br />");

                                emailAlertManager.TableRows.Add("Subject", activityPM.Subject);
                                emailAlertManager.TableRows.Add("Notes", activityPM.Notes);
                                if (activityPM.ActivityTypeCode == "TS" || activityPM.ActivityTypeCode == "CL")
                                {
                                    emailAlertManager.TableRows.Add("Due Date", (activityPM.DueDate != null ? activityPM.DueDate.ToString() : ""));
                                }
                                else if (activityPM.ActivityTypeCode == "AP")
                                {
                                    emailAlertManager.TableRows.Add("Start Date", (activityPM.StartDateTime != null ? activityPM.StartDateTime.ToString() : ""));
                                }

                                if (!string.IsNullOrEmpty(activityPM.MeetingSummary))
                                {
                                    emailAlertManager.TableRows.Add("Meeting Summary", activityPM.MeetingSummary);
                                }

                                if (activityType.Code == "CL" && !string.IsNullOrEmpty(activityPM.Description))
                                {
                                    emailAlertManager.TableRows.Add("Description", activityPM.Description);
                                }

                                break;
                            }

                        case "GAAC":
                        case "GATC":
                        case "GAPC":
                            {
                                subject = activityType.Name + (customer != null ? " for " + customer.Card.EnglishName : "") + " closed";

                                emailAlertManager.HtmlTemplate.Append(activityType.Name + " with the following details was closed by " + updatedByContact.EnglishName + ":");
                                emailAlertManager.HtmlTemplate.Append("<br /><br />");

                                emailAlertManager.TableRows.Add("Subject", activityPM.Subject);
                                emailAlertManager.TableRows.Add("Notes", activityPM.Notes);
                                emailAlertManager.TableRows.Add("Complete date & time", (activityPM.CompleteDate != null ? activityPM.CompleteDate.ToString() : ""));
                                emailAlertManager.TableRows.Add("Meeting Summary", activityPM.MeetingSummary);

                                if (activityType.Code == "CL" && !string.IsNullOrEmpty(activityPM.Description))
                                {
                                    emailAlertManager.TableRows.Add("Description", activityPM.Description);
                                }

                                break;
                            }
                    }                    

                    if (alertSettings.SettingLevelCode == "OWNR")
                    {
                        toEmails = ownerContact.Email;
                    }

                    else
                    {
                        toEmails = alertSettings.To;
                    }

                    emailAlertManager.SendEmailAlert(tenant, toEmails, subject);
                }
            }
        }
    }
}
