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
    public class AirlineStatisticsService
    {
        bool isNewEntity;
        private int tenant;
        public AirlineStatistics Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private AirlineStatisticsPM entityPM;
        private ICommonDataContext objectContext;
        private AirlineStatisticsRepository entityRepository;

        public AirlineStatisticsService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AirlineStatisticsRepository(objectContext);
        }

        public void Create(AirlineStatisticsPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("AirlineStatistics", tenant).ToString();
            this.Poco = new AirlineStatistics();
            this.Poco.Id = this.entityPM.Id;

            AirlineStatisticsMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(AirlineStatisticsPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleAirlineStatistics(theEntityPm.Tenant, theEntityPm.Id);

            AirlineStatisticsMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
