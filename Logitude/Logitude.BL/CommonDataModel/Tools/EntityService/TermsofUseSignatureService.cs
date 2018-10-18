using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TermsofUseSignatureService
    {
        bool isNewEntity;
        private int tenant;
        public TermsofUseSignature Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TermsofUseSignaturePM entityPm;
        private ICommonDataContext objectContext;
        private TermsofUseSignatureRepository entityRepository;
        private TermsofUseSignatureQuery entityQuery;
        public TermsofUseSignatureService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TermsofUseSignatureRepository(objectContext);
            this.entityQuery = new TermsofUseSignatureQuery(tenant);
        }

        public void Create(TermsofUseSignaturePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
              
            TermsofUseSignatureQuery termsofUseSignatureQuery = new TermsofUseSignatureQuery(entityRepository);

            bool exist = (from a in termsofUseSignatureQuery.GetTermsofUseSignaturesByTenant(entityPM.Tenant, entityPM.ContactId)
                          where a.Tenant == entityPM.Tenant && a.Id == entityPM.Id
                          select a).Any();
            if (!exist)
            {
                this.Poco = new TermsofUseSignature();
                entityPM.Id = IdCounter.GetNumber("TermsofUseSignature", entityPM.Tenant).ToString();
                this.Poco.Id = this.entityPm.Id;
                TermsofUseSignatureMapping.MapEntity(entityPM, Poco, isNewEntity);
                TermsofUseSignatureValidating.Validate(entityPM);
                TermsofUseSignatureTracing.Trace(entityPM, Poco, isNewEntity);
            
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();

            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "TermsofUseSignature");
                throw new Exception(msg);
            }
        }

        public void Update(TermsofUseSignaturePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTermsofUseSignature(entityPM.Id , entityPm.Tenant);

            bool exist = (from a in entityQuery.GetTermsofUseSignaturesByTenant(entityPM.Tenant, entityPM.ContactId)
                          where
                          a.Id == entityPM.Id
                          && a.Tenant == entityPM.Tenant
                          select a).Any();

            if (!exist)
            {

                TermsofUseSignatureValidating.Validate(entityPM);
                TermsofUseSignatureTracing.Trace(entityPM, Poco, isNewEntity);
                TermsofUseSignatureMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
               
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "TermsofUseSignature");
                throw new Exception(msg);

            }
        }
    }
}
