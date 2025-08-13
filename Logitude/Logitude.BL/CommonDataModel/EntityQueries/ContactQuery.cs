using System;
using System.Web;
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ContactQuery
    {
        ContactRepository repository;

        public ContactQuery()
        {
            repository = new ContactRepository();
        }

        public ContactQuery(int tenant)
        {
            repository = new ContactRepository(tenant);
        }

        public ContactQuery(ContactRepository contactRepository)
        {
            repository = contactRepository;
        }
        public ContactPM GetSinglePMFromCache(string id, int tenant)
        {
            string key = $"GetSinglePMFromCache({id},{tenant})";
            return CacheManager.GetOrInsertNewObject<ContactPM>(key, () =>
            {
                return GetSinglePM(id, tenant);
            });

        }
        public ContactPM GetSinglePMFromCacheWithSystemUser(string id, int tenant)
        {
            string key = $"GetSinglePMFromCache({id},{tenant})";
            return CacheManager.GetOrInsertNewObject<ContactPM>(key, () =>
            {
                return GetSinglePMWithSystemUser(id, tenant);
            });

        }
        public ContactPM GetSinglePMWithSystemUser(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ContactPM instance = (from a in repository.context.Contacts
                                      where a.Tenant == tenant  
                                      && a.Id == id
                                      select new ContactPM()
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
                                          DigitalPortalLanguage = a.DigitalPortalLanguage
                                      }).FirstOrDefault();

                if (instance != null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == instance.Email.ToLower()).FirstOrDefault();
                        GlobalContact globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == instance.Email.ToLower() && cn.GlobalTenantId == tenant).FirstOrDefault();

                        if (globalContact != null)
                        {
                            instance.IsUser = globalContact.IsUser;
                        }

                        if (contactPassword != null)
                        {
                            instance.Password = contactPassword.Password;
                            instance.IsLocked = contactPassword.IsLocked;
                            instance.MustChangePassword = contactPassword.MustChangePassword;
                            instance.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }

                    instance.HasCardContact = false;
                    CardContactRepository cardcontactRep = new CardContactRepository(tenant);
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

                return instance;
            }

            return null;
        }
        public ContactPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                ContactPM instance = repository.context
                                               .Contacts
                                               .Where(a => a.Tenant == tenant
                                                           && a.UserType.Equals("R", StringComparison.InvariantCultureIgnoreCase)
                                                           && a.Id == id)
                                               .Select( a => new ContactPM()
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
                                                   DigitalPortalLanguage = a.DigitalPortalLanguage
                                               })
                                               .FirstOrDefault();

                if (instance != null)
                {
                    TenantRepository tenantRepository = new TenantRepository(tenant);

                    var tenantInfo = tenantRepository.GetSingleTenantWithOutIncluded(tenant);

                    var tenantAddress = tenantInfo.Address != null ? tenantInfo.Address.City : "";

                    instance.TimeZone = $"(UTC {tenantInfo.TimeZoneOffset}:00) {tenantAddress}";
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == instance.Email.ToLower()).FirstOrDefault();
                        GlobalContact globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == instance.Email.ToLower() && cn.GlobalTenantId == tenant).FirstOrDefault();

                        if (globalContact != null)
                        {
                            instance.IsUser = globalContact.IsUser;
                        }

                        if (contactPassword != null)
                        {
                            instance.Password = contactPassword.Password;
                            instance.IsLocked = contactPassword.IsLocked;
                            instance.MustChangePassword = contactPassword.MustChangePassword;
                            instance.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }

                    instance.HasCardContact = false;
                    CardContactRepository cardcontactRep = new CardContactRepository(tenant);
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

                return instance;
            }

            return null;
        }

        public List<ContactPM> GetContactPMsByTenant(int tenant)
        {
            List<ContactPM> contacts = (from a in repository.context.Contacts
                                        where a.Tenant == tenant && a.UserType == "R"
                                        select new ContactPM()
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
                                            Mobile = a.Mobile,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            Signature = a.Signature,
                                            SignatureHtml = a.SignatureHtml,
                                            ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                            DisplayGettingStarted = a.DisplayGettingStarted,
                                            DontShowLocal = a.DontShowLocalLabels,
                                            BirthdayReminder = a.BirthdayReminder,
                                            AnniversaryReminder = a.AnniversaryReminder,
                                            ImageDetailId = a.ImageDetailId,
                                            DoneDate = a.DoneDate,
                                            BirthDayOfYear = a.BirthDayOfYear,
                                            ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                            Position = a.Position,
                                            ExternalId = a.ExternalId,
                                            IndexColor = a.IndexColor,
                                            CompanyName = a.CompanyName,
                                            CreateDate = a.CreateDate,
                                            DigitalPortalLanguage = a.DigitalPortalLanguage
                                        }).ToList();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                List<ContactPassword> contactPasswords = globalContext.ContactPasswords.Where(c => contacts.Any(ct => ct.Email == c.Email)).ToList();

                foreach (var c in contacts)
                {
                    if (c.Email != null)
                    {
                        ContactPassword contactPassword = contactPasswords.Where(cn => cn.Email == c.Email.ToLower()).FirstOrDefault();
                        if (contactPassword != null)
                        {
                            c.IsLocked = contactPassword.IsLocked;
                            c.MustChangePassword = contactPassword.MustChangePassword;
                            c.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }
                }
            }

            return contacts;
        }

        public List<ContactPM> GetContactPMsWithoutPassWordsByTenant(int tenant)
        {
            List<ContactPM> contacts = (from a in repository.context.Contacts
                                        where a.Tenant == tenant && a.UserType == "R"
                                        select new ContactPM()
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
                                            Mobile = a.Mobile,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            Signature = a.Signature,
                                            SignatureHtml = a.SignatureHtml,
                                            ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                            DisplayGettingStarted = a.DisplayGettingStarted,
                                            DontShowLocal = a.DontShowLocalLabels,
                                            BirthdayReminder = a.BirthdayReminder,
                                            AnniversaryReminder = a.AnniversaryReminder,
                                            ImageDetailId = a.ImageDetailId,
                                            DoneDate = a.DoneDate,
                                            BirthDayOfYear = a.BirthDayOfYear,
                                            ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                            Position = a.Position,
                                            ExternalId = a.ExternalId,
                                            IndexColor = a.IndexColor,
                                            CompanyName = a.CompanyName,
                                            CreateDate = a.CreateDate,
                                            DigitalPortalLanguage = a.DigitalPortalLanguage
                                        }).ToList(); 

            return contacts;
        }

        public List<ExtendedContactPM> GetExtendedContactPMsByTenant(int tenant)
        {
            List<ExtendedContactPM> contacts = (from Contact in repository.context.Contacts.Include("User")
                                                where Contact.Tenant == tenant && Contact.UserType == "R"
                                                join CardContact in repository.context.CardContacts
                                                on Contact.Id equals CardContact.ContactId
                                                into CardContacts
                                                select new ExtendedContactPM()
                                                {
                                                    Id = Contact.Id,
                                                    Tenant = Contact.Tenant,
                                                    EnglishName = Contact.EnglishName,
                                                    Email = Contact.Email,
                                                    UserRoles = Contact.User != null ? Contact.User.UserRoles : null,
                                                    CardId = CardContacts.FirstOrDefault() != null ? CardContacts.FirstOrDefault().CardId : null
                                                }).ToList();

            return contacts;
        }

        public ExtendedContactPM GetSingleExtendedContactPMsByTenant(string id, int tenant)
        {
            ExtendedContactPM contact = (from Contact in repository.context.Contacts.Include("User")
                                         where Contact.Tenant == tenant && Contact.UserType == "R" && Contact.Id == id
                                         join CardContact in repository.context.CardContacts
                                         on Contact.Id equals CardContact.ContactId
                                         into CardContacts
                                         select new ExtendedContactPM()
                                         {
                                             Id = Contact.Id,
                                             Tenant = Contact.Tenant,
                                             EnglishName = Contact.EnglishName,
                                             Email = Contact.Email,
                                             UserRoles = Contact.User != null ? Contact.User.UserRoles : null,
                                             CardId = CardContacts.FirstOrDefault() != null ? CardContacts.FirstOrDefault().CardId : null
                                         }).FirstOrDefault();

            return contact;
        }

        public List<ContactPM> GetContactsByEmail(string email, int tenant)
        {
            email = email.ToLower();
            List<ContactPM> contacts = (from a in repository.context.Contacts
                                        where a.Email == email && a.UserType == "R"
                                        && a.Tenant == tenant
                                        select new ContactPM()
                                        {
                                            Anniversary = a.Anniversary,
                                            Birthday = a.Birthday,
                                            BusinessPhone = a.BusinessPhone,
                                            Email = a.Email,
                                            SearchFields = a.SearchFields,
                                            EnglishName = a.EnglishName,
                                            FacebookId = a.FacebookId,
                                            Fax = a.Fax,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            LocalName = a.LocalName,
                                            ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                            Mobile = a.Mobile,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            Signature = a.Signature,
                                            SignatureHtml = a.SignatureHtml,
                                            DontShowLocal = a.DontShowLocalLabels,
                                            DisplayGettingStarted = a.DisplayGettingStarted,
                                            BirthdayReminder = a.BirthdayReminder,
                                            AnniversaryReminder = a.AnniversaryReminder,
                                            ImageDetailId = a.ImageDetailId,
                                            DoneDate = a.DoneDate,
                                            BirthDayOfYear = a.BirthDayOfYear,
                                            ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                            Position = a.Position,
                                            ExternalId = a.ExternalId,
                                            IndexColor = a.IndexColor,
                                            CompanyName = a.CompanyName,
                                            CreateDate = a.CreateDate,
                                            DigitalPortalLanguage = a.DigitalPortalLanguage
                                        }).ToList();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                List<ContactPassword> contactPasswords = globalContext.ContactPasswords.Where(c => c.Email == email).ToList();

                foreach (var c in contacts)
                {
                    ContactPassword contactPassword = contactPasswords.Where(cn => cn.Email == c.Email).FirstOrDefault();
                    if (contactPassword != null)
                    {
                        c.IsLocked = contactPassword.IsLocked;
                        c.MustChangePassword = contactPassword.MustChangePassword;
                        c.NumberOfRetries = contactPassword.NumberOfRetries;
                    }
                }
            }

            if (contacts.Count > 0)
            {
                foreach (ContactPM contact in contacts)
                {
                    contact.HasCardContact = false;
                    CardContactRepository cardcontactRep = new CardContactRepository(tenant);
                    CardContact cardContact = cardcontactRep.GetSingleCardContactByContactId(contact.Id, contact.Tenant);
                    if (cardContact != null)
                    {
                        contact.HasCardContact = true;
                        contact.IsAll = cardContact.IsAll;
                        contact.IsAirExport = cardContact.IsAirExport;
                        contact.IsAirImport = cardContact.IsAirImport;
                        contact.IsInlandExport = cardContact.IsInlandExport;
                        contact.IsInlandImport = cardContact.IsInlandImport;
                        contact.IsOceanExport = cardContact.IsOceanExport;
                        contact.IsOceanImport = cardContact.IsOceanImport;
                        contact.IsInlandDomestic = cardContact.IsInlandDomestic;
                        contact.IsCustomsImport = cardContact.IsCustomsImport;
                    }
                }
            }

            return contacts;
        }

        public ContactPM GetContactByFacebookId(string facebookId, int tenant)
        {
            ContactPM contact = (from a in repository.context.Contacts
                                 where a.FacebookId == facebookId && a.UserType == "R"
                                 select new ContactPM()
                                 {
                                     Anniversary = a.Anniversary,
                                     Birthday = a.Birthday,
                                     BusinessPhone = a.BusinessPhone,
                                     Email = a.Email,
                                     SearchFields = a.SearchFields,
                                     EnglishName = a.EnglishName,
                                     FacebookId = a.FacebookId,
                                     Fax = a.Fax,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     Mobile = a.Mobile,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     Signature = a.Signature,
                                     SignatureHtml = a.SignatureHtml,
                                     DontShowLocal = a.DontShowLocalLabels,
                                     DisplayGettingStarted = a.DisplayGettingStarted,
                                     BirthdayReminder = a.BirthdayReminder,
                                     AnniversaryReminder = a.AnniversaryReminder,
                                     ImageDetailId = a.ImageDetailId,
                                     DoneDate = a.DoneDate,
                                     BirthDayOfYear = a.BirthDayOfYear,
                                     ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                     Position = a.Position,
                                     ExternalId = a.ExternalId,
                                     IndexColor = a.IndexColor,
                                     CompanyName = a.CompanyName,
                                     CreateDate = a.CreateDate,
                                     DigitalPortalLanguage = a.DigitalPortalLanguage
                                 }).FirstOrDefault();

            if (contact != null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email.ToLower()).FirstOrDefault();

                    if (contactPassword != null)
                    {
                        contact.IsLocked = contactPassword.IsLocked;
                        contact.MustChangePassword = contactPassword.MustChangePassword;
                        contact.NumberOfRetries = contactPassword.NumberOfRetries;
                    }
                }
            }

            return contact;
        }

        public ContactPM GetSingleContact(string email, int tenant)
        {
            email = email.ToLower();
            ContactPM contact = (from a in repository.context.Contacts
                                 where a.Email == email && a.UserType == "R"
                                 && a.Tenant == tenant
                                 select new ContactPM()
                                 {
                                     Anniversary = a.Anniversary,
                                     Birthday = a.Birthday,
                                     BusinessPhone = a.BusinessPhone,
                                     Email = a.Email,
                                     SearchFields = a.SearchFields,
                                     EnglishName = a.EnglishName,
                                     FacebookId = a.FacebookId,
                                     Fax = a.Fax,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     Mobile = a.Mobile,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     Signature = a.Signature,
                                     SignatureHtml = a.SignatureHtml,
                                     DontShowLocal = a.DontShowLocalLabels,
                                     DisplayGettingStarted = a.DisplayGettingStarted,
                                     BirthdayReminder = a.BirthdayReminder,
                                     AnniversaryReminder = a.AnniversaryReminder,
                                     ImageDetailId = a.ImageDetailId,
                                     DoneDate = a.DoneDate,
                                     BirthDayOfYear = a.BirthDayOfYear,
                                     ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                     Position = a.Position,
                                     ExternalId = a.ExternalId,
                                     IndexColor = a.IndexColor,
                                     CompanyName = a.CompanyName,
                                     CreateDate = a.CreateDate,
                                     DigitalPortalLanguage = a.DigitalPortalLanguage
                                 }).FirstOrDefault();

            if (contact != null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email).FirstOrDefault();

                    if (contactPassword != null)
                    {
                        contact.IsLocked = contactPassword.IsLocked;
                        contact.MustChangePassword = contactPassword.MustChangePassword;
                        contact.NumberOfRetries = contactPassword.NumberOfRetries;
                    }
                }
            }

            return contact;
        }

        public ContactPM GetSingleContactByExternalId(string externalId, int tenant)
        {
            ContactPM contact = (from a in repository.context.Contacts
                                 where a.ExternalId == externalId && a.UserType == "R"
                                 && a.Tenant == tenant
                                 select new ContactPM()
                                 {
                                     Anniversary = a.Anniversary,
                                     Birthday = a.Birthday,
                                     BusinessPhone = a.BusinessPhone,
                                     Email = a.Email,
                                     SearchFields = a.SearchFields,
                                     EnglishName = a.EnglishName,
                                     FacebookId = a.FacebookId,
                                     Fax = a.Fax,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     Mobile = a.Mobile,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     Signature = a.Signature,
                                     SignatureHtml = a.SignatureHtml,
                                     DontShowLocal = a.DontShowLocalLabels,
                                     DisplayGettingStarted = a.DisplayGettingStarted,
                                     BirthdayReminder = a.BirthdayReminder,
                                     AnniversaryReminder = a.AnniversaryReminder,
                                     ImageDetailId = a.ImageDetailId,
                                     DoneDate = a.DoneDate,
                                     BirthDayOfYear = a.BirthDayOfYear,
                                     ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                     Position = a.Position,
                                     ExternalId = a.ExternalId,
                                     IndexColor = a.IndexColor,
                                     CompanyName = a.CompanyName,
                                     CreateDate = a.CreateDate,
                                     DigitalPortalLanguage = a.DigitalPortalLanguage
                                 }).FirstOrDefault();

            if (contact != null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email).FirstOrDefault();

                    if (contactPassword != null)
                    {
                        contact.IsLocked = contactPassword.IsLocked;
                        contact.MustChangePassword = contactPassword.MustChangePassword;
                        contact.NumberOfRetries = contactPassword.NumberOfRetries;
                    }
                }
            }

            return contact;
        }

        public ContactPM GetContactByEmailOnly(string email, int tenant)
        {
            email = email.ToLower();
            string cacheKey = $"ContactPM_{email}_{tenant}";
            ContactPM entity;
            if (HttpContext.Current != null)
            {
                entity = (ContactPM)CacheManager.CacheWrapper.Get(cacheKey);
                if (entity == null)
                {
                    bool isTenant0User = false;
                    ContactPM contact = (from a in repository.context.Contacts
                                         where a.Email == email && a.InActive == false //  all tracing classes call this method with the system user
                                         && a.Tenant == tenant
                                         select new ContactPM()
                                         {
                                             Anniversary = a.Anniversary,
                                             Birthday = a.Birthday,
                                             BusinessPhone = a.BusinessPhone,
                                             Email = a.Email,
                                             SearchFields = a.SearchFields,
                                             EnglishName = a.EnglishName,
                                             FacebookId = a.FacebookId,
                                             Fax = a.Fax,
                                             Id = a.Id,
                                             InActive = a.InActive,
                                             LocalName = a.LocalName,
                                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                             Mobile = a.Mobile,
                                             Notes = a.Notes,
                                             Tenant = a.Tenant,
                                             Signature = a.Signature,
                                             SignatureHtml = a.SignatureHtml,
                                             DontShowLocal = a.DontShowLocalLabels,
                                             DisplayGettingStarted = a.DisplayGettingStarted,
                                             BirthdayReminder = a.BirthdayReminder,
                                             AnniversaryReminder = a.AnniversaryReminder,
                                             ImageDetailId = a.ImageDetailId,
                                             DoneDate = a.DoneDate,
                                             BirthDayOfYear = a.BirthDayOfYear,
                                             ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                             Position = a.Position,
                                             ExternalId = a.ExternalId,
                                             IndexColor = a.IndexColor,
                                             CompanyName = a.CompanyName,
                                             CreateDate = a.CreateDate,
                                         }).FirstOrDefault();

                    if (contact == null)
                    {
                        contact = (from a in repository.context.Contacts
                                   where a.Email == email
                                   && a.Tenant == 0 && a.InActive == false
                                   select new ContactPM()
                                   {
                                       Anniversary = a.Anniversary,
                                       Birthday = a.Birthday,
                                       BusinessPhone = a.BusinessPhone,
                                       Email = a.Email,
                                       SearchFields = a.SearchFields,
                                       EnglishName = a.EnglishName,
                                       FacebookId = a.FacebookId,
                                       Fax = a.Fax,
                                       Id = a.Id,
                                       InActive = a.InActive,
                                       LocalName = a.LocalName,
                                       ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                       Mobile = a.Mobile,
                                       Notes = a.Notes,
                                       Tenant = a.Tenant,
                                       Signature = a.Signature,
                                       SignatureHtml = a.SignatureHtml,
                                       DontShowLocal = a.DontShowLocalLabels,
                                       DisplayGettingStarted = a.DisplayGettingStarted,
                                       BirthdayReminder = a.BirthdayReminder,
                                       AnniversaryReminder = a.AnniversaryReminder,
                                       ImageDetailId = a.ImageDetailId,
                                       DoneDate = a.DoneDate,
                                       BirthDayOfYear = a.BirthDayOfYear,
                                       ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                       Position = a.Position,
                                       ExternalId = a.ExternalId,
                                       IndexColor = a.IndexColor,
                                       CompanyName = a.CompanyName,
                                       CreateDate = a.CreateDate,
                                       DigitalPortalLanguage = a.DigitalPortalLanguage
                                   }).FirstOrDefault();

                        isTenant0User = true;
                    }

                    if (contact != null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();
                            ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email.ToLower()).FirstOrDefault();
                            GlobalContact globalContact = null;

                            if (isTenant0User)
                            {
                                globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == contact.Email.ToLower() && cn.GlobalTenantId == 0).FirstOrDefault();
                            }

                            else
                            {
                                globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == contact.Email.ToLower() && cn.GlobalTenantId == tenant).FirstOrDefault();
                            }

                            if (contactPassword != null)
                            {
                                contact.IsUser = globalContact != null ? globalContact.IsUser: false;
                                contact.HasPassword = true;
                                contact.IsLocked = contactPassword.IsLocked;
                                contact.MustChangePassword = contactPassword.MustChangePassword;
                                contact.NumberOfRetries = contactPassword.NumberOfRetries;
                            }
                        }
                    }

                    entity = contact;
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
                ContactPM contact = (from a in repository.context.Contacts
                                     where a.Email == email && a.UserType == "R"
                                     && a.Tenant == tenant
                                     select new ContactPM()
                                     {
                                         Anniversary = a.Anniversary,
                                         Birthday = a.Birthday,
                                         BusinessPhone = a.BusinessPhone,
                                         Email = a.Email,
                                         SearchFields = a.SearchFields,
                                         EnglishName = a.EnglishName,
                                         FacebookId = a.FacebookId,
                                         Fax = a.Fax,
                                         Id = a.Id,
                                         InActive = a.InActive,
                                         LocalName = a.LocalName,
                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                         Mobile = a.Mobile,
                                         Notes = a.Notes,
                                         Tenant = a.Tenant,
                                         Signature = a.Signature,
                                         SignatureHtml = a.SignatureHtml,
                                         DontShowLocal = a.DontShowLocalLabels,
                                         DisplayGettingStarted = a.DisplayGettingStarted,
                                         BirthdayReminder = a.BirthdayReminder,
                                         AnniversaryReminder = a.AnniversaryReminder,
                                         ImageDetailId = a.ImageDetailId,
                                         DoneDate = a.DoneDate,
                                         BirthDayOfYear = a.BirthDayOfYear,
                                         ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                         Position = a.Position,
                                         ExternalId = a.ExternalId,
                                         IndexColor = a.IndexColor,
                                         CompanyName = a.CompanyName,
                                         CreateDate = a.CreateDate,
                                         DigitalPortalLanguage = a.DigitalPortalLanguage
                                     }).FirstOrDefault();

                if (contact == null)
                {
                    contact = (from a in repository.context.Contacts
                               where a.Email == email
                               && a.Tenant == 0
                               select new ContactPM()
                               {
                                   Anniversary = a.Anniversary,
                                   Birthday = a.Birthday,
                                   BusinessPhone = a.BusinessPhone,
                                   Email = a.Email,
                                   SearchFields = a.SearchFields,
                                   EnglishName = a.EnglishName,
                                   FacebookId = a.FacebookId,
                                   Fax = a.Fax,
                                   Id = a.Id,
                                   InActive = a.InActive,
                                   LocalName = a.LocalName,
                                   ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                   Mobile = a.Mobile,
                                   Notes = a.Notes,
                                   Tenant = a.Tenant,
                                   Signature = a.Signature,
                                   SignatureHtml = a.SignatureHtml,
                                   DontShowLocal = a.DontShowLocalLabels,
                                   DisplayGettingStarted = a.DisplayGettingStarted,
                                   BirthdayReminder = a.BirthdayReminder,
                                   AnniversaryReminder = a.AnniversaryReminder,
                                   ImageDetailId = a.ImageDetailId,
                                   DoneDate = a.DoneDate,
                                   BirthDayOfYear = a.BirthDayOfYear,
                                   ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                   Position = a.Position,
                                   ExternalId = a.ExternalId,
                                   IndexColor = a.IndexColor,
                                   CompanyName = a.CompanyName,
                                   CreateDate = a.CreateDate,
                                   DigitalPortalLanguage = a.DigitalPortalLanguage
                               }).FirstOrDefault();
                }
                if (contact != null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email.ToLower()).FirstOrDefault();
                        if (contactPassword != null)
                        {
                            contact.IsLocked = contactPassword.IsLocked;
                            contact.MustChangePassword = contactPassword.MustChangePassword;
                            contact.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }
                }
                entity = contact;
            }
            return entity;
        }
        public string GetContactIdByLoggedEmail(int tenant)
        {
            string email = HttpContext.Current.User.Identity.Name;

            return repository.GetConactIdByemail(email, tenant);
        }

        public ContactPM GetContactById(string id, int tenant)
        {
            id = id.ToLower();
            string entityName = "ContactPM" + id + tenant;
            entityName = entityName.ToLower();
            ContactPM entity;
            UserRepository usersRepository = new UserRepository(tenant);

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    bool isTenant0User = false;
                    ContactPM contact = (from a in repository.context.Contacts
                                         where a.Email == id && a.InActive == false // all tracing classes call this method with the system user
                                         && a.Tenant == tenant
                                         select new ContactPM()
                                         {
                                             Anniversary = a.Anniversary,
                                             Birthday = a.Birthday,
                                             BusinessPhone = a.BusinessPhone,
                                             Email = a.Email,
                                             SearchFields = a.SearchFields,
                                             EnglishName = a.EnglishName,
                                             FacebookId = a.FacebookId,
                                             Fax = a.Fax,
                                             Id = a.Id,
                                             InActive = a.InActive,
                                             LocalName = a.LocalName,
                                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                             Mobile = a.Mobile,
                                             Notes = a.Notes,
                                             Tenant = a.Tenant,
                                             Signature = a.Signature,
                                             SignatureHtml = a.SignatureHtml,
                                             DontShowLocal = a.DontShowLocalLabels,
                                             DisplayGettingStarted = a.DisplayGettingStarted,
                                             BirthdayReminder = a.BirthdayReminder,
                                             AnniversaryReminder = a.AnniversaryReminder,
                                             ImageDetailId = a.ImageDetailId,
                                             DoneDate = a.DoneDate,
                                             BirthDayOfYear = a.BirthDayOfYear,
                                             ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                             Position = a.Position,
                                             ExternalId = a.ExternalId,
                                             IndexColor = a.IndexColor,
                                             CompanyName = a.CompanyName,
                                             CreateDate = a.CreateDate,
                                             DigitalPortalLanguage = a.DigitalPortalLanguage
                                         }).FirstOrDefault();

                    if (contact == null)
                    {
                        contact = (from a in repository.context.Contacts
                                   where a.Email == id
                                   && a.Tenant == 0 && a.InActive == false
                                   select new ContactPM()
                                   {
                                       Anniversary = a.Anniversary,
                                       Birthday = a.Birthday,
                                       BusinessPhone = a.BusinessPhone,
                                       Email = a.Email,
                                       SearchFields = a.SearchFields,
                                       EnglishName = a.EnglishName,
                                       FacebookId = a.FacebookId,
                                       Fax = a.Fax,
                                       Id = a.Id,
                                       InActive = a.InActive,
                                       LocalName = a.LocalName,
                                       ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                       Mobile = a.Mobile,
                                       Notes = a.Notes,
                                       Tenant = a.Tenant,
                                       Signature = a.Signature,
                                       SignatureHtml = a.SignatureHtml,
                                       DontShowLocal = a.DontShowLocalLabels,
                                       DisplayGettingStarted = a.DisplayGettingStarted,
                                       BirthdayReminder = a.BirthdayReminder,
                                       AnniversaryReminder = a.AnniversaryReminder,
                                       ImageDetailId = a.ImageDetailId,
                                       DoneDate = a.DoneDate,
                                       BirthDayOfYear = a.BirthDayOfYear,
                                       ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                       Position = a.Position,
                                       ExternalId = a.ExternalId,
                                       IndexColor = a.IndexColor,
                                       CompanyName = a.CompanyName,
                                       CreateDate = a.CreateDate,
                                       DigitalPortalLanguage = a.DigitalPortalLanguage
                                   }).FirstOrDefault();

                        isTenant0User = true;
                    }

                    if (contact != null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();
                            ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email.ToLower()).FirstOrDefault();
                            GlobalContact globalContact = null;

                            if (isTenant0User)
                            {
                                globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == contact.Email.ToLower() && cn.GlobalTenantId == 0).FirstOrDefault();
                            }

                            else
                            {
                                globalContact = globalContext.GlobalContacts.Where(cn => cn.Email == contact.Email.ToLower() && cn.GlobalTenantId == tenant).FirstOrDefault();
                            }

                            if (contactPassword != null)
                            {
                                contact.IsUser = globalContact.IsUser;
                                contact.HasPassword = true;
                                contact.IsLocked = contactPassword.IsLocked;
                                contact.MustChangePassword = contactPassword.MustChangePassword;
                                contact.NumberOfRetries = contactPassword.NumberOfRetries;
                            }
                        }
                    }

                    entity = contact;
                    if (entity != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (ContactPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                bool isTenant0User = false;
                ContactPM contact = (from a in repository.context.Contacts
                                     where a.Email == id && a.UserType == "R"
                                     && a.Tenant == tenant
                                     select new ContactPM()
                                     {
                                         Anniversary = a.Anniversary,
                                         Birthday = a.Birthday,
                                         BusinessPhone = a.BusinessPhone,
                                         Email = a.Email,
                                         SearchFields = a.SearchFields,
                                         EnglishName = a.EnglishName,
                                         FacebookId = a.FacebookId,
                                         Fax = a.Fax,
                                         Id = a.Id,
                                         InActive = a.InActive,
                                         LocalName = a.LocalName,
                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                         Mobile = a.Mobile,
                                         Notes = a.Notes,
                                         Tenant = a.Tenant,
                                         Signature = a.Signature,
                                         SignatureHtml = a.SignatureHtml,
                                         DontShowLocal = a.DontShowLocalLabels,
                                         DisplayGettingStarted = a.DisplayGettingStarted,
                                         BirthdayReminder = a.BirthdayReminder,
                                         AnniversaryReminder = a.AnniversaryReminder,
                                         ImageDetailId = a.ImageDetailId,
                                         DoneDate = a.DoneDate,
                                         BirthDayOfYear = a.BirthDayOfYear,
                                         ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                         Position = a.Position,
                                         ExternalId = a.ExternalId,
                                         IndexColor = a.IndexColor,
                                         CompanyName = a.CompanyName,
                                         CreateDate = a.CreateDate,
                                         DigitalPortalLanguage = a.DigitalPortalLanguage
                                     }).FirstOrDefault();

                if (contact == null)
                {
                    contact = (from a in repository.context.Contacts
                               where a.Email == id
                               && a.Tenant == 0
                               select new ContactPM()
                               {
                                   Anniversary = a.Anniversary,
                                   Birthday = a.Birthday,
                                   BusinessPhone = a.BusinessPhone,
                                   Email = a.Email,
                                   SearchFields = a.SearchFields,
                                   EnglishName = a.EnglishName,
                                   FacebookId = a.FacebookId,
                                   Fax = a.Fax,
                                   Id = a.Id,
                                   InActive = a.InActive,
                                   LocalName = a.LocalName,
                                   ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                   Mobile = a.Mobile,
                                   Notes = a.Notes,
                                   Tenant = a.Tenant,
                                   Signature = a.Signature,
                                   SignatureHtml = a.SignatureHtml,
                                   DontShowLocal = a.DontShowLocalLabels,
                                   DisplayGettingStarted = a.DisplayGettingStarted,
                                   BirthdayReminder = a.BirthdayReminder,
                                   AnniversaryReminder = a.AnniversaryReminder,
                                   ImageDetailId = a.ImageDetailId,
                                   DoneDate = a.DoneDate,
                                   BirthDayOfYear = a.BirthDayOfYear,
                                   ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                   Position = a.Position,
                                   ExternalId = a.ExternalId,
                                   IndexColor = a.IndexColor,
                                   CompanyName = a.CompanyName,
                                   CreateDate = a.CreateDate,
                                   DigitalPortalLanguage = a.DigitalPortalLanguage
                               }).FirstOrDefault();

                    isTenant0User = true;
                }

                if (contact != null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email.ToLower()).FirstOrDefault();

                        if (contactPassword != null)
                        {
                            contact.IsLocked = contactPassword.IsLocked;
                            contact.MustChangePassword = contactPassword.MustChangePassword;
                            contact.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }
                }

                entity = contact;
            }

            return entity;
        }

        public ContactPM GetContactByNameAndTenant(string name, int tenant, bool getFromCache)
        {
            name = name.ToLower();
            string entityName = "ContactPM" + name + tenant;
            entityName = entityName.ToLower();
            ContactPM entity;

            if (getFromCache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Contacts
                                  where (a.Tenant == tenant || a.Tenant == 0) && a.Email == name
                                  select new ContactPM()
                                  {
                                      Anniversary = a.Anniversary,
                                      Birthday = a.Birthday,
                                      BusinessPhone = a.BusinessPhone,
                                      Email = a.Email,
                                      SearchFields = a.SearchFields,
                                      EnglishName = a.EnglishName,
                                      FacebookId = a.FacebookId,
                                      Fax = a.Fax,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      LocalName = a.LocalName,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      Mobile = a.Mobile,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      Signature = a.Signature,
                                      SignatureHtml = a.SignatureHtml,
                                      DontShowLocal = a.DontShowLocalLabels,
                                      DisplayGettingStarted = a.DisplayGettingStarted,
                                      BirthdayReminder = a.BirthdayReminder,
                                      AnniversaryReminder = a.AnniversaryReminder,
                                      ImageDetailId = a.ImageDetailId,
                                      DoneDate = a.DoneDate,
                                      BirthDayOfYear = a.BirthDayOfYear,
                                      ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                      Position = a.Position,
                                      ExternalId = a.ExternalId,
                                      IndexColor = a.IndexColor,
                                      CompanyName = a.CompanyName,
                                      CreateDate = a.CreateDate,
                                      DigitalPortalLanguage = a.DigitalPortalLanguage
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                IGlobalContext globalContext = GlobalContext.GetContext();
                                List<ContactPassword> contactPasswords = globalContext.ContactPasswords.Where(c => c.Email == name).ToList();

                                ContactPassword contactPassword = contactPasswords.Where(cn => cn.Email == entity.Email).FirstOrDefault();
                                if (contactPassword != null)
                                {
                                    entity.IsLocked = contactPassword.IsLocked;
                                    entity.MustChangePassword = contactPassword.MustChangePassword;
                                    entity.NumberOfRetries = contactPassword.NumberOfRetries;
                                }

                                string cname = "ContactPM" + entity.Email + entity.Tenant;
                                cname = cname.ToLower();

                                if (CacheManager.CacheWrapper.Get(cname) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(cname, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                        }

                    }
                    else
                    {
                        entity = (ContactPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    ContactPM contact = (from a in repository.context.Contacts
                                         where a.Email == name && (a.Tenant == tenant || a.Tenant == 0)
                                         select new ContactPM()
                                         {
                                             Anniversary = a.Anniversary,
                                             Birthday = a.Birthday,
                                             BusinessPhone = a.BusinessPhone,
                                             Email = a.Email,
                                             SearchFields = a.SearchFields,
                                             EnglishName = a.EnglishName,
                                             FacebookId = a.FacebookId,
                                             Fax = a.Fax,
                                             Id = a.Id,
                                             InActive = a.InActive,
                                             LocalName = a.LocalName,
                                             ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                             Mobile = a.Mobile,
                                             Notes = a.Notes,
                                             Tenant = a.Tenant,
                                             Signature = a.Signature,
                                             SignatureHtml = a.SignatureHtml,
                                             DontShowLocal = a.DontShowLocalLabels,
                                             DisplayGettingStarted = a.DisplayGettingStarted,
                                             BirthdayReminder = a.BirthdayReminder,
                                             AnniversaryReminder = a.AnniversaryReminder,
                                             ImageDetailId = a.ImageDetailId,
                                             DoneDate = a.DoneDate,
                                             BirthDayOfYear = a.BirthDayOfYear,
                                             ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                             Position = a.Position,
                                             ExternalId = a.ExternalId,
                                             IndexColor = a.IndexColor,
                                             CompanyName = a.CompanyName,
                                             CreateDate = a.CreateDate,
                                             DigitalPortalLanguage = a.DigitalPortalLanguage
                                         }).FirstOrDefault();

                    if (contact != null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();
                            ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email).FirstOrDefault();

                            if (contactPassword != null)
                            {
                                contact.Password = contactPassword.Password;
                                contact.IsLocked = contactPassword.IsLocked;
                                contact.MustChangePassword = contactPassword.MustChangePassword;
                                contact.NumberOfRetries = contactPassword.NumberOfRetries;
                            }
                        }
                    }
                    entity = contact;
                }
            }
            else
            {
                ContactPM contact = (from a in repository.context.Contacts
                                     where a.Email == name && (a.Tenant == tenant || a.Tenant == 0)
                                     select new ContactPM()
                                     {
                                         Anniversary = a.Anniversary,
                                         Birthday = a.Birthday,
                                         BusinessPhone = a.BusinessPhone,
                                         Email = a.Email,
                                         SearchFields = a.SearchFields,
                                         EnglishName = a.EnglishName,
                                         FacebookId = a.FacebookId,
                                         Fax = a.Fax,
                                         Id = a.Id,
                                         InActive = a.InActive,
                                         LocalName = a.LocalName,
                                         ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                         Mobile = a.Mobile,
                                         Notes = a.Notes,
                                         Tenant = a.Tenant,
                                         Signature = a.Signature,
                                         SignatureHtml = a.SignatureHtml,
                                         DontShowLocal = a.DontShowLocalLabels,
                                         DisplayGettingStarted = a.DisplayGettingStarted,
                                         BirthdayReminder = a.BirthdayReminder,
                                         AnniversaryReminder = a.AnniversaryReminder,
                                         ImageDetailId = a.ImageDetailId,
                                         DoneDate = a.DoneDate,
                                         BirthDayOfYear = a.BirthDayOfYear,
                                         ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                         Position = a.Position,
                                         IndexColor = a.IndexColor,
                                         ExternalId = a.ExternalId,
                                         CompanyName = a.CompanyName,
                                         CreateDate = a.CreateDate,
                                         DigitalPortalLanguage = a.DigitalPortalLanguage
                                     }).FirstOrDefault();

                if (contact != null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email).FirstOrDefault();

                        if (contactPassword != null)
                        {
                            contact.Password = contactPassword.Password;
                            contact.IsLocked = contactPassword.IsLocked;
                            contact.MustChangePassword = contactPassword.MustChangePassword;
                            contact.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }
                }
                entity = contact;
            }
            return entity;
        }

        public IQueryable<ContactList> GetIQueryableEntityList(IQueryable<Contact> iQueryable)
        {
            IQueryable<ContactList> result = from a in iQueryable

                                             select new ContactList()
                                             {
                                                 Anniversary = a.Anniversary,
                                                 Birthday = a.Birthday,
                                                 BusinessPhone = a.BusinessPhone,
                                                 Email = a.Email,
                                                 EnglishName = a.EnglishName,
                                                 Fax = a.Fax,
                                                 Id = a.Id,
                                                 InActive = a.InActive,
                                                 LocalName = a.LocalName,
                                                 Mobile = a.Mobile,
                                                 Notes = a.Notes,
                                                 Tenant = a.Tenant,
                                                 Name = a.EnglishName,
                                                 SearchFields = a.SearchFields,
                                                 DontShowLocal = a.DontShowLocalLabels,
                                                 BirthdayReminder = a.BirthdayReminder,
                                                 AnniversaryReminder = a.AnniversaryReminder,
                                                 ImageDetailId = a.ImageDetailId,
                                                 DoneDate = a.DoneDate,
                                                 BirthDayOfYear = a.BirthDayOfYear,
                                                 ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                                 Position = a.Position,
                                                 IndexColor = a.IndexColor,
                                                 CompanyName = a.CompanyName,
                                                 CreateDate = a.CreateDate,
                                                 DigitalPortalLanguage = a.DigitalPortalLanguage
                                             };

            return result;
        }

        public IQueryable<ContactPM> GetContactsbyCardId(string id, int tenant)
        {
            CardContactRepository cardContactRepository = new CardContactRepository(tenant);
            CardContactAdditionalServiceQuery cardContactAdditionalServiceQuery = new CardContactAdditionalServiceQuery(tenant);
            CardContactProductQuery cardContactProductQuery = new CardContactProductQuery(tenant);
            IQueryable<CardContact> cardContacts = cardContactRepository.GetCardContacts(tenant);

            IQueryable<ContactPM> contacts = from a in cardContacts
                                             where a.CardId == id && a.Tenant == tenant
                                             select new ContactPM()
                                             {
                                                 Anniversary = a.Contact.Anniversary,
                                                 Birthday = a.Contact.Birthday,
                                                 BusinessPhone = a.Contact.BusinessPhone,
                                                 Email = a.Contact.Email,
                                                 EnglishName = a.Contact.EnglishName,
                                                 FacebookId = a.Contact.FacebookId,
                                                 DontShowLocalLabels = a.Contact.DontShowLocalLabels,
                                                 Fax = a.Contact.Fax,
                                                 Id = a.Contact.Id,
                                                 InActive = a.Contact.InActive,
                                                 LocalName = a.Contact.LocalName,
                                                 Mobile = a.Contact.Mobile,
                                                 Notes = a.Contact.Notes,
                                                 Tenant = a.Contact.Tenant,
                                                 CardId = id,
                                                 Position = a.Contact.Position,
                                                 IsAirExport = a.IsAirExport,
                                                 IsAirImport = a.IsAirImport,
                                                 IsOceanExport = a.IsOceanExport,
                                                 IsOceanImport = a.IsOceanImport,
                                                 IsAll = a.IsAll,
                                                 IsInlandDomestic = a.IsInlandDomestic,
                                                 IsCustomsImport = a.IsCustomsImport,
                                                 IsInlandExport = a.IsInlandExport,
                                                 IsInlandImport = a.IsInlandImport,
                                                 ExternalId = a.Contact.ExternalId,
                                                 IndexColor = a.Contact.IndexColor,
                                                 CompanyName = a.Contact.CompanyName,
                                                 CreateDate = a.Contact.CreateDate,
                                                 ContactForAccounting = a.Contact.ContactForAccounting,
                                             };

            List<ContactPM> entityList = contacts.ToList();

            foreach (ContactPM contact in entityList)
            {
                CardContact cardContact = cardContacts.Where(d => d.CardId == id && d.ContactId == contact.Id).FirstOrDefault();
                if (cardContact != null)
                {
                    contact.CardContactAdditionalServices = cardContactAdditionalServiceQuery.GetCardContactAdditionalServicePMsByCardContactId(cardContact.Id, tenant);
                    contact.CardContactProducts = cardContactProductQuery.GetCardContactProductPMsByCardContactId(cardContact.Id, tenant);
                }
            }

            return entityList.AsQueryable();
        }

        public IQueryable<ContactList> GetContactListsbyCardId(string id, int tenant)
        {
            CardContactRepository cardContactRepository = new CardContactRepository(tenant);

            var contacts = cardContactRepository.GetCardContacts(tenant).Where(c => c.CardId == id && c.ContactId == c.ContactId && c.Tenant == tenant).Select(a => new ContactList
            {
                Anniversary = a.Contact.Anniversary,
                Birthday = a.Contact.Birthday,
                BusinessPhone = a.Contact.BusinessPhone,
                Email = a.Contact.Email,
                EnglishName = a.Contact.EnglishName,
                Fax = a.Contact.Fax,
                Id = a.ContactId,
                InActive = a.Contact.InActive,
                LocalName = a.Contact.LocalName,
                Mobile = a.Contact.Mobile,
                Notes = a.Contact.Notes,
                Tenant = a.Contact.Tenant,
                Name = a.Contact.EnglishName,
                SearchFields = a.Contact.SearchFields,
                DontShowLocal = a.Contact.DontShowLocalLabels,
                BirthdayReminder = a.Contact.BirthdayReminder,
                AnniversaryReminder = a.Contact.AnniversaryReminder,
                ImageDetailId = a.Contact.ImageDetailId,
                DoneDate = a.Contact.DoneDate,
                BirthDayOfYear = a.Contact.BirthDayOfYear,
                ContactDoneMethodCode = a.Contact.ContactDoneMethod != null ? a.Contact.ContactDoneMethod.Code : null,
                Position = a.Contact.Position,
                IndexColor = a.Contact.IndexColor,
                CompanyName = a.Contact.CompanyName,
                CreateDate = a.Contact.CreateDate,
            });

            return contacts;
        }

        public IQueryable<ContactList> GetContactLists(int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);

            IQueryable<Contact> contacts = contactRep.GetActiveContacts(tenant);
            IQueryable<ContactList> contactLists = (from a in contacts
                                                    select new ContactList()
                                                    {
                                                        Anniversary = a.Anniversary,
                                                        Birthday = a.Birthday,
                                                        BusinessPhone = a.BusinessPhone,
                                                        Email = a.Email,
                                                        EnglishName = a.EnglishName,
                                                        Fax = a.Fax,
                                                        Id = a.Id,
                                                        InActive = a.InActive,
                                                        LocalName = a.LocalName,
                                                        Mobile = a.Mobile,
                                                        Notes = a.Notes,
                                                        Tenant = a.Tenant,
                                                        Name = a.EnglishName,
                                                        SearchFields = a.SearchFields,
                                                        DontShowLocal = a.DontShowLocalLabels,
                                                        BirthdayReminder = a.BirthdayReminder,
                                                        AnniversaryReminder = a.AnniversaryReminder,
                                                        ImageDetailId = a.ImageDetailId,
                                                        DoneDate = a.DoneDate,
                                                        BirthDayOfYear = a.BirthDayOfYear,
                                                        ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                                        Position = a.Position,
                                                        IndexColor = a.IndexColor,
                                                        CompanyName = a.CompanyName,
                                                        CreateDate = a.CreateDate,
                                                        DigitalPortalLanguage = a.DigitalPortalLanguage
                                                    });

            return contactLists;
        }

        public IQueryable<ContactList> GetContactListsFollowShipment(List<string> trackedIds, int tenant)
        {
            IQueryable<Contact> contacts = repository.GetContactsByIds(trackedIds, tenant);

            IQueryable<ContactList> contactLists = (from a in contacts
                                                    where trackedIds.Contains(a.Id)
                                                    select new ContactList()
                                                    {
                                                        Anniversary = a.Anniversary,
                                                        Birthday = a.Birthday,
                                                        BusinessPhone = a.BusinessPhone,
                                                        Email = a.Email,
                                                        EnglishName = a.EnglishName,
                                                        Fax = a.Fax,
                                                        Id = a.Id,
                                                        InActive = a.InActive,
                                                        LocalName = a.LocalName,
                                                        Mobile = a.Mobile,
                                                        Notes = a.Notes,
                                                        Tenant = a.Tenant,
                                                        Name = a.EnglishName,
                                                        SearchFields = a.SearchFields,
                                                        DontShowLocal = a.DontShowLocalLabels,
                                                        BirthdayReminder = a.BirthdayReminder,
                                                        AnniversaryReminder = a.AnniversaryReminder,
                                                        ImageDetailId = a.ImageDetailId,
                                                        DoneDate = a.DoneDate,
                                                        BirthDayOfYear = a.BirthDayOfYear,
                                                        ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                                        Position = a.Position,
                                                        IndexColor = a.IndexColor,
                                                        CompanyName = a.CompanyName,
                                                        CreateDate = a.CreateDate,
                                                        DigitalPortalLanguage = a.DigitalPortalLanguage
                                                    });
            return contactLists;
        }

        public List<string> GetContactEmailsListsByIds(List<string> contactIds, int tenant)
        {
            IQueryable<Contact> contacts = repository.GetContactsByIds(contactIds, tenant);
            List<string> contactEmailLists = (from a in contacts
                                              where contactIds.Contains(a.Id) && !a.InActive && a.UserType == "R"
                                              select a.Email).ToList();
            return contactEmailLists;
        }




        public List<ContactList> GetContactListsByIds(List<string> trackedIds, int tenant)
        {
            IQueryable<Contact> contacts = repository.GetContactsByIds(trackedIds, tenant);

            List<ContactList> contactLists = (from a in contacts
                                              where trackedIds.Contains(a.Id)
                                              select new ContactList()
                                              {

                                                  Anniversary = a.Anniversary,
                                                  Birthday = a.Birthday,
                                                  BusinessPhone = a.BusinessPhone,
                                                  Email = a.Email,
                                                  EnglishName = a.EnglishName,
                                                  Fax = a.Fax,
                                                  Id = a.Id,
                                                  InActive = a.InActive,
                                                  LocalName = a.LocalName,
                                                  Mobile = a.Mobile,
                                                  Notes = a.Notes,
                                                  Tenant = a.Tenant,
                                                  Name = a.EnglishName,
                                                  SearchFields = a.SearchFields,
                                                  DontShowLocal = a.DontShowLocalLabels,
                                                  BirthdayReminder = a.BirthdayReminder,
                                                  AnniversaryReminder = a.AnniversaryReminder,
                                                  ImageDetailId = a.ImageDetailId,
                                                  DoneDate = a.DoneDate,
                                                  BirthDayOfYear = a.BirthDayOfYear,
                                                  ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                                  Position = a.Position,
                                                  IndexColor = a.IndexColor,
                                                  CompanyName = a.CompanyName,
                                                  CreateDate = a.CreateDate,
                                                  DigitalPortalLanguage = a.DigitalPortalLanguage
                                              }).ToList();
            return contactLists;
        }

        public ContactPM GetContactByEmailOnlyForLogin(string email, int tenant)
        {
            email = email.ToLower();
            ContactPM contactPM = this.GetContactByEmailOnly(email, 0);
            if (contactPM == null)
            {
                contactPM = this.GetContactByEmailOnly(email, tenant);
            }
            return contactPM;
        }

        public string GetContactEmailById(string id, int tenant)
        {
            string email = (from a in repository.context.Contacts
                            where a.Id == id
                            && a.Tenant == tenant
                            select new ContactPM()
                            {
                                Id = a.Id,
                                Tenant = a.Tenant,
                                Email = a.Email,

                            }.Email).FirstOrDefault();

            return email;
        }

        public byte[] GetSignatureHtmlByContactId(string contactId, int tenant)
        {

            byte[] result = null;
            Contact contact = repository.context.Contacts.Where(d => d.Id == contactId).FirstOrDefault();
            if (contact != null)
            {
                result = contact.SignatureHtml;
            }
            return result;
        }

        public List<ContactList> GetContactListsByEmailsString(string emails, int tenant)
        {
            List<ContactList> contacts = new List<ContactList>();
            List<string> emailsList = emails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (emailsList.Count() > 0)
            {
                ContactPM mycontact = null;
                foreach (var item in emailsList)
                {
                    mycontact = GetSingleContact(item, tenant);
                    if (mycontact != null)
                    {
                        contacts.Add(new ContactList()
                        {
                            Email = mycontact.Email,
                            EnglishName = mycontact.EnglishName,
                            SearchFields = mycontact.SearchFields,
                        });
                    }
                    else
                    {
                        contacts.Add(new ContactList()
                        {
                            Email = item,
                            EnglishName = item,
                            SearchFields = item,
                        });
                    }
                }
            }

            return contacts;
        }


        public IQueryable<ContactList> GetContactListsByListIds(List<string> contactIds, int tenant)
        {
            IQueryable<ContactList> contactLists = (from a in repository.context.Contacts
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


        public ContactList GetContactListsById(string contactId, int tenant)
        {
            ContactList contactList = (from a in repository.context.Contacts
                                       where a.Id == contactId && a.Tenant == tenant
                                       select new ContactList()
                                       {
                                           Id = a.Id,
                                           Tenant = a.Tenant,
                                           EnglishName = a.EnglishName,
                                           LocalName = a.LocalName,
                                           Mobile = a.Mobile,
                                           Fax = a.Fax,
                                           BusinessPhone = a.BusinessPhone,
                                           Email = a.Email,
                                       }).FirstOrDefault();
            return contactList;
        }


        public ContactPM GetSingleContactPM(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ContactPM instance = (from a in repository.context.Contacts
                                      where a.UserType == "R"
                                      && a.Id == id
                                      select new ContactPM()
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
                                          DigitalPortalLanguage = a.DigitalPortalLanguage
                                      }).FirstOrDefault();

                if (instance != null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == instance.Email.ToLower()).FirstOrDefault();

                        if (contactPassword != null)
                        {
                            instance.Password = contactPassword.Password;
                            instance.IsLocked = contactPassword.IsLocked;
                            instance.MustChangePassword = contactPassword.MustChangePassword;
                            instance.NumberOfRetries = contactPassword.NumberOfRetries;
                        }
                    }

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

                return instance;
            }

            return null;
        }

        public string GetRealEmailByEnglishName(string englishName, int tenant, string xxxDotCom)
        {
            if (string.IsNullOrEmpty(englishName) || string.IsNullOrEmpty(xxxDotCom))
            {
                return null;
            }
            englishName = englishName.ToUpperInvariant();
            xxxDotCom = xxxDotCom.ToLowerInvariant();

            return (from a in repository.context.Contacts
                            where a.EnglishName.ToUpper() == englishName 
                            && a.Tenant == tenant && a.Email.ToLower() != xxxDotCom && a.UserType == "R"
                            && !a.InActive && a.Email != null && a.Email != string.Empty
                            select a.Email).FirstOrDefault();
        }

        public ContactPM GetFirstContactByEnglishNamePM(string Name, int tenant)
        {

            ContactPM instance = (from a in repository.context.Contacts
                                  where a.Tenant == tenant && a.EnglishName == Name
                                  select new ContactPM()
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
                                      DigitalPortalLanguage = a.DigitalPortalLanguage
                                  }).FirstOrDefault();



            return instance;
        }

        public string GetContactNameId(string id, int tenant)
        {
            string result = null;
            Contact contact = repository.context.Contacts.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();
            if (contact != null)
            {
                result = contact.EnglishName;
            }

            return result;

        }

        public List<ContactList> GetContactListsByEmailLists(List<string> emails, int tenant)
        {
            List<ContactList> contactLists = (from a in repository.context.Contacts
                                              where emails.Contains(a.Email) && a.Tenant == tenant
                                              select new ContactList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  EnglishName = a.EnglishName,
                                                  LocalName = a.LocalName,
                                                  Mobile = a.Mobile,
                                                  Fax = a.Fax,
                                                  BusinessPhone = a.BusinessPhone,
                                                  Email = a.Email,

                                              }).ToList();
            return contactLists;


        }


        public IQueryable<ContactList> GetContactListsByListIdsForTenantReport(List<string> contactIds)
        {
            IQueryable<ContactList> contactLists = (from a in repository.context.Contacts
                                                    where contactIds.Contains(a.Id) && a.UserType == "R"
                                                    select new ContactList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        EnglishName = a.EnglishName,
                                                        InActive = a.InActive,
                                                        Email = a.Email,
                                                    });
            return contactLists;
        }


        public List<ContactList> GetContactUserListsByTenants(List<int> tenants, bool includeInactiveUsers)
        {
            List<ContactList> result = new List<ContactList>();

            List<string> userIds = (from a in repository.context.Users
                                    where tenants.Contains(a.Tenant)
                                    select a.Id).ToList();

            IQueryable<ContactList> contactLists = GetContactListsByListIdsForTenantReport(userIds);

            if (!includeInactiveUsers)
            {
                contactLists = contactLists.Where(d => !d.InActive);
            }

            result = contactLists.ToList();

            List<string> contactIds = result.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();

            UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
            UserLastLoginQuery query = new UserLastLoginQuery(rep);
            List<UserLastLoginPM> userLastLoginPMLists = query.GetUserLastLoginPMsByUserIds(contactIds).ToList();
            foreach (ContactList contact in result)
            {
                UserLastLoginPM userLastLoginPM = userLastLoginPMLists.Where(d => d.Id == contact.Id).FirstOrDefault();
                if (userLastLoginPM != null)
                {
                    contact.LastLoginDate = userLastLoginPM.LoginDateTime;
                }

            }

            return result;
        }


        public ContactPM GetSingleByEmail(string email, int tenant)
        {
            email = email.ToLower();
            ContactPM contact = (from a in repository.context.Contacts
                                 where a.Email == email
                                 && a.Tenant == tenant
                                 select new ContactPM()
                                 {
                                     Anniversary = a.Anniversary,
                                     Birthday = a.Birthday,
                                     BusinessPhone = a.BusinessPhone,
                                     Email = a.Email,
                                     SearchFields = a.SearchFields,
                                     EnglishName = a.EnglishName,
                                     FacebookId = a.FacebookId,
                                     Fax = a.Fax,
                                     Id = a.Id,
                                     InActive = a.InActive,
                                     LocalName = a.LocalName,
                                     ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                     Mobile = a.Mobile,
                                     Notes = a.Notes,
                                     Tenant = a.Tenant,
                                     Signature = a.Signature,
                                     SignatureHtml = a.SignatureHtml,
                                     DontShowLocal = a.DontShowLocalLabels,
                                     DisplayGettingStarted = a.DisplayGettingStarted,
                                     BirthdayReminder = a.BirthdayReminder,
                                     AnniversaryReminder = a.AnniversaryReminder,
                                     ImageDetailId = a.ImageDetailId,
                                     DoneDate = a.DoneDate,
                                     BirthDayOfYear = a.BirthDayOfYear,
                                     ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                     Position = a.Position,
                                     ExternalId = a.ExternalId,
                                     IndexColor = a.IndexColor,
                                     CompanyName = a.CompanyName,
                                     CreateDate = a.CreateDate,
                                     DigitalPortalLanguage = a.DigitalPortalLanguage
                                 }).FirstOrDefault();

            if (contact != null)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(cn => cn.Email == contact.Email).FirstOrDefault();

                    if (contactPassword != null)
                    {
                        contact.IsLocked = contactPassword.IsLocked;
                        contact.MustChangePassword = contactPassword.MustChangePassword;
                        contact.NumberOfRetries = contactPassword.NumberOfRetries;
                    }
                }
            }

            return contact;
        }

        public ContactPM GetSinglePMByEmail(string email, int tenant)
        {
            return this.GetSingleByEmail(email, tenant);
        }
        public IQueryable<ContactList> GetDemoTenantContactList(IQueryable<Contact> iQueryable, string loggedUserId, int tenant)
        {
            List<ContactList> result = new List<ContactList>();

            int index = 1;
            foreach (Contact contact in iQueryable)
            {
                string email = contact.Email;
                string name = contact.EnglishName;

                if (contact.Id != loggedUserId)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        string[] emailParts = contact.Email.Split('@');
                        email = "contact" + index + "@democompany.com ";
                    }
                    name = "Contact" + index;
                }

                ContactList newItem = new ContactList()
                {
                    Email = email,
                    EnglishName = name,
                    Id = contact.Id,
                    Anniversary = contact.Anniversary,
                    Birthday = contact.Birthday,
                    BusinessPhone = contact.BusinessPhone,
                    SearchFields = contact.SearchFields,
                    Fax = contact.Fax,
                    InActive = contact.InActive,
                    LocalName = contact.LocalName,
                    Mobile = contact.Mobile,
                    Notes = contact.Notes,
                    Tenant = contact.Tenant,
                    DontShowLocal = contact.DontShowLocalLabels,
                    DisplayGettingStarted = contact.DisplayGettingStarted,
                    BirthdayReminder = contact.BirthdayReminder,
                    AnniversaryReminder = contact.AnniversaryReminder,
                    ImageDetailId = contact.ImageDetailId,
                    DoneDate = contact.DoneDate,
                    BirthDayOfYear = contact.BirthDayOfYear,
                    ContactDoneMethodCode = contact.ContactDoneMethod != null ? contact.ContactDoneMethod.Code : null,
                    Position = contact.Position,
                    IndexColor = contact.IndexColor,
                    CompanyName = contact.CompanyName,
                    CreateDate = contact.CreateDate,
                    DigitalPortalLanguage = contact.DigitalPortalLanguage
                };
                result.Add(newItem);
                index++;
            }

            return result.AsQueryable();
        }

        public string GetContactIdByEmail(string email,int tenant)
        {
            string id = repository.GetConactIdByemail(email, tenant);
            return id;
        }


        public ContactPM GetSingleByEmailWithoutTenant(string email)
        {
            email = email.ToLower();
            ContactPM contact = (from a in repository.context.Contacts
                                 where a.Email == email
                                 select new ContactPM()
                                 {
                                     Id = a.Id,
                                 }).FirstOrDefault();
            return contact;
        }

        public IQueryable<ContactPM> GetContactsbyCustomerId(string id, int tenant)
        {
            CardContactRepository cardContactRepository = new CardContactRepository(tenant);
            IQueryable<ContactPM> contacts = from a in cardContactRepository.context.CardContacts.Include("Contact")
                                             where a.CardId == id && a.Tenant == tenant
                                             select new ContactPM()
                                             {
                                                 BusinessPhone = a.Contact.BusinessPhone,
                                                 Email = a.Contact.Email,
                                                 EnglishName = a.Contact.EnglishName,
                                                 Fax = a.Contact.Fax,
                                                 Id = a.Contact.Id,
                                                 InActive = a.Contact.InActive,
                                                 LocalName = a.Contact.LocalName,
                                                 Mobile = a.Contact.Mobile,
                                                 Notes = a.Contact.Notes,
                                                 Tenant = a.Contact.Tenant,
                                                 CardId = id,
                                                 Position = a.Contact.Position,
                                                 CreateDate = a.Contact.CreateDate
                                             };

            return contacts;
        }
        public static string UserBranchRestriction(string paymentBranchId, int tenant, string action = "perform this action")
        {
            {
                string rv = "";
                bool isError = false;
                if (!String.IsNullOrEmpty(paymentBranchId))
                {
                    ContactQuery contactRep = new ContactQuery(tenant);
                    UserQuery userQuery = new UserQuery(tenant);

                    ContactPM contact = contactRep.GetContactByNameAndTenant(Logitude.BL.Security.SecurityUtility.GetAuthenticatedWorkWebUser(), tenant, false);
                    UserPM user = userQuery.GetSinglePM(contact.Id, tenant);

                    if (user != null && user.IsBranchRestricted)
                    {
                        if (user.UserPermittedBranches == null || user.UserPermittedBranches.Count == 0)
                            isError = true;
                        else
                        {
                            List<string> userPermittedBranchIds = user.UserPermittedBranches.Select(item => item.Id).ToList<string>();
                            if (userPermittedBranchIds == null || userPermittedBranchIds.Count == 0
                                || !userPermittedBranchIds.Contains(paymentBranchId))
                                isError = true;
                        }
                        if (isError)
                        {
                            BranchQuery branchQuery = new BranchQuery(tenant);
                            BranchPM branch = branchQuery.GetSinglePM(paymentBranchId, tenant);
                            string base_text = "User " + user.Code + " is not permitted to " + action + " in Branch ";
                            if (branch != null)
                                rv = base_text + branch.Code;
                            else
                                rv = base_text + paymentBranchId;
                        }
                    }
                }
                return rv;
            }
        }

        public Contact GetContactByEmail(string email, int tenant)
        {
            return repository.GetContactByEmail(email, tenant);
        }

    }
}