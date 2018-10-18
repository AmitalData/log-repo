using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TermsofUseQuery
    {
        TermsofUseRepository repository;

        public TermsofUseQuery()
        {
            repository = new TermsofUseRepository(); 
        }

        public TermsofUseQuery(int tenant)
        {
            repository = new TermsofUseRepository(tenant);
        }

        public TermsofUseQuery(TermsofUseRepository repository)
        {
            this.repository = repository;
        }

        public TermsofUsePM GetSinglePM(DateTime toUdate, int version)
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses
                                        where a.Version == version
                                        && a.Date == toUdate
                                        select new TermsofUsePM()
                                        {
                                            Date = a.Date,
                                            Version = a.Version,
                                        }).FirstOrDefault();
            return termsofUses;
        }
        
        public TermsofUsePM GetSinglePMByVersion(int version)
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses
                                        where a.Version == version

                                        select new TermsofUsePM()
                                        {
                                            Date = a.Date,
                                            Version = a.Version,
                                        }).FirstOrDefault();
            return termsofUses;
        }
        
        public IQueryable<TermsofUsePM> GetTermsofUsePMsByVersion()
        {
            IQueryable<TermsofUsePM> termsofUses = from a in repository.context.TermsofUses

                                                   select new TermsofUsePM()
                                                   {
                                                       Date = a.Date,
                                                       Version = a.Version,
                                                   };
            return termsofUses;
        }

        public TermsofUsePM GetTermsofUseDeflut()
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses.OrderByDescending(d=>d.Version)
                                        select new TermsofUsePM()
                                        {
                                            Date = a.Date,
                                            Version = a.Version,
                                        }).FirstOrDefault();



         

            return termsofUses;
        }
        

    }
}
