using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for OpportunitySummaryWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OpportunitySummaryWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetOpportunitySummaryData(string opportunityId, int tenant, string documentTypeCode)
        {
            OpportunitySummaryDataProvider opportunitySummaryDataProvider = GetOpportunitySummaryDataProvider(opportunityId, tenant, documentTypeCode);
            XmlSerializer serializer = new XmlSerializer(typeof(OpportunitySummaryDataProvider));

            using (MemoryStream memstream = new MemoryStream())
            {
                serializer.Serialize(memstream, opportunitySummaryDataProvider);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                return bytearray;
            }
        }

        public OpportunitySummaryDataProvider GetOpportunitySummaryDataProvider(string opportunityId, int tenant, string documentTypeCode)
        {
            OpportunitySummaryDataProvider totalData = new OpportunitySummaryDataProvider();
            totalData.OpportunityContacts = new List<OpportunityContactClass>();
            totalData.OpportunityProducts = new List<OpportunityProductClass>();
            totalData.OpportunityAdditionalServices = new List<OpportunityAdditionalServiceClass>();
            totalData.OpportunityTasks = new List<OpportunityTaskClass>();

            ICRMContext crmContext = CRMContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
            ActivityRepository activityRepository = new ActivityRepository(crmContext);
            ActivityInviteeRepository activityInviteeRepository = new ActivityInviteeRepository(crmContext);
            OpportunityAdditionalServiceRepository opportunityAdditionalServiceRepository = new OpportunityAdditionalServiceRepository(crmContext); 
            OpportunityAdditionalServiceQueryService opportunityAdditionalServiceQueryService = new OpportunityAdditionalServiceQueryService(opportunityAdditionalServiceRepository);
            OpportunityProductRepository opportunityProductRepository = new OpportunityProductRepository(crmContext);
            OpportunityProductQueryService opportunityProductQueryService = new OpportunityProductQueryService(opportunityProductRepository);
            CustomerRepository customerRepository = new CustomerRepository(commonContext);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            UserRepository userRepository = new UserRepository(commonContext);
            CardContactRepository cardContactRepository = new CardContactRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            IndustryRepository industryRepository = new IndustryRepository(commonContext);
            CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(commonContext);
            CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
            CustomerProductRepository customerProductRepository = new CustomerProductRepository(commonContext);
            CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
            TenantRepository tenantRepository = new TenantRepository(commonContext);

            Opportunity myEntity = opportunityRepository.GetSingle(opportunityId, tenant);

            if (myEntity != null)
            {
                Tenant myTenant = tenantRepository.GetSingleByTenant(tenant);
                if (myTenant != null)
                {
                    totalData.TenantName = myTenant.Company;
                    //totalData.RightToLeft = myTenant.IsNotesRightToLeftEnabled;
                }

                totalData.Subject = myEntity.Subject;

                IQueryable<Activity> activities = activityRepository.GetActivitiesByOpportunityId(myEntity.Id, tenant);
                List<string> customerContactIds = new List<string>();
                List<string> meetingContactIds = new List<string>();
                List<string> meetingUsertIds = new List<string>();
                List<string> tenantUsersIds = new List<string>();
                List<string> tenantContactsIds = new List<string>();

                IQueryable<User> users = userRepository.GetUsers(tenant);
                IQueryable<Contact> contacts = contactRepository.GetActiveContacts(tenant);

                if (users != null && users.Count() > 0)
                {
                    foreach (User user in users)
                    {
                        tenantUsersIds.Add(user.Id);
                    }
                }

                if (contacts != null && contacts.Count() > 0)
                {
                    foreach (Contact contact in contacts)
                    {
                        tenantContactsIds.Add(contact.Id);
                    }
                }

                //Customer
                if (!string.IsNullOrEmpty(myEntity.CustomerId))
                {
                    Customer customer = customerRepository.GetSingleCustomer(myEntity.CustomerId, tenant, true);
                    if (customer != null)
                    {
                        totalData.CustomerName = customer.Card.EnglishName;
                        totalData.CustomerLocalName = customer.Card.LocalName;
                        totalData.CustomerVat = customer.Card.VatNumber;

                        if (!string.IsNullOrEmpty(customer.IndustryId))
                        {
                            Industry industry = industryRepository.GetSingleIndustry(customer.IndustryId, tenant);
                            if (industry != null)
                            {
                                totalData.CustomerIndustry = industry.Name;
                            }
                        }

                        Address address = addressRepository.GetMainAddressByCardId(customer.Id, tenant);
                        if (address != null)
                        {
                            totalData.CustomerMainAddress = General.GetAddress(address);
                        }

                        IQueryable<CardContact> cardContacts = cardContactRepository.GetCardContactsByCardId(myEntity.CustomerId);
                        foreach (CardContact contact in cardContacts)
                        {
                            customerContactIds.Add(contact.ContactId);
                        }
                    }
                }

                //Owner
                if (!string.IsNullOrEmpty(myEntity.OwnerId))
                {
                    Contact owner = contactRepository.GetSingleContact(myEntity.OwnerId, tenant);
                    if (owner != null)
                    {
                        totalData.OpportunityOwner = owner.EnglishName;
                    }
                }

                //Meeting Summay
                IQueryable<Activity> meetingSummaries = activities.Where(d => d.ActivityTypeCode == "AP" && d.ActivityStatusCode == "C");
                if (meetingSummaries != null && meetingSummaries.Count() > 0)
                {
                    Activity firstMeetingSummary = meetingSummaries.OrderByDescending(d => d.StartDateTime).FirstOrDefault();

                    if (firstMeetingSummary != null)
                    {
                        totalData.MeetingSummary = firstMeetingSummary.MeetingSummary;
                        totalData.MeetingSummaryRightToLeft = firstMeetingSummary.MeetingSummaryRightToLeft;
                        ActivityKeys keys = new ActivityKeys() { Id = firstMeetingSummary.Id };
                        List<ActivityInvitee> invitees = activityInviteeRepository.GetMulti(keys);

                        foreach (ActivityInvitee invitee in invitees)
                        {
                            meetingContactIds.Add(invitee.ContactId);
                        }
                    }
                }

                //Contacts
                if (!string.IsNullOrEmpty(myEntity.ContactId))
                {
                    Contact contact = contactRepository.GetSingleContact(myEntity.ContactId, tenant);
                    if (contact != null)
                    {
                        OpportunityContactClass mainRecord = new OpportunityContactClass();

                        mainRecord.ContactName = contact.EnglishName;
                        mainRecord.ContactEmail = contact.Email;
                        mainRecord.ContactPhone = contact.BusinessPhone;
                        mainRecord.ContactPosition = contact.Position;
                        mainRecord.ContactNotes = contact.Notes;

                        totalData.OpportunityContacts.Add(mainRecord);
                    }
                }

                foreach (string id in meetingContactIds)
                {
                    if (customerContactIds.Contains(id))
                    {
                        if (id != myEntity.ContactId)
                        {
                            Contact contact = contactRepository.GetSingleContact(id, tenant);
                            if (contact != null)
                            {
                                OpportunityContactClass record = new OpportunityContactClass();

                                record.ContactName = contact.EnglishName;
                                record.ContactEmail = contact.Email;
                                record.ContactPhone = contact.BusinessPhone;
                                record.ContactPosition = contact.Position;
                                record.ContactNotes = contact.Notes;

                                totalData.OpportunityContacts.Add(record);
                            }
                        }
                    }

                    if (tenantUsersIds.Contains(id))
                    {
                        meetingUsertIds.Add(id);
                    }
                }

                if (meetingUsertIds != null && meetingUsertIds.Count > 0)
                {
                    string meetingUsersNames = "";
                    foreach (string id in meetingUsertIds)
                    {
                        User user = userRepository.GetSingleUser(id, tenant);

                        if (user != null)
                        {
                            if (string.IsNullOrEmpty(meetingUsersNames))
                            {
                                meetingUsersNames = user.Contact.EnglishName;
                            }
                            else
                            {
                                meetingUsersNames = meetingUsersNames + ", " + user.Contact.EnglishName;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(meetingUsersNames))
                    {
                        if (string.IsNullOrEmpty(totalData.OpportunityOwner))
                        {
                            totalData.OpportunityOwner = meetingUsersNames;
                        }
                        else
                        {
                            totalData.OpportunityOwner = totalData.OpportunityOwner + ", " + meetingUsersNames;
                        }
                    }
                }

                //Additional Services
                List<OpportunityAdditionalServicePM> additionalServices = opportunityAdditionalServiceQueryService.GetOpportunityAdditionalServicesByOpportunityId(myEntity.Id, tenant);
                if (additionalServices != null && additionalServices.Count > 0)
                {
                    foreach (OpportunityAdditionalServicePM service in additionalServices)
                    {
                        OpportunityAdditionalServiceClass record = new OpportunityAdditionalServiceClass();
                        record.AdditionalServiceName = service.EnglishName;
                        record.AdditionalServiceNote = service.Notes;
                        record.NotesRightToLeft = service.NotesRightToLeft;
                        totalData.OpportunityAdditionalServices.Add(record);
                    }
                }

                else
                {
                    //Get from customer
                    List<CustomerAdditionalServicePM> customerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(myEntity.CustomerId, tenant);
                    if (customerAdditionalServices != null && customerAdditionalServices.Count > 0)
                    {
                        foreach (CustomerAdditionalServicePM service in customerAdditionalServices)
                        {
                            OpportunityAdditionalServiceClass record = new OpportunityAdditionalServiceClass();
                            record.AdditionalServiceName = service.AdditionalServiceName;
                            record.AdditionalServiceNote = service.Notes;
                            record.NotesRightToLeft = service.NotesRightToLeft;
                            totalData.OpportunityAdditionalServices.Add(record);
                        }
                    }
                }

                //Products
                List<OpportunityProductPM> products = opportunityProductQueryService.GetOpportunityProductsByOpportunityId(myEntity.Id, tenant);
                if (products != null && products.Count > 0)
                {
                    foreach (OpportunityProductPM product in products)
                    {
                        if ((product.NumberOfShipments != null && product.NumberOfShipments != 0)
                            || (product.ChargeableWeight != null && product.ChargeableWeight != 0)
                            || (product.Revenue != null && product.Revenue != 0)
                            || (product.TEU != null && product.TEU != 0)
                            || (product.OpportunityProductLocations != null && product.OpportunityProductLocations.Count > 0))
                        {
                            List<OpportunityProductLocationPM> locations = product.OpportunityProductLocations;

                            OpportunityProductClass record = new OpportunityProductClass();
                            record.ProductName = product.OpportunityProductTypeName;
                            record.ProductNote = product.Notes;
                            record.ProductPrepaidCollect = product.PrepaidCollectName;
                            record.NotesRightToLeft = product.NotesRightToLeft;
                            string summary = "";
                            if (product.NumberOfShipments != null && product.NumberOfShipments != 0)
                            {
                                summary = "No of Shipments: " + product.NumberOfShipments;
                            }

                            if (product.ChargeableWeight != null && product.ChargeableWeight != 0)
                            {
                                if (string.IsNullOrEmpty(summary))
                                {
                                    summary = "Chargeable Weight: " + product.ChargeableWeight;
                                }

                                else
                                {
                                    summary = summary + Environment.NewLine + "Chargeable Weight: " + product.ChargeableWeight;
                                }
                            }

                            if (product.Revenue != null && product.Revenue != 0)
                            {
                                if (string.IsNullOrEmpty(summary))
                                {
                                    summary = "Revenue: " + product.Revenue;
                                }

                                else
                                {
                                    summary = summary + Environment.NewLine + "Revenue: " + product.Revenue;
                                }
                            }

                            if (product.TEU != null && product.TEU != 0)
                            {
                                if (string.IsNullOrEmpty(summary))
                                {
                                    summary = "TEU: " + product.TEU;
                                }

                                else
                                {
                                    summary = summary + Environment.NewLine + "TEU: " + product.TEU;
                                }
                            }

                            if (locations != null && locations.Count > 0)
                            {
                                string countries = "";
                                foreach (OpportunityProductLocationPM location in locations)
                                {
                                    if (string.IsNullOrEmpty(countries))
                                    {
                                        countries = "Countries: " + location.LocationCode;
                                    }
                                    else
                                    {
                                        countries = countries + ", " + location.LocationCode;
                                    }
                                }

                                if (string.IsNullOrEmpty(summary))
                                {
                                    summary = countries;
                                }

                                else
                                {
                                    summary = summary + Environment.NewLine + countries;
                                }
                            }

                            record.ProductSummary = summary;
                            totalData.OpportunityProducts.Add(record);
                        }
                    }
                }

                else
                {
                    //Get from customer
                    List<CustomerProductPM> customerProductss = customerProductQuery.GetCustomerProductPMsByCustomerId(myEntity.CustomerId, tenant);
                    if (customerProductss != null && customerProductss.Count > 0)
                    {
                        foreach (CustomerProductPM product in customerProductss)
                        {
                            if ((product.PotentialNumberOfShipments != null && product.PotentialNumberOfShipments != 0)
                            || (product.PotentialChargeableWeight != null && product.PotentialChargeableWeight != 0)
                            || (product.PotentialRevenue != null && product.PotentialRevenue != 0)
                            || (product.PotentialTEU != null && product.PotentialTEU != 0)
                            || (product.ProductLocations != null && product.ProductLocations.Count > 0))
                            {
                                List<CustomerProductLocationPM> locations = product.ProductLocations;

                                OpportunityProductClass record = new OpportunityProductClass();
                                record.ProductName = product.ProductTypeName;
                                record.ProductNote = product.Notes;
                                record.ProductPrepaidCollect = product.PrepaidCollectName;
                                record.NotesRightToLeft = product.NotesRightToLeft;

                                string summary = "";
                                if (product.PotentialNumberOfShipments != null && product.PotentialNumberOfShipments != 0)
                                {
                                    summary = "No of Shipments: " + product.PotentialNumberOfShipments;
                                }

                                if (product.PotentialChargeableWeight != null && product.PotentialChargeableWeight != 0)
                                {
                                    if (string.IsNullOrEmpty(summary))
                                    {
                                        summary = "Chargeable Weight: " + product.PotentialChargeableWeight;
                                    }

                                    else
                                    {
                                        summary = summary + Environment.NewLine + "Chargeable Weight: " + product.PotentialChargeableWeight;
                                    }
                                }

                                if (product.PotentialRevenue != null && product.PotentialRevenue != 0)
                                {
                                    if (string.IsNullOrEmpty(summary))
                                    {
                                        summary = "Revenue: " + product.PotentialRevenue;
                                    }

                                    else
                                    {
                                        summary = summary + Environment.NewLine + "Revenue: " + product.PotentialRevenue;
                                    }
                                }

                                if (product.PotentialTEU != null && product.PotentialTEU != 0)
                                {
                                    if (string.IsNullOrEmpty(summary))
                                    {
                                        summary = "TEU: " + product.PotentialTEU;
                                    }

                                    else
                                    {
                                        summary = summary + Environment.NewLine + "TEU: " + product.PotentialTEU;
                                    }
                                }

                                if (locations != null && locations.Count > 0)
                                {
                                    string countries = "";
                                    foreach (CustomerProductLocationPM location in locations)
                                    {
                                        if (string.IsNullOrEmpty(countries))
                                        {
                                            countries = "Countries: " + location.CountryCode;
                                        }
                                        else
                                        {
                                            countries = countries + ", " + location.CountryCode;
                                        }
                                    }

                                    if (string.IsNullOrEmpty(summary))
                                    {
                                        summary = countries;
                                    }

                                    else
                                    {
                                        summary = summary + Environment.NewLine + countries;
                                    }
                                }

                                record.ProductSummary = summary;
                                totalData.OpportunityProducts.Add(record);
                            }
                        }
                    }
                }

                //Tasks
                IQueryable<Activity> tasks = activities.Where(d => d.ActivityTypeCode == "TS" && d.ActivityStatusCode != "C");
                if (tasks != null && tasks.Count() > 0)
                {
                    foreach (Activity task in tasks)
                    {
                        OpportunityTaskClass record = new OpportunityTaskClass();
                        record.TaskSubject = task.Subject;
                        record.TaskDueDate = task.DueDate;

                        if (!string.IsNullOrEmpty(task.OwnerId))
                        {
                            Contact owner = contactRepository.GetSingleContact(task.OwnerId, tenant);
                            if (owner != null)
                            {
                                record.TaskOwner = owner.EnglishName;
                            }
                        }                        

                        totalData.OpportunityTasks.Add(record);
                    }
                }
            }

            return totalData;
        }
    }
}
