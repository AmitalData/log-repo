using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Syncfusion.Calculate;
using Syncfusion.XlsIO;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using System.Transactions;
using Logitude.Server.Tools.Helpers;
using System.Text.RegularExpressions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using System.Data.Common;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for ExcelExportService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ExcelExportService : System.Web.Services.WebService
    {
        private Syncfusion.Calculate.CalculateConfig calculateConfig1;

        public CalculateConfig CalculateConfig1
        {
            get { return calculateConfig1; }
            set { calculateConfig1 = value; }
        }

        [WebMethod]
        public byte[] ExportQueryToExcel(byte[] xmlFilters, string queryId, int tenant,string userid, string typename)
        {
            //string xmlData = "";
            //System.IO.MemoryStream memory = new System.IO.MemoryStream();
            byte[] bytes = null; 
            try
            {

                bytes = new ExportToExcelHelper().ExportQueryToExcel(new ExportToExcelArgs() { XmlFilters = xmlFilters, QueryCode = queryId, Tenant = tenant, UserId = userid, TypeName = typename });

                //FilterSerializer filterSerializer = new FilterSerializer();
                //QueryRepository queryRep = new QueryRepository(tenant);
                //QueryColumnRepository queryColumnRep = new QueryColumnRepository(tenant);
                //QueryQuery queryQuery = new QueryQuery(queryRep);
                //QueryPM query = queryQuery.GetSingleQueryPM(queryId, tenant);
                //QueryColumnQuery queryColumnQuery = new QueryColumnQuery(queryColumnRep);
                //List<QueryColumnPM> queryColumns = queryColumnQuery.GetQueryColumnsByQueryIdAndUser(tenant, userid, query.Id).OrderBy(q => q.IndexOrder).ToList();
                //if (queryColumns.Count==0)
                //{
                //    queryColumns = queryColumnQuery.GetQueryColumnsByQueryIdAndUser(0, userid, query.Id).OrderBy(q => q.IndexOrder).ToList();
                //}

                //if (queryColumns.Count == 0)
                //{
                //    queryColumns = queryColumnQuery.GetZeroQueryColumnsByQueryId(0, query.Id).OrderBy(q => q.IndexOrder).ToList();
                //}

                //MemoryStream memorystream = new MemoryStream(xmlFilters);
                //XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
                //QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
                //queryOperations.GetAll = true;
                //queryOperations.PageIndex = 0;
                //queryOperations.GetAll = true;
                //Type contextType = Type.GetType(typename.Replace("Context", "Service"));

                //object context = Activator.CreateInstance(contextType);

                //DomainServiceContext con = new DomainServiceContext(new MockServiceProvider(), DomainOperationType.Query);
                //MethodInfo methodInfo = context.GetType().GetMethod("Initialize");
                //object[] parameters1 = new object[] { con };
                //methodInfo.Invoke(context, parameters1);

                //MethodInfo getListMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "Filters") : context.GetType().GetMethod("Get" + query.ObjectTableName + "Filters");
                //MethodInfo getCountMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + query.ObjectTableName + "FiltersCount");
                //if (getCountMethodInfo == null)
                //{
                //    getCountMethodInfo = query.ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + query.ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + query.ObjectTableName + "Count");
                
                //}
                //System.Linq.IQueryable querableEntities = null;

                //if (getListMethodInfo != null && getCountMethodInfo != null)
                //{
                //    //Get data count
                //    xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                //    object[] parameters = new object[] { xmlFilters, tenant };
                //    int count = (int)getCountMethodInfo.Invoke(context, parameters);

                //    // Get dataList
                //    queryOperations.PageSize = count;
                //    xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                //    parameters = new object[] { xmlFilters, tenant };

                //    querableEntities = getListMethodInfo.Invoke(context, parameters) as IQueryable;

                //    if (querableEntities == null)
                //    {
                //        var queryResult = getListMethodInfo.Invoke(context, parameters);
                //        if (queryResult != null)
                //        {
                //            IList list = queryResult as IList;
                //            if (list != null)
                //            {
                //                querableEntities = list.AsQueryable();
                //            }

                //        }
                //    }

                //    if (querableEntities != null)
                //    {
                         
                //        IEnumerator datalist = querableEntities.GetEnumerator();
                //        xmlData = ConvertDataList2Xml(datalist, query, queryColumns,tenant);


                //        //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
                //        //The instantiation process consists of two steps.

                //        //Step 1 : Instantiate the spreadsheet creation engine.
                //        ExcelEngine excelEngine = new ExcelEngine();
                //        //Step 2 : Instantiate the excel application object.
                //        IApplication application = excelEngine.Excel;

                //        //A new workbook is created.[Equivalent to creating a new workbook in MS Excel]
                //        //The new workbook will have 5 worksheets
                //        IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
                //        //The first worksheet object in the worksheets collection is accessed.
                //        IWorksheet sheet = workbook.Worksheets[0];
                //        //****************************** Creating excel from xml string *****************************

                //        //sheet.Range["A2:H2"].Merge();
                //        //sheet.Range["A1:P1"].Merge();
                //        //sheet.Range["A1:H2"].Merge();
                //        //sheet.Range["A2:H2"].CellStyle.FillBackground = ExcelKnownColors.LightGreen;

                //        sheet.Range["A2:C2"].Merge();
                //        sheet.Range["A2:C2"].Text = query.Code; //+ " " + (query.Code.Contains(query.ObjectTableName)?"": query.ObjectTableName + "s");//"First Flight";
                //        sheet.Range["A2:C2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                //        sheet.Range["A2:C2"].CellStyle.Font.Bold = true;
                //        sheet.Range["A2:C2"].CellStyle.Font.Color = ExcelKnownColors.Black;
                //        sheet.Range["A2:C2"].CellStyle.Font.Size = 12;
                //        sheet.Range["A2:C2"].CellStyle.Font.FontName = "Thoma";
                //        sheet.Range["A2:Z2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                //        sheet.Range["A3:Z3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                   

                //        foreach (QueryColumnPM column in queryColumns)
                //        {
                //            sheet.AutofitColumn(column.IndexOrder + 1);
                           
                //        }
                      
                //        XmlReader reader = XmlReader.Create(new StringReader(xmlData));
                //        XmlDataDocument doc = new XmlDataDocument();
                //        doc.Load(reader);

                //        doc.GetElementsByTagName(query.ObjectTableName);
                //        XmlNodeList entitiesList = doc.GetElementsByTagName(query.ObjectTableName);
                    
                //        if (entitiesList.Count == 0)
                //        {
                //            string ip = "";
                //            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                //            {
                //                ip = HttpContext.Current.Request.UserHostAddress;
                //            }
                //            ExceptionHandler.HandleException(new Exception("Error while building xml file, table columns are empty!"), DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ExcelExportService : ExportQueryToExcel Method",ip);
                //            return null;
                //        }

                //        int[,] array = new int[,] { { 65, 0 } };
                //        foreach (XmlNode node in entitiesList.Item(0).ChildNodes)
                //        {
                //            string nodename = TranslateTextsClass.Translate(node.Name, tenant);
                //            nodename = nodename != null ? nodename : "";
                //            nodename = nodename.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");

                //            string sheetColumn = "";
                //            if (array[0, 0] <= 90 && array[0, 1] == 0)
                //            {
                //                char a = (char)array[0, 0];
                //                sheetColumn = a.ToString();
                //                array[0, 0]++;
                //            }
                //            else
                //            {
                //                if (array[0, 1] == 0)
                //                {
                //                    array[0, 0] = 65;
                //                    array[0, 1] = 65;

                //                    sheet.Range["AA2:AZ2"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;
                //                    sheet.Range["AA3:AZ3"].CellStyle.ColorIndex = ExcelKnownColors.Grey_25_percent;

                //                }
                //                if (array[0, 1] <= 90)
                //                {
                //                    string a = sheetColumn = ((char)array[0, 0]).ToString() + ((char)array[0, 1]).ToString();
                //                    array[0, 1]++;
                //                }
                //                else
                //                    break;

                //            };

                //            sheet.Range[sheetColumn.ToString() + "3"].Text = nodename;
                //            IRange range = sheet.Range[sheetColumn + "3"];
                //            range.CellStyle.Font.FontName = "Times New Roman";
                //            range.CellStyle.Font.Bold = true;

                //        }

                //        int cellRow = 4;
                //        foreach (XmlNode node in entitiesList)
                //        {
                //            int cellCol = 1;
                //            foreach (XmlNode childNode in node.ChildNodes)
                //            {

                //                QueryColumnPM column = queryColumns.Where(q => q.ObjectFieldListLabelTextCodeCode == childNode.Name || q.ObjectFieldFullNameTextCodeCode == childNode.Name).FirstOrDefault();
                                
                //                switch (column.ObjectFieldDataTypeCode)
                //                {
                //                    case "Text":
                //                        sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                //                        break;

                //                    case "Boolean":
                //                        Boolean b = false;
                //                        Boolean.TryParse(childNode.InnerText.Trim(), out b);
                //                        sheet.Range[cellRow, cellCol].Boolean = b;
                //                        break;

                //                    case "Constant":
                //                        sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                //                        break;

                //                    case "DateTime":
                //                        DateTime date;
                //                        if (DateTime.TryParse(childNode.InnerText.Trim(), out date))
                //                        {
                //                            sheet.Range[cellRow, cellCol].DateTime = date.Date;
                //                        }
                //                        else
                //                        {
                //                            sheet.Range[cellRow, cellCol].Text = "";
                //                        }


                //                        break;
                //                    case "Decimal":
                //                        double dex = 0;
                //                        double.TryParse(childNode.InnerText.Trim(), out dex);
                //                        sheet.Range[cellRow, cellCol].Number = dex;
                //                        break;
                //                    case "Double":
                //                        double d = 0;
                //                        double.TryParse(childNode.InnerText.Trim(), out d);
                //                        sheet.Range[cellRow, cellCol].Number = d;
                //                        break;
                //                    case "Integer":
                //                        int x = 0;
                //                        int.TryParse(childNode.InnerText.Trim(), out x);
                //                        sheet.Range[cellRow, cellCol].Number = x;
                //                        break;

                //                    default:
                //                        sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                //                        break;
                //                }

                //                //sheet.Range[cellRow, cellCol].Text = childNode.InnerText.Trim();
                //                //if (cellCol <= 2)
                //                //    sheet.Range[cellRow, cellCol].ColumnWidth = 30;



                //                cellCol++;
                //            }
                //            cellRow++;

                //        }


                //        //***************************************************************************
                //        //Inserting sample text into the range of cells of the first worksheet.
                //        //sheet.Range["A1:N30"].Text = "Hello World";

                //        //Saving the workbook to disk.

                //        workbook.SaveAs(memory, ExcelSaveType.SaveAsXLS);

                //        //No exception will be thrown if there are unsaved workbooks.
                //        excelEngine.ThrowNotSavedOnDestroy = false;
                //        excelEngine.Dispose();
                //    }
                //}
            }
            catch (Exception e)
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
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ExcelExportService : ExportQueryToExcel Method",ip);
            }


            return bytes;// memory.ToArray();
        }



        #region ConvertDataList2Xml
         
        private string ConvertDataList2Xml(IEnumerator dataList,QueryPM query, List<QueryColumnPM> queryColumns,int tenant)
        {

            TextCodeRepository textCodeRepoitory = new TextCodeRepository(tenant);
            string queryName = TranslateTextsClass.Translate(query.NameTextCodeCode, tenant).Replace(" ", "_") + "_" + query.ObjectTableName + "s";
             
            queryName = queryName.Replace(":", "").Replace("/", "").Replace("\"", "").Replace("?", "").Replace("*", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("'","");

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
                            PropertyInfo info = entity.GetType().GetProperty(column.ObjectFieldName);
                            if (info != null)
                            {
                                value = info.GetValue(entity, null) != null ? info.GetValue(entity, null).ToString() : " ";
                            }


                            col.Value = Fix(value);

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
            catch { }

            return entities.ToString();



        }
         
        #endregion

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

        [WebMethod]
        public byte[] ExportFeaturesToCSVFile()
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(0);
            var List = (from PackageFeature in commonDataContext.PackageFeatures
                        join Feature in commonDataContext.Features on PackageFeature.FeatureId equals Feature.Id into FeaturePackages
                        from FeaturePackage in FeaturePackages.DefaultIfEmpty()
                        where PackageFeature.Tenant == 0
                        orderby PackageFeature.PackageCode
                        select new
                        {
                            PackageCode = PackageFeature.PackageCode,
                            ObjectTableName = PackageFeature.Feature.ObjectTable.Name,
                            FeatureCode = FeaturePackage.Code,
                        }


                        ).ToList();

//            var list2 = (from Table in webFreightContext.ObjectTables
//                         join objt in List on Table.Id
//equals objt.ObjectTableId into FeatureTables
//                         from FeatureTable in FeatureTables.DefaultIfEmpty()


//                         select new
//                         {
//                             PackageCode = FeatureTable.PackageCode,
//                             TableName = Table.Name,
//                             FeatureCode = FeatureTable.FeatureCode,
//                         }
//                 ).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var packagefeature in List)
            {

                string line = packagefeature.PackageCode + "," + packagefeature.ObjectTableName + "," + packagefeature.FeatureCode;
                sb.AppendLine(line);
            }


            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());

            return buffer;

        }

        public byte[] ExportFeaturesToCSVFile2()
        {
            PackageFeatureRepository rep = new PackageFeatureRepository(0);
            FeatureRepository featurrep = new FeatureRepository(0);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(0);
            List<PackageFeature> packagefeatures = rep.GetPackageFeaturesByTenant(0).OrderBy(f=>f.PackageCode).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (PackageFeature packagefeature in packagefeatures)
            {
                Feature feature = featurrep.GetSingleFeature(packagefeature.FeatureId);
                ObjectTable table = objecttablerep.GetSingleObjectTable(feature.ObjectTableId,0,true);
                string line = packagefeature.PackageCode +","+ table.Name + "," + feature.Code;
                sb.AppendLine(line);
            }


            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());

            return buffer;
            
        }






        [WebMethod]
        public string ImportFeaturePackages(byte[] data)
        {
            ExportImportHelper helper = new ExportImportHelper();
            string error = helper.ImportPackageFeatures(data);
            return error;
        }

        [WebMethod]
        public void ImportClockTime(byte[] data,int tenant)
        {
            
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "TMC",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = data,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = data.Length,
                Tenant = tenant,
                
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }


        [WebMethod]
        public byte[] ExportRoleFeaturesToCSVFile()
        {
            RoleRepository roleRep = new RoleRepository(0);
            RoleFeatureRepository rep = new RoleFeatureRepository(0);
            FeatureRepository featurrep = new FeatureRepository(0);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(0);
            List<Role> roles = roleRep.GetRoles(0).ToList();
            List<RoleFeature> rolefeatures = rep.GetRoleFeaturesByTenant(0).OrderBy(f => f.RoleId).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (RoleFeature rolefeature in rolefeatures)
            {
                Feature feature = featurrep.GetSingleFeatureByUniqeCode(rolefeature.FeatureUniqeCode);
                if (feature != null)
                {
                    ObjectTable table = objecttablerep.GetSingleObjectTable(feature.ObjectTableId, 0, true);
                    Role role = roles.Where(d => d.Id == rolefeature.RoleId).FirstOrDefault();
                    string line = role.Code + "," + table.Name + "," + feature.Code + "," + rolefeature.FeatureAccessLevelCode;
                    sb.AppendLine(line);
                }
             
            }


            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());

            return buffer;

        }


        [WebMethod]
        public byte[] ExportRoleFeaturesFromSourceDB(string dbConnectionString)
        {
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionString, null);
            ICommonDataContext commonDataContext = new CommonDataContext(connection);
            IWebFreightContext webDataContext = new WebFreightContext(connection);

            RoleRepository roleRep = new RoleRepository(commonDataContext);
            RoleFeatureRepository rep = new RoleFeatureRepository(commonDataContext);
            FeatureRepository featurrep = new FeatureRepository(commonDataContext);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(webDataContext);
            List<Role> roles = roleRep.GetRoles(0).ToList();
            List<RoleFeature> rolefeatures = rep.GetRoleFeaturesByTenant(0).OrderBy(f => f.RoleId).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (RoleFeature rolefeature in rolefeatures)
            {
                Feature feature = featurrep.GetSingleFeatureByUniqeCode(rolefeature.FeatureUniqeCode);
                if (feature != null)
                {
                    ObjectTable table = objecttablerep.GetSingleObjectTable(feature.ObjectTableId, 0, true);
                    Role role = roles.Where(d => d.Id == rolefeature.RoleId).FirstOrDefault();
                    string line = role.Code + "," + table.Name + "," + feature.Code + "," + rolefeature.FeatureAccessLevelCode;
                    sb.AppendLine(line);
                }
            }

            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
            return buffer;
        }


        [WebMethod]
        public byte[] ExportPackagesFeaturesFromSourceDB(string dbConnectionString)
        {
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionString, null);
            ICommonDataContext commonDataContext = new CommonDataContext(connection);

            var List = (from PackageFeature in commonDataContext.PackageFeatures
                        join Feature in commonDataContext.Features on PackageFeature.FeatureId equals Feature.Id into FeaturePackages
                        from FeaturePackage in FeaturePackages.DefaultIfEmpty()
                        where PackageFeature.Tenant == 0
                        orderby PackageFeature.PackageCode
                        select new
                        {
                            PackageCode = PackageFeature.PackageCode,
                            ObjectTableName = PackageFeature.Feature.ObjectTable.Name,
                            FeatureCode = FeaturePackage.Code,
                        }).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var packagefeature in List)
            {

                string line = packagefeature.PackageCode + "," + packagefeature.ObjectTableName + "," + packagefeature.FeatureCode;
                sb.AppendLine(line);
            }

            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
            return buffer;
        }


        [WebMethod]
        public string ImportRoleFeatures(byte[] data)
        {
            ExportImportHelper helper = new WebServices.ExportImportHelper();
            string message = helper.ImportRoleFeatures(data);
            return message;
        }
        
      

        private void InitializeComponent()
        {
            this.CalculateConfig1 = new Syncfusion.Calculate.CalculateConfig();

        }


 

    }
}
