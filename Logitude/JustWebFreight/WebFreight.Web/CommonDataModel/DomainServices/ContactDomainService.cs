using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using WebFreight.Web.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    // Implements application logic using the CommonDataContext context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public partial class ContactDomainService : LogitudeDomainService
    {
        private ICommonDataContext objectContext;
        private ContactRepository contactRepository;
        private CardContactRepository cardContactRepository;
        private RoleRepository roleRepository;
        private ContactTenantRepository contactTenantRepository;
        private ContactTenantRoleRepository contactTenantRoleRepository;
        private UserRepository userRepository;
        private UserLastLoginRepository userLastLoginRepository;
        private RoleFeatureRepository roleFeatureRepository;
        private FeatureRepository featureRepository;
        private RestrictionRepository restrictionRepository;
        private PackageFeatureRepository packageFeatureRepository;

        private FeatureQuery featureQuery;
        private RoleFeatureQuery roleFeatureQuery;
        private RestrictionQuery restrictionQuery;
        private UserLastLoginQuery userLastLoginQuery;
        private UserPermittedBranchRepository userPermittedBranchRepository;
        private UserPermittedBranchQuery userPermittedBranchQuery;
        public void UpdateContactList(ContactList currentEntity)
        {
        }

        public bool DoesContactEmailExist(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            contactRepository = new ContactRepository(tenant);
            return (contactRepository.GetActiveContacts(tenant).Where(d => d.Email == email && d.Tenant == tenant)).Any();
        }
        
        [Invoke]
        public ContactPM CheckIfContactExists(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            ContactPM contact = contactQuery.GetContactByEmailOnly(email, tenant);
          
            return contact;
        }
        
        public Contact GetSingleContactByEmail(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            return (contactRepository.GetActiveContacts(tenant).Where(d => d.Email == email && d.Tenant == tenant)).FirstOrDefault();
        }

        //public IQueryable<Contact> GetContacts(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

        //    contactRepository = new ContactRepository(tenant);
        //    return contactRepository.GetContacts(0);
        //}

        public List<ContactPM> GetFirstContactsByTenantType(string input, int tenant, string typeInput)
        {
            //contactRepository = new ContactRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            input = input.ToUpper();
            string[] types = typeInput.Split(',');
            List<ContactPM> result = new List<ContactPM>();

            foreach (string type in types)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                List<ContactPM> contacts = contactQuery.GetContactPMsByTenant(tenant).Where(c => c.Tenant == tenant).Where(c => c.EnglishName.ToUpper().StartsWith(input)).Take(50).OrderBy(p => p.EnglishName).ToList();
                foreach (ContactPM contact in contacts)
                {
                    result.Add(contact);
                }
            }
            return result;
        }

        public ContactPM GetContactByFacebookId(string facebookId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            return contactQuery.GetContactByFacebookId(facebookId, tenant);
        }

        [InvokeAttribute]
        public bool IsEmailExists(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            contactRepository = new ContactRepository(objectContext);

            
            bool result = false;
            bool isContactExists = contactRepository.IsContactByEmailExists(email, tenant);

            result = (isContactExists);
            return result;
        }

        public ContactPM GetContactByEmailOnly(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactByEmailOnly(email, tenant);
        }

        public ContactPM GetContactByEmailOnlyForLogin(string email, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactByEmailOnlyForLogin(email, tenant);
        }

        public ContactPM GetSingleContact(string id, int tenant)
        {

            //contactRepository = new ContactRepository(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetSinglePM(id, tenant);
        }

        public ContactList GetSingleContactList(string id, int tenant)
        {
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            ContactList contactList = null;
            Contact contact = contactRepository.GetSingleContact(id, tenant);

            if (contact != null)
            {
                List<Contact> singleEntityList = new List<Contact>();
                singleEntityList.Add(contact);

                IQueryable<Contact> iQueryable = singleEntityList.AsQueryable();
                ContactQuery contactQuery = new ContactQuery(contactRepository);
                IQueryable<ContactList> iQueryableEntityList = contactQuery.GetIQueryableEntityList(iQueryable);
                contactList = iQueryableEntityList.FirstOrDefault();
            }
            return contactList;
        }

        public IQueryable<ContactList> GetContactLists(int tenant)
        {
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            IQueryable<Contact> contacts = contactRepository.GetActiveContacts(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            IQueryable<ContactList> query2 = contactQuery.GetIQueryableEntityList(contacts);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public List<ContactList> GetContactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Contact> contacts = contactRepository.GetContacts(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            ContactCustomFilter customfilters = new ContactCustomFilter(tenant);
            contacts = customfilters.GetFilteredQuery(queryOperations, contacts);

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            contacts = filter.GetFilteredQuery<Contact>(nonListQueryOperation, contacts);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            ContactQuery contactQuery = new ContactQuery(contactRepository);
            IQueryable<ContactList> query2 = contactQuery.GetIQueryableEntityList(contacts);
            query2 = filter.GetFilteredQuery<ContactList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ContactList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Contact", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ContactList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            List<string> contactIds = (from a in query2
                                       select a.Id).ToList();

            List<ContactList> result = query2.ToList();

            if (!queryOperations.GetAll)
            {
                CardContactRepository cardContactRepository = new CardContactRepository(tenant);
                List<CardContact> cardContacts = cardContactRepository.GetCardsContactsForContactIds(contactIds, tenant).ToList();
                foreach (ContactList contact in result)
                {
                    List<Card> cards = cardContacts.Where(c => c.ContactId == contact.Id).Select(c => c.Card).ToList();
                    string str = String.Empty;
                    foreach (Card card in cards)
                    {
                        str = str + card.EnglishName + "; ";

                    }
                    if (!string.IsNullOrEmpty(str) && str.Length >= 2)
                    {
                        contact.Company = str.Remove(str.Length - 2);
                    }

                }
            } 
            return result;
        }

        public int GetContactFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            contactRepository = new ContactRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Contact> contacts = contactRepository.GetContacts(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            contacts = filter.GetFilteredQuery<Contact>(nonListQueryOperation, contacts);

            ContactCustomFilter customfilters = new ContactCustomFilter(tenant);
            contacts = customfilters.GetFilteredQuery(queryOperations, contacts);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            IQueryable<ContactList> query2 = contactQuery.GetIQueryableEntityList(contacts);
            query2 = filter.GetFilteredQuery<ContactList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        //public List<ContactPM> GetContactsBySearchQuery(string filterText, string contactType, int tenant)
        //{
        //    SecurityUtility.CheckContactFeature("Contact", "READ", tenant);  

        //    contactRepository = new ContactRepository(tenant);
        //    SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
        //    ContactQuery contactQuery = new ContactQuery(contactRepository);
        //    return contactQuery.GetContactsBySearchQuery(filterText, contactType, tenant);
        //}

        //public List<ContactPM> GetContactsByEmailOrName(string email, string name, int tenant)
        //{
        //    //contactRepository = new ContactRepository(tenant);
        //    SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
        //    ContactQuery contactQuery = new ContactQuery(tenant);
        //    return contactQuery.GetContactsByEmailOrName(email, name, tenant).Where(d => d.Tenant == tenant).ToList();
        //}

        public List<ContactPM> GetContactsByTenant(int tenant)
        {
            //contactRepository = new ContactRepository(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactPMsByTenant(tenant).Where(d => d.Tenant == tenant).ToList();
        }

        public List<ContactPM> GetContactsByEmail(string email, int tenant)
        {
            //contactRepository = new ContactRepository(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactsByEmail(email, tenant).ToList();
            //return ContactsRepository.GetContactPMsByTenant(tenant).Where(d => d.Email == email && d.Tenant == tenant);
        }

        //public List<ContactPM> GetContactsByID(string id, int tenant)
        //{
        //    //contactRepository = new ContactRepository(tenant);
        //    SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
        //    ContactQuery contactQuery = new ContactQuery(tenant);
        //    return contactQuery.GetContactPMsByTenant(tenant).Where(d => d.Id == id && d.Tenant == tenant).ToList();
        //}

        public void MapContactContactPM(ContactPM contactPm, Contact contact)
        {
            contact.Anniversary = contactPm.Anniversary;
            contact.Birthday = contactPm.Birthday;
            contact.BusinessPhone = contactPm.BusinessPhone;
            contact.Email = contactPm.Email;
            contact.EnglishName = contactPm.EnglishName;
            contact.FacebookId = contactPm.FacebookId;
            contact.Fax = contactPm.Fax;
            contact.Id = contactPm.Id;
            contact.InActive = contactPm.InActive;
            contact.LocalName = contactPm.LocalName;
            contact.Mobile = contactPm.Mobile;
            //contact.Password = contactPm.Password!=null?contactPm.Password:contact.Password;
            contact.Notes = contactPm.Notes;
            contact.Tenant = contactPm.Tenant;
            contact.Signature = contactPm.Signature;
            //contact.NumberOfRetries = contactPm.NumberOfRetries;
            //contact.IsLocked = contactPm.IsLocked;
            //contact.MustChangePassword = contactPm.MustChangePassword;
            contact.SearchFields = contactPm.EnglishName + "," + contactPm.LocalName + "," + contactPm.Email + "," + contactPm.BusinessPhone + "," + contactPm.Mobile + "," + contactPm.Fax;
            contact.DisplayGettingStarted = contactPm.DisplayGettingStarted;
            contact.BirthdayReminder = contactPm.BirthdayReminder;
            contact.AnniversaryReminder = contactPm.AnniversaryReminder;
            contact.DoneDate = contactPm.DoneDate;
            contact.BirthDayOfYear = contactPm.Birthday != null ? contactPm.Birthday.Value.DayOfYear : 0;
            contact.ContactDoneMethodCode = contactPm.ContactDoneMethodCode;
            contact.Position = contactPm.Position;
            contact.ExternalId = contactPm.ExternalId;
            contact.CreateDate = contactPm.CreateDate;
            contact.DigitalPortalLanguage = contactPm.DigitalPortalLanguage;
        }

        public void InsertContact(ContactPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Contact", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            ContactService service = new ContactService(objectContext, entityPM.Tenant);
            service.Create(entityPM);
        }

        public void UpdateContact(ContactPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Contact", "UPDATE", entityPM.Tenant); 

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.IsChangeSignatur)
            {
                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                entityPM.SignatureHtml = htmlEditorHelper.ConvertXmlByteToHtmlByte(entityPM.Signature);  
            }

            ContactService service = new ContactService(objectContext, entityPM.Tenant);
            service.Update(entityPM);
        }

        public void DeleteContact(ContactPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.Id != null)
            {
                if (entityPM.Id.Contains(","))
                {
                    string[] ids = entityPM.Id.Split(',');
                    entityPM.Id = ids[0];
                }
            }

            contactRepository = new ContactRepository(objectContext);
            Contact entity = contactRepository.GetSingleContact(entityPM.Id, entityPM.Tenant);
            if (entity != null)
            {
                contactRepository.Remove(entity);
            }
        }

        public void UpdateCard(CardPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CardService service = new CardService(objectContext, entityPM);
            service.Update();
        }

        public List<CardPM> GetCardsForContact(string contactId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardContactQuery cardContactQuery = new CardContactQuery(tenant);
            List<CardPM> myResult = cardContactQuery.GetCardsForContact(contactId, tenant);

            return myResult;
        }

        protected override bool PersistChangeSet()
        {
            objectContext.SaveChanges();
            return base.PersistChangeSet();
        }

        protected override bool ExecuteChangeSet()
        {
            return base.ExecuteChangeSet();
        }

        public ContactSummary GetContactsSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            contactRepository = new ContactRepository(tenant);
            IQueryable<Contact> dataSource = contactRepository.GetContacts(tenant);
            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            int currentDayOfYear = todayDate.Value.DayOfYear;
            int day1 = currentDayOfYear -5;
            int day2 = currentDayOfYear + 5;

            if (day1 < 1)
            {
                day1 = 1;
            }
            if (day2 < 1)
            {
                day2 = 365;
            }

            ContactSummary summaryClass = new ContactSummary() { Id = tenant };

            summaryClass.UpcomingEventsCount = dataSource.Where (c =>
                                                        (c.BirthdayReminder
                                                        && c.BirthDayOfYear >= day1
                                                        && c.BirthDayOfYear <= day2)
                                                    ).Count();

            summaryClass.WithoutRemindersCount = dataSource.Where(c => !c.BirthdayReminder).Count();
            //summaryClass.AllContactsCount = dataSource.Count();

            return summaryClass;
        }


        [Invoke]
        public void SayHappyBirthday(string contactId, string doneCode, int tenant)
        {
            SecurityUtility.CheckContactFeature("Contact", "UPDATE", tenant);
            
            contactRepository = new ContactRepository (tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            Contact entity = contactRepository.GetSingleContact(contactId, tenant);

            if (entity != null)
            {
                entity.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entity.ContactDoneMethodCode = doneCode;

                contactRepository.Update(entity);
                contactRepository.SubmitChanges();                
            }

            ContactPM currentContact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            if (currentContact != null)
            {
                if (entity.ContactDoneMethodCode != "NO")
                {
                    ContactDoneMethodRepository rep = new ContactDoneMethodRepository(tenant);
                    ContactDoneMethod method = rep.GetSingleContactDoneMethod(doneCode);

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "HPCO",
                        UserId = currentContact.Id,
                        EntityId = contactId,
                        ObjectTableName = "Contact",
                        Notes = "Felicitated By: " + method.Name,
                    });
                }
            }
        }
    }
}


