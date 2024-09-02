using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
//using Logitude.Server.Tools.SignalRHubs;
using Logitude.SystemLogs;
using Microsoft.AspNet.SignalR;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

using Stimulsoft.Report.Web;
using System;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using WebFreight.Web.Helpers;
using WebFreight.Web.Stimulsoft.fonts;
using WebFreight.Web.Helpers.StimulReportCustomizationDataProvider;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using static WebFreight.Web.Helpers.ReportHelper;

namespace WebFreight.Web.Stimulsoft
{
    public partial class Designer : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            StimulsoftFontsService.AddFonts();
            StiWebDesigner.CacheHelper = new StiMyCacheHelper();
            StiWebViewer.CacheHelper = new StiMyCacheHelper();

            if (Page != null && !Page.IsPostBack)
            {
                //this.LogitudeStiWebDesigner.UseRelativeUrls = true;
                //this.LogitudeStiWebDesigner.CacheMode = StiServerCacheMode.ObjectSession;
                //this.LogitudeStiWebDesigner.CacheItemPriority = CacheItemPriority.
                // this.LogitudeStiWebDesigner.CacheMode = StiServerCacheMode.ObjectSession;
                //this.LogitudeStiWebDesigner.CacheMode = Web.StiServerCacheMode

                //this.StiMobileDesigner1.RenderMode = StiRenderMode.AjaxWithCache;

                //String closeScript = "<script type='text/javascript'> window.parent.postMessage('true', '*');</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", closeScript, false);

                var templateId = Request.QueryString["templateId"];
                var reportTemplateId = Request.QueryString["reportTemplateId"];
                var reportsTemplateId = Request.QueryString["reportsTemplateId"];
                var tenantPar = Request.QueryString["tenant"];
                var token = Request.QueryString["token"];
                var processType = Request.QueryString["processtype"];
                var templateType = Request.QueryString["templateType"];

                StiReport report = new StiReport();
                #region Document Type Template
                if (!string.IsNullOrEmpty(tenantPar) && !string.IsNullOrEmpty(token))
                {
                    int tenant = int.Parse(tenantPar);
                   
                    if (!string.IsNullOrEmpty(templateId))
                    {
                        DocumentTypeTemplateQuery tempQuery = new DocumentTypeTemplateQuery(tenant);
                        DocumentTypeTemplatePM documentTypeTemplatePM = tempQuery.GetSinglePM(templateId, tenant);
                        if (documentTypeTemplatePM.TemplateBody != null)
                        {
                            report.Load(documentTypeTemplatePM.TemplateBody);
                            StiVariable logo = report.Dictionary.Variables["Logo"];
                            if (logo != null)
                            {
                                logo.ValueObject = null;
                            }
                        }

                        List<StiBusinessObjectData> stiBusinessObjects = new StiBusinessObjectDataService().Get(documentTypeTemplatePM);
                        if (stiBusinessObjects.Count > 0)
                        {
                            report.RegBusinessObject(stiBusinessObjects);
                            report.Dictionary.SynchronizeBusinessObjects(stiBusinessObjects.Count());
                        }

                    }
                    #endregion

                #region ReportTemplate
                    else if(!string.IsNullOrEmpty(reportTemplateId))
                    {
                        ReportHelper reportHelper = new ReportHelper();
                        byte[] fileData = reportHelper.LoadDataToStimulReport(processType, reportTemplateId, tenant, reportsTemplateId, templateType);
                        if (fileData != null)
                        {
                            report.Load(fileData);
                        }
                        if (processType == "ReportPreview" || templateType == "E")
                        {
                            LogitudeStiWebDesigner.ShowSaveButton = false;
                            LogitudeStiWebDesigner.ShowSaveDialog = false;
                            LogitudeStiWebDesigner.ShowDictionary = false;
                            LogitudeStiWebDesigner.ShowReportTree = false;
                            LogitudeStiWebDesigner.ShowPanel = false;
                            LogitudeStiWebDesigner.ShowTooltips = false;
                            LogitudeStiWebDesigner.ShowTooltipsHelp = false;
                            LogitudeStiWebDesigner.ShowFileMenu = false;
                            LogitudeStiWebDesigner.ShowInsertButton = false;
                            LogitudeStiWebDesigner.ShowLayoutButton = false;
                            LogitudeStiWebDesigner.ShowPreviewButton = templateType != "E";
                            LogitudeStiWebDesigner.ViewStateMode = ViewStateMode.Disabled;
                            LogitudeStiWebDesigner.Enabled = false;
                            //  LogitudeStiWebDesigner.ShowPropertiesGrid = false;
                        }
                    }
                    #endregion

                    ReFillBusinessObjects(report.Dictionary.BusinessObjects, processType, tenant, reportTemplateId);

                    LogitudeStiWebDesigner.Report = report;
                }
                
   
                else
                {
                    // Response.Output.Write("Sorry you’re not authenticated to view this document.");
                }



                //Microsoft.AspNet.SignalR.Client.IHubProxy _context = GlobalHost.ConnectionManager.GetHubContext<LogitudeGeneralHub>(); 
                //_context.On<string, string>("Message4U", (name, tenant) =>
                //{

                //}


            }
        }

        private void ReFillBusinessObjects(StiBusinessObjectsCollection bo, string processType, int tenant, string reportTemplateId)
        {
            if (processType == "ReportPreview" || bo == null || tenant == null || string.IsNullOrEmpty(reportTemplateId))
                return;
            
            ReportsTemplatesVersionQuery reportsTemplatesVersionQuery = new ReportsTemplatesVersionQuery(tenant);
            ReportsTemplatesVersionPM reportsTemplatesVersionPM = reportsTemplatesVersionQuery.GetLastReportsTemplatesVersionPMByReportsTemplateId(reportTemplateId, tenant);
            if(reportsTemplatesVersionPM == null)
                return;

            string code = new ReportQuery(tenant).GetReportCodeById(reportsTemplatesVersionPM.ReportId, tenant);
            if (string.IsNullOrEmpty(code))
                return;
            
            ReportHelper reportHelper = new ReportHelper();
            new List<ISlvLeaf>();
            string dpName = reportHelper.GetDataProviderName(code);
            if (string.IsNullOrEmpty(dpName))
                return;

            List<ISlvLeaf> variablesList = reportHelper.GetPropertyNames(dpName, new List<ISlvLeaf>());
            StiBusinessObject businessObject = null;

            if (bo.Count == 0)
            {
                string dpNameLastPart = dpName.Substring(dpName.LastIndexOf('.') + 1);
                businessObject = new StiBusinessObject(code, dpNameLastPart, dpNameLastPart, Guid.NewGuid().ToString("N"));
                bo.Add(businessObject);
            }
            else
                businessObject = bo[0];
            
            CreateBusinessObject(businessObject, variablesList);
        }   

        private void CreateBusinessObject(StiBusinessObject businessObject, List<ISlvLeaf> variablesList)
        {
            variablesList.ForEach(variable =>
            {
                if (variable.expanded)
                {
                    if (businessObject.BusinessObjects.ToList().Any(child => child.Name == variable.content))
                        return;

                    var child = new StiBusinessObject("", variable.content, variable.content, Guid.NewGuid().ToString("N"));
                    businessObject.BusinessObjects.Add(child);
                    CreateBusinessObject(child, variable.children);
                }
                else if (businessObject.Columns.ToList().Any(col => col.Name == variable.content))
                    return;
                else
                    businessObject.Columns.Add(new StiDataColumn(variable.content, variable.type));
            });            
        }

        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
        protected void LogitudeStiWebDesigner_SaveReport(object sender, StiSaveReportEventArgs e)
        {
            //try
            //{
            StiReport report = e.Report;

            var templateId = Request.QueryString["templateId"];
            var tenantPar = Request.QueryString["tenant"];
            var token = Request.QueryString["token"];
            var sessionId = Request.QueryString["sessionId"];
            var reportTemplateId = Request.QueryString["reportTemplateId"];
            var processType = Request.QueryString["processtype"];
            if (!string.IsNullOrEmpty(token) && processType != "ReportPreview")
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken != null)
                {
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(authToken.Email), new string[0]);
                    if (!string.IsNullOrEmpty(tenantPar) && !string.IsNullOrEmpty(token))
                    {
                        int tenant = int.Parse(tenantPar);

                        UserRepository userRep = new UserRepository(tenant);
                        User loggedUser = userRep.GetSingleUserByEmail(authToken.Email, tenant);

                        #region Document type Template
                        if (!string.IsNullOrEmpty(templateId))
                        {
                            DocumentTypeTemplateQuery tempQuery = new DocumentTypeTemplateQuery(tenant);
                            DocumentTypeTemplatePM documentTypeTemplatePM = tempQuery.GetSinglePM(templateId, tenant);

                            if (documentTypeTemplatePM != null)
                            {
                                documentTypeTemplatePM.LastUpdatedByUserId = loggedUser.Id;
                                documentTypeTemplatePM.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                documentTypeTemplatePM.TemplateBody = report.SaveToByteArray();//StiMobileDesigner1.Report.Save(//report.SaveDocumentToByteArray();
                                documentTypeTemplatePM.TemplateTechnologyCode = "AG";

                                DocumentTypeTemplateService templateService = new DocumentTypeTemplateService(CommonDataContext.GetContext(tenant), tenant);
                                templateService.Update(documentTypeTemplatePM);
                            }

                            this.LogitudeStiWebDesigner.Visible = false;

                            //SignalRHubMessageSender.SendSignalRMessage("StimulSaved", "User" + loggedUser.Id + tenant + sessionId, templateId);
                        }
                        #endregion

                        #region ReportTemplate
                        else if (!string.IsNullOrEmpty(reportTemplateId))
                        {
                            byte[] fileData = report.SaveToByteArray();
                            ReportHelper reportHelper = new ReportHelper();
                            reportHelper.StimulReportSaved(processType, reportTemplateId, fileData, loggedUser.Id, tenant);
                            this.LogitudeStiWebDesigner.Visible = false;
                            //SignalRHubMessageSender.SendSignalRMessage("StimulReportSaved", "User" + loggedUser.Id + tenant + sessionId, reportTemplateId);
                        }

                        #endregion


                        //HubEventPublisher.PublishChannelEvent(new HubChannelEvent() { ChannelName = "Tenant" + tenant, EventName = "StimulSaved", Data = templateId });
                        //String closeScript = "<script type='text/javascript'> window.parent.postMessage('true', '*');</script>";
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", closeScript, false);

                        //ClientScript.RegisterStartupScript(GetType(), "AutoPostBackScript",
                        //                  "alert('hi');", true);

                        //String closeScript = "<script type='text/javascript'> console.log('olaaaaaaaaaaaaaaaaa');  alert('Called!');</script>";
                        // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", closeScript, false);


                        //String closeScript = "<script type='text/javascript'> window.sessionStorage.setItem('designerClosed','true')</script>";
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", closeScript, false);

                        //String closeScript = "<script type='text/javascript'>self.close();</script>";
                        //String closeScript = "<script type='text/javascript'>window.stimulsoftDesignerComponentRef.zone.run(() => { window.stimulsoftDesignerComponentRef.component.stimuldesignerFinished('true'); })</script>";
                        //String closeScript = "<script type='text/javascript'> window.parent.postMessage('true', '*');</script>";

                        String closeScript = "<script type='text/javascript'>self.close();</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", closeScript, false);


                    }
                   


                }
                //else
                //{
                //    HttpContext.Current.User = null;
                //    Response.Output.Write("Sorry you’re not authenticated to view this document.");
                //}
            }
            else
            {
                //Response.Output.Write("Sorry you’re not authenticated to view this document.");
            }


            // }
            //catch (Exception exception)
            //{
            //    string ErrorMessage = exception.Message;
            //    Response.Clear();
            //    Response.Output.Write(ErrorMessage);
            //}

        }

    }


    public class StiMyCacheHelper : StiCacheHelper
    {
        public override StiReport GetReport(string guid, StiServerCacheMode mode, TimeSpan timeout, CacheItemPriority priority)
        {
            //string path = Path.Combine(HttpContext.Current.Server.MapPath(string.Empty), "CacheFiles", guid);
            //if (File.Exists(path))
            //{
            //    StiReport report = new StiReport();
            //    string packedReport = File.ReadAllText(path);
            //    if (guid.EndsWith("template")) report.LoadPackedReportFromString(packedReport);
            //    else report.LoadPackedDocumentFromString(packedReport);

            //    return report;
            //}
            //return null;

            BlobFileInfo fileInfo = GetBlobFileInfo(guid);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] result = storageservice.Read(fileInfo);
            if (result != null)
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                string packedReport = encoding.GetString(result);
                if (!string.IsNullOrEmpty(packedReport))
                {
                    StiReport report = new StiReport();
                    if (guid.EndsWith("template")) report.LoadPackedReportFromString(packedReport);
                    else report.LoadPackedDocumentFromString(packedReport);

                    return report;
                }
            }
            return null;
            //return base.GetReport(guid, mode, timeout, priority);
        }

        public override void SaveReport(StiReport report, string guid, StiServerCacheMode mode, TimeSpan timeout, CacheItemPriority priority)
        {
            string packedReport = guid.EndsWith("template") ? report.SavePackedReportToString() : report.SavePackedDocumentToString();
            //string path = Path.Combine(HttpContext.Current.Server.MapPath(string.Empty), "CacheFiles", guid);
            //File.WriteAllText(path, packedReport);

            BlobFileInfo fileInfo = GetBlobFileInfo(guid);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            byte[] reportData = encoding.GetBytes(packedReport);
            //if (reportData != null)
            // {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

            fileInfo.FileSize = reportData.Length;
            storageservice.Write(reportData, fileInfo);
            //  }

            //base.SaveReport(report, guid, mode, timeout, priority);
        }

        private BlobFileInfo GetBlobFileInfo(string guid)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = guid,
                FolderName = "temp",
                Extension = "txt",
                Tenant = 0,
            };
            return fileInfo;
        }
    }
}