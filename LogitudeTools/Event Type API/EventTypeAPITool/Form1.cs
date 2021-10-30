using CloudRestClientTool;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CloudRestClientTool
{
    public partial class Form1 : Form
    {
        private string token;
        private bool isConnected;
        private bool isSendButtonEnabled;

        public Form1()
        {
            InitializeComponent();
            Application.EnableVisualStyles();

            this.SetDefaultValues();
            BuildOperationList();

        }


        private void BuildOperationList()
        {

            this.operationCombo.SelectedItem = null;
            this.operationCombo.Items.Clear(); 
            this.operationCombo.Items.Add("Get");

            this.operationCombo.SelectedIndex = 0;
        }


        private void SetDefaultValues()
        {
            txtServerUrl.Text = "http://localhost:9996/api/";
            txtCredentialsPrimary.Text = "7861264e-4b5d-45bf-a81f-71da0a4621d2"; 
            ObjectTableTextBox.Text = "Shipment";

        }
        private async void LoginWithCredentials()
        {
            try
            {
                isConnected = false;
                if (!isConnected)
                {
                    APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
                    {
                        PrimaryKey = txtCredentialsPrimary.Text,
                    };

                    using (var client = new HttpClient())
                    {
                        string AuthURI = txtServerUrl.Text + "APIAuthentication";
                        var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
                        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                        var result = await client.PostAsync(AuthURI, content);
                        var tempUser = result.Content.ReadAsStringAsync().Result;
                        ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
                        token = User.Token;

                        if (User.HasError)
                        {
                            lblMessage.Text = "Authentication Error!";
                            lblMessage.ForeColor = Color.Red;
                        }

                        else
                        {
                            lblMessage.Text = "Connected!";
                            lblMessage.ForeColor = Color.Green;
                            isConnected = true;

                        }
                    }

                    this.ChangeFormState();
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = "Authentication Error: " + ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
        private void ChangeFormState()
        {
            if (!isConnected)
            {
                this.Cursor = Cursors.WaitCursor;
                this.btnCallApi.Enabled = false;
            }
            else
            {
                this.Cursor = Cursors.Default;
                this.btnCallApi.Enabled = true;
            }
        }
        private void SetXmlBrouserXml(string xmlString)
        {
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()); 
            System.Xml.Xsl.XslCompiledTransform xTrans = new System.Xml.Xsl.XslCompiledTransform();

            StringReader sr = new StringReader(xmlString);
            XmlReader xReader = XmlReader.Create(sr);

            xmlBrowser1.XmlDocumentTransformType = XmlRender.XmlBrowser.XslTransformType.XSL;

             

            XmlDocument _xd = new XmlDocument();
            _xd.Load(xReader);
            xmlBrowser1.XmlDocument = _xd;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.LoginWithCredentials();
        }
        private void btnCallApi_Click(object sender, EventArgs e)
        {
            this.xmlBrowser1.DocumentText = "";
            this.xmlBrowser1.XmlText = "";
            txtReponseCode.Text = "";

            this.CallEntityApi();
        }
        private void rdbJson_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbJson.Checked)
            {
                txtRequestContentType.Text = "application/json";
            }
            else
            {
                txtRequestContentType.Text = "application/xml";
            }
        }
      
        private void btnCopyResponseBody_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(xmlBrowser1.XmlText);
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ObjectTableTextBox.Text = "";
        }
     
        private void operationCombo_SelectedIndexChanged(object sender, EventArgs e)
        { 
            isSendButtonEnabled = operationCombo.SelectedIndex != -1 ? true : false;  
            ChangeFormState();
        }


        private async void CallEntityApi()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!string.IsNullOrEmpty(this.token))
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = new TimeSpan(0, 10, 0);

                        client.DefaultRequestHeaders.Add("Token", token);
                        var content = new StringContent(ObjectTableTextBox.Text, Encoding.UTF8, txtRequestContentType.Text);
                        HttpResponseMessage response = new HttpResponseMessage();
                        client.DefaultRequestHeaders.Add("Accept", "application/xml");

                        switch (operationCombo.SelectedItem)
                        {
                            case "Get": 
                                response = await GetEventTypes(client, response); 
                                break;
                            default:
                                MessageBox.Show("select a service to test");
                                break;
                        }
                         

                        txtReponseCode.Text = ((int)response.StatusCode).ToString();

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var resultData = response.Content.ReadAsStringAsync().Result;
                            this.SetXmlBrouserXml(resultData);
                        }

                        else
                        {
                            var resultData2 = response.Content.ReadAsStringAsync().Result;
                            this.SetXmlBrouserXml(resultData2);
                        }


                    }

                }

                else
                {
                    lblMessage.Text = "Authentication Error!";
                    lblMessage.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            this.Cursor = Cursors.Default;
        }

        private async Task<HttpResponseMessage> GetEventTypes(HttpClient client, HttpResponseMessage response)
        {
            if (!string.IsNullOrEmpty(ObjectTableTextBox.Text))
            {
                response = await client.GetAsync(GetEventTypeAPI());
            }
            else
            {
                MessageBox.Show("Please insert object table name");
                this.Cursor = Cursors.Default;
            }

            return response;
        }

        private string GetEventTypeAPI()
        {
            return txtServerUrl.Text + "/" + "EventType" + "?objectTableName=" + ObjectTableTextBox.Text + "&connectedToStatus=" + ConnectedToStatus.Checked;
        }

        private void connectToTest_Click(object sender, EventArgs e)
        {
            txtServerUrl.Text = "https://test.logitudeworld.com/test/api/";
            txtCredentialsPrimary.Text = "1859482b-755c-4259-a2be-8b4dc2e531f0";
            ObjectTableTextBox.Text = "Shipment";
            this.LoginWithCredentials();
        }

         
    }
}

