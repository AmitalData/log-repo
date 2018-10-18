using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateTermsofUseSignatureList(TermsofUseSignatureList currentEntity)
        {
        }

        public IQueryable<TermsofUseSignaturePM> GetTermsofUseSignatures(int tenant, string contactId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            termsofUseSignatureQuery = new TermsofUseSignatureQuery(tenant);
            return termsofUseSignatureQuery.GetTermsofUseSignaturesByTenant(tenant, contactId);
        }

        public IQueryable<TermsofUseSignatureList> GetTermsofUseSignatureLists(int tenant, string contactId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            termsofUseSignatureRepository = new TermsofUseSignatureRepository(tenant);
            termsofUseSignatureQuery = new TermsofUseSignatureQuery(termsofUseSignatureRepository);

            IQueryable<TermsofUseSignature> iQueryable = termsofUseSignatureRepository.GetTermsofUseSignatures();
            IQueryable<TermsofUseSignatureList> query2 = termsofUseSignatureQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        public TermsofUseSignaturePM GetSingleTermsofUseSignature(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            termsofUseSignatureQuery = new TermsofUseSignatureQuery(tenant);
            return termsofUseSignatureQuery.GetSinglePM(id, tenant);
        }

        public TermsofUseSignatureList GetSingleTermsofUseSignatureList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            termsofUseSignatureRepository = new TermsofUseSignatureRepository(tenant);
            TermsofUseSignatureList entityList = null;
            TermsofUseSignature entity = termsofUseSignatureRepository.GetSingleTermsofUseSignature(id, tenant);

            if (entity != null)
            {
                List<TermsofUseSignature> singleEntityList = new List<TermsofUseSignature>();
                singleEntityList.Add(entity);

                IQueryable<TermsofUseSignature> iQueryable = singleEntityList.AsQueryable();
                termsofUseSignatureQuery = new TermsofUseSignatureQuery(termsofUseSignatureRepository);
                IQueryable<TermsofUseSignatureList> iQueryableEntityList = termsofUseSignatureQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }
            return entityList;
        }

        public TermsofUseSignaturePM GetTermsofUseSignatureById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            termsofUseSignatureQuery = new TermsofUseSignatureQuery(tenant);
            TermsofUseSignaturePM termsofUseSignature = termsofUseSignatureQuery.GetSinglePM(id, tenant);
            return termsofUseSignature;
        }

        public void InsertTermsofUseSignature(TermsofUseSignaturePM termsofUseSignature)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(termsofUseSignature.Tenant);
            }
            TermsofUseSignatureService service = new TermsofUseSignatureService(objectContext, termsofUseSignature.Tenant);
            service.Create(termsofUseSignature);


            //termsofUseSignatureRepository = new TermsofUseSignatureRepository(objectContext);
            //termsofUseSignatureQuery = new TermsofUseSignatureQuery(termsofUseSignatureRepository);

            //bool exist = (from a in termsofUseSignatureQuery.GetTermsofUseSignaturesByTenant(termsofUseSignature.Tenant, termsofUseSignature.ContactId)
            //              where a.Tenant == termsofUseSignature.Tenant && a.Id == termsofUseSignature.Id
            //              select a).Any();
            //if (!exist)
            //{
            //    TermsofUseSignature newTermsofUseSignature = new TermsofUseSignature();

            //    termsofUseSignature.Id = IdCounter.GetNumber("TermsofUseSignature", termsofUseSignature.Tenant).ToString();
            //    newTermsofUseSignature.Id = termsofUseSignature.Id;

            //    MapTermsofUseSignatureTermsofUseSignaturePM(termsofUseSignature, newTermsofUseSignature);

            //    termsofUseSignatureRepository.Add(newTermsofUseSignature);

            //}

            //else
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", termsofUseSignature.Tenant);
            //    msg = msg.Replace("%Entity", "TermsofUseSignature");
            //    throw new Exception(msg);
            //}
        }

        //public void MapTermsofUseSignatureTermsofUseSignaturePM(TermsofUseSignaturePM termsofUseSignaturePm, TermsofUseSignature termsofUseSignature)
        //{
        //    termsofUseSignature.Tenant = termsofUseSignaturePm.Tenant;
        //    termsofUseSignature.SignedDatetime = termsofUseSignaturePm.SignedDatetime;
        //    termsofUseSignature.ContactId = termsofUseSignaturePm.ContactId;
        //    termsofUseSignature.TermsofUseVersion = termsofUseSignaturePm.TermsofUseVersion;
        //}

        public void UpdateTermsofUseSignature(TermsofUseSignaturePM currentTermsofUseSignature)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentTermsofUseSignature.Tenant);
            }
            TermsofUseSignatureService service = new TermsofUseSignatureService(objectContext, currentTermsofUseSignature.Tenant);
            service.Update(currentTermsofUseSignature);
            TableLastUpdateClass.UpdateTableHistory(currentTermsofUseSignature.Tenant, "TermsofUseSignature");

            //termsofUseSignatureRepository = new TermsofUseSignatureRepository(objectContext);
            //termsofUseSignatureQuery = new TermsofUseSignatureQuery(termsofUseSignatureRepository);

            //bool exist = (from a in termsofUseSignatureQuery.GetTermsofUseSignaturesByTenant(currentTermsofUseSignature.Tenant, currentTermsofUseSignature.ContactId)
            //              where
            //              a.Id == currentTermsofUseSignature.Id
            //              && a.Tenant == currentTermsofUseSignature.Tenant
            //              select a).Any();

            //if (!exist)
            //{
            //    TermsofUseSignature entity = termsofUseSignatureRepository.GetSingleTermsofUseSignature(currentTermsofUseSignature.Id, currentTermsofUseSignature.Tenant);
            //    MapTermsofUseSignatureTermsofUseSignaturePM(currentTermsofUseSignature, entity);
            //    termsofUseSignatureRepository.Update(entity);
            //    TableLastUpdateClass.UpdateTableHistory(currentTermsofUseSignature.Tenant, "TermsofUseSignature");
            //}
            //else
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentTermsofUseSignature.Tenant);
            //    msg = msg.Replace("%Entity", "TermsofUseSignature");
            //    throw new Exception(msg);

            //}
        }

        public void DeleteTermsofUseSignature(TermsofUseSignaturePM termsofUseSignature)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(termsofUseSignature.Tenant);
            }
            termsofUseSignatureRepository = new TermsofUseSignatureRepository(objectContext);
            TermsofUseSignature entity = termsofUseSignatureRepository.GetSingleTermsofUseSignature(termsofUseSignature.Id, termsofUseSignature.Tenant);
            termsofUseSignatureRepository.Remove(entity);
        }
    }
}