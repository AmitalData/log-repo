using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ConfirmationNumberDefaultService
    {
        bool isNewEntity;
        private int tenant;
        public ConfirmationNumberDefault Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ConfirmationNumberDefaultPM entityPM;
        private IInvoiceContext objectContext;
        private ConfirmationNumberDefaultRepository entityRepository;
        public ConfirmationNumberDefaultService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ConfirmationNumberDefaultRepository(objectContext);
        }

        public void Create(ConfirmationNumberDefaultPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            //this.entityPM.Id = IdCounter.GetNumber("SATInterfaceSetting", tenant).ToString();
            this.Poco = new ConfirmationNumberDefault();
            //this.Poco.Id = this.entityPM.Id;

            ConfirmationNumberDefaultMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ConfirmationNumberDefaultPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleConfirmationNumberDefault(entityPM.Id,entityPM.Tenant);
            ConfirmationNumberDefaultMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }
    }
}
