using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CardContactRepository:IRepository<CardContact>
    {
        ICommonDataContext commonDataContext;
        public CardContactRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }


        

        public CardContactRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CardContact> GetCardContacts(int tenant)
        {
            return (from d in context.CardContacts.Include("Contact").Include("Card.Customer").Include("Card.Customer.SalesmanUser").Include("Card.Agent")
                    where d.Tenant == tenant
                    select d);
        }


        public IQueryable<CardContact> GetCardsContactsForContactIds(List<string> contactIdsList, int tenant)
        {
            return (from d in context.CardContacts.Include("Card.Customer")
                    where d.Tenant == tenant && contactIdsList.Contains(d.ContactId)
                    select d);
        }

        public int GetCardsContactsForContactIds_Count(List<string> contactIdsList, int tenant)
        {
            var cardsList = (from d in context.CardContacts.Include("Card.Customer")
                             where d.Tenant == tenant && contactIdsList.Contains(d.ContactId) && (d.Card != null && d.Card.IsCustomer)
                             select d).GroupBy(a => a.CardId).ToList();

            return cardsList != null ? cardsList.Count() : 0;
        }

        public string GetCardsContactsForContactIds_Ids(List<string> contactIdsList, int tenant)
        {
            var ids = "";
            List<string> cardsList = (from d in context.CardContacts.Include("Card.Customer")
                                      where d.Tenant == tenant && contactIdsList.Contains(d.ContactId) && (d.Card != null && d.Card.IsCustomer)
                                      select d).GroupBy(a => a.CardId)
                            .Select(grp => grp.FirstOrDefault().CardId).ToList();

            if (cardsList != null && cardsList.Count() > 0)
            {
                ids = string.Join(",", cardsList);
                ids.TrimEnd(',');
            }
            return ids;
        }


        public string GetCardsContactsForContactIds_Names(List<string> contactIdsList, int tenant)
        {
            var ids = "";
            List<string> cardsList = (from d in context.CardContacts.Include("Card.Customer")
                                      where d.Tenant == tenant && contactIdsList.Contains(d.ContactId) && (d.Card != null && d.Card.IsCustomer)
                                      select d).GroupBy(a => a.Card).Select(grp => grp.FirstOrDefault().Card.EnglishName).ToList();
            if (cardsList != null && cardsList.Count() > 0)
            {
                ids = string.Join(",", cardsList);
                ids.TrimEnd(',');
            }
            return ids;
        }

        public List<CardContact> GetCardContactForContact(string contactId, int tenant)
        {
            return (from record in context.CardContacts.Include("Card")
                    where record.ContactId == contactId select record).ToList();
        }

        public IQueryable<Contact> GetContactsByCardId(string cardId)
        {
            return (from d in context.CardContacts.Include("Contact") where d.CardId == cardId select d.Contact);
        }

        public IQueryable<Contact> GetContactsByCardIds(List<string> cardIds)
        {
            return (from d in context.CardContacts.Include("Contact") where cardIds.Contains(d.CardId) select d.Contact);
        }

        public IQueryable<CardContact> GetCardContactsByCardId(string cardId)
        {
            return (from record in context.CardContacts.Include("Contact") where record.CardId == cardId select record);
        }

        public CardContact GetSingleCardContact(string id, int tenant)
        {
            return (from record in context.CardContacts.Include("Contact") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public CardContact GetSingleCardContactByContactId(string contactId, int tenant)
        {
            return (from record in context.CardContacts.Include("Contact") where record.ContactId == contactId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public List<Card> GeContactCustomersByContact(string contactId,string searchText, int tenant)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return (from record in context.CardContacts.Include("Card").Include("Contact") where record.ContactId == contactId && record.Tenant == tenant && record.Card.PartnerTypeId == "CS" select record.Card).ToList();
            }
            else
            {
                return (from record in context.CardContacts.Include("Card").Include("Contact") where record.ContactId == contactId && record.Tenant == tenant && record.Card.PartnerTypeId == "CS" && record.Card.SearchFields.Contains(searchText) select record.Card).ToList();
            }
        }

        public CardContact GetSingleCardContact(string contactId,string cardId, int tenant)
        {
            return (from record in context.CardContacts.Include("Contact") where record.ContactId == contactId && record.CardId == cardId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public CardContact GetSingleCardContactByExternal(string contactExternalId, string cardCode, int tenant)
        {
            return (from record in context.CardContacts.Include("Contact").Include("Card") where record.Contact.ExternalId == contactExternalId && record.Card.Code == cardCode && record.Tenant == tenant select record).FirstOrDefault();
        }

        public string GetSinglePartnerContactId(string cardId)
        {
            string result = null;

            List<CardContact> cardContact = context.CardContacts.Include("Contact").Where(d => d.CardId == cardId).ToList();
            if (cardContact.Count == 1)
            {
                result = cardContact.First().ContactId;
            }

            return result;
        }

        public CardContact GetCardContactByContactAndCard(string cardId, string contactId, int tenant)
        {
            return (from record in context.CardContacts.Include("Contact") where record.CardId == cardId && record.Tenant == tenant && record.ContactId == contactId select record).FirstOrDefault();
        }

        public void Add(CardContact entity)
        {
            context.CardContacts.Add(entity);

        }

        public void Remove(CardContact entity)
        {
            try
            {
                context.CardContacts.Attach(entity);
            }
            catch { }
            context.CardContacts.Remove(entity);
        }

        public void Update(CardContact entity)
        {
            context.CardContacts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CardContact> All()
        {
            return context.CardContacts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CardContact> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CardContact GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<CardContact> GetCardsContactsForCustomerIds(List<string> customerIdsList, int tenant)
        {
            return (from d in context.CardContacts.Include("Contact").Include("Card.Customer")
                    where d.Tenant == tenant && customerIdsList.Contains(d.CardId)
                    select d);
        }
    }
}