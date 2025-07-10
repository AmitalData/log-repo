
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class SignStationExtendedController : ApiController
    {



        public HttpResponseMessage GetSignStationGroupByStatus(string searchfields)
        {

            //_StatusList = ["Start", "תקין", "כישלון", "ממתין להזנת סיסמא", "כרטיס שגוי", "ללא הגדרה"];
            //_StatusList = ["Start", "OK", "Failure", "Waitingtoenterapassword", "Incorrectcard", "NoDefinition"];

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);


                List<MySignStationList> entityLists;


                var signQueueHSMService = new SignQueueHSMService();
                if (true || signQueueHSMService.IsHSMSign_IsOn(tenant))
                {
                    entityLists = GetSignStationDBHSM(searchfields, tenant);

                }
                else
                {
                    entityLists = GetAllStation(searchfields, tenant);
                }
                if (!String.IsNullOrWhiteSpace(searchfields))
                {
                    entityLists = entityLists
                        .Where(r => String.Concat(r.MachineUser, r.MachineUser, r.PersonId, r.SignerName, r.Status).ToLower().Contains(searchfields.ToLower())).ToList();
                }

                var q = (from a in entityLists
                         group a by a.Status into gStatus
                         select new { gStatus.Key, Total = gStatus.Count() }

                );

                var response = q.ToList();
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                string logKey = PerformanceLogger.LogCurrentTime();
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                //return reponseMessage;

                return reponseMessage;//Request.CreateResponse(HttpStatusCode.OK, entityLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static List<MySignStationList> GetSignStationDBHSM(string searchfields, int tenant)
        {
            List<MySignStationList> entityLists;
            var dbSignQueueService = new SignQueueHybridDbService();
            entityLists = dbSignQueueService.GetAllStation(searchfields, tenant);
            var signQueueHSMService = new SignQueueHSMService();
            if (signQueueHSMService.IsHSMSign_IsOn(tenant))
            {
                var hsmCertList = signQueueHSMService.GetHSMAllCertificates(tenant, false);

                entityLists = entityLists ?? new List<MySignStationList>();
                hsmCertList = hsmCertList ?? new List<MySignStationList>();
                entityLists = hsmCertList.Union(entityLists).ToList();
            }

            return entityLists;
        }

        public HttpResponseMessage GetSignStations(int skip, int take, string sortingCol, string sortingDir, string searchfields, string FilterByStatus)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);


                List<MySignStationList> entityLists = new List<MySignStationList>(); /*= GetAllStation(searchfields, tenant)*/;

                var signQueueHSMService = new SignQueueHSMService();
                if (true || signQueueHSMService.IsHSMSign_IsOn(tenant))

                {
                    entityLists = GetSignStationDBHSM(searchfields, tenant);
                }
                else
                {
                    entityLists = GetAllStation(searchfields, tenant);
                }
                if (!String.IsNullOrWhiteSpace(FilterByStatus))
                {
                    entityLists = entityLists.Where(r => r.Status.Trim().ToString().Equals(FilterByStatus.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                }


                ServiceResponse response = new ServiceResponse();
                //if (filters.GetCount)
                {
                    int count = entityLists.Count;
                    response.Count = count;
                }

                entityLists = GetList(skip, take, sortingCol, sortingDir, entityLists);
                //entityLists.Add(new SignStationList() { MachineName = "itzik" });

                response.Result = entityLists;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                string logKey = PerformanceLogger.LogCurrentTime();
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                //return reponseMessage;

                return reponseMessage;//Request.CreateResponse(HttpStatusCode.OK, entityLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static List<MySignStationList> GetAllStation(string searchfields, int tenant)
        {
            var entityLists = new List<MySignStationList>();
            List<SubscribeSignServer> mySubscribeSignServerList = SignQueue.Instance.GetCopyOfMySubscribeSignServerList(tenant);


            mySubscribeSignServerList.ForEach(
                s =>
                {
                    entityLists.Add(new MySignStationList()
                    {
                        PersonId = s.MySignCertificateClass.PersonId,
                        SignerName = s.MySignCertificateClass.SignerName,

                        MachineName = s.MySignCertificateClass.MachineName,
                        MachineUser = s.MySignCertificateClass.UserName,

                        CustomsAgentId = s.MySignCertificateClass.CustomsAgentId,


                        IsCompanySignOn = s.IsCompanySignOn,
                        IsPersonalSignOn = s.IsPersonalSignOn,

                        Status = "תקין",
                        LastSignAt = s.LastAccessedAt

                    });
                }
                );
            var statusList = SignQueue.Instance.GetCopyOfMySubscribeSignServerStatusList();
            statusList.ForEach(
                stsRow =>
                {
                    var fromentityLists = entityLists.FirstOrDefault(r => r.MachineName == stsRow.MachineName);
                    if (fromentityLists != null)
                    {

                        fromentityLists.Status = stsRow.Status;
                        fromentityLists.LastSignAt = stsRow.LastSuccessSignningAt;
                        fromentityLists.IsOk = stsRow.IsOk;
                        fromentityLists.VersionByFeatures = stsRow.VersionByFeatures;


                    }
                    else
                    {
                        entityLists.Add(new MySignStationList()
                        {
                            MachineName = stsRow.MachineName,
                            CustomsAgentId = stsRow.MySignCertificateClass.CustomsAgentId,
                            MachineUser = stsRow.UserName,
                            IsCompanySignOn = stsRow.IsCompanySignOn,
                            IsPersonalSignOn = stsRow.IsPersonalSignOn,
                            PersonId = stsRow.MySignCertificateClass.PersonId,
                            SignerName = stsRow.MySignCertificateClass.SignerName,
                            Status = stsRow.Status,
                            LastSignAt = stsRow.LastSuccessSignningAt,
                            IsOk = stsRow.IsOk,
                            VersionByFeatures = stsRow.VersionByFeatures,
                        });
                    }
                }
                );


            if (!String.IsNullOrWhiteSpace(searchfields))
            {
                entityLists = entityLists
                    .Where(r => String.Concat(r.MachineUser, r.MachineUser, r.PersonId, r.SignerName, r.Status).ToLower().Contains(searchfields.ToLower())).ToList();
            }

            return entityLists;
        }

        List<MySignStationList> GetList(int skip, int take, string sortingCol, string sortingDir, List<MySignStationList> entityLists)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();


            IQueryable<MySignStationList> query2 = entityLists.AsQueryable(); ;
            if (!String.IsNullOrWhiteSpace(sortingDir))
            {
                QueryOperations queryOperations = new QueryOperations() { SortByColumnName = sortingCol, SortDirectin = sortingDir };

                switch (sortingCol)
                {

                    case "IsPersonalSignOn":
                    case "IsCompanySignOn":
                        {
                            query2 = sortClass.GetSorterQuery<MySignStationList, bool>(queryOperations, query2);
                        }
                        break;
                    case "LastSignAt":
                        {
                            query2 = sortClass.GetSorterQuery<MySignStationList, DateTime>(queryOperations, query2);
                        }
                        break;
                    default:
                        query2 = sortClass.GetSorterQuery<MySignStationList, string>(queryOperations, query2);
                        break;
                }
            }


            //if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skip);
                query2 = query2.Take(take);
            }
            return query2.ToList();


        }


    }


}