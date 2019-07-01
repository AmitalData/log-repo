
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class OccasionDataMapping: IMapping<OccasionPM, Occasion>
   {

        public void CustomPMToPOCO(OccasionPM entityPM, Occasion entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(OccasionPM entityPM, Occasion entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.TypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OwnerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OccasionStatusName);

            if (!string.IsNullOrEmpty(entityPOCO.OccasionTypeId))
            {
                OccasionTypeRepository iRepository = new OccasionTypeRepository(entityPOCO.Tenant);
                OccasionTypeKeys iKeys = new OccasionTypeKeys() { Id = entityPOCO.OccasionTypeId };
                OccasionType iEntity = iRepository.GetSingle(iKeys);
                if (iEntity != null)
                {
                    entityPM.TypeName = iEntity.Name;
                }
            }


            if (!string.IsNullOrEmpty(entityPOCO.OwnerId))
            {
                UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
                User user = userRepository.GetSingleUser(entityPOCO.OwnerId, entityPOCO.Tenant, false);
                if (user != null)
                {
                    entityPM.OwnerName = user.Contact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.OccasionStatusId))
            {
                OccasionStatusRepository iRepository = new OccasionStatusRepository(entityPOCO.Tenant);
                OccasionStatusKeys iKeys = new OccasionStatusKeys() { Code = entityPOCO.OccasionStatusId };
                OccasionStatus iEntity = iRepository.GetSingle(iKeys);
                if (iEntity != null)
                {
                    entityPM.OccasionStatusName = iEntity.Name;
                }
            }
        }

        private void BuildSearchFields(OccasionPM entityPM, Occasion entityPOCO, bool p)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            }

            if (!string.IsNullOrEmpty(entityPM.OwnerId))
            {
                Contact owner = ContactRepository.GetSingleContact(entityPM.OwnerId, entityPM.Tenant, true);
                if (owner != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, owner.EnglishName);
                }
            }

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   