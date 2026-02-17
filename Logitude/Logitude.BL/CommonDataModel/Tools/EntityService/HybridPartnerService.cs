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
  public  class HybridPartnerService
    {
           bool isNewEntity;
       
        public HybridPartner Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private HybridPartnerPM entityPM;
        private ICommonDataContext objectContext;
        private HybridPartnerRepository entityRepository;
        public HybridPartnerService(ICommonDataContext objectContext,int Tenant)
        { 
            this.ObjectContext = objectContext;
            this.entityRepository = new HybridPartnerRepository(objectContext);
        }
        public HybridPartnerService(ICommonDataContext objectContext)
        {
        
            this.ObjectContext = objectContext;
            this.entityRepository = new HybridPartnerRepository(objectContext);
        }

        public void Create(HybridPartnerPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("HybridPartner", 0).ToString();
            this.Poco = new HybridPartner();
            this.Poco.Id = this.entityPM.Id;
            HybridPartnerMapping.MapEntity(entityPM, Poco);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();


       

           
        }

        public void Update(HybridPartnerPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleHybridPartner(entityPM.Id);
            HybridPartnerMapping.MapEntity(entityPM, Poco);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
