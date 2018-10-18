using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EmailAlerts
{
    public class QuoteEmailAlert
    {
        public void SendEmailAlert(QuotePM entityPM, Quote entityPoco, int tenant, string alertCode, bool isNew)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IQuotesContext quoteontext = QuotesContext.GetContext(tenant);

            ContactRepository contactRepository = new ContactRepository(commonContext);
            CardRepository cardRepository = new CardRepository(commonContext);
            EmailAlertSettingRepository emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);

            EmailAlertSetting alertSettings = emailAlertSettingRepository.GetSingleEmailAlertSettingByCodeObjectTableName(alertCode, "Quote", tenant);
            
            if (alertSettings != null)
            {
                if (alertSettings.InActive == false)
                {
                    string customerName = cardRepository.GetEnglishNameCardById(entityPM.CustomerId, tenant);
                    string createdByContactName = contactRepository.GetContactNameById(entityPM.CreatedByUserId, tenant);
                    Contact owner = contactRepository.GetSingleContact(entityPM.SalesmanUserId, tenant);

                    string ownerName = string.Empty;
                    string ownerEmails = string.Empty;

                    if (owner != null)
                    {
                        ownerName = owner.EnglishName;
                        ownerEmails = owner.Email;
                    }


                    string subject = "";
                    string toEmails = "";
                    EmailAlersManager emailAlertManager = new EmailAlersManager();
                
                    switch (alertCode)
                    {
                        case "OQTA": // Quote Assign
                        case "GNQT":// New Quote 
                            // : New Quote for [customer name] assigned to [owner name] [quote No]
                            subject = (isNew ? "New " : "") + "Quote for " + customerName + " assigned to " + ownerName + " " + entityPM.QuoteNumber;

                            emailAlertManager.HtmlTemplate.Append((isNew ? "New " : "") + "Quote was opened by " + createdByContactName + " and assigned to " + ownerName + " with the following details:");
                            emailAlertManager.HtmlTemplate.Append("<br /><br />");
                           
                            emailAlertManager.TableRows.Add("Subject", entityPM.Subject);
                            emailAlertManager.TableRows.Add("Customer", customerName);
                            emailAlertManager.TableRows.Add("Notes", entityPM.Notes);


                            break;

                        case "GQTA"://  Quote Accepted
                        case "GQTD"://  Quote Declined

                            // Quote for [customer name] was Accepted Declined [quote no]
                            subject = "Quote for " + customerName + " was " + (alertCode == "GQTA" ? "Accepted" : "Declined") + " " + entityPM.QuoteNumber;
             

                            emailAlertManager.HtmlTemplate.Append("Quote was " + (alertCode == "GQTA" ? "Accepted" : "Declined") + " with the following details:");
                            emailAlertManager.HtmlTemplate.Append("<br /><br />");
                              
                            emailAlertManager.TableRows.Add("Subject", entityPM.Subject);
                            emailAlertManager.TableRows.Add("Customer", customerName);
                            emailAlertManager.TableRows.Add("Quote Owner", (ownerName));
                            emailAlertManager.TableRows.Add("Number", entityPM.QuoteNumber);
 

                            break;
                    }

                    if (alertSettings.SettingLevelCode == "OWNR")
                    {
                        toEmails = ownerEmails;
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
