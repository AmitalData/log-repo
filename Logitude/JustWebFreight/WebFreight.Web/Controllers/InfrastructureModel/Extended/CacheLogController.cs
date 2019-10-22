using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http; 
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class CacheLogController : ApiController
    {
        public HttpResponseMessage GetKeysList()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                List<CacheLog> keys = CacheLogger.KeysGetCounter
                    .Select(d=> new CacheLog() { Key = d.Key, Count = d.Value } )
                    .OrderByDescending(d=>d.Count)
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, keys);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostResetLog()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CacheLogger.KeysGetCounter.Clear();

                return Request.CreateResponse(HttpStatusCode.OK, "Reset");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutEnableLog(bool enabled)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CacheLogger.IsCacheLoggerEnabled = enabled;

                return Request.CreateResponse(HttpStatusCode.OK, "Reset");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetIsLoggerEnabled()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, CacheLogger.IsCacheLoggerEnabled);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        
        public HttpResponseMessage GetLoggedCardFetchInterval(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                CardQuery cardQuery = new CardQuery(authToken.Tenant);
                CardPM cardPM = cardQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, cardPM);
                

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetCacheLogExcelFile()
        {
            try
            {
                int tenant = GetAndAuthinticatTenant();

                string fileName = "CacheLog-" + string.Format("{0:HHmmssf-dd-MM-yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));

                byte[] cacheLogExcelBytes = GetCacheLogKeysAsExcelFile();

                if (cacheLogExcelBytes != null)
                    ExportExcelFileToBlob(tenant, fileName, cacheLogExcelBytes);

                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void ExportExcelFileToBlob(int tenant, string fileName, byte[] cacheLogExcelBytes)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = "others",
                Extension = "xls",
                Tenant = tenant,
                FileSize = cacheLogExcelBytes.Length,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(cacheLogExcelBytes, fileInfo);
        }

        private int GetAndAuthinticatTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);
            return tenant;
        }

        private byte[] GetCacheLogKeysAsExcelFile()
        {
            DataTable dataTable = GetCacheLogDataTable();

            IWorkbook workbook = GetExcelWorkbook();

            FillWorkbookDataFromDataTable(dataTable, workbook);

            return GetWorkbookAsBytes(workbook);
        }

        private void FillWorkbookDataFromDataTable(DataTable dataTable, IWorkbook workbook)
        {
            IWorksheet excelSheet = GetCustomizeExcelSheet(workbook);
            excelSheet.ImportDataTable(dataTable, true, 1, 1);
        }

        private byte[] GetWorkbookAsBytes(IWorkbook workbook)
        {
            MemoryStream memory = new MemoryStream();
            workbook.SaveAs(memory);
            byte[] excelFileBytes = memory.ToArray();
            return excelFileBytes;
        }

        private DataTable GetCacheLogDataTable()
        {
            DataTable dataTable = BuildDataTableForCacheLog();

            List<CacheLog> keys = GetCacheLogKeys();

            FillDataTableRowsFromKeys(dataTable, keys);
            return dataTable;
        }

        private IWorksheet GetCustomizeExcelSheet(IWorkbook workbook)
        {
            IWorksheet firstSheet = workbook.Worksheets[0];

            string range = "A1:B1";

            firstSheet.SetColumnWidth(1, 80);
            firstSheet.Range["A1:A2000"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;

            firstSheet.SetColumnWidth(2, 15);
            firstSheet.Range["B1:B2000"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;

            firstSheet.Range[range].CellStyle.Color = System.Drawing.Color.Gray;
            firstSheet.Range[range].CellStyle.Font.Color = ExcelKnownColors.White;

            return firstSheet;
        }

        private IWorkbook GetExcelWorkbook()
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            return workbook;
        }

        private void FillDataTableRowsFromKeys(DataTable dataTable, List<CacheLog> keys)
        {
            foreach (CacheLog key in keys)
            {
                DataRow newRow = GetNewRowsForKey(dataTable, key);
                dataTable.Rows.Add(newRow);
            }
        }

        private DataRow GetNewRowsForKey(DataTable dataTable, CacheLog key)
        {
            DataRow newRow = dataTable.NewRow();
            newRow["Key"] = key.Key;
            newRow["Count"] = key.Count;
            return newRow;
        }

        private List<CacheLog> GetCacheLogKeys()
        {
            return CacheLogger.KeysGetCounter
                    .Select(d => new CacheLog() { Key = d.Key, Count = d.Value })
                    .OrderByDescending(d => d.Count)
                    .ToList();
        }

        private DataTable BuildDataTableForCacheLog()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Key");
            table.Columns.Add("Count");

            return table;
        }
    }
}