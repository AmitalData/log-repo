using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class FTPDetailMapping
    {
        public static void MapEntity(FTPDetailPM itemPM, FTPDetail itemPoco, bool isNewEntity, string loggedContactId)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.CreatedByUserId = loggedContactId;
                itemPoco.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            }

            itemPoco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPoco.UpdatedByUserId = loggedContactId;
            itemPoco.UserName = itemPM.UserName;
            itemPoco.Password = itemPM.Password;
            itemPoco.Host = itemPM.Host;
            itemPoco.Folder = itemPM.Folder;
            itemPoco.InActive = itemPM.InActive;
            itemPoco.UseSFTP = itemPM.UseSFTP;

        }
    }
}
