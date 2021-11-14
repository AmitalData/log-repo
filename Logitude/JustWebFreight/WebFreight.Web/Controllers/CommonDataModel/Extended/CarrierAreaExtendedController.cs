using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class CarrierAreaExtendedController : ApiController
    {
        public HttpResponseMessage GetDownloadCarrierAreaPorts(string carrierAreaId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                string loggedUserEmail = authToken.Email;

                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                CarrierAreaRepository carrierAreaRepository = new CarrierAreaRepository(context);
                CarrierAreaQuery carrierAreaQuery = new CarrierAreaQuery(carrierAreaRepository);
                CarrierAreaPM carrierArea = carrierAreaQuery.GetSinglePMWithComposition(carrierAreaId, tenant);
                string fileName = "";

                if (carrierArea != null)
                {
                    if (carrierArea.CarrierAreasPorts.Count > 0)
                    {
                        System.IO.MemoryStream memory = new System.IO.MemoryStream();
                        ExcelEngine excelEngine = new ExcelEngine();
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                        IWorksheet sheet1 = workbook.Worksheets[0];
                        sheet1.Name = "Packages";
                        sheet1.Range["A1:B1"].CellStyle.Font.Bold = true;
                        sheet1.Range["A1:B1"].CellStyle.Font.Size = 10;
                        sheet1.Range["A1:B1"].CellStyle.Font.FontName = "Calibri";
                        sheet1.Range["A1:B1"].CellStyle.Font.Color = ExcelKnownColors.White;
                        sheet1.Range["A1:B1"].CellStyle.Color = System.Drawing.Color.Gray;
                        sheet1.Range["A1:B1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                        sheet1.Range["A1"].ColumnWidth = 15;
                        sheet1.Range["B1"].ColumnWidth = 30;

                        DataTable dataTable1 = new DataTable();
                        dataTable1.Columns.Add("Port Code");
                        dataTable1.Columns.Add("Port Name");

                        foreach (CarrierAreasPortPM areaPort in carrierArea.CarrierAreasPorts)
                        {
                            DataRow row = dataTable1.NewRow();
                            row[0] = carrierArea.TransportModeCode == "A" ? areaPort.Code: areaPort.CombinedCode;
                            row[1] = areaPort.Name;
                            dataTable1.Rows.Add(row);
                        }

                        sheet1.ImportDataTable(dataTable1, true, 1, 1);
                        workbook.SaveAs(memory);
                        byte[] data = memory.ToArray();

                        fileName = (carrierArea.TransportModeCode == "A" ? "Airline-" : "Shipping Line-") + carrierArea.Name + "-" + String.Format("{0:dd-MM-yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));

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

        [ActionName("PostUploadCarrierAreaPortsExcelFile")]
        public HttpResponseMessage PostUploadCarrierAreaPortsExcelFile(CarrierAreaParameters filter)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(filter.Tenant);

                byte[] fileData = Convert.FromBase64String(filter.FileData);

                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                List<ExcelPort> myResult = this.BuildPortsFromExcelLines(sheet, filter, authToken.Tenant);

                filter.RowsCount = sheet.UsedRange.Rows.Count() - 1;
                filter.ExcelPorts = myResult;

                return Request.CreateResponse(HttpStatusCode.OK, filter);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private List<ExcelPort> BuildPortsFromExcelLines(IWorksheet sheet, CarrierAreaParameters filter, int tenant)
        {
            List<ExcelPort> myResult = new List<ExcelPort>();

            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
            foreach (IRange row in sheet.UsedRange.Rows.Skip(1))
            {
                String[] rowData = new String[sheet.Columns.Count()];
                ExcelPort excelPort = new ExcelPort();

                for (int i = 0; i < sheet.Columns.Count(); i++)
                {
                    rowData[i] = row.Cells[i].Value2.ToString();
                }

                if (rowData.Length > 0)
                {
                    if (!string.IsNullOrEmpty(rowData[0]))
                    {
                        string portCode = rowData[0].Trim();

                        Port port = this.GetPortDetails(portCode, filter.TransportMode, tenant);
                        if (port != null)
                        {
                            excelPort.PortId = port.Id;
                            excelPort.PortCode = port.Code;
                            excelPort.PortName = port.EnglishName;
                            excelPort.PortCountryCode = port.Country == null ? null : port.Country.Code;
                        }

                        else
                        {
                            excelPort.HasError = true;
                            excelPort.ExcelPortCode = portCode;
                        }
                    }

                    else
                    {
                        excelPort.HasError = true;
                    }
                }

                myResult.Add(excelPort);
            }

            return myResult;
        }
        private Port GetPortDetails(string code, string transportMode, int tenant)
        {
            Port myPort = null;

            if (!string.IsNullOrEmpty(code))
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                PortRepository portRepository = new PortRepository(commonContext);

                code = code.Trim();
                if (transportMode == "A")
                {
                    myPort = portRepository.GetAirlinePortByCode(tenant, code, true);
                }

                else
                {
                    myPort = portRepository.GetOceanPortByCombinedCode(code, tenant);
                }

                if (myPort == null)
                {
                    Port portZero = null;
                    if (transportMode == "A")
                    {
                        portZero = portRepository.GetAirlinePortByCode(0, code, true);
                    }

                    else
                    {
                        portZero = portRepository.GetOceanPortByCombinedCode(code, 0);
                    }

                    if (portZero != null)
                    {
                        myPort = this.GetPortCopyToCurrentTenant(portZero, tenant, commonContext, portRepository);
                    }
                }
            }

            return myPort;
        }
        private Port GetPortCopyToCurrentTenant(Port ZeroPort, int tenant, ICommonDataContext commonContext, PortRepository portRepository)
        {
            CountryRepository countryRepository = new CountryRepository(commonContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(commonContext);

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
    }

    public class CarrierAreaParameters
    {
        public int Tenant { get; set; }
        public string FileData { get; set; }
        public string CarrierAreaId { get; set; }
        public string TransportMode { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int RowsCount { get; set; }
        public List<ExcelPort> ExcelPorts { get; set; }
    }

    public class ExcelPort
    {
        public string PortId { get; set; }
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortCountryCode { get; set; }
        public bool HasError { get; set; }
        public string ExcelPortCode { get; set; }
    }
}