using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class AWBSpecialHandlingCodeService
    {
        private int tenant;
        private bool isNewEntity;
        private IShipmentsContext objectContext;
        private AWBSpecialHandlingCodePM entityPM;
        private AWBSpecialHandlingCode entityPOCO;
        private AWBSpecialHandlingCodeRepository entityRepository;        
        public AWBSpecialHandlingCodeService(IShipmentsContext objectContext, AWBSpecialHandlingCodePM entityPM)
        {
            this.tenant = 0;
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.entityRepository = new AWBSpecialHandlingCodeRepository(objectContext);
        }

        public void Create()
        {
            this.isNewEntity = true;
            this.entityPM.Id = IdCounter.GetNumber("AWBSpecialHandlingCode", tenant).ToString();
            this.entityPOCO = new AWBSpecialHandlingCode();
            this.entityPOCO.Id = this.entityPM.Id;

            if (!string.IsNullOrEmpty(entityPM.AirlineId))
            {
                AirlineRepository rep = new AirlineRepository(tenant);
                Airline airline = rep.GetSingleAirline(entityPM.AirlineId, tenant);
                if (airline != null)
                {
                    airline.HasAdaptations = true;

                    rep.Update(airline);
                    rep.SubmitChanges();
                }
            }

            AWBSpecialHandlingCodeMapping.MapEntity(entityPM, entityPOCO, isNewEntity);
            entityRepository.Add(entityPOCO);
            entityRepository.SubmitChanges();
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.entityPOCO = entityRepository.GetSingleAWBHandlingCode(entityPM.Id);

            AWBSpecialHandlingCodeMapping.MapEntity(entityPM, entityPOCO, isNewEntity);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();
        }
    }
}
