using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public partial class CopyFromTenant0ExtendedController : ApiController
    {
        public CopyFromTenant0ExtendedController()
        {

        }






        public HttpResponseMessage GetAll(int tenant)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("CopyFromTenant0", "READ", authToken.Tenant);
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                CopyFromTenant0ListQueryService copyFromTenant0ListQueryService = new CopyFromTenant0ListQueryService(MyContext);
                List<CopyFromTenant0List> result = copyFromTenant0ListQueryService.GetList(tenant);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        [HttpGet]

        public HttpResponseMessage CopyTableFromTenant0(string tableName)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("CopyFromTenant0", "READ", authToken.Tenant);
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                string message = "Succeeded";
                if (!string.IsNullOrEmpty(tableName)) {
                    string table = tableName.Replace(" ", "");
                   

                    switch (table)
                    {
                     
                        case "ChartOfAccounts":
                            {
                                ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(authToken.Tenant);
                                chartOfAccountQueryService.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                   
                        case "GLAccounts":
                            {
                                GLAccountQueryService glAccountQueryService = new GLAccountQueryService(authToken.Tenant);
                                glAccountQueryService.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                        
                        case "AutomaticReconcileMethods":
                            {
                                AutomaticReconcileMethodQueryService automaticReconcileMethodQueryService = new AutomaticReconcileMethodQueryService(authToken.Tenant);
                                automaticReconcileMethodQueryService.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                    
                        case "VatTypes":
                            {
                                VatTypeQueryService vatTypeQueryService = new VatTypeQueryService(authToken.Tenant);
                                vatTypeQueryService.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                        
                        case "TasksScheduler":
                            {
                                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(authToken.Tenant);
                                tasksSchedulerQuery.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }

                          
                        case "Tenants":
                            {
                                TenantQuery tenantQuery = new TenantQuery(authToken.Tenant);
                             tenantQuery.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                      
                        case "CountryCities":
                            {
                                CountryCityQuery countryCityQuery = new CountryCityQuery(authToken.Tenant);
                                countryCityQuery.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                        case "ComputingPartners":
                            {
                                ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(authToken.Tenant);
                                computingPartnerQuery.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                        case "DocumentTypesandTemplates":
                            {
                                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(authToken.Tenant);
                                documentTypeTemplateQuery.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                        case "ReportTemplates":
                            {
                                ReportHelper reportHelper = new ReportHelper();
                                reportHelper.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }
                        case "AccountingSettings":
                            {
                                AccountingSettingQuery accountingSettingQuery = new AccountingSettingQuery(authToken.Tenant);
                                accountingSettingQuery.CopyFromTenant0(0, authToken.Tenant);
                                break;
                            }


                        default:
                            message = "No table found";
                            break;
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


    }
}