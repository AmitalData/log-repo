using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardContactAdditionalServiceQuery
    {
        CardContactAdditionalServiceRepository repository;


        public CardContactAdditionalServiceQuery(int tenant)
        {
            repository = new CardContactAdditionalServiceRepository(tenant);
        }

        public CardContactAdditionalServiceQuery(CardContactAdditionalServiceRepository CardContactAdditionalServiceRepository)
        {
            repository = CardContactAdditionalServiceRepository;
        }

        public CardContactAdditionalServicePM GetSinglePM(string id, int tenant)
        {
            CardContactAdditionalServicePM entity = (from a in repository.context.CardContactAdditionalServices.Include("AdditionalService")
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
                (from a in repository.context.CardContactAdditionalServices.Include("AdditionalService")
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
