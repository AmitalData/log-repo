using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class IATACodeService
    {
        private int tenant;
        private bool isNewEntity;
        private IWebFreightContext objectContext;
        private IATACodePM entityPM;
        private IATACode entityPOCO;
        private IATACodeRepository entityRepository;  
        public IATACodeService(IWebFreightContext objectContext, IATACodePM entityPM)
        {
            this.tenant = 0;
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.entityRepository = new IATACodeRepository(objectContext);
        }

        public void Create()
        {
            this.isNewEntity = true;
            this.entityPM.Id = IdCounter.GetNumber("IATACode", tenant).ToString();
            this.entityPOCO = new IATACode();
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

            IATACodeMapping.MapEntity(entityPM, entityPOCO, isNewEntity);
            entityRepository.Add(entityPOCO);
            entityRepository.SubmitChanges();
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.entityPOCO = entityRepository.GetSingleIATACode(entityPM.Id);

            IATACodeMapping.MapEntity(entityPM, entityPOCO, isNewEntity);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();
        }
    }
}
