using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
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
    public class MoveTypeService
    {
        bool isNewEntity;
        private int tenant;
        public MoveType Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MoveTypePM entityPM;
        private IWebFreightContext objectContext;
        private MoveTypeRepository entityRepository;
        public MoveTypeService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MoveTypeRepository(objectContext);
        }

        public void Create(MoveTypePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "MoveTypePM");
                throw new ApplicationException(msg);
            }

            else
            {
                this.entityPM.Id = IdCounter.GetNumber("MoveType", tenant).ToString();
                this.Poco = new MoveType();
                this.Poco.Id = this.entityPM.Id;

                MoveTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
                MoveTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }
        }

        public void Update(MoveTypePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "MoveType");
                throw new ApplicationException(msg);
            }

            else
            {
                this.Poco = entityRepository.GetSingleMoveType(theEntityPm.Id, theEntityPm.Tenant);

                MoveTypeTracing.Trace(theEntityPm, Poco, isNewEntity);
                MoveTypeMapping.MapEntity(theEntityPm, Poco, isNewEntity);

                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
        }

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetMoveTypesByTenant(entityPM.Tenant)
                            where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetMoveTypesByTenant(entityPM.Tenant)
                            where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant && a.Id != entityPM.Id
                            select a).Any();
            }

            return myResult;
        }
    }
}
