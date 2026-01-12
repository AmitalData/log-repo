using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
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
    public class MasavInterfaceService
    {
        bool isNewEntity;
        private int tenant;
        public MasavInterface Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MasavInterfacePM entityPM;
        private IInvoiceContext objectContext;
        private MasavInterfaceRepository entityRepository;
        public MasavInterfaceService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MasavInterfaceRepository(objectContext);
        }

        public void Create(MasavInterfacePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            //this.entityPM.Id = IdCounter.GetNumber("MasavInterface", tenant).ToString();
            this.Poco = new MasavInterface();
            //this.Poco.Id = this.entityPM.Id;

            MasavInterfaceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(MasavInterfacePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMasavInterface(theEntityPm.Id,entityPM.Tenant);
            MasavInterfaceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }
    }
}
