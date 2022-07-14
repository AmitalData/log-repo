using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class ElasticIndexerWaterMarksController : ApiController
    {
        IndexerWaterMarkRepository IndexerRepo = new IndexerWaterMarkRepository(0);
        public HttpResponseMessage Get(string tableName)
        {
            DateTime? LastUpdateDate = null;
            var WaterMark = IndexerRepo.GetSingle(tableName);
            //if (WaterMark != null)
            //{
            //    LastUpdateDate = WaterMark.LastUpdateDate;
            //}
            return Request.CreateResponse(HttpStatusCode.OK, WaterMark);
        }
        public HttpResponseMessage Post(IndexerWaterMark waterMark)
        {
            try
            {
                var currentWaterMark = IndexerRepo.GetSingle(waterMark.TableName);
                if (currentWaterMark != null)
                {
                    currentWaterMark.LastUpdateDate = waterMark.LastUpdateDate;
                    IndexerRepo.Update(currentWaterMark);
                }
                else
                {
                    currentWaterMark = waterMark;
                    IndexerRepo.Add(waterMark);
                }
                IndexerRepo.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, currentWaterMark);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ElasticIndexerWaterMarksController : GetTenantLogoUri Method", null);
                return null;
            }

        }          
    } 
}