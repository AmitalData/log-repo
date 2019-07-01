using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Linq;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using System.Collections.Generic;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Syncfusion.XlsIO;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Stimulsoft.Base.Excel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools.QueueService;
using WebFreight.Web.Helpers.APIHelpers;
using System.Reflection;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TariffDomainController : ApiController
    {
        private PortRepository portRepository;

        public HttpResponseMessage GetTariffsCounts()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                string loggedContactId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }


                TariffQueryService tariffQueryService = new TariffQueryService(tenant);

                TariffsSummary myResult = tariffQueryService.GetCount(tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAvailableAirlineFreightTariffs(string FromPort, string ToPort, string BetweenDate, double Weight)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);
                DateTime? BetweenDateOBJ = DateHelper.GetDate(BetweenDate);
                if (BetweenDateOBJ == null)
                {
                    BetweenDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }



                TariffQueryService tariffQueryService = new TariffQueryService(tenant);

                List<TariffSearchSummary> myResult = tariffQueryService.GetTariffSearchSummary(FromPort, ToPort, BetweenDateOBJ, Weight, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTenantTariffSetting()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ITariffModuleContext iContext = TariffModuleContext.GetContext(authToken.Tenant);
                TariffSettingPM entityPM = (from d in iContext.TariffSettings
                                            where d.Tenant == authToken.Tenant
                                            select new TariffSettingPM()
                                            {
                                                Id = d.Id,
                                                Tenant = d.Tenant,
                                                DefaultPriceSteps = d.DefaultPriceSteps,
                                                DefaultWarningPercentage = d.DefaultWarningPercentage
                                            }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDownloadTariff(string tariffId, int version, string type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                string loggedUserEmail = authToken.Email;

                ITariffModuleContext context = TariffModuleContext.GetContext(tenant);
                TariffQueryService tariffQuery = new TariffQueryService(context);
                TariffPM tariff = tariffQuery.GetSingle(tariffId, false, false);
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                string fileName = "";
                if (tariff != null)
                {
                    TariffVersionQueryService tariffVersionQuery = new TariffVersionQueryService(context);
                    TariffVersionPM tariffVersion = tariffVersionQuery.GetSingle(tariffId, version, true, false);

                    if (tariffVersion != null)
                    {
                        byte[] data = null;

                        if (tariff.TypeCode == "AFC")
                        {
                            data = this.ExportAirFreightCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }

                        else if (tariff.TypeCode == "ASC")
                        {
                            data = this.ExportAirSurchargesCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }

                        fileName = "Tariffs" + DateTime.Now.ToShortDateString();

                        if (data != null)
                        {
                            BlobFileInfo fileInfo = new BlobFileInfo()
                            {
                                FileName = fileName,
                                FolderName = "others",
                                Extension = "xls",
                                Tenant = tenant,
                                FileSize = data.Length,
                            };

                            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                            storageservice.Write(data, fileInfo);
                            this.EventTrace(tariff, type, loggedContact);
                        }


                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void EventTrace(TariffPM tariffPM, string type, ContactPM loggedContact)
        {
            if (type == "Template")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tariffPM.Tenant,
                    EventTypeCode = "TDLD",
                    UserId = loggedContact.Id,
                    EntityId = tariffPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = "Tariff Header exported"
                });
            }
            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tariffPM.Tenant,
                    EventTypeCode = "TDLD",
                    UserId = loggedContact.Id,
                    EntityId = tariffPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = "Tariff Lines exported"
                });
            }


        }
        private byte[] ExportAirFreightCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            tariffLines = tariffLines.OrderBy(P => P.Index).ToList();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            // Build excel headers 
            DataTable table = new DataTable();
            string[] steps = tariff.PriceSteps.Split(',');
            if (steps.Length > 1)
            {
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("Min Price");

                sheet1.InsertColumn(3, steps.Length, ExcelInsertOptions.FormatAsAfter);
                for (int i = 0; i < steps.Length; i++)
                {
                    table.Columns.Add(steps[i] + " KG");
                }

                table.Columns.Add("Notes");
            }
            else
            {
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("Min Price");
                table.Columns.Add(tariff.PriceSteps + " KG");
                table.Columns.Add("Notes");
            }

            string range = "A1:E1";
            if (steps.Length == 2)
            {
                range = "A1:F1";
            }

            else if (steps.Length == 3)
            {
                range = "A1:G1";
            }

            else if (steps.Length == 4)
            {
                range = "A1:H1";
            }

            else if (steps.Length == 5)
            {
                range = "A1:I1";
            }

            else if (steps.Length == 6)
            {
                range = "A1:J1";
            }

            else if (steps.Length == 7)
            {
                range = "A1:K1";
            }

            else if (steps.Length == 8)
            {
                range = "A1:L1";
            }

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.OriginPortCode ?? null;
                        row[1] = item.DestinationPortCode ?? null;
                        row[2] = item.MinPrice ?? null;
                        row[3] = item.Step1Price ?? null;

                        if (item.Step2Price.HasValue)
                        {
                            row[4] = item.Step2Price ?? null;
                        }

                        if (item.Step3Price.HasValue)
                        {
                            row[5] = item.Step3Price ?? null;
                        }

                        if (item.Step4Price.HasValue)
                        {
                            row[6] = item.Step4Price ?? null;
                        }

                        if (item.Step5Price.HasValue)
                        {
                            row[7] = item.Step5Price ?? null;
                        }

                        if (item.Step6Price.HasValue)
                        {
                            row[8] = item.Step6Price ?? null;
                        }

                        if (item.Step7Price.HasValue)
                        {
                            row[9] = item.Step7Price ?? null;
                        }

                        if (item.Step8Price.HasValue)
                        {
                            row[10] = item.Step8Price ?? null;
                        }

                        row[steps.Length + 3] = item.Notes ?? null;

                        table.Rows.Add(row);
                    }
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory);
            return memory.ToArray();
        }
        private byte[] ExportAirSurchargesCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            tariffLines = tariffLines.OrderBy(P => P.Index).ToList();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            DataTable table = new DataTable();

            #region header
            table.Columns.Add("From");
            table.Columns.Add("To");
            table.Columns.Add("Start Date", typeof(DateTime));

            int count = 0;
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
            ChargesType chargesType = null;
            if (!string.IsNullOrEmpty(tariff.Surcharge1Id))
            {
                count = 1;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge1Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge2Id))
            {
                count = 2;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge2Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge3Id))
            {
                count = 3;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge3Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge4Id))
            {
                count = 4;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge4Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge5Id))
            {
                count = 5;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge5Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge6Id))
            {
                count = 6;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge6Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge7Id))
            {
                count = 7;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge7Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge8Id))
            {
                count = 8;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge8Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge9Id))
            {
                count = 9;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge9Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge10Id))
            {
                count = 10;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge10Id, tenant);
                if (chargesType != null)
                {
                    table.Columns.Add(chargesType.Code);
                }
            }

            table.Columns.Add("Notes");
            #endregion

            #region range
            string range = "A1:D1";
            if (count == 1)
            {
                range = "A1:E1";
            }

            else if (count == 2)
            {
                range = "A1:F1";
            }

            else if (count == 3)
            {
                range = "A1:G1";
            }

            else if (count == 4)
            {
                range = "A1:H1";
            }

            else if (count == 5)
            {
                range = "A1:I1";
            }

            else if (count == 6)
            {
                range = "A1:J1";
            }

            else if (count == 7)
            {
                range = "A1:K1";
            }

            else if (count == 8)
            {
                range = "A1:L1";
            }

            else if (count == 9)
            {
                range = "A1:M1";
            }

            else if (count == 10)
            {
                range = "A1:N1";
            }
            #endregion

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.OriginPortCode ?? null;
                        row[1] = item.DestinationPortCode ?? null;
                        row[2] = item.StartDate ?? null;

                        if (item.Surcharge1Price.HasValue)
                        {
                            row[3] = item.Surcharge1Price ?? null;
                        }

                        if (item.Surcharge2Price.HasValue)
                        {
                            row[4] = item.Surcharge2Price ?? null;
                        }

                        if (item.Surcharge3Price.HasValue)
                        {
                            row[5] = item.Surcharge3Price ?? null;
                        }

                        if (item.Surcharge4Price.HasValue)
                        {
                            row[6] = item.Surcharge4Price ?? null;
                        }

                        if (item.Surcharge5Price.HasValue)
                        {
                            row[7] = item.Surcharge5Price ?? null;
                        }

                        if (item.Surcharge6Price.HasValue)
                        {
                            row[8] = item.Surcharge6Price ?? null;
                        }

                        if (item.Surcharge7Price.HasValue)
                        {
                            row[9] = item.Surcharge7Price ?? null;
                        }

                        if (item.Surcharge8Price.HasValue)
                        {
                            row[10] = item.Surcharge8Price ?? null;
                        }

                        if (item.Surcharge9Price.HasValue)
                        {
                            row[11] = item.Surcharge9Price ?? null;
                        }

                        if (item.Surcharge10Price.HasValue)
                        {
                            row[12] = item.Surcharge10Price ?? null;
                        }

                        row[count + 3] = item.Notes ?? null;

                        table.Rows.Add(row);
                    }

                    string dateRange = "C2:C" + (tariffLines.Count + 1);
                    sheet1.Range[dateRange].ColumnWidth = 14;
                    sheet1.Range[dateRange].NumberFormat = "dd/mm/yyyy";
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory);
            return memory.ToArray();
        }

        public HttpResponseMessage GetApproveVersion(string tariffId, int version)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                string loggedContactId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }

                ITariffModuleContext context = TariffModuleContext.GetContext(tenant);
                TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(context);

                TariffVersion tariffVersion = tariffVersionRepository.GetSingle(tariffId, version, tenant);
                if (tariffVersion != null)
                {
                    tariffVersion.IsDraft = false;
                    tariffVersion.ApprovedByUserId = loggedContactId;
                    tariffVersion.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                    tariffVersionRepository.Update(tariffVersion);
                    tariffVersionRepository.SubmitChanges();

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "VNAP",
                        UserId = loggedContactId,
                        EntityId = tariffId,
                        ObjectTableName = "Tariff",
                        Notes = "Version " + tariffVersion.Version + " approved",
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostUploadExcelFile")]
        public HttpResponseMessage PostUploadExcelFile(TariffFilterParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                filter.Tenant = authToken.Tenant;

                string loggedUserEmail = authToken.Email;

                ContactQuery contactQuery = new ContactQuery(authToken.Tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, authToken.Tenant);

                this.portRepository = new PortRepository(authToken.Tenant);

                byte[] fileData = Convert.FromBase64String(filter.FileData);

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                List<ExcelTariffLines> tariffLinesResult = new List<ExcelTariffLines>();
                if (filter.TariffType == "AFC")
                {
                    tariffLinesResult = this.BuildAirFreightCostExcelLines(sheet, authToken.Tenant);
                }

                else if (filter.TariffType == "ASC")
                {
                    tariffLinesResult = this.BuildAirSurchargesCostExcelLines(sheet, authToken.Tenant);
                }


                return Request.CreateResponse(HttpStatusCode.OK, tariffLinesResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostGenerateTariffsFromExcel")]
        public HttpResponseMessage PostGenerateTariffsFromExcel(TariffFilterParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                filter.Tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                ContactQuery contactQuery = new ContactQuery(authToken.Tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, authToken.Tenant);

                this.portRepository = new PortRepository(authToken.Tenant);

                byte[] fileData = Convert.FromBase64String(filter.FileData);

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                List<ExcelTariffLines> tariffLinesResult = new List<ExcelTariffLines>();
                tariffLinesResult = this.BuildAirFreightCostExcelLines(sheet, authToken.Tenant);

                this.GenerateExcel(tariffLinesResult, loggedUserEmail, authToken.Tenant);

                //TariffsExcelGeneratorArgs args = new TariffsExcelGeneratorArgs() {
                //    LoggedUserEmail = loggedUserEmail,
                //    Tenant = authToken.Tenant, 
                //    TariffLines = tariffLinesResult,
                //};
                //var stringwriter = new System.IO.StringWriter();
                //var serializer = new XmlSerializer(typeof(TariffsExcelGeneratorArgs));
                //serializer.Serialize(stringwriter, args);
                //string xmlParameters = stringwriter.ToString();

                //BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
                //{
                //    Subject = "Generate Tariffs From Excel",
                //    Tenant = authToken.Tenant,
                //    ChangeSetOp = ChangeSetOperation.Insert,
                //    ClassName = "WebFreight.Web.Helpers.APIHelpers.TariffGeneratorFromExcel,WebFreight.Web",
                //    CreateDate = DateTime.Now,
                //    PrametersXml = xmlParameters,
                //    StatusCode = "C",
                //};

                //IInfrastructureContext MyContext = InfrastructureContext.GetContext(authToken.Tenant);
                //BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                //bteUpdateService.Update(taskExe, true);

                //// 2- Send to queue
                //IQueueService queueservice = new DbQueueService();
                //queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                //queueservice.Send(new Dictionary<string, string>()
                //{
                //    { "BatchTaskExecutionId", taskExe.Id },
                //    { "Tenant", authToken.Tenant.ToString() }
                //});

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void GenerateExcel(List<ExcelTariffLines> tariffLines, string loggedUserEmail, int tenant)
        {
            ITariffModuleContext iContext = TariffModuleContext.GetContext(tenant);
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            UserRepository userRepository = new UserRepository(context);
            TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == tenant select d).FirstOrDefault();

            if (iTariffSetting != null)
            {
                Random random = new Random();
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                User loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, false);
                List<Card> airlines = context.Cards.Where(d => d.Tenant == tenant && d.PartnerTypeId == "AL").Take(20).ToList();
                List<string> currencyIds = context.Currencies.Where(d => d.Tenant == tenant).Select(s => s.Id).ToList();
                List<Port> ports = context.Ports.Where(d => d.Tenant == tenant).ToList();

                for (int i = 0; i < 20; i++)
                {
                    List<TariffVersion> versions = new List<TariffVersion>();
                    Card airline = airlines[random.Next(airlines.Count)];

                    Tariff tariff = new Tariff()
                    {
                        Id = IdCounter.GetNumber("Tariff", tenant),
                        TariffNumber = CodeCounter.GetNumber("Tariff", tenant).ToString(),
                        PriceSteps = iTariffSetting.DefaultPriceSteps,
                        Tenant = tenant,
                        CreateDate = todayDate,
                        UpdateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        UpdatedByUserId = loggedUser.Id,
                        SellerId = airline.Id,
                        Name = "Test Tariff From Excel" + i,
                        StartDate = todayDate.AddMonths(i),
                        ExpirationDate = todayDate.AddYears(1),
                        CurrencyId = currencyIds[random.Next(currencyIds.Count)],
                        LastVersion = 50,
                        TypeCode = "AFC",
                        ConcurrencyGUID = Guid.NewGuid().ToString(),
                        SearchFields = "Test Tariff From Excel " + i + "," + airline.EnglishName,
                    };

                    TariffVersion draftVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 1,
                        IsDraft = true,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate,
                        ExpirationDate = todayDate.AddYears(1),
                        Tenant = tenant,
                    };
                    iContext.TariffVersions.Add(draftVersion);

                    TariffVersion activeVersion = new TariffVersion()
                    {
                        TariffId = tariff.Id,
                        Version = 2,
                        IsDraft = false,
                        CreateDate = todayDate,
                        CreatedByUserId = loggedUser.Id,
                        StartDate = todayDate.AddMonths(-1),
                        ExpirationDate = todayDate.AddMonths(2),
                        Tenant = tenant,
                        ApproveDate = todayDate,
                        ApprovedByUserId = loggedUser.Id,
                        ParentVersionNumber = 1,
                    };
                    iContext.TariffVersions.Add(activeVersion);

                    versions.Add(draftVersion);
                    versions.Add(activeVersion);

                    for (int j = 3; j <= 50; j++)
                    {
                        TariffVersion version = new TariffVersion()
                        {
                            TariffId = tariff.Id,
                            Version = j,
                            IsDraft = false,
                            CreateDate = todayDate,
                            CreatedByUserId = loggedUser.Id,
                            StartDate = todayDate.AddMonths(-(j - 1)),
                            ExpirationDate = todayDate.AddMonths(-j),
                            Tenant = tenant,
                            ApproveDate = todayDate,
                            ApprovedByUserId = loggedUser.Id,
                            ParentVersionNumber = j - 1,
                        };
                        iContext.TariffVersions.Add(version);
                        versions.Add(version);
                    }

                    foreach (TariffVersion item in versions)
                    {
                        for (int k = 0; k < 70; k++)
                        {
                            Port fromPort = ports[random.Next(ports.Count)];
                            Port toPort = ports[random.Next(ports.Count)];
                            string[] steps = iTariffSetting.DefaultPriceSteps.Split(',');

                            TariffLine line = new TariffLine()
                            {
                                Id = IdCounter.GetNumber("TariffLine", tenant),
                                Tenant = tenant,
                                TariffId = tariff.Id,
                                Version = item.Version,
                                StartDate = item.StartDate,
                                ExpirationDate = item.ExpirationDate,
                                OriginPortId = fromPort.Id,
                                DestinationPortId = toPort.Id,
                                Index = k,
                                LineUniqueKey = fromPort.Code + "," + toPort.Code,
                                LineUniqueKeyText = fromPort.Code + "," + toPort.Code + k,
                            };

                            // Generate Random Lines
                            for (var s = 1; s <= steps.Length; s++)
                            {

                            }
                            iContext.TariffLines.Add(line);
                        }
                    }

                    TariffVersion lastVersion = versions.Where(d => d.Version == 50).FirstOrDefault();
                    tariff.LastStartDate = lastVersion.StartDate;
                    tariff.LastExpirationDate = lastVersion.ExpirationDate;

                    iContext.Tariffs.Add(tariff);
                    airlines.Remove(airline);

                    iContext.SaveChanges();
                }
            }
        }

        private List<ExcelTariffLines> BuildAirFreightCostExcelLines(IWorksheet sheet, int tenant)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();
            int rowIndex = 0;
            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelTariffLines tariffLine = new ExcelTariffLines();
                tariffLine.Index = rowIndex;

                String notesRowData = row.Cells[sheet.Columns.Count() - 1].Value2.ToString();

                for (int i = 0; i < sheet.Columns.Count() - 1; i++)
                {
                    if (row.Cells[i].HasFormula)
                    {
                        rowData[i] = row.Cells[i].FormulaNumberValue.ToString();
                    }
                    else
                    {
                        rowData[i] = row.Cells[i].Value2.ToString();
                    }
                }

                Port fromPort = this.GetPortDetails(rowData[0], tenant);
                if (fromPort != null)
                {
                    if (fromPort.IsAir)
                    {
                        tariffLine.FromPortId = fromPort.Id;
                        tariffLine.FromPortCode = fromPort.Code;
                        tariffLine.FromPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.FromPortIsNotAir = true;
                    }
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(rowData[0]);
                }

                Port toPort = this.GetPortDetails(rowData[1], tenant);
                if (toPort != null)
                {
                    if (toPort.IsAir)
                    {
                        tariffLine.ToPortId = toPort.Id;
                        tariffLine.ToPortCode = toPort.Code;
                        tariffLine.ToPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.ToPortIsNotAir = true;
                    }
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(rowData[1]);
                }

                if (rowData.Length > 2)
                {
                    if (this.IsNumber(rowData[2]))
                    {
                        tariffLine.MinPrice = Convert.ToDecimal(rowData[2]);
                    }
                    else
                    {
                        tariffLine.MinPriceText = this.TrimTo_20(rowData[2]);
                    }
                }

                if (rowData.Length > 3)
                {
                    if (this.IsNumber(rowData[3]))
                    {
                        tariffLine.Step1Price = Convert.ToDecimal(rowData[3]);
                    }
                    else
                    {
                        tariffLine.Step1PriceText = this.TrimTo_20(rowData[3]);
                    }
                }

                if (rowData.Length > 4)
                {
                    if (this.IsNumber(rowData[4]))
                    {
                        tariffLine.Step2Price = Convert.ToDecimal(rowData[4]);
                    }
                    else
                    {
                        tariffLine.Step2PriceText = this.TrimTo_20(rowData[4]);
                    }
                }

                if (rowData.Length > 5)
                {
                    if (this.IsNumber(rowData[5]))
                    {
                        tariffLine.Step3Price = Convert.ToDecimal(rowData[5]);
                    }
                    else
                    {
                        tariffLine.Step3PriceText = this.TrimTo_20(rowData[5]);
                    }
                }

                if (rowData.Length > 6)
                {
                    if (this.IsNumber(rowData[6]))
                    {
                        tariffLine.Step4Price = Convert.ToDecimal(rowData[6]);
                    }
                    else
                    {
                        tariffLine.Step4PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (rowData.Length > 7)
                {
                    if (this.IsNumber(rowData[7]))
                    {
                        tariffLine.Step5Price = Convert.ToDecimal(rowData[7]);
                    }
                    else
                    {
                        tariffLine.Step5PriceText = this.TrimTo_20(rowData[7]);
                    }
                }

                if (rowData.Length > 8)
                {
                    if (this.IsNumber(rowData[8]))
                    {
                        tariffLine.Step6Price = Convert.ToDecimal(rowData[8]);
                    }
                    else
                    {
                        tariffLine.Step6PriceText = this.TrimTo_20(rowData[8]);
                    }
                }

                if (rowData.Length > 9)
                {
                    if (this.IsNumber(rowData[9]))
                    {
                        tariffLine.Step7Price = Convert.ToDecimal(rowData[9]);
                    }
                    else
                    {
                        tariffLine.Step7PriceText = this.TrimTo_20(rowData[9]);
                    }
                }

                if (rowData.Length > 10)
                {
                    if (this.IsNumber(rowData[10]))
                    {
                        tariffLine.Step8Price = Convert.ToDecimal(rowData[10]);
                    }
                    else
                    {
                        tariffLine.Step8PriceText = this.TrimTo_20(rowData[10]);
                    }
                }

                if (!string.IsNullOrEmpty(notescolumn))
                {
                    tariffLine.Notes = notesRowData;

                    if (notesRowData.Length > 500)
                    {
                        tariffLine.Notes = notesRowData.Substring(0, 500);
                    }
                }

                tariffLine.IsUploaded = true;
                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_AirFreightCost(item);
            }

            return myResult;
        }
        private List<ExcelTariffLines> BuildAirSurchargesCostExcelLines(IWorksheet sheet, int tenant)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();
            int rowIndex = 0;

            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelTariffLines tariffLine = new ExcelTariffLines();
                tariffLine.Index = rowIndex;

                String notesRowData = row.Cells[sheet.Columns.Count() - 1].Value2.ToString();

                for (int i = 0; i < sheet.Columns.Count() - 1; i++)
                {
                    if (row.Cells[i].HasFormula)
                    {
                        rowData[i] = row.Cells[i].FormulaNumberValue.ToString();
                    }
                    else
                    {
                        rowData[i] = row.Cells[i].Value2.ToString();
                    }
                }

                Port fromPort = this.GetPortDetails(rowData[0], tenant);
                if (fromPort != null)
                {
                    if (fromPort.IsAir)
                    {
                        tariffLine.FromPortId = fromPort.Id;
                        tariffLine.FromPortCode = fromPort.Code;
                        tariffLine.FromPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.FromPortIsNotAir = true;
                    }
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(rowData[0]);
                }

                Port toPort = this.GetPortDetails(rowData[1], tenant);
                if (toPort != null)
                {
                    if (toPort.IsAir)
                    {
                        tariffLine.ToPortId = toPort.Id;
                        tariffLine.ToPortCode = toPort.Code;
                        tariffLine.ToPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.ToPortIsNotAir = true;
                    }
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(rowData[1]);
                }

                if (rowData.Length > 2)
                {
                    if (this.IsDateTime(rowData[2]))
                    {
                        tariffLine.StartDate = Convert.ToDateTime(rowData[2]);
                    }
                    else
                    {
                        tariffLine.StartDateText = this.TrimTo_20(rowData[2]);
                    }
                }

                if (rowData.Length > 3)
                {
                    if (this.IsNumber(rowData[3]))
                    {
                        tariffLine.Surcharge1Price = Convert.ToDecimal(rowData[3]);
                    }
                    else
                    {
                        tariffLine.Surcharge1PriceText = this.TrimTo_20(rowData[3]);
                    }
                }

                if (rowData.Length > 4)
                {
                    if (this.IsNumber(rowData[4]))
                    {
                        tariffLine.Surcharge2Price = Convert.ToDecimal(rowData[4]);
                    }
                    else
                    {
                        tariffLine.Surcharge2PriceText = this.TrimTo_20(rowData[4]);
                    }
                }

                if (rowData.Length > 5)
                {
                    if (this.IsNumber(rowData[5]))
                    {
                        tariffLine.Surcharge3Price = Convert.ToDecimal(rowData[5]);
                    }
                    else
                    {
                        tariffLine.Surcharge3PriceText = this.TrimTo_20(rowData[5]);
                    }
                }

                if (rowData.Length > 6)
                {
                    if (this.IsNumber(rowData[6]))
                    {
                        tariffLine.Surcharge4Price = Convert.ToDecimal(rowData[6]);
                    }
                    else
                    {
                        tariffLine.Surcharge4PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (rowData.Length > 7)
                {
                    if (this.IsNumber(rowData[7]))
                    {
                        tariffLine.Surcharge5Price = Convert.ToDecimal(rowData[7]);
                    }
                    else
                    {
                        tariffLine.Surcharge5PriceText = this.TrimTo_20(rowData[7]);
                    }
                }

                if (rowData.Length > 8)
                {
                    if (this.IsNumber(rowData[8]))
                    {
                        tariffLine.Surcharge6Price = Convert.ToDecimal(rowData[8]);
                    }
                    else
                    {
                        tariffLine.Surcharge6PriceText = this.TrimTo_20(rowData[8]);
                    }
                }

                if (rowData.Length > 9)
                {
                    if (this.IsNumber(rowData[9]))
                    {
                        tariffLine.Surcharge7Price = Convert.ToDecimal(rowData[9]);
                    }
                    else
                    {
                        tariffLine.Surcharge7PriceText = this.TrimTo_20(rowData[9]);
                    }
                }

                if (rowData.Length > 10)
                {
                    if (this.IsNumber(rowData[10]))
                    {
                        tariffLine.Surcharge8Price = Convert.ToDecimal(rowData[10]);
                    }
                    else
                    {
                        tariffLine.Surcharge8PriceText = this.TrimTo_20(rowData[10]);
                    }
                }

                if (rowData.Length > 11)
                {
                    if (this.IsNumber(rowData[11]))
                    {
                        tariffLine.Surcharge9Price = Convert.ToDecimal(rowData[11]);
                    }
                    else
                    {
                        tariffLine.Surcharge9PriceText = this.TrimTo_20(rowData[11]);
                    }
                }

                if (rowData.Length > 12)
                {
                    if (this.IsNumber(rowData[12]))
                    {
                        tariffLine.Surcharge10Price = Convert.ToDecimal(rowData[12]);
                    }
                    else
                    {
                        tariffLine.Surcharge10PriceText = this.TrimTo_20(rowData[12]);
                    }
                }

                if (!string.IsNullOrEmpty(notescolumn))
                {
                    tariffLine.Notes = notesRowData;

                    if (notesRowData.Length > 500)
                    {
                        tariffLine.Notes = notesRowData.Substring(0, 500);
                    }
                }

                tariffLine.IsUploaded = true;
                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_AirSurchargesCost(item);
            }

            return myResult;
        }

        private void SetErrors_AirFreightCost(ExcelTariffLines item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.FromPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                }
            }
            else if (string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (item.FromPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Origin Port should be Air";
                    }

                    else
                    {
                        errorText = errorText + ", Origin Port should be Air";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Missing Origin Port";
                    }

                    else
                    {
                        errorText = errorText + ", Missing Origin Port";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.ToPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                }
            }
            else if (string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (item.ToPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Destination Port should be Air";
                    }

                    else
                    {
                        errorText = errorText + ", Destination Port should be Air";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Missing Destination Port";
                    }

                    else
                    {
                        errorText = errorText + ", Missing Destination Port";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.MinPriceText) && item.MinPrice == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Min price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Min price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step1PriceText) && item.Step1Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 1 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 1 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step2PriceText) && item.Step2Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 2 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 2 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step3PriceText) && item.Step3Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 3 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 3 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step4PriceText) && item.Step4Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 4 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 4 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step5PriceText) && item.Step5Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 5 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 5 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step6PriceText) && item.Step6Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 6 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 6 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step7PriceText) && item.Step7Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 7 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 7 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Step8PriceText) && item.Step8Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Step 8 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Step 8 price format is invalid";
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }
        private void SetErrors_AirSurchargesCost(ExcelTariffLines item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.FromPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                }
            }
            else if (string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (item.FromPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Origin Port should be Air";
                    }

                    else
                    {
                        errorText = errorText + ", Origin Port should be Air";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Missing Origin Port";
                    }

                    else
                    {
                        errorText = errorText + ", Missing Origin Port";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + item.ToPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                }
            }
            else if (string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (item.ToPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Destination Port should be Air";
                    }

                    else
                    {
                        errorText = errorText + ", Destination Port should be Air";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Missing Destination Port";
                    }

                    else
                    {
                        errorText = errorText + ", Missing Destination Port";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge1PriceText) && item.Surcharge1Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 1 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 1 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge2PriceText) && item.Surcharge2Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 2 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 2 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge3PriceText) && item.Surcharge3Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 3 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 3 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge4PriceText) && item.Surcharge4Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 4 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 4 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge5PriceText) && item.Surcharge5Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 5 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 5 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge6PriceText) && item.Surcharge6Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 6 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 6 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge7PriceText) && item.Surcharge7Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 7 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 7 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge8PriceText) && item.Surcharge8Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 8 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 8 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge9PriceText) && item.Surcharge9Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 9 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 9 price format is invalid";
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge10PriceText) && item.Surcharge10Price == null)
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Surcharge 10 price format is invalid";
                }

                else
                {
                    errorText = errorText + ", Surcharge 10 price format is invalid";
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }

        private Port GetPortDetails(string code, int tenant)
        {
            Port myPort = null;

            myPort = this.portRepository.GetSinglePortByCode(tenant, code, true);
            if (myPort == null)
            {
                Port portZero = this.portRepository.GetSinglePortByCode(0, code, true);
                if (portZero != null)
                {
                    myPort = this.GetPortCopyToCurrentTenant(portZero, tenant);
                }
            }

            return myPort;
        }
        private Port GetPortCopyToCurrentTenant(Port ZeroPort, int tenant)
        {
            ICommonDataContext objectContext = this.portRepository.context;

            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            newPort = portRepository.GetSinglePortByCodeCountryCode(tenant, ZeroPort.Code, ZeroPort.Country.Code, false);
            Country country = null;

            if (newPort == null)
            {
                country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);

                if (country == null)
                {
                    GlobalZone globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(ZeroPort.Country.GlobalZone.Code, tenant);

                    if (globalzone == null)
                    {
                        GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(ZeroPort.Country.GlobalZoneId, 0);
                        globalzone = new GlobalZone()
                        {
                            Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                            Code = oldZone.Code,
                            EnglishName = oldZone.EnglishName,
                            LocalName = oldZone.LocalName,
                            Notes = oldZone.Notes,
                            SearchFields = oldZone.SearchFields,
                            Tenant = tenant,
                        };

                        globalZoneRepository.Add(globalzone);
                        globalZoneRepository.SubmitChanges();
                    }

                    Country oldCountry = CountryRepository.GetSingleCountry(ZeroPort.CountryId, 0, false);
                    country = new Country()
                    {
                        Id = IdCounter.GetNumber("Country", tenant).ToString(),
                        Tenant = tenant,
                        GlobalZoneId = oldCountry.GlobalZoneId,
                        EC = oldCountry.EC,
                        EnglishName = oldCountry.EnglishName,
                        Code = oldCountry.Code,
                        InActive = oldCountry.InActive,
                        Notes = oldCountry.Notes,
                        LocalName = oldCountry.LocalName,
                        SearchFields = oldCountry.SearchFields,
                    };

                    countryRepository.Add(country);
                    countryRepository.SubmitChanges();
                }

                newPort = new Port()
                {
                    Id = IdCounter.GetNumber("Port", tenant).ToString(),
                    Code = ZeroPort.Code,
                    EnglishName = ZeroPort.EnglishName,
                    LocalName = ZeroPort.LocalName,
                    Tenant = tenant,
                    AddedManually = false,
                    InActive = false,
                    CountryId = country.Id,
                    IsAir = ZeroPort.IsAir,
                    IsInland = ZeroPort.IsInland,
                    IsOcean = ZeroPort.IsOcean,
                    Latitude = ZeroPort.Latitude,
                    Longtitude = ZeroPort.Longtitude,
                    SearchFields = ZeroPort.SearchFields,
                    Notes = ZeroPort.Notes,
                };

                portRepository.Add(newPort);
                portRepository.SubmitChanges();
            }

            if (country == null)
            {
                country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);
            }

            return newPort;
        }
        private string TrimTo_20(string text)
        {
            string trimmedText = text;

            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length > 20)
                {
                    trimmedText = text.Substring(0, 20);
                }
            }

            return trimmedText;
        }
        private bool IsNumber(string text)
        {
            bool isNumber = false;

            if (!string.IsNullOrEmpty(text))
            {
                decimal value;
                if (Decimal.TryParse(text, out value))
                {
                    isNumber = true;
                }
            }

            return isNumber;
        }
        private bool IsDateTime(string text)
        {
            bool isDateTime = false;

            if (!string.IsNullOrEmpty(text))
            {
                DateTime value;
                if (DateTime.TryParse(text, out value))
                {
                    isDateTime = true;
                }
            }

            return isDateTime;
        }

        public HttpResponseMessage GetGenerateTariffs()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                GenerateTariffsArgs args = new GenerateTariffsArgs() { LoggedUserEmail = loggedUserEmail, Tenant = tenant };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(GenerateTariffsArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();

                BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Generate Tariffs",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "WebFreight.Web.Helpers.APIHelpers.GenerateTariffsHelper,WebFreight.Web",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",
                };

                IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });

                return Request.CreateResponse(HttpStatusCode.OK, taskExe);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTariffVersionLines(string tariffId, int version)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ITariffModuleContext iContext = TariffModuleContext.GetContext(authToken.Tenant);
                TariffLineQueryService tariffLineQueryService = new TariffLineQueryService(iContext);
                List<TariffLinePM> tariffLinePMs = tariffLineQueryService.GetTariffLinesByTariffAndVersion(tariffId, version, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, tariffLinePMs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [ActionName("PostUpdateSurcharge")]
        public HttpResponseMessage PostUpdateSurcharge(UpdateSurchargeArgs args)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ITariffModuleContext tariffContext = TariffModuleContext.GetContext(authToken.Tenant);
                TariffQueryService tariffQueryService = new TariffQueryService(tariffContext);
                TariffPM tariff = tariffQueryService.GetSingle(args.TariffId, true, false);
                TariffUpdateService tariffUpdateService = new TariffUpdateService(tariffContext, new Dictionary<string, IContext>(), authToken.Tenant);

                if (tariff != null)
                {
                    TariffVersionPM iDraftVersion = tariff.TariffVersions.Where(d => d.IsDraft).FirstOrDefault();

                    if (iDraftVersion != null)
                    {
                        List<FromToClass> routs = this.ComputeRoutsList(args.From, args.To, authToken.Tenant);
                        bool isValid = this.ValidateStartDate(tariff, iDraftVersion, routs, args.StartDate);

                        if (isValid)
                        {
                            foreach (FromToClass rout in routs)
                            {
                                TariffLinePM myLine = iDraftVersion.TariffLines.Where(d => d.OriginPortId == rout.FromCode && d.DestinationPortId == rout.ToCode).FirstOrDefault();
                                if (myLine != null)
                                {
                                    myLine.ChangeSetOp = ChangeSetOperation.Update;
                                    myLine.StartDate = args.StartDate;

                                    foreach (string charge in args.Surcharge)
                                    {
                                        string[] charge_array = charge.Split(',');
                                        decimal value = Convert.ToDecimal(charge_array[1]);

                                        PropertyInfo valuePropInfo = myLine.GetType().GetProperty("Surcharge" + charge_array[2] + "Price");
                                        valuePropInfo.SetValue(myLine, value, null);
                                    }
                                }

                                else
                                {
                                    TariffLinePM tariffLine = new TariffLinePM()
                                    {
                                        ChangeSetOp = ChangeSetOperation.Insert,
                                        Version = iDraftVersion.Version,
                                        Tenant = authToken.Tenant,
                                        DestinationPortId = rout.ToCode,
                                        OriginPortId = rout.FromCode,
                                        StartDate = args.StartDate,
                                        TariffId = args.TariffId,
                                    };

                                    foreach (string charge in args.Surcharge)
                                    {
                                        string[] charge_array = charge.Split(',');

                                        decimal value1 = Convert.ToDecimal(charge_array[1]);
                                        PropertyInfo valuePropInfo1 = tariffLine.GetType().GetProperty("Surcharge" + charge_array[2] + "Price");
                                        valuePropInfo1.SetValue(tariffLine, value1, null);
                                    }

                                    iDraftVersion.TariffLines.Add(tariffLine);
                                }
                            }

                            tariff.IsSurchargeUpdate = true;
                            tariff.ChangeSetOp = ChangeSetOperation.Update;
                            iDraftVersion.ChangeSetOp = ChangeSetOperation.Update;
                            tariffUpdateService.Update(tariff, true);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private bool ValidateStartDate(TariffPM tariff, TariffVersionPM iDraftVersion, List<FromToClass> routs, DateTime startDate)
        {
            bool isValid = true;

            TariffVersionPM iPreviousVersion = tariff.ActiveVersions.OrderByDescending(o => o.CreateDate).FirstOrDefault();
            if (iPreviousVersion != null)
            {
                List<TariffLinePM> draftLines = iDraftVersion.TariffLines.Where(d => (routs.Select(s => s.FromCode).Contains(d.OriginPortId) && routs.Select(s => s.ToCode).Contains(d.DestinationPortId))).ToList();
                foreach (TariffLinePM linePM in draftLines)
                {
                    if (linePM.StartDate != null)
                    {
                        var iPreviousLine = iPreviousVersion.TariffLines.Where(d => d.OriginPortId == linePM.OriginPortId && d.DestinationPortId == linePM.DestinationPortId).FirstOrDefault();
                        if (iPreviousLine != null)
                        {
                            if (startDate > iPreviousLine.StartDate)
                            {
                                isValid = false;
                                throw new ApplicationException("New start date can't be before the current tariff start date");
                            }
                        }
                    }
                }
            }

            return isValid;
        }
        private List<FromToClass> ComputeRoutsList(List<string> fromList, List<string> toList, int tenant)
        {
            List<FromToClass> myResult = new List<FromToClass>();

            foreach (string item_from in fromList)
            {
                string[] from = item_from.Split(',');

                if (from[0] == "Port")
                {
                    foreach (string item_to in toList)
                    {
                        string[] to = item_to.Split(',');

                        if (to[0] == "Port")
                        {
                            FromToClass routItem = new FromToClass()
                            {
                                FromCode = from[1],
                                ToCode = to[1],
                            };

                            myResult.Add(routItem);
                        }
                    }
                }

                else if (from[0] == "Area")
                {
                    AirlineAreasPortRepository airlineAreasPortRepository = new AirlineAreasPortRepository(tenant);
                    List<AirlineAreasPort> areasPorts = airlineAreasPortRepository.GetAirlineAreasPortByAreaId(from[1], tenant);
                    if (areasPorts != null && areasPorts.Count > 0)
                    {

                    }
                }
            }

            return myResult;
        }
    }

    public class TariffFilterParameter
    {
        public int Tenant { get; set; }
        public string FileData { get; set; }
        public string PriceSteps { get; set; }
        public string TariffId { get; set; }
        public int Version { get; set; }
        public string TariffType { get; set; }
        public string FileName { get; set; }
    }
    public class ExcelTariffLines
    {
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string ToPortId { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? Step1Price { get; set; }
        public decimal? Step2Price { get; set; }
        public decimal? Step3Price { get; set; }
        public decimal? Step4Price { get; set; }
        public decimal? Step5Price { get; set; }
        public decimal? Step6Price { get; set; }
        public decimal? Step7Price { get; set; }
        public decimal? Step8Price { get; set; }

        public bool FromPortIsNotAir { get; set; }
        public bool ToPortIsNotAir { get; set; }

        public string FromPortText { get; set; }
        public string ToPortText { get; set; }
        public string MinPriceText { get; set; }
        public string Step1PriceText { get; set; }
        public string Step2PriceText { get; set; }
        public string Step3PriceText { get; set; }
        public string Step4PriceText { get; set; }
        public string Step5PriceText { get; set; }
        public string Step6PriceText { get; set; }
        public string Step7PriceText { get; set; }
        public string Step8PriceText { get; set; }

        public bool HasErrors { get; set; }
        public string ErrorText { get; set; }

        public decimal? Surcharge1Price { get; set; }
        public decimal? Surcharge2Price { get; set; }
        public decimal? Surcharge3Price { get; set; }
        public decimal? Surcharge4Price { get; set; }
        public decimal? Surcharge5Price { get; set; }
        public decimal? Surcharge6Price { get; set; }
        public decimal? Surcharge7Price { get; set; }
        public decimal? Surcharge8Price { get; set; }
        public decimal? Surcharge9Price { get; set; }
        public decimal? Surcharge10Price { get; set; }

        public string Surcharge1PriceText { get; set; }
        public string Surcharge2PriceText { get; set; }
        public string Surcharge3PriceText { get; set; }
        public string Surcharge4PriceText { get; set; }
        public string Surcharge5PriceText { get; set; }
        public string Surcharge6PriceText { get; set; }
        public string Surcharge7PriceText { get; set; }
        public string Surcharge8PriceText { get; set; }
        public string Surcharge9PriceText { get; set; }
        public string Surcharge10PriceText { get; set; }
        public bool IsUploaded { get; set; }
        public int Index { get; set; }

        public string Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateText { get; set; }
    }
    public class UpdateSurchargeArgs
    {
        public string TariffId { get; set; }
        public int VersionNumber { get; set; }
        public List<string> From { get; set; }
        public List<string> To { get; set; }
        public List<string> Surcharge { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class FromToClass
    {
        public string FromCode { get; set; }
        public string ToCode { get; set; }
    }
}