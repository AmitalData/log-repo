using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.DataMapping;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class CreditCardTypeService
    {
        bool isNewEntity;
        private int tenant;
        public CreditCardType Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CreditCardTypePM entityPM;
        private IInvoiceContext objectContext;
        private CreditCardTypeRepository entityRepository;
        public CreditCardTypeService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CreditCardTypeRepository(objectContext);
        }

        public void Create(CreditCardTypePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("CreditCardType", tenant).ToString();

            this.Poco = new CreditCardType();

            this.Poco.Id = theEntityPm.Id;

            CreditCardTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            CreditCardTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();           
        }

        public void Update(CreditCardTypePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;

            this.Poco = entityRepository.GetSingleCreditCardType(theEntityPm.Id, theEntityPm.Tenant);

            CreditCardTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
            CreditCardTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}