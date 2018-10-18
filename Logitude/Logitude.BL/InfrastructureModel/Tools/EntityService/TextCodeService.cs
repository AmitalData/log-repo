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
    public class TextCodeService
    {
        bool isNewEntity;
        private int tenant;
        public TextCode Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TextCodePM entityPM;
        private IWebFreightContext objectContext;
        private TextCodeRepository entityRepository;
        public TextCodeService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TextCodeRepository(objectContext);
        }

        public void Create(TextCodePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
         
            if (theEntityPm.DefaultText == null)
            {
            }
            this.entityPM.Id = IdCounter.GetNumber("TextCode", tenant).ToString();
            this.Poco = new TextCode();
            this.Poco.Id = this.entityPM.Id;
     
            TextCodeValidating.Validate(theEntityPm);
            TextCodeTracing.Trace(theEntityPm, Poco, isNewEntity);
            TextCodeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(TextCodePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTextCode(theEntityPm.Id);

            TextCodeValidating.Validate(theEntityPm);
            TextCodeTracing.Trace(theEntityPm, Poco, isNewEntity);
            TextCodeMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}