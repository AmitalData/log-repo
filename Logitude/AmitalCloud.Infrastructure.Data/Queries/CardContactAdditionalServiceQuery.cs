using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;   
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AmitalCloud.Infrastructure.Domain.Interfaces;
namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class CardContactAdditionalServiceQuery
    {
        IRepository<CardContactAdditionalService> repository;
        IAmitalCloudContext context;
        public CardContactAdditionalServiceQuery() :this(0)
        {
        }
        public CardContactAdditionalServiceQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository< CardContactAdditionalService>(context);
        }
        public CardContactAdditionalServiceQuery(IRepository<CardContactAdditionalService> repository)
        {
            this.repository = repository;
        }
        public CardContactAdditionalServicePM GetSinglePM(string id, int tenant)
        {
            CardContactAdditionalServicePM entity = (from a in context.CardContactAdditionalServices.Include("AdditionalService")
                                           where a.Tenant == tenant && a.Id == id
                                           select new CardContactAdditionalServicePM()
                                           {
                                               Id = a.Id,
                                               CardContactId = a.CardContactId,
                                               Tenant = a.Tenant,
                                               AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                                               AdditionalServiceId = a.AdditionalServiceId,
                                           }).FirstOrDefault();

            return entity;
        }

        public List<CardContactAdditionalServicePM> GetCardContactAdditionalServicePMsByCardContactId(string CardContactId, int tenant)
        {
            List<CardContactAdditionalServicePM> result =
                (from a in context.CardContactAdditionalServices.Include("AdditionalService")
                 where a.Tenant == tenant && a.CardContactId == CardContactId
                 select new CardContactAdditionalServicePM()
                 {
                     Id = a.Id,
                     CardContactId = a.CardContactId,
                     Tenant = a.Tenant,
                     AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                     AdditionalServiceId = a.AdditionalServiceId,
                 }).ToList();

            return result;
        }
    }
}
