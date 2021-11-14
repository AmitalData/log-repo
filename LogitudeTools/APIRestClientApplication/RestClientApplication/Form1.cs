
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
using System.Xml.Serialization;

namespace RestClientApplication
{
    public partial class Form1 : Form
    {
        //string uri = "http://localhost:9996/api/";
        string Token;
        APIResponseParameters responseParameters = null;
        public Form1()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            this.BuildOperationComboBox();
            this.actionCombo.Items.Add("Accept");
            this.actionCombo.Items.Add("Decline");
            this.actionCombo.Items.Add("Cancel");

            this.apiCombo.Items.Add("Vendor");
            this.apiCombo.Items.Add("CargoTrackingShipmentDetails");


        }

        private void BuildOperationComboBox()
        {
            if (apiCombo.SelectedItem != null)
            {
                if (apiCombo.SelectedItem.Equals("Rates Update"))
                {
                    this.operationCombo.SelectedItem = null;
                    this.operationCombo.Items.Clear();
                    this.operationCombo.Items.Add("Create (POST)");
                }
                else
                {
                    AddGeneralOperationsToComboBox();
                }
            }
            else
            {
                AddGeneralOperationsToComboBox();
            }
        }

        private void AddGeneralOperationsToComboBox()
        {
            this.operationCombo.SelectedItem = null;
            this.operationCombo.Items.Clear();
            this.operationCombo.Items.Add("Create (POST)");
            this.operationCombo.Items.Add("Update (PUT)");
            this.operationCombo.Items.Add("Get");
            this.operationCombo.Items.Add("Cancel");
        }


        private bool isConnected;
        private async void LoginWithCredentials()
        {
            try
            {
                this.apiCombo.Items.Clear();
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
                        Token = User.Token;

                        if (User.HasError)
                        {
                            lblMessage.Text = "Authentication Error!";
                            lblMessage.ForeColor = Color.Red;
                        }

                        else
                        {
                            GetUserTenantAPIsParameters(User.Token, AuthURI);
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
        private async void GetUserTenantAPIsParameters(string token, string AuthURL)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    AuthURL += "/" + "?token=" + token;
                    var result = await client.GetAsync(AuthURL);
                    var responseBody = result.Content.ReadAsStringAsync().Result;
                    responseParameters = JsonConvert.DeserializeObject<APIResponseParameters>(responseBody);
                    BuildApisNamesList(responseParameters.APIsNames);
                }
            }
            catch
            {
                throw new Exception();
            }

        }
        private void BuildApisNamesList(List<string> names)
        {
            if (names != null)
            {
                foreach (string name in names)
                {
                    this.apiCombo.Items.Add(name);
                }
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

            groupBox3.Visible = apiCombo.Text == "ARInvoice";
            this.BuildOperationComboBox();

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
            this.BuildOperationComboBox();
        }

        private bool isSendButtonEnabled;
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

        private string apiName = "";
        private void CheckAPI()
        {
            actionCombo.Visible = false;
            ActionLabel.Visible = false;
            string requestText = "";
            txtParameter.Visible = false;
            lblParameter.Visible = false;

            txtParameter2.Visible = false;
            lblParameter2.Visible = false;
            textBox1.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            textBox2.Visible = false;
            label12.Visible = false;
            textBox3.Visible = false;
            textBox7.Visible = false;
            textBox4.Visible = false;
            label16.Visible = false;

            panel1.Visible = false;
            switch (apiCombo.SelectedItem)
            {
                #region House
                case "House":
                    {
                        apiName = "house";

                        switch (operationCombo.SelectedIndex)
                        {
                            case 0:
                                {
                                    requestText = responseParameters.XMLRequestText["PostHouse"];
                                    break;
                                }

                            case 1:
                                {
                                    requestText = responseParameters.XMLRequestText["PutHouse"];
                                    break;
                                }

                            case 2:
                                lblParameter.Text = "Number";
                                lblParameter2.Text = "Id";

                                txtParameter.Visible = true;
                                lblParameter.Visible = true;

                                txtParameter2.Visible = true;
                                lblParameter2.Visible = true;
                                break;
                        }
                        break;
                    }
                #endregion

                #region Direct
                case "Direct":
                    {

                        if (operationCombo.SelectedIndex == 2)
                        {
                            lblParameter.Text = "Number";
                            lblParameter2.Text = "Id";

                            txtParameter.Visible = true;
                            lblParameter.Visible = true;

                            txtParameter2.Visible = true;
                            lblParameter2.Visible = true;
                        }

                        apiName = "direct";

                        requestText = responseParameters.XMLRequestText["PostDirect"];
                        break;
                    }
                #endregion

                #region Customs
                case "Customs":
                    {
                        if (operationCombo.SelectedIndex == 2)
                        {
                            lblParameter.Text = "Number";
                            lblParameter2.Text = "Id";

                            txtParameter.Visible = true;
                            lblParameter.Visible = true;

                            txtParameter2.Visible = true;
                            lblParameter2.Visible = true;
                        }

                        apiName = "customs";
                        requestText = responseParameters.XMLRequestText["Customs"];
                        break;
                    }
                #endregion

                #region Quote
                case "Quote":
                    {
                        actionCombo.Visible = true;
                        ActionLabel.Visible = true;
                        apiName = "quotestatus";

                        switch (actionCombo.SelectedIndex)
                        {
                            case 0:
                                {
                                    requestText = responseParameters.XMLRequestText["AcceptQuote"];
                                    break;
                                }

                            case 1:
                                {
                                    requestText = responseParameters.XMLRequestText["DeclineQuote"];
                                    break;
                                }

                            case 2:
                                {
                                    requestText = responseParameters.XMLRequestText["CancleQuote"];
                                    break;
                                }



                            default:
                                {
                                    requestText = responseParameters.XMLRequestText["DefultQuoteStatus"];
                                    break;
                                }

                        }

                        break;
                    }
                #endregion

                #region ARInvoice
                case "ARInvoice":
                    {
                        textBox4.Visible = true;
                        lblParameter.Visible = true;
                        lblParameter.Text = "Id:";

                        txtParameter2.Visible = true;
                        lblParameter2.Visible = true;
                        lblParameter2.Text = "Number:";
                        apiName = "ARInvoice";
                        requestText = responseParameters.XMLRequestText["ARInvoice"];
                        break;
                    }
                #endregion



                case "GLAccount":
                    {
                        textBox1.Visible = true;
                        label10.Visible = true;
                        label11.Visible = true;
                        textBox2.Visible = true;
                        label12.Visible = true;
                        textBox3.Visible = true;
                        apiName = "GLAccount";
                        XmlDocument xmldoc = new XmlDocument();

                        FileStream fs = new FileStream("GLAccountXML.xml", FileMode.Open, FileAccess.Read);
                        xmldoc.Load(fs);
                        requestText = xmldoc.InnerXml.ToString();


                        break;
                    }

                case "Journal":
                    {
                        textBox4.Visible = true;
                        lblParameter.Visible = true;
                        lblParameter.Text = "Id:";

                        txtParameter2.Visible = true;
                        lblParameter2.Visible = true;
                        lblParameter2.Text = "Number:";
                        panel1.Visible = true;
                        //txtParameter2.Visible = true;
                        //lblParameter2.Visible = true;
                        apiName = "Journal";

                        break;
                    }



                #region Master
                case "Master":
                    {
                        apiName = "master";
                        requestText = responseParameters.XMLRequestText["PostMaster"];
                        break;
                    }
                #endregion

                #region Customer
                case "Customer":
                    {
                        apiName = "customer";
                        requestText = responseParameters.XMLRequestText["Customer"];
                        break;
                    }
                #endregion

                #region Vendor
                case "Vendor":
                    {
                        txtParameter2.Visible = true;
                        apiName = "vendorpartner";
                      //  requestText = responseParameters.XMLRequestText["Vendor"];
                        break;
                    }
                #endregion

                #region ARPayment
                case "ARPayment":
                    {
                        textBox4.Visible = true;
                        lblParameter.Visible = true;
                        lblParameter.Text = "Id:";

                        txtParameter2.Visible = true;
                        lblParameter2.Visible = true;
                        lblParameter2.Text = "Number:";
                        apiName = "ARPayment";

                        break;
                    }
                #endregion

                #region cancel ARPayment
                case "Cancel ARPayment":
                    {
                        apiName = "ARPaymentCancellation";
                        this.operationCombo.SelectedIndex = 1;
                        break;
                    }

                #endregion

                #region APInvoice
                case "APInvoice":
                    {
                        textBox4.Visible = true;
                        lblParameter.Visible = true;
                        lblParameter.Text = "Id:";

                        txtParameter2.Visible = true;
                        lblParameter2.Visible = true;
                        lblParameter2.Text = "Number:";
                        label16.Visible = true;
                        label16.Text = "External ID:";
                        textBox7.Visible = true;
                        apiName = "APInvoice";

                        break;
                    }
                #endregion


                #region ARInvoiceAdditionalData
                case "ARInvoice Additional Data":
                    {
                        apiName = "ARInvoiceAdditionalData";
                        break;
                    }
                #endregion


                #region CustomerOpenFilesAmount
                case "Customer Open Files Amount":
                    {

                        apiName = "CustomerOpenFilesAmount";

                        break;
                    }
                #endregion

                #region APInvoiceCancellation
                case "APInvoice Cancellation":
                    {

                        lblParameter.Text = "External ID:";
                        lblParameter.Visible = true;
                        textBox4.Visible = true;
                        apiName = "APInvoiceCancellation";
                        break;
                    }

                #endregion

                #region GLAccountMoreData

                case "GL Account More Data":
                    {

                        lblParameter.Text = "Internal NO.:";
                        lblParameter.Visible = true;
                        txtParameter.Visible = true;
                        apiName = "GLAccountMoreData";
                        break;
                    }

                #endregion

                #region Rates Update
                case "Rates Update":
                    {
                        lblParameter.Visible = false;
                        txtParameter.Visible = false;
                        requestText = responseParameters.XMLRequestText["RatesUpdate"];
                        apiName = "RatesUpdate";
                        break;
                    }
                #endregion 

                #region CargoTrackingShipmentDetails
                case "Cargo Tracking Shipment Details":
                    {
                        lblParameter.Text = "House:";
                        lblParameter.Visible = true;
                        txtParameter.Visible = true;
                        apiName = "CargoTrackingShipmentDetails";
                        break;
                    }
                    #endregion

            }

            txtRequestBody.Text = requestText;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

            if (!string.IsNullOrEmpty(this.Token))
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 10, 0); // 10 minutes

                    client.DefaultRequestHeaders.Add("Token", Token);
                    //client.DefaultRequestHeaders.Add("WorkerRoleName", "development");

                    var content = new StringContent(txtRequestBody.Text, Encoding.UTF8, txtRequestContentType.Text);
                    HttpResponseMessage response = new HttpResponseMessage();

                    if (operationCombo.SelectedIndex == 0)
                    {
                        response = await client.PostAsync(txtServerUrl.Text + "/" + api, content);
                    }
                    else if (operationCombo.SelectedIndex == 1)
                    {
                        response = await client.PutAsync(txtServerUrl.Text + "/" + api, content);
                    }
                    else if (operationCombo.SelectedIndex == 2)
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/xml");

                        if (!string.IsNullOrEmpty(txtParameter.Text))
                        {
                            response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?number=" + txtParameter.Text);
                        }
                        else
                        {

                            if (apiName == "GLAccount")
                            {
                                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?id=" + textBox1.Text + "&DisplayNumber=" + textBox2.Text + "&InternalNumber=" + textBox3.Text);
                            }
                            else if (apiName == "ARInvoice" || apiName == "ARPayment")
                            {
                                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?id=" + textBox4.Text + "&number=" + txtParameter2.Text);
                            }
                            else if (apiName == "APInvoice")
                            {

                                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?id=" + textBox4.Text + "&number=" + txtParameter2.Text + "&externalId=" + textBox7.Text + "&internalNumber=" + null);
                            }
                            else if (apiName == "Journal")
                            {
                                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?id=" + textBox4.Text + "&number=" + txtParameter2.Text + "&externalNo=" + textBox6.Text + "&externalSystem=" + textBox5.Text);
                            }
                            else
                                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?id=" + txtParameter2.Text);

                        }


                    }

                    else if (operationCombo.SelectedIndex == 3)
                    {
                        if (apiName == "APInvoiceCancellation")
                        {
                            client.DefaultRequestHeaders.Add("Accept", "application/xml");
                            response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?externalId=" + textBox4.Text);

                        }
                        else
                        {
                            response = await client.DeleteAsync(txtServerUrl.Text + "/" + api + "?entityXML=" + txtRequestBody.Text);
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

                    //isSendButtonEnabled = true;
                }

                //this.ChangeFormState();
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
            txtCredentialsPrimary.Text = "8234e5d4-e6c5-47c8-8c2d-67875e5a3edf"; //"7df4e51e-8a83-4dff-a9b4-654539f0b9aa";
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

        private void txtCredentialsPrimary_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void GetARInvoiceBtn_Click(object sender, EventArgs e)
        {
            //ModifyXmlFields(txtRequestBody.Text);
            //ModifyInvoiceFields(txtRequestBody.Text);
        }
        private void CopyResponse1000Btn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(responseBody);

        }

        private void ModifyInvoiceFields(string xmlResult)
        {
            // Deserilaize
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ARInvoice";
            xRoot.IsNullable = true;
            string xmlIN = xmlResult;
            StringReader stringReader = new StringReader(xmlIN);
            XmlSerializer serializer = new XmlSerializer(typeof(ARInvoice), xRoot);
            ARInvoice invoicePM = serializer.Deserialize(stringReader) as ARInvoice;

            // Modify Amounts

            // Serialize
            var stringwriter = new StringWriter();
            var serializer2 = new XmlSerializer(typeof(ARInvoice));
            serializer2.Serialize(stringwriter, invoicePM);
            string xmlOUT = stringwriter.ToString();

            // Send Request
            invoiceXML = xmlOUT.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");



            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlResult);
            var root = doc.FirstChild;

            if (root.Attributes["Id"] != null)
            {
                root.Attributes.Remove(root.Attributes["Id"]);
            }

            this.invoiceXML = doc.InnerXml;

        }


        private string ModifyXmlFields(string xmlResult, int iterateNumber)
        {
            return xmlResult;

            decimal number = iterateNumber * 100;

            var doc = new XmlDocument();
            doc.LoadXml(xmlResult);
            var root = doc.FirstChild;

            if (root.HasChildNodes)
            {
                XmlNodeList x = root.ChildNodes;
                string sss;
                foreach (XmlNode item in x)
                {
                    if (item.Name == "ARInvoiceLines")
                    {
                        //item = lines
                        foreach (XmlNode line in item.ChildNodes)
                        {
                            foreach (XmlNode lineNode in line.ChildNodes)
                            { // invoice line
                                if (lineNode.Name == "UnitPriceInForeignCurrency") lineNode.InnerText = number.ToString();
                                if (lineNode.Name == "LocalCurrencyAmount") lineNode.InnerText = number.ToString();
                                if (lineNode.Name == "ForeignCurrencyAmount") lineNode.InnerText = number.ToString();
                                if (lineNode.Name == "InvoiceCurrencyAmount") lineNode.InnerText = number.ToString();
                                if (lineNode.Name == "ProfitCurrencyAmount") lineNode.InnerText = "0";
                            }
                        }
                    }

                    if (item.Name == "SubTotalInInvoiceCurrency") item.InnerText = number.ToString();
                    if (item.Name == "SubTotalInLocalCurrency") item.InnerText = number.ToString();
                    if (item.Name == "AmountInInvoiceCurrency") item.InnerText = number.ToString();
                    if (item.Name == "AmountInLocalCurrency") item.InnerText = number.ToString();
                    if (item.Name == "AmountInProfitCurrency") item.InnerText = "0";
                }

            }

            return doc.InnerXml;
        }


        string invoiceXML;
        private async void Post1000ARInvoice_ClickAsync(object sender, EventArgs e)
        {
            CopyResponse.Visible = false;

            int length = Convert.ToInt32(postCountTxt.Text);
            this.Cursor = Cursors.WaitCursor;


            if (!string.IsNullOrEmpty(this.Token))
            {
                string resultTxt = "";
                for (int i = 0; i < length; i++)
                {

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Token", Token);

                        var content = new StringContent(ModifyXmlFields(txtRequestBody.Text, i + 1), Encoding.UTF8, "application/xml");

                        //var content = new StringContent(txtRequestBody.Text, Encoding.UTF8, "application/xml");

                        HttpResponseMessage response = new HttpResponseMessage();

                        response = await client.PostAsync(txtServerUrl.Text + "/" + apiName, content);

                        txtReponseCode.Text = ((int)response.StatusCode).ToString();

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var resultData = response.Content.ReadAsStringAsync().Result;
                            resultTxt += resultData;
                        }

                        else
                        {
                            var resultData2 = response.Content.ReadAsStringAsync().Result;
                            resultTxt += resultData2;
                        }

                    }
                    count1000.Text = "Creating Invoices: " + (i + 1) + "/" + length;
                } // eof for
                count1000.Text = postCountTxt.Text + " Invoices Created";

                CopyResponse.Visible = true;
                responseBody = resultTxt;
                MessageBox.Show("Invoices created successfully");
                //this.SetXmlBrouserXml(resultTxt);
            }

            else
            {
                lblMessage.Text = "Authentication Error!";
                lblMessage.ForeColor = Color.Red;
            }

            this.Cursor = Cursors.Default;
        }

        string responseBody;

        void SerializeARInvoice()
        {

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
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
