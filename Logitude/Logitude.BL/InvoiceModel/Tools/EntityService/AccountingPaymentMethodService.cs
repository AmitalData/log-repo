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
    public class AccountingPaymentMethodService
    {
        bool isNewEntity;
        private int tenant;
        public AccountingPaymentMethod Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AccountingPaymentMethodPM entityPM;
        private IInvoiceContext objectContext;
        private AccountingPaymentMethodRepository entityRepository;
        public AccountingPaymentMethodService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AccountingPaymentMethodRepository(objectContext);
        }

        public void Create(AccountingPaymentMethodPM theEntityPm)
        {
            bool exist = false;
            if (!string.IsNullOrEmpty(theEntityPm.Code))
            {
                exist = (from a in entityRepository.GetAccountingPaymentMethods(tenant)
                         where a.Code == theEntityPm.Code && a.Tenant == tenant
                         select a).Any();
            }

            if (!exist)
            {
                this.isNewEntity = true;
                this.entityPM = theEntityPm;
                this.entityPM.Id = IdCounter.GetNumber("AccountingPaymentMethod", tenant).ToString();

                this.Poco = new AccountingPaymentMethod();

                this.Poco.Id = theEntityPm.Id;

                AccountingPaymentMethodTracing.Trace(theEntityPm, Poco, isNewEntity);
                AccountingPaymentMethodMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant);
                msg = msg.Replace("%Entity", "AccountingPaymentMethod");
                throw new Exception(msg);
            }
        }

        public void Update(AccountingPaymentMethodPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;

            this.Poco = entityRepository.GetSingleAccountingPaymentMethod(theEntityPm.Id, theEntityPm.Tenant);

            AccountingPaymentMethodTracing.Trace(theEntityPm, Poco, isNewEntity);
            AccountingPaymentMethodMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
