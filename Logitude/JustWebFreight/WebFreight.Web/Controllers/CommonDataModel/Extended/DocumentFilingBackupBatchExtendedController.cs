using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class DocumentFilingBackupBatchExtendedController : ApiController
    {
        public HttpResponseMessage GetDocumentFilingBackupBatchPMs()
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("DocumentFilingBackupBatch", "READ", authToken.Tenant);
                DocumentFilingBackupBatchQuery documentFilingBackupBatchQuery = new DocumentFilingBackupBatchQuery(authToken.Tenant);
                List<DocumentFilingBackupBatchPM> documentFilingBackupBatchPMs = documentFilingBackupBatchQuery.GetDocumentFilingBackupBatchPMs(authToken.Tenant).ToList();
                foreach (var item in documentFilingBackupBatchPMs)
                {
                    var queueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(authToken.Tenant);
                    var AllQueues = queueMessageMoreDetailsQuery.GetIQueryableQueueMessageMoreDetailsPMByField1Field2(item.Id, item.BatchNumber);
                    item.TotalFailed = AllQueues.Where(a => a.Status == -1).Count();
                    item.TotalSucceeded = AllQueues.Where(a => a.Status == 1).Count();
                    item.TotalDocuments = AllQueues.Count();
                     
                    if (item.TotalDocuments == (item.TotalFailed + item.TotalSucceeded))
                    {
                        if (item.TotalDocuments != 0 || item.Status != "Created")
                        {
                            if (item.TotalFailed > 0)
                            {
                                item.Status = "Failed";
                            }
                            else
                            {
                                item.Status = "Done";
                            }
                        }
                    }
                    else if (item.TotalDocuments != (item.TotalFailed + item.TotalSucceeded))
                    {
                        item.Status = "In Progress";
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentFilingBackupBatchPMs);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}