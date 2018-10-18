using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public IQueryable<CardContact> GetCardContacts(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CardContactRepository = new CardContactRepository(tenant);
            return CardContactRepository.GetCardContacts(0);
        }

        public IQueryable<CardContactPM> GetCardContactsByContactID(string contactId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            cardContactQuery = new CardContactQuery(tenant);
            return cardContactQuery.GetCardContactPMsByTenant(tenant).Where(cc => cc.ContactId == contactId && cc.Tenant == tenant);
        }

        public IQueryable<CardContactPM> GetCardContactsByCardID(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            cardContactQuery = new CardContactQuery(tenant);
            return cardContactQuery.GetCardContactPMsByTenant(tenant).Where(cc => cc.CardId == cardId && cc.Tenant == tenant);
        }

        //public void MapCardContactCardContactPM(CardContactPM cardContactPm, CardContact cardContact)
        //{
        //    cardContact.CardId = cardContactPm.CardId;
        //    cardContact.ContactId = cardContactPm.ContactId;
        //    cardContact.Tenant = cardContactPm.Tenant;
        //}

        public void InsertCardContact(CardContactPM cardContact)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(cardContact.Tenant);
            }
            CardContactService service = new CardContactService(objectContext, cardContact.Tenant);
            service.Create(cardContact);

            //CardContactRepository = new CardContactRepository(objectContext);
            
            //CardContact newEntity = new CardContact();
            //newEntity.Id = IdCounter.GetNumber("CardContact", cardContact.Tenant).ToString();
            //cardContact.Id = newEntity.Id;
            //MapCardContactCardContactPM(cardContact, newEntity);
            //CardContactRepository.Add(newEntity);
        }

        public void UpdateCardContact(CardContactPM currentCardContact)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentCardContact.Tenant);
            }
            CardContactService service = new CardContactService(objectContext, currentCardContact.Tenant);
            service.Update(currentCardContact);

            //CardContactRepository = new CardContactRepository(objectContext);
            //CardContact entity = CardContactRepository.GetSingleCardContact(currentCardContact.Id, currentCardContact.Tenant);
            //MapCardContactCardContactPM(currentCardContact, entity);
            //CardContactRepository.Update(entity);
        }

        public void DeleteCardContact(CardContactPM cardContact)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(cardContact.Tenant);
            }
            CardContactRepository = new CardContactRepository(objectContext);
            CardContact entity = CardContactRepository.GetSingleCardContact(cardContact.Id, cardContact.Tenant);
            CardContactRepository.Remove(entity);
        }
    }
}
