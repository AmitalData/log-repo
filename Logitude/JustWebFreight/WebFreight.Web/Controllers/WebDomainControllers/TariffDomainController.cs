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
using Stimulsoft.Report.Export;
using Logitude.BL.Helpers;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;
using Logitude.TariffModule.Data.EntityListQueryServices;
using Logitude.TariffModule.BL.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

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

        [ActionName("PostAvailableAirlineFreightTariffs")]
        public HttpResponseMessage PostAvailableAirlineFreightTariffs(TariffSearchArgs args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);
                
                DateTime? betweenDate = DateHelper.GetDate(args.Date);
                if (betweenDate == null)
                {
                    betweenDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }
                args.BetweenDate = betweenDate;

                PriceCheckManager priceCheckManager = new PriceCheckManager(args, tenant);
                List<TariffSearchSummary> myResult = priceCheckManager.GetSummary();
                
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCheckDatesValidty(string FromPort, string ToPort, string ToDate, string TariffId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                DateTime? ToDateOBJ = DateHelper.GetDate(ToDate);
                if (ToDateOBJ == null)
                {
                    ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }



                TariffQueryService tariffQueryService = new TariffQueryService(tenant);

                bool myResult = tariffQueryService.CheckDatesValidity(FromPort, ToPort, ToDateOBJ, TariffId);


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
                                                DefaultWarningPercentage = d.DefaultWarningPercentage,
                                                AirDefaultStepsId = d.AirDefaultStepsId, 
                                                LCLDefaultStepsId = d.LCLDefaultStepsId,
                                                ContainerDefaults = d.ContainerDefaults,
                                                DefaultCurrencyId = d.DefaultCurrencyId,
                                            }).FirstOrDefault();

                IInfrastructureContext iInfrastructureContext = InfrastructureContext.GetContext(entityPM.Tenant);
                PriceStep airPriceSteps = (from d in iInfrastructureContext.PriceSteps where d.Tenant == entityPM.Tenant && d.Id == entityPM.AirDefaultStepsId select d).FirstOrDefault();
                PriceStep lclPriceSteps = (from d in iInfrastructureContext.PriceSteps where d.Tenant == entityPM.Tenant && d.Id == entityPM.LCLDefaultStepsId select d).FirstOrDefault();
                entityPM.AirDefaultSteps = airPriceSteps != null ? airPriceSteps.Steps : null;
                entityPM.LCLDefaultSteps = lclPriceSteps != null ? lclPriceSteps.Steps : null;
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

                        if (tariff.TypeCode == "AFC" || tariff.TypeCode == "OLC")
                        {
                            data = this.ExportAirFreightCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }
                        
                        else if(tariff.TypeCode == "OFC")
                        {
                            data = this.ExportOceanFCLFreightCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }

                        else if (tariff.TypeCode == "ASC" || tariff.TypeCode == "OSC")
                        {
                            data = this.ExportAirSurchargesCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }
                        
                        else if(tariff.TypeCode == "OFS")
                        {
                            data = this.ExportOceanFCLSurchargesCostLinesToExcel(tariff, tariffVersion.TariffLines, tenant, type);
                        }                  

                        fileName = "Tariff-" + tariff.TariffNumber + "-" + String.Format("{0:dd-MM-yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));

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

                table.Columns.Add("Transit Time");
                table.Columns.Add("Notes");
            }
            else
            {
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("Min Price");
                table.Columns.Add(tariff.PriceSteps + " KG");
                table.Columns.Add("Transit Time");
                table.Columns.Add("Notes");
            }

            string range = "A1:F1";
            if (steps.Length == 2)
            {
                range = "A1:G1";
            }

            else if (steps.Length == 3)
            {
                range = "A1:H1";
            }

            else if (steps.Length == 4)
            {
                range = "A1:I1";
            }

            else if (steps.Length == 5)
            {
                range = "A1:J1";
            }

            else if (steps.Length == 6)
            {
                range = "A1:K1";
            }

            else if (steps.Length == 7)
            {
                range = "A1:L1";
            }

            else if (steps.Length == 8)
            {
                range = "A1:M1";
            }

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        if(tariff.TypeCode == "AFC")
                        {
                            row[0] = item.OriginPortCode ?? null;
                            row[1] = item.DestinationPortCode ?? null;
                        } else
                        {
                            row[0] = item.OriginPortCombinedCode ?? null;
                            row[1] = item.DestinationPortCombinedCode ?? null;
                        }
                        
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

                        row[steps.Length + 3] = item.TransitTime ?? null;

                        row[steps.Length + 4] = item.Notes ?? null;

                        table.Rows.Add(row);
                    }
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet1.Columns[steps.Length + 3].ColumnWidth = 14;
            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.SaveAs(memory);
            return memory.ToArray();
        }
        private byte[] ExportAirSurchargesCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            tariffLines = tariffLines.Where(d => !d.IsFromAllOtherPorts && !d.IsToAllOtherPorts).ToList();
            tariffLines = tariffLines.OrderBy(P => P.Index).ToList();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            DataTable table = new DataTable();

            #region header
            table.Columns.Add("From");
            table.Columns.Add("To");
            table.Columns.Add("Currency");
            table.Columns.Add("Start Date", typeof(DateTime));

            int count = 0;
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
            MeasurementRepository measurementRepository = new MeasurementRepository(tenant);
            ChargesType chargesType = null;
            Measurement measurement = null;

            bool isCharge1Fixed = false;
            bool isCharge2Fixed = false;
            bool isCharge3Fixed = false;
            bool isCharge4Fixed = false;
            bool isCharge5Fixed = false;
            bool isCharge6Fixed = false;
            bool isCharge7Fixed = false;
            bool isCharge8Fixed = false;
            bool isCharge9Fixed = false;
            bool isCharge10Fixed = false;

            if (!string.IsNullOrEmpty(tariff.Surcharge1Id))
            {
                count = 1;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge1Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge1UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge1Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge2Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge2Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge2UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge2Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge3Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge3Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge3UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge3Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge4Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge4Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge4UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge4Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge5Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge5Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge5UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge5Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge6Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge6Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge6UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge6Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge7Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge7Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge7UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge7Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge8Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge8Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge8UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge8Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge9Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge9Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge9UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge9Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.Surcharge10Id))
            {
                count++;
                chargesType = chargesTypeRepository.GetSingleChargesType(tariff.Surcharge10Id, tenant);
                measurement = measurementRepository.GetSingleMeasurement(tariff.Surcharge10UOM, tenant);

                if (chargesType != null && measurement != null)
                {
                    if (measurement.Code != "FIXD")
                    {
                        count++;
                        table.Columns.Add("Min " + chargesType.Code);
                    }

                    else
                    {
                        isCharge10Fixed = true;
                    }

                    table.Columns.Add(chargesType.Code);
                }
            }

            table.Columns.Add("Notes");
            #endregion

            #region range
            string range = "A1:E1";
            if (count == 1)
            {
                range = "A1:F1";
            }
            else if (count == 2)
            {
                range = "A1:G1";
            }
            else if (count == 3)
            {
                range = "A1:H1";
            }
            else if (count == 4)
            {
                range = "A1:I1";
            }
            else if (count == 5)
            {
                range = "A1:J1";
            }
            else if (count == 6)
            {
                range = "A1:K1";
            }
            else if (count == 7)
            {
                range = "A1:L1";
            }
            else if (count == 8)
            {
                range = "A1:M1";
            }
            else if (count == 9)
            {
                range = "A1:N1";
            }
            else if (count == 10)
            {
                range = "A1:O1";
            }
            else if (count == 11)
            {
                range = "A1:P1";
            }
            else if (count == 12)
            {
                range = "A1:Q1";
            }
            else if (count == 13)
            {
                range = "A1:R1";
            }
            else if (count == 14)
            {
                range = "A1:S1";
            }
            else if (count == 15)
            {
                range = "A1:T1";
            }
            else if (count == 16)
            {
                range = "A1:U1";
            }
            else if (count == 17)
            {
                range = "A1:V1";
            }
            else if (count == 18)
            {
                range = "A1:W1";
            }
            else if (count == 19)
            {
                range = "A1:X1";
            }
            else if (count == 20)
            {
                range = "A1:Y1";
            }
            #endregion

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        if(tariff.TypeCode == "ASC")
                        {
                            row[0] = item.OriginPortCode ?? null;
                            row[1] = item.DestinationPortCode ?? null;
                        } else
                        {
                            row[0] = item.OriginPortCombinedCode ?? null;
                            row[1] = item.DestinationPortCombinedCode ?? null;
                        }
                        row[2] = item.CurrencyCode ?? null;
                        row[3] = item.StartDate ?? null;

                        int rowIndex = 4;

                        if (!isCharge1Fixed)
                        {
                            if (item.Surcharge1MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge1MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge1Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge1Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge2Fixed)
                        {
                            if (item.Surcharge2MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge2MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge2Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge2Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge3Fixed)
                        {
                            if (item.Surcharge3MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge3MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge3Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge3Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge4Fixed)
                        {
                            if (item.Surcharge4MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge4MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge4Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge4Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge5Fixed)
                        {
                            if (item.Surcharge5MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge5MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge5Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge5Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge6Fixed)
                        {
                            if (item.Surcharge6MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge6MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge6Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge6Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge7Fixed)
                        {
                            if (item.Surcharge7MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge7MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge7Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge7Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge8Fixed)
                        {
                            if (item.Surcharge8MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge8MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge8Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge8Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge9Fixed)
                        {
                            if (item.Surcharge9MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge9MinPrice ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge9Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge9Price ?? null;
                        }
                        rowIndex++;

                        if (!isCharge10Fixed)
                        {
                            if (item.Surcharge10MinPrice.HasValue)
                            {
                                row[rowIndex++] = item.Surcharge10Price ?? null;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }

                        if (item.Surcharge10Price.HasValue)
                        {
                            row[rowIndex] = item.Surcharge10Price ?? null;
                        }
                        rowIndex++;

                        row[count + 4] = item.Notes ?? null;

                        table.Rows.Add(row);
                    }

                    string dateRange = "C2:C" + (tariffLines.Count + 1);
                    sheet1.Range[dateRange].ColumnWidth = 14;
                    sheet1.Range[dateRange].NumberFormat = "dd/MM/yyyy";
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.SaveAs(memory);
            return memory.ToArray();
        }
        private byte[] ExportOceanFCLFreightCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
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

            int count = 0;
            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
            PackageType packageType = null;

            if (!string.IsNullOrEmpty(tariff.ContainerType1Id))
            {
                count = 1;
                packageType = packageTypeRepository.GetSinglePackageType(tariff.ContainerType1Id, tenant);

                if (packageType != null)
                {
                    table.Columns.Add(packageType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType2Id))
            {
                count++;
                packageType = packageTypeRepository.GetSinglePackageType(tariff.ContainerType2Id, tenant);

                if (packageType != null)
                {
                    table.Columns.Add(packageType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType3Id))
            {
                count++;
                packageType = packageTypeRepository.GetSinglePackageType(tariff.ContainerType3Id, tenant);

                if (packageType != null)
                {
                    table.Columns.Add(packageType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType4Id))
            {
                count++;
                packageType = packageTypeRepository.GetSinglePackageType(tariff.ContainerType4Id, tenant);

                if (packageType != null)
                {
                    table.Columns.Add(packageType.Code);
                }
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType5Id))
            {
                count++;
                packageType = packageTypeRepository.GetSinglePackageType(tariff.ContainerType5Id, tenant);

                if (packageType != null)
                {
                    table.Columns.Add(packageType.Code);
                }
            }

            table.Columns.Add("Transit Time");
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
            #endregion

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.OriginPortCombinedCode ?? null;
                        row[1] = item.DestinationPortCombinedCode ?? null;

                        int rowIndex = 2;
                        
                        if (item.Surcharge1Price.HasValue)
                        {
                            row[rowIndex++] = item.Surcharge1Price ?? null;
                        }
                        
                        if (item.Surcharge2Price.HasValue)
                        {
                            row[rowIndex++] = item.Surcharge2Price ?? null;
                        }

                        if (item.Surcharge3Price.HasValue)
                        {
                            row[rowIndex++] = item.Surcharge3Price ?? null;
                        }
                        
                        if (item.Surcharge4Price.HasValue)
                        {
                            row[rowIndex++] = item.Surcharge4Price ?? null;
                        }
                        
                        if (item.Surcharge5Price.HasValue)
                        {
                            row[rowIndex++] = item.Surcharge5Price ?? null;
                        }

                        row[count + 2] = item.TransitTime ?? null;
                        row[count + 3] = item.Notes ?? null;

                        table.Rows.Add(row);
                    }
                }
            }

            sheet1.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range[range].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet1.Columns[count + 2].ColumnWidth = 14;
            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.SaveAs(memory);
            return memory.ToArray();
        }
        private byte[] ExportOceanFCLSurchargesCostLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant, string type)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            tariffLines = tariffLines.Where(d => !d.IsFromAllOtherPorts && !d.IsToAllOtherPorts).ToList();
            tariffLines = tariffLines.OrderBy(P => P.Index).ToList();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet1 = workbook.Worksheets[0];

            DataTable table = new DataTable();

            #region header
            table.Columns.Add("From");
            table.Columns.Add("To");
            table.Columns.Add("Currency");
            table.Columns.Add("Start Date", typeof(DateTime));

            List<ChargeContainerClass> chargeContainers = this.BuildChargeContainerClass(tariff, tenant);

            bool isContainer1Exists = false;
            bool isContainer2Exists = false;
            bool isContainer3Exists = false;
            bool isContainer4Exists = false;
            bool isContainer5Exists = false;

            if (!string.IsNullOrEmpty(tariff.ContainerType1Id))
            {
                isContainer1Exists = true;
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType2Id))
            {
                isContainer2Exists = true;
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType3Id))
            {
                isContainer3Exists = true;
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType4Id))
            {
                isContainer4Exists = true;
            }

            if (!string.IsNullOrEmpty(tariff.ContainerType5Id))
            {
                isContainer5Exists = true;
            }

            int count = chargeContainers.Count();
            foreach(ChargeContainerClass item in chargeContainers)
            {
                table.Columns.Add(item.ChargeCode + "(" + item.ContainerCode + ")");
            }

            table.Columns.Add("Notes");
            #endregion

            #region range
            string range = "A1:E1";
            if (count == 1)
            {
                range = "A1:F1";
            }
            else if (count == 2)
            {
                range = "A1:G1";
            }
            else if (count == 3)
            {
                range = "A1:H1";
            }
            else if (count == 4)
            {
                range = "A1:I1";
            }
            else if (count == 5)
            {
                range = "A1:J1";
            }
            else if (count == 6)
            {
                range = "A1:K1";
            }
            else if (count == 7)
            {
                range = "A1:L1";
            }
            else if (count == 8)
            {
                range = "A1:M1";
            }
            else if (count == 9)
            {
                range = "A1:N1";
            }
            else if (count == 10)
            {
                range = "A1:O1";
            }
            else if (count == 11)
            {
                range = "A1:P1";
            }
            else if (count == 12)
            {
                range = "A1:Q1";
            }
            else if (count == 13)
            {
                range = "A1:R1";
            }
            else if (count == 14)
            {
                range = "A1:S1";
            }
            else if (count == 15)
            {
                range = "A1:T1";
            }
            else if (count == 16)
            {
                range = "A1:U1";
            }
            else if (count == 17)
            {
                range = "A1:V1";
            }
            else if (count == 18)
            {
                range = "A1:W1";
            }
            else if (count == 19)
            {
                range = "A1:X1";
            }
            else if (count == 20)
            {
                range = "A1:Y1";
            }
            #endregion

            if (type == "Data")
            {
                if (tariffLines != null && tariffLines.Count > 0)
                {
                    foreach (var item in tariffLines)
                    {
                        DataRow row = table.NewRow();
                        row[0] = item.OriginPortCombinedCode ?? null;
                        row[1] = item.DestinationPortCombinedCode ?? null;
                        row[2] = item.CurrencyCode ?? null;
                        row[3] = item.StartDate ?? null;

                        int rowIndex = 4;

                        foreach(TariffLinesContainersPricePM containersPrice in item.ContainersPrices)
                        {
                            if (isContainer1Exists)
                            {
                                if (containersPrice.Price1.HasValue)
                                {
                                    row[rowIndex++] = containersPrice.Price1 ?? null;
                                }
                                else
                                {
                                    rowIndex++;
                                }
                            }

                            if (isContainer2Exists)
                            {
                                if (containersPrice.Price2.HasValue)
                                {
                                    row[rowIndex++] = containersPrice.Price2 ?? null;
                                }
                                else
                                {
                                    rowIndex++;
                                }
                            }

                            if (isContainer3Exists)
                            {
                                if (containersPrice.Price3.HasValue)
                                {
                                    row[rowIndex++] = containersPrice.Price3 ?? null;
                                }
                                else
                                {
                                    rowIndex++;
                                }
                            }

                            if (isContainer4Exists)
                            {
                                if (containersPrice.Price4.HasValue)
                                {
                                    row[rowIndex++] = containersPrice.Price4 ?? null;
                                }
                                else
                                {
                                    rowIndex++;
                                }
                            }

                            if (isContainer5Exists)
                            {
                                if (containersPrice.Price5.HasValue)
                                {
                                    row[rowIndex++] = containersPrice.Price5 ?? null;
                                }
                                else
                                {
                                    rowIndex++;
                                }
                            }
                        }

                        row[count + 4] = item.Notes ?? null;

                        table.Rows.Add(row);
                    }

                    string dateRange = "C2:C" + (tariffLines.Count + 1);
                    sheet1.Range[dateRange].ColumnWidth = 14;
                    sheet1.Range[dateRange].NumberFormat = "dd/MM/yyyy";
                }
            }

            sheet1.Range["A1"].EntireRow.CellStyle.Font.Color = ExcelKnownColors.White;
            sheet1.Range["A1"].EntireRow.CellStyle.Color = System.Drawing.Color.Gray;
            sheet1.Range["A1"].EntireRow.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.SaveAs(memory);
            return memory.ToArray();
        }
        private List<ChargeContainerClass> BuildChargeContainerClass(TariffPM tariff, int tenant)
        {
            List<ChargeContainerClass> myResult = new List<ChargeContainerClass>();

            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);

            List<PropertyInfo> tariffProperties = tariff.GetType().GetProperties().ToList();

            List<string> containersCodes = new List<string>();
            List<string> chargesCodes = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                object containerId = tariffProperties.FirstOrDefault(f => f.Name == ("ContainerType" + i + "Id")).GetValue(tariff);

                if (containerId != null)
                {
                    PackageType packageType = packageTypeRepository.GetSinglePackageType(containerId.ToString(), tenant);
                    if(packageType != null)
                    {
                        containersCodes.Add(packageType.Code);
                    }
                }
            }

            for (int i = 1; i <= 10; i++)
            {
                object chargeId = tariffProperties.FirstOrDefault(f => f.Name == ("Surcharge" + i + "Id")).GetValue(tariff);

                if (chargeId != null)
                {
                    ChargesType chargesType = chargesTypeRepository.GetSingleChargesType(chargeId.ToString(), tenant);
                    if (chargesType != null)
                    {
                        chargesCodes.Add(chargesType.Code);
                    }
                }
            }

            for (int index_c = 0; index_c < chargesCodes.Count; index_c++)
            {
                for(int index_P = 0; index_P < containersCodes.Count; index_P++)
                {
                    myResult.Add(new ChargeContainerClass()
                    {
                        ChargeIndex = index_c + 1,
                        ContainerIndex = index_P + 1,
                        ChargeCode = chargesCodes[index_c],
                        ContainerCode = containersCodes[index_P],
                        //ChargeId = ,
                        //ContainerId = ,
                    });
                }
            }

            return myResult;
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

        private string TariffType = ""; 
        [ActionName("PostUploadExcelFile")]
        public HttpResponseMessage PostUploadExcelFile(TariffFilterParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                filter.Tenant = authToken.Tenant;

                UploadTariffHelper uploadTariffHelper = new UploadTariffHelper(filter);
                uploadTariffHelper.Upload();

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
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

                //byte[] fileData = Convert.FromBase64String(filter.FileData);

                //TariffsExcelGeneratorArgs args = new TariffsExcelGeneratorArgs()
                //{
                //    LoggedUserEmail = loggedUserEmail,
                //    Tenant = authToken.Tenant,
                //};


                //this.UploadExcelFileToStorage(fileData, args, authToken.Tenant);


                GenerateTariffsArgs args = new GenerateTariffsArgs() { LoggedUserEmail = loggedUserEmail, Tenant = authToken.Tenant };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(GenerateTariffsArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();


                //var stringwriter = new System.IO.StringWriter();
                //var serializer = new XmlSerializer(typeof(TariffsExcelGeneratorArgs));
                //serializer.Serialize(stringwriter, args);
                //string xmlParameters = stringwriter.ToString();

                BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Generate Tariffs From Excel",
                    Tenant = authToken.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "WebFreight.Web.Helpers.APIHelpers.TariffGeneratorFromExcel,WebFreight.Web",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",
                };

                IInfrastructureContext MyContext = InfrastructureContext.GetContext(authToken.Tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", authToken.Tenant.ToString() }
                }, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, taskExe);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void UploadExcelFileToStorage(byte[] fileData, TariffsExcelGeneratorArgs args, int tenant)
        {
            string extension = "";
            Document document = null;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            string fileName = "Air Tariffs Excel File";

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = "others",
                Extension = "xlsx",
                Tenant = tenant,
            };

            byte[] result = storageservice.Read(fileInfo);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(fileData);
            ExcelEngine excelEngine = new ExcelEngine();

            if (memoryStream != null)
            {
                byte[] ByteData = memoryStream.ToArray();
                DocumentRepository documentRepository = new DocumentRepository(tenant);
                document = new Document()
                {
                    FileName = fileName,
                    CreateDate = DateTime.Now,
                    Extension = extension,
                    FileSize = ByteData.Length,
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "others",
                };

                documentRepository.Add(document);
                documentRepository.SubmitChanges();
                args.DocumentId = document.Id;
                fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = "others",
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = document.FileSize,

                };
                storageservice.Write(ByteData, fileInfo);
            }
        }
        public List<ExcelTariffLines> BuildOceanAirFreightCostExcelLines(IWorksheet sheet, int tenant, TariffFilterParameter filter = null)
        {
            List<ExcelTariffLines> myResult = new List<ExcelTariffLines>();
            int rowIndex = 0;
            string notescolumn = sheet.Columns[sheet.Columns.Count() - 1].DisplayText;

            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count() - 1];
                ExcelTariffLines tariffLine = new ExcelTariffLines();
                tariffLine.Index = rowIndex;
                var rowDataLength = rowData.Length;
                var StepLength = rowData.Length;
                var tariffType = "";
                if (filter != null && !string.IsNullOrEmpty(filter.PriceSteps))
                {
                    tariffType = filter.TariffType;
                    StepLength = filter.PriceSteps.Split(',').Length + 3;
                }

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

                /*From Port*/
                string fromPortCode = rowData[0];
                if(!Regex.IsMatch(fromPortCode, @"^[a-zA-Z0-9]+$"))
                {
                    fromPortCode = Regex.Replace(fromPortCode, @"[^a-zA-Z0-9]+", "");
                }

                Port fromPort = this.GetPortDetails(fromPortCode, tenant);
                if (fromPort != null)
                {
                    if ((fromPort.IsAir && tariffType == "AFC") || (fromPort.IsOcean && tariffType == "OLC"))
                    {
                        tariffLine.FromPortId = fromPort.Id;
                        tariffLine.FromPortCode = fromPort.Code;
                        tariffLine.FromPortCombinedCode = fromPort.CombinedCode;
                        tariffLine.FromPortName = fromPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.FromPortIsNotAir = true;
                        tariffLine.FromPortText = fromPortCode;
                    }
                }
                else
                {
                    tariffLine.FromPortText = this.TrimTo_20(fromPortCode);
                }

                /*To Port*/
                string toPortCode = rowData[1];
                if (!Regex.IsMatch(toPortCode, @"^[a-zA-Z0-9]+$"))
                {
                    toPortCode = Regex.Replace(toPortCode, @"[^a-zA-Z0-9]+", "");
                }

                Port toPort = this.GetPortDetails(toPortCode, tenant);
                if (toPort != null)
                {
                    if ((toPort.IsAir && tariffType == "AFC") || (toPort.IsOcean && tariffType == "OLC"))
                    {
                        tariffLine.ToPortId = toPort.Id;
                        tariffLine.ToPortCode = toPort.Code;
                        tariffLine.ToPortCombinedCode = toPort.CombinedCode;
                        tariffLine.ToPortName = toPort.EnglishName;
                    }

                    else
                    {
                        tariffLine.ToPortIsNotAir = true;
                        tariffLine.ToPortText = toPortCode;
                    }
                }
                else
                {
                    tariffLine.ToPortText = this.TrimTo_20(toPortCode);
                }

                if (StepLength > 2 && rowDataLength > 2)
                {
                    if (this.IsNumber(rowData[2]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[2]);
                        if (myNumber >= 0)
                        {
                            tariffLine.MinPrice = myNumber;
                        }

                        else
                        {
                            tariffLine.IsMinPriceMinus = true;
                            tariffLine.MinPriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.MinPriceText = this.TrimTo_20(rowData[2]);
                    }
                }

                if (StepLength > 3 && rowDataLength > 3)
                {
                    if (this.IsNumber(rowData[3]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[3]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step1Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep1PriceMinus = true;
                            tariffLine.Step1PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step1PriceText = this.TrimTo_20(rowData[3]);
                    }
                }

                if (StepLength > 4 && rowDataLength > 4)
                {
                    if (this.IsNumber(rowData[4]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[4]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step2Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep2PriceMinus = true;
                            tariffLine.Step2PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step2PriceText = this.TrimTo_20(rowData[4]);
                    }
                }

                if (StepLength > 5 && rowDataLength > 5)
                {
                    if (this.IsNumber(rowData[5]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[5]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step3Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep3PriceMinus = true;
                            tariffLine.Step3PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step3PriceText = this.TrimTo_20(rowData[5]);
                    }
                }

                if (StepLength > 6 && rowDataLength > 6)
                {
                    if (this.IsNumber(rowData[6]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[6]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step4Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep4PriceMinus = true;
                            tariffLine.Step4PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step4PriceText = this.TrimTo_20(rowData[6]);
                    }
                }

                if (StepLength > 7 && rowDataLength > 7)
                {
                    if (this.IsNumber(rowData[7]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[7]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step5Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep5PriceMinus = true;
                            tariffLine.Step5PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step5PriceText = this.TrimTo_20(rowData[7]);
                    }
                }

                if (StepLength > 8 && rowDataLength > 8)
                {
                    if (this.IsNumber(rowData[8]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[8]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step6Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep6PriceMinus = true;
                            tariffLine.Step6PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step6PriceText = this.TrimTo_20(rowData[8]);
                    }
                }

                if (StepLength > 9 && rowDataLength > 9)
                {
                    if (this.IsNumber(rowData[9]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[9]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step7Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep7PriceMinus = true;
                            tariffLine.Step7PriceText = String.Format("{0:0.000}", myNumber);
                        }
                    }
                    else
                    {
                        tariffLine.Step7PriceText = this.TrimTo_20(rowData[9]);
                    }
                }

                if (StepLength > 10 && rowDataLength > 10)
                {
                    if (this.IsNumber(rowData[10]))
                    {
                        decimal myNumber = Convert.ToDecimal(rowData[10]);

                        if (myNumber >= 0)
                        {
                            tariffLine.Step8Price = myNumber;
                        }
                        else
                        {
                            tariffLine.IsStep8PriceMinus = true;
                            tariffLine.Step8PriceText = String.Format("{0:0.000}", myNumber);
                        }
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

                myResult.Add(tariffLine);
                rowIndex++;
            }

            foreach (ExcelTariffLines item in myResult)
            {
                this.SetErrors_AirFreightCost(item);
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

                if (item.FromPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.FromPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Origin Port";
                }

                else
                {
                    errorText = errorText + ", Missing Origin Port";
                }
            }

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (item.ToPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.ToPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Destination Port";
                }

                else
                {
                    errorText = errorText + ", Missing Destination Port";
                }
            }

            if (!string.IsNullOrEmpty(item.MinPriceText) && item.MinPrice == null)
            {
                error = true;

                if (item.IsMinPriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Min price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Min price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Min price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Min price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step1PriceText) && item.Step1Price == null)
            {
                error = true;

                if (item.IsStep1PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 1 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 1 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 1 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 1 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step2PriceText) && item.Step2Price == null)
            {
                error = true;

                if (item.IsStep2PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 2 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 2 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 2 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 2 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step3PriceText) && item.Step3Price == null)
            {
                error = true;

                if (item.IsStep3PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 3 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 3 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 3 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 3 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step4PriceText) && item.Step4Price == null)
            {
                error = true;

                if (item.IsStep4PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 4 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 4 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 4 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 4 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step5PriceText) && item.Step5Price == null)
            {
                error = true;

                if (item.IsStep5PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 5 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 5 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 5 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 5 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step6PriceText) && item.Step6Price == null)
            {
                error = true;

                if (item.IsStep6PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 6 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 6 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 6 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 6 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step7PriceText) && item.Step7Price == null)
            {
                error = true;

                if (item.IsStep7PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 7 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 7 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 7 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 7 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Step8PriceText) && item.Step8Price == null)
            {
                error = true;

                if (item.IsStep8PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 8 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Step 8 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 8 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 8 price format is invalid";
                    }
                }
            }

            item.HasErrors = error;
            item.ErrorText = errorText;
        }
        private void SetErrors_OceanFCLFreightCost(ExcelTariffLines item)
        {
            bool error = false;
            string errorText = "";

            if (!string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (item.FromPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.FromPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.FromPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.FromPortText) && string.IsNullOrEmpty(item.FromPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Origin Port";
                }

                else
                {
                    errorText = errorText + ", Missing Origin Port";
                }
            }

            if (!string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (item.ToPortIsNotAir)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port Not Found";
                    }

                    else
                    {
                        errorText = errorText + ", Port Not Found";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Port with code " + item.ToPortText + " not found";
                    }

                    else
                    {
                        errorText = errorText + ", Port with code " + item.ToPortText + " not found";
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.ToPortText) && string.IsNullOrEmpty(item.ToPortId))
            {
                error = true;

                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Destination Port";
                }

                else
                {
                    errorText = errorText + ", Missing Destination Port";
                }
            }
            
            if (!string.IsNullOrEmpty(item.Surcharge1PriceText) && item.Surcharge1Price == null)
            {
                error = true;

                if (item.IsSurcharge1PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 1 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 1 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 1 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 1 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge2PriceText) && item.Surcharge2Price == null)
            {
                error = true;

                if (item.IsSurcharge2PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 2 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 2 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 2 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 2 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge3PriceText) && item.Surcharge3Price == null)
            {
                error = true;

                if (item.IsSurcharge3PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 3 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 3 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 3 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 3 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge4PriceText) && item.Surcharge4Price == null)
            {
                error = true;

                if (item.IsSurcharge4PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 4 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 4 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 4 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 4 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.Surcharge5PriceText) && item.Surcharge5Price == null)
            {
                error = true;

                if (item.IsSurcharge5PriceMinus)
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 5 price can't be minus";
                    }

                    else
                    {
                        errorText = errorText + ", Container 5 price can't be minus";
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Container 5 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Container 5 price format is invalid";
                    }
                }
            }
            
            item.HasErrors = error;
            item.ErrorText = errorText;
        }
        private Port GetPortDetails(string code, int tenant)
        {
            Port myPort = null;
            if (this.portRepository == null)
            {
                this.portRepository = new PortRepository(tenant);
            }

            if (!string.IsNullOrEmpty(code))
            {
                code = code.Trim();
                if (this.TariffType == "AFC")
                {
                    myPort = this.portRepository.GetAirlinePortByCode(tenant, code, true);
                }
                else if (this.TariffType == "OLC" || this.TariffType == "OFC")
                {
                    myPort = this.portRepository.GetOceanPortByCombinedCode(code, tenant);
                }

                if (myPort == null)
                {
                    Port portZero = null;
                    if (this.TariffType == "AFC")
                    {
                        portZero = this.portRepository.GetAirlinePortByCode(0, code, true);
                    }
                    else if (this.TariffType == "OLC" || this.TariffType == "OFC")
                    {
                        portZero = this.portRepository.GetOceanPortByCombinedCode(code, 0);
                    }
                    if (portZero != null)
                    {
                        myPort = this.GetPortCopyToCurrentTenant(portZero, tenant);
                    }
                }
            }
            return myPort;
        }
        private Port GetPortCopyToCurrentTenant(Port ZeroPort, int tenant)
        {
            ICommonDataContext objectContext = this.portRepository.context;

            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Country country = countryRepository.GetSingleCountryByCode(ZeroPort.Country.Code, tenant, false);

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

            Port newPort = new Port()
            {
                Id = IdCounter.GetNumber("Port", tenant).ToString(),
                Code = ZeroPort.Code,
                CombinedCode = ZeroPort.CombinedCode,
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

            TableLastUpdateClass.UpdateTableHistory(tenant, "Port");

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
                                    }
                , tenant);

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
                TariffSurchargesUpdateUpdateService tariffSurchargeUpdateService = new TariffSurchargesUpdateUpdateService(tariffContext, new Dictionary<string, IContext>(), authToken.Tenant);

                if (tariff != null)
                {
                    TariffVersionPM iDraftVersion = tariff.TariffVersions.Where(d => d.IsDraft).FirstOrDefault();
                    TariffSurchargesUpdatePM tariffSurchageLog;
                    if (iDraftVersion != null)
                    {
                        List<FromToClass> routs = this.ComputeRoutsList(args.From, args.To, authToken.Tenant, tariff.TypeCode);
                        bool isValid = this.ValidateStartDate(tariff, iDraftVersion, routs, args.StartDate, tariffContext);

                        if(isValid)
                        {
                            int currentLinesCount = iDraftVersion.TariffLines.Count;

                            if (routs.Count + currentLinesCount > 1000)
                            {
                                isValid = false;
                                throw new ApplicationException("Can't perform this update due to tariff lines limitation to 1000");
                            }
                        }

                        if (isValid)
                        {
                            string mySurchargesText = "";
                            foreach (FromToClass rout in routs)
                            {
                                mySurchargesText = "";
                                TariffLinePM myLine = iDraftVersion.TariffLines.Where(d => d.OriginPortId == rout.FromCode && d.DestinationPortId == rout.ToCode).FirstOrDefault();
                                
                                // Update
                                if (myLine != null)
                                {
                                    myLine.ChangeSetOp = ChangeSetOperation.Update;
                                    myLine.StartDate = args.StartDate;

                                    foreach (string charge in args.Surcharge)
                                    {
                                        string[] charge_array = charge.Split(',');

                                        if(tariff.TypeCode == "OFS")
                                        {
                                            var arrayChargeType = "";

                                            if (this.FixFilter(charge_array[1]) != null)
                                            {
                                                arrayChargeType = charge_array[1];
                                            }

                                            mySurchargesText += arrayChargeType + ", ";

                                            string chargeId = null;
                                            if (this.FixFilter(charge_array[0]) != null)
                                            {
                                                chargeId = charge_array[0];
                                            }

                                            TariffLinesContainersPricePM containersPricePM = myLine.ContainersPrices.Where(d => d.SurchargeId == chargeId).FirstOrDefault();
                                            if (containersPricePM == null)
                                            {
                                                this.BuildContainersPrices(tariff, charge_array, myLine, true);
                                            }

                                            else
                                            {
                                                this.BuildContainersPrices(tariff, charge_array, myLine, false, containersPricePM);
                                            }
                                        }

                                        else
                                        {
                                            decimal? price = null;
                                            decimal? minPrice = null;

                                            if (this.FixFilter(charge_array[1]) != null)
                                            {
                                                price = Convert.ToDecimal(charge_array[1]);
                                            }

                                            if (this.FixFilter(charge_array[2]) != null)
                                            {
                                                minPrice = Convert.ToDecimal(charge_array[2]);
                                            }

                                            var arrayChargeType = "";
                                            if (this.FixFilter(charge_array[4]) != null)
                                            {
                                                arrayChargeType = charge_array[4];
                                            }

                                            mySurchargesText += arrayChargeType + ", ";

                                            PropertyInfo valuePropInfo1 = myLine.GetType().GetProperty("Surcharge" + charge_array[3] + "Price");
                                            PropertyInfo valuePropInfo2 = myLine.GetType().GetProperty("Surcharge" + charge_array[3] + "MinPrice");

                                            valuePropInfo1.SetValue(myLine, price, null);
                                            valuePropInfo2.SetValue(myLine, minPrice, null);
                                        }
                                    }
                                }
                                // New 
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
                                        CurrencyId = tariff.CurrencyId,
                                    };

                                    foreach (string charge in args.Surcharge)
                                    {
                                        string[] charge_array = charge.Split(',');

                                        if (tariff.TypeCode == "OFS")
                                        {
                                            var arrayChargeType = "";

                                            if (this.FixFilter(charge_array[1]) != null)
                                            {
                                                arrayChargeType = charge_array[1];
                                            }

                                            mySurchargesText += arrayChargeType + ", ";

                                            this.BuildContainersPrices(tariff, charge_array, tariffLine, true);
                                        }

                                        else
                                        {
                                            decimal? price = null;
                                            decimal? minPrice = null;

                                            if (this.FixFilter(charge_array[1]) != null)
                                            {
                                                price = Convert.ToDecimal(charge_array[1]);
                                            }

                                            if (this.FixFilter(charge_array[2]) != null)
                                            {
                                                minPrice = Convert.ToDecimal(charge_array[2]);
                                            }
                                            var arrayChargeType = "";
                                            if (this.FixFilter(charge_array[4]) != null)
                                            {
                                                arrayChargeType = charge_array[4];
                                            }

                                            mySurchargesText += arrayChargeType + ", ";
                                            PropertyInfo valuePropInfo1 = tariffLine.GetType().GetProperty("Surcharge" + charge_array[3] + "Price");
                                            PropertyInfo valuePropInfo2 = tariffLine.GetType().GetProperty("Surcharge" + charge_array[3] + "MinPrice");
                                            valuePropInfo1.SetValue(tariffLine, price, null);
                                            valuePropInfo2.SetValue(tariffLine, minPrice, null);
                                        }
                                    }

                                    iDraftVersion.TariffLines.Add(tariffLine);
                                }
                            }

                            foreach (var item in SurchargeLog)
                            {
                                tariffSurchageLog = new TariffSurchargesUpdatePM();
                                tariffSurchageLog.TariffId = tariff.Id;
                                tariffSurchageLog.Version = tariff.LastVersion;
                                tariffSurchageLog.Surcharges = mySurchargesText != null ? mySurchargesText.Trim().TrimEnd(',') : mySurchargesText;
                                tariffSurchageLog.StartDate = args.StartDate;
                                tariffSurchageLog.LinesUpdated = item.Count;
                                tariffSurchageLog.UpdateMethodCode = "BA";
                                tariffSurchageLog.ChangeSetOp = ChangeSetOperation.Insert;
                                tariffSurchageLog.Tenant = authToken.Tenant;
                                tariffSurchageLog.To = SurchargeLog != null ? string.Join(", ", item.ToPorts.ToArray()) : null;
                                tariffSurchageLog.From = SurchargeLog != null ? string.Join(", ", item.FromPorts.ToArray()) : null;
                                tariffSurchargeUpdateService.Update(tariffSurchageLog, true);
                            }

                            tariff.IsSurchargeUpdate = true;
                            tariff.IsFromUpdateScreen = true;
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

        private void BuildContainersPrices(TariffPM tariff, string[] charge_array, TariffLinePM tariffLine, bool isNew, TariffLinesContainersPricePM containersPricePM = null)
        {
            string chargeId = null;
            decimal? price1 = null;
            decimal? price2 = null;
            decimal? price3 = null;
            decimal? price4 = null;
            decimal? price5 = null;
            decimal? costPrice = null;

            if (this.FixFilter(charge_array[0]) != null)
            {
                chargeId = charge_array[0];
            }

            if (this.FixFilter(charge_array[2]) != null)
            {
                price1 = Convert.ToDecimal(charge_array[2]);
            }

            if (this.FixFilter(charge_array[3]) != null)
            {
                price2 = Convert.ToDecimal(charge_array[3]);
            }

            if (this.FixFilter(charge_array[4]) != null)
            {
                price3 = Convert.ToDecimal(charge_array[4]);
            }

            if (this.FixFilter(charge_array[5]) != null)
            {
                price4 = Convert.ToDecimal(charge_array[5]);
            }

            if (this.FixFilter(charge_array[6]) != null)
            {
                price5 = Convert.ToDecimal(charge_array[6]);
            }

            if (this.FixFilter(charge_array[7]) != null)
            {
                costPrice = Convert.ToDecimal(charge_array[7]);
            }

            if (isNew)
            {
                containersPricePM = new TariffLinesContainersPricePM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,                    
                    Tenant = tariffLine.Tenant,
                    TariffId = tariffLine.TariffId,
                    SurchargeId = chargeId,
                    Price1 = price1,
                    Price2 = price2,
                    Price3 = price3,
                    Price4 = price4,
                    Price5 = price5,
                    CostPrice = costPrice,
                };

                tariffLine.ContainersPrices.Add(containersPricePM);
            }

            else
            {
                containersPricePM.ChangeSetOp = ChangeSetOperation.Update;
                containersPricePM.CostPrice = costPrice;
                if (!string.IsNullOrEmpty(tariff.ContainerType1Id))
                {
                    containersPricePM.Price1 = price1;
                }

                if (!string.IsNullOrEmpty(tariff.ContainerType2Id))
                {
                    containersPricePM.Price2 = price2;
                }

                if (!string.IsNullOrEmpty(tariff.ContainerType3Id))
                {
                    containersPricePM.Price3 = price3;
                }

                if (!string.IsNullOrEmpty(tariff.ContainerType4Id))
                {
                    containersPricePM.Price4 = price4;
                }

                if (!string.IsNullOrEmpty(tariff.ContainerType5Id))
                {
                    containersPricePM.Price5 = price5;
                }
            }
        }

        private bool ValidateStartDate(TariffPM tariff, TariffVersionPM iDraftVersion, List<FromToClass> routs, DateTime startDate, ITariffModuleContext tariffContext)
        {
            bool isValid = true;

            TariffVersionPM iPreviousVersion = tariff.ActiveVersions.OrderByDescending(o => o.CreateDate).FirstOrDefault();
            if (iPreviousVersion != null)
            {
                List<TariffLine> iPreviousLines = tariffContext.TariffLines.Where(d => d.Version == iPreviousVersion.Version && d.TariffId == tariff.Id).ToList();

                List<TariffLinePM> draftLines = iDraftVersion.TariffLines.Where(d => (routs.Select(s => s.FromCode).Contains(d.OriginPortId) && routs.Select(s => s.ToCode).Contains(d.DestinationPortId))).ToList();
                foreach (TariffLinePM linePM in draftLines)
                {
                    if (linePM.StartDate != null)
                    {
                        TariffLine iPreviousLine = iPreviousLines.Where(d => d.OriginPortId == linePM.OriginPortId && d.DestinationPortId == linePM.DestinationPortId).FirstOrDefault();
                        if (iPreviousLine != null)
                        {
                            if (startDate < iPreviousLine.StartDate)
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

        List<SurchargeLog> SurchargeLog = new List<SurchargeLog>();
        private List<FromToClass> ComputeRoutsList(List<string> fromList, List<string> toList, int tenant, string typeCode)
        {
            List<FromToClass> myResult = new List<FromToClass>();
            CarrierAreasPortRepository carrierAreasPortRepository = new CarrierAreasPortRepository(tenant);
            CarrierAreaRepository carrierAreasRepository = new CarrierAreaRepository(tenant);
            var surchargeLogItem = new SurchargeLog();
            List<string> areasFromPorts = new List<string>();
            List<string> areasToPorts = new List<string>();
            var isFirstTime = true;
            foreach (string item_from in fromList)
            {
                string[] from = item_from.Split(',');

                if (from[0] == "Port")
                {
                    if (typeCode.StartsWith("A"))
                    {
                        areasFromPorts.Add(from[2]);
                    } else
                    {
                        areasFromPorts.Add(from[3]);
                    }
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
                            if (isFirstTime)
                            {
                                if (typeCode.StartsWith("A"))
                                {
                                    areasToPorts.Add(to[2]);
                                } else
                                {
                                    areasToPorts.Add(to[3]);
                                }
                            }

                            surchargeLogItem.Count += 1;
                        }

                        else if (to[0] == "Area")
                        {
                            List<CarrierAreasPort> areasPorts = carrierAreasPortRepository.GetCarrierAreasPortByAreaId(to[1], tenant);
                            var area = carrierAreasRepository.GetSingleCarrierArea(to[1], tenant);
                            if (isFirstTime && area != null)
                            {
                                areasToPorts.Add(area.Name);
                            }
                            if (areasPorts != null && areasPorts.Count > 0)
                            {
                                foreach (CarrierAreasPort port in areasPorts)
                                {
                                    FromToClass routItem = new FromToClass()
                                    {
                                        FromCode = from[1],
                                        ToCode = port.PortId,
                                    };

                                    myResult.Add(routItem);
                                }
                                surchargeLogItem.Count += areasPorts.Count();
                            }
                        }
                    }
                }
                else if (from[0] == "Area")
                {
                    List<CarrierAreasPort> areasPorts = carrierAreasPortRepository.GetCarrierAreasPortByAreaId(from[1], tenant);
                    if (areasPorts != null && areasPorts.Count > 0)
                    {
                        var area = carrierAreasRepository.GetSingleCarrierArea(from[1], tenant);
                        areasFromPorts.Add(area.Name);
                        var isFirstTimeAreaLoop = true;
                        foreach (CarrierAreasPort port in areasPorts)
                        {
                            foreach (string item_to in toList)
                            {
                                string[] to = item_to.Split(',');

                                if (to[0] == "Port")
                                {
                                    FromToClass routItem = new FromToClass()
                                    {
                                        FromCode = port.PortId,
                                        ToCode = to[1],
                                    };

                                    myResult.Add(routItem);
                                    if (isFirstTime && isFirstTimeAreaLoop)
                                    {
                                        if (typeCode.StartsWith("A"))
                                        {
                                            areasToPorts.Add(to[2]);
                                        } else
                                        {
                                            areasToPorts.Add(to[3]);
                                        }
                                    }
                                    surchargeLogItem.Count += 1;

                                }
                                else if (to[0] == "Area")
                                {
                                    List<CarrierAreasPort> toAreasPorts = carrierAreasPortRepository.GetCarrierAreasPortByAreaId(to[1], tenant);
                                    var toarea = carrierAreasRepository.GetSingleCarrierArea(to[1], tenant);
                                    if (isFirstTime && isFirstTimeAreaLoop && toarea != null)
                                    {
                                        areasToPorts.Add(toarea.Name);
                                    }
                                    if (toAreasPorts != null && toAreasPorts.Count > 0)
                                    {
                                        foreach (CarrierAreasPort toPort in toAreasPorts)
                                        {
                                            FromToClass routItem = new FromToClass()
                                            {
                                                FromCode = port.PortId,
                                                ToCode = toPort.PortId,
                                            };

                                            myResult.Add(routItem);
                                        }
                                        surchargeLogItem.Count += toAreasPorts.Count();
                                    }
                                }
                            }
                            isFirstTimeAreaLoop = false;
                        }
                    }
                }

                isFirstTime = false;
            }

            surchargeLogItem.ToPorts = areasToPorts;
            surchargeLogItem.FromPorts = areasFromPorts;
            SurchargeLog.Add(surchargeLogItem);
            return myResult;
        }
        private string FixFilter(string filter)
        {
            string myResult = filter;

            if (myResult != null)
            {
                switch (myResult.ToLower())
                {
                    case "null":
                    case "undefined":
                        {
                            myResult = null;
                            break;
                        }
                }
            }

            return myResult;
        }
        
        public HttpResponseMessage GetTariffsLogsByTariffId(string tariffId, int version)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            string loggedUserEmail = authToken.Email;
            List<TariffSurchargesUpdatePM> logs = new List<TariffSurchargesUpdatePM>();

            TariffSurchargesUpdateQueryService query = new TariffSurchargesUpdateQueryService(tenant);
            logs = query.GetTariffsLogsByTariffId(tariffId, version, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, logs);
        }

        public HttpResponseMessage GetAllVersionsWithLinesForTariff(string tariffId)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            string loggedUserEmail = authToken.Email;

            ITariffModuleContext iContext = TariffModuleContext.GetContext(tenant);
            TariffVersionQueryService iTariffVersionQueryService = new TariffVersionQueryService(iContext);          
            List<TariffVersionPM> entityPMs = iTariffVersionQueryService.GetAllVersionsWithLines(tariffId, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, entityPMs);
        }
        public HttpResponseMessage GetRecentTariffs()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);
                
                string mail = SecurityUtility.GetAuthenticatedUser();
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

                ITariffModuleContext iContext = TariffModuleContext.GetContext(tenant);
                TariffListQueryService tariffListQueryservice = new TariffListQueryService(iContext);
                List<TariffList> myResult = tariffListQueryservice.GetRecentTariffs(contact.Id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTariffLineContainerPrices(string tariffId, int version, string fromPortId, string toPortId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                fromPortId = this.FixFilter(fromPortId);
                toPortId = this.FixFilter(toPortId);

                ITariffModuleContext iContext = TariffModuleContext.GetContext(authToken.Tenant);
                TariffLinesContainersPriceQueryService tariffLinesContainersPriceQueryService = new TariffLinesContainersPriceQueryService(iContext);
                List<TariffLinesContainersPricePM> myResult = tariffLinesContainersPriceQueryService.GetContainerPricesByVersionAndPorts(tariffId, version, fromPortId, toPortId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetRefreshPortsFromTranslations(string tariffId, int version)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ITariffModuleContext tariffModuleContext = TariffModuleContext.GetContext(tenant);
                TariffRepository tariffRepository = new TariffRepository(tariffModuleContext);
                Tariff tariff = tariffRepository.GetSingle(tariffId, tenant);

                if (tariff != null)
                {
                    TariffLineRepository tariffLineRepository = new TariffLineRepository(tariffModuleContext);
                    List<TariffLine> iDraftVersionLines = tariffLineRepository.GetTariffLinesByTariffAndVersion(tariffId, version, tenant);

                    if (iDraftVersionLines.Count > 0)
                    {
                        iDraftVersionLines = iDraftVersionLines.Where(d => d.HasErrors && (string.IsNullOrEmpty(d.OriginPortId) || string.IsNullOrEmpty(d.DestinationPortId))).ToList();

                        TariffCarrierTranslationRepository tariffCarrierTranslationRepository = new TariffCarrierTranslationRepository(tenant);

                        foreach (TariffLine tariffLine in iDraftVersionLines)
                        {
                            bool tariffLineUpdated = false;

                            if (string.IsNullOrEmpty(tariffLine.OriginPortId) && !string.IsNullOrEmpty(tariffLine.OriginPortText))
                            {
                                TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetCarrierTranslationByPartnerCodeAndCarrier(tariffLine.OriginPortText, tariff.SellerId, tenant);
                                if (carrierTranslation != null)
                                {
                                    tariffLine.OriginPortId = carrierTranslation.PortId;
                                    tariffLine.OriginPortText = null;
                                    tariffLineUpdated = true;
                                }
                            }

                            if (string.IsNullOrEmpty(tariffLine.DestinationPortId) && !string.IsNullOrEmpty(tariffLine.DestinationPortText))
                            {
                                TariffCarrierTranslation carrierTranslation = tariffCarrierTranslationRepository.GetCarrierTranslationByPartnerCodeAndCarrier(tariffLine.DestinationPortText, tariff.SellerId, tenant);
                                if (carrierTranslation != null)
                                {
                                    tariffLine.DestinationPortId = carrierTranslation.PortId;
                                    tariffLine.DestinationPortText = null;
                                    tariffLineUpdated = true;
                                }
                            }

                            if (tariffLineUpdated)
                            {
                                tariffLine.ErrorText = TariffLineHelper.ComputeTariffLineErrorText(tariffLine);

                                if (string.IsNullOrEmpty(tariffLine.ErrorText))
                                {
                                    tariffLine.HasErrors = false;
                                }
                                else
                                {
                                    tariffLine.HasErrors = true;
                                }

                                tariffLineRepository.Update(tariffLine);
                            }
                        }

                        tariffLineRepository.SubmitChanges();
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTariffsPricesConnectedToPayables(string freightTariffId, string shipmentId, string tariffType)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Tariff", "READ", tenant);

                freightTariffId = this.FixFilter(freightTariffId);
                shipmentId = this.FixFilter(shipmentId);
                
                PriceCheckManager priceCheckManager = new PriceCheckManager(freightTariffId, shipmentId, tariffType, tenant);
                List<TariffSearchSummary> myResult = priceCheckManager.GetSummaryForExistedTariff();
                
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class SurchargeLog
    {
        public List<string> FromPorts { get; set; }
        public List<string> ToPorts { get; set; }
        public int Count { get; set; }
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

    public class ChargeContainerClass
    {
        public int ChargeIndex { get; set; }
        public int ContainerIndex { get; set; }
        public string ChargeCode { get; set; }
        public string ContainerCode { get; set; }
        public string ChargeId { get; set; }
        public string ContainerId { get; set; }
    }

    public class ExcelTariffLines
    {
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortCombinedCode { get; set; }
        public string FromPortName { get; set; }
        public string ToPortId { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortCombinedCode { get; set; }
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

        public bool IsMinPriceMinus { get; set; }
        public bool IsStep1PriceMinus { get; set; }
        public bool IsStep2PriceMinus { get; set; }
        public bool IsStep3PriceMinus { get; set; }
        public bool IsStep4PriceMinus { get; set; }
        public bool IsStep5PriceMinus { get; set; }
        public bool IsStep6PriceMinus { get; set; }
        public bool IsStep7PriceMinus { get; set; }
        public bool IsStep8PriceMinus { get; set; }

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

        public bool IsSurcharge1PriceMinus { get; set; }
        public bool IsSurcharge2PriceMinus { get; set; }
        public bool IsSurcharge3PriceMinus { get; set; }
        public bool IsSurcharge4PriceMinus { get; set; }
        public bool IsSurcharge5PriceMinus { get; set; }
        public bool IsSurcharge6PriceMinus { get; set; }
        public bool IsSurcharge7PriceMinus { get; set; }
        public bool IsSurcharge8PriceMinus { get; set; }
        public bool IsSurcharge9PriceMinus { get; set; }
        public bool IsSurcharge10PriceMinus { get; set; }

        public int Index { get; set; }

        public string Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateText { get; set; }
        public string TransitTime { get; set; }
    }
}