using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class FreelancerGroupTypeService
    {
        bool isNewEntity;
        private int tenant;
        public FreelancerGroupType Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private FreelancerGroupTypePM entityPm;
        private ICommonDataContext objectContext;
        private FreelancerGroupTypeRepository entityRepository;

        public FreelancerGroupTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new FreelancerGroupTypeRepository(objectContext);
        }


        public void Create(FreelancerGroupTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("FreelancerGroupType", tenant).ToString();
            this.Poco = new FreelancerGroupType();
            this.Poco.Id = this.entityPm.Id;

            FreelancerGroupTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(FreelancerGroupTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingle(entityPM.Id, entityPm.Tenant);

            FreelancerGroupTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
