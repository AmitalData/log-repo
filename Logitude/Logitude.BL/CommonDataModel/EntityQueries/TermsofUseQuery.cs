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
                                        where a.VersionNumber == version
                                        && a.Date == toUdate
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            Tenant = a.Tenant,
                                            VersionDocumentId = a.VersionDocumentId,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();
            return termsofUses;
        }
         

        public TermsofUsePM GetSingleById(int id)
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses
                                        where a.Id == id 
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            Tenant = a.Tenant,
                                            VersionDocumentId = a.VersionDocumentId,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();
            return termsofUses;
        }
  
        public TermsofUsePM GetSinglePMByVersion(int version)
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses
                                        where a.VersionNumber == version

                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            Tenant = a.Tenant,
                                            VersionDocumentId = a.VersionDocumentId,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();
            return termsofUses;
        }

        public TermsofUsePM GetSinglePMById(int id)
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses
                                        where a.Id == id

                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            Tenant = a.Tenant,
                                            VersionDocumentId = a.VersionDocumentId,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();
            return termsofUses;
        }

        public IQueryable<TermsofUsePM> GetTermsofUsePMsByVersion()
        {
            IQueryable<TermsofUsePM> termsofUses = from a in repository.context.TermsofUses

                                                   select new TermsofUsePM()
                                                   {
                                                       Id = a.Id,
                                                       Date = a.Date,
                                                       VersionNumber = a.VersionNumber,
                                                       Tenant = a.Tenant,
                                                       VersionDocumentId = a.VersionDocumentId,
                                                       PrivateLabelId = a.PrivateLabelId,
                                                   };
            return termsofUses;
        }

        public TermsofUsePM GetTermsofUseDefault()
        {
            //Default not is from Tenant 0
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses.OrderByDescending(d=>d.VersionNumber)
                                        where a.Tenant == 0 && a.PrivateLabelId == null
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            Tenant = a.Tenant,
                                            VersionDocumentId = a.VersionDocumentId,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();



         

            return termsofUses;
        }


        public TermsofUsePM GetTermsOfUse(TenantPM tenantPM)
        {
            TermsofUsePM termsofUses;

            if (string.IsNullOrEmpty(tenantPM.PrivateLabelId))
            {
                termsofUses = GetTermsofUseDefault();

            }
            else
            {
                termsofUses = GetTermOfUseByPrivateLabel(tenantPM.PrivateLabelId);
            }
            return termsofUses;


        }

        public TermsofUsePM GetTermOfUseByPrivateLabel(string privateLabelId)
        {
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses.OrderByDescending(d => d.VersionNumber)
                                        where a.PrivateLabelId == privateLabelId
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            VersionDocumentId = a.VersionDocumentId,
                                            Tenant = a.Tenant,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();
            return termsofUses;
        }



        public TermsofUsePM GetTermOfUseByTenant(int tenant)
        { 
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses.OrderByDescending(d => d.VersionNumber)
                                        where a.Tenant == tenant
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            VersionDocumentId = a.VersionDocumentId,
                                            Tenant = a.Tenant,
                                            PrivateLabelId = a.PrivateLabelId,
                                        }).FirstOrDefault();
            return termsofUses;
        }


         

        public IQueryable<TermsofUsePM> GetTermsofUseByTenant(int tenant)
        {
            IQueryable<TermsofUsePM> termsofUse = (from a in repository.context.TermsofUses
                                                                      where a.Tenant == tenant
                                                                      select new TermsofUsePM()
                                                                      { 
                                                                          Id = a.Id,
                                                                          Date = a.Date,
                                                                          VersionNumber = a.VersionNumber,
                                                                          VersionDocumentId = a.VersionDocumentId,
                                                                          Tenant = a.Tenant,
                                                                          PrivateLabelId = a.PrivateLabelId,
                                                                      });
            return termsofUse;
        }

    }
}
