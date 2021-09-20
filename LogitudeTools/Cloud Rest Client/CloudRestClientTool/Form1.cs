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
            this.operationCombo.Items.Add("Create (POST)");
            this.operationCombo.Items.Add("Update (PUT)");
            this.operationCombo.Items.Add("Get");
            this.operationCombo.Items.Add("Cancel");

            this.operationCombo.SelectedIndex = 0;
        }


        private void SetDefaultValues()
        {
            txtServerUrl.Text = "https://test.logitudeworld.com/test/api/";
            txtCredentialsPrimary.Text = "1859482b-755c-4259-a2be-8b4dc2e531f0";
            txtRequestBody.Text = XDocument.Load(@"ShipmentOrder.xml").ToString();
            
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
        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtRequestBody.Text);
        }
        private void btnCopyResponseBody_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(xmlBrowser1.XmlText);
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            txtRequestBody.Text = "";
        }
        private void PasteButton_Click(object sender, EventArgs e)
        {
            txtRequestBody.Text = Clipboard.GetText();
        }

        private void operationCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

            isSendButtonEnabled = operationCombo.SelectedIndex != -1 ? true : false;
            ShipmentOrderParameterTextBox.Visible = (operationCombo.SelectedIndex == 2 || operationCombo.SelectedIndex == 3) ? true : false;
            lblParameter.Visible = operationCombo.SelectedIndex == 2 ? true : false;

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
                        var content = new StringContent(txtRequestBody.Text, Encoding.UTF8, txtRequestContentType.Text);
                        HttpResponseMessage response = new HttpResponseMessage();

                        if (operationCombo.SelectedIndex == 0)
                        {
                            response = await client.PostAsync(txtServerUrl.Text + "/" + "ShipmentOrder", content);
                        }
                        else if (operationCombo.SelectedIndex == 1)
                        {
                            response = await client.PutAsync(txtServerUrl.Text + "/" + "ShipmentOrder", content);
                        }
                        else if (operationCombo.SelectedIndex == 2)
                        {
                            client.DefaultRequestHeaders.Add("Accept", "application/xml");

                            if (!string.IsNullOrEmpty(ShipmentOrderParameterTextBox.Text))
                            {
                                response = await client.GetAsync(txtServerUrl.Text + "/" + "ShipmentOrder" + "?orderNumber=" + ShipmentOrderParameterTextBox.Text);
                            }
                            else
                            {
                                MessageBox.Show("Please insert order number");
                                this.Cursor = Cursors.Default;
                                return;
                            }

                        }
                        else if (operationCombo.SelectedIndex == 3)
                        {
                            client.DefaultRequestHeaders.Add("Accept", "application/xml");

                            if (!string.IsNullOrEmpty(ShipmentOrderParameterTextBox.Text))
                            {
                                response = await client.DeleteAsync(txtServerUrl.Text + "/" + "ShipmentOrder" + "?orderNumber=" + ShipmentOrderParameterTextBox.Text);
                            }
                            else
                            {
                                MessageBox.Show("Please insert order number");
                                this.Cursor = Cursors.Default;
                                return;
                            }

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

        private void ShipmentOrderParameterTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

