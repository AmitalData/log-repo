
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
    public class CustomsShipperService
    {
        bool isNewEntity;
        private int tenant;
        public CustomsShipper Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomsShipperPM entityPm;
        private ICommonDataContext objectContext;
        private CustomsShipperRepository entityRepository;


        public CustomsShipperService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomsShipperRepository(objectContext);
 


        }



        public void Create(CustomsShipperPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            this.Poco = new CustomsShipper();
            this.entityPm.Id = IdCounter.GetNumber("CustomsShipper", tenant).ToString();
            CustomsShipperMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(CustomsShipperPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;

            this.Poco = entityRepository.GetSingleCustomsShipper(entityPM.Id, entityPm.Tenant);
            CustomsShipperMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }



    }
}
