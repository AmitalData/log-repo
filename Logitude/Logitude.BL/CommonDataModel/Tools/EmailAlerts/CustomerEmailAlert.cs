using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.Tools.EmailAlerts
{
    public class CustomerEmailAlert
    {
        public void SendEmailAlert(CustomerPM entityPM, int tenant, string alertCode, bool isNew)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            EmailAlertSettingRepository emailAlertSettingRepository = new EmailAlertSettingRepository(tenant);
            EmailAlertSetting alertSettings = emailAlertSettingRepository.GetSingleEmailAlertSettingByCodeObjectTableName(alertCode, "Customer", tenant);
     
            if (alertSettings != null)
            {
                if (alertSettings.InActive == false)
                {
                    Contact createdByContact = contactRepository.GetSingleContact(entityPM.CreatedByUserId, tenant);
                    Contact ownerContact = contactRepository.GetSingleContact(entityPM.SalesmanUserId, tenant);
                    string subject = "";
                    string toEmails = "";

                    EmailAlersManager emailAlertManager = new EmailAlersManager();

                    switch (alertCode)
                    {
                        case "GCAC": // Customer to be activated

                            subject = "Customer " + entityPM.EnglishName + " to be activated " + entityPM.Code;

                            emailAlertManager.HtmlTemplate.Append("Customer  with the following details was applied to be activated :");
                            emailAlertManager.HtmlTemplate.Append("<br /><br />");
                            emailAlertManager.TableRows.Add("Customer", entityPM.EnglishName);
                            emailAlertManager.TableRows.Add("VAT#", entityPM.VatNumber);
                            emailAlertManager.TableRows.Add("Owner", (ownerContact != null ? ownerContact.EnglishName : ""));
                             

                            break;

                        case "GCFS"://  Customer first shipment
                        case "GCFI"://  Customer first invoice
                            string action = (alertCode == "GCFS" ? "shipment opened" : "invoice issued");
                            subject = "First " + action + " for " + entityPM.EnglishName;
                      
                            emailAlertManager.HtmlTemplate.Append(subject);
                           
                            

                            break;
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
