using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.WebServices
{
    public class SyncCCUTablesController : ApiController
    {
        public IHttpActionResult GetSyncData(string fileNo)
        {
            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                List<EntityRecord> res = new SyncRecordQuery(tenant).GetUnsyncRecordsAndMarkAsInProcess(tenant, fileNo);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateSyncData(string fileNo, DateTime syncDT)
        {
            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                new SyncRecordQuery(tenant).UpdateSyncData(tenant, fileNo, syncDT);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetLastSyncDate(string fileNo)
        {
            try
            {
                int tenant = HeaderHelper.Authenticate().Tenant;
                DateTime? lastSync = new SyncRecordQuery(tenant).GetLastSyncDate(tenant, fileNo);

                return Ok(lastSync);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}