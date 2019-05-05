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

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TariffDomainController : ApiController
    {
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
                                            }).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetDownloadTariffLines(string tariffId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                TariffQueryService tariffQuery = new TariffQueryService(tenant);
                TariffPM tariffPM = tariffQuery.GetSingle(tariffId, true, false);
                List<TariffLinePM> tariffLines = tariffPM.TariffLines;

                byte[] data = this.ExportTariffLinesToExcel(tariffPM, tariffLines, tenant);

                string fileName = "Tariffs" + DateTime.Now.ToShortDateString();
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

                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private byte[] ExportTariffLinesToExcel(TariffPM tariff, List<TariffLinePM> tariffLines, int tenant)
        {
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
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
            }
            else
            {
                table.Columns.Add("From");
                table.Columns.Add("To");
                table.Columns.Add("Min Price");
                table.Columns.Add(tariff.PriceSteps + " KG");
            }

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
                    table.Rows.Add(row);
                }
            }
            sheet1.ImportDataTable(table, true, 1, 1);
            workbook.Version = ExcelVersion.Excel2007;
            workbook.SaveAs(memory);
            return memory.ToArray();
        }

        private PortRepository portRepository;

        [ActionName("PostUploadExcelFile")]
        public HttpResponseMessage PostUploadExcelFile(TariffFilterParameter filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                this.portRepository = new PortRepository(authToken.Tenant);

                byte[] fileData = Convert.FromBase64String(filter.FileData);

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                List<ExcelTariffLines> tariffLinesResult = new List<ExcelTariffLines>();
                foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
                {
                    String[] rowData = new String[sheet.Columns.Count()];
                    ExcelTariffLines tariffLine = new ExcelTariffLines();                                       

                    for (int i = 0; i < sheet.Columns.Count(); i++)
                    {
                        rowData[i] = row.Cells[i].Value2.ToString();
                    }
                    
                    Port fromPort = this.GetPortDetails(rowData[0], authToken.Tenant);
                    if (fromPort != null)
                    {
                        tariffLine.FromPortId = fromPort.Id;
                        tariffLine.FromPortCode = fromPort.Code;
                        tariffLine.FromPortName = fromPort.EnglishName;
                    }   
                    else
                    {
                        tariffLine.FromPortText = this.TrimTo_20(rowData[0]);
                    }

                    Port toPort = this.GetPortDetails(rowData[1], authToken.Tenant);
                    if (toPort != null)
                    {
                        tariffLine.ToPortId = toPort.Id;
                        tariffLine.ToPortCode = toPort.Code;
                        tariffLine.ToPortName = toPort.EnglishName;
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

                    if (rowData.Length >= 10)
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

                    tariffLinesResult.Add(tariffLine);
                }

                return Request.CreateResponse(HttpStatusCode.OK, tariffLinesResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetApproveVersion(string tariffId)
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
                TariffRepository tariffRepository = new TariffRepository(context);
                TariffVersionRepository tariffVersionRepository = new TariffVersionRepository(context);

                Tariff tariff = tariffRepository.GetSingle(tariffId, tenant);

                if (tariff != null)
                {
                    TariffVersion tariffVersion = tariffVersionRepository.GetSingle(tariffId, tariff.LastVersion, tenant);
                    if(tariffVersion != null)
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
                }

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
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
    }

    public class TariffFilterParameter
    {
        public int Tenant { get; set; }
        public string FileData { get; set; }
        public string PriceSteps { get; set; }
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
    }
}


