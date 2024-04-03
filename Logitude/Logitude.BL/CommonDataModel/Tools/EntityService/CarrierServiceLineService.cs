using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CarrierServiceLineService
    {
        private bool isNewEntity;
        private int tenant;
        private CarrierServiceLine entityPoco;
        private ICommonDataContext commonDataContext;
        private CarrierServiceLinePM entityPm;
        private ICommonDataContext objectContext;
        private CarrierServiceLineRepository entityRepository;

        public CarrierServiceLineService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.commonDataContext = objectContext;
            this.entityRepository = new CarrierServiceLineRepository(objectContext);
        }

        public void Create(CarrierServiceLinePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CarrierServiceLine", tenant).ToString();
            this.entityPoco = new CarrierServiceLine();
            this.entityPoco.Id = this.entityPm.Id;

            CarrierServiceLineMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(CarrierServiceLinePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.entityPoco = entityRepository.GetSingleCarrierServiceLine(entityPM.Id, entityPm.Tenant);

            CarrierServiceLineMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }
    }
}
