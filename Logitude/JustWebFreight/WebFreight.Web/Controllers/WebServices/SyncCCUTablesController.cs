using NetCommonHelper.Logger;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.Models;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class SyncCCUTablesController : ApiController
    {
        public IHttpActionResult GetSyncData(string fileNo = null, int? customsFileNo = null, bool allTask = false)
        {
            DevLog.Instance.WriteDebug($"GetSyncData, fileNo: {fileNo}, customsFileNo: {customsFileNo}, allTask: {allTask}");
            if(string.IsNullOrEmpty(fileNo) && !customsFileNo.HasValue)
                return BadRequest("FileNo or customsFileNo is required");

            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                List<EntityRecord> res = new SyncRecordQuery(tenant).GetUnsyncRecordsAndMarkAsInProcess(tenant, fileNo, customsFileNo, allTask);

                return Ok(res);
            }
            catch (Exception ex)
            {
                DevLog.Instance.WriteFatal(ex, "GetSyncData faild, fileNo: " + fileNo);
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateSyncData(string fileNo, DateTime syncDT)
        {
            DevLog.Instance.WriteDebug("UpdateSyncData, fileNo: " +  fileNo + ", syncDT: " +  syncDT);
            if (string.IsNullOrEmpty(fileNo) || syncDT == null)
                return BadRequest("FileNo and syncDT is required");

            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                new SyncRecordQuery(tenant).UpdateSyncData(tenant, fileNo, syncDT);

                return Ok();
            }
            catch (Exception ex)
            {
                DevLog.Instance.WriteFatal(ex, "UpdateSyncData faild, fileNo: " + fileNo + " +, syncDT: " + syncDT);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetLastSyncDate(string fileNo)
        {
            DevLog.Instance.WriteDebug("GetLastSyncDate, fileNo: " + fileNo);
            if (string.IsNullOrEmpty(fileNo))
                return BadRequest("FileNo is required");

            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                DateTime? lastSync = new SyncRecordQuery(tenant).GetLastSyncDate(tenant, fileNo);

                return Ok(lastSync);
            }
            catch (Exception ex)
            {
                DevLog.Instance.WriteFatal(ex, "GetLastSyncDate faild, fileNo: " + fileNo);
                return InternalServerError(ex);
            }
        }
    }
}