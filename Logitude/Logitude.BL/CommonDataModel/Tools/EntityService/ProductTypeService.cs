using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ProductTypeService
    {
        bool isNewEntity;
        private int tenant;
        public ProductType Poco { get; set; }
        private ProductTypePM entityPm;
        private ICommonDataContext objectContext;
        private ProductTypeRepository entityRepository;
        private ProductTypeModificationRepository modificationRepository;
        public ProductTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ProductTypeRepository(objectContext);
            this.modificationRepository = new ProductTypeModificationRepository(objectContext);
        }
        public void Create(ProductTypePM entityPM)
        {

        }
        public void Update(ProductTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleProductType(entityPM.Code);

            ProductTypeModification modification = modificationRepository.GetSingleProductTypeModification(entityPm.Code, entityPm.Tenant);

            if (modification == null)
            {
                modification = new ProductTypeModification() { ProductTypeCode = entityPm.Code, Tenant = entityPm.Tenant, InActive = entityPm.InActive };

                modificationRepository.Add(modification);
                modificationRepository.SubmitChanges();
            }

            ProductTypeTracing.Trace(entityPM, Poco, isNewEntity, modification);
            ProductTypeMapping.MapEntity(entityPM, Poco, isNewEntity, modification);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}