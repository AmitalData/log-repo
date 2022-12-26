using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class AWBAdditionalHandlingInfoService
    {
        private int tenant;
        private bool isNewEntity;
        private IShipmentsContext objectContext;
        private AWBAdditionalHandlingInfoPM entityPM;
        private AWBAdditionalHandlingInfo entityPoco;
        private AWBAdditionalHandlingInfoRepository entityRepository;       

        public AWBAdditionalHandlingInfoService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AWBAdditionalHandlingInfoRepository(objectContext);
        }

        public void Create(AWBAdditionalHandlingInfoPM entityPM)
        {
            
        }

        public void Update(AWBAdditionalHandlingInfoPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPoco = entityRepository.GetSingleAWBAdditionalHandlingInfo(entityPM.Id, tenant);

            this.MapEntityFields();

            this.entityRepository.Update(entityPoco);
            this.entityRepository.SubmitChanges();
        }

        private void MapEntityFields()
        {
            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;                
            }

            entityPoco.Code = entityPM.Code;
            entityPoco.Name = entityPM.Name;
            entityPoco.PrintDescription = entityPM.PrintDescription;

            this.BuildSearchField();
        }

        private void BuildSearchField()
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PrintDescription);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
