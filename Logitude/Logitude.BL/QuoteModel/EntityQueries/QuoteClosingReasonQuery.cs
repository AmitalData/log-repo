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
    public class QuoteClosingReasonQuery
    {
        QuoteClosingReasonRepository repository;

        public QuoteClosingReasonQuery()
        {
            repository = new QuoteClosingReasonRepository(); 
        }

        public QuoteClosingReasonQuery(int tenant)
        {
            repository = new QuoteClosingReasonRepository(tenant);
        }

        public QuoteClosingReasonQuery(QuoteClosingReasonRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public QuoteClosingReasonPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.QuoteClosingReasons.Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select new QuoteClosingReasonPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdatedByUserId = a.UpdatedByUserId,
                        Inactive = a.Inactive,
                        CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                        UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                    }).FirstOrDefault();
        }

        public IQueryable<QuoteClosingReasonList> GetIQueryableEntityList(IQueryable<QuoteClosingReason> iQueryable)
        {
            IQueryable<QuoteClosingReasonList> result = from a in iQueryable
                                                        select new QuoteClosingReasonList()
                                                        {
                                                            Code = a.Code,
                                                            Name = a.Name,
                                                            SearchFields = a.SearchFields,
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            Inactive = a.Inactive,
                                                            CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                                                            UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                                                        };
            return result;
        }
    }
}