using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteStageQuery
    {
        QuoteStageRepository repository;

        public QuoteStageQuery(int tenant)
        {
            repository = new QuoteStageRepository(tenant);
        }

        public QuoteStageQuery(QuoteStageRepository repository)
        {
            this.repository = repository;
        }

        public QuoteStagePM GetSinglePM(string id, int tenant)
        {
            QuoteStagePM entityPM = (from a in repository.Context.QuoteStages.Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                                     where a.Id == id && a.Tenant == tenant
                                     select new QuoteStagePM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                Code = a.Code,
                                                Name = a.Name,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                                                MaxDays = a.MaxDays,
                                                SearchFields = a.SearchFields,
                                                Rank = a.Rank,
                                                InActive = a.InActive,
                                            }).FirstOrDefault();


            return entityPM;
        }

        public IQueryable<QuoteStageList> GetIQueryableEntityList(IQueryable<QuoteStage> iQueryable)
        {
            IQueryable<QuoteStageList> myResult =
                from a in iQueryable
                select new QuoteStageList()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    Code = a.Code,
                    Name = a.Name,
                    UpdateDate = a.UpdateDate,
                    UpdatedByUserId = a.UpdatedByUserId,
                    UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                    MaxDays = a.MaxDays,
                    SearchFields = a.SearchFields,
                    Rank = a.Rank,
                    InActive = a.InActive,
                };

            return myResult;
        }

        public QuoteStageList GetSingleListByCode(string code, int tenant)
        {
            QuoteStageList entityPM = (from a in repository.Context.QuoteStages
                                     where a.Code == code && a.Tenant == tenant
                                       select new QuoteStageList()
                                     {
                                         Id = a.Id,
                                         Tenant = a.Tenant,
                                         Code = a.Code,
                                         Name = a.Name,
                                         UpdateDate = a.UpdateDate,
                                         UpdatedByUserId = a.UpdatedByUserId,
                                         UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                                         MaxDays = a.MaxDays,
                                         SearchFields = a.SearchFields,
                                         Rank = a.Rank,
                                         InActive = a.InActive,
                                     }).FirstOrDefault();


            return entityPM;
        }
    }
}