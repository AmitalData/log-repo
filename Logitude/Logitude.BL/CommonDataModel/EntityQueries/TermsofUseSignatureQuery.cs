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
    public class TermsofUseSignatureQuery
    {
        TermsofUseSignatureRepository repository;

        public TermsofUseSignatureQuery()
        {
            repository = new TermsofUseSignatureRepository(); 
        }

        public TermsofUseSignatureQuery(int tenant)
        {
            repository = new TermsofUseSignatureRepository(tenant);
        }

        public TermsofUseSignatureQuery(TermsofUseSignatureRepository repository)
        {
            this.repository = repository;
        }

        public TermsofUseSignaturePM GetSinglePM(string id, int tenant)
        {
            TermsofUseSignaturePM termsofUseSignatures = (from a in repository.context.TermsofUseSignatures
                                                          where a.Id == id
                                                          && a.Tenant == tenant
                                                          select new TermsofUseSignaturePM()
                                                          {
                                                              Id = a.Id,
                                                              SignedDatetime = a.SignedDatetime,
                                                              Tenant = a.Tenant,
                                                              ContactId = a.ContactId,
                                                              TermsofUseVersion = a.TermsofUseVersion,
                                                          }).FirstOrDefault();
            return termsofUseSignatures;
        }

        public IQueryable<TermsofUseSignaturePM> GetTermsofUseSignaturesByTenant(int tenant, string contactId)
        {
            IQueryable<TermsofUseSignaturePM> termsofUseSignatures = (from a in repository.context.TermsofUseSignatures
                                                                      where a.Tenant == tenant && a.ContactId == contactId
                                                                      select new TermsofUseSignaturePM()
                                                                      {
                                                                          Id = a.Id,
                                                                          SignedDatetime = a.SignedDatetime,
                                                                          Tenant = a.Tenant,
                                                                          ContactId = a.ContactId,
                                                                          TermsofUseVersion = a.TermsofUseVersion,
                                                                      });
            return termsofUseSignatures;
        }

        public IQueryable<TermsofUseSignaturePM> GetTermsofUseSignatureByTenant(string id, int tenant)
        {
            IQueryable<TermsofUseSignaturePM> result = (from a in repository.context.TermsofUseSignatures
                                                        where a.Tenant == tenant && a.Id == id
                                                        select new TermsofUseSignaturePM()
                                                        {
                                                            Id = a.Id,
                                                            SignedDatetime = a.SignedDatetime,
                                                            Tenant = a.Tenant,
                                                            ContactId = a.ContactId,
                                                            TermsofUseVersion = a.TermsofUseVersion,
                                                        }
                                           );
            return result;
        }

        public IQueryable<TermsofUseSignatureList> GetIQueryableEntityList(IQueryable<TermsofUseSignature> iQueryable)
        {
            IQueryable<TermsofUseSignatureList> result = from entity in iQueryable
                                                         select new TermsofUseSignatureList()
                                                         {
                                                             Id = entity.Id,
                                                             Tenant = entity.Tenant,
                                                             SignedDatetime = entity.SignedDatetime,
                                                             ContactId = entity.ContactId,
                                                             TermsofUseVersion = entity.TermsofUseVersion,
                                                         };
            return result;
        }

        public TermsofUseSignaturePM GetTermsofUseSignatureByContactIdAndVersion(int version, string userId ,int  tenant)
        {
            TermsofUseSignaturePM termsofUseSignatures = (from a in repository.context.TermsofUseSignatures
                                                          where a.ContactId == userId && a.Tenant == tenant
                                                          && a.TermsofUseVersion == version
                                                          select new TermsofUseSignaturePM()
                                                          {
                                                              Id = a.Id,
                                                              SignedDatetime = a.SignedDatetime,
                                                              Tenant = a.Tenant,
                                                              ContactId = a.ContactId,
                                                              TermsofUseVersion = a.TermsofUseVersion,
                                                          }).FirstOrDefault();

            return termsofUseSignatures;
        }
    }
}
