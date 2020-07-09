using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class InterestReportsForEligibleCustomerCreationController : ApiController
    {
        public HttpResponseMessage PostInterestReportsForEligibleCustomerCreationInBatch(DateTime interestCalculationDate)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;
                InterestReportsCreationForCustomersArgs interestReportsCreationForCustomersArgs = new InterestReportsCreationForCustomersArgs(interestCalculationDate, tenant);
                string batchTaskId = CreateBatchTaskExecution(interestReportsCreationForCustomersArgs);


                return Request.CreateResponse(HttpStatusCode.OK, batchTaskId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private string CreateBatchTaskExecution(InterestReportsCreationForCustomersArgs args)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportsCreationForCustomersArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Interest Reports Creation In Batch",
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportsForEligibleCustomersCreationService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(args.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  args.Tenant.ToString() }
                }, args.Tenant);


            return taskExe.Id;
        }

    }
}