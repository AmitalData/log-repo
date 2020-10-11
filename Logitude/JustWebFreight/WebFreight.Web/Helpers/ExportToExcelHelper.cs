using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.DomainServices.Server;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityPMs;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools;
using System.Drawing;

namespace WebFreight.Web.Helpers
{
    //public class ExportToExcelHelper
    public partial class ExportToExcelHelper
    {
        public byte[] ExportQueryToExcel(ExportToExcelArgs exportToExcelArgs)
        {
            string xmlData = "";
            System.IO.MemoryStream memory = new System.IO.MemoryStream();

            int tenant = exportToExcelArgs.Tenant;
            byte[] xmlFilters = exportToExcelArgs.XmlFilters;
            string typename = exportToExcelArgs.TypeName;

             FilterSerializer filterSerializer = new FilterSerializer();
            QueryRepository queryRep = new QueryRepository(tenant);
            QueryColumnRepository queryColumnRep = new QueryColumnRepository(tenant);
            QueryQuery queryQuery = new QueryQuery(queryRep);
            QueryPM query = exportToExcelArgs.QueryPM!=null ? exportToExcelArgs.QueryPM: queryQuery.GetSingleQueryPM(exportToExcelArgs.QueryCode, tenant);
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(queryColumnRep);
            List<QueryColumnPM> queryColumns = exportToExcelArgs.QueryColumns!=null ? exportToExcelArgs.QueryColumns :queryColumnQuery.GetQueryColumnsByQueryCodeAndUser(tenant, exportToExcelArgs.UserId, query.UniqueCode).OrderBy(q => q.IndexOrder).ToList();

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
                    MethodsInfo = getMethodsInfo("WebFreight.Web.WarehouseModel.DomainServices.WarehousesDomainService", query);
                    if (MethodsInfo != null)
                    {
                        getListMethodInfo = MethodsInfo.ListMethodInfo;
                        getCountMethodInfo = MethodsInfo.CountMethodInfo;
                        context = MethodsInfo.context;
                        stop = true;
                    }
                }

            }

            System.Linq.IQueryable querableEntities = null;

            if (getListMethodInfo != null && getCountMethodInfo != null)
            {
                //Get data count
                xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                object[] parameters = new object[] { xmlFilters, tenant };
                int count = (int)getCountMethodInfo.Invoke(context, parameters);

                // Get dataList
                int pagesize = count;
                if (pagesize > 65534)
                {
                    queryOperations.GetAll = false;
                    pagesize = 65000;
                }
                queryOperations.PageSize = pagesize;
                xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                parameters = new object[] { xmlFilters, tenant };

                var queryResult = getListMethodInfo.Invoke(context, parameters);
                querableEntities = queryResult as IQueryable;
                IEnumerator datalist =null;
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

                    //IEnumerator datalist = querableEntities.GetEnumerator();
                    xmlData = ConvertDataList2Xml(datalist, query, queryColumns, tenant);


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
                    sheet.Range["A2:C2"].Text =!string.IsNullOrEmpty(query.DisplayText) ? query.DisplayText : TextCodesTranslator.TranslateText(query.NameTextCodeCode, tenant);
                    sheet.Range["A2:C2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    sheet.Range["A2:C2"].CellStyle.Font.Bold = true;
                    sheet.Range["A2:C2"].CellStyle.Font.Color = ExcelKnownColors.Black;
                    sheet.Range["A2:C2"].CellStyle.Font.Size = 12;
                    sheet.Range["A2:C2"].CellStyle.Font.FontName = "Thoma";
                    sheet.Range["A2:Z2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                    sheet.Range["A3:Z3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;


                    foreach (QueryColumnPM column in queryColumns)
                    {
                        sheet.AutofitColumn(column.IndexOrder + 1);

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
                        int start = 65;
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
                    //TenantRepository tenantRepoitory = new TenantRepository(tenant);
                    //var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
                    int cellRow = 4;
                    TenantRepository tenantRepoitory = new TenantRepository(tenant);
                    var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
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


                    //***************************************************************************
                    //Inserting sample text into the range of cells of the first worksheet.
                    //sheet.Range["A1:N30"].Text = "Hello World";

                    //Saving the workbook to disk.

                    workbook.SaveAs(memory, ExcelSaveType.SaveAsXLS);

                    //No exception will be thrown if there are unsaved workbooks.
                    excelEngine.ThrowNotSavedOnDestroy = false;
                    excelEngine.Dispose();
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


            return memory.ToArray();
        }

        public byte[] ExportBIQueryToExcel(BIReportXMLData bIReportXMLData, int tenant)
        {
            BIReportQueryService query = new BIReportQueryService(tenant);
            BIReportPM biReportEntityPM = query.GetSingle(bIReportXMLData.BIReportId, false, false);
            ///////////////////////////////////////////////////////////

            DWQueryBuilderHelper QBHelper = new DWQueryBuilderHelper(tenant);
            SqlCommandDefinition sqlCommandDefinition = QBHelper.GetQuerySQL(bIReportXMLData.DWQueryData);
            DataTable dataTable = QBHelper.GetDWQueryData(sqlCommandDefinition);

            var bITabularViewSettings = LogitudeXmlSerializer.DeserializeObject<BITabularViewSettings>(biReportEntityPM.AGGridOptionsXML);
            List<string> MeasurmentColumns = null;
            List<ExcelTotals> excelTotals = new List<ExcelTotals>();
            if (bIReportXMLData.IncludeTotals)
            {
                MeasurmentColumns = bITabularViewSettings.Columns.Where(c => c.DataTypeCode == "Double" || c.DataTypeCode == "Decimal" || c.DataTypeCode == "Integer").Select(c => c.Code).ToList();
            }

            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
            workbook.Version = ExcelVersion.Excel2007;
            IWorksheet sheet = workbook.Worksheets[0];

            int count = dataTable.Columns.Count;
            List<DataColumn> deletedColumns = new List<DataColumn>();

            var columnNames = bITabularViewSettings.Columns.OrderBy(a => a.Index).Select(d => d.Code).ToList();
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                dataTable.Columns[columnName].SetOrdinal(columnIndex);

                if (bIReportXMLData.IncludeTotals)
                {
                    var measurmentColumn = MeasurmentColumns.Where(c => c == columnName).FirstOrDefault();
                    if(measurmentColumn != null) excelTotals.Add(new ExcelTotals(measurmentColumn, 0, columnIndex));
                }
                columnIndex++;
            }

            if (deletedColumns.Count > 0)
            {
                foreach (var item in deletedColumns)
                {
                    dataTable.Columns.Remove(item);
                }
            }
            sheet.ImportDataTable(dataTable, true, 1, 1);

            // sheet Format - Width 
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == dataTable.Columns[i].ColumnName).FirstOrDefault();
                if (agColumn != null)
                {
                    sheet.Columns[i].ColumnWidth = agColumn.Width / 7.5;
                }
            }

            TenantRepository tenantRepoitory = new TenantRepository(tenant);
            var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
            var rows = dataTable.Rows.Count;
            for (int j = 1; j <= dataTable.Columns.Count; j++)
            {
                var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == dataTable.Columns[j-1].ColumnName).FirstOrDefault();
                if (agColumn != null)
                {

                    var writeRange = sheet.Range[2,j, rows+1, j];
                    switch (agColumn.DataTypeCode)
                    {
                        case "Constant":
                        case "Text":
                            writeRange.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            break;
                        case "DateTime":
                            string datetimeformat = @"dd\/MM\/yyyy";
                            if (!string.IsNullOrEmpty(CurTenant.DateTimeFormat))
                            {
                                datetimeformat = CurTenant.DateTimeFormat;
                            }
                            writeRange.NumberFormat = datetimeformat;
                            break;
                        case "Decimal":
                        case "Double":
                            writeRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            writeRange.NumberFormat = "###,##0.00";
                            break;
                        case "Integer":
                            writeRange.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            writeRange.NumberFormat = "###,##";
                            break;

                        default:
                            writeRange.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            break;
                    }
                }
            }


            int cellRow = 2;
            foreach (DataRow row in dataTable.Rows)
            {
                int cellCol = 1;
                int excelTotalCount = 0;
                for (int j = 1; j <= dataTable.Columns.Count; j++)
                {
                    var agColumn = bITabularViewSettings.Columns.Where(a => a.Name == dataTable.Columns[j - 1].ColumnName).FirstOrDefault();
                    if (agColumn != null)
                    {
                        if (agColumn.DataTypeCode == "Text")
                        {
                            string value = Convert.ToString(row[agColumn.Name]);
                            sheet.Range[cellRow, cellCol].Text = value;
                        }
                        else if (bIReportXMLData.IncludeTotals && (agColumn.DataTypeCode == "Double" || agColumn.DataTypeCode == "Decimal" || agColumn.DataTypeCode == "Integer"))
                        {
                            string value = row[agColumn.Name].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                excelTotals[excelTotalCount].Total += Double.Parse(value);
                            }
                            excelTotalCount += 1;
                        }
                    }
                    cellCol++;

                }
                cellRow++;
            }
            if (bIReportXMLData.IncludeTotals)
            {
                foreach (var ex in excelTotals)
                {
                    sheet.Range[rows + 2, ex.IndexOrder + 1].Cells[0].CellStyle.Color = Color.Orange;
                    sheet.Range[rows + 2, ex.IndexOrder + 1].Value = ex.Total.ToString();
                }
            }

            workbook.SaveAs(memory);
            workbook.Close();
            excelEngine.Dispose();
            return memory.ToArray();
        }

        private string ConvertDataList2Xml(IEnumerator dataList, QueryPM query, List<QueryColumnPM> queryColumns, int tenant)
        {

            TextCodeRepository textCodeRepoitory = new TextCodeRepository(tenant);
            TenantRepository tenantRepoitory = new TenantRepository(tenant);
            var CurTenant = tenantRepoitory.GetSingleByTenant(tenant);
            string queryName =!string.IsNullOrEmpty(query.DisplayText) ? query.DisplayText : TranslateTextsClass.Translate(query.NameTextCodeCode, tenant).Replace(" ", "_") + "_" + query.ObjectTableName + "s";

            queryName = ExportToExcelHelper.GetValidFileName(queryName);//queryName.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("'", "");
            queryName = queryName.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("'", "");

            //if (queryName.Length > 31)
            //    queryName = queryName.Substring(0, 31);

            System.Xml.Linq.XElement entities = new System.Xml.Linq.XElement(queryName);
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
                            string text = !string.IsNullOrWhiteSpace(column.ObjectFieldListLabelTextCodeCode) ? column.ObjectFieldListLabelTextCodeCode : column.ObjectFieldFullNameTextCodeCode;
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
            string value = string.Empty ;
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
                else value= GetPropertyValue(entity, "ForwarderShipmentNumber");
            }
            else
            {
                PropertyInfo info = entity.GetType().GetProperty(column.ObjectFieldName);
                if (info != null)
                {
                    value = info.GetValue(entity, null) != null ? info.GetValue(entity, null).ToString() : " ";
                }
            }


            return string.IsNullOrEmpty(value) ? " ":value;
        }

        private  string GetLogBoxTaskFieldValue(object entity)
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

            return (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result)) ? "":result ;
        }

        private static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.
            String ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
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
    }
}
class ReflectionProperties
{
    public MethodInfo ListMethodInfo { get; set; }
    public MethodInfo CountMethodInfo { get; set; }
    public object context { get; set; }

}

class ExcelTotals
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

    public QueryPM QueryPM { get; set; }
    public  List<QueryColumnPM> QueryColumns { get; set; }
    

}
