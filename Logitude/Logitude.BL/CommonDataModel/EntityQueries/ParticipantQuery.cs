using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.CustomFields;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ParticipantQuery
    {
        ParticipantRepository repository;

        public ParticipantQuery()
        {
            repository = new ParticipantRepository(); 
        }

        public ParticipantQuery(int tenant)
        {
            repository = new ParticipantRepository(tenant);
        }

        public ParticipantQuery(ParticipantRepository ParticipantRepository)
        {
            repository = ParticipantRepository;
        }

        public ParticipantPM GetSinglePM(string id, int tenant)
        {
            ParticipantPM Participant = (from a in repository.context.Participants.Include("Card")
                             where a.Id == id && a.Tenant == tenant
                             select new ParticipantPM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 ForwarderTenant = a.ForwarderTenant,
                                 TTY = a.TTY,
                                 Registered = a.Registered,
                                 RegistrationRequested = a.RegistrationRequested,
                                 Code = a.Card.Code,
                                 EnglishName = a.Card.EnglishName,
                                 LocalName = a.Card.LocalName,
                                 ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                 PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                 CreateDate = a.Card.CreateDate,
                                 InActive = a.Card.InActive,
                                 Notes = a.Card.Notes,
                                 PartnerTypeId = a.Card.PartnerTypeId,
                                 PaymentTermId = a.Card.PaymentTermId,
                                 VatNumber = a.Card.VatNumber,
                                 ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                 Website = a.Card.Website,
                                 InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                 VatTypeId = a.Card.VatTypeId,
                                 BankName = a.Card.BankName,
                                 BankAddress = a.Card.BankAddress,
                                 IBANNumber = a.Card.IBANNumber,
                                 Swift = a.Card.Swift,
                                 AccountNumber =a.Card.AccountNumber,
                                 PrimaryContactId = a.Card.PrimaryContactId, 
                                 CountryId = a.Card.CountryId,
                                 RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                                 IsDirect = a.IsDirect,
                                 RegistrationDate = a.RegistrationDate,
                                 FWBNotifyContacts = a.FWBNotifyContacts,
                                 FHLNotifyContacts = a.FHLNotifyContacts,
                                 FFRNotifyContacts = a.FFRNotifyContacts,

                                 Card = new CardPM()
                                 {
                                     Id = a.Id,
                                     Tenant = a.Tenant,
                                     EnglishName = a.Card.EnglishName,
                                     PrimaryContactId = a.Card.PrimaryContactId,
                                     Code=a.Card.Code,
                                     PartnerTypeId=a.Card.PartnerTypeId,
                                     GLAccountId=a.Card.GLAccountId,
                                 },

                             }).FirstOrDefault();

            ParticipantPM securedPm = new ParticipantPM();
            SecuredMapping.GetMappedPM(Participant, securedPm, "Participant", tenant);
            if (securedPm != null && Participant != null)
            {
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Participant", Tenant = tenant, Type = "PM", Entities = new List<ParticipantPM> { securedPm }.Cast<object>().ToList() }).Set();
            }
            return securedPm;
        }

        public IQueryable<ParticipantPM> GetParticipantPMsByTenant(int tenant)
        {
            IQueryable<ParticipantPM> Participants = from a in repository.context.Participants.Include("Card")
                                         where a.Tenant == tenant
                                         select new ParticipantPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             ForwarderTenant = a.ForwarderTenant,
                                             TTY = a.TTY,
                                             Registered = a.Registered,
                                             RegistrationRequested = a.RegistrationRequested,
                                             Code = a.Card.Code,
                                             EnglishName = a.Card.EnglishName,
                                             LocalName = a.Card.LocalName,
                                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                             CreateDate = a.Card.CreateDate,
                                             InActive = a.Card.InActive,
                                             Notes = a.Card.Notes,
                                             PartnerTypeId = a.Card.PartnerTypeId,
                                             PaymentTermId = a.Card.PaymentTermId,
                                             VatNumber = a.Card.VatNumber,
                                             ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                             Website = a.Card.Website,
                                             InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                             VatTypeId = a.Card.VatTypeId,
                                             BankName = a.Card.BankName,
                                             BankAddress = a.Card.BankAddress,
                                             IBANNumber = a.Card.IBANNumber,
                                             Swift = a.Card.Swift,
                                             AccountNumber = a.Card.AccountNumber,
                                             PrimaryContactId = a.Card.PrimaryContactId,
                                             CountryId = a.Card.CountryId,
                                             RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                                             IsDirect = a.IsDirect,
                                             RegistrationDate = a.RegistrationDate,
                                             FWBNotifyContacts = a.FWBNotifyContacts,
                                             FHLNotifyContacts = a.FHLNotifyContacts,
                                             FFRNotifyContacts = a.FFRNotifyContacts,

                                             Card = new CardPM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 EnglishName = a.Card.EnglishName,
                                                 PrimaryContactId = a.Card.PrimaryContactId,
                                             },
                                         };
            return Participants;
        }

        public IQueryable<ParticipantPM> GetParticipantsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Participants.Include("Card")
                         where a.Tenant == tenant
                         select new ParticipantPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             ForwarderTenant = a.ForwarderTenant,
                             TTY = a.TTY,
                             Registered = a.Registered,
                             RegistrationRequested = a.RegistrationRequested,
                             Code = a.Card.Code,
                             EnglishName = a.Card.EnglishName,
                             LocalName = a.Card.LocalName,
                             ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                             PayablesAccountingCard = a.Card.PayablesAccountingCard,
                             CreateDate = a.Card.CreateDate,
                             InActive = a.Card.InActive,
                             Notes = a.Card.Notes,
                             PartnerTypeId = a.Card.PartnerTypeId,
                             PaymentTermId = a.Card.PaymentTermId,
                             VatNumber = a.Card.VatNumber,
                             ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                             Website = a.Card.Website,
                             InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                             VatTypeId = a.Card.VatTypeId,
                             BankName = a.Card.BankName,
                             BankAddress = a.Card.BankAddress,
                             IBANNumber = a.Card.IBANNumber,
                             Swift = a.Card.Swift,
                             AccountNumber = a.Card.AccountNumber,
                             PrimaryContactId = a.Card.PrimaryContactId,
                             CountryId = a.Card.CountryId,
                             RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                             IsDirect = a.IsDirect,
                             RegistrationDate = a.RegistrationDate,
                             FWBNotifyContacts = a.FWBNotifyContacts,
                             FHLNotifyContacts = a.FHLNotifyContacts,
                             FFRNotifyContacts = a.FFRNotifyContacts,

                             Card = new CardPM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 EnglishName = a.Card.EnglishName,
                                 PrimaryContactId = a.Card.PrimaryContactId,
                             },

                         }).AsQueryable();

            IQueryable<ParticipantPM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));

                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<ParticipantList> GetIQueryableEntityList(IQueryable<Participant> iQueryable)
        {
            string objcetTableId = new ObjectTableQuery(0).GetObjectTableIdByName("Card");
            IQueryable<ParticipantList> result = from a in iQueryable.Include("Card").Include("Forwarder")
                                                 join customFieldsMainObject in repository.context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                 from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
                                                 select new ParticipantList()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               ForwarderTenant = a.ForwarderTenant,
                                               TTY = a.TTY,
                                               Registered = a.Registered,
                                               RegistrationRequested = a.RegistrationRequested,
                                               Code = a.Card.Code,
                                               EnglishName = a.Card.EnglishName,
                                               LocalName = a.Card.LocalName,
                                               ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                               PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                               CreateDate = a.Card.CreateDate,
                                               InActive = a.Card.InActive,
                                               Notes = a.Card.Notes,
                                               PartnerTypeId = a.Card.PartnerTypeId,
                                               PaymentTermId = a.Card.PaymentTermId,
                                               VatNumber = a.Card.VatNumber,
                                               ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                                               Website = a.Card.Website,
                                               InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                                               VatTypeId = a.Card.VatTypeId,                                              
                                               AccountNumber = a.Card.AccountNumber,
                                               PrimaryContactId = a.Card.PrimaryContactId,
                                               CountryId = a.Card.CountryId,
                                               RegistrationUpdatedBy = a.RegistrationUpdatedBy,
                                               SearchFields = a.Card.SearchFields,
                                               ForwarderTenantName = a.Forwarder == null ? null : a.Forwarder.Company,
                                               IsDirect = a.IsDirect,
                                               RegistrationDate = a.RegistrationDate,
                                               PrimaryContactName = a.PrimaryContactName,
                                               PrimaryContactEmail = a.PrimaryContactEmail,
                                               PrimaryContactPhone = a.PrimaryContactPhone,
                                               StateName = a.Card.StateName,
                                               Field1 = customFieldsMainObject != null ? customFieldsMainObject.Field1 : null,
                                               Field2 = customFieldsMainObject != null ? customFieldsMainObject.Field2 : null,
                                               Field3 = customFieldsMainObject != null ? customFieldsMainObject.Field3 : null,
                                               Field4 = customFieldsMainObject != null ? customFieldsMainObject.Field4 : null,
                                               Field5 = customFieldsMainObject != null ? customFieldsMainObject.Field5 : null,
                                               Field6 = customFieldsMainObject != null ? customFieldsMainObject.Field6 : null,
                                               Field7 = customFieldsMainObject != null ? customFieldsMainObject.Field7 : null,
                                               Field8 = customFieldsMainObject != null ? customFieldsMainObject.Field8 : null,
                                               Field9 = customFieldsMainObject != null ? customFieldsMainObject.Field9 : null,
                                               Field10 = customFieldsMainObject != null ? customFieldsMainObject.Field10 : null,
                                               Field11 = customFieldsMainObject != null ? customFieldsMainObject.Field11 : null,
                                               Field12 = customFieldsMainObject != null ? customFieldsMainObject.Field12 : null,
                                               Field13 = customFieldsMainObject != null ? customFieldsMainObject.Field13 : null,
                                               Field14 = customFieldsMainObject != null ? customFieldsMainObject.Field14 : null,
                                               Field15 = customFieldsMainObject != null ? customFieldsMainObject.Field15 : null,
                                               Field16 = customFieldsMainObject != null ? customFieldsMainObject.Field16 : null,
                                               Field17 = customFieldsMainObject != null ? customFieldsMainObject.Field17 : null,
                                               Field18 = customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
                                               Field19 = customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
                                               Field20 = customFieldsMainObject != null ? customFieldsMainObject.Field20 : null,
                                               Field21 = customFieldsMainObject != null ? customFieldsMainObject.Field21 : null,
                                               Field22 = customFieldsMainObject != null ? customFieldsMainObject.Field22 : null,
                                               Field23 = customFieldsMainObject != null ? customFieldsMainObject.Field23 : null,
                                               Field24 = customFieldsMainObject != null ? customFieldsMainObject.Field24 : null,
                                               Field25 = customFieldsMainObject != null ? customFieldsMainObject.Field25 : null,
                                               Field26 = customFieldsMainObject != null ? customFieldsMainObject.Field26 : null,
                                               Field27 = customFieldsMainObject != null ? customFieldsMainObject.Field27 : null,
                                               Field28 = customFieldsMainObject != null ? customFieldsMainObject.Field28 : null,
                                               Field29 = customFieldsMainObject != null ? customFieldsMainObject.Field29 : null,
                                               Field30 = customFieldsMainObject != null ? customFieldsMainObject.Field30 : null,
                                               Field31 = customFieldsMainObject != null ? customFieldsMainObject.Field31 : null,
                                               Field32 = customFieldsMainObject != null ? customFieldsMainObject.Field32 : null,
                                               Field33 = customFieldsMainObject != null ? customFieldsMainObject.Field33 : null,
                                               Field34 = customFieldsMainObject != null ? customFieldsMainObject.Field34 : null,
                                               Field35 = customFieldsMainObject != null ? customFieldsMainObject.Field35 : null,
                                               Field36 = customFieldsMainObject != null ? customFieldsMainObject.Field36 : null,
                                               Field37 = customFieldsMainObject != null ? customFieldsMainObject.Field37 : null,
                                               Field38 = customFieldsMainObject != null ? customFieldsMainObject.Field38 : null,
                                               Field39 = customFieldsMainObject != null ? customFieldsMainObject.Field39 : null,
                                               Field40 = customFieldsMainObject != null ? customFieldsMainObject.Field40 : null,
                                               Field41 = customFieldsMainObject != null ? customFieldsMainObject.Field41 : null,
                                               Field42 = customFieldsMainObject != null ? customFieldsMainObject.Field42 : null,
                                               Field43 = customFieldsMainObject != null ? customFieldsMainObject.Field43 : null,
                                               Field44 = customFieldsMainObject != null ? customFieldsMainObject.Field44 : null,
                                               Field45 = customFieldsMainObject != null ? customFieldsMainObject.Field45 : null,
                                               Field46 = customFieldsMainObject != null ? customFieldsMainObject.Field46 : null,
                                               Field47 = customFieldsMainObject != null ? customFieldsMainObject.Field47 : null,
                                               Field48 = customFieldsMainObject != null ? customFieldsMainObject.Field48 : null,
                                               Field49 = customFieldsMainObject != null ? customFieldsMainObject.Field49 : null,
                                               Field50 = customFieldsMainObject != null ? customFieldsMainObject.Field50 : null,
                                           };
            return result;
        }
    }
}
