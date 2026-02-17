using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class DescriptionOfGoodsService
    {

        bool isNewEntity;
        private int tenant;
        public DescriptionOfGoods Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DescriptionOfGoodsPM entityPM;
        private IWebFreightContext objectContext;
        private DescriptionOfGoodsRepository entityRepository;
        public DescriptionOfGoodsService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DescriptionOfGoodsRepository(objectContext);
        }

        public void Create(DescriptionOfGoodsPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DescriptionOfGoods", tenant).ToString();
            this.Poco = new DescriptionOfGoods();
            this.Poco.Id = this.entityPM.Id;

            DescriptionOfGoodsValidating.Validate(theEntityPm);
            DescriptionOfGoodsTracing.Trace(theEntityPm, Poco, isNewEntity);
            DescriptionOfGoodsMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(DescriptionOfGoodsPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDescriptionOfGoods(theEntityPm.Id);

            DescriptionOfGoodsValidating.Validate(theEntityPm);
            DescriptionOfGoodsTracing.Trace(theEntityPm, Poco, isNewEntity);
            DescriptionOfGoodsMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}