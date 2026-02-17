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
        private string serviceContextUser;
        private IShipmentsContext objectContext;
        private AWBAdditionalHandlingInfoPM entityPM;
        private AWBAdditionalHandlingInfo entityPoco;
        private AWBAdditionalHandlingInfoRepository entityRepository;
        public AWBAdditionalHandlingInfoService(IShipmentsContext objectContext, AWBAdditionalHandlingInfoPM entityPM, string serviceContextUser)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.serviceContextUser = serviceContextUser;
            this.entityRepository = new AWBAdditionalHandlingInfoRepository(objectContext);
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.entityPoco = entityRepository.GetSingleAWBAdditionalHandlingInfo(entityPM.Id);

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
                entityPoco.Code = entityPM.Code;
                entityPoco.Name = entityPM.Name;
            }

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
