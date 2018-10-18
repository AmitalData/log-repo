using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class TipsVisibilityQuery
    {
               TipsVisibilityRepository repository;
        public TipsVisibilityQuery()
        {
            repository = new TipsVisibilityRepository(); 
        }

        public TipsVisibilityQuery(int tenant)
        {
            repository = new TipsVisibilityRepository(tenant);
        }

        public TipsVisibilityQuery(TipsVisibilityRepository tipsVisibilityRepository)
        {
            repository = tipsVisibilityRepository;
        }

        public List<TipsVisibilityPM> GetTipsVisibilities(int tenant, string userId)
        {
            List<TipsVisibilityPM> tipsVisibilities = (from a in this.repository.context.TipsVisibilities
                                                       where a.Tenant == tenant && a.UserId == userId
                                                       select new TipsVisibilityPM()
                                                       {
                                                           Id = a.Id,
                                                           IsVisible = a.IsVisible,
                                                           Tenant = a.Tenant,
                                                           TipCode = a.TipCode,
                                                           UserId = a.UserId
                                                       }
                                                     ).ToList();

            return tipsVisibilities;
        }

        public TipsVisibilityPM GetSingleTipsVisibilityPM(string id, int tenant)
        {
            TipsVisibilityPM tipsVisibility = (from a in this.repository.context.TipsVisibilities
                                               where a.Tenant == tenant && a.Id == id
                                               select new TipsVisibilityPM()
                                               {
                                                   Id = a.Id,
                                                   IsVisible = a.IsVisible,
                                                   Tenant = a.Tenant,
                                                   TipCode = a.TipCode,
                                                   UserId = a.UserId
                                               }
                                                     ).FirstOrDefault();

            return tipsVisibility;
        }



        public List<TipsVisibilityPM> GetTipsVisibilitiesByTipCode(int tenant, string userId, string tipCode)
        {
            List<TipsVisibilityPM> tipsVisibilities = (from a in this.repository.context.TipsVisibilities
                                                       where a.Tenant == tenant && a.UserId == userId && a.TipCode == tipCode
                                                       select new TipsVisibilityPM()
                                                       {
                                                           Id = a.Id,
                                                           IsVisible = a.IsVisible,
                                                           Tenant = a.Tenant,
                                                           TipCode = a.TipCode,
                                                           UserId = a.UserId
                                                       }
                                                     ).ToList();

            return tipsVisibilities;
        }



    }
}