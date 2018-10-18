using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class GlobalDomainController : ApiController
    {
        public HttpResponseMessage GetAWBMessagingStockTenantsList(int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                IGlobalContext objectContext = GlobalContext.GetContext();
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository(objectContext);
                GlobalTenantRepository globalTenantsRepository = new GlobalTenantRepository(objectContext);

                List<TenantManagementList> myResult = new List<TenantManagementList>();
                IQueryable<TenantManagement> iQueryable1 = tenantManagementRepository.GetAWBStockPrepaidTenants();
                IQueryable<GlobalTenant> iQueryable2 = globalTenantsRepository.GetAllGlobalTenant().Where(d => d.IsActive);

                foreach (TenantManagement item in iQueryable1)
                {
                    if (iQueryable2.Where(d => d.Id == item.Id).Any())
                    {
                        myResult.Add(new TenantManagementList()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            PackageCode = item.PackageCode,
                        });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCheckTenantMangmnt(string loggedUserId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                IGlobalContext context = new GlobalContext();
                GlobalDomainService globalDomainService = new GlobalDomainService(context);
                TenantUserDataClass myResult = globalDomainService.CheckTenantMangmnt(tenant, loggedUserId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetGlobalSetting()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    JSGlobalSettings myResult = new JSGlobalSettings();

                    SettingRepository mySettingRepository = new SettingRepository();
                    Setting mySetting = mySettingRepository.GetSingleSetting("1");
                    if (mySetting != null)
                    {
                        myResult.Id = mySetting.Id;
                        myResult.LogitudeURL = mySetting.LogitudeURL;
                        myResult.LogoCode = mySetting.LogoCode;
                        myResult.WorkEnvironment = mySetting.WorkEnvironment;
                        myResult.SameUserLoginEnabled = mySetting.SameUserLoginEnabled;
                        myResult.LayoutDirection = mySetting.LayoutDirection;
                        myResult.ReportsRunUsingWR = mySetting.ReportsRunUsingWR;
                        myResult.DocumentFilingEmailDomain = mySetting.DocumentFilingEmailDomain;
                        myResult.DeploymentStage = mySetting.DeploymentStage;

                        if (LogitudeSettings.IsCostomsDeploy)
                        {
                            myResult.ProductInfo = LogitudeSettings.ProductInfo;//.Replace(Environment.NewLine ,"<br>") ;
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception e)
            {
                // the following Code was added by Rabaia in order to fix the Isolation Snapshot problem (Login Stuck on 87%) (Temporarly) 
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);

                        JSGlobalSettings myResult = new JSGlobalSettings();

                        SettingRepository mySettingRepository = new SettingRepository();
                        Setting mySetting = mySettingRepository.GetSingleSetting("1");
                        if (mySetting != null)
                        {
                            myResult.Id = mySetting.Id;
                            myResult.LogitudeURL = mySetting.LogitudeURL;
                            myResult.LogoCode = mySetting.LogoCode;
                            myResult.WorkEnvironment = mySetting.WorkEnvironment;
                            myResult.SameUserLoginEnabled = mySetting.SameUserLoginEnabled;
                            myResult.LayoutDirection = mySetting.LayoutDirection;
                            myResult.DocumentFilingEmailDomain = mySetting.DocumentFilingEmailDomain;
                        }

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
        }
        public HttpResponseMessage GetPrivateLableById(string Id)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    //string token = HttpContext.Current.Request.Headers["Token"];
                    //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    //int tenant = authToken.Tenant;

                    //SecurityUtility.AuthenticationOnTenant(tenant);

                    TenantManagmentPrivateLabelsRepository myRepository = new TenantManagmentPrivateLabelsRepository();
                    TenantManagmentPrivateLabels PrivateLable = myRepository.GetSingleTenantManagmentPrivateLabels(Id);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, PrivateLable);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTenantSetting()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    TenantSettingRepository entityRepository = new TenantSettingRepository(tenant);
                    TenantSettingQuery entityQuery = new TenantSettingQuery(entityRepository);
                    List<TenantSettingPM> myResult = entityQuery.GetTenantSettingsByTenant(tenant).ToList();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAccountingSystem(string AccountingSystemCode)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    AccountingSystemPM myResult = null;
                    if (!string.IsNullOrEmpty(AccountingSystemCode) && AccountingSystemCode != "null")
                    {
                        AccountingSystemQuery accountSystems = new AccountingSystemQuery(tenant);
                        myResult = accountSystems.GetSinglePM(AccountingSystemCode);
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllHelpResources()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                IGlobalContext context = new GlobalContext();
                GlobalDomainService globalDomainService = new GlobalDomainService(context);
                var result = globalDomainService.GetAllHelpResources(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineTenantExistsForAirline(string code)
        {
            try
            {
                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenantManagementRepository);

                    TenantManagement entityPOCO = tenantManagementRepository.GetTenantManagementByConnectedArline(code);
                    TenantManagementPM myResult = null;

                    if (entityPOCO != null)
                    {
                        myResult = tenantManagementQuery.GetSinglePM(entityPOCO.Id);
                    }
                    
                    //scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                //}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllBatchServicesDefinitionsPMs(string filterByDateCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                
                List<BatchServicesDefinitionPM> myResult = this.GetAllBatchServices(filterByDateCode);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private List<BatchServicesDefinitionPM> GetAllBatchServices(string dateFilterCode)
        {
            DateTime? filterByDate = null;
            switch (dateFilterCode)
            {
                case "L1D":
                    {
                        filterByDate = DateTime.UtcNow.AddHours(-24);
                        break;
                    }
                case "L2D":
                    {
                        filterByDate = DateTime.UtcNow.AddHours(-48);
                        break;
                    }
                case "L1H":
                    {
                        filterByDate = DateTime.UtcNow.AddHours(-1);
                        break;
                    }
                case "L2H":
                    {
                        filterByDate = DateTime.UtcNow.AddHours(-2);
                        break;
                    }
                case "L1Y":
                    {
                        filterByDate = DateTime.UtcNow.AddYears(-1);
                        break;
                    }
            }

            BatchServicesDefinitionRepository repository = new BatchServicesDefinitionRepository();
            List<BatchServicesDefinitionPM> TempList = new List<BatchServicesDefinitionPM>();

            IQueryable<BatchServicesDefinitionPM> result = (from a in repository.context.BatchServicesDefinitions
                          select new BatchServicesDefinitionPM()
                          {
                              ClassName = a.ClassName,
                              Code = a.Code,
                              InActive = a.BatchServicesDefinitionMods.InActive,
                              NumberOfThreads = a.BatchServicesDefinitionMods.NumberOfThreads,
                              Parameter1 = a.Parameter1,
                              Parameter2 = a.Parameter2
                          });

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                BatchServicesLogRepository Repo = new BatchServicesLogRepository();

                foreach (BatchServicesDefinitionPM item in result)
                {
                    List<BatchServicesLog> Logs = Repo.GetBatchServicesLogsByCode(item.Code, filterByDate);
                    int DoneItemsInFiveMinutes = 0;
                    int DoneItemsInOneHour = 0;
                    int DoneItemsInOneMinute = 0;
                    int NumberOfDoneItems = 0;
                    DateTime? LActivity = null;

                    foreach (BatchServicesLog Log in Logs)
                    {
                        item.CPU = Log.CPU;
                        DoneItemsInFiveMinutes += Log.DoneItemsInFiveMinutes;
                        DoneItemsInOneHour += Log.DoneItemsInOneHour;
                        DoneItemsInOneMinute += Log.DoneItemsInOneMinute;
                        NumberOfDoneItems += Log.NumberOfDoneItems != null ? (int)Log.NumberOfDoneItems : 0;

                        if (Log.LastActivity > LActivity && LActivity != null)
                        {
                            LActivity = Log.LastActivity;
                        }
                        else if (LActivity == null && Log.LastActivity != null)
                        {
                            LActivity = Log.LastActivity;
                        }
                    }

                    item.DoneItemsInFiveMinutes = DoneItemsInFiveMinutes;
                    item.DoneItemsInOneHour = DoneItemsInOneHour;
                    item.DoneItemsInOneMinute = DoneItemsInOneMinute;
                    item.NumberOfDoneItems = NumberOfDoneItems;
                    item.LastActivity = LActivity;
                    TempList.Add(item);
                }

                scope.Complete();
            }

            return TempList;
        }
        public HttpResponseMessage GetUpdateTenantZeroService(string Message)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                UpdateTenantZeroService myService = new UpdateTenantZeroService();
                myService.SendMessageToQueue(Message);

                bool myResult = true;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetParentTenants()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                IGlobalContext objectContext = GlobalContext.GetContext();
                TenantManagementRepository repository = new TenantManagementRepository(objectContext);
                TenantManagementQuery query = new TenantManagementQuery(repository);

                IQueryable<TenantManagement> parentTenants = repository.GetParentTenantManagements();
                IQueryable<TenantManagementList> result = query.GetIQueryableEntityList(parentTenants);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSettingsDocumentFilingEmailDomain()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SettingRepository mySettingRepository = new SettingRepository();
                Setting mySetting = mySettingRepository.GetSingleSetting("1");
                string myResult = mySetting.DocumentFilingEmailDomain;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class JSGlobalSettings
    {
        public string Id { get; set; }
        public string LogitudeURL { get; set; }
        public string LogoCode { get; set; }
        public string WorkEnvironment { get; set; } //customs,main....
        public bool SameUserLoginEnabled { get; set; }
        public string LayoutDirection { get; set; }
        public string ProductInfo { get; internal set; }
        public bool ReportsRunUsingWR { get; set; }
        public string DocumentFilingEmailDomain { get; set; }
        public string DeploymentStage { get; set; }
    }


}