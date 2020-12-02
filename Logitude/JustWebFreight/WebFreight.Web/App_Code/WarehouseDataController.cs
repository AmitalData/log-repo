using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace WebFreight.Web.App_Code
{
    public class WarehouseDataController : ApiController
    {
        public string GetWarehouseData()
        {
            List<WarehouseDataClass> warehouseDataClassDataLists = new List<WarehouseDataClass>();
            warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "abed@mail.com", EnglishName = "abed", Number = null, Number2 = 3, Phone = "5222" , IsLoad = false, decimalCol = 1 , CreateDate = DateTime.Today });
          //  warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "abed@mail.com", EnglishName = "test", Number = null, Number2 = null, Phone = "213" , IsLoad =true, decimalCol = 1, CreateDate = DateTime.Now });
           // warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "ahmadb@mail.com", EnglishName = "ahmadb", Number = null, Number2 = null, Phone = "213" , IsLoad = false, decimalCol = 1, CreateDate = DateTime.Now });
           // warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "mahdi@mail.com", EnglishName = "mahdi", Number = null, Number2 = null, Phone = "213", IsLoad = false, decimalCol = 1, CreateDate = DateTime.Now });
           // warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "islam@mail.com", EnglishName = "islam", Number = null, Number2 = null, Phone = "213" , IsLoad = false, decimalCol = 1, CreateDate = DateTime.Now });
          //  warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "ayman@mail.com", EnglishName = "abed", Number = null, Number2 = null, Phone = "213" , IsLoad = false, decimalCol = 1, CreateDate = DateTime.Now });
           // warehouseDataClassDataLists.Add(new WarehouseDataClass() { Tenant = 1, Email = "SSSSS@mail.com", EnglishName = "abed", Number = null, Number2 = null, Phone = "213" , IsLoad = false, decimalCol = 1, CreateDate = DateTime.Now });

            var xmalData = LogitudeXmlSerializer.SerializeObjectToJosnString(warehouseDataClassDataLists);

       

            return xmalData;


        }

        public string GetWarehouseFactData()
        {

            string connectionString = "Data Source=.;Initial Catalog=Logitude2-5_Main;Integrated Security=False;Persist Security Info=True;User ID=sa;Password= Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            var dataTable = new DataTable();

            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand("SELECT SourceTenant , ParentTenant , House ,Master, Direction,TransportMode , Level,type,ShipmentNumber ,TotalReceivablesInLocalCurrency , DepartedDate, ArrivedDate,IsArrived  from Fact_Shipments where ArrivedDate is not null or DepartedDate is not null ", sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                dataTable.Load(reader);
                reader.Close();
            }

            string josnString = "{ \"warehouse\" :";
         //   JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            josnString += JsonConvert.SerializeObject(dataTable);
            josnString += "}";

            josnString = josnString.Replace("ShipmentNumber", "Shipment Number");
            return josnString;

        }




        private string BliudJosnString(DataTable dataTable)
        {
            string result = "";
            StringBuilder myStringBuilder = new StringBuilder("{ \"warehouse\" : [");
            myStringBuilder.Append("");
            int count = 0;
    
            foreach (DataRow row in dataTable.Rows)
            {
                if (count != 0) myStringBuilder.Append(",");
                myStringBuilder.Append("{");
         

                count += 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnType = column.DataType.ToString();
                    string columnName = column.ColumnName.ToString();
                    string value = (row[columnName]) != null ? (row[columnName]).ToString() : "";
                    if (columnType == "System.Boolean")
                    {
                        if (!string.IsNullOrEmpty(value)) value = value.ToLower();
                    }
                   
                    if (columnType == "System.String" || columnType == "System.DateTime" || columnType == "System.Date") myStringBuilder.Append("\"" + columnName + "\" :" + "\"" + value + "\""); 
                    else myStringBuilder.Append("\"" + columnName + "\" :" + (string.IsNullOrEmpty(value) ? "null" : value));


                    if (columnName != dataTable.Columns[dataTable.Columns.Count - 1].ColumnName) myStringBuilder.Append(","); 

                }
                myStringBuilder.Append("}");
        
            }
            myStringBuilder.Append("]");
            myStringBuilder.Append("}");
            result = myStringBuilder.ToString();
            result = result.Replace("ShipmentNumber", "Shipment Number");

            return result;
        }


        public void SaveWarehouseReport(WarehouseDataParameters warehouseData)
        {
            if (!string.IsNullOrEmpty(warehouseData.ReportData))
            {

        
                byte[] reportData = System.Text.Encoding.UTF8.GetBytes(warehouseData.ReportData);
                if (reportData != null)
                {
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = "WarehouseReportData",
                        FolderName = "others",
                        Extension = "JSON",
                        Tenant = 1,
                    };

                    fileInfo.FileSize = reportData.Length;
                    storageservice.Write(reportData, fileInfo);

                    GetWarehouseReportStimualAsPdf();
                }
            }
        }

        public string GetWarehouseJosnReport()
        {
            string josnReport= string.Empty;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = "WarehouseReportData",
                FolderName = "others",
                Extension = "JSON",
                Tenant = 1,
            };
           byte[] result =  storageservice.Read(fileInfo);
            if (result != null)
            {
                josnReport = System.Text.Encoding.UTF8.GetString(result);


            }
            return josnReport;
        }

        public void GetWarehouseReportStimualAsPdf()
        {
            try
            { 
            string reportData = string.Empty;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = "WarehouseReportData",
                FolderName = "others",
                Extension = "JSON",
                Tenant = 1,
            };
            byte[] result = storageservice.Read(fileInfo);
                if (result != null)
                {
                    StiReport report = new StiReport();

                    string reportJosn = System.Text.Encoding.UTF8.GetString(result);
                    report.LoadFromJson(reportJosn);

                   
                    var dataJons = GetWarehouseFactData();
                    XNode node = JsonConvert.DeserializeXNode(dataJons, "Root");
                    DataSet dataSet = new DataSet("Demo");
                    XmlReader reader = XmlReader.Create(new StringReader(node.ToString()));
                    dataSet.ReadXml(reader);
                    report.RegData(dataSet);

                    MemoryStream memoryStream = new MemoryStream();
                    report.CalculationMode = StiCalculationMode.Interpretation;
                    report.Dictionary.Synchronize();


                    report.AutoLocalizeReportOnRun = true;
                    report.Render(false);
                    
                    report.ExportDocument(StiExportFormat.Pdf, memoryStream);


                    fileInfo = new BlobFileInfo()
                    {
                        FileName = "WarehouseReportPDF",
                        FolderName = "others",
                        Extension = "pdf",
                        Tenant = 1,
                    };

                    fileInfo.FileSize = memoryStream.ToArray().Length;
                    storageservice.Write(memoryStream.ToArray(), fileInfo);
                }
            }
            catch(Exception ex)
            {

            }
     
        }

    }


    public class WarehouseDataClass
    {
        public int Tenant { get; set; }
        public string Email { get; set; }
        public decimal decimalCol { get; set; }
        public bool IsLoad { get; set; }
        public string EnglishName { get; set; }
        public int? Number { get; set; }
        public int? Number2 { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class WarehouseDataParameters
    {
        public string ReportData { get; set; }
        public byte[] ReportDataByte { get; set; }
    }


}