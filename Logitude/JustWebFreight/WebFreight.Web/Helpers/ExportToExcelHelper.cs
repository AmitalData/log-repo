using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using static Stimulsoft.Report.StiOptions.Export;
using System.Diagnostics;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using System.Web.UI.WebControls;
using IWorkbook = Syncfusion.XlsIO.IWorkbook;
using static NPOI.HSSF.Util.HSSFColor;
using NPOI.SS.Formula.Functions;
using Match = System.Text.RegularExpressions.Match;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System.Globalization;
using System.Configuration;
using NPOI.HSSF.UserModel;
using static System.Net.Mime.MediaTypeNames;

namespace WebFreight.Web.Helpers
{
    //public class ExportToExcelHelper
    public partial class ExportToExcelHelper
    {
        public readonly int MaxCellLength = 32767;
        public byte[] ExportQueryToExcel(ExportToExcelArgs exportToExcelArgs)
        {
            Stopwatch _Stopwatch;
            Stopwatch _Stopwatch1;


            LogTime("start all at : ", exportToExcelArgs.IsXslxFormat, exportToExcelArgs.Tenant);
            _Stopwatch1 = Stopwatch.StartNew();
            string xmlData = "";
            MemoryStream memory = new MemoryStream();

            int tenant = exportToExcelArgs.Tenant;
            byte[] xmlFilters = exportToExcelArgs.XmlFilters;
            string typename = exportToExcelArgs.TypeName;

            FilterSerializer filterSerializer = new FilterSerializer();
            QueryRepository queryRep = new QueryRepository(tenant);
            QueryColumnRepository queryColumnRep = new QueryColumnRepository(tenant);
            QueryQuery queryQuery = new QueryQuery(queryRep);
            QueryPM query = exportToExcelArgs.QueryPM != null ? exportToExcelArgs.QueryPM : queryQuery.GetSingleQueryPM(exportToExcelArgs.QueryCode, tenant);
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(queryColumnRep);
            List<QueryColumnPM> queryColumns = exportToExcelArgs.QueryColumns != null ?
                                                exportToExcelArgs.QueryColumns :
                                                queryColumnQuery.GetQueryColumnsByQueryCodeAndUser(tenant, exportToExcelArgs.UserId, query.UniqueCode)
                                                .OrderBy(q => q.IndexOrder)
                                                .ToList();

            if (exportToExcelArgs.QueryColumns == null)
            {
                if (queryColumns.Count == 0)
                {
                    queryColumns = queryColumnQuery.GetQueryColumnsByQueryCodeAndUser(0, exportToExcelArgs.UserId, query.UniqueCode).OrderBy(q => q.IndexOrder).ToList();
                }

                if (queryColumns.Count == 0)
                {
                    queryColumns = queryColumnQuery.GetZeroQueryColumnsByQueryCode(0, query.UniqueCode).OrderBy(q => q.IndexOrder).ToList();
                }
            }

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            queryOperations.GetAll = true;
            queryOperations.PageIndex = 0;
            queryOperations.GetAll = true;
            queryOperations.UserId = exportToExcelArgs.UserId;
            MethodInfo getListMethodInfo = null;
            MethodInfo getCountMethodInfo = null;
            object context = null;

            if (typename != null)
            {
                Type contextType = Type.GetType(typename.Replace("Context", "Service"));

                context = Activator.CreateInstance(contextType);

                DomainServiceContext con = new DomainServiceContext(new MockServiceProvider(), DomainOperationType.Query);
                MethodInfo methodInfo = context.GetType().GetMethod("Initialize");
                object[] parameters1 = new object[] { con };
                methodInfo.Invoke(context, parameters1);

                //ShipmentFollowUp
                switch (query.QuerySection)
                {
                    case "ShipmentFollowUp":
                        {
                            getListMethodInfo = context.GetType().GetMethod("GetFollowUpsByShipmentsFilter");
                            getCountMethodInfo = context.GetType().GetMethod("GetFollowUpsByShipmentsFilterCount");

                            break;
                        }
                    case "QuoteFollowUp":
                        {
                            getListMethodInfo = context.GetType().GetMethod("GetFollowUpsByQuotesFilter");
                            getCountMethodInfo = context.GetType().GetMethod("GetFollowUpsByQuotesFilterCount");
                            break;
                        }
                    default:
                        {
                            getListMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "Filters") : context.GetType().GetMethod("Get" + query.ObjectTableName + "Filters");
                            getCountMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + query.ObjectTableName + "FiltersCount");
                            if (getCountMethodInfo == null)
                            {
                                getCountMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + query.ObjectTableName + "Count");

                            }

                            break;
                        }
                }

            }
            else
            {
                var MethodsInfo = getMethodsInfo("WebFreight.Web.ShipmentsModel.DomainServices.ShipmentsDomainService", query);
                bool stop = false;
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.CommonDataDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.InfrastructureModel.DomainServices.WebFreightDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.QuoteModel.DomainServices.QuotesDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.GlobalModel.GlobalDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.ContactDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.PartnersDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.PaymentTermDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.BookingModel.DomainServices.BookingsDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.InvoiceModel.DomainServices.InvoiceDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CustomModel.DomainServices.CustomDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CustomModel.DomainServices.DeclarationDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CustomModel.DomainServices.ClaimDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CRMModel.DomainServices.CRMDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.CRMModel.DomainServices.EmployeeGroupDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices.AccountingDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices.ARPaymentChequeDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.WarehouseModel.DomainServices.WarehousesDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.TariffModel.DomainServices.TariffDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.WorkflowModel.DomainServices.WorkFlowDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.WorkflowModel.DomainServices.WorkFlowInstanceDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.InfrastructureModel.DomainServices.InfrastructureDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices.TaxReportDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices.TaxDeductionReportDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

                if (stop == false)
                {
                    MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices.OpenFormatReportDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }
            }

            IQueryable querableEntities = null;

            if (getListMethodInfo != null && getCountMethodInfo != null)
            {
                //Get data count
                _Stopwatch = Stopwatch.StartNew();
                xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                object[] parameters = new object[] { xmlFilters, tenant };
                int count = 0;
                count = (int)getCountMethodInfo.Invoke(context, parameters);

                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "1 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                if (count > 0 || Logitude.Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("NXL", tenant))
                {
                    //Get dataList
                    if (exportToExcelArgs.IsXslxFormat)
                    {
                        queryOperations.PageSize = count;
                    }
                    else
                    {
                        int pagesize = count;
                        if (pagesize > 65534)
                        {
                            queryOperations.GetAll = false;
                            pagesize = 65000;
                        }
                        queryOperations.PageSize = pagesize;
                    }

                    xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                    parameters = new object[] { xmlFilters, tenant };
                    _Stopwatch = Stopwatch.StartNew();
                    LogTime("start queryResult at : ", exportToExcelArgs.IsXslxFormat, tenant);
                    var queryResult = getListMethodInfo.Invoke(context, parameters);
                    querableEntities = queryResult as IQueryable;
                    LogTime("stop queryResult at : ", exportToExcelArgs.IsXslxFormat, tenant);
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "2 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                    IEnumerator datalist = null;
                    if (querableEntities == null)
                    {
                        //var queryResult = getListMethodInfo.Invoke(context, parameters);
                        if (queryResult != null)
                        {
                            IList list = queryResult as IList;
                            if (list != null)
                            {
                                //querableEntities = list.AsQueryable(); 
                                datalist = list.GetEnumerator();
                            }

                        }
                    }
                    else
                    {
                        datalist = querableEntities.GetEnumerator();
                    }


                    if (datalist != null)
                    {
                        _Stopwatch = Stopwatch.StartNew();
                        //IEnumerator datalist = querableEntities.GetEnumerator();


                        if (exportToExcelArgs.IsXslxFormat)
                        {
                            return this.NpoiExcelGenerator(datalist, query, queryColumns, tenant);
                        }
                        else
                        {
                            xmlData = ConvertDataList2Xml(datalist, query, queryColumns, tenant);
                            LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "3 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
                            //The instantiation process consists of two steps.

                            //Step 1 : Instantiate the spreadsheet creation engine.
                            ExcelEngine excelEngine = new ExcelEngine();
                            //Step 2 : Instantiate the excel application object.
                            IApplication application = excelEngine.Excel;

                            //A new workbook is created.[Equivalent to creating a new workbook in MS Excel]
                            //The new workbook will have 5 worksheets
                            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
                            //The first worksheet object in the worksheets collection is accessed.
                            IWorksheet sheet = workbook.Worksheets[0];
                            //****************************** Creating excel from xml string *****************************

                            //sheet.Range["A2:H2"].Merge();
                            //sheet.Range["A1:P1"].Merge();
                            //sheet.Range["A1:H2"].Merge();
                            //sheet.Range["A2:H2"].CellStyle.FillBackground = ExcelKnownColors.LightGreen;

                            sheet.Range["A2:C2"].Merge();
                            sheet.Range["A2:C2"].Text = !string.IsNullOrEmpty(query.DisplayText) ? query.DisplayText : TextCodesTranslator.TranslateText(query.NameTextCodeCode, tenant);
                            sheet.Range["A2:C2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            sheet.Range["A2:C2"].CellStyle.Font.Bold = true;
                            sheet.Range["A2:C2"].CellStyle.Font.Color = ExcelKnownColors.Black;
                            sheet.Range["A2:C2"].CellStyle.Font.Size = 12;
                            sheet.Range["A2:C2"].CellStyle.Font.FontName = "Thoma";
                            sheet.Range["A2:Z2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                            sheet.Range["A3:Z3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                            if (!FeatureToggleHelper.HasFeatureToggle("CXE", tenant))
                            {
                                foreach (QueryColumnPM column in queryColumns)
                                {
                                    sheet.AutofitColumn(column.IndexOrder + 1);
                                }
                            }

                            XmlReader reader = XmlReader.Create(new StringReader(xmlData));
                            XmlDataDocument doc = new XmlDataDocument();
                            doc.Load(reader);

                            doc.GetElementsByTagName(query.ObjectTableName);
                            XmlNodeList entitiesList = doc.GetElementsByTagName(query.ObjectTableName);

                            if (entitiesList.Count == 0)
                            {
                                string ip = "";
                                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                                {
                                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                                    if (string.IsNullOrEmpty(currentIP))
                                    {
                                        currentIP = HttpContext.Current.Request.UserHostAddress;
                                    }
                                    ip = currentIP;
                                }
                                //ExceptionHandler.HandleException(new Exception("Error while building xml file, table columns are empty!"), DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ExcelExportService : ExportQueryToExcel Method", ip);
                                return null;
                            }

                            int[,] array = new int[,] { { 65, 0 } };
                            int start = 65;
                            _Stopwatch = Stopwatch.StartNew();
                            foreach (XmlNode node in entitiesList.Item(0).ChildNodes)
                            {
                                string nodename = TranslateTextsClass.Translate(node.Name, tenant);
                                QueryColumnPM column = queryColumns.Where(q => q.ObjectFieldListLabelTextCodeCode == node.Name || q.ObjectFieldFullNameTextCodeCode == node.Name).FirstOrDefault();
                                if (column != null)
                                {
                                    if (!string.IsNullOrEmpty(column.DisplayText))
                                    {

                                        nodename = column.DisplayText;
                                    }
                                }

                                nodename = nodename != null ? nodename : "";
                                nodename = nodename.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");

                                string sheetColumn = "";
                                if (array[0, 0] <= 90 && array[0, 1] == 0)
                                {
                                    char a = (char)array[0, 0];
                                    sheetColumn = a.ToString();
                                    array[0, 0]++;
                                }
                                else
                                {
                                    if (array[0, 1] == 0)
                                    {
                                        array[0, 0] = start;
                                        array[0, 1] = 65;

                                        sheet.Range["AA2:AZ2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                                        sheet.Range["AA3:AZ3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                                    }
                                    if (array[0, 1] <= 90)
                                    {
                                        string a = sheetColumn = ((char)array[0, 0]).ToString() + ((char)array[0, 1]).ToString();
                                        array[0, 1]++;
                                    }
                                    else
                                    {
                                        start += 1;
                                        array[0, 0] = start;
                                        array[0, 1] = 65;
                                        string a = sheetColumn = ((char)array[0, 0]).ToString() + ((char)array[0, 1]).ToString();
                                        array[0, 1]++;
                                        char startCharacter = Convert.ToChar(start);
                                        sheet.Range["AA2:" + startCharacter + "Z2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                                        sheet.Range["AA3:" + startCharacter + "Z3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                                    }

                                };

                                sheet.Range[sheetColumn.ToString() + "3"].Text = nodename;
                                IRange range = sheet.Range[sheetColumn + "3"];
                                //sheet.Range.NumberFormat = "yyyy-mm-dd;@";
                                range.CellStyle.Font.FontName = "Times New Roman";
                                range.CellStyle.Font.Bold = true;

                            }
                            LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "4 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                            //TenantRepository tenantRepoitory = new TenantRepository(tenant);
                            //var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
                            int cellRow = 4;
                            _Stopwatch = Stopwatch.StartNew();
                            TenantRepository tenantRepoitory = new TenantRepository(tenant);
                            var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
                            LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "5 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                            _Stopwatch = Stopwatch.StartNew();
                            foreach (XmlNode node in entitiesList)
                            {
                                int cellCol = 1;
                                foreach (XmlNode childNode in node.ChildNodes)
                                {

                                    QueryColumnPM column = queryColumns.Where(q => q.ObjectFieldListLabelTextCodeCode == childNode.Name || q.ObjectFieldFullNameTextCodeCode == childNode.Name).FirstOrDefault();

                                    switch (column.ObjectFieldDataTypeCode)
                                    {
                                        case "Text":
                                            {
                                                sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                                break;
                                            }

                                        case "Boolean":
                                            {
                                                Boolean b = false;
                                                Boolean.TryParse(childNode.InnerText.Trim(), out b);
                                                sheet.Range[cellRow, cellCol].Boolean = b;
                                                break;
                                            }

                                        case "Constant":
                                            {
                                                sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                                break;
                                            }

                                        case "DateTime":
                                            {
                                                DateTime date;
                                                if (DateTime.TryParse(childNode.InnerText.Trim(), out date))
                                                {
                                                    sheet.Range[cellRow, cellCol].DateTime = date.Date;
                                                    string datetimeformat = @"dd\/MM\/yyyy";
                                                    if (!string.IsNullOrEmpty(CurTenant.DateTimeFormat))
                                                    {
                                                        datetimeformat = CurTenant.DateTimeFormat;
                                                    }
                                                    sheet.Range[cellRow, cellCol].NumberFormat = datetimeformat;
                                                }
                                                else
                                                {
                                                    sheet.Range[cellRow, cellCol].Text = "";
                                                }
                                                break;
                                            }

                                        case "Decimal":
                                            {
                                                double dex = 0;
                                                double.TryParse(childNode.InnerText.Trim(), out dex);
                                                sheet.Range[cellRow, cellCol].Number = dex;
                                                break;
                                            }

                                        case "SigDouble":
                                        case "Double":
                                            {
                                                double d = 0;
                                                double.TryParse(childNode.InnerText.Trim(), out d);
                                                sheet.Range[cellRow, cellCol].Number = d;
                                                break;
                                            }

                                        case "Integer":
                                            {
                                                int x = 0;
                                                int.TryParse(childNode.InnerText.Trim(), out x);
                                                sheet.Range[cellRow, cellCol].Number = x;
                                                break;
                                            }

                                        default:
                                            {
                                                sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                                break;
                                            }
                                    }

                                    //sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                    //if (cellCol <= 2)
                                    //    sheet.Range[cellRow, cellCol].ColumnWidth = 30;



                                    cellCol++;
                                }
                                cellRow++;


                            }
                            LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "6 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                            //***************************************************************************
                            //Inserting sample text into the range of cells of the first worksheet.
                            //sheet.Range["A1:N30"].Text = "Hello World";

                            //Saving the workbook to disk.


                            if (FeatureToggleHelper.HasFeatureToggle("CXE", tenant))
                            {
                                int i = 1;

                                foreach (QueryColumnPM _ in queryColumns)
                                {
                                    sheet.AutofitColumn(i);
                                    if (_.ObjectFieldName == "CustomerReference" && sheet.GetColumnWidth(i) > 30)
                                        sheet.SetColumnWidth(i, 30);
                                    i++;
                                }
                            }
                            _Stopwatch = Stopwatch.StartNew();
                            if (exportToExcelArgs.IsXslxFormat)
                            {
                                workbook.Version = ExcelVersion.Excel2010;
                                workbook.SaveAs(memory);
                            }
                            else
                            {
                                workbook.SaveAs(memory, ExcelSaveType.SaveAsXLS);
                            }
                            LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "7 Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                            //No exception will be thrown if there are unsaved workbooks.
                            excelEngine.ThrowNotSavedOnDestroy = false;
                            excelEngine.Dispose();
                        }
                    }


                }
            }
            //}
            //catch (Exception e)
            //{
            //    string ip = "";
            //    if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //    {
            //        ip = HttpContext.Current.Request.UserHostAddress;
            //    }
            //    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ExcelExportService : ExportQueryToExcel Method", ip);
            //}
            LogTime("stop all  at : ", exportToExcelArgs.IsXslxFormat, tenant);


            LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "8 allTheFunction Took:" + _Stopwatch1.Elapsed.ToString()); _Stopwatch1.Restart();

            return memory.ToArray();
        }

        public byte[] ExportDataToExcel(ExportToExcelArgs args)
        {
            MemoryStream memory = new MemoryStream();
            var data = args.Data;
            if (data != null)
            {
                string xmlData = ConvertDataList2Xml(data, args.QueryPM, args.QueryColumns, args.Tenant);

                ExcelEngine excelEngine;
                IWorksheet sheet = InitializeExcelSheet(args, args.QueryPM, out excelEngine, out IWorkbook workbook);

                XmlNodeList entitiesList = GetEntitiesByXML(args, xmlData);

                if (entitiesList.Count == 0)
                    return null;

                FillDataOnExcelCells(args, sheet, entitiesList);

                SaveExcelFileToMemoreyStream(memory, workbook);
                excelEngine.ThrowNotSavedOnDestroy = false;
                excelEngine.Dispose();
            }

            return memory.ToArray();
        }

        private IWorksheet InitializeExcelSheet(ExportToExcelArgs args, QueryPM dummyQuery, out ExcelEngine excelEngine, out IWorkbook workbook)
        {
            IWorksheet sheet;
            sheet = InitializeExcelFile(out excelEngine, out workbook);
            CreateSheetHeader(dummyQuery, sheet);
            return sheet;
        }

        private static void SaveExcelFileToMemoreyStream(MemoryStream memory, IWorkbook workbook)
        {
            workbook.SaveAs(memory, ExcelSaveType.SaveAsXLS);

        }

        private static void FillDataOnExcelCells(ExportToExcelArgs args, IWorksheet sheet, XmlNodeList entitiesList)
        {
            int[,] array = new int[,] { { 65, 0 } };
            int start = 65;
            foreach (XmlNode node in entitiesList.Item(0).ChildNodes)
            {
                string nodename = TranslateTextsClass.Translate(node.Name, args.Tenant);
                QueryColumnPM column = args.QueryColumns.Where(q => q.ObjectFieldListLabelTextCodeCode == node.Name || q.ObjectFieldFullNameTextCodeCode == node.Name).FirstOrDefault();

                if (column != null)
                {
                    if (!string.IsNullOrEmpty(column.DisplayText))
                    {

                        nodename = column.DisplayText;
                    }
                }

                nodename = nodename != null ? nodename : "";
                nodename = nodename.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");

                string sheetColumn = "";
                if (array[0, 0] <= 90 && array[0, 1] == 0)
                {
                    char a = (char)array[0, 0];
                    sheetColumn = a.ToString();
                    array[0, 0]++;
                }
                else
                {
                    if (array[0, 1] == 0)
                    {
                        array[0, 0] = start;
                        array[0, 1] = 65;

                        sheet.Range["AA2:AZ2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                        sheet.Range["AA3:AZ3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                    }
                    if (array[0, 1] <= 90)
                    {
                        string a = sheetColumn = ((char)array[0, 0]).ToString() + ((char)array[0, 1]).ToString();
                        array[0, 1]++;
                    }
                    else
                    {
                        start += 1;
                        array[0, 0] = start;
                        array[0, 1] = 65;
                        string a = sheetColumn = ((char)array[0, 0]).ToString() + ((char)array[0, 1]).ToString();
                        array[0, 1]++;
                        char startCharacter = Convert.ToChar(start);
                        sheet.Range["AA2:" + startCharacter + "Z2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                        sheet.Range["AA3:" + startCharacter + "Z3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    }

                };

                sheet.Range[sheetColumn.ToString() + "3"].Text = nodename;
                IRange range = sheet.Range[sheetColumn + "3"];
                //sheet.Range.NumberFormat = "yyyy-mm-dd;@";
                range.CellStyle.Font.FontName = "Times New Roman";
                range.CellStyle.Font.Bold = true;

            }


            int cellRow = 4;
            TenantRepository tenantRepoitory = new TenantRepository(args.Tenant);
            var CurTenant = tenantRepoitory.GetSingleByTenant(args.Tenant);
            foreach (XmlNode node in entitiesList)
            {
                int cellCol = 1;
                foreach (XmlNode childNode in node.ChildNodes)
                {

                    QueryColumnPM column = args.QueryColumns.Where(q => q.ObjectFieldListLabelTextCodeCode == childNode.Name || q.ObjectFieldFullNameTextCodeCode == childNode.Name).FirstOrDefault();

                    switch (column.ObjectFieldDataTypeCode)
                    {
                        case "Text":
                            {
                                sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                break;
                            }

                        case "Boolean":
                            {
                                Boolean b = false;
                                Boolean.TryParse(childNode.InnerText.Trim(), out b);
                                sheet.Range[cellRow, cellCol].Boolean = b;
                                break;
                            }

                        case "Constant":
                            {
                                sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                break;
                            }

                        case "DateTime":
                            {
                                DateTime date;
                                if (DateTime.TryParse(childNode.InnerText.Trim(), out date))
                                {
                                    sheet.Range[cellRow, cellCol].DateTime = date.Date;
                                    string datetimeformat = @"dd\/MM\/yyyy";
                                    if (!string.IsNullOrEmpty(CurTenant.DateTimeFormat))
                                    {
                                        datetimeformat = CurTenant.DateTimeFormat;
                                    }
                                    sheet.Range[cellRow, cellCol].NumberFormat = datetimeformat;
                                }
                                else
                                {
                                    sheet.Range[cellRow, cellCol].Text = "";
                                }
                                break;
                            }

                        case "Decimal":
                            {
                                double dex = 0;
                                double.TryParse(childNode.InnerText.Trim(), out dex);
                                sheet.Range[cellRow, cellCol].Number = dex;
                                break;
                            }

                        case "SigDouble":
                        case "Double":
                            {
                                double d = 0;
                                double.TryParse(childNode.InnerText.Trim(), out d);
                                sheet.Range[cellRow, cellCol].Number = d;
                                break;
                            }

                        case "Integer":
                            {
                                int x = 0;
                                int.TryParse(childNode.InnerText.Trim(), out x);
                                sheet.Range[cellRow, cellCol].Number = x;
                                break;
                            }

                        default:
                            {
                                sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                                break;
                            }
                    }

                    //sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                    //if (cellCol <= 2)
                    //    sheet.Range[cellRow, cellCol].ColumnWidth = 30;



                    cellCol++;
                }
                cellRow++;


            }
        }

        private static XmlNodeList GetEntitiesByXML(ExportToExcelArgs args, string xmlData)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xmlData));
            XmlDataDocument xmlDocument = new XmlDataDocument();
            xmlDocument.Load(reader);

            xmlDocument.GetElementsByTagName(args.QueryPM.ObjectTableName);
            XmlNodeList entitiesList = xmlDocument.GetElementsByTagName(args.QueryPM.ObjectTableName);
            return entitiesList;
        }

        private static void CreateSheetHeader(QueryPM dummyQuery, IWorksheet sheet)
        {
            sheet.Range["A2:C2"].Merge();
            sheet.Range["A2:C2"].Text = !string.IsNullOrEmpty(dummyQuery.DisplayText) ? dummyQuery.DisplayText : TextCodesTranslator.TranslateText(dummyQuery.NameTextCodeCode, dummyQuery.Tenant);
            sheet.Range["A2:C2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet.Range["A2:C2"].CellStyle.Font.Bold = true;
            sheet.Range["A2:C2"].CellStyle.Font.Color = ExcelKnownColors.Black;
            sheet.Range["A2:C2"].CellStyle.Font.Size = 12;
            sheet.Range["A2:C2"].CellStyle.Font.FontName = "Thoma";
            sheet.Range["A2:Z2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
            sheet.Range["A3:Z3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
        }

        private IWorksheet InitializeExcelFile(out ExcelEngine excelEngine, out IWorkbook workbook)
        {
            excelEngine = new ExcelEngine();
            workbook = excelEngine.Excel.Workbooks.Create(1);
            IWorksheet sheet = workbook.Worksheets[0];

            return sheet;
        }

        private string ConvertDataList2Xml(IEnumerator dataList, QueryPM query, List<QueryColumnPM> queryColumns, int tenant)
        {

            TextCodeRepository textCodeRepoitory = new TextCodeRepository(tenant);
            TenantRepository tenantRepoitory = new TenantRepository(tenant);
            var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
            System.Xml.Linq.XElement entities = new System.Xml.Linq.XElement(query.ObjectTableName + "s");
            try
            {
                int datacount = 0;

                if (dataList != null)
                {
                    while (dataList.MoveNext())
                    {
                        object entity = dataList.Current;
                        System.Xml.Linq.XElement table = new System.Xml.Linq.XElement(query.ObjectTableName);

                        foreach (QueryColumnPM column in queryColumns)
                        {
                            string text = !string.IsNullOrWhiteSpace(column.ObjectFieldListLabelTextCodeCode)
                                          ? column.ObjectFieldListLabelTextCodeCode
                                          : column.ObjectFieldFullNameTextCodeCode;

                            System.Xml.Linq.XElement col = new System.Xml.Linq.XElement(text);
                            string value = " ";

                            if (query.EditWizardName == "LogBoxMainComponent")
                            {
                                value = ResoloveLogBoxShipmentFieldValue(entity, column);
                            }
                            else
                            {
                                PropertyInfo info = entity.GetType().GetProperty(column.ObjectFieldName);
                                if (info != null)
                                {
                                    value = info.GetValue(entity, null) != null ? info.GetValue(entity, null).ToString() : " ";
                                }
                            }

                            var temp = Fix(value);
                            col.Value = temp;

                            table.Add(col);

                        }

                        entities.Add(table);

                        datacount++;
                    }
                }

                if (dataList == null || datacount == 0)
                {
                    System.Xml.Linq.XElement table = new System.Xml.Linq.XElement(query.ObjectTableName);

                    foreach (QueryColumnPM column in queryColumns)
                    {
                        string text = !string.IsNullOrWhiteSpace(column.ObjectFieldListLabelTextCodeCode) ? column.ObjectFieldListLabelTextCodeCode : column.ObjectFieldFullNameTextCodeCode;
                        System.Xml.Linq.XElement col = new System.Xml.Linq.XElement(text);
                        string value = " ";
                        col.Value = value;
                        table.Add(col);
                    }


                    entities.Add(table);
                }
            }
            catch (Exception ex)
            {

            }

            return entities.ToString();
        }

        private string ResoloveLogBoxShipmentFieldValue(object entity, QueryColumnPM column)
        {
            string value = string.Empty;
            if (column.ObjectFieldName == "Task") value = GetLogBoxTaskFieldValue(entity);
            else if (column.ObjectFieldName == "CustomerReference")
            {
                string customerReference1 = GetPropertyValue(entity, "CustomerReference1");
                string customerReference2 = GetPropertyValue(entity, "CustomerReference2");
                if (!string.IsNullOrEmpty(customerReference1)) value = customerReference1;
                if (string.IsNullOrEmpty(customerReference1) && !string.IsNullOrEmpty(customerReference2)) value += customerReference2;
                if (!string.IsNullOrEmpty(customerReference1) && !string.IsNullOrEmpty(customerReference2)) value += " / " + customerReference2;
            }
            else if (column.ObjectFieldName.Contains("ShipmentNumber_"))
            {
                if (column.ObjectFieldName.Split('_')[1] == "MyShipments") value = GetPropertyValue(entity, "CustomerReference1");
                else value = GetPropertyValue(entity, "ForwarderShipmentNumber");
            }
            else
            {
                PropertyInfo info = entity.GetType().GetProperty(column.ObjectFieldName);
                if (info != null)
                {
                    value = info.GetValue(entity, null) != null ? info.GetValue(entity, null).ToString() : " ";
                }
            }


            return string.IsNullOrEmpty(value) ? " " : value;
        }

        private string GetLogBoxTaskFieldValue(object entity)
        {
            string value = string.Empty;
            if (GetPropertyValue(entity, "IsRequestedDocuments") == "True" || (!string.IsNullOrEmpty(GetPropertyValue(entity, "RequestedDocumentsCount")) && GetPropertyValue(entity, "RequestedDocumentsCount") != "0"))
            {
                value = (!string.IsNullOrEmpty(value) ? (value + " \\ Requested Document") : "Requested Document");
            }

            if (GetPropertyValue(entity, "IsImporterApprovalRequried") == "True")
            {
                value = (!string.IsNullOrEmpty(value) ? (value + " \\ Declaration Approval") : "Declaration Approval");
            }


            if (GetPropertyValue(entity, "IsDigitalSignRequired") == "True")
            {
                value = (!string.IsNullOrEmpty(value) ? (value + " \\ Sign Required") : "Sign Required");
            }

            if (GetPropertyValue(entity, "IsDepositionRequired") == "True")
            {
                value = (!string.IsNullOrEmpty(value) ? (value + " \\ Deposition Required") : "Deposition Required");
            }

            return value;
        }

        private static string GetPropertyValue(object entity, string fieldName)
        {
            string result = string.Empty;
            PropertyInfo info = entity.GetType().GetProperty(fieldName);
            if (info != null)
            {
                result = info.GetValue(entity, null) != null ? info.GetValue(entity, null).ToString() : " ";

            }

            return (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result)) ? "" : result;
        }

        private static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.  
            String ret = Regex.Replace(fileName.Trim(), "[^א-תA-Za-z0-9_. ]+", "");
            return ret.Replace(" ", String.Empty);
        }
        Lazy<Regex> ControlChars = new Lazy<Regex>(() => new Regex("[\x00-\x1f]", RegexOptions.Compiled));

        private string FixData_Replace(Match match)
        {
            if ((match.Value.Equals("\t")) || (match.Value.Equals("\n")) || (match.Value.Equals("\r")))
                return match.Value;

            return "&#" + ((int)match.Value[0]).ToString("X4") + ";";
        }

        public string Fix(object data, MatchEvaluator replacer = null)
        {
            if (data == null) return null;
            string fixed_data;
            if (replacer != null) fixed_data = ControlChars.Value.Replace(data.ToString(), replacer);
            else fixed_data = ControlChars.Value.Replace(data.ToString(), FixData_Replace);
            return fixed_data;
        }

        private ReflectionProperties getMethodsInfo(string ContextName, QueryPM query)
        {
            Type contextType = Type.GetType(ContextName);

            object context = Activator.CreateInstance(contextType);

            DomainServiceContext con = new DomainServiceContext(new MockServiceProvider(), DomainOperationType.Query);
            MethodInfo methodInfo = context.GetType().GetMethod("Initialize");
            object[] parameters1 = new object[] { con };
            methodInfo.Invoke(context, parameters1);

            MethodInfo getListMethodInfo = null;
            MethodInfo getCountMethodInfo = null;
            switch (query.QuerySection)
            {
                case "ShipmentLogBox":
                    {
                        getListMethodInfo = context.GetType().GetMethod("GetLogBoxShipmentFilters");
                        getCountMethodInfo = context.GetType().GetMethod("GetLogBoxShipmentFiltersCount");

                        break;
                    }
                case "ShipmentFollowUp":
                    {
                        getListMethodInfo = context.GetType().GetMethod("GetFollowUpsByShipmentsFilter");
                        getCountMethodInfo = context.GetType().GetMethod("GetFollowUpsByShipmentsFilterCount");

                        break;
                    }
                case "QuoteFollowUp":
                    {
                        getListMethodInfo = context.GetType().GetMethod("GetFollowUpsByQuotesFilter");
                        getCountMethodInfo = context.GetType().GetMethod("GetFollowUpsByQuotesFilterCount");
                        break;
                    }
                default:
                    {
                        getListMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "Filters") : context.GetType().GetMethod("Get" + query.ObjectTableName + "Filters");
                        getCountMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + query.ObjectTableName + "FiltersCount");
                        if (getCountMethodInfo == null)
                        {
                            getCountMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + query.ObjectTableName + "Count");

                        }

                        break;
                    }
            }

            ReflectionProperties ReturnData = null;
            if (getListMethodInfo != null && getCountMethodInfo != null)
            {
                ReturnData = new ReflectionProperties();
                ReturnData.ListMethodInfo = getListMethodInfo;
                ReturnData.CountMethodInfo = getCountMethodInfo;
                ReturnData.context = context;
            }
            return ReturnData;
        }

        // Method to set cell value with length check
        public void SetCellValueWithMaxLength(ICell cell, string value)
        {
            if (value.Length > MaxCellLength)
            {
                value = value.Substring(0, MaxCellLength);
            }
            cell.SetCellValue(value);
        }

        // Optionally, you can have a method to get the max cell length
        public int GetMaxCellLength()
        {
            return MaxCellLength;
        }
        private byte[] NpoiExcelGenerator(IEnumerator dataList, QueryPM query, List<QueryColumnPM> queryColumns, int tenant)
        {
            LogTime("start NpoiExcelGenerator Func at : ", true);

            MemoryStream ms = new MemoryStream();
            TextCodeRepository textCodeRepoitory = new TextCodeRepository(tenant);
            TenantRepository tenantRepoitory = new TenantRepository(tenant);
            var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
            string queryName = !string.IsNullOrEmpty(query.DisplayText) ? query.DisplayText : TranslateTextsClass.Translate(query.NameTextCodeCode, tenant).Replace(" ", "_") + "_" + query.ObjectTableName + "s";

            queryName = ExportToExcelHelper.GetValidFileName(queryName);//queryName.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("'", "");
            queryName = queryName.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("'", "");

            if (queryName.Length > 31)
                queryName = queryName.Substring(0, 31);

            try
            {
                int datacount = 0;

                if (dataList != null)


                {


                    var workbook = new XSSFWorkbook();

                    #region Styles

                    var DataHeaderCellFont = workbook.CreateFont();
                    DataHeaderCellFont.Color = IndexedColors.Black.Index;
                    DataHeaderCellFont.IsBold = true;
                    DataHeaderCellFont.FontHeightInPoints = 12;
                    DataHeaderCellFont.FontName = "Thoma";

                    var QueryNameHeaderCellFont = workbook.CreateFont();
                    //QueryNameHeaderCellFont.Color = IndexedColors.Black.Index;
                    QueryNameHeaderCellFont.IsBold = true;
                    QueryNameHeaderCellFont.FontHeightInPoints = 10;
                    QueryNameHeaderCellFont.FontName = "Times New Roman";


                    var DataCellFont = workbook.CreateFont();
                    DataCellFont.FontHeightInPoints = 10;
                    DataCellFont.FontName = "Thoma";


                    var DataHeaderCellFontStyle = workbook.CreateCellStyle();
                    DataHeaderCellFontStyle.SetFont(DataHeaderCellFont);
                    //DataHeaderCellFontStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                    DataHeaderCellFontStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                    DataHeaderCellFontStyle.FillPattern = FillPattern.SolidForeground;

                    DataHeaderCellFontStyle.Alignment = HorizontalAlignment.Center;


                    var QueryNameHeaderCellFontStyle = workbook.CreateCellStyle();
                    QueryNameHeaderCellFontStyle.SetFont(QueryNameHeaderCellFont);
                    QueryNameHeaderCellFontStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                    QueryNameHeaderCellFontStyle.FillPattern = FillPattern.SolidForeground;

                    var DataCellFontStyle = workbook.CreateCellStyle();
                    DataCellFontStyle.SetFont(DataCellFont);


                    #endregion



                    var sheet = workbook.CreateSheet(queryName);
                    sheet.SetColumnWidth(0, 1000);
                    int WorkingRowIndex = 1;
                    ICell cell;
                    IRow row;
                    #region header

                    row = sheet.CreateRow(WorkingRowIndex);
                    CellRangeAddress region = new CellRangeAddress(WorkingRowIndex, WorkingRowIndex, 0, queryColumns.Count - 1);

                    sheet.AddMergedRegion(region);


                    cell = row.CreateCell(0);
                    cell.CellStyle = DataHeaderCellFontStyle;

                    cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                    var Text = !string.IsNullOrEmpty(query.DisplayText) ? query.DisplayText : TextCodesTranslator.TranslateText(query.NameTextCodeCode, tenant);
                    SetCellValueWithMaxLength(cell, Text);
                    WorkingRowIndex++;

                    #endregion
                    var i = 0;
                    row = sheet.CreateRow(WorkingRowIndex);
                    foreach (QueryColumnPM column in queryColumns)
                    {

                        string text = !string.IsNullOrWhiteSpace(column.ObjectFieldListLabelTextCodeCode)
                                      ? column.ObjectFieldListLabelTextCodeCode
                                     : column.ObjectFieldFullNameTextCodeCode;


                        if (!string.IsNullOrEmpty(column.DisplayText))
                        {

                            text = column.DisplayText;
                        }
                        else if (!string.IsNullOrEmpty(column.ObjectFieldFieldLableTextCodeDefaultText))
                        {
                            text = column.ObjectFieldFieldLableTextCodeDefaultText;
                        }


                        text = text != null ? text : "";
                        text = text.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");


                        if (!FeatureToggleHelper.HasFeatureToggle("CXE", tenant))
                        {
                            sheet.AutoSizeColumn(column.IndexOrder + 1);
                        }
                        sheet.SetColumnWidth(i, 20 * 256);
                        cell = row.CreateCell(i);
                        cell.CellStyle = QueryNameHeaderCellFontStyle;

                        cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                        SetCellValueWithMaxLength(cell, text);
                        i++;

                    }

                    while (dataList.MoveNext())
                    {
                        var a = dataList.Current;

                        WorkingRowIndex++;

                        row = sheet.CreateRow(WorkingRowIndex);


                        i = 0;


                        foreach (QueryColumnPM column in queryColumns)
                        {
                            object value = null;


                            cell = row.CreateCell(i);
                            cell.CellStyle = DataCellFontStyle;
                            sheet.SetColumnWidth(i, 20 * 256);


                            var b = column.ObjectFieldName.ToString();
                            PropertyInfo info = a.GetType().GetProperty(b);
                            if (info != null)
                            {
                                value = info.GetValue(a, null) != null ? info.GetValue(a, null) : null;
                            }

                            if (column.ObjectFieldDataTypeCode == "DateTime" && value != null)
                            {

                                value = ((DateTime)value).ToString("dd/MM/yyyy");

                            }

                            cell.SetCellType(GetCellType(column.ObjectFieldDataTypeCode));

                            //value = value != null ? value : "";
                            //if (value != null)
                            //    value = value.ToString();
                            if (value != null)
                            {
                                if (GetCellType(column.ObjectFieldDataTypeCode) == CellType.Numeric)
                                {

                                    if (value is double || value is int || value is float || value is decimal || value is long ||
                                        value is double? || value is int? || value is float? || value is decimal? || value is long?)
                                    {
                                        cell.SetCellValue(Convert.ToDouble(value));
                                    }

                                    else
                                    {
                                        SetCellValueWithMaxLength(cell, value.ToString());
                                    }

                                }
                                else
                                    SetCellValueWithMaxLength(cell, value.ToString());
                            }
                            else
                            {

                            }
                            i++;
                        }


                    }

                    workbook.Write(ms);


                    GC.Collect();


                }
            }
            catch (Exception ex)
            {

            }

            LogTime("Stop NpoiExcelGenerator Func at : ", true);

            return ms.ToArray();

        }

        private static CellType GetCellType(string ObjectFieldDataTypeCode)
        {
            switch (ObjectFieldDataTypeCode)
            {
                case "Text":
                case "Constant":
                case "DateTime":
                    {
                        return CellType.String;
                    }

                case "Boolean":
                    {
                        return CellType.Boolean;
                    }


                case "Decimal":
                case "SigDouble":
                case "Double":
                case "Integer":
                    {
                        return CellType.Numeric;
                    }


                default:
                    {
                        return CellType.String;
                    }
            }
        }
        private void LogTime(string msg, bool isXslx, int tenant = 0)
        {


            if (Logitude.Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("NXL", tenant) && isXslx)
            {
                //DateTime stopLogAt = DateTime.MinValue;
                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20230601T000000.LogUntilDateyyyyMMdd"];
                //if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                //stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                msg += DateTime.Now.ToString();
                DateTime date = new DateTime(2023, 06, 01);
                LogitudeSettings.HandleLogMe(msg, false, "ExportToExcel", date);
            }

        }
    }
}
class ReflectionProperties
{
    public MethodInfo ListMethodInfo { get; set; }
    public MethodInfo CountMethodInfo { get; set; }
    public object context { get; set; }
}
public class ExcelTotals
{
    public ExcelTotals(string fieldCode, double total, int indexOrder)
    {
        FieldCode = fieldCode;
        Total = total;
        IndexOrder = indexOrder;
    }
    public string FieldCode { get; set; }
    public double Total { get; set; }
    public int IndexOrder { get; set; }
}

public class ExportToExcelArgs
{
    public byte[] XmlFilters { get; set; }
    public string QueryCode { get; set; }
    public int Tenant { get; set; }
    public string UserId { get; set; }
    public string TypeName { get; set; }
    public bool RunOnDigitalWorker { get; set; }
    public QueryPM QueryPM { get; set; }
    public List<QueryColumnPM> QueryColumns { get; set; }
    public IEnumerator Data { get; set; }
    public bool IsXslxFormat { get; set; }
}
