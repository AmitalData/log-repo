using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.DataContracts;
using Logitude.SystemLogs.Repositories;
using Logitude.SystemLogs.POCOs;
using Simplog.Data.Helpers;
using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public IQueryable<SharedLogisticContactPM> GetSharedLogisticContactsbyCardId(string cardId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

            CardContactRepository = new CardContactRepository(tenant);

            var contacts = CardContactRepository.GetCardContacts(tenant).Where(c => c.CardId == cardId && c.ContactId == c.ContactId && c.Tenant == tenant).Select(a => new SharedLogisticContactPM
            {
                Id = a.Id,
                CardId = cardId,
                ContactId = a.Contact.Id,
                Name = a.Contact.EnglishName,
                Email = a.Contact.Email,
                InternetAccess = a.InternetAccess,
                Tenant = a.Tenant,
                EnglishName = a.Contact.EnglishName,
                BusinessPhone = a.Contact.BusinessPhone,
                Mobile = a.Contact.Mobile,
                Fax = a.Contact.Fax,
                LastLoginDate = a.LastLoginDate,
            });

            return contacts;
        }

        public void UpdateSharedLogisticContact(SharedLogisticContactPM entityPM)
        {
            SecurityUtility.CheckContactFeature("Contact", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CardContactRepository repository = new CardContactRepository(objectContext);
            CardContact entity = repository.GetSingleCardContact(entityPM.Id, entityPM.Tenant);

            if (entity != null)
            {
                entity.InternetAccess = entityPM.InternetAccess;
            }

            repository.Update(entity);
        }

        public SharedLogisticsSummary GetSharedLogisticsSummaryData(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository = new CardRepository(objectContext);
            ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();

            SharedLogisticsSummary result = new SharedLogisticsSummary();
            IQueryable<ContactActivityLog> AllSharedLogisticsList = contactLogRep.GetSharedLogisticsContactLogs(tenant);

            List<SharedLogisticsCardLog> customersList = new List<SharedLogisticsCardLog>();
            List<SharedLogisticsCardLog> agnetsList = new List<SharedLogisticsCardLog>();
           // List<string> cardids = (from a in AllSharedLogisticsList select a.CardId).ToList();
            foreach (ContactActivityLog log in AllSharedLogisticsList)
            {
                if (!string.IsNullOrEmpty(log.PartnerTypeId))
                {
                    if (log.PartnerTypeId == "CS") customersList.Add(new SharedLogisticsCardLog() { LogDateTime = log.LogDateTime, CardId = log.CardId });
                    else if (log.PartnerTypeId == "AG") agnetsList.Add(new SharedLogisticsCardLog() { LogDateTime = log.LogDateTime, CardId = log.CardId }); 
                }
             

                //Card card = CardRepository.GetSingleCardWithoutInclude(log.CardId, tenant);

                //if (card != null)
                //{             
                //    if (card.PartnerTypeId == "CS")
                //    {
                //        customersList.Add(new SharedLogisticsCardLog() 
                //        { 
                //            LogDateTime = log.LogDateTime,
                //            CardId = card.Id,
                //        });
                //    }
                //    else if (card.PartnerTypeId == "AG")
                //    {
                //        agnetsList.Add(new SharedLogisticsCardLog()
                //        {
                //            LogDateTime = log.LogDateTime,
                //            CardId = card.Id,
                //        });
                //    }
                //}
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime todayDate1 = todayDate;
            DateTime todayDate2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime lastWeekDate = todayDate.AddDays(-7);
            DateTime yesterdayDate = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime lastMonthDate = todayDate.AddDays(-30);

            List<SharedLogisticsCardLog> TodayCustomersList = customersList.Where(d => d.LogDateTime >= todayDate1 && d.LogDateTime <= todayDate2).ToList(); ;
            List<SharedLogisticsCardLog> LastWeekCustomersList = customersList.Where(d => d.LogDateTime >= lastWeekDate && d.LogDateTime <= yesterdayDate).ToList();
            List<SharedLogisticsCardLog> LastMonthCustomersList = customersList.Where(d => d.LogDateTime >= lastMonthDate && d.LogDateTime <= yesterdayDate).ToList();

            result.TodayCustomersCount = TodayCustomersList.GroupBy(d => d.CardId).Count();
            result.LastWeekCustomersCount = LastWeekCustomersList.GroupBy(d => d.CardId).Count();
            result.LastMonthCustomersCount = LastMonthCustomersList.GroupBy(d => d.CardId).Count();

            List<SharedLogisticsCardLog> TodayAgentsList = agnetsList.Where(d => d.LogDateTime >= todayDate1 && d.LogDateTime <= todayDate2).ToList();
            List<SharedLogisticsCardLog> LastWeekAgentsList = agnetsList.Where(d => d.LogDateTime >= lastWeekDate && d.LogDateTime <= yesterdayDate).ToList();
            List<SharedLogisticsCardLog> LastMonthAgentsList = agnetsList.Where(d => d.LogDateTime >= lastMonthDate && d.LogDateTime <= yesterdayDate).ToList();

            result.TodayAgentsCount = TodayAgentsList.GroupBy(d => d.CardId).Count();
            result.LastWeekAgentsCount = LastWeekAgentsList.GroupBy(d => d.CardId).Count();
            result.LastMonthAgentsCount = LastMonthAgentsList.GroupBy(d => d.CardId).Count();

            result.Id = 1;

            return result;
        }

        public List<CardLogDetails> GetCardLogDetails(string partnerTypeId, string dateParameter, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            CardRepository = new CardRepository(objectContext);
            ContactRepository = new Simplog.Data.CommonDataModel.Repositories.ContactRepository(objectContext);

            List<CardLogDetails> result = new List<CardLogDetails>();
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();
            List<ContactActivityLog> AllSharedLogisticsList = contactLogRep.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact && d.PartnerTypeId == partnerTypeId).ToList();
            
            DateTime? date1 = null;
            DateTime? date2 = null;            
            
            switch (dateParameter)
            {
                case "T":
                    {
                        date1 = todayDate;
                        date2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;
                    }
                case "W":
                    {
                        date1 = todayDate.AddDays(-7);
                        date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;
                    }
                case "M":
                    {
                        date1 = todayDate.AddDays(-30);
                        date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;
                    }
            }

            List<ContactActivityLog> filterdList = AllSharedLogisticsList.Where(d => d.LogDateTime >= date1 && d.LogDateTime <= date2).ToList();

            int i = 0;
            result = (from item in filterdList
                      group item by new { item.CardId, item.ContactId }
                          into g
                          select new CardLogDetails()
                          {
                              Id = (i += 1),
                              CardId = g.Key.CardId,
                              ContactId = g.Key.ContactId,
                          }).ToList();
            
            foreach (CardLogDetails item in result)
            {
                Card card = CardRepository.GetSingleCardWithoutInclude(item.CardId, tenant);
                Contact contact = ContactRepository.GetSingleContact(item.ContactId, tenant);
                item.CardName = card.EnglishName;
                item.ContactName = contact.EnglishName;

                if (item.CardLogActivityDetails == null)
                {
                    item.CardLogActivityDetails = new List<CardLogActivityDetails>();
                }

                int j = 0;
                item.CardLogActivityDetails = (from r in filterdList
                                               where r.CardId == item.CardId && r.ContactId == item.ContactId
                                               select new CardLogActivityDetails()
                                               {
                                                   Id = (j += 1),
                                                   Activity = r.Activity,
                                                   CardId = r.CardId,
                                                   Module = r.Module,
                                                   PartnerTypeId = r.PartnerTypeId,
                                                   ContactId = r.ContactId,
                                                   GMTLogDateTime = r.GMTLogDateTime,
                                               }).ToList();

                item.NumberOfActivities = item.CardLogActivityDetails.Count();
            }

            return result.OrderByDescending(d => d.NumberOfActivities).ToList();
        }

        public List<CardLogActivityDetails> GetCardLogActivityDetailsList(string cardId, string contactId, string partnerTypeId, string dateParameter, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<CardLogActivityDetails> list = new List<CardLogActivityDetails>();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();
            List<ContactActivityLog> AllSharedLogisticsList = contactLogRep.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact && d.PartnerTypeId == partnerTypeId).ToList();

            DateTime? date1 = null;
            DateTime? date2 = null;

            switch (dateParameter)
            {
                case "T":
                    {
                        date1 = todayDate;
                        date2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;
                    }
                case "W":
                    {
                        date1 = todayDate.AddDays(-7);
                        date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;
                    }
                case "M":
                    {
                        date1 = todayDate.AddDays(-30);
                        date2 = todayDate.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;
                    }
            }

            List<ContactActivityLog> filterdList = AllSharedLogisticsList.Where(d => d.LogDateTime >= date1 && d.LogDateTime <= date2).ToList();

            int j = 0;
            list = (from r in filterdList
                    where r.CardId == cardId && r.ContactId == contactId
                    select new CardLogActivityDetails()
                    {
                        Id = (j += 1),
                        Activity = r.Activity,
                        CardId = r.CardId,
                        Module = r.Module,
                        PartnerTypeId = r.PartnerTypeId,
                        ContactId = r.ContactId,
                        GMTLogDateTime = r.GMTLogDateTime,
                    }).OrderByDescending(d=>d.GMTLogDateTime).ToList();

            return list;
        }

        //public List<LastLoginPartners> GetLastLoginPartners(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(tenant);
        //    }

        //    List<LastLoginPartners> myResult = new List<LastLoginPartners>();

        //    ContactActivityLogRepository myRepository = new ContactActivityLogRepository();
        //    IQueryable<ContactActivityLog> iQueryable = myRepository.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact == true && d.Activity == "Customer Access");
                
        //    int index = 0;
        //    List<LastLoginPartners> mySourceDate = (from d in iQueryable
        //                                            group d by new { d.CardId, d.ContactId } into g
        //                                            select new LastLoginPartners()
        //                                            {
        //                                                Id = g.Key.CardId + g.Key.ContactId,
        //                                                CardId = g.Key.CardId,
        //                                                ContactId = g.Key.ContactId,
        //                                            }).ToList();

        //    CardRepository myCardRepository = new Simplog.Data.CommonDataModel.Repositories.CardRepository(tenant);
        //    ContactRepository myContactRepository = new Simplog.Data.CommonDataModel.Repositories.ContactRepository(tenant);

        //    List<string> allCardsId = mySourceDate.Select(s => s.CardId).ToList();
        //    List<string> allContactsId = mySourceDate.Select(s => s.ContactId).ToList();
        //    List<Card> allCards = myCardRepository.GetCards(allCardsId, tenant).ToList();
        //    List<Contact> allContacts = myContactRepository.GetContacts(allContactsId, tenant).ToList();
        //    List<ContactActivityLog> allLogs = myRepository.GetLogs(allCardsId, allContactsId, tenant);

        //    //foreach (LastLoginPartners item in temp)
        //    //{
        //    //    ContactActivityLog log = AllSharedLogisticsList.Where(c => c.ContactId == item.ContactId && c.CardId == item.CardId).OrderByDescending(d => d.GMTLogDateTime).FirstOrDefault();

        //    //    if (log != null)
        //    //    {
        //    //        Card card = CardRepository.GetSingleCardWithoutInclude(log.CardId, tenant);
        //    //        Contact contact = ContactRepository.GetSingleContact(log.ContactId, tenant);

        //    //        result.Add(new LastLoginPartners() 
        //    //        {
        //    //            Id = item.Id,
        //    //            CardId = log.CardId,
        //    //            CardName = card != null ? card.EnglishName : "",
        //    //            ContactId = log.ContactId,
        //    //            ContactName = contact != null ? contact.EnglishName : "",
        //    //            PartnerTypeName = card == null ? "" : (card.PartnerType == null ? "" : card.PartnerType.Name),
        //    //            LastAccess = log.GMTLogDateTime,
        //    //        });
        //    //    } 
        //    //}
        //    return myResult;
        //    //return result.Take(10).ToList();
        //}

        public List<LastLoginPartners> GetLastLoginPartners(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            List<LastLoginPartners> result = new List<LastLoginPartners>();
            ContactActivityLogRepository contactLogRep = new ContactActivityLogRepository();
            List<ContactActivityLog> AllSharedLogisticsList = contactLogRep.GetContactActivityLogs(tenant).Where(d => d.IsSharedLogisticsContact && (d.Activity == "Customer Access")).ToList(); // || d.Activity == "Agent Access"
            CardRepository = new Simplog.Data.CommonDataModel.Repositories.CardRepository(objectContext);
            ContactRepository = new Simplog.Data.CommonDataModel.Repositories.ContactRepository(objectContext);

            int i = 0;
            List<LastLoginPartners> temp = (from r in AllSharedLogisticsList
                                            group r by new { r.CardId, r.ContactId , r.Via }
                                                into g
                                                select new LastLoginPartners()
                                                {
                                                    Id = (i += 1),
                                                    CardId = g.Key.CardId,
                                                    ContactId = g.Key.ContactId,
                                                    Via = g.Key.Via,
                                                    
                                                }).ToList();

            foreach (LastLoginPartners item in temp)
            {
             
                ContactActivityLog log = AllSharedLogisticsList.Where(c => c.ContactId == item.ContactId && c.CardId == item.CardId).OrderByDescending(d => d.GMTLogDateTime).FirstOrDefault();

                if (log != null)
                {
                    Card card = CardRepository.GetSingleCardWithoutInclude(log.CardId, tenant);
                    Contact contact = ContactRepository.GetSingleContact(log.ContactId, tenant);

                    result.Add(new LastLoginPartners()
                    {
                        Id = item.Id,
                        CardId = log.CardId,
                        CardName = card != null ? card.EnglishName : "",
                        ContactId = log.ContactId,
                        ContactName = contact != null ? contact.EnglishName : "",
                        PartnerTypeName = card == null ? "" : (card.PartnerType == null ? "" : card.PartnerType.Name),
                        LastAccess = log.GMTLogDateTime,
                        Via = log.Via,
                    });
                }
            }

            return result.OrderByDescending(d => d.LastAccess).Take(10).ToList();
        }
    }
}