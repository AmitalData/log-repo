using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseDataViews.Service;

namespace WarehouseDataViews
{
    public partial class Form1 : Form
    {

        //Pre Private DB
        //private int? tenant = 570;
        //string dbSourceConnection = "LogitudeMain_PreR3,logitudemanager,!LO009008,logitudetest.database.windows.net";
        //string dbDestinationConnection = "UnicargoDW,logitudeep,!LO852456,logitude-ep.database.windows.net";

        //Online PrivateDB
        //private int? tenant = 570;
        //string dbSourceConnection = "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net";
        //string dbDestinationConnection = "T570Unicargo,Admin1423,London2015!London2015!,logitudedw1.database.windows.net";



        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";
        public Form1()
        {
            InitializeComponent();
            this.SourceConnectionTextBox.Text = dbSourceConnection;

        }

        private void CreateViewsButton_Click(object sender, EventArgs e)
        {
            CreateDataWarehouseViews();
        }
        private void CreateDataWarehouseViews()
        {
            try
            {
                ResultLabel.Text = "";
                string[] sourceConnectionArray = dbSourceConnection.Split(',');
                string connectionString = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                PrivateDataWarehouseViewService privateDataWarehouseViewService = new PrivateDataWarehouseViewService(connectionString);
                var dWHSettingsTable = privateDataWarehouseViewService.GetDataTableFromSql(connectionString, "SELECT  * from  DWHSettings where Catalog is not null");
                foreach (DataRow row in dWHSettingsTable.Rows)
                {
                    int tenant = Int32.Parse(row["Tenant"].ToString());
                    string catalog = row["Catalog"].ToString();
                    string userName = row["UserName"].ToString();
                    string password = row["Password"].ToString();
                    string server = row["Server"].ToString();
                    string privateUserName = row["PrivateUserName"].ToString();
                    bool isParentTenant =bool.Parse( row["IsParentTenant"].ToString());

                    string destinationConnectionString = BuildConnectionString(catalog, userName, password, server);
                    privateDataWarehouseViewService.GeneratePrivateViews(new PrivateViewArgs() { ConnectionString = destinationConnectionString, UserName = privateUserName, Tenant = tenant, Catalog = catalog, ApplyGrantOnViews = (!string.IsNullOrEmpty(privateUserName)?true:false) , IsParentTenant = isParentTenant });
                    
                }
                SetResultLable(true);
            }
            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }
        }




        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }




        private void DeleteViewsButton_Click(object sender, EventArgs e)
        {

            DeleteDataWarehouseViews();
        }
        private void DeleteDataWarehouseViews()
        {
            try
            {
                ResultLabel.Text = "";
                string[] sourceConnectionArray = dbSourceConnection.Split(',');
                string connectionString = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                GeneralDataWarehouseViewsService generalDataWarehouseViewsService = new GeneralDataWarehouseViewsService();
                var dWHSettingsTable = generalDataWarehouseViewsService.GetDataTableFromSql(connectionString, "SELECT  * from  DWHSettings where Catalog is not null");
                foreach (DataRow row in dWHSettingsTable.Rows)
                {
                    int tenant = Int32.Parse(row["Tenant"].ToString());
                    string catalog = row["Catalog"].ToString();
                    string userName = row["UserName"].ToString();
                    string password = row["Password"].ToString();
                    string server = row["Server"].ToString();
                    string destinationConnectionString = BuildConnectionString(catalog, userName, password, server);
                    string deleteViewsSql = "DECLARE @sql VARCHAR(MAX) = '', @crlf VARCHAR(2) = CHAR(13) + CHAR(10); SELECT @sql = @sql + 'DROP VIEW ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(v.name) + ';' + @crlf FROM sys.views v PRINT @sql;EXEC(@sql); ";
                    generalDataWarehouseViewsService.RunSql(destinationConnectionString, deleteViewsSql);
                }
                SetResultLable(true);


            }

            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }

        }


        private void DisplayExceptionMessage(Exception ex)
        {
            SetResultLable(false);
            string message = ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : "");
            if (message.Length > 1500) message = message.Substring(0, 1500);
            MessageBox.Show(message);
        }
        private void SetResultLable(bool isSuccess)
        {
            ResultLabel.Text = isSuccess ? "Done" : "Fail";
            ResultLabel.ForeColor = isSuccess ? Color.Green : Color.Red;
        }
        private void SourceConnectionTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox sourceConnectionTextBox = sender as TextBox;
            this.dbSourceConnection = sourceConnectionTextBox.Text;

        }

       
    }
}
