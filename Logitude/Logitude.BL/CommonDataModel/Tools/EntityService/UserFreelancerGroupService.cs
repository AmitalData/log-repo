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
    public class UserFreelancerGroupService
    {
        bool isNewEntity;
        private int tenant;
        public UserFreelancerGroup Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private UserFreelancerGroupPM entityPm;
        private ICommonDataContext objectContext;
        private UserFreelancerGroupRepository entityRepository;

        public UserFreelancerGroupService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new UserFreelancerGroupRepository(objectContext);
        }


        public void Create(UserFreelancerGroupPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("UserFreelancerGroup", tenant).ToString();
            this.Poco = new UserFreelancerGroup();
            this.Poco.Id = this.entityPm.Id;

            UserFreelancerGroupMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }
        
    }
}
