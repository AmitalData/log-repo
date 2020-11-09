using Json2KeyValue;
using RestClientApplication;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ExternalAPITool
{
    public partial class Form1 : Form
    {
        private string token;
        private bool isConnected;
        private bool isSendButtonEnabled;
        private string apiName = "";

        public Form1()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            this.apiCombo.Items.Add("Shipment");
            this.operationCombo.Items.Add("POST");
            this.actionCombo.Items.Add("Accept");
            this.actionCombo.Items.Add("Decline");
            this.actionCombo.Items.Add("Cancel");
            this.apiCombo.SelectedIndex = 0;
            this.operationCombo.SelectedIndex = 0;
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
        private void apiCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckAPI();
            isSendButtonEnabled = false;
            if (apiCombo.SelectedIndex != -1)
            {
                isSendButtonEnabled = true;
            }
            this.ChangeFormState();
        }
        private void operationCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            isSendButtonEnabled = false;
            if (operationCombo.SelectedIndex != -1)
            {
                isSendButtonEnabled = true;
            }

            this.ChangeFormState();
            this.CheckAPI();
        }
        private void actionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CheckAPI();
        }
        private void ChangeFormState()
        {
            if (!isSendButtonEnabled && !isConnected)
            {
                this.Cursor = Cursors.WaitCursor;
                this.btnCallApi.Enabled = false;
            }

            else
            {
                this.Cursor = Cursors.Default;
                if (operationCombo.SelectedIndex != -1 && apiCombo.SelectedIndex != -1)
                {
                    this.btnCallApi.Enabled = true;
                }
            }
        }
        private void CheckAPI()
        {
            actionCombo.Visible = false;
            ActionLabel.Visible = false;
            string requestText = "";
            if (requestText != "")
                txtRequestBody.Text = requestText;
        }
        private void btnCallApi_Click(object sender, EventArgs e)
        {
            this.xmlBrowser1.DocumentText = "";
            this.xmlBrowser1.XmlText = "";
            txtReponseCode.Text = "";

            this.CallEntityApi(apiName);
        }
        private async void CallEntityApi(string api)
        {
            this.Cursor = Cursors.WaitCursor;

            if (!string.IsNullOrEmpty(this.token))
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 10, 0); // 10 minutes

                    client.DefaultRequestHeaders.Add("Token", token);
                    var content = new StringContent(txtRequestBody.Text, Encoding.UTF8, txtRequestContentType.Text);
                    HttpResponseMessage response = new HttpResponseMessage();

                    if (operationCombo.SelectedIndex == 0)
                    {
                        response = await client.PostAsync(txtServerUrl.Text + "/" + api, content);
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

            this.Cursor = Cursors.Default;
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
        private void btnConnect_Click_1(object sender, EventArgs e)
        {
            txtServerUrl.Text = "http://test.logitudeworld.com/test/api/";
            txtCredentialsPrimary.Text = "8234e5d4-e6c5-47c8-8c2d-67875e5a3edf";
            this.LoginWithCredentials();
        }
        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtRequestBody.Text);
        }
        private void btnCopyResponseBody_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(xmlBrowser1.XmlText);
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
        private void button2_Click(object sender, EventArgs e)
        {
            txtRequestBody.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            txtRequestBody.Text = Clipboard.GetText();

        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.LoginWithCredentials();
        }
    }
}

