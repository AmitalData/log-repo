using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data;
using Logitude.Workflow.BL;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.BL.EntityUpdateServices;
using Logitude.Workflow.Data.EntityListQueryServices;
using Logitude.Workflow.BL.EntityQueryServices;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.IO.Compression;
using Microsoft.WindowsAzure.Storage.Auth;
using Logitude.Workflow.Data.WorkflowStorage;

namespace WebFreight.Web.Controllers.WorkflowModel.Extended
{
    public class WorkflowInstanceExtendedController : ApiController
    {
        
        public HttpResponseMessage GetActivities(string workflowInstanceId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authenticationToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authenticationToken.Tenant);
                SecurityUtility.CheckContactFeature("WorkFlowInstanceActivity", "READ", authenticationToken.Tenant);

                int tenant = 951;//authenticationToken.Tenant;


                WorkflowInstanceStorage workflowInstanceStorage = new WorkflowInstanceStorage();

                List<WorkFlowInstanceActivityList> workflowInstanceActivities = workflowInstanceStorage.GetActivities(workflowInstanceId, tenant);

                if (workflowInstanceActivities == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ApiExceptionBuilder.BuildException(new Exception("WorkFlow instance not found")));
                }



                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                //return Request.CreateResponse(HttpStatusCode.OK, workflowInstanceActivities);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




        //private void UploadWorkflowInstanceFile()
        //{
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=logitudeflowstest;AccountKey=VfZwc4wLykgT5sB0vx5T2IWXAyZarLIjTkpaXsKquTRZ55BrwMxvxdfl5WWQDqY7HClucgbprXPb+ASty9hinQ==";
        //    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
        //    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        //    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("workflowinstances");
        //    cloudBlobContainer.CreateIfNotExists();


        //    //string fileName = "1-264256" + "." + "951";
        //    string fileName = "1" + "." + "1";

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
        //        {
        //            var jsonFile = archive.CreateEntry(fileName + ".json");

        //            using (var entryStream = jsonFile.Open())
        //            using (var streamWriter = new StreamWriter(entryStream))
        //            {
        //                streamWriter.Write(GetWorkflowInstanceJsonString());
        //            }
        //        }

        //        var bytes = memoryStream.GetBuffer();

        //        cloudBlobContainer.GetBlockBlobReference(fileName + ".zip").UploadFromByteArray(bytes, 0, bytes.Length);
        //    }

        //}

    }
}