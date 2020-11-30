using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseDataViews.Service;

namespace WarehouseDataViews
{
    public partial class CheckFeatureForm : Form
    {
        string featureCode = string.Empty;
        int tenant;

        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";

        public CheckFeatureForm()
        {
            InitializeComponent();
            this.SourceStringtextBox.Text = dbSourceConnection;
            this.FeatureNameTextBox.Text = "BIReport.Fact_Charges";
        }

        private void CheckFeatureButton_Click(object sender, EventArgs e)
        {
            string[] sourceConnectionArray = dbSourceConnection.Split(',');
            string sourceconnectionString = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            FeaturePrivateDataWarehouseService featurePrivateDataWarehouseService = new FeaturePrivateDataWarehouseService(sourceconnectionString.Replace("Main", "Global"), sourceconnectionString, tenant);
            if (featurePrivateDataWarehouseService.CheckFeature(featureCode))
            {
                MessageBox.Show("Feature is exist");
            }
            else MessageBox.Show("Feature dosn't  exist");

        }


        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        private void FeatureNameTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox featureNameTextBox = sender as TextBox;
            this.featureCode = featureNameTextBox.Text;
        }

        private void TenantNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tenantNumberTextBox = sender as TextBox;
            this.tenant = Int32.Parse(tenantNumberTextBox.Text);
        }

        private void SourceStringtextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox featureName = sender as TextBox;
            this.dbSourceConnection = SourceStringtextBox.Text;
        }
    }
}
