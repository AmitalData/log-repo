using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.CRMModel.DomainServices;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using System.Web;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools;

namespace WebFreight.Web.Helpers
{
    public class LogitudeLeadHelper
    {
        public LogitudeLeadPM GetLeadById(string id)
        {
            LogitudeLeadQuery logitudeLeadQuery = new LogitudeLeadQuery();
            LogitudeLeadPM leadPM = logitudeLeadQuery.GetSinglePM(id);

            if (leadPM != null)
            {
                CountryRepository countryRepository = new CountryRepository(LogitudeSettings.LogitudeCRMTenantNumber);
                Country country = countryRepository.GetSingleCountryByName(leadPM.Country, LogitudeSettings.LogitudeCRMTenantNumber);
                if (country == null || leadPM.Country == "Unassigned")
                {
                    leadPM.Country = null;
                }
                if (leadPM.CompanyName == "Unassigned")
                {
                    leadPM.CompanyName = null;
                }
                if (leadPM.ContactName == "Unassigned")
                {
                    leadPM.ContactName = null;
                }
            }

            return leadPM;


        }

        private string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }



        public string CreateLogitudeLead(LogitudeLeadPM leadPM)
        {
            if (leadPM != null)
            {
                if ((!string.IsNullOrEmpty(leadPM.CompanyName) && leadPM.CompanyName.ToLower().Contains("https")) || (!string.IsNullOrEmpty(leadPM.ContactName) && leadPM.ContactName.ToLower().Contains("https"))) return null;
                IGlobalContext globalContext = GlobalContext.GetContext();
                if (globalContext == null) globalContext = GlobalContext.GetContext();
                LogitudeLeadService logitudeLeadService = new LogitudeLeadService(globalContext);
                if (string.IsNullOrEmpty(leadPM.Id))
                {
                    if (string.IsNullOrEmpty(leadPM.Country))
                    {
                        leadPM.Country = "Unassigned";
                    }
                    if (string.IsNullOrEmpty(leadPM.CompanyName))
                    {
                        leadPM.CompanyName = "Unassigned";
                    }
                    if (string.IsNullOrEmpty(leadPM.ContactName))
                    {
                        leadPM.ContactName = "Unassigned";
                    }
                    if (string.IsNullOrEmpty(leadPM.RequestType))
                    {
                        leadPM.RequestType = "DemoTenant";
                    }

                    LogitudeLeadPM LogitudeLeadpm = new LogitudeLeadPM()
                    {
                        Email = leadPM.Email,
                        ContactName = TruncateLongString(leadPM.ContactName, 40),
                        PhoneNumber = TruncateLongString(leadPM.PhoneNumber, 40),
                        CompanyName = TruncateLongString(leadPM.CompanyName, 100),
                        Comments = TruncateLongString(leadPM.Comments, 500),
                        Country = TruncateLongString(leadPM.Country, 120),
                        ZipCode = TruncateLongString(leadPM.ZipCode, 15),
                        RequestType = TruncateLongString(leadPM.RequestType, 20),
                        PackageCode = TruncateLongString(leadPM.PackageCode, 4),
                        City = TruncateLongString(leadPM.City, 25),
                        State = TruncateLongString(leadPM.State, 40),
                        Street = TruncateLongString(leadPM.Street, 65),
                        NumberOfBranches = leadPM.NumberOfBranches,
                        NumberOfUsers = leadPM.NumberOfUsers,
                        IsEmailVerified = false,
                        IsSentToCustomer = false,
                        TenantNumber = 1,
                        LastUpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        StatusCode = "InProgress",
                        IATACode = leadPM.IATACode,
                        CASSCode = leadPM.CASSCode,
                        LeadSource = leadPM.LeadSource,
                        VatNumber = leadPM.VatNumber,
                        ClientId = leadPM.ClientId,
                        LeadOrigin = leadPM.LeadOrigin,
                        Campaign = leadPM.Campaign,
                    };

                    logitudeLeadService.Create(LogitudeLeadpm);
                    AddCommunicationLog(LogitudeLeadpm , "Logitude Lead");
                }
            }
            return null;
        }

        public void AddCommunicationLog(LogitudeLeadPM leadPM , string subject)
        {
            byte[] documentXML = LogitudeXmlSerializer.SerializeObject(leadPM);

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                CommunicationsParams logParams = new CommunicationsParams()
                {
                    From = leadPM.CompanyName,
                    Tenant =0,
                    CommunicationLogTypeCode = "Lead",
                    QueueName = "LogitudeLeadQueue",
                    Priority = 1,
                    InOut = "O",
                    Status = "W",
                    Subject = subject,
                    FolderName = "LogitudeLeadQueue",
                    ByteData = documentXML,

                };
                Communications.AddCommunicationLog(logParams);

                scope.Complete();
            }
        }








        public string VerifiyLogitudeLead(LogitudeLeadPM leadPM)
        {
            LogitudeLeadQuery logitudeLeadQuery = new LogitudeLeadQuery();
            LogitudeLeadPM entityPM = logitudeLeadQuery.GetSinglePM(leadPM.Id);
            if (entityPM != null )
            {
                entityPM.LastUpdateDate = DateTime.Now;
                entityPM.Email = leadPM.Email;
                entityPM.ContactName = TruncateLongString(leadPM.ContactName, 60);
                entityPM.CompanyName = TruncateLongString(leadPM.CompanyName, 100);
                entityPM.PhoneNumber = TruncateLongString(leadPM.PhoneNumber, 40);
                entityPM.NumberOfUsers = leadPM.NumberOfUsers;
                entityPM.Country = TruncateLongString(leadPM.Country, 120);
                entityPM.Comments = TruncateLongString(leadPM.Comments, 500);
                entityPM.Comments = leadPM.Comments;
                entityPM.Comments = leadPM.Comments;
                entityPM.IsEmailVerified = leadPM.IsEmailVerified;

                IGlobalContext globalContext = GlobalContext.GetContext();
                if (globalContext == null) globalContext = GlobalContext.GetContext();
                LogitudeLeadService logitudeLeadService = new LogitudeLeadService(globalContext);

                logitudeLeadService.Update(entityPM);

                ICommonDataContext commonContext = CommonDataContext.GetContext(LogitudeSettings.LogitudeCRMTenantNumber);
                CountryRepository countryRepository = new CountryRepository(commonContext);
                AddressRepository addressRepository = new AddressRepository(commonContext);
                CustomerRepository customerRepository = new CustomerRepository(commonContext);
                CardRepository cardRepository = new CardRepository(commonContext);
                ContactRepository contactRepository = new ContactRepository(commonContext);

                Card customerCard = cardRepository.GetSingleCardByIdAndTenant(entityPM.CustomerId, LogitudeSettings.LogitudeCRMTenantNumber, false);
                if (customerCard != null)
                {
                    customerCard.EnglishName = entityPM.CompanyName;
                    customerCard.LocalName = entityPM.CompanyName;
                    customerCard.Notes = entityPM.Country;
                    customerCard.CityName = entityPM.City != "Unassigned" ? entityPM.City : null;
                    customerCard.CountryName = entityPM.Country;


                    cardRepository.Update(customerCard);
                    cardRepository.SubmitChanges();

                    if (!string.IsNullOrEmpty(customerCard.PrimaryContactId))
                    {
                        Contact customerContact = contactRepository.GetSingleContact(customerCard.PrimaryContactId, LogitudeSettings.LogitudeCRMTenantNumber);
                        customerContact.EnglishName = TruncateLongString(entityPM.ContactName, 60);
                        customerContact.LocalName = TruncateLongString(entityPM.ContactName, 60);
                        customerContact.BusinessPhone = entityPM.PhoneNumber;
                        contactRepository.Update(customerContact);
                        contactRepository.SubmitChanges();
                    }
                }

                Address customerAddress = addressRepository.GetMainAddressByCardId(entityPM.CustomerId, LogitudeSettings.LogitudeCRMTenantNumber);
                if (customerAddress != null)
                {
                    customerAddress.Name = entityPM.CompanyName;
                    Country country = countryRepository.GetSingleCountryByName(entityPM.Country, LogitudeSettings.LogitudeCRMTenantNumber);
                    if (country != null)
                    {
                        customerAddress.CountryId = country.Id;
                        customerAddress.PhoneNumber = entityPM.PhoneNumber;
                        customerAddress.City = entityPM.City != "Unassigned" ? entityPM.City : null;
                    }

                    addressRepository.SubmitChanges();
                }

                ICRMContext crmContext = CRMContext.GetContext(LogitudeSettings.LogitudeCRMTenantNumber);

                CRMDomainService crmDomainService = new CRMDomainService();
                OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
                StageRepository stageRepository = new StageRepository(crmContext);

                Opportunity opportunity = opportunityRepository.GetSingle(entityPM.OpportunityId, LogitudeSettings.LogitudeCRMTenantNumber); ;//crmDomainService.GetSingleOpportunityPM(entityPM.OpportunityId, LogitudeSettings.LogitudeCRMTenantNumber);//
                if (opportunity != null)
                {
                    Stage stage = stageRepository.GetStageByCode("OPN", LogitudeSettings.LogitudeCRMTenantNumber);

                    DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(LogitudeSettings.LogitudeCRMTenantNumber);
                    OpportunityStage opportunityStage = new OpportunityStage()
                    {
                        Id = IdCounter.GetNumber("OpportunityStage", opportunity.Tenant),
                        OpportunityId = opportunity.Id,
                        Tenant = opportunity.Tenant,
                        FromStageId = opportunity.StageId,
                        ToStageId = stage.Id,
                        StartDate = opportunity.LastStageDate,
                        EndDate = todayDate,
                    };

                    opportunity.LastStageDate = todayDate;

                    OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(opportunity.Tenant);
                    opportunityStageRepository.Add(opportunityStage);
                    opportunityStageRepository.SubmitChanges();

                    opportunity.Notes = entityPM.Comments;
                    opportunity.NumberOfShipments = entityPM.NumberOfUsers;
                    opportunity.StageId = stage.Id;
                    opportunity.Probability = stage.Probability;
                    opportunityRepository.Update(opportunity);
                    opportunityRepository.SubmitChanges();
                }

                AddCommunicationLog(entityPM, "Verifiy Logitude Lead");
            }


            return null;
        }
    }
}