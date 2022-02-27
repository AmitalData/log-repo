using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.Helpers;
using System;
using System.Linq;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class QuoteChargesGroupService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteChargesGroup entityPoco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteChargesGroupPM entityPM;
        private IWebFreightContext objectContext;
        private QuoteChargesGroupRepository entityRepository;
        public QuoteChargesGroupService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteChargesGroupRepository(objectContext);
        }

        public void Create(QuoteChargesGroupPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;

            bool exist = this.IsQuoteChargesGroupExists();
            if (exist)
            {
                ThrowQuoteChargesGroupExistsExcption();
            }

            this.entityPM.Id = IdCounter.GetNumber("QuoteChargesGroup", tenant).ToString();
            this.entityPoco = new QuoteChargesGroup();
            this.entityPoco.Id = this.entityPM.Id;

            QuoteChargesGroupMapping.MapEntity(theEntityPm, entityPoco, isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteChargesGroupPM quoteChargesGroupPM)
        {
            this.isNewEntity = false;
            this.entityPM = quoteChargesGroupPM;
            bool exist = this.IsQuoteChargesGroupExists();
            if (exist)
            {
                ThrowQuoteChargesGroupExistsExcption();
            }

            this.entityPoco = entityRepository.GetSingleQuoteChargesGroup(quoteChargesGroupPM.Id, quoteChargesGroupPM.Tenant);
            QuoteChargesGroupMapping.MapEntity(quoteChargesGroupPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();

        }

        private bool IsQuoteChargesGroupExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetQuoteChargesGroups(entityPM.Tenant)
                            where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetQuoteChargesGroups(entityPM.Tenant)
                            where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant && a.Id != entityPM.Id
                            select a).Any();
            }

            return myResult;
        }
        private void ThrowQuoteChargesGroupExistsExcption()
        {
            string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
            msg = msg.Replace("%Entity", "Quote Charges Group");
            throw new ApplicationException(msg);
        }
    }
}