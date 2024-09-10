using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.BL.Validators;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AutoSignUpWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AutoSignUpWcfService.svc or AutoSignUpWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AutoSignUpWcfService : IAutoSignUpWcfService
    {

        public Response Insert(DataContracts.AutoSignUpData entity, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                 //SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                int crmTenant = LogitudeSettings.LogitudeCRMTenantNumber;
                ICommonDataContext objectContext = CommonDataContext.GetContext(crmTenant);
                ContactRepository contactRepository = new ContactRepository(objectContext);
                Contact contact = contactRepository.GetSingleContactByEmail(entity.Email, crmTenant);

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (string.IsNullOrEmpty(entity.Email))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Email property is required";
                        return response;
                    }

                   
                    if (contact == null)
                    {
                        LogitudeLeadRepository leadRepository = new LogitudeLeadRepository();
                        LogitudeLead Lead = new LogitudeLead()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = entity.Email,
                            CreateDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now,
                            ContactName = entity.ContactName,
                            PhoneNumber = entity.PhoneNumber,
                            CompanyName = entity.CompanyName,
                            Comments = entity.Comments,
                            Country = entity.Country,
                            StatusCode = "InProgress",
                            NumberOfBranches = entity.NumberOfBranches,
                            NumberOfUsers = entity.NumberOfUsers,
                            IsEmailVerified = false,
                            IsSentToCustomer = false,
                            RequestType = "DemoTenant",

                        };


                        leadRepository.Add(Lead);
                        leadRepository.SubmitChanges();
                        response.Result = Lead.Id;
                    }
                    else
                    {
                         
                        string subject = "The contact received is already found in the CRM tenant";
                        string fromEmail = SettingUtil.Emails.FromNoReply;
                        string toEmails = SettingUtil.Emails.CrmManagers;
                        
                        StringBuilder HtmlTemplate = new StringBuilder();
                        HtmlTemplate.Append("<div style='text-align:left;font-family:Verdana;font-weight:bold;font-size:14px'>The contact received is already found in the CRM tenant:</div>");
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("Email: ").Append(entity.Email);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("ContactName: ").Append(entity.ContactName);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("CompanyName: ").Append(entity.CompanyName);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("Country: ").Append(entity.Country);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("PhoneNumber: ").Append(entity.PhoneNumber);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("NumberOfUsers: ").Append(entity.NumberOfUsers);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("NumberOfBranches: ").Append(entity.NumberOfBranches);
                        HtmlTemplate.Append("</br>");
                        HtmlTemplate.Append("Comments: ").Append(entity.Comments);

                        
                        EmailCommunicationParams emailParams = new EmailCommunicationParams()
                        {
                            Subject = subject,
                            From = fromEmail,
                            To = toEmails,
                            EmailBody = HtmlTemplate.ToString(),
                            Tenant = crmTenant,
                            LoggingUserId = contact.Id,
                            IsBodySecured = false,
                        };

                        Communications.AddEmailCommunicationLogQueue(emailParams, crmTenant);


                        response.HasError = true;
                        response.ErrorMessage = subject;
                       
                    }

                    scope.Complete();

                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }
    }
}
