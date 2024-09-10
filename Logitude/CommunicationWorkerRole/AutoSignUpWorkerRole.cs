using System;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;

using WebFreight.Web.GlobalModel;

using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using Logitude.Server.Tools;

namespace CommunicationWorkerRole
{
    public class AutoSignUpWorkerRole : WorkerEntryPoint
    {

        public override void Run()
        {

            if (!string.IsNullOrEmpty(LogitudeSettings.AutoSignupEmail) && !string.IsNullOrEmpty(LogitudeSettings.AutoSignupPassword))
            {
                //IGlobalContext globalContext = GlobalContext.GetContext();
                LogitudeLeadRepository leadRepository = new LogitudeLeadRepository();
                AutoSignupEmailRepository autosignupRepository = new AutoSignupEmailRepository();
                AutoSignupEmail autosignupEmail = null;

                int sleeptime = 120000;

                while (IsRunning)
                {
                    if (!General.IsUpdating()) 
                    {

                        try
                        {


                            autosignupEmail = autosignupRepository.GetAutoSignupEmailsByStatus("New").FirstOrDefault();
                            LastActivity = DateTime.UtcNow;
                            if (autosignupEmail != null)
                            {
                                string htmlString = autosignupEmail.EmailBody;
                                HtmlStringParsingParams htmlParams = new HtmlStringParsingParams();
                                ParseHtmlString(htmlParams, htmlString);

                                //(!string.IsNullOrEmpty(pops1.MessageSubject) && pops1.MessageSubject.Contains("Inquiry from Logitude site"))

                                if (string.IsNullOrWhiteSpace(htmlParams.Email) || string.IsNullOrEmpty(htmlParams.Email) ||
                                    (!string.IsNullOrEmpty(autosignupEmail.EmailSubject) && (autosignupEmail.EmailSubject.Contains("Inquiry from Logitude site")
                                    || autosignupEmail.EmailSubject.Contains("reseller") || autosignupEmail.EmailSubject.Contains("Logitude Website - New Contact"))))
                                {
                                    autosignupEmail.Status = "Fail";
                                    autosignupRepository.Update(autosignupEmail);
                                    autosignupRepository.SubmitChanges();

                                    Thread.Sleep(sleeptime);

                                    return;
                                }



                                int crmTenant = LogitudeSettings.LogitudeCRMTenantNumber;
                                SettingRepository settingRepository = new SettingRepository();
                                ICommonDataContext objectContext = CommonDataContext.GetContext(crmTenant);
                                ContactRepository contactRepository = new ContactRepository(objectContext);
                                Contact contact = contactRepository.GetSingleContactByEmail(htmlParams.Email, crmTenant);
                                if (contact == null)
                                {
                                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                                    {

                                        int numerofusers = 0;
                                        int numerofbranches = 0;
                                        int.TryParse(htmlParams.NumberOfUsers, out numerofusers);
                                        int.TryParse(htmlParams.NumberOfBranches, out numerofbranches);

                                        LogitudeLead Lead = new LogitudeLead()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Email = htmlParams.Email,
                                            CreateDate = DateTime.Now,
                                            LastUpdateDate = DateTime.Now,
                                            ContactName = TruncateLongString(htmlParams.ContactName, 40),
                                            PhoneNumber = TruncateLongString(htmlParams.Phone, 40),
                                            CompanyName = TruncateLongString(htmlParams.Company, 100),
                                            Comments = TruncateLongString(htmlParams.Comments, 500),
                                            Country = TruncateLongString(htmlParams.Country, 120),
                                            StatusCode = "InProgress",
                                            NumberOfBranches = numerofbranches,
                                            NumberOfUsers = numerofusers,
                                            IsEmailVerified = false,
                                            IsSentToCustomer = false,
                                            RequestType = "DemoTenant",

                                        };

                                        BuildSearchFields(Lead);

                                        leadRepository.Add(Lead);
                                        autosignupEmail.Status = "Done";
                                        autosignupRepository.Update(autosignupEmail);

                                        leadRepository.SubmitChanges();
                                        autosignupRepository.SubmitChanges();


                                        scope.Complete();

                                    }


                                }
                                else
                                {
                                    string emailbody = "<div style='text-align:left;font-family:Verdana;font-weight:bold;font-size:14px'>The contact received is already found in the CRM tenant:</div>" + htmlString;
                                    EmailCommunicationParams emailParams = new EmailCommunicationParams();

                                    emailParams = new EmailCommunicationParams()
                                    {
                                        From = SettingUtil.Emails.FromNoReply,
                                        To = SettingUtil.Emails.CrmManagers ,
                                        Subject = "The contact received is already found in the CRM tenant",
                                        EmailBody = emailbody,
                                        Tenant = crmTenant,
                                        IsBodySecured = true,
                                    };




                                    Communications.AddEmailCommunicationLogQueue(emailParams, crmTenant);

                                    autosignupEmail.Status = "Done";
                                    autosignupRepository.Update(autosignupEmail);
                                    autosignupRepository.SubmitChanges();
                                }
                                LogDoneItemInMemory();
                            }

                            Thread.Sleep(sleeptime);

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


                            ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "AutoSignUpWorkerRole : Run() Method" + Environment.NewLine + Error, null);

                            if (autosignupEmail != null)
                            {
                                if (autosignupEmail.Retries < 5)
                                {
                                    autosignupEmail.Retries++;
                                    autosignupEmail.Status = "New";
                                }
                                else
                                {
                                    autosignupEmail.Status = "Fail";
                                }
                                autosignupRepository.Update(autosignupEmail);
                                autosignupRepository.SubmitChanges();
                            }

                            Thread.Sleep(sleeptime);
                        }
                        catch (Exception e)
                        {

                            ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "AutoSignUpWorkerRole : Run() Method", null);

                            if (autosignupEmail != null)
                            {
                                if (autosignupEmail.Retries < 5)
                                {
                                    autosignupEmail.Retries++;
                                    autosignupEmail.Status = "New";
                                }
                                else
                                {
                                    autosignupEmail.Status = "Fail";
                                }
                                autosignupRepository.Update(autosignupEmail);
                                autosignupRepository.SubmitChanges();
                            }

                            Thread.Sleep(sleeptime);
                        }


                    }
                    else
                    {
                        Thread.Sleep(sleeptime);

                    }

                }
            }
        }

        public string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }


        private void BuildSearchFields(LogitudeLead lead)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(lead.CompanyName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.CompanyName : mySearchFields + "," + lead.CompanyName;
            }

            if (!string.IsNullOrEmpty(lead.Email))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.Email : mySearchFields + "," + lead.Email;
            }

            if (!string.IsNullOrEmpty(lead.ContactName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.ContactName : mySearchFields + "," + lead.ContactName;
            }

            if (!string.IsNullOrEmpty(lead.Comments))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.Comments : mySearchFields + "," + lead.Comments;
            }

            if (!string.IsNullOrEmpty(lead.PhoneNumber))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.PhoneNumber : mySearchFields + "," + lead.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(lead.StatusCode))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.StatusCode : mySearchFields + "," + lead.StatusCode;
            }

            if (!string.IsNullOrEmpty(lead.RequestType))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.RequestType : mySearchFields + "," + lead.RequestType;
            }

            if (!string.IsNullOrEmpty(lead.Country))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? lead.Country : mySearchFields + "," + lead.Country;
            }

            lead.SearchFields = mySearchFields;

        }

        private void ParseHtmlString(HtmlStringParsingParams htmlParams, string htmlString)
        {
            string[] result = htmlString.Split(new string[] { "<td" }, StringSplitOptions.None);

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i].Contains("Company Name"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Company = secondSplit[0];

                }
                if (result[i].Contains("Contact Name"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.ContactName = secondSplit[0];

                }
                if (result[i].Contains("Phone Number"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Phone = secondSplit[0];

                }
                if (result[i].Contains("Country"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Country = secondSplit[0];

                }
                if (result[i].Contains("E-mail"))
                {
                    //class="MsoNormal"><a href="mailto:adrian@sabit.ro">adrian@sabit.ro</a><o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Email = secondSplit[0];

                    htmlParams.Email = !string.IsNullOrEmpty(htmlParams.Email) ? htmlParams.Email.Replace("\r\n", "") : null;

                }
                if (result[i].Contains("Number of Branches"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.NumberOfBranches = secondSplit[0];

                }
                if (result[i].Contains("Number of Users"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.NumberOfUsers = secondSplit[0];

                }

                if (result[i].Contains("Comments"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Comments = secondSplit[0];

                }

            }
        }



        public override bool OnStart()
        {

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "AutoSignUp";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }




        public class HtmlStringParsingParams
        {
            public string Company { get; set; }
            public string Country { get; set; }
            public string ContactName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string NumberOfBranches { get; set; }
            public string NumberOfUsers { get; set; }
            public string Comments { get; set; }

            public bool IsEmailVerified { get; set; }

            public bool IsSentToCustomer { get; set; }
            public string RequestType { get; set; }

            public string StatusCode { get; set; }
            public DateTime? LastUpdateDate { get; set; }

            public DateTime? CreateDate { get; set; }

            public int TenantNumber { get; set; }

        }

    }
}
