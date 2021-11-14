using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
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
    public class PortTimeZoneService
    {
        bool isNewEntity;
        private int tenant;
        public PortTimeZone Poco { get; set; }
        private PortTimeZonePM entityPM;
        private ICommonDataContext objectContext;
        private PortTimeZoneRepository entityRepository;
        public PortTimeZoneService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new PortTimeZoneRepository(objectContext);
        }

        public void Create(PortTimeZonePM entityPM)
        {
                        
        }

        public void Update(PortTimeZonePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSinglePortTimeZone(entityPM.Code);

            PortTimeZoneTracing.Trace(entityPM, Poco, isNewEntity);
            PortTimeZoneMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
