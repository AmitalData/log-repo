using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CommodityService
    {
        bool isNewEntity;
        private int tenant;
        private CommodityPM entityPM;
        private ICommonDataContext objectContext;
        private CommodityRepository entityRepository;
        public Commodity Poco { get; set; }
        public CommodityService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CommodityRepository(objectContext);
        }

        public void Create(CommodityPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("Commodity", tenant).ToString();
            this.Poco = new Commodity();
            this.Poco.Id = this.entityPM.Id;

            if (tenant == 0)
            {
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
            }

            CommodityTracing.Trace(entityPM, Poco, isNewEntity);
            CommodityMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CommodityPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCommodity(entityPM.Id, tenant);

            CommodityTracing.Trace(entityPM, Poco, isNewEntity);
            CommodityMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
