using Devart.Data.Oracle;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Xml;
using WebFreight.Web.MetaDataUpdate;
using WebFreight.Web.WebServices;

namespace Logitude.Update
{
    public partial class EnhancedUpdateForm : Form
    {
        public Form1 ParentForm1 { get; set; }
        string message = "";
        public EnhancedUpdateForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Execute());
            thread.IsBackground = true;
            thread.Start();
        }

        private void Execute()
        {
            StartExecutingGlobalBeforeScripts();
            this.StartExecutingThebeforeScripts();
            this.StartUpdate();
            this.StartBuildZipFiles();
            this.StartExecutingTheAfterScripts();
            StartExecutingGlobalAfterScripts();
            ImportRoleFeatures();
            ImportPackageFeatures();
            message += Environment.NewLine + "Updating Meta Data is Finished.";
            this.AppendTextBox(message);
        }

        private void StartExecutingThebeforeScripts()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("script");
                
                for (i = 0; i <= xmlnode.Count - 1; i++)
                {

                    var order = xmlnode[i].Attributes.GetNamedItem("order");
                    var value = order.Value;
                    if (value == "before")
                    {
                        xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        str = xmlnode[i].InnerText.Trim();
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();

                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Executing script on main db: " + Environment.NewLine + str;
                        }
                        else
                        {
                            message += Environment.NewLine + "Executing script on main db: " + Environment.NewLine + str;
                        }

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                        string strConnString = GetConnection(0);
                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            using (OracleConnection cn = new OracleConnection(strConnString))
                            {
                                OracleCommand orclCommand1 = new OracleCommand(str, cn);

                                cn.Open();
                                orclCommand1.ExecuteNonQuery();
                                cn.Close();
                            }
                        }
                        else
                        {
                            using (SqlConnection cn = new SqlConnection(strConnString))
                            {
                                SqlCommand sqlCommand = new SqlCommand(str, cn);

                                cn.Open();
                                sqlCommand.ExecuteNonQuery();
                                cn.Close();
                            }
                        }

                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;

                        message += Environment.NewLine + "script on main db executed successfully";

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                    }
                }
            }
            catch (Exception ex)
            {

                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private void StartExecutingTheAfterScripts()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("script");

                for (i = 0; i <= xmlnode.Count - 1; i++)
                {

                    var order = xmlnode[i].Attributes.GetNamedItem("order");
                    var value = order.Value;
                    if (value == "after")
                    {
                        xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        str = xmlnode[i].InnerText.Trim();
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();

                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Executing script on main db: " + Environment.NewLine + str;
                        }
                        else
                        {
                            message += Environment.NewLine + "Executing script on main db: " + Environment.NewLine + str;
                        }

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                        string strConnString = GetConnection(0);
                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            using (OracleConnection cn = new OracleConnection(strConnString))
                            {
                                OracleCommand orclCommand1 = new OracleCommand(str, cn);

                                cn.Open();
                                orclCommand1.ExecuteNonQuery();
                                cn.Close();
                            }
                        }
                        else
                        {
                            using (SqlConnection cn = new SqlConnection(strConnString))
                            {
                                SqlCommand sqlCommand = new SqlCommand(str, cn);

                                cn.Open();
                                sqlCommand.ExecuteNonQuery();
                                cn.Close();
                            }
                        }

                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;

                        message += Environment.NewLine + "script on main db executed successfully";

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                    }
                }
            }
            catch (Exception ex)
            {

                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private void StartExecutingGlobalBeforeScripts()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("globalscript");

                for (i = 0; i <= xmlnode.Count - 1; i++)
                {

                    var order = xmlnode[i].Attributes.GetNamedItem("order");
                    var value = order.Value;
                    if (value == "before")
                    {
                        xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        str = xmlnode[i].InnerText.Trim();
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();

                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Executing script on global db: " + Environment.NewLine + str;
                        }
                        else
                        {
                            message += Environment.NewLine + "Executing script on global db: " + Environment.NewLine + str;
                        }

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                        var mainConnection = GetConnection(0);
                        string strConnString = mainConnection.Replace("_Main", "_Global");
                        
                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            using (OracleConnection cn = new OracleConnection(strConnString))
                            {
                                OracleCommand orclCommand1 = new OracleCommand(str, cn);

                                cn.Open();
                                orclCommand1.ExecuteNonQuery();
                                cn.Close();
                            }
                        }
                        else
                        {
                            using (SqlConnection cn = new SqlConnection(strConnString))
                            {
                                SqlCommand sqlCommand = new SqlCommand(str, cn);

                                cn.Open();
                                sqlCommand.ExecuteNonQuery();
                                cn.Close();
                            }
                        }

                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;

                        message += Environment.NewLine + "script on global db executed successfully";

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                    }
                }
            }
            catch (Exception ex)
            {

                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private void StartExecutingGlobalAfterScripts()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("globalscript");

                for (i = 0; i <= xmlnode.Count - 1; i++)
                {

                    var order = xmlnode[i].Attributes.GetNamedItem("order");
                    var value = order.Value;
                    if (value == "after")
                    {
                        xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        str = xmlnode[i].InnerText.Trim();
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();

                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Executing script on global db: " + Environment.NewLine + str;
                        }
                        else
                        {
                            message += Environment.NewLine + "Executing script on global db: " + Environment.NewLine + str;
                        }

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                        var mainConnection = GetConnection(0);
                        string strConnString = mainConnection.Replace("_Main", "_Global");

                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            using (OracleConnection cn = new OracleConnection(strConnString))
                            {
                                OracleCommand orclCommand1 = new OracleCommand(str, cn);

                                cn.Open();
                                orclCommand1.ExecuteNonQuery();
                                cn.Close();
                            }
                        }
                        else
                        {
                            using (SqlConnection cn = new SqlConnection(strConnString))
                            {
                                SqlCommand sqlCommand = new SqlCommand(str, cn);

                                cn.Open();
                                sqlCommand.ExecuteNonQuery();
                                cn.Close();
                            }
                        }

                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;

                        message += Environment.NewLine + "script on global db executed successfully";

                        //this.SetControlPropertyValue(textBox1, "Text", message);
                        this.AppendTextBox(message);
                    }
                }
            }
            catch (Exception ex)
            {

                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private void StartBuildZipFiles()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("BuildZipFiles");
                for (i = 0; i <= xmlnode.Count - 1; i++)
                {
                    xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                    str = xmlnode[i].InnerText.Trim();
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    if (string.IsNullOrEmpty(message))
                    {
                        message += this.GetMessage(str, true);
                    }
                    else
                    {
                        message += Environment.NewLine + this.GetMessage(str, true);
                    }

                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);

                    if (str.ToLower() != "buildzipfilesgeneral")
                    {
                        TenantsUpdateClass.BuildObjectTablesZipFilesData(false, false);
                    }
                    else if(str.ToLower() != "buildzipfilescustoms")
                    {
                        TenantsUpdateClass.BuildObjectTablesZipFilesData(false, true);
                    }

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    message += Environment.NewLine + this.GetMessage(str, false, ts.ToString());

                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);

                }
            }
            catch (Exception ex)
            {
                
                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
            
        }

        private void StartUpdate()
        {
            
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlNodeList xmlnode;
                int i = 0;
                string str = null;
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                xmlnode = xmldoc.GetElementsByTagName("Update");
                for (i = 0; i <= xmlnode.Count - 1; i++)
                {
                    xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                    str = xmlnode[i].InnerText.Trim();
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    if (string.IsNullOrEmpty(message))
                    {
                        message += this.GetMessage(str, true);
                    }
                    else
                    {
                        message += Environment.NewLine + this.GetMessage(str, true);
                    }

                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);
                    if (str.ToLower() != "tenants")
                    {
                        ParentForm1.UpdateModule(0, str.ToLower());
                    }
                    else
                    {
                        UpdateTenants();
                    }

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    message += Environment.NewLine + this.GetMessage(str, false, ts.ToString());

                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);
                }

            }
            catch (Exception ex)
            {

                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private void UpdateTenants()
        {
            List<GlobalTenant> globalTenants;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                globalTenants = GlobalTenantRepository.GetGlobalTenants();

            }

            if (globalTenants != null)
            {
                GlobalTenant tenantZero = globalTenants.Where(d => d.Id == 0).FirstOrDefault();
                List<GlobalTenant> upgradableTenants = (from a in globalTenants
                                                        where a.Version != tenantZero.Version && a.Id != 0 && a.Version != -1 && a.IsActive == true
                                                        select a).ToList();
                if (upgradableTenants.Count > 0)
                {
                    foreach (GlobalTenant tenant in upgradableTenants)
                    {
                        if (tenant.Id != 0)
                        {
                            TenantsUpdateClass.UpdateDataForTenant(tenant.Id, "");
                        }
                    }


                }
            }
        }

        private void ImportRoleFeatures()
        {
            try
            {


                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);

                XmlDocument xmldoc = new XmlDocument();
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("RoleFeatures");
                if (xmlnode.Count > 0)
                {
                    string path = xmlnode[0].InnerText.Trim();


                    ExportImportHelper service = new ExportImportHelper();
                    FileStream a = new FileStream(path, FileMode.Open);
                    byte[] array = new byte[a.Length];
                    a.Read(array, 0, (int)(a.Length));
                    a.Close();
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    if (string.IsNullOrEmpty(message))
                    {
                        message += "Importing Role Features from file: " + path;
                    }
                    else
                    {
                        message += Environment.NewLine + "Importing Role Features from file: " + path;
                    }
                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);

                    string errormessage = service.ImportRoleFeatures(array);
                    if (string.IsNullOrEmpty(errormessage))
                    {
                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Importing role features completed succeeded";
                        }
                        else
                        {
                            message += Environment.NewLine + "Importing role features completed succeeded";


                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Importing role features completed with errors: " + Environment.NewLine + errormessage;
                        }
                        else
                        {
                            message += Environment.NewLine + "Importing role features completed with errors: " + Environment.NewLine + errormessage;
                        }
                    }

                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);
                }

            }
            catch (Exception ex)
            {
                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private void ImportPackageFeatures()
        {
            try
            {
                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);

                XmlDocument xmldoc = new XmlDocument();
                FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("PackageFeatures");
                if (xmlnode.Count > 0)
                {
                    string path = xmlnode[0].InnerText.Trim();

                    ExportImportHelper service = new ExportImportHelper();
                    FileStream a = new FileStream(path, FileMode.Open);
                    byte[] array = new byte[a.Length];
                    a.Read(array, 0, (int)(a.Length));
                    a.Close();
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    if (string.IsNullOrEmpty(message))
                    {
                        message += "Importing Package Features from file: " + path;
                    }
                    else
                    {
                        message += Environment.NewLine + "Importing Package Features from file: " + path;
                    }
                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);

                    string errormessage = service.ImportPackageFeatures(array);
                    if (string.IsNullOrEmpty(errormessage))
                    {
                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Importing Package features completed succeeded";
                        }
                        else
                        {
                            message += Environment.NewLine + "Importing Package features completed succeeded";


                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(message))
                        {
                            message += "Importing Package features completed with errors: " + Environment.NewLine + errormessage;
                        }
                        else
                        {
                            message += Environment.NewLine + "Importing Package features completed with errors: " + Environment.NewLine + errormessage;
                        }
                    }

                    //this.SetControlPropertyValue(textBox1, "Text", message);
                    this.AppendTextBox(message);
                }

            }
            catch (Exception ex)
            {
                message += Environment.NewLine + "Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine + "Inner Exception Stack: " + ex.InnerException.StackTrace + Environment.NewLine;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "2nd Inner Exception: " + ex.InnerException.InnerException.Message + Environment.NewLine + "2nd Inner Exception Stack: " + ex.InnerException.InnerException.StackTrace + Environment.NewLine;
                    }
                }

                //this.SetControlPropertyValue(textBox1, "Text", message);
                this.AppendTextBox(message);
            }
        }

        private string GetMessage(string str,bool start,string time=null)
        {
            switch (str)
            {
                case "updatetenantzero":
                    {
                        if (start)
                        {
                            return "Tenant zero meta data is being updated....";
                        }
                        else
                        {
                            return "Tenant zero meta data was updated successfully in: " + time;
                        }
                    }
                case "customs":
                    {
                        if (start)
                        {
                            return "Customs meta data is being updated....";
                        }
                        else
                        {
                            return "Customs meta data was updated successfully in: " + time;
                        }
                    }
                case "crm":
                    {
                        if (start)
                        {
                            return "CRM meta data is being updated....";
                        }
                        else
                        {
                            return "CRM meta data was updated successfully in: " + time;
                        }
                    }
                case "booking":
                    {
                        if (start)
                        {
                            return "Booking meta data is being updated....";
                        }
                        else
                        {
                            return "Booking meta data was updated successfully in: " + time;
                        }
                    }
                case "social":
                    {
                        if (start)
                        {
                            return "social meta data is being updated....";
                        }
                        else
                        {
                            return "social meta data was updated successfully in: " + time;
                        }
                    }
                case "warehouse":
                    {
                        if (start)
                        {
                            return "warehouse meta data is being updated....";
                        }
                        else
                        {
                            return "warehouse meta data was updated successfully in: " + time;
                        }
                    }
                case "accounting":
                    {
                        if (start)
                        {
                            return "accounting meta data is being updated....";
                        }
                        else
                        {
                            return "accounting meta data was updated successfully in: " + time;
                        }
                    }
                case "tenants":
                    {
                        if (start)
                        {
                            return "all tenants meta data is being updated....";
                        }
                        else
                        {
                            return "all tenants meta data was updated successfully in: " + time;
                        }
                    }
                case "buildzipfilescustoms":
                    {
                        if (start)
                        {
                            return "Building zip files for customs data....";
                        }
                        else
                        {
                            return "zip files for customs data was built successfully in: " + time;
                        }
                    }
                case "buildzipfilesgeneral":
                    {
                        if (start)
                        {
                            return "Building zip files for general data....";
                        }
                        else
                        {
                            return "zip files for general data was built successfully in: " + time;
                        }
                    }
                default: { return "please provide a proper choice :Updates{updatetenantzero,customs,crm,booking,social,accounting,warehouse} ,BuildZipFiles{buildzipfilescustoms,buildzipfilesgeneral}"; }
            }
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

        private void CallControlMethod(Control oControl,string MethodName)
        {
            //textBox.SelectionStart = textBox.TextLength;
            //textBox.ScrollToCaret();
            object methodParams = null;
            MethodInfo dynMethod = oControl.GetType().GetMethod(MethodName);
            dynMethod.Invoke(oControl, new object[] {  });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var mainConnection= GetConnection(0);
            // string globalconnection = mainConnection.Replace("_Main", "_Global");
            Thread thread = new Thread(() => testTextBox());
            thread.IsBackground = true;
            thread.Start();
         

        }
        private void testTextBox()
        {
            string messageee = "hellooooo";
            for (int i = 0; i < 1000; i++)
            {
                AppendTextBox(messageee);
                //message += Environment.NewLine + messageee;
                //this.SetControlPropertyValue(textBox1, "Text", messageee);
                //this.SetControlPropertyValue(textBox1, "SelectionStart", message.Length);
                //this.CallControlMethod(textBox1, "ScrollToCaret");
            }
        }

        public void AppendTextBox(string value)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<string>(AppendTextBox), new object[] { Environment.NewLine });
                textBox1.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                this.SetControlPropertyValue(textBox1, "SelectionStart", value.Length);
                textBox1.Invoke(new Action(ScrollToCaret));
                return;
            }
            textBox1.Text = value;

        }

        public void ScrollToCaret()
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action(ScrollToCaret));
                return;
            }
            textBox1.ScrollToCaret();
        }
    }
}
