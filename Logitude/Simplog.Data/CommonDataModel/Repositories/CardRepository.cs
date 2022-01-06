using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Text;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CardRepository : IRepository<Card>
    {
        ICommonDataContext commonDataContext;

        public CardRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CardRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CardRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public List<Card> GetCardsBySearch(string name, string code, string city, string country, int tenant)
        {
            List<Card> tenantResult = context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Where(d => d.Tenant == tenant && d.PartnerTypeId == "CS" && !d.InActive).ToList();
            List<Card> nameResult = string.IsNullOrEmpty(name) ? tenantResult : tenantResult.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper())).ToList();
            List<Card> codeResult = string.IsNullOrEmpty(code) ? nameResult : nameResult.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper())).ToList();

            //PartnersDomainService partnersContext = new PartnersDomainService();
            AddressRepository addressRepository = new AddressRepository(commonDataContext);
            CountryRepository countryRepository = new CountryRepository(commonDataContext);
            List<Card> cityResult = new List<Card>();
            if (string.IsNullOrEmpty(city))
            {
                cityResult = codeResult;
            }

            else
            {
                foreach (Card item in codeResult)
                {
                    Address addr = addressRepository.GetMainAddressByCardId(item.Id, item.Tenant);
                    if (addr.City != null)
                    {
                        if (addr.City.ToUpper().StartsWith(city.ToUpper()))
                        {
                            cityResult.Add(item);
                        }
                    }
                }
            }

            List<Card> countryResult = new List<Card>();
            if (string.IsNullOrEmpty(country))
            {
                countryResult = cityResult;
            }

            else
            {
                foreach (Card item in cityResult)
                {
                    Address addr = addressRepository.GetMainAddressByCardId(item.Id, item.Tenant);
                    Country addressCountry = countryRepository.GetSingleCountry(addr.CountryId, tenant);
                    if (addressCountry.EnglishName != null)
                    {
                        if (addressCountry.EnglishName.ToUpper().StartsWith(country.ToUpper()))
                        {
                            countryResult.Add(item);
                        }
                    }
                }
            }

            return countryResult;
        }

        public IQueryable<Card> GetCards(int tenant)
        {
            return (from d in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent")
                    where d.Tenant == tenant
                    select d);
        }

        public IQueryable<Card> GetCarrierCards(int tenant)
        {
            return (from record in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent")
                    where record.Tenant == tenant && (record.PartnerTypeId == "AL" ||
                        record.PartnerTypeId == "SL" || record.PartnerTypeId == "TR")
                    select record);
        }

        public bool IsAirlineExistsInTenant(string myCode, int tenant)
        {
            bool myResult = (from d in context.Cards
                             where d.Tenant == tenant
                             && d.Code == myCode
                             && d.PartnerTypeId == "AL"
                             select d).Any();

            return myResult;
        }
        public Card GetSingleCardCache(string id, int Tenant)
        {
            string entityKeyString = $"GetSingleCard({id},{Tenant})";
            var res = CacheManager.GetOrInsertNewObject<Card>(entityKeyString, () =>
            {
                //return this.GetSingleCard(id, Tenant);
                Card entity = Queryable.FirstOrDefault<Card>((from a in context.Cards

                                                                  //.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("ImageDetail").Include("InvoiceCurrency").Include("VatType").Include("Trucker").Include("ShippingLine").Include("CustomAgent").Include("ShippingAgent").Include("Warehouse").Include("Agent").Include("Vendor")

                                                              where a.Id == id
                                                              select a));

                return entity;

            });
            return res;
        }
        public Card GetSingleCard(string id, int tenant)
        {
            Card entity = Queryable.FirstOrDefault<Card>((from a in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("ImageDetail").Include("InvoiceCurrency").Include("VatType").Include("Trucker").Include("ShippingLine").Include("CustomAgent").Include("ShippingAgent").Include("Warehouse").Include("Agent").Include("Vendor")
                                                          where a.Id == id
                                                          select a));

            return entity;
        }
        public Card GetCardWithCollecter(string id, int tenant)
        {
            var card = context.Cards
                .Include("CollectorUser.Contact")
                .Include("CollectorUser.Contact")
                .Where(e => e.Id == id && e.Tenant == tenant).FirstOrDefault();
            return card;
        }
        public Card Get(string id, int tenant)
        {
            Card entity = Queryable.FirstOrDefault<Card>((from a in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("ImageDetail").Include("InvoiceCurrency").Include("VatType").Include("Trucker").Include("ShippingLine").Include("CustomAgent").Include("ShippingAgent").Include("Warehouse").Include("Agent").Include("Vendor")
                                                          where a.Id == id
                                                          select a));

            return entity;
        }

        public Card GetSingleCardWithoutInclude(string id, int tenant)
        {
            Card entity = Queryable.FirstOrDefault<Card>((from a in context.Cards.Include("PartnerType")
                                                          where a.Id == id
                                                          select a));

            return entity;
        }

        public string GetAccountingCardFromCard(string id, int tenant)
        {
            string accountingCard = (from a in context.Cards
                                     where a.Id == id
                                     && a.Tenant == tenant
                                     select a.ReceivablesAccountingCard).FirstOrDefault();

            return accountingCard;
        }

        public Card GetCardByGLAccountId(string id, int tenant, bool fromCache = false)
        {

            string key = $"GetCardByGLAccountId({id},{tenant})";
            var cardFromCache = CacheManager.GetOrInsertNewObject<Card>(key, () =>
            {
                Card card = (from a in context.Cards
                             where a.Tenant == tenant
                             && a.GLAccountId == id
                             select a).FirstOrDefault();
                return card;
            }, fromCache);


            return cardFromCache;
        }


        public List<Card> GetCardsByGLAccountIds(List<string> glaccountIds, int tenant)
        {



            List<Card> cards = (from a in context.Cards
                                where a.Tenant == tenant
                                && glaccountIds.Contains(a.GLAccountId)
                                select a).ToList();
            return cards;




        }



        public List<string> GetGLAccountIdssByCardIds(List<string> Ids, int tenant)
        {



            List<string> cards = (from a in context.Cards
                                  where a.Tenant == tenant
                                  && Ids.Contains(a.Id)
                                  select a.GLAccountId).ToList();
            return cards;




        }

        public string GetGLAccountIdByCardId(string cardId, int tenant)
        {
            string cards = (from a in context.Cards
                            where a.Tenant == tenant
                            && a.Id == cardId
                            select a.GLAccountId).FirstOrDefault();
            return cards;
        }

        public Card GetSingleCardByCode(string code, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Card" + code + tenant;
                Card entity;
                if (getFromCache)
                {

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        entity = (from a in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus")
                                  where a.Tenant == tenant && a.Code == code
                                  select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (Card)CacheManager.CacheWrapper.Get(entityName);
                    }


                }
                else
                {
                    entity = (from record in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus") where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
        public Card GetSingleCardByCodeAndType(string code, string partnerTypeId, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(partnerTypeId))
            {
                string entityName = "Card" + code + tenant + partnerTypeId;
                Card entity;
                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        entity = (from d in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus")
                                  where
                                  d.Tenant == tenant
                                  && d.Code == code
                                  && d.PartnerTypeId == partnerTypeId
                                  select d).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }

                    else
                    {
                        entity = (Card)CacheManager.CacheWrapper.Get(entityName);
                    }

                }

                else
                {
                    entity = (from d in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus")
                              where
                              d.Tenant == tenant
                              && d.Code == code
                              && d.PartnerTypeId == partnerTypeId
                              select d).FirstOrDefault();
                }

                return entity;
            }

            return null;
        }

        public Card GetSingleCardByCodeForHybrid(string code, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Card" + code + tenant;
                Card entity;
                if (getFromCache)
                {

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        entity = (from a in context.Cards
                                  where a.Tenant == tenant && a.Code == code
                                  select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (Card)CacheManager.CacheWrapper.Get(entityName);
                    }


                }
                else
                {
                    entity = (from record in context.Cards where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
        public static Card GetSingleCard(string id, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "Card" + id + tenant;
                Card entity;
                if (getFromCache)
                {

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);
                        entity = (from a in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent").Include("Customer.AccountManagerUser.Contact")
                                  where a.Tenant == tenant && a.Id == id
                                  select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (Card)CacheManager.CacheWrapper.Get(entityName);
                    }


                }
                else
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    entity = (from record in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent")
                              where record.Id == id && record.Tenant == tenant
                              select record).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public Card GetSingleCardByIdAndTenant(string id, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "Card" + id + tenant;
                Card entity;
                if (getFromCache)
                {

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        entity = (from a in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent").Include("Customer.AccountManagerUser.Contact")
                                  where a.Tenant == tenant && a.Id == id
                                  select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (Card)CacheManager.CacheWrapper.Get(entityName);
                    }


                }
                else
                {

                    entity = (from record in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent")
                              where record.Id == id && record.Tenant == tenant
                              select record).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public string GetCardIdByCode(string code, int tenant)
        {
            string entityId = null;
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "Card_CachedId" + code + tenant;



                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {

                    entityId = (from a in context.Cards
                                where a.Tenant == tenant && a.Code == code
                                select a.Id).FirstOrDefault();

                    if (CacheManager.CacheWrapper.Get(entityName) == null && entityId != null)
                    {
                        CacheManager.CacheWrapper.Insert(entityName, entityId, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    entityId = (string)CacheManager.CacheWrapper.Get(entityName);
                }


            }

            return entityId;
        }



        public string GetActiveCardIdByCode(string code, int tenant)
        {

            string entityId = (from a in context.Cards
                               where a.Tenant == tenant && a.Code == code && !a.InActive
                               select a.Id).FirstOrDefault();

            return entityId;
        }



        public Card GetCardWithPrimaryContactById(string id, int tenant)
        {
            return (from a in context.Cards.Include("PrimaryContact")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }



        public IQueryable<Card> GetCards(List<string> allCardsId, int tenant)
        {
            IQueryable<Card> myResult = null;

            if (allCardsId.Count > 0)
            {
                myResult = context.Cards.Where(d => allCardsId.Contains(d.Id));
            }

            return myResult;
        }

        public void Add(Card entity)
        {
            context.Cards.Add(entity);
        }

        public void Remove(Card entity)
        {
            try
            {
                context.Cards.Attach(entity);
            }

            catch
            {
            };
            context.Cards.Remove(entity);
        }

        public void Update(Card entity)
        {
            try
            {
                context.Cards.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<Card> All()
        {
            return context.Cards.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Card> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Card GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Card GetSingleCardByVatNumber(string vatNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(vatNumber)) return null;
            Card entity = Queryable.FirstOrDefault<Card>((from a in context.Cards
                                                          where a.VatNumber == vatNumber && a.Tenant == tenant
                                                          select a));

            return entity;
        }

        public Dictionary<string, string> GetCustomerNamesById(List<string> customerIds)
        {
            //   Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, string> customers = (from a in context.Cards
                                                    where customerIds.Contains(a.Id)
                                                    select a).ToDictionary(d => d.Id, f => f.LocalName != null ? f.LocalName : f.EnglishName);



            //foreach (var item in DeclarationCustomerIds)
            //{
            //    var customerName = (from a in customers where a.Key == item.Value select a.Value);
            //    result.Add(item.Key, customerName.ToString());

            //}
            return customers;
        }

        public IQueryable<Card> GetCardsByIds(List<string> cardIds, int tenant)
        {
            IQueryable<Card> cardLists = (from a in context.Cards
                                          where cardIds.Contains(a.Id)
                                          && a.Tenant == tenant
                                          select a);
            return cardLists;
        }
        public List<Card> GetAllCardsByContactId(string contactId, int tenant)
        {
            return context.Cards.Where(a => a.PrimaryContactId == contactId && a.Tenant == tenant).ToList();
        }


        public IQueryable<Card> GetWarehouseCards(int tenant)
        {
            return (from record in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("Agent")
                    where record.Tenant == tenant && record.PartnerTypeId == "WH"
                    select record);
        }


        public string GetEnglishNameCardById(string id, int tenant)
        {
            string englishName = ((from a in context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus").Include("ImageDetail").Include("InvoiceCurrency").Include("VatType").Include("Trucker").Include("ShippingLine").Include("CustomAgent").Include("ShippingAgent").Include("Warehouse").Include("Agent").Include("Vendor")
                                   where a.Id == id
                                   select a.EnglishName)).FirstOrDefault();

            return englishName;
        }

        public IQueryable<Card> GetAirlineCards(int tenant)
        {
            return (from record in context.Cards
                    where record.Tenant == tenant && record.PartnerTypeId == "AL"
                    select record);
        }

        public bool IsUploadingUniqueKeyExist(string uniqueCode)
        {
            return (from a in context.Cards
                    where a.UploadingUniqueKey == uniqueCode
                    select a).Any();
        }

        public Card GetSingleCardByUniqueCode(string unique, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(unique))
            {
                string entityName = "Card" + unique + tenant;
                Card entity;
                if (getFromCache)
                {

                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        entity = (from a in context.Cards
                                  where a.Tenant == tenant && a.UploadingUniqueKey == unique
                                  select a).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (Card)CacheManager.CacheWrapper.Get(entityName);
                    }


                }
                else
                {
                    entity = (from record in context.Cards where record.UploadingUniqueKey == unique && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public List<Card> GetCardsFromIdList(List<string> ids, int tenant)
        {
            ICommonDataContext cardViewContext = CommonDataContext.GetContext(tenant);
            List<Card> cards = new List<Card>();
            if (ids.Count() != 0)
            {
                StringBuilder values = new StringBuilder();
                values.AppendFormat("{0}", "'" + ids[0] + "'");
                for (int i = 1; i < ids.Count; i++)
                    values.AppendFormat(", {0}", "'" + ids[i] + "'");

                string sql = string.Format("SELECT * FROM CARDS WHERE ID IN ({0})", values);
                cards = cardViewContext.GetActiveDbContext().Database.SqlQuery<Card>(sql).ToList();
            }

            return cards;
        }

    }
}
