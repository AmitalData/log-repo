using Json2KeyValue;
using CloudRestClientTool;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Xml;

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
        }


        private void SetDefaultValues()
        {
            txtServerUrl.Text = "http://localhost:9996/api/";
            txtCredentialsPrimary.Text = "518eb8ea-ad91-48ea-8b0a-fe7b736ea0c8";
            string requestText = @"<ShipmentOrder xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' 
            xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Tenant>1</Tenant><OrderNumber>1</OrderNumber>
            <TransportMode Code='A'><Name>Air</Name></TransportMode><Consignee Code='10011'>
            <EnglishName>AyahAgent</EnglishName><LocalName>AyahAgent</LocalName><MainAddress><Name>AyahAgent</Name>
            <Address1>add1</Address1><Address2>add2</Address2><Country Code='ES'><EnglishName>Spain225</EnglishName>
            <LocalName>Spain</LocalName></Country><City>City</City><ZipCode>213</ZipCode><PhoneNumber>0598078666</PhoneNumber>
            <FaxNumber>12321312</FaxNumber><AddressType><Name>Main</Name></AddressType></MainAddress>
            <IsDisconnectedFromGLAccount>false</IsDisconnectedFromGLAccount><ICAO /></Consignee><Shipper Code='10011'>
            <EnglishName>AyahAgent</EnglishName><LocalName>AyahAgent</LocalName><MainAddress><Name>AyahAgent</Name><Address1>add1</Address1>
            <Address2>add2</Address2><Country Code='ES'><EnglishName>Spain225</EnglishName><LocalName>Spain</LocalName></Country><City>City
            </City><ZipCode>213</ZipCode><PhoneNumber>0598078666</PhoneNumber><FaxNumber>12321312</FaxNumber><AddressType><Name>Main</Name>
            </AddressType></MainAddress><IsDisconnectedFromGLAccount>false</IsDisconnectedFromGLAccount><ICAO /></Shipper><Agent Code='10011'>
            <EnglishName>AyahAgent</EnglishName><LocalName>AyahAgent</LocalName><MainAddress><Name>AyahAgent</Name><Address1>add1</Address1>
            <Address2>add2</Address2><Country Code='ES'><EnglishName>Spain225</EnglishName><LocalName>Spain</LocalName></Country><City>City</City>
            <ZipCode>213</ZipCode><PhoneNumber>0598078666</PhoneNumber><FaxNumber>12321312</FaxNumber><AddressType><Name>Main</Name></AddressType>
            </MainAddress><IsDisconnectedFromGLAccount>false</IsDisconnectedFromGLAccount><ICAO /></Agent><Incoterm Code='CFR'><Name>Cost &amp; Freight 5555</Name>
            </Incoterm><AccountManager Code='mog@mail.com'><EnglishName>luffy1 </EnglishName><LocalName>luffy</LocalName></AccountManager><PONumber>1234</PONumber>
            <ShipmentType Code='Air'><Name>Air</Name></ShipmentType><House>house test</House><Vessel Code='rrr'><EnglishName>123</EnglishName></Vessel><CustomsAgent Code='10011'>
            <EnglishName>AyahAgent</EnglishName><LocalName>AyahAgent</LocalName><MainAddress><Name>AyahAgent</Name><Address1>add1</Address1><Address2>add2</Address2><Country Code='ES'>
            <EnglishName>Spain225</EnglishName><LocalName>Spain</LocalName></Country><City>City</City><ZipCode>213</ZipCode><PhoneNumber>0598078666</PhoneNumber><FaxNumber>12321312</FaxNumber>
            <AddressType><Name>Main</Name></AddressType></MainAddress><IsDisconnectedFromGLAccount>false</IsDisconnectedFromGLAccount><ICAO /></CustomsAgent><SpecialServicesType Code='5'>
            <Name>Air Express</Name></SpecialServicesType><Forwarder Code='TW'><EnglishName>Trans World Airlines</EnglishName><LocalName>Trans World Airlines</LocalName>
            <IsDisconnectedFromGLAccount>false</IsDisconnectedFromGLAccount></Forwarder><IsReadyForPickup>false</IsReadyForPickup>
            <DescriptionOfGoods>desc test</DescriptionOfGoods><CustomerReferences>customer ref test</CustomerReferences></ShipmentOrder>";

            txtRequestBody.Text = requestText;
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

            isSendButtonEnabled = operationCombo.SelectedIndex != -1 ? true:false;
            ShipmentOrderParameterTextBox.Visible = operationCombo.SelectedIndex == 2 ? true : false;
            lblParameter.Visible =  operationCombo.SelectedIndex == 2 ? true : false;
            
            ChangeFormState();
        }


        private async void CallEntityApi()
        {
            this.Cursor = Cursors.WaitCursor;

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

        private void ShipmentOrderParameterTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

