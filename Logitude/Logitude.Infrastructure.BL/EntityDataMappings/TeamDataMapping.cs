
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class TeamDataMapping: IMapping<TeamPM, Team>
   {
        public void CustomPMToPOCO(TeamPM entityPM, Team entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(TeamPM entityPM, Team entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ManagerUserName);
            if (!string.IsNullOrEmpty(entityPOCO.ManagerUserId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPOCO.Tenant);
                Contact contact = contactRepository.GetSingleContact(entityPOCO.ManagerUserId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.ManagerUserName = contact.EnglishName;
                }
            }
        }

        private void BuildSearchFields(TeamPM entityPM, Team entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   