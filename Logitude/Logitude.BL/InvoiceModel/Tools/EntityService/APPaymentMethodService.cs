using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class APPaymentMethodService
    {
        bool isNewEntity;
        private int tenant;
        public APPaymentMethod Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private APPaymentMethodPM entityPM;
        private IInvoiceContext objectContext;
        private APPaymentMethodRepository entityRepository;
        public APPaymentMethodService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new APPaymentMethodRepository(objectContext);
        }

        public void Create(APPaymentMethodPM theEntityPm)
        {
            bool exist = false;
            if (!string.IsNullOrEmpty(theEntityPm.Code))
            {
                exist = (from a in entityRepository.GetAPPaymentMethods(tenant)
                         where a.Code == theEntityPm.Code && a.Tenant == tenant
                         select a).Any();
            }

            if (!exist)
            {
                this.isNewEntity = true;
                this.entityPM = theEntityPm;
                this.entityPM.Id = IdCounter.GetNumber("APPaymentMethod", tenant).ToString();

                this.Poco = new APPaymentMethod();

                this.Poco.Id = theEntityPm.Id;

                APPaymentMethodTracing.Trace(theEntityPm, Poco, isNewEntity);
                APPaymentMethodMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant);
                msg = msg.Replace("%Entity", "APPaymentMethod");
                throw new Exception(msg);
            }
        }

        public void Update(APPaymentMethodPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;

            this.Poco = entityRepository.GetSingleAPPaymentMethod(theEntityPm.Id, theEntityPm.Tenant);

            APPaymentMethodTracing.Trace(theEntityPm, Poco, isNewEntity);
            APPaymentMethodMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
