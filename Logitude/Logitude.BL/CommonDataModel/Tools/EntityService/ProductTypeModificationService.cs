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
    public class ProductTypeModificationService
    {
        bool isNewEntity;
        private int tenant;
        public ProductTypeModification Poco { get; set; }
        private ProductTypeModificationPM entityPm;
        private ICommonDataContext objectContext;
        private ProductTypeModificationRepository modificationRepository;
        public ProductTypeModificationService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.modificationRepository = new ProductTypeModificationRepository(objectContext);
        }
        public void Create(ProductTypeModificationPM entityPM)
        {

        }
        public void Update(ProductTypeModificationPM entityPM)
        {

            ProductTypeModification modification =modificationRepository.GetSingle(entityPM.ProductTypeCode);
            if (modification != null)
            {
                modification.CostTariffUse = entityPM.CostTariffUse;
                modification.SaleTariffUse = entityPM.SaleTariffUse;
                //modificationRepository.Add(modification);
                modificationRepository.Update(modification);
                modificationRepository.SubmitChanges();
            }
            else
            {

            }

            /*ProductTypeTracing.Trace(entityPM, Poco, isNewEntity, modification);
            ProductTypeMapping.MapEntity(entityPM, Poco, isNewEntity, modification);
            */

        }
    }
}