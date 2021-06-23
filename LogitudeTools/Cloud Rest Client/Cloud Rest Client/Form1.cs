using Cloud_Rest_Client.Models;
using Json2KeyValue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Cloud_Rest_Client
{
    public partial class Form1 : Form
    {
        string Token;
        public Form1()
        {
            InitializeComponent();

            this.cmbAPI.SelectedIndex = 0;
            this.cmbOperations.SelectedIndex = 0;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.LoginWithCredentials();
        }

        private bool isConnected;
        private async void LoginWithCredentials()
        {
            try
            {
                this.ChangeFormState(true);
                isConnected = false;
                UserCredentials authModel = new UserCredentials()
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                };

                using (var client = new HttpClient())
                {
                    string AuthURI = txtServerUrl.Text + "authn";
                    var serializedObject = JsonConvert.SerializeObject(authModel);
                    var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    var httpResponse = await client.PostAsync(AuthURI, content);
                    var result = httpResponse.Content.ReadAsStringAsync().Result;
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse>(result);
                        Token = apiResponse.Token;

                        lblMessage.Text = "Connected!";
                        lblMessage.ForeColor = Color.Green;
                        isConnected = true;
                    }
                    else
                    {
                        lblMessage.Text = "Authentication Error!" + Environment.NewLine + result;
                        lblMessage.ForeColor = Color.Red;
                    }

                }

               

            }

            catch (Exception ex)
            {
                lblMessage.Text = "Authentication Error: " + ex.Message + Environment.NewLine + ex.InnerException?.Message;
                lblMessage.ForeColor = Color.Red;
            }

            this.ChangeFormState();
        }

        private bool isSendButtonEnabled = false;
        private void ChangeFormState(bool waiting = false)
        {
            this.Cursor = waiting ? Cursors.WaitCursor : Cursors.Default;
            if (!isSendButtonEnabled && !isConnected)
            {
                this.btnCallApi.Enabled = false;
            }
            else
            {
                if (cmbOperations.SelectedIndex != -1 && cmbOperations.SelectedIndex != -1)
                {
                    this.btnCallApi.Enabled = true;
                }
            }
        }


        private void btnCallApi_Click(object sender, EventArgs e)
        {
            this.xmlBrowser1.DocumentText = "";
            this.xmlBrowser1.XmlText = "";
            txtReponseCode.Text = "";

            this.CallEntityApi(cmbAPI.SelectedItem.ToString());
        }

        private async void CallEntityApi(string api)
        {
            

            if (string.IsNullOrEmpty(this.Token))
            {
                lblMessage.Text = "Authentication Error!";
                lblMessage.ForeColor = Color.Red;

                return;
            }
             
            using (var client = new HttpClient())
            {
                this.ChangeFormState(true);

                client.DefaultRequestHeaders.Add("Token", Token);
                HttpResponseMessage response = new HttpResponseMessage();
                client.DefaultRequestHeaders.Add("Accept", "application/xml");
                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?housenumber=" + txtHouseNumber.Text + "&&latestStatusOnly=" + cbxLatestStatusOnly.Checked);
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

                //isSendButtonEnabled = true;
            }

            this.ChangeFormState();



            this.Cursor = Cursors.Default;
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

        private void btnCopyResponseBody_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(xmlBrowser1.XmlText);
        }
    }
}
