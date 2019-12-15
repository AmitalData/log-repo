using DeclarationApprovalRequestTester.AccountingPartnerServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeclarationApprovalRequestTester
{
    public partial class Form1 : Form
    {
        string Token = "";
        public Form1()
        {
            InitializeComponent();
            txtUserName.Text = "admin@fnarsoft.com";
            txtPassword.Text = "1";
            txtShipmentNumber.Enabled = false;
            txtTenant.Enabled = false;
            txtDeclarationXmlData.Enabled = false;
            tabControl1.Enabled = false;
            txtAccountingPartnerData.Text = "<?xml version='1.0' ?><AccountingPartnerPM xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Id>1-1316</Id><Tenant>1</Tenant><IsSecured>false</IsSecured><IsHybrid>false</IsHybrid><EnglishName>aaaaa</EnglishName><LocalName>aaaaa</LocalName><InActive>false</InActive><Code>1002</Code><CreateDate>2019-10-28T12:15:42.7877893Z</CreateDate><UpdateDate>2019-10-28T12:15:42.7877893Z</UpdateDate><CreatedByUserId>1-1</CreatedByUserId><UpdatedByUserId>1-1</UpdatedByUserId><PartnerTypeId>AC</PartnerTypeId><FieldsChanged>false</FieldsChanged><SearchFields>1002,aaaaa,aaaaa</SearchFields><IsExternal>false</IsExternal><IsFirstContactToAdd>false</IsFirstContactToAdd><EnableConsolidationInvoices>false</EnableConsolidationInvoices><Addresses /><Contacts /><CardExternalCodeByCurrencies /></AccountingPartnerPM>";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabDec")
            {
                SendDecleration();
            }
            else if (tabControl1.SelectedTab.Name == "tabAP")
            {
                SendAccountingPartner();
            }

        }

        private void txtGetToken_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Enter User Name and Password");
                return;
            }
            LoginWcfServiceReference.LoginWcfServiceClient loginService = new LoginWcfServiceReference.LoginWcfServiceClient();
            LoginWcfServiceReference.Response loginResponse = loginService.Login(txtUserName.Text, txtPassword.Text);
            if (!loginResponse.HasError)
            {
                Token = loginResponse.Result;
                txtShipmentNumber.Enabled = true;
                txtTenant.Enabled = true;
                txtDeclarationXmlData.Enabled = true;
                tabControl1.Enabled = true;

            }
        }

        private void SendDecleration()
        {
            if (string.IsNullOrEmpty(txtTenant.Text) || string.IsNullOrEmpty(txtShipmentNumber.Text))
            {
                MessageBox.Show("Enter Tenant # and Shipment #");
                return;
            }
            int Tenant = 0;//
            int.TryParse(txtTenant.Text, out Tenant);
            string ShipmentNumber = txtShipmentNumber.Text;
            string DeclarationXmlData = txtDeclarationXmlData.Text;
            DeclarationApprovalRequestServiceReference.DeclarationApprovalRequestPM DeclarationApprovalRequestPM = new DeclarationApprovalRequestServiceReference.DeclarationApprovalRequestPM();
            DeclarationApprovalRequestPM.Tenant = Tenant;
            DeclarationApprovalRequestPM.ForwarderShipmentNumber = ShipmentNumber;
            DeclarationApprovalRequestPM.DeclarationXmlData = DeclarationXmlData;

            DeclarationApprovalRequestServiceReference.DeclarationApprovalRequestWcfServiceClient MyDeclarationApprovalClient = new DeclarationApprovalRequestServiceReference.DeclarationApprovalRequestWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)MyDeclarationApprovalClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                MyDeclarationApprovalClient.RequestDeclarationApproval(DeclarationApprovalRequestPM);
            }
        }

        private void SendAccountingPartner()
        {
            string AccountingPartnerXmlData = txtAccountingPartnerData.Text;
            //AccountingPartnerServiceReference.AccountingPartnerPM AccountingPartnerPM = new AccountingPartnerServiceReference.AccountingPartnerPM();
            byte[] byteArray = Encoding.ASCII.GetBytes(AccountingPartnerXmlData);
            MemoryStream stream = new MemoryStream(byteArray);

            // convert stream to string
            //StreamReader reader = new StreamReader(stream);
            //string text = reader.ReadToEnd();

            XmlSerializer xsSubmit = new XmlSerializer(typeof(AccountingPartnerPM));
            AccountingPartnerPM AccountingPartnerPM = xsSubmit.Deserialize(stream) as AccountingPartnerPM;

            AccountingPartnerServiceReference.AccountingPartnerWcfServiceClient MyAccountingPartnerClient = new AccountingPartnerServiceReference.AccountingPartnerWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)MyAccountingPartnerClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                var response = MyAccountingPartnerClient.Upsert(AccountingPartnerPM,false);
                if (response.HasError)
                {
                    MessageBox.Show(response.ErrorMessage,"Error");
                }
                else
                {
                    MessageBox.Show("Accounting Partner Upserted Successfully","Sucssess");
                }
            }
        }
    }
}
