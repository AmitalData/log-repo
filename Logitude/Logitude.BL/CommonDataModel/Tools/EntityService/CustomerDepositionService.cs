

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerDepositionService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerDeposition Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerDepositionPM entityPm;
        private ICommonDataContext objectContext;
        private CustomerDepositionRepository entityRepository;

  
        public CustomerDepositionService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerDepositionRepository(objectContext);
       


        }



        public void Create(CustomerDepositionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
     
            this.Poco = new CustomerDeposition();
            this.entityPm.Id = IdCounter.GetNumber("CustomerDeposition", tenant).ToString();
            CustomerDepositionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(CustomerDepositionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
       
            this.Poco = entityRepository.GetSingleCustomerDeposition(entityPM.Id, entityPm.Tenant);
            CustomerDepositionMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

       

    }
}
