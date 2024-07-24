using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.SystemLogs;
using Syncfusion.XlsIO;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Newtonsoft.Json;
using Simplog.Data.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalPortalReportController : ApiController
    {
        [HttpPost]
        [Route("DigitalPortalReport/GetDigitalToExcelData")]
        public HttpResponseMessage GetDigitalToExcelData(GeneralFilters filters)
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, filters.CardId);
                var service = new DigitalPortalQueryToExcelExportService();
                var result = service.ExportQueryDataToExcel(filters);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalPortalReport/UploadDigitalTextCode")]
        public HttpResponseMessage UploadDigitalTextCode(ExcelPackageTextCodeFilter filter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (authToken.Tenant != 0)
                {
                    throw new Exception("This operation is allowed only for customer care users");
                }

                byte[] fileData = Convert.FromBase64String(filter.FileData);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(fileData);
                var excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                IWorksheet sheet = workbook.Worksheets[0];

                var importedData = this.BuildTextCodeObjects(sheet);
                var textCodeQuery = new DigitalTextCodeQueryService(0);
                var objectTables = textCodeQuery.GetDigitalTextCodesObjetTables(0).ToDictionary(a => a.ObjectTableName, x => x.ObjectTableId);
                var digitalProfileQuery = new DigitalProfileQueryService(0);
                var tenantDigitalProfiles = digitalProfileQuery.GetDigitalProfileQuery(0).ToDictionary(a => a.Code, x => x.Id);

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(0).Date;

                foreach (var item in importedData)
                {
                    var internalKeys = item.Value;

                    foreach (var internalData in internalKeys)
                    {
                        var customTextCodes = textCodeQuery.GetDigitalTextCodesQuery(0,
                                                                                     objectTables[item.Key],
                                                                                     internalData.Key,
                                                                                     filter.LanguageCode);

                        if (customTextCodes != null)
                        {
                            customTextCodes.Labels = JsonConvert.SerializeObject(internalKeys[internalData.Key]);
                            customTextCodes.UpdateDate = todayDate;
                        }
                        else
                        {
                            customTextCodes = new DigitalTextCodeList
                            {
                                ObjectTableId = objectTables[item.Key],
                                Tenant = 0,
                                ProfileId = tenantDigitalProfiles[internalData.Key],
                                Labels = JsonConvert.SerializeObject(internalKeys[internalData.Key]),
                                LanguageCode = filter.LanguageCode,
                                CreateDate = todayDate,
                                UpdateDate = todayDate
                            };
                        }

                        textCodeQuery.UpdateDigitalTextCodes(customTextCodes);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private Dictionary<string, Dictionary<string, List<DigitalTextCodeUpdateObject>>> BuildTextCodeObjects(IWorksheet sheet)
        {
            var data = new Dictionary<string, Dictionary<string, List<DigitalTextCodeUpdateObject>>>();

            if (sheet.Columns.Count() < 6)
            {
                throw new Exception("Excel not in the correct format");
            }

            int rowNumber = 4;
            foreach (IRange row in sheet.UsedRange.Rows.Skip(3))
            {
                var rowData = new string[sheet.Columns.Count()];

                for (int i = 0; i < sheet.Columns.Count(); i++)
                {
                    rowData[i] = row.Cells[i].Value2.ToString();
                }

                ValidateRowData(rowNumber, rowData);

                var labelObject = new DigitalTextCodeUpdateObject
                {
                    TextCode = rowData[0],
                    FieldCode = rowData[1],
                    DefaultText = !string.IsNullOrWhiteSpace(rowData[3]) ? rowData[3] : rowData[2],
                    DisplayText = ""
                };

                if (data.ContainsKey(rowData[5]))
                {
                    var res = data[rowData[5]];

                    if (res.ContainsKey(rowData[4]))
                    {
                        res[rowData[4]].Add(labelObject);
                    }
                    else
                    {
                        res.Add(rowData[4], new List<DigitalTextCodeUpdateObject> { labelObject });
                    }
                }
                else
                {
                    var internalKey = new Dictionary<string, List<DigitalTextCodeUpdateObject>>();
                    internalKey.Add(rowData[4], new List<DigitalTextCodeUpdateObject> { labelObject });
                    data.Add(rowData[5], internalKey);
                }

                rowNumber++;
            }

            return data;
        }

        private static void ValidateRowData(int rowNumber, string[] rowData)
        {
            if (string.IsNullOrWhiteSpace(rowData[0]))
            {
                throw new Exception($"Text code can't be empty for line {rowNumber}");
            }

            if (string.IsNullOrWhiteSpace(rowData[5]))
            {
                throw new Exception($"Entity name can't be empty for line {rowNumber}");
            }

            if (string.IsNullOrWhiteSpace(rowData[4]))
            {
                throw new Exception($"Profile code can't be empty for line {rowNumber}");
            }
        }

        [HttpGet]
        [Route("DigitalPortalReport/GetDigitalExportExecutionLogStatus")]
        public HttpResponseMessage GetDigitalExportExecutionLogStatus(string logId, string cardId)
        {
            int tenant = 0;
            string email = "";

            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
                var queryExecutionLogRepository = new QueryExportExecutionLogRepository(authToken.Tenant);
                QueryExportExecutionLog queryExecutionLog = queryExecutionLogRepository.GetSingle(logId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, queryExecutionLog);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}