using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class CommunicationLogsExtendedController : ApiController
    {

        public HttpResponseMessage Post(CommunicationLogExtendedArgs communicationLogExtendedArgs)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("CommunicationLog", "NEW", authToken.Tenant);

                    int tenant = authToken.Tenant;
                    ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                    ObjectTablePM table = tablesQuery.GetObjectTableByName(communicationLogExtendedArgs.ObjectTableName, 0);

                    CommunicationsParams logParams = new CommunicationsParams()
                    {
                        Tenant = tenant,
                        From = communicationLogExtendedArgs.From,
                        To = communicationLogExtendedArgs.To,
                        CommunicationLogTypeCode = communicationLogExtendedArgs.CommunicationLogTypeCode,
                        Priority = communicationLogExtendedArgs.Priority,
                        InOut = communicationLogExtendedArgs.InOut,
                        Status = communicationLogExtendedArgs.CommunicationStatusTypeCode,
                        LoggingObjectTableId = table.Id,
                        LoggingEntityId = communicationLogExtendedArgs.EntityId,
                        Subject = communicationLogExtendedArgs.Subject,
                        FolderName = communicationLogExtendedArgs.FolderName,
                        ByteData = Encoding.UTF8.GetBytes(communicationLogExtendedArgs.MessageBody),
                        FileExtension = communicationLogExtendedArgs.FileExtension,
                        Logs = communicationLogExtendedArgs.MessageException,
                    };

                    Communications.AddCommunicationLog(logParams);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, communicationLogExtendedArgs);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}