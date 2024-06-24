using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Utils;

namespace WebFreight.Web.Controllers.WebServices
{
    public class SyncCCUTablesController : ApiController
    {
        public IHttpActionResult GetSyncData(string fileNo)
        {
            Logger.LogDebug("GetSyncData, fileNo: '{0}'", fileNo);
            if(string.IsNullOrEmpty(fileNo))
                return BadRequest("FileNo is required");

            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                List<EntityRecord> res = new SyncRecordQuery(tenant).GetUnsyncRecordsAndMarkAsInProcess(tenant, fileNo);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetSyncData faild, fileNo: '{0}'", fileNo);
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateSyncData(string fileNo, DateTime syncDT)
        {
            Logger.LogDebug("UpdateSyncData, fileNo: '{0}', syncDT: '{1}'", fileNo, syncDT);
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
                Logger.LogError(ex, "UpdateSyncData faild, fileNo: '{0}', syncDT: '{1}'", fileNo, syncDT);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetLastSyncDate(string fileNo)
        {
            Logger.LogDebug("GetLastSyncDate, fileNo: '{0}'", fileNo);
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
                Logger.LogError(ex, "GetLastSyncDate faild, fileNo: '{0}'", fileNo);
                return InternalServerError(ex);
            }
        }
    }
}