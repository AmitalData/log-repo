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
  public  class DocumentFilingBackupBatchMapping
    {
      public static void MapEntity(DocumentFilingBackupBatchPM entityPM, DocumentFilingBackupBatch entityPOCO, bool isNewState, Tenant loggedTenant)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.BatchNumber = entityPM.BatchNumber;
                entityPOCO.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(loggedTenant.Id);

            }

            else
            {
                entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            }
             entityPOCO.IncludeBackedUp = entityPM.IncludeBackedUp;
            entityPOCO.DoneDate = entityPM.DoneDate;
            entityPOCO.Status = entityPM.Status;
            entityPOCO.FromDatetime = entityPM.FromDatetime;
            entityPOCO.ToDatetime = entityPM.ToDatetime;
            entityPOCO.TotalDocuments = entityPM.TotalDocuments;
            entityPOCO.TotalSucceeded = entityPM.TotalSucceeded;
            entityPOCO.TotalFailed = entityPM.TotalFailed;

        }

     
    }
}
