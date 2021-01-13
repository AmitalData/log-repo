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
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Data.Helpers;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.Data;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Text;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.CRM.BL.EntityPMs;
using Logitude.Server.Tools.StorageService;
using System.Web;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.GlobalModel.EntityPMs;

namespace CommunicationWorkerRole
{
    public class LeadsWrokerRole : WorkerEntryPoint
    {
        private LogitudeLeadRepository leadRepository = null;

        public override void Run()
        {


            while (IsRunning)
            {

                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService(queueName, 0);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;

                        if (response.MessageId != null)
                        {

                            string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
                            ICommonDataContext context = CommonDataContext.GetContext(tenant);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
                            CommunicationLog commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

                            bool processEnebled = true;

                            if (processEnebled)
                            {
                                if (commLog != null)
                                {
                                    if (commLog.CommunicationStatusTypeCode == "D")
                                    {
                                        queueservice.Complete();
                                    }
                                    else
                                    {
                                        try
                                        {
                                            ProcessLogitudeLead(context, commLog);
                                            queueservice.Complete();
                                            LogDoneItemInMemory();
                                        }
                                        catch (Exception exc)
                                        {

                                            commLog.Retries++;
                                            commLog.ExceptionMessage = GetExceptionMessage(exc);
                                            if (response.RetryNumber <= 1)
                                            {
                                                queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                            }

                                            if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                            {
                                                queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                            }
                                            if (response.RetryNumber >= 3)
                                            {
                                                queueservice.CompleteAsFailed();

                                                commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                                                commLog.DoneDateUTC = DateTime.UtcNow;
                                                commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                                                commLog.LastStatusDateUTC = DateTime.UtcNow;
                                                commLog.CommunicationStatusTypeCode = "F";

                                            }
                                            communicationLogRep.Update(commLog);
                                            communicationLogRep.SubmitChanges();
                                        }

                                    }
                                }
                                else
                                {
                                    if (response.RetryNumber <= 1)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                    }

                                    if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                    {
                                        queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                    }
                                    if (response.RetryNumber >= 3)
                                    {
                                        queueservice.Complete();
                                        AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
                                            + ",at utc time:" + DateTime.UtcNow + ",at LogitudeLead worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "LogitudeLead worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private static string GetExceptionMessage(Exception exc)
        {
            string exceptionMessage = exc.Message;
            if (exc.InnerException != null)
            {
                exceptionMessage = exceptionMessage + Environment.NewLine + exc.InnerException;
            }
            if (exc.StackTrace != null)
            {
                exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
            }
            exceptionMessage = StringHelper.TruncateLongString(exceptionMessage, 4000);

            return exceptionMessage;
        }

        private  void ProcessLogitudeLead(ICommonDataContext context, CommunicationLog commLog)
        {
             leadRepository = new LogitudeLeadRepository();
             LogitudeLead lead = GetLogitudeLeads(commLog);
            if (lead != null && lead.StatusCode == "InProgress" && (lead.IsEmailVerified || (lead.IsEmailVerified == false && lead.IsSentToCustomer == false)))
            {
      
                CreateTenant(lead);
            }

            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(context);
            commLog.CommunicationStatusTypeCode = "D";
            commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
            commLog.DoneDateUTC = DateTime.UtcNow;
            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
            commLog.LastStatusDateUTC = DateTime.UtcNow;
            commLogrepository.Update(commLog);
            commLogrepository.SubmitChanges();
        }

        private LogitudeLead GetLogitudeLeads(CommunicationLog commLog)
        {
            LogitudeLead logitudeLead = null;
            string filename = commLog.DocumentId + "." + commLog.Document.Extension;
            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = commLog.Document.Id,
                FolderName = commLog.Document.Folder,
                Extension = commLog.Document.Extension,
                Tenant = commLog.Document.Tenant,
                FileSize = commLog.Document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);
            if (datainByte != null)
            {
                var  logitudeLeadPM = LogitudeXmlSerializer.DeserializeObject<LogitudeLeadPM>(datainByte);
                if (logitudeLeadPM != null)
                {
                    logitudeLead = leadRepository.GetSingleLogitudeLead(logitudeLeadPM.Id);
                }
            }

            return logitudeLead;

        }

        private void CreateTenant(LogitudeLead lead)
        {

                int crmTenant = LogitudeSettings.LogitudeCRMTenantNumber;
                int demoTenant = GetDemoTenant(lead);

                ICommonDataContext commonContext = CommonDataContext.GetContext(crmTenant);

                UserRepository userRepository = new UserRepository(demoTenant);
                UserQuery userQuery = new UserQuery(userRepository);
                BranchRepository branchRepository = new BranchRepository(demoTenant);
                DepartmentRepository departmentRepository = new DepartmentRepository(demoTenant);
                RoleRepository roleRepository = new RoleRepository(demoTenant);


                ICRMContext crmContext = CRMContext.GetContext(crmTenant);

                BranchRepository branchRep = new BranchRepository(commonContext);
                DepartmentRepository departmentRep = new DepartmentRepository(commonContext);
                RoleRepository roleRep = new RoleRepository(commonContext);
                ContactRepository contactRepository = new ContactRepository(commonContext);

                OpportunityUpdateService opportunityService = new OpportunityUpdateService(crmContext);
                OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
                StageRepository stageRepository = new StageRepository(crmContext);
                LeadSourceRepository leadSourceRepository = new LeadSourceRepository(commonContext);
                CountryRepository countryRepository = new CountryRepository(commonContext);
                BusinessUnitRepository businessUnitRepository = new BusinessUnitRepository(commonContext);
                OpportunityTypeRepository opportunityTypeRepository = new OpportunityTypeRepository(crmContext);

                string ownerEmail = "pool@logitudeworld.com";
                if (LogitudeSettings.DeploymentStage == "logboxwe1" || LogitudeSettings.DeploymentStage == "Dev")
                {
                    ownerEmail = "system@tenant" + crmTenant + ".com";
                }

                User ownerUser = userRepository.GetSingleUserByEmail(ownerEmail, crmTenant, false);
                Branch branch = branchRep.GetBranchByName("Main Office", crmTenant);
                Simplog.Data.CommonDataModel.EntityPOCOs.Department department = departmentRep.GetDepartmentByName("Management", crmTenant);
                Simplog.Data.CommonDataModel.EntityPOCOs.Role role = roleRep.GetSingleByCode("ADMN", 0);


                ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);
                ObjectTable table = objectTableRepository.GetObjectTableByName("Opportunity", 0, false);

                string toEmail = lead.Email;

                if (LogitudeSettings.DeploymentStage == "Dev")
                {
                    toEmail = "islam@logitudeworld.com;jalal@logitudeworld.com";
                }



                StringBuilder HtmlTemplate = new StringBuilder();

                if (lead.IsSentToCustomer == false && lead.IsEmailVerified == false)
                {
                    Contact leadContact = contactRepository.GetSingleContactByEmail(lead.Email, crmTenant);
                    if (leadContact != null)
                    {
                        //string emailbody = "<div style='text-align:left;font-family:Verdana;font-weight:bold;font-size:14px'>The contact received is already found in the CRM tenant:</div>" + htmlString;


                        HtmlTemplate.Append("<div style='text-align:left'>");
                        HtmlTemplate.Append("Dear " + leadContact.EnglishName + ", " + (!string.IsNullOrEmpty(lead.CompanyName) && lead.CompanyName != "Unassigned" ? lead.CompanyName : ""));// " (" + lead.Country + ")");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("Thank you for your interest in Logitude World, the first Freight Forwarding solution built in the cloud.");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("We value your inquiry to get new access to our demo environment. Your details are currently available in our database and one of our dedicated team members will get in touch with you shortly.");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("If you have any questions or inquiries please, feel free to contact us at <a href='mailto:info@logitudeworld.com'>info@logitudeworld.com</a>");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("Best Regards,");
                        HtmlTemplate.Append("<br />");
                        HtmlTemplate.Append("<div style='text-align:left;font-weight:bold;color:#1F497D'>The Logitude Team</div>");
                        HtmlTemplate.Append("<a href='http://www.Logitudeworld.com'>www.Logitudeworld.com</a>");
                        HtmlTemplate.Append("<br />");
                        HtmlTemplate.Append("<img width='258' height='101' src='cid:logo0' />");
                        HtmlTemplate.Append("</div>");
                        string emailbody = HtmlTemplate.ToString();

                        EmailCommunicationParams emailParams = new EmailCommunicationParams();
                        if (LogitudeSettings.DeploymentStage == "Simplog")
                        {

                            emailParams = new EmailCommunicationParams()
                            {
                                From = "info@logitudeworld.com",
                                To = leadContact.Email,
                                CC = "info@logitudeworld.com",
                                Subject = "Your inquiry re Logitude World demo",
                                EmailBody = emailbody,
                                Tenant = crmTenant,
                            };
                        }
                        else
                        {


                            emailParams = new EmailCommunicationParams()
                            {
                                From = "admin@fnarsoft.com",
                                To = "islam@logitudeworld.com;jalal@logitudeworld.com",
                                Subject = "Your inquiry re Logitude World demo",
                                EmailBody = emailbody,
                                Tenant = crmTenant,
                            };


                        }


                        Communications.AddEmailCommunicationLogQueue(emailParams, crmTenant);

                        lead.StatusCode = "Completed";
                        leadRepository.Update(lead);
                        leadRepository.SubmitChanges();
                        return;
                    }


                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {

                        Contact customercontact = new Contact()
                        {
                            Id = IdCounter.GetNumber("Contact", crmTenant),
                            Email = TruncateLongString(lead.Email, 70),
                            EnglishName = TruncateLongString(lead.ContactName, 60),
                            BusinessPhone = TruncateLongString(lead.PhoneNumber, 25),
                            Tenant = crmTenant,
                            LocalName = TruncateLongString(lead.ContactName, 60),
                            UserType = "R",
                        };

                        customercontact.ComputedKey = (!string.IsNullOrEmpty(customercontact.Email) ? customercontact.Email : customercontact.Id);
                        contactRepository.Add(customercontact);
                        contactRepository.SubmitChanges();


                        Country country = countryRepository.GetSingleCountryByName(lead.Country, crmTenant);
                        if (country == null)
                        {
                            country = countryRepository.GetSingleCountryByCode("--", crmTenant);

                        }

                        CustomerPM customerPM = new CustomerPM()
                        {
                            Code = CodeCounter.GetNumber("Customer", crmTenant).ToString(),
                            PartnerTypeId = "PO",
                            Tenant = crmTenant,
                            EnglishName = TruncateLongString(lead.CompanyName, 70),
                            LocalName = TruncateLongString(lead.CompanyName, 100),
                            CustomerStatusCode = "POT",
                            IsCustomer = true,
                            PhoneNumber = lead.PhoneNumber,
                            Notes = lead.Country,
                            IsHybrid = true,
                            CountryId_Potential = country.Id,
                            PhoneNumber_Potential = lead.PhoneNumber,
                            City_Potential = "Unassigned",
                            PrimaryContactId = customercontact.Id,
                            VatNumber = lead.VatNumber,
                        };


                        CustomerQuery customerQuery = new CustomerQuery(crmTenant);
                        CustomerList logitudeReseller = customerQuery.GetSingleCustomerListByCode("73946", crmTenant);

                        string resellerId = "Atlas";
                        if (lead.LeadSource == "Aerolineas")
                        {
                            customerPM.Field2 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field2", TableName = "Customer", Value = "Aerolinea" };
                            customerPM.Field4 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field4", TableName = "Customer", Value = lead.IATACode };
                            customerPM.Field5 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field5", TableName = "Customer", Value = lead.CASSCode };

                        }
                        else if (lead.LeadSource == "Atlas")
                        {

                            CustomerList customerList = customerQuery.GetSingleCustomerListByCode("75228", crmTenant);

                            if (customerList != null) resellerId = customerList.Id;
                            customerPM.Field2 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field2", TableName = "Customer", Value = resellerId };
                            customerPM.Field4 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field4", TableName = "Customer", Value = lead.IATACode };
                            customerPM.Field5 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field5", TableName = "Customer", Value = lead.CASSCode };


                            LeadSourceQuery leadSourceQuery = new LeadSourceQuery(crmTenant);
                            LeadSourcePM leadSourcePM = leadSourceQuery.GetLeadSourceByCode("PA", crmTenant);
                            if (leadSourcePM != null) customerPM.LeadSourceId = leadSourcePM.Id;

                            // 
                        }
                        else if (LogitudeSettings.DeploymentStage == "Simplog" && logitudeReseller != null)
                        {
                            customerPM.Field2 = new Simplog.Server.Infrastructure.DataContracts.CustomFieldClass() { FieldName = "Field2", TableName = "Customer", Value = logitudeReseller.Id };
                        }

                        customerPM.Contacts.Add(new ContactPM()
                        {
                            Id = customercontact.Id,
                            Email = customercontact.Email,
                            EnglishName = customercontact.EnglishName,
                            BusinessPhone = customercontact.BusinessPhone,
                            Tenant = customercontact.Tenant,
                            LocalName = customercontact.LocalName,
                            IsCreatedWithPartner = true,
                        });


                        CustomerService service = new CustomerService(commonContext, customerPM, ownerUser.Id);
                        service.Create();
                        CustomerTracing customerTracingClass = new CustomerTracing(customerPM, service.entityPOCO, ownerUser.Id, true);
                        customerTracingClass.Trace();

                        commonContext.SaveChanges();

                        //if (lead.RequestType != "LogBox")
                        //{
                        IQueryable<BusinessUnit> businessUnitList = businessUnitRepository.GetBusinessUnits(crmTenant);
                        BusinessUnit businessUnit = businessUnitList.Where(b => b.Name == "Organization").FirstOrDefault();
                        // BusinessUnit businessUnit = businessUnitRepository.GetBusinessUnits(crmTenant);.Where(b => b.Name == "Organization").FirstOrDefault();

                        LeadSource leadsouce = leadSourceRepository.GetSingleLeadSourceByCode("WB", crmTenant);
                        Stage stage = stageRepository.GetStageByCode("QUA", crmTenant);
                        OpportunityType type = opportunityTypeRepository.GetOpportunityTypeByCode("N", crmTenant);

                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(crmTenant);

                        Opportunity opportunity = new Opportunity()
                        {
                            Id = IdCounter.GetNumber("Opportunity", crmTenant),
                            Tenant = crmTenant,
                            CreatedByUserId = ownerUser.Id,
                            UpdatedByUserId = ownerUser.Id,
                            CreateDate = todayDate,
                            UpdateDate = todayDate,
                            LastStageDate = todayDate,
                            OpportunityTypeId = type.Id,
                            Subject = "New Business",
                            CustomerId = customerPM.Id,
                            OwnerId = ownerUser.Id,
                            RatingCode = "N",
                            StageId = stage.Id,
                            LeadSourceId = leadsouce.Id,
                            Notes = lead.Comments,
                            NumberOfShipments = lead.NumberOfUsers,
                            BusinessUnitId = businessUnit.Id,
                            ConcurrencyGUID = Guid.NewGuid().ToString(),
                            ContactId = customercontact.Id,
                            Probability = stage.Probability,
                        };


                        if (lead.LeadSource == "Atlas")
                        {
                            opportunity.Field1 = resellerId;
                        }
                        else if (LogitudeSettings.DeploymentStage == "Simplog" && logitudeReseller != null)
                        {
                            opportunity.Field1 = logitudeReseller.Id;
                        }

                        BuildOpportunitySearchFields(opportunity);

                        opportunityRepository.Add(opportunity);
                        opportunityRepository.SubmitChanges();
                        lead.OpportunityId = opportunity.Id;
                        // }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = crmTenant,
                            EventTypeCode = "CROP",
                            UserId = ownerUser.Id,
                            EntityId = opportunity.Id,
                            ObjectTableName = "Opportunity",
                            Notes = null
                        });

                        lead.CustomerId = customerPM.Id;
                        scope.Complete();
                    }


                    if (lead.RequestType == "LogBox")
                    {
                        lead.StatusCode = "Completed";
                        leadRepository.Update(lead);
                        leadRepository.SubmitChanges();
                        return;
                    }

                    if (lead.LeadSource == "Aerolineas" || lead.LeadSource == "Atlas")
                    {
                        lead.StatusCode = "Completed";
                    }

                    lead.IsSentToCustomer = true;
                    leadRepository.Update(lead);
                    leadRepository.SubmitChanges();



                    if (lead.LeadSource == "Aerolineas" || lead.LeadSource == "Atlas")
                    {

                        string subject = lead.LeadSource == "Aerolineas" ? "Inquiry from Logitude-Aerolineas for a new environment from " + lead.CompanyName : "Inquiry from Logitude-Atlas for a new environment from " + lead.CompanyName;



                        HtmlTemplate.Append("<div style='text-align:left'>");
                        HtmlTemplate.Append("<Table style='text-align:left'>");

                        HtmlTemplate.Append("<tr><td> Company Name : </td><td>" + lead.CompanyName + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Contact Name : </td><td>" + lead.ContactName + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Phone Number : </td><td>" + lead.PhoneNumber + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> E-mail : </td><td>" + lead.Email + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Country : </td><td>" + lead.Country + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> City : </td><td>" + lead.City + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> State : </td><td>" + lead.State + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Street : </td><td>" + lead.Street + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Create Date : </td><td>" + lead.CreateDate.ToString() + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Number of Branches : </td><td>" + lead.NumberOfBranches + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Number of Users : </td><td>" + lead.NumberOfUsers + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Comments : </td><td>" + lead.Comments + "</td></tr>");
                        // HtmlTemplate.Append("<tr><td> Package Code : </td><td>" + lead.PackageCode + "</td></tr>");
                        // HtmlTemplate.Append("<tr><td> Status Code : </td><td>" + lead.StatusCode + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> Zip Code : </td><td>" + lead.ZipCode + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> IATA Code : </td><td>" + lead.IATACode + "</td></tr>");
                        HtmlTemplate.Append("<tr><td> CASS Code : </td><td>" + lead.CASSCode + "</td></tr>");

                        HtmlTemplate.Append("<Table/>");
                        HtmlTemplate.Append("<div/>");
                        string emailbody = HtmlTemplate.ToString();
                        //string Emails = "info@logitudeworld.com, Leandro.Martinez@aerolineas.com.ar, mcerruti@aerolineas.com.ar  ";
                        string Emails = lead.LeadSource == "Aerolineas" ? "registration@logitudeworld.com;Leandro.Martinez@aerolineas.com.ar;mcerruti@aerolineas.com.ar" : "Mirjam.Schubert@champ.aero;info@logitudeworld.com";

                        EmailParameters parameters = new EmailParameters();
                        parameters = new EmailParameters()
                        {
                            From = "registration@logitudeworld.com",
                            To = Emails,
                            Cc = "",
                            Bcc = "",
                            Subject = subject,
                            Body = emailbody,
                            IsBodyHtml = true,
                            Tenant = demoTenant,
                            EmailView = System.Net.Mime.MediaTypeNames.Text.Html,

                        };

                        SendEmail(crmTenant, ownerUser, lead, table, emailbody, parameters, false);

                        // Aerolineas 
                    }
                    else
                    {

                        #region send email to customer
                        string path = LogitudeSettings.LogitudeURL + "/SignUpVerification.aspx?id=" + lead.Id;
                        HtmlTemplate.Append("<div style='text-align:left'>");
                        HtmlTemplate.Append("Dear " + (!string.IsNullOrEmpty(lead.ContactName) && lead.ContactName != "Unassigned" ? (lead.ContactName + ", ") : ",") + (!string.IsNullOrEmpty(lead.CompanyName) && lead.CompanyName != "Unassigned" ? lead.CompanyName : ""));// " (" + lead.Country + ")");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("Thank You for your interest in Logitude World, the First Freight Forwarding solution built in the cloud.");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("In order to proceed with the demo environment, please confirm your detail using the following link: <a href='" + path + "'/>" + path + "</a>");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("As a result we will send you an email with your user name and password for our demo environment and a Quick Tour Guide.");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("If you have any questions or inquiries please, feel free to contact us at <a href='mailto:info@logitudeworld.com'>info@logitudeworld.com</a>");
                        HtmlTemplate.Append("<br /><br />");
                        HtmlTemplate.Append("Best Regards,");
                        HtmlTemplate.Append("<br />");
                        HtmlTemplate.Append("<div style='text-align:left;font-weight:bold;color:#1F497D'>The Logitude Team</div>");
                        HtmlTemplate.Append("<a href='http://www.Logitudeworld.com'>www.Logitudeworld.com</a>");
                        HtmlTemplate.Append("<br />");
                        HtmlTemplate.Append("<img width='258' height='101' src='cid:logo0' />");
                        HtmlTemplate.Append("</div>");
                        string emailbody = HtmlTemplate.ToString();
                        EmailParameters parameters = new EmailParameters();
                        if (LogitudeSettings.DeploymentStage == "Dev")
                        {
                            parameters = new EmailParameters()
                            {
                                From = "info@logitudeworld.com",
                                To = toEmail,
                                Cc = "",
                                Bcc = "",
                                Subject = "Signup Request Verification",
                                Body = emailbody,
                                IsBodyHtml = true,
                                Tenant = demoTenant,
                                EmailView = System.Net.Mime.MediaTypeNames.Text.Html,

                            };
                        }
                        else
                        {
                            parameters = new EmailParameters()
                            {
                                From = "info@logitudeworld.com",
                                To = toEmail,
                                Cc = "info@logitudeworld.com",
                                Bcc = "jalal@logitudeworld.com",
                                Subject = "Signup Request Verification",
                                Body = emailbody,
                                IsBodyHtml = true,
                                Tenant = demoTenant,
                                EmailView = System.Net.Mime.MediaTypeNames.Text.Html,

                            };
                        }

                        SendEmail(crmTenant, ownerUser, lead, table, emailbody, parameters, false);

                        #endregion
                    }
                }

                else if (lead.IsEmailVerified)
                {

                    if (lead.RequestType == "DemoTenant")
                    {

                        UserPM user = userQuery.GetSingleUserPMByEmail(lead.Email, demoTenant, true);
                        if (user == null)
                        {
                            UserPM createdUser = AddNewUserToDemoTenant(demoTenant, lead, userRepository, branchRepository, departmentRepository, roleRepository);


                            CustomerRepository customerRepository = new CustomerRepository(commonContext);
                            AddressRepository addressRepository = new AddressRepository(commonContext);

                            Opportunity opportunity = opportunityRepository.GetSingle(lead.OpportunityId, LogitudeSettings.LogitudeCRMTenantNumber);
                            if (opportunity != null)
                            {
                                Stage stage = stageRepository.GetStageByCode("UPS", LogitudeSettings.LogitudeCRMTenantNumber);
                                DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(opportunity.Tenant);

                                OpportunityStage opportunityStage = new OpportunityStage()
                                {
                                    Id = IdCounter.GetNumber("OpportunityStage", opportunity.Tenant),
                                    OpportunityId = opportunity.Id,
                                    Tenant = opportunity.Tenant,
                                    FromStageId = opportunity.StageId,
                                    ToStageId = stage.Id,
                                    StartDate = opportunity.LastStageDate,
                                    EndDate = todayDateTime,
                                };

                                opportunity.LastStageDate = todayDateTime;

                                OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(crmContext);
                                opportunityStageRepository.Add(opportunityStage);
                                opportunityStageRepository.SubmitChanges();

                                opportunity.StageId = stage.Id;
                                opportunityRepository.SubmitChanges();
                            }



                            #region send email to customer care
                            SendPasswordEmailToUser(demoTenant, crmTenant, ownerUser, lead, table, HtmlTemplate, createdUser);

                            #endregion

                            lead.IsUserOpened = true;
                            lead.IsUserEmailSent = true;
                            lead.StatusCode = "Completed";

                            leadRepository.Update(lead);
                            leadRepository.SubmitChanges();

                            LogDoneItemInMemory();
                        }
                        else
                        {

                            //if (!lead.IsUserEmailSent)
                            //{
                            //    SendPasswordEmailToUser(demoTenant, crmTenant, ownerUser, lead, table, HtmlTemplate, user);
                            //}

                            //lead.IsUserEmailSent = true;
                            lead.IsUserOpened = true;
                            lead.StatusCode = "Completed";
                            leadRepository.Update(lead);
                            leadRepository.SubmitChanges();
                            LogDoneItemInMemory();
                        }


                    }
                    else
                    {

                    }


                }

            }


        public static UserPM AddNewUserToDemoTenant(int tenant, LogitudeLead lead, UserRepository userRepository, BranchRepository branchRepository, DepartmentRepository departmentRepository, RoleRepository roleRepository)
        {
            //CommonDataDomainService commonDomain = new CommonDataDomainService();



            WebFreightDomainService webFreightdomain = new WebFreightDomainService();
            Branch branch = branchRepository.GetBranchByName("Main Office", tenant);
            if (branch == null)
                branch = branchRepository.context.Branches.FirstOrDefault(b => b.Tenant == tenant);

            Department department = departmentRepository.GetDepartmentByName("Operational", tenant);

            if (department == null)
                department = departmentRepository.context.Departments.FirstOrDefault(d => d.Tenant == tenant);

            UserPM user = new UserPM();
            user.Tenant = tenant;
            user.InternetAccess = true;
            user.Email = lead.Email;

            //user.Id = IdCounter.GetNumber("User").ToString();

            user.EnglishName = lead.ContactName;
            //user.EnglishLastName = txtLstEnglishName.Text;
            user.Birthday = DateTime.Now;
            user.Fax = "";
            user.Mobile = "";
            user.LocalName = lead.ContactName;
            //user.LocalLastName = txtLstEnglishName.Text;
            user.Anniversary = DateTime.Now;
            user.DepartmentId = department.Id;
            user.BranchId = branch.Id;
            user.BusinessPhone = (lead.PhoneNumber != null ? TruncateLongString(lead.PhoneNumber, 25) : lead.PhoneNumber);
            //user.Contact.Id = "11";
            user.SignupRole = true;
            user.SearchFields = lead.Email + "," + lead.ContactName + "," + lead.PhoneNumber;
            user.BusinessUnitId = tenant.ToString();

            user.Notes = lead.CompanyName + " (" + lead.Country + ")";
            //InsertUser(user, userRepository, contactRepository, roleRepository, contactTenantRoleRepository, contactTenantRepository);
            user.ExpirationDate = DateTime.Now.AddDays(7);
            user.CreateDate = DateTime.Now;

            IGlobalContext globalContext = GlobalContext.GetContext();
            if (!globalContext.ContactPasswords.Where(c => c.Email == user.Email).Any())
            {
                user.Password = PasswordGenerator.Generate(8); //"123";
            }
            else
            {
                user.HasPassword = true;
            }

            UserService service = new UserService(userRepository.context, user.Tenant);
            service.Create(user);

            return user;

        }

        private void SendPasswordEmailToUser(int demoTenant, int crmTenant, User ownerUser, LogitudeLead lead, ObjectTable table, StringBuilder HtmlTemplate, UserPM createdUser)
        {

            // HtmlTemplate.Append("<div style='text-align:right;margin-right:150px'>" + lead.CompanyName + " (" + lead.Country + ") </div>");
            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("Dear " + lead.ContactName + " , " + lead.CompanyName + " (" + lead.Country + ") ");
            HtmlTemplate.Append("<br /><br />");
            //HtmlTemplate.Append("Thank You for your interest in Logitude World, the First Freight Forwarding solution built in the cloud.");
            //HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Upon your request,  please find your User Name and Password below:");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("<b>Email: </b>" + lead.Email);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("<b>Password: </b>" + (createdUser.HasPassword ? "Your current password." : createdUser.Password));//+ "  (you will need to change the password on you first login)");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("To log in please, click on the link below (or copy and paste it into your browser):");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append(LogitudeSettings.LogitudeURL);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Please note that the trial period will end in 7 days.");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("The Access is to the demo environment where other customers might be using at the same time.");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Please use the following  link to see our Quick Tour Guide,");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("https://www.logitudeworld.com/wp-content/uploads/2016/10/getting_around.pdf");
            HtmlTemplate.Append("<br /><br />");

            HtmlTemplate.Append("The demo environment provide access to all Logitude modules, including the e-AWB module, which is design to also run as a stand-alone application.");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("Please use the following link for a Quick Tour through e-AWB: ");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("https://www.logitudeworld.com/wp-content/uploads/2016/10/eawb_quicktour.pdf");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("If you want to explore our Logitude World in more detail, we can create a private environment for you. To get this free 30-day trial please, contact me.");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("Best Regards,");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<div style='text-align:left;font-weight:bold;color:#1F497D'>The Logitude Team</div>");
            HtmlTemplate.Append("<a href='http://www.Logitudeworld.com'>www.Logitudeworld.com</a>");
            HtmlTemplate.Append("<br />");
            HtmlTemplate.Append("<img width='258' height='101' src='cid:logo0' />");
            HtmlTemplate.Append("</div>");

            string emailbody = HtmlTemplate.ToString();
            EmailCommunicationParams emailParams = new EmailCommunicationParams();

            if (LogitudeSettings.DeploymentStage == "Simplog")
            {
                emailParams = new EmailCommunicationParams()
                {
                    From = "info@logitudeworld.com",
                    To = lead.Email,
                    CC = "info@logitudeworld.com",
                    Subject = "Username & Password (7-day Trial)",
                    EmailBody = emailbody,
                    Tenant = demoTenant,
                    IsBodySecured = true,
                };
            }
            else
            {

                emailParams = new EmailCommunicationParams()
                {
                    From = "info@logitudeworld.com",
                    To = "islam@logitudeWorld.com;jalal@logitudeworld.com",
                    CC = "",
                    BCC = "",
                    Subject = "Username & Password (7-day Trial)",
                    EmailBody = emailbody,
                    Tenant = demoTenant,
                    IsBodySecured = true,
                };


            }

            Communications.AddEmailCommunicationLogQueue(emailParams, demoTenant);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] messageByte = enc.GetBytes(emailbody);

            string txtMessage = emailbody.Replace("<div style='text-align:left'>", "").Replace("<br />", Environment.NewLine).Replace("<img width='258' height='101' src='cid:logo0' />", "").Replace("<a href='mailto:info@logitudeworld.com'>info@logitudeworld.com</a>", "info@logitudeworld.com")
                .Replace("<a href='http://www.Logitudeworld.com'>www.Logitudeworld.com</a>", "http://www.Logitudeworld.com").Replace(" <a href='", "").Replace("</a>", "").Replace("<div style='text-align:left;font-weight:bold;color:#1F497D'>The Logitude Team</div>", "").Replace("</div>", "")
                .Replace("<b>", "").Replace("</b>", "").Replace("<div style='text-align:right;margin-right:150px'>", "").Replace("<div style='text-align:left;'>", "");
            byte[] messagetxtByte = enc.GetBytes(txtMessage);

            SendHtmlDocumentForOpportunity(messageByte, messagetxtByte, crmTenant, emailParams.To, emailParams.Subject, emailParams.CC, emailParams.BCC, ownerUser.Id, lead.OpportunityId, lead.CustomerId, table.Id, null, null);


        }

        public string SendHtmlDocumentForOpportunity(byte[] htmlData, byte[] textData, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string opportunityId, string customerId, string objectTableId, string attachments, string entityReference)
        {

            string result = null;
            //using (TransactionScope scope = TransactionFactory.GetTransaction())
            //{
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            ICRMContext crmContext = CRMContext.GetContext(tenant);

            DocumentOutRepository documentOutRepository = new DocumentOutRepository(context);
            DocumentRepository documentRep = new DocumentRepository(context);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(context);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(context);
            UserRepository userRepository = new UserRepository(context);
            CustomerRepository customerRepository = new CustomerRepository(context);
            OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ContactRepository contactRepository = new ContactRepository(context);

            ObjectTable table = objectTableRepository.GetSingleObjectTable(objectTableId, 0, false);

            User user = userRepository.GetSingleUser(userId, tenant, false);
            Opportunity opportunity = opportunityRepository.GetSingle(opportunityId, tenant);
            Customer customer = customerRepository.GetSingleCustomer(customerId, tenant, false);

            if (HttpContext.Current != null)
            {
                SecurityUtility.CheckFeatureAccessLevelPermission("Activity", "NEW", userId, user.BusinessUnitId, tenant);
            }

            string eventTypeCode = null;
            if (!string.IsNullOrEmpty(toEmail))
            {
                string entityId = null;
                string messagesubject = null;
                string docTypeCode = null;
                if (table.Name == "Opportunity")
                {
                    eventTypeCode = "OEMO";
                    docTypeCode = "OPPO";
                    entityId = opportunityId;
                    messagesubject = subject;//"[" + opportunity.Subject + " / " + customer.Card.Code + "] " + subject;
                }
                else
                {
                    eventTypeCode = "CEMO";
                    docTypeCode = "CUST";
                    entityId = customerId;
                    messagesubject = subject;//"[" + customer.Card.Code + "] " + subject;
                }

                DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode(docTypeCode, tenant);
                DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(entityId, documentType.Id, tenant);//documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, null, documentType.Id, tenant);

                if (documentout == null)
                {

                    DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentType.Id, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = null, ChildEntityReference = null, DirectionCode = "O" };
                    newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
                    newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                    newDocumentFiling.CreatedByUserId = userId;
                    newDocumentFiling.OwnerId = userId;
                    newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.UpdatedByUserId = userId;
                    newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;

                    documentsFilingRepository.Add(newDocumentFiling);

                    documentout = new DocumentOut() { Id = newDocumentFiling.Id, EmailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId, DocumentTemplateId = documentType.DocumentTypeDefaultReportTemplateId, Tenant = tenant, Issued = false };
                    documentout.Id = newDocumentFiling.Id;

                    documentOutRepository.Add(documentout);
                    documentOutRepository.SubmitChanges();

                }


                if (documentout != null)
                {

                    documentout.Issued = true;
                    documentout.DocumentsFiling.UpdatedByUserId = userId;
                    documentout.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentOutRepository.Update(documentout);
                }



                Document document = new Document()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    Extension = "html",
                    FileSize = Convert.ToInt32(htmlData.Length),
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant).ToString(),
                    Folder = "others",
                };


                documentRep.Add(document);

                context.SaveChanges();

                //string filename = document.Id + ".html";
                //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));


                //using (Stream memstream = blobfile.OpenWrite())
                //{
                //    memstream.Write(htmlData, 0, htmlData.Length);
                //    memstream.Close();

                //}


                string filename = document.Id + ".html";
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = htmlData.Length,

                };
                storageservice.Write(htmlData, fileInfo);


                // Save Html to CommunicationLog

                CommunicationLog log = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    InOut = "O",
                    To = toEmail,
                    CC = cc,
                    BCC = bcc,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    DocumentId = document.Id,
                    CreatedByUserId = userId,
                    EntityId = entityId,
                    ObjectTableId = objectTableId,
                    DocumentOutId = documentout.Id,
                    DocumentsFilingId = null,
                    EntityReference = entityReference,
                    Subject = subject,
                    Tenant = tenant,
                    LastStatusDateUTC = DateTime.UtcNow,
                    CreateDateUTC = DateTime.UtcNow,
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationLogTypeCode = "E",
                    CommunicationStatusTypeCode = "D",


                };



                context.CommunicationLogs.Add(log);

                context.SaveChanges();

                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                string messageText = enc.GetString(textData);
                ActivityPM activityPM = new ActivityPM()
                {
                    Subject = messagesubject,
                    Tenant = tenant,
                    IsOpen = true,
                    ActivityStatusCode = "C",
                    PriorityCode = "02",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CreatedByUserId = user.Id,
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdatedByUserId = user.Id,
                    BranchId = user.BranchId,
                    OwnerId = user.Id,
                    BusinessUnitId = user.BusinessUnitId,
                    ActivityTypeCode = "EO",
                    OpportunityId = opportunityId,
                    CustomerId = customerId,
                    CommunicationLogId = log.Id,
                    Description = messageText,
                    SenderEmail = user.Contact.Email,
                    SenderContactId = user.Id,
                    IsMarkedCompleted = true,
                    SendReceiveDate = TenantServerConfigration.GetCurrentDateTime(tenant),

                };


                ActivityUpdateService service = new ActivityUpdateService(crmContext, new Dictionary<string, IContext>(), tenant);
                activityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                List<Contact> contacts = contactRepository.GetActiveContacts(tenant).ToList();

                if (!string.IsNullOrEmpty(toEmail))
                {
                    string[] toEmails = toEmail.Split(';');
                    foreach (string email in toEmails)
                    {
                        Contact contact = contacts.Where(c => c.Email == email).FirstOrDefault();
                        ActivityEmailRecipientPM emailRecipientPM = new ActivityEmailRecipientPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            ContactId = contact != null ? contact.Id : null,
                            Email = email,
                            SenderContactId = user.Id,
                            RecipientTypeCode = "TO",
                            Tenant = tenant,
                        };

                        activityPM.ActivityEmailRecipients.Add(emailRecipientPM);

                    }
                }

                if (!string.IsNullOrEmpty(cc))
                {
                    string[] ccEmails = cc.Split(';');
                    foreach (string email in ccEmails)
                    {
                        Contact contact = contacts.Where(c => c.Email == email).FirstOrDefault();
                        ActivityEmailRecipientPM emailRecipientPM = new ActivityEmailRecipientPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            ContactId = contact != null ? contact.Id : null,
                            Email = email,
                            SenderContactId = user.Id,
                            RecipientTypeCode = "CC",
                            Tenant = tenant,
                        };

                        activityPM.ActivityEmailRecipients.Add(emailRecipientPM);

                    }

                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    string[] bccEmails = bcc.Split(';');
                    foreach (string email in bccEmails)
                    {
                        Contact contact = contacts.Where(c => c.Email == email).FirstOrDefault();
                        ActivityEmailRecipientPM emailRecipientPM = new ActivityEmailRecipientPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            ContactId = contact != null ? contact.Id : null,
                            Email = email,
                            SenderContactId = user.Id,
                            RecipientTypeCode = "BCC",
                            Tenant = tenant,
                        };

                        activityPM.ActivityEmailRecipients.Add(emailRecipientPM);

                    }
                }


                service.Update(activityPM, true);

                result = document.Id;



                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityId,
                    EventTypeCode = eventTypeCode,
                    Tenant = tenant,
                    Notes = messagesubject,
                    ObjectTableName = table.Name,
                    UserId = userId,
                });

                //    scope.Complete();
                //}


                //[opportunity subject / customer code] message subject
            }

            return result;
        }


        public static string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }

        private int GetDemoTenant(LogitudeLead lead)
        {
            int demoTenant = 65;
            if (lead != null && (lead.Country == "United States of America" || lead.Country == "United States" || lead.Country == "USA" || lead.Country == "US"))
            {
                demoTenant = 2279;
            }

            if (LogitudeSettings.DeploymentStage == "Dev") demoTenant = 1;
            else if (LogitudeSettings.DeploymentStage == "logboxwe1") demoTenant = LogitudeSettings.LogitudeCRMTenantNumber;

            return demoTenant;
        }

        private void SendEmail(int crmTenant, User ownerUser, LogitudeLead lead, ObjectTable table, string emailbody, EmailParameters parameters, bool isBodySecured)
        {
            string path = LogitudeSettings.LogitudeURL + "/SignUpVerification.aspx?id=" + lead.Id;

            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                From = parameters.From,
                To = parameters.To,
                CC = parameters.Cc,
                BCC = parameters.Bcc,
                Subject = parameters.Subject,
                EmailBody = emailbody,
                Tenant = crmTenant,
                IsBodySecured = isBodySecured,
            };
            Communications.AddEmailCommunicationLogQueue(emailParams, crmTenant);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] messageByte = enc.GetBytes(emailbody);

            string txtMessage = emailbody.Replace("<div style='text-align:left'>", "").Replace("<br />", Environment.NewLine).Replace("<img width='258' height='101' src='cid:logo0' />", "").Replace("<a href='mailto:info@logitudeworld.com'>info@logitudeworld.com</a>", "info@logitudeworld.com")
                .Replace("<a href='http://www.Logitudeworld.com'>www.Logitudeworld.com</a>", "http://www.Logitudeworld.com").Replace(" <a href='" + path + "'/>", "")
                .Replace("</a>", "").Replace("<div style='text-align:left;font-weight:bold;color:#1F497D'>The Logitude Team</div>", "").Replace("</div>", "");
            byte[] messagetxtByte = enc.GetBytes(txtMessage);

            SendHtmlDocumentForOpportunity(messageByte, messagetxtByte, crmTenant, parameters.To, parameters.Subject, parameters.Cc, parameters.Bcc, ownerUser.Id, lead.OpportunityId, lead.CustomerId, table.Id, null, null);
        }
        private void BuildOpportunitySearchFields(Opportunity entity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entity.Subject))
            {
                result = string.IsNullOrEmpty(result) ? entity.Subject : result + "," + entity.Subject;
            }

            if (!string.IsNullOrEmpty(entity.CustomerId))
            {
                CardRepository cardRepository = new CardRepository(entity.Tenant);
                Card card = cardRepository.GetSingleCard(entity.CustomerId, entity.Tenant);
                if (card != null)
                {
                    result = string.IsNullOrEmpty(result) ? card.EnglishName : result + "," + card.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entity.ContactId))
            {
                ContactRepository contactRepository = new ContactRepository(entity.Tenant);
                Contact contact = contactRepository.GetSingleContact(entity.ContactId, entity.Tenant);
                if (contact != null)
                {
                    result = string.IsNullOrEmpty(result) ? contact.EnglishName : result + "," + contact.EnglishName;
                }
            }

            entity.SearchFields = result;

        }



        public override bool OnStart()
        {

            ConnectClient();
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "Leads";
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        DbQueueService queueservice;
        string queueName = "LogitudeLeadQueue";
        public void ConnectClient()
        {
            try
            {


                queueservice = new DbQueueService(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
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

    }
}
