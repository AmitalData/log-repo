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
    public partial class CompareViewForm : Form
    {


        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";
        string dbDestinationConnection = "Logitude2-5_Main,sa,Saas256,.";



        public CompareViewForm()
        {
            InitializeComponent();

            this.SourceConnectiontextBox.Text = dbSourceConnection;
            this.DestinationtconnectionTextBox.Text = dbDestinationConnection;
        }



        private void CompareButton_Click(object sender, EventArgs e)
        {
            CompareViews();
        }


        private void CompareViews()
        {
            try
            {
                ResultRichTextBox.Text = "";
                string[] sourceConnectionArray = dbSourceConnection.Split(',');
                string sourceconnectionString = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);

                string[] destinationConnectionArray = dbDestinationConnection.Split(',');
                string destinationconnectionString = BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
                CompareViewsService compareViewsService = new CompareViewsService(sourceconnectionString, destinationconnectionString);
                ResultRichTextBox.Text = compareViewsService.Compare();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }



        private void SourceConnectiontextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox sourceConnectionTextBox = sender as TextBox;
            this.dbSourceConnection = sourceConnectionTextBox.Text;
        }



        private void DestinationtconnectionTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox destinationConnectionTextBox = sender as TextBox;
            this.dbDestinationConnection = destinationConnectionTextBox.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
