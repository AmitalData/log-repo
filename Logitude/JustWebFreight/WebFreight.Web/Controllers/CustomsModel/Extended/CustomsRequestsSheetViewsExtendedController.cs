using System;
using WebFreight.Web.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.CustomsMessaging.RabbitMQ;
using RabbitMQ.Client;
using Logitude.Customs.BL.EntityQueryServices;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public partial class CustomsRequestsSheetViewsExtendedController : ApiController
    {

        public HttpResponseMessage GetStatistics(bool includingFuture)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ICustomContext context = CustomContext.GetContext(tenant);
                var summary = new InterfaceManagementQueryService(context).GetQueueMessagesSatistic(tenant, includingFuture);

                return Request.CreateResponse(HttpStatusCode.OK, summary);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
           
        }


        public HttpResponseMessage GetStatisticsByCourierDeclarations(string CourierMasterId)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ICustomContext context = CustomContext.GetContext(tenant);
                CustomsRequestsSheetListQueryService customsRequestsSheetQuery = new CustomsRequestsSheetListQueryService(context);
                var summary = customsRequestsSheetQuery.GetStatisticsByCourierDeclarations(tenant, CourierMasterId);         

                return Request.CreateResponse(HttpStatusCode.OK, summary);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public uint GetMessageCount(string queueName)
        {
            try
            {
                var factory = RabbitmqHelper.GetConnectionFactory(tryFromAppSettings: false);
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    return channel.MessageCount(queueName);
                }
            }
            catch (Exception)
            {
                return 0;
            }
            
        }
    }
}