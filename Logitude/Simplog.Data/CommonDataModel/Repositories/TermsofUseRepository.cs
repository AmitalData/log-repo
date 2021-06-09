using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TermsofUseRepository : IRepository<TermsofUse>
    {
        ICommonDataContext commonDataContext;

        public TermsofUseRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TermsofUseRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TermsofUseRepository(int version)
        {
            commonDataContext = CommonDataContext.GetContext(version);
            
        }

        public void Add(TermsofUse entity)
        {
            context.TermsofUses.Add(entity);
        }

        public void Remove(TermsofUse entity)
        {
            context.TermsofUses.Attach(entity);
            context.TermsofUses.Remove(entity);
        }
        
        public void Update(TermsofUse entity)
        {
            context.TermsofUses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TermsofUse> All()
        {
            return context.TermsofUses.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public int GetTermsofUseCount()
        {
            return (from record in context.TermsofUses select record).Count();
        }

        public TermsofUse GetSingleTermsofUse(DateTime toUdate, int version)
        {
            return (from record in context.TermsofUses where record.Date == toUdate && record.VersionNumber == version select record).FirstOrDefault();
        }


        public int GetLastTermsofUseVersionNumber(string privateLabeldId)
        {
            // private label id 
            return (from record in context.TermsofUses where record.PrivateLabelId == privateLabeldId select record).OrderByDescending(d => d.VersionNumber).Select(d=>d.VersionNumber).FirstOrDefault();
        }

        public int GetLastTermsofUseVersionNumberForTenantZero()
        {
             
            return (from record in context.TermsofUses where record.Tenant == 0 select record).OrderByDescending(d => d.VersionNumber).Select(d => d.VersionNumber).FirstOrDefault();
        }


        public IQueryable<TermsofUse> GetTermsofUses()
        {
            return context.TermsofUses;
        }

        public IQueryable<TermsofUse> GetTermsofUsesByVersion(int version)
        {
            IQueryable<TermsofUse> termsofUses = from a in context.TermsofUses
                                                 where a.VersionNumber == version
                                                 select a;
            return termsofUses;
        }

        public IQueryable<TermsofUse> GetById(int id)
        {
            IQueryable<TermsofUse> termsofUses = from a in context.TermsofUses
                                                 where a.Id == id
                                                 select a;
            return termsofUses;
        }


        public List<TermsofUse> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TermsofUse GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
