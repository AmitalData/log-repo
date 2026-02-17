using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
                                            IsNew = a.IsNew
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
                                            IsNew = a.IsNew
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
                                            IsNew = a.IsNew
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
                                            IsNew = a.IsNew
                                        }).FirstOrDefault();
            return termsofUses;
        }

        public string GetLatestTermsOfUseDocumentId(string privateLabeldId)
        {
            var documentId = (from a in repository.context.TermsofUses.OrderByDescending(d => d.VersionNumber)
                              where a.PrivateLabelId == privateLabeldId
                              select a.VersionDocumentId).FirstOrDefault();
            return documentId;
        }


        public bool CheckIfDocumentExist(int tenant , string documentId)
        {
            return (from a in repository.context.TermsofUses
                              where a.VersionDocumentId == documentId && (a.Tenant == 0 || a.Tenant == tenant)
                              select a).Any();
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
                                                       IsNew = a.IsNew
                                                   };
            return termsofUses;
        }

        public TermsofUsePM GetTermsofUseDefault(bool useNewTermsOfUse)
        { 
            TermsofUsePM termsofUses = (from a in repository.context.TermsofUses.OrderByDescending(d=>d.VersionNumber)
                                        where a.Tenant == 0 && a.PrivateLabelId == null && a.IsNew == useNewTermsOfUse
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            Tenant = a.Tenant,
                                            VersionDocumentId = a.VersionDocumentId,
                                            PrivateLabelId = a.PrivateLabelId,
                                            IsNew = a.IsNew
                                        }).FirstOrDefault();



         

            return termsofUses;
        } 
   

        public TermsofUsePM GetTermOfUseByPrivateLabel(string privateLabelId)
        {
            return (from a in repository.context.TermsofUses.OrderByDescending(d => d.VersionNumber)
                                        where a.PrivateLabelId == privateLabelId
                                        select new TermsofUsePM()
                                        {
                                            Id = a.Id,
                                            Date = a.Date,
                                            VersionNumber = a.VersionNumber,
                                            VersionDocumentId = a.VersionDocumentId,
                                            Tenant = a.Tenant,
                                            PrivateLabelId = a.PrivateLabelId,
                                            IsNew = a.IsNew
                                        }).FirstOrDefault(); 
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
                                            IsNew = a.IsNew
                                        }).FirstOrDefault();
            return termsofUses;
        }




        public IQueryable<TermsofUsePM> GetByPrivateLabeldId(string privatelabeldId)
        {
            IQueryable<TermsofUsePM> termsofUse = (from a in repository.context.TermsofUses
                                                   where a.PrivateLabelId == privatelabeldId
                                                   select new TermsofUsePM()
                                                   {
                                                       Id = a.Id,
                                                       Date = a.Date,
                                                       VersionNumber = a.VersionNumber,
                                                       VersionDocumentId = a.VersionDocumentId,
                                                       Tenant = a.Tenant,
                                                       PrivateLabelId = a.PrivateLabelId,
                                                       IsNew = a.IsNew
                                                   });
            return termsofUse;
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
                                                                          IsNew = a.IsNew
                                                                      });
            return termsofUse;
        }

    }
}
