using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class DocumentFilingBackupSettingMapping
    {
        public static void MapEntity(DocumentFilingBackupSettingPM entityPM, DocumentFilingBackupSetting entity, bool isNewState)
        {
            if (isNewState)
            {
                entity.Tenant = entityPM.Tenant;
            }

            entity.IsActive = entityPM.IsActive;
            entity.FTPDetailId = entityPM.FTPDetailId;
            entity.ActivationDate = entityPM.ActivationDate;
            entity.DeactivationDate = entityPM.DeactivationDate;

        }
    }
}
