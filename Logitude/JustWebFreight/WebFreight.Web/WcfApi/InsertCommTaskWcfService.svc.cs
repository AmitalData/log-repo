using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.Server.Tools;
using WebFreight.Web.Security;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.ServiceModel.Activation;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InsertCommTaskWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select InsertCommTaskWcfService.svc or InsertCommTaskWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class InsertCommTaskWcfService : IInsertCommTaskWcfService
    {
        public Response InsertTask(int myTenant, int destinationTenant, int priority, string subject, List<QueueTask> queueTasks)
        {
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(myTenant);

                string userEmail = "system@tenant" + destinationTenant + ".com";
                UserRepository userRepository = new UserRepository(destinationTenant);
                User user = userRepository.GetSingleUserByEmail(userEmail, destinationTenant, true);

                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = destinationTenant,
                    CommunicationLogTypeCode = "Q",
                    QueueName = "externaltasksqueue" + destinationTenant + priority,
                    Priority = priority,
                    InOut = "I",
                    Status = "W",
                    LoggingUserId = user.Id,
                    Subject = subject,
                    FolderName = "ExternalTasksQueue",
                };

                logParams.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);
                var Id = Communications.AddCommunicationLog(logParams);
                response.Result = Id;


                return response;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }

        }
    }
}
