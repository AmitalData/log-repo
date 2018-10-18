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
    public class SATInterfaceSettingService
    {
        bool isNewEntity;
        private int tenant;
        public SATInterfaceSetting Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private SATInterfaceSettingPM entityPM;
        private IInvoiceContext objectContext;
        private SATInterfaceSettingRepository entityRepository;
        public SATInterfaceSettingService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new SATInterfaceSettingRepository(objectContext);
        }

        public void Create(SATInterfaceSettingPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            //this.entityPM.Id = IdCounter.GetNumber("SATInterfaceSetting", tenant).ToString();
            this.Poco = new SATInterfaceSetting();
            //this.Poco.Id = this.entityPM.Id;

            SATInterfaceSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(SATInterfaceSettingPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            SATInterfaceSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }
    }
}
