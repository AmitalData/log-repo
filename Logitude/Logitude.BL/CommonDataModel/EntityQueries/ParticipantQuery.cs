using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;

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
                                 },

                             }).FirstOrDefault();

            ParticipantPM securedPm = new ParticipantPM();
            SecuredMapping.GetMappedPM(Participant, securedPm, "Participant", tenant);

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
            IQueryable<ParticipantList> result = from a in iQueryable.Include("Card").Include("Forwarder")
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
                                           };
            return result;
        }
    }
}
