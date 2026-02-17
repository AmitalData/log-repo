using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class TipQuery
    {
        TipRepository repository;
        public TipQuery()
        {
            repository = new TipRepository(); 
        }

        public TipQuery(int tenant)
        {
            repository = new TipRepository(tenant);
        }

        public TipQuery(TipRepository tipRepository)
        {
            repository = tipRepository;
        }

        public List<TipPM> GetTipsPMs()
        {
            IQueryable<TipPM> tips = from a in repository.context.Tips.Include("TextCode")

                                     select new TipPM()
                                     {
                                         Code = a.Code,
                                         ShortTextCodeId = a.ShortTextCode,
                                         Tenant = a.Tenant,
                                         VisibilityDefaultValue = a.VisibilityDefaultValue,
                                         ObjectTableId = a.ObjectTableId,
                                         ShortTextCodeCode = a.TextCode.Code,
                                     };

            return tips.ToList();
        }

        public TipPM GetSingleTipPM(string code, int tenant)
        {
            TipPM tip = (from a in repository.context.Tips.Include("TextCode")
                         where a.Code == code && a.Tenant == tenant
                         select new TipPM()
                         {
                             Code = a.Code,
                             ShortTextCodeId = a.ShortTextCode,
                             Tenant = a.Tenant,
                             VisibilityDefaultValue = a.VisibilityDefaultValue,
                             ObjectTableId = a.ObjectTableId,
                             ShortTextCodeCode = a.TextCode.Code,
                         }).FirstOrDefault();

            return tip;
        }



    }
}