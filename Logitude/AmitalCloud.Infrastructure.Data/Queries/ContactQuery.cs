using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using System.Data.Entity;
using System.Linq.Expressions;
using AmitalCloud.Infrastructure.Domain.Interfaces;
namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ContactQuery
    {
        IRepository<Contact> repository;
        IAmitalCloudContext context ;
        public ContactQuery() : this(0) 
        {
        }
        public ContactQuery(int tenant)
        {            
            context =  AmitalCloudContext.GetContext(tenant);
            repository = new Repository<Contact>(context);
        }
        #region GetSingle ContactPM
        private ContactPM GetContactPMFromCache(int tenant, string cacheKey, Expression<Func<ContactPM, bool>> predicate)
        {
            ContactPM entity;
            if (HttpContext.Current != null)
            {
                entity = (ContactPM)CacheManager.CacheWrapper.Get(cacheKey);
                if (entity == null)
                {
                    entity = GetEntityPMWithPassword(tenant, predicate);
                    if (entity != null)
                    {
                        if (CacheManager.CacheWrapper.Get(cacheKey) == null)
                        {
                            CacheManager.CacheWrapper.Insert(cacheKey, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }
            }
            else
            {
                entity = GetEntityPMWithPassword(tenant, predicate);
            }

            return entity;
        }
        private IQueryable<ContactPM> GetContactPMQuery()
        {
            return (from a in context.Contacts where a.UserType == "R"
                    let contact = new ContactPM()
                    {
                        Anniversary = a.Anniversary,
                        Birthday = a.Birthday,
                        BusinessPhone = a.BusinessPhone,
                        Email = a.Email,
                        EnglishName = a.EnglishName,
                        FacebookId = a.FacebookId,
                        Fax = a.Fax,
                        Id = a.Id,
                        InActive = a.InActive,
                        LocalName = a.LocalName,
                        SearchFields = a.SearchFields,
                        DontShowLocalLabels = a.DontShowLocalLabels,
                        Mobile = a.Mobile,
                        Notes = a.Notes,
                        Tenant = a.Tenant,
                        Signature = a.Signature,
                        SignatureHtml = a.SignatureHtml,
                        ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                        DontShowLocal = a.DontShowLocalLabels,
                        DisplayGettingStarted = a.DisplayGettingStarted,
                        BirthdayReminder = a.BirthdayReminder,
                        AnniversaryReminder = a.AnniversaryReminder,
                        ImageDetailId = a.ImageDetailId,
                        DoneDate = a.DoneDate,
                        BirthDayOfYear = a.BirthDayOfYear,
                        ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                        ContactDoneMethodName = a.ContactDoneMethod != null ? a.ContactDoneMethod.Name : null,
                        Position = a.Position,
                        ExternalId = a.ExternalId,
                        CompanyName = a.CompanyName,
                        CreateDate = a.CreateDate,
                        IndexColor = a.IndexColor,
                        DigitalPortalLanguage = a.DigitalPortalLanguage,
                    }
                    select contact);
        }
        private void GetContactPassword( ContactPM instance)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == instance.Email.ToLower()).FirstOrDefault();
                GlobalContact globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == instance.Email.ToLower() && cn.GlobalTenantId == instance.Tenant).FirstOrDefault();
                if (globalContact != null)
                {
                    instance.IsUser = globalContact.IsUser;
                }
                if (contactPassword != null)
                {
                    //instance.Password = contactPassword.Password;
                    instance.IsLocked = contactPassword.IsLocked;
                    instance.MustChangePassword = contactPassword.MustChangePassword;
                    instance.NumberOfRetries = contactPassword.NumberOfRetries;
                }
                scope.Complete();
            }
        }
        private void GetCardContact( ContactPM instance)
        {
            instance.HasCardContact = false;
            CardContactRepository cardcontactRep = new CardContactRepository(instance.Tenant);
            CardContact cardContact = cardcontactRep.GetSingleCardContactByContactId(instance.Id, instance.Tenant);
            if (cardContact != null)
            {
                instance.HasCardContact = true;
                instance.IsAll = cardContact.IsAll;
                instance.IsAirExport = cardContact.IsAirExport;
                instance.IsAirImport = cardContact.IsAirImport;
                instance.IsInlandExport = cardContact.IsInlandExport;
                instance.IsInlandImport = cardContact.IsInlandImport;
                instance.IsOceanExport = cardContact.IsOceanExport;
                instance.IsOceanImport = cardContact.IsOceanImport;
                instance.IsInlandDomestic = cardContact.IsInlandDomestic;
                instance.IsCustomsImport = cardContact.IsCustomsImport;
            }
        }
        private ContactPM GetEntityPMWithPassword(int tenant, Expression<Func<ContactPM, bool>> predicate)
        {
            ContactPM entity = GetContactPMQuery().Where(predicate).FirstOrDefault();
            if (entity == null)
            {
                tenant = 0;
                entity = GetContactPMQuery().Where(predicate).FirstOrDefault();
            }
            if (entity != null)
            {
                GetContactPassword(entity);
            }

            return entity;
        }



        //public ContactPM GetSinglePMFromCache(string id, int tenant)
        //{
        //    string key = $"GetSingleContactPM_({id},{tenant})";
        //    return CacheManager.GetOrInsertNewObject<ContactPM>(key, () =>
        //    {
        //        return GetSinglePM(id, tenant);
        //    });
        //}
        //public ContactPM GetSinglePMFromCacheWithSystemUser(string id, int tenant)
        //{
        //    string key = $"GetSingleContactPMWithUser_({id},{tenant})";
        //    return CacheManager.GetOrInsertNewObject<ContactPM>(key, () =>
        //    {
        //        return GetSinglePMWithSystemUser(id, tenant);
        //    });
        //}
        //public ContactPM GetSinglePMWithSystemUser(string id, int tenant) => GetEntityPMWithPassword(tenant, a => a.Id == id);  
        //public ContactPM GetSinglePM(string id, int tenant) => GetEntityPMWithPassword(tenant, a => a.Id == id && a.Tenant == tenant);  
        //public ContactPM GetContactByFacebookId(string facebookId, int tenant) => GetEntityPMWithPassword(tenant, a => a.FacebookId == facebookId && a.Tenant == tenant);
        public ContactPM GetSingleContact(string email, int tenant) =>GetEntityPMWithPassword(tenant, a => a.Email == email.ToLower() && a.Tenant == tenant); 
        //public ContactPM GetSingleContactByExternalId(string externalId, int tenant) => GetEntityPMWithPassword(tenant, a => a.ExternalId == externalId && a.Tenant == tenant); 
        public ContactPM GetContactByEmailOnly(string email, int tenant) => GetContactPMFromCache(tenant, $"ContactPM_({email}_{tenant})".ToLower(), a => a.Email == email && a.InActive == false && a.Tenant == tenant);
        //public ContactPM GetContactById(string id, int tenant) => GetContactPMFromCache(tenant, $"ContactPM_({id}_{tenant})".ToLower(), a => a.Id == id && a.Tenant == tenant && a.InActive ==false); 
        //public ContactPM GetContactByNameAndTenant(string name, int tenant, bool getFromCache)
        //{
        //    string cacheKey = $"ContactPM_({name}_{tenant})".ToLower();
        //    return getFromCache
        //        ? GetContactPMFromCache(tenant, cacheKey, a => a.Email == name.ToLower() && a.Tenant == tenant)
        //        : GetEntityPMWithPassword(tenant, a => a.Email == name.ToLower() && a.Tenant == tenant || a.Tenant == 0);
        //}
        //public ContactPM GetContactByEmailOnlyForLogin(string email, int tenant) => GetContactByEmailOnly(email, tenant);
        //public ContactPM GetSingleContactPM(string id) => GetEntityPMWithPassword(0, a => a.Id == id);
        //public ContactPM GetFirstContactByEnglishNamePM(string Name, int tenant) => GetContactPMQuery().Where(a => a.EnglishName == Name && a.Tenant == tenant).FirstOrDefault();   
        public ContactPM GetSingleByEmail(string email, int tenant) => GetSingleContact(email, tenant); 
        //public ContactPM GetSinglePMByEmail(string email, int tenant) => GetSingleContact(email, tenant);   
        //public ContactPM GetSingleByEmailWithoutTenant(string email)
        //{
        //    email = email.ToLower();
        //    ContactPM contact = (from a in context.Contacts
        //                         where a.Email == email
        //                         select new ContactPM()
        //                         {
        //                             Id = a.Id,
        //                         }).FirstOrDefault();
        //    return contact;
        //}


        #endregion GetSingle ContactPM  

        #region GetList<ContactPM>
        //public List<ContactPM> GetContactsByEmail(string email, int tenant)
        //{
        //    email = email.ToLower();
        //    List<ContactPM> contacts = (from a in context.Contacts
        //                                where a.Email == email && a.UserType == "R"
        //                                && a.Tenant == tenant
        //                                select new ContactPM()
        //                                {
        //                                    Anniversary = a.Anniversary,
        //                                    Birthday = a.Birthday,
        //                                    BusinessPhone = a.BusinessPhone,
        //                                    Email = a.Email,
        //                                    SearchFields = a.SearchFields,
        //                                    EnglishName = a.EnglishName,
        //                                    FacebookId = a.FacebookId,
        //                                    Fax = a.Fax,
        //                                    Id = a.Id,
        //                                    InActive = a.InActive,
        //                                    LocalName = a.LocalName,
        //                                    ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
        //                                    Mobile = a.Mobile,
        //                                    Notes = a.Notes,
        //                                    Tenant = a.Tenant,
        //                                    Signature = a.Signature,
        //                                    SignatureHtml = a.SignatureHtml,
        //                                    DontShowLocal = a.DontShowLocalLabels,
        //                                    DisplayGettingStarted = a.DisplayGettingStarted,
        //                                    BirthdayReminder = a.BirthdayReminder,
        //                                    AnniversaryReminder = a.AnniversaryReminder,
        //                                    ImageDetailId = a.ImageDetailId,
        //                                    DoneDate = a.DoneDate,
        //                                    BirthDayOfYear = a.BirthDayOfYear,
        //                                    ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                    Position = a.Position,
        //                                    ExternalId = a.ExternalId,
        //                                    IndexColor = a.IndexColor,
        //                                    CompanyName = a.CompanyName,
        //                                    CreateDate = a.CreateDate,
        //                                    DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                }).ToList();
        //    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //    {
        //        IGlobalContext globalContext = GlobalContext.GetContext();
        //        List<ContactPassword> contactPasswords = globalContext.ContactPasswords.Where(c => c.Email == email).ToList();

        //        foreach (var c in contacts)
        //        {
        //            ContactPassword contactPassword = contactPasswords.Where(cn => cn.Email == c.Email).FirstOrDefault();
        //            if (contactPassword != null)
        //            {
        //                c.IsLocked = contactPassword.IsLocked;
        //                c.MustChangePassword = contactPassword.MustChangePassword;
        //                c.NumberOfRetries = contactPassword.NumberOfRetries;
        //            }
        //        }
        //    }
        //    if (contacts.Count > 0)
        //    {
        //        foreach (ContactPM contact in contacts)
        //        {
        //            contact.HasCardContact = false;
        //            CardContactRepository cardcontactRep = new CardContactRepository(tenant);
        //            CardContact cardContact = cardcontactRep.GetSingleCardContactByContactId(contact.Id, contact.Tenant);
        //            if (cardContact != null)
        //            {
        //                contact.HasCardContact = true;
        //                contact.IsAll = cardContact.IsAll;
        //                contact.IsAirExport = cardContact.IsAirExport;
        //                contact.IsAirImport = cardContact.IsAirImport;
        //                contact.IsInlandExport = cardContact.IsInlandExport;
        //                contact.IsInlandImport = cardContact.IsInlandImport;
        //                contact.IsOceanExport = cardContact.IsOceanExport;
        //                contact.IsOceanImport = cardContact.IsOceanImport;
        //                contact.IsInlandDomestic = cardContact.IsInlandDomestic;
        //                contact.IsCustomsImport = cardContact.IsCustomsImport;
        //            }
        //        }
        //    }
        //    return contacts;
        //}
        //public List<ContactPM> GetContactPMsByTenant(int tenant)
        //{
        //    List<ContactPM> contacts = (from a in context.Contacts
        //                                where a.Tenant == tenant && a.UserType == "R"
        //                                select new ContactPM()
        //                                {
        //                                    Anniversary = a.Anniversary,
        //                                    Birthday = a.Birthday,
        //                                    BusinessPhone = a.BusinessPhone,
        //                                    Email = a.Email,
        //                                    EnglishName = a.EnglishName,
        //                                    FacebookId = a.FacebookId,
        //                                    Fax = a.Fax,
        //                                    Id = a.Id,
        //                                    InActive = a.InActive,
        //                                    LocalName = a.LocalName,
        //                                    SearchFields = a.SearchFields,
        //                                    Mobile = a.Mobile,
        //                                    Notes = a.Notes,
        //                                    Tenant = a.Tenant,
        //                                    Signature = a.Signature,
        //                                    SignatureHtml = a.SignatureHtml,
        //                                    ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
        //                                    DisplayGettingStarted = a.DisplayGettingStarted,
        //                                    DontShowLocal = a.DontShowLocalLabels,
        //                                    BirthdayReminder = a.BirthdayReminder,
        //                                    AnniversaryReminder = a.AnniversaryReminder,
        //                                    ImageDetailId = a.ImageDetailId,
        //                                    DoneDate = a.DoneDate,
        //                                    BirthDayOfYear = a.BirthDayOfYear,
        //                                    ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                    Position = a.Position,
        //                                    ExternalId = a.ExternalId,
        //                                    IndexColor = a.IndexColor,
        //                                    CompanyName = a.CompanyName,
        //                                    CreateDate = a.CreateDate,
        //                                    DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                }).ToList();
        //    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //    {
        //        IGlobalContext globalContext = GlobalContext.GetContext();
        //        List<ContactPassword> contactPasswords = globalContext.ContactPasswords.Where(c => contacts.Any(ct => ct.Email == c.Email)).ToList();
        //        foreach (var c in contacts)
        //        {
        //            if (c.Email != null)
        //            {
        //                ContactPassword contactPassword = contactPasswords.Where(cn => cn.Email == c.Email.ToLower()).FirstOrDefault();
        //                if (contactPassword != null)
        //                {
        //                    c.IsLocked = contactPassword.IsLocked;
        //                    c.MustChangePassword = contactPassword.MustChangePassword;
        //                    c.NumberOfRetries = contactPassword.NumberOfRetries;
        //                }
        //            }
        //        }
        //    }
        //    return contacts;
        //}
        //public List<ContactPM> GetContactPMsWithoutPassWordsByTenant(int tenant)
        //{
        //    List<ContactPM> contacts = (from a in context.Contacts
        //                                where a.Tenant == tenant && a.UserType == "R"
        //                                select new ContactPM()
        //                                {
        //                                    Anniversary = a.Anniversary,
        //                                    Birthday = a.Birthday,
        //                                    BusinessPhone = a.BusinessPhone,
        //                                    Email = a.Email,
        //                                    EnglishName = a.EnglishName,
        //                                    FacebookId = a.FacebookId,
        //                                    Fax = a.Fax,
        //                                    Id = a.Id,
        //                                    InActive = a.InActive,
        //                                    LocalName = a.LocalName,
        //                                    SearchFields = a.SearchFields,
        //                                    Mobile = a.Mobile,
        //                                    Notes = a.Notes,
        //                                    Tenant = a.Tenant,
        //                                    Signature = a.Signature,
        //                                    SignatureHtml = a.SignatureHtml,
        //                                    ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
        //                                    DisplayGettingStarted = a.DisplayGettingStarted,
        //                                    DontShowLocal = a.DontShowLocalLabels,
        //                                    BirthdayReminder = a.BirthdayReminder,
        //                                    AnniversaryReminder = a.AnniversaryReminder,
        //                                    ImageDetailId = a.ImageDetailId,
        //                                    DoneDate = a.DoneDate,
        //                                    BirthDayOfYear = a.BirthDayOfYear,
        //                                    ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                    Position = a.Position,
        //                                    ExternalId = a.ExternalId,
        //                                    IndexColor = a.IndexColor,
        //                                    CompanyName = a.CompanyName,
        //                                    CreateDate = a.CreateDate,
        //                                    DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                }).ToList(); 
        //    return contacts;
        //}

        #endregion GetList<ContactPM>

        //public List<ExtendedContactPM> GetExtendedContactPMsByTenant(int tenant)
        //{
        //    List<ExtendedContactPM> contacts = (from Contact in context.Contacts.Include("User")
        //                                        where Contact.Tenant == tenant && Contact.UserType == "R"
        //                                        join CardContact in context.CardContacts
        //                                        on Contact.Id equals CardContact.ContactId
        //                                        into CardContacts
        //                                        select new ExtendedContactPM()
        //                                        {
        //                                            Id = Contact.Id,
        //                                            Tenant = Contact.Tenant,
        //                                            EnglishName = Contact.EnglishName,
        //                                            Email = Contact.Email,
        //                                            UserRoles = Contact.User != null ? Contact.User.UserRoles : null,
        //                                            CardId = CardContacts.FirstOrDefault() != null ? CardContacts.FirstOrDefault().CardId : null
        //                                        }).ToList();
        //    return contacts;
        //}
        //public ExtendedContactPM GetSingleExtendedContactPMsByTenant(string id, int tenant)
        //{
        //    ExtendedContactPM contact = (from Contact in context.Contacts.Include("User")
        //                                 where Contact.Tenant == tenant && Contact.UserType == "R" && Contact.Id == id
        //                                 join CardContact in context.CardContacts
        //                                 on Contact.Id equals CardContact.ContactId
        //                                 into CardContacts
        //                                 select new ExtendedContactPM()
        //                                 {
        //                                     Id = Contact.Id,
        //                                     Tenant = Contact.Tenant,
        //                                     EnglishName = Contact.EnglishName,
        //                                     Email = Contact.Email,
        //                                     UserRoles = Contact.User != null ? Contact.User.UserRoles : null,
        //                                     CardId = CardContacts.FirstOrDefault() != null ? CardContacts.FirstOrDefault().CardId : null
        //                                 }).FirstOrDefault();
        //    return contact;
        //}
        //public string GetContactIdByLoggedEmail(int tenant)
        //{
        //    string email = HttpContext.Current.User.Identity.Name;
        //    return repository.GetConactIdByemail(email, tenant);
        //}
        //public IQueryable<ContactList> GetIQueryableEntityList(IQueryable<Contact> iQueryable)
        //{
        //    IQueryable<ContactList> result = from a in iQueryable
        //                                     select new ContactList()
        //                                     {
        //                                         Anniversary = a.Anniversary,
        //                                         Birthday = a.Birthday,
        //                                         BusinessPhone = a.BusinessPhone,
        //                                         Email = a.Email,
        //                                         EnglishName = a.EnglishName,
        //                                         Fax = a.Fax,
        //                                         Id = a.Id,
        //                                         InActive = a.InActive,
        //                                         LocalName = a.LocalName,
        //                                         Mobile = a.Mobile,
        //                                         Notes = a.Notes,
        //                                         Tenant = a.Tenant,
        //                                         Name = a.EnglishName,
        //                                         SearchFields = a.SearchFields,
        //                                         DontShowLocal = a.DontShowLocalLabels,
        //                                         BirthdayReminder = a.BirthdayReminder,
        //                                         AnniversaryReminder = a.AnniversaryReminder,
        //                                         ImageDetailId = a.ImageDetailId,
        //                                         DoneDate = a.DoneDate,
        //                                         BirthDayOfYear = a.BirthDayOfYear,
        //                                         ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                         Position = a.Position,
        //                                         IndexColor = a.IndexColor,
        //                                         CompanyName = a.CompanyName,
        //                                         CreateDate = a.CreateDate,
        //                                         DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                     };
        //    return result;
        //}
        //public IQueryable<ContactPM> GetContactsbyCardId(string id, int tenant)
        //{
        //    CardContactRepository cardContactRepository = new CardContactRepository(tenant);
        //    CardContactAdditionalServiceQuery cardContactAdditionalServiceQuery = new CardContactAdditionalServiceQuery(tenant);
        //    CardContactProductQuery cardContactProductQuery = new CardContactProductQuery(tenant);
        //    IQueryable<CardContact> cardContacts = cardContactRepository.GetCardContacts(tenant);
        //    IQueryable<ContactPM> contacts = from a in cardContacts
        //                                     where a.CardId == id && a.Tenant == tenant
        //                                     select new ContactPM()
        //                                     {
        //                                         Anniversary = a.Contact.Anniversary,
        //                                         Birthday = a.Contact.Birthday,
        //                                         BusinessPhone = a.Contact.BusinessPhone,
        //                                         Email = a.Contact.Email,
        //                                         EnglishName = a.Contact.EnglishName,
        //                                         FacebookId = a.Contact.FacebookId,
        //                                         DontShowLocalLabels = a.Contact.DontShowLocalLabels,
        //                                         Fax = a.Contact.Fax,
        //                                         Id = a.Contact.Id,
        //                                         InActive = a.Contact.InActive,
        //                                         LocalName = a.Contact.LocalName,
        //                                         Mobile = a.Contact.Mobile,
        //                                         Notes = a.Contact.Notes,
        //                                         Tenant = a.Contact.Tenant,
        //                                         CardId = id,
        //                                         Position = a.Contact.Position,
        //                                         IsAirExport = a.IsAirExport,
        //                                         IsAirImport = a.IsAirImport,
        //                                         IsOceanExport = a.IsOceanExport,
        //                                         IsOceanImport = a.IsOceanImport,
        //                                         IsAll = a.IsAll,
        //                                         IsInlandDomestic = a.IsInlandDomestic,
        //                                         IsCustomsImport = a.IsCustomsImport,
        //                                         IsInlandExport = a.IsInlandExport,
        //                                         IsInlandImport = a.IsInlandImport,
        //                                         ExternalId = a.Contact.ExternalId,
        //                                         IndexColor = a.Contact.IndexColor,
        //                                         CompanyName = a.Contact.CompanyName,
        //                                         CreateDate = a.Contact.CreateDate,
        //                                         ContactForAccounting = a.Contact.ContactForAccounting,
        //                                     };
        //    List<ContactPM> entityList = contacts.ToList();
        //    foreach (ContactPM contact in entityList)
        //    {
        //        CardContact cardContact = cardContacts.Where(d => d.CardId == id && d.ContactId == contact.Id).FirstOrDefault();
        //        if (cardContact != null)
        //        {
        //            contact.CardContactAdditionalServices = cardContactAdditionalServiceQuery.GetCardContactAdditionalServicePMsByCardContactId(cardContact.Id, tenant);
        //            contact.CardContactProducts = cardContactProductQuery.GetCardContactProductPMsByCardContactId(cardContact.Id, tenant);
        //        }
        //    }
        //    return entityList.AsQueryable();
        //}
        //public IQueryable<ContactList> GetContactListsbyCardId(string id, int tenant)
        //{
        //    CardContactRepository cardContactRepository = new CardContactRepository(tenant);
        //    var contacts = cardContactRepository.GetCardContacts(tenant).Where(c => c.CardId == id && c.ContactId == c.ContactId && c.Tenant == tenant).Select(a => new ContactList
        //    {
        //        Anniversary = a.Contact.Anniversary,
        //        Birthday = a.Contact.Birthday,
        //        BusinessPhone = a.Contact.BusinessPhone,
        //        Email = a.Contact.Email,
        //        EnglishName = a.Contact.EnglishName,
        //        Fax = a.Contact.Fax,
        //        Id = a.ContactId,
        //        InActive = a.Contact.InActive,
        //        LocalName = a.Contact.LocalName,
        //        Mobile = a.Contact.Mobile,
        //        Notes = a.Contact.Notes,
        //        Tenant = a.Contact.Tenant,
        //        Name = a.Contact.EnglishName,
        //        SearchFields = a.Contact.SearchFields,
        //        DontShowLocal = a.Contact.DontShowLocalLabels,
        //        BirthdayReminder = a.Contact.BirthdayReminder,
        //        AnniversaryReminder = a.Contact.AnniversaryReminder,
        //        ImageDetailId = a.Contact.ImageDetailId,
        //        DoneDate = a.Contact.DoneDate,
        //        BirthDayOfYear = a.Contact.BirthDayOfYear,
        //        ContactDoneMethodCode = a.Contact.ContactDoneMethod != null ? a.Contact.ContactDoneMethod.Code : null,
        //        Position = a.Contact.Position,
        //        IndexColor = a.Contact.IndexColor,
        //        CompanyName = a.Contact.CompanyName,
        //        CreateDate = a.Contact.CreateDate,
        //    });
        //    return contacts;
        //}
        //public IQueryable<ContactList> GetContactLists(int tenant)
        //{
        //    ContactRepository contactRep = new ContactRepository(tenant);
        //    IQueryable<Contact> contacts = contactRep.GetActiveContacts(tenant);
        //    IQueryable<ContactList> contactLists = (from a in contacts
        //                                            select new ContactList()
        //                                            {
        //                                                Anniversary = a.Anniversary,
        //                                                Birthday = a.Birthday,
        //                                                BusinessPhone = a.BusinessPhone,
        //                                                Email = a.Email,
        //                                                EnglishName = a.EnglishName,
        //                                                Fax = a.Fax,
        //                                                Id = a.Id,
        //                                                InActive = a.InActive,
        //                                                LocalName = a.LocalName,
        //                                                Mobile = a.Mobile,
        //                                                Notes = a.Notes,
        //                                                Tenant = a.Tenant,
        //                                                Name = a.EnglishName,
        //                                                SearchFields = a.SearchFields,
        //                                                DontShowLocal = a.DontShowLocalLabels,
        //                                                BirthdayReminder = a.BirthdayReminder,
        //                                                AnniversaryReminder = a.AnniversaryReminder,
        //                                                ImageDetailId = a.ImageDetailId,
        //                                                DoneDate = a.DoneDate,
        //                                                BirthDayOfYear = a.BirthDayOfYear,
        //                                                ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                                Position = a.Position,
        //                                                IndexColor = a.IndexColor,
        //                                                CompanyName = a.CompanyName,
        //                                                CreateDate = a.CreateDate,
        //                                                DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                            });
        //    return contactLists;
        //}
        //public IQueryable<ContactList> GetContactListsFollowShipment(List<string> trackedIds, int tenant)
        //{
        //    IQueryable<Contact> contacts = repository.GetContactsByIds(trackedIds, tenant);
        //    IQueryable<ContactList> contactLists = (from a in contacts
        //                                            where trackedIds.Contains(a.Id)
        //                                            select new ContactList()
        //                                            {
        //                                                Anniversary = a.Anniversary,
        //                                                Birthday = a.Birthday,
        //                                                BusinessPhone = a.BusinessPhone,
        //                                                Email = a.Email,
        //                                                EnglishName = a.EnglishName,
        //                                                Fax = a.Fax,
        //                                                Id = a.Id,
        //                                                InActive = a.InActive,
        //                                                LocalName = a.LocalName,
        //                                                Mobile = a.Mobile,
        //                                                Notes = a.Notes,
        //                                                Tenant = a.Tenant,
        //                                                Name = a.EnglishName,
        //                                                SearchFields = a.SearchFields,
        //                                                DontShowLocal = a.DontShowLocalLabels,
        //                                                BirthdayReminder = a.BirthdayReminder,
        //                                                AnniversaryReminder = a.AnniversaryReminder,
        //                                                ImageDetailId = a.ImageDetailId,
        //                                                DoneDate = a.DoneDate,
        //                                                BirthDayOfYear = a.BirthDayOfYear,
        //                                                ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                                Position = a.Position,
        //                                                IndexColor = a.IndexColor,
        //                                                CompanyName = a.CompanyName,
        //                                                CreateDate = a.CreateDate,
        //                                                DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                            });
        //    return contactLists;
        //}
        //public List<string> GetContactEmailsListsByIds(List<string> contactIds, int tenant)
        //{
        //    IQueryable<Contact> contacts = repository.GetContactsByIds(contactIds, tenant);
        //    List<string> contactEmailLists = (from a in contacts
        //                                      where contactIds.Contains(a.Id) && !a.InActive && a.UserType == "R"
        //                                      select a.Email).ToList();
        //    return contactEmailLists;
        //}

        #region GetList<ContactList>
        //public List<ContactList> GetContactListsByIds(List<string> trackedIds, int tenant)
        //{
        //    IQueryable<Contact> contacts = repository.GetContactsByIds(trackedIds, tenant);
        //    List<ContactList> contactLists = (from a in contacts
        //                                      where trackedIds.Contains(a.Id)
        //                                      select new ContactList()
        //                                      {

        //                                          Anniversary = a.Anniversary,
        //                                          Birthday = a.Birthday,
        //                                          BusinessPhone = a.BusinessPhone,
        //                                          Email = a.Email,
        //                                          EnglishName = a.EnglishName,
        //                                          Fax = a.Fax,
        //                                          Id = a.Id,
        //                                          InActive = a.InActive,
        //                                          LocalName = a.LocalName,
        //                                          Mobile = a.Mobile,
        //                                          Notes = a.Notes,
        //                                          Tenant = a.Tenant,
        //                                          Name = a.EnglishName,
        //                                          SearchFields = a.SearchFields,
        //                                          DontShowLocal = a.DontShowLocalLabels,
        //                                          BirthdayReminder = a.BirthdayReminder,
        //                                          AnniversaryReminder = a.AnniversaryReminder,
        //                                          ImageDetailId = a.ImageDetailId,
        //                                          DoneDate = a.DoneDate,
        //                                          BirthDayOfYear = a.BirthDayOfYear,
        //                                          ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
        //                                          Position = a.Position,
        //                                          IndexColor = a.IndexColor,
        //                                          CompanyName = a.CompanyName,
        //                                          CreateDate = a.CreateDate,
        //                                          DigitalPortalLanguage = a.DigitalPortalLanguage
        //                                      }).ToList();
        //    return contactLists;
        //}
        //public List<ContactList> GetContactListsByEmailsString(string emails, int tenant)
        //{
        //    List<ContactList> contacts = new List<ContactList>();
        //    List<string> emailsList = emails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        //    if (emailsList.Count() > 0)
        //    {
        //        ContactPM mycontact = null;
        //        foreach (var item in emailsList)
        //        {
        //            mycontact = GetSingleContact(item, tenant);
        //            if (mycontact != null)
        //            {
        //                contacts.Add(new ContactList()
        //                {
        //                    Email = mycontact.Email,
        //                    EnglishName = mycontact.EnglishName,
        //                    SearchFields = mycontact.SearchFields,
        //                });
        //            }
        //            else
        //            {
        //                contacts.Add(new ContactList()
        //                {
        //                    Email = item,
        //                    EnglishName = item,
        //                    SearchFields = item,
        //                });
        //            }
        //        }
        //    }
        //    return contacts;
        //}
        //public List<ContactList> GetContactUserListsByTenants(List<int> tenants, bool includeInactiveUsers)
        //{
        //    List<ContactList> result = new List<ContactList>();
        //    List<string> userIds = (from a in context.Users
        //                            where tenants.Contains(a.Tenant)
        //                            select a.Id).ToList();
        //    IQueryable<ContactList> contactLists = GetContactListsByListIdsForTenantReport(userIds);
        //    if (!includeInactiveUsers)
        //    {
        //        contactLists = contactLists.Where(d => !d.InActive);
        //    }
        //    result = contactLists.ToList();
        //    List<string> contactIds = result.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();
        //    UserLastLoginRepository rep = new UserLastLoginRepository(this.context);
        //    UserLastLoginQuery query = new UserLastLoginQuery(rep);
        //    List<UserLastLoginPM> userLastLoginPMLists = query.GetUserLastLoginPMsByUserIds(contactIds).ToList();
        //    foreach (ContactList contact in result)
        //    {
        //        UserLastLoginPM userLastLoginPM = userLastLoginPMLists.Where(d => d.Id == contact.Id).FirstOrDefault();
        //        if (userLastLoginPM != null)
        //        {
        //            contact.LastLoginDate = userLastLoginPM.LoginDateTime;
        //        }
        //    }
        //    return result;
        //}
        //public List<ContactList> GetContactListsByEmailLists(List<string> emails, int tenant)
        //{
        //    List<ContactList> contactLists = (from a in context.Contacts
        //                                      where emails.Contains(a.Email) && a.Tenant == tenant
        //                                      select new ContactList()
        //                                      {
        //                                          Id = a.Id,
        //                                          Tenant = a.Tenant,
        //                                          EnglishName = a.EnglishName,
        //                                          LocalName = a.LocalName,
        //                                          Mobile = a.Mobile,
        //                                          Fax = a.Fax,
        //                                          BusinessPhone = a.BusinessPhone,
        //                                          Email = a.Email,
        //                                      }).ToList();
        //    return contactLists;
        //}


        #endregion GetList<ContactList>

        //public string GetContactEmailById(string id, int tenant)
        //{
        //    string email = (from a in context.Contacts
        //                    where a.Id == id
        //                    && a.Tenant == tenant
        //                    select new ContactPM()
        //                    {
        //                        Id = a.Id,
        //                        Tenant = a.Tenant,
        //                        Email = a.Email,
        //                    }.Email).FirstOrDefault();
        //    return email;
        //}
        //public byte[] GetSignatureHtmlByContactId(string contactId, int tenant)
        //{
        //    byte[] result = null;
        //    Contact contact = context.Contacts.Where(d => d.Id == contactId).FirstOrDefault();
        //    if (contact != null)
        //    {
        //        result = contact.SignatureHtml;
        //    }
        //    return result;
        //}
        public IQueryable<ContactList> GetContactListsByListIds(List<string> contactIds, int tenant)
        {
            IQueryable<ContactList> contactLists = (from a in context.Contacts
                                                    where contactIds.Contains(a.Id) &&  a.Tenant == tenant
                                                    select new ContactList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        EnglishName = a.EnglishName,
                                                        LocalName = a.LocalName,
                                                        Mobile = a.Mobile,
                                                        Fax = a.Fax,
                                                        BusinessPhone = a.BusinessPhone,
                                                        InActive = a.InActive,
                                                    });
            return contactLists;
       }
        //public ContactList GetContactListsById(string contactId, int tenant)
        //{
        //    ContactList contactList = (from a in context.Contacts
        //                               where a.Id == contactId && a.Tenant == tenant
        //                               select new ContactList()
        //                               {
        //                                   Id = a.Id,
        //                                   Tenant = a.Tenant,
        //                                   EnglishName = a.EnglishName,
        //                                   LocalName = a.LocalName,
        //                                   Mobile = a.Mobile,
        //                                   Fax = a.Fax,
        //                                   BusinessPhone = a.BusinessPhone,
        //                               }).FirstOrDefault();
        //    return contactList;
        //}
        //public string GetContactNameId(string id, int tenant)
        //{
        //    string result = null;
        //    Contact contact = context.Contacts.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();
        //    if (contact != null)
        //    {
        //        result = contact.EnglishName;
        //    }
        //    return result;
        //}
        //public IQueryable<ContactList> GetContactListsByListIdsForTenantReport(List<string> contactIds)
        //{
        //    IQueryable<ContactList> contactLists = (from a in context.Contacts
        //                                            where contactIds.Contains(a.Id) && a.UserType == "R"
        //                                            select new ContactList()
        //                                            {
        //                                                Id = a.Id,
        //                                                Tenant = a.Tenant,
        //                                                EnglishName = a.EnglishName,
        //                                                InActive = a.InActive,
        //                                                Email = a.Email,
        //                                           });
        //    return contactLists;
        //}
        //public IQueryable<ContactList> GetDemoTenantContactList(IQueryable<Contact> iQueryable, string loggedUserId, int tenant)
        //{
        //    List<ContactList> result = new List<ContactList>();
        //    int index = 1;
        //    foreach (Contact contact in iQueryable)
        //    {
        //        string email = contact.Email;
        //        string name = contact.EnglishName;
        //        if (contact.Id != loggedUserId)
        //        {
        //            if (!string.IsNullOrEmpty(email))
        //            {
        //                string[] emailParts = contact.Email.Split('@');
        //                email = "contact" + index + "@democompany.com ";
        //            }
        //            name = "Contact" + index;
        //        }
        //        ContactList newItem = new ContactList()
        //        {
        //            Email = email,
        //            EnglishName = name,
        //            Id = contact.Id,
        //            Anniversary = contact.Anniversary,
        //            Birthday = contact.Birthday,
        //            BusinessPhone = contact.BusinessPhone,
        //            SearchFields = contact.SearchFields,
        //            Fax = contact.Fax,
        //            InActive = contact.InActive,
        //            LocalName = contact.LocalName,
        //            Mobile = contact.Mobile,
        //            Notes = contact.Notes,
        //            Tenant = contact.Tenant,
        //            DontShowLocal = contact.DontShowLocalLabels,
        //            DisplayGettingStarted = contact.DisplayGettingStarted,
        //            BirthdayReminder = contact.BirthdayReminder,
        //            AnniversaryReminder = contact.AnniversaryReminder,
        //            ImageDetailId = contact.ImageDetailId,
        //            DoneDate = contact.DoneDate,
        //            BirthDayOfYear = contact.BirthDayOfYear,
        //            ContactDoneMethodCode = contact.ContactDoneMethod != null ? contact.ContactDoneMethod.Code : null,
        //            Position = contact.Position,
        //            IndexColor = contact.IndexColor,
        //            CompanyName = contact.CompanyName,
        //            CreateDate = contact.CreateDate,
        //            DigitalPortalLanguage = contact.DigitalPortalLanguage
        //        };
        //        result.Add(newItem);
        //        index++;
        //    }

        //    return result.AsQueryable();
        //}
        //public string GetContactIdByEmail(string email,int tenant)
        //{
        //    string id = repository.GetConactIdByemail(email, tenant);
        //    return id;
        //}
        //public IQueryable<ContactPM> GetContactsbyCustomerId(string id, int tenant)
        //{
        //     IQueryable<ContactPM> contacts = from a in context.CardContacts.Include("Contact")
        //                                     where a.CardId == id && a.Tenant == tenant
        //                                     select new ContactPM()
        //                                     {
        //                                         BusinessPhone = a.Contact.BusinessPhone,
        //                                         Email = a.Contact.Email,
        //                                         EnglishName = a.Contact.EnglishName,
        //                                         Fax = a.Contact.Fax,
        //                                         Id = a.Contact.Id,
        //                                         InActive = a.Contact.InActive,
        //                                         LocalName = a.Contact.LocalName,
        //                                         Mobile = a.Contact.Mobile,
        //                                         Notes = a.Contact.Notes,
        //                                         Tenant = a.Contact.Tenant,
        //                                         CardId = id,
        //                                         Position = a.Contact.Position,
        //                                         CreateDate = a.Contact.CreateDate
        //                                     };
        //    return contacts;
        //}
    }
}