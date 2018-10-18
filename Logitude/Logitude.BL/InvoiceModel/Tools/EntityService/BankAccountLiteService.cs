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
using Simplog.Data.Helpers;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class BankAccountLiteService
    {
        bool isNewEntity;
        private int tenant;
        public BankAccountLite Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private BankAccountLitePM entityPM;
        private IInvoiceContext objectContext;
        private BankAccountLiteRepository entityRepository;
        public BankAccountLiteService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new BankAccountLiteRepository(objectContext);
        }

        public void Create(BankAccountLitePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("BankAccountLite", tenant).ToString();
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.Poco = new BankAccountLite();
            this.Poco.Id = theEntityPm.Id;
            BankAccountLiteTracing.Trace(theEntityPm, Poco, isNewEntity);
            BankAccountLiteMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(BankAccountLitePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.Poco = entityRepository.GetSingleBankAccountLite(theEntityPm.Id, theEntityPm.Tenant);

            BankAccountLiteTracing.Trace(theEntityPm, Poco, isNewEntity);
            BankAccountLiteMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
