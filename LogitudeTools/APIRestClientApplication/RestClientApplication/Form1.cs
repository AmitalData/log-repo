
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

        public Form1()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
           

            this.apiCombo.Items.Add("House");
            this.apiCombo.Items.Add("Direct");
            this.apiCombo.Items.Add("Customs");
            this.apiCombo.Items.Add("Quote");
            this.apiCombo.Items.Add("ARInvoice");
            this.apiCombo.Items.Add("GLAccount");
            this.apiCombo.Items.Add("Journal");
            this.apiCombo.Items.Add("Master");
            this.apiCombo.Items.Add("Customer");
            this.apiCombo.Items.Add("Vendor");

            this.apiCombo.Items.Add("ARPayment");
            this.apiCombo.Items.Add("Cancel ARPayment");
            this.apiCombo.Items.Add("APInvoice");
            this.apiCombo.Items.Add("ARInvoiceAdditionalData");
            this.apiCombo.Items.Add("CustomerOpenFilesAmount");
            apiCombo.Items.Add("APInvoiceCancellation");
            apiCombo.Items.Add("GLAccountMoreData");
            this.operationCombo.Items.Add("Create (POST)");
            this.operationCombo.Items.Add("Update (PUT)");
            this.operationCombo.Items.Add("Get");
            this.operationCombo.Items.Add("Cancel");

            this.actionCombo.Items.Add("Accept");
            this.actionCombo.Items.Add("Decline");
            this.actionCombo.Items.Add("Cancel");
        }

        private bool isConnected;
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
                        Token = User.Token;

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

            groupBox3.Visible = apiCombo.Text == "ARInvoice";

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
            switch (apiCombo.SelectedIndex)
            {
                #region House
                case 0:
                    {
                        apiName = "house";

                        switch (operationCombo.SelectedIndex)
                        {
                            case 0:
                                {
                                    requestText = @"<House>
                                                       <Direction Code='E' />
                                                       <TransportMode Code='A' />
                                                       <Shipper Code='70002' />
                                                       <ShipperReference1>SR1</ShipperReference1>
                                                       <ShipperReference2>SR2</ShipperReference2>
                                                       <Consignee Code='70003' />
                                                       <ConsigneeReference1>CR1</ConsigneeReference1>
                                                       <ConsigneeReference2>CR2</ConsigneeReference2>
                                           <Customer Code='70002' />
<FromPort Code='DE222' />
                                                         <FromPort Code='PFAAA' />
                                                       <ToPort Code='DZAAE' />
                                                       <GrossWeightUnit Code='KG'/>
                                                       <ChargeableWeightUnit Code='KG'/>
                                                       <VolumeUnit  Code='CBM' />
                                                       <Commodity>653</Commodity>
                                                       <Incoterm Code='CIF' />
                                                       <HouseNo>00001146</HouseNo>
                                                       <HouseDate>2017-10-24T00:00:00</HouseDate>
        

                                                            <Receivable>
                                                                <ChargesType Code='AFT'/>
                                                                <Measurement Code='GRWT'/>
                                                                <Currency Code='USD'/>
                                                                <Quantity>10</Quantity>
                                                                <UnitPrice>10</UnitPrice>
                                                            </Receivable>
                                                        </Receivables>

                                                        <Payables>
                                                            <Payable>
                                                                <ChargesType Code='AFT'/>
                                                                <Measurement Code='GRWT'/>
                                                                <Currency Code='USD'/>
                                                                <Quantity>10</Quantity>
                                                                <UnitPrice>10</UnitPrice>
                                                            </Payable>
                                                        </Payables>
<AirPackages>
                                                         
                                                       
                                                    </House>
                                                     ";
                                    break;
                                }

                            case 1:
                                {
                                    requestText = @"<House ShipmentNumber='EXP9115'>
               <Direction Code='E' />
               <TransportMode Code='A' />
               <Shipper Code='70002' />
               <ShipperReference1>SR1</ShipperReference1>
               <ShipperReference2>SR2</ShipperReference2>
               <Consignee Code='70003' />
               <ConsigneeReference1>CR1</ConsigneeReference1>
               <ConsigneeReference2>CR2</ConsigneeReference2>
               <Customer Code='70002' />
               <FromPort Code='DE222' />
               <ToPort Code='DE223' />
               <GrossWeightUnit Code='KG'/>
               <ChargeableWeightUnit Code='KG'/>
               <VolumeUnit  Code='CBM' />
               <Commodity>653</Commodity>
               <Incoterm Code='CIF' />
               <HouseNo>00001146</HouseNo>               
               <HouseDate>2017-10-24T00:00:00</HouseDate>
               <AirPackages>
                  <AirPackage>
                     <Length>25</Length>
                     <Width>50</Width>
                     <Height>70</Height>
                     <Pieces>100</Pieces>
                     <Volume>8.75</Volume>
                     <GrossWeight>220</GrossWeight>
                                            <Reference1>Ref1</Reference1>
                                            <Reference2>Ref2</Reference2>
                                            <Reference3>Ref3</Reference3>
                                            <Commodity>653</Commodity>
                  </AirPackage>
                  <AirPackage>
                     <Length>200</Length>
                     <Width>100</Width>
                     <Height>150</Height>
                     <Pieces>50</Pieces>
                     <Volume>150</Volume>
                     <GrossWeight>200</GrossWeight>
                                              <Reference1>Ref1</Reference1>
                                            <Reference2>Ref2</Reference2>
                                            <Reference3>Ref3</Reference3>
                                            <Commodity>653</Commodity>
                  </AirPackage>
               </AirPackages>              
            </House>
             ";
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
                case 1:
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

                        requestText = @"<Direct xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
               <Direction Code='E' /><!-- E - Export , I - Import , D - Domistic , R - Drop -->
               <TransportMode Code='A' /><!-- A - Air , I - Inland , O - Ocean -->
               <Shipper Code='70002' /><!-- Code or ParnerCode only one Field is requiered-->
               <ShipperReference1>SR1</ShipperReference1>
               <ShipperReference2>SR2</ShipperReference2>
               <Consignee Code='70003' /><!-- Code or ParnerCode only one Field is requiered-->
               <ConsigneeReference1>CR1</ConsigneeReference1>
               <ConsigneeReference2>CR2</ConsigneeReference2>
               <Customer Code='70002' /><!-- Code or ParnerCode only one Field is requiered-->
               <FromPort Code='USJFK' /><!-- Code or ParnerCode only one Field is requiered-->
               <ToPort Code='AUAAB' /><!-- Code or ParnerCode only one Field is requiered-->
               <GrossWeightUnit Code='KG'/>
               <ChargeableWeightUnit Code='KG'/>
               <VolumeUnit  Code='CBM' />
               <Commodity>653</Commodity>
               <Incoterm Code='CIF' /> <!-- Code or ParnerCode only one Field is requiered-->
               <Master>00001146</Master>
               <AirPackages>
                  <AirPackage>
                     <Length>25</Length>
                     <Width>50</Width>
                     <Height>70</Height>
                     <Pieces>100</Pieces>
                     <Volume>8.75</Volume>
                     <GrossWeight>220</GrossWeight>
                                            <Reference1>Ref1</Reference1>
                                            <Reference2>Ref2</Reference2>
                                            <Reference3>Ref3</Reference3>
                                            <Commodity>653</Commodity>
                  </AirPackage>
                  <AirPackage>
                     <Length>200</Length>
                     <Width>100</Width>
                     <Height>150</Height>
                     <Pieces>50</Pieces>
                     <Volume>150</Volume>
                     <GrossWeight>200</GrossWeight>
                                              <Reference1>Ref1</Reference1>
                                            <Reference2>Ref2</Reference2>
                                            <Reference3>Ref3</Reference3>
                                            <Commodity>653</Commodity>
                  </AirPackage>
               </AirPackages>
            </Direct>
             ";
                        break;
                    }
                #endregion

                #region Customs
                case 2:
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
                        requestText = @"<Customs>
                                            <ShipmentNumber>123456</ShipmentNumber>      
                                            <TransportMode Code='I'/>
                                            <ShipperReference1>SR1</ShipperReference1>              
                                            <ShipperReference2> SR2</ShipperReference2>              
                                            <Consignee Code='70000'/>               
                                            <ConsigneeReference1>CR1</ConsigneeReference1>                 
                                            <ConsigneeReference2>CR2</ConsigneeReference2>                 
                                            <Customer Code='70000'/>                 
                                            <FromPort Code='USJFK'/>                    
                                            <ToPort Code='AUAAB'/>
                                            <ChargeableWeightUnit Code='KG'/>                             
                                            <VolumeUnit Code='CBM'/>                              
                                            <Commodity>653</Commodity>                              
                                            <MainCarriageCarrier>AA</MainCarriageCarrier>
                                            <HouseNo>00001146</HouseNo>                              
                                            <HouseDate>2017-10-24T00:00:00</HouseDate>
                                            <CustomsClearanceDate>2017-10-25T00:00:00</CustomsClearanceDate>
                                            <DeclarationNumber>3334</DeclarationNumber>                                              
                                            <Incoterm Code='CIF'/>                                                 
                                            <AirPackages>                                                 
                                                <AirPackage>                                                 
                                                    <Length>25</Length>                                                 
                                                    <Width>50</Width>                                                 
                                                    <Height>70</Height>                                                 
                                                    <Pieces>100</Pieces>                                                 
                                                    <Volume>8.75</Volume>                                                 
                                                    <GrossWeight>220</GrossWeight>                                                 
                                                    <Reference1>Ref1</Reference1>                                                 
                                                    <Reference2>Ref2</Reference2>                                                 
                                                    <Reference3>Ref3</Reference3>                                                 
                                                    <Commodity>653</Commodity>                                                 
                                                </AirPackage>                                                 
                                                <AirPackage>                                                 
                                                    <Length>200</Length>                                                 
                                                    <Width>100</Width>                                                 
                                                    <Height>150</Height>                                                 
                                                    <Pieces>50</Pieces>                                                 
                                                    <Volume>150</Volume>                                                 
                                                    <GrossWeight>200</GrossWeight>                                                 
                                                    <Reference1>Ref1</Reference1>                                                 
                                                    <Reference2>Ref2</Reference2>                                                 
                                                    <Reference3>Ref3</Reference3>                                                 
                                                    <Commodity>653</Commodity>                                                 
                                                </AirPackage>                                                 
                                            </AirPackages>
                                        </Customs>";
                        break;
                    }
                #endregion

                #region Quote
                case 3:
                    {
                        actionCombo.Visible = true;
                        ActionLabel.Visible = true;
                        apiName = "quotestatus";

                        switch (actionCombo.SelectedIndex)
                        {
                            case 0:
                                {
                                    requestText = @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1289</QuoteNumber>
	                                        <QuoteAcceptNote>This quote accepted manually by Samar</QuoteAcceptNote>
	                                        <QuoteAcceptDate>2017-12-31T00:00:00Z</QuoteAcceptDate>
                                            <DueDate>2017-12-31T00:00:00Z</DueDate>
	                                        <Stage Id='1-439' Code='QTAC'>
		                                        <Name>Accepted</Name>		                                        
	                                        </Stage>
                                        </QuoteStatus>                
                                        ";
                                    break;
                                }

                            case 1:
                                {
                                    requestText = @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1289</QuoteNumber>	                                        
	                                        <QuoteDeclineDate>2017-12-31T00:00:00Z</QuoteDeclineDate>
                                            <QuoteDeclineNote>This quote declined manually by Samar</QuoteDeclineNote>
                                            <DueDate>2017-12-31T00:00:00Z</DueDate>
	                                        <QuoteDeclineReason Code='XQ'>
                                                <Name>Expired Quote</Name>
                                            </QuoteDeclineReason>
	                                        <Stage Code='QTDC'>
		                                        <Name>Declined</Name>		                                        
	                                        </Stage>
                                        </QuoteStatus>                
                                        ";
                                    break;
                                }

                            case 2:
                                {
                                    requestText = @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1289</QuoteNumber>
	                                        <IsQuoteCancel xsi:nil='true' />
	                                        <QuoteCancelNote>This quote cancelled by Samar</QuoteCancelNote>
	                                        <QuoteCancelDate>2017-12-31T00:00:00Z</QuoteCancelDate>
                                        </QuoteStatus>                
                                        ";
                                    break;
                                }



                            default:
                                {
                                    requestText = @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1288</QuoteNumber>	                                        <
	                                        <QuoteAcceptNote>This quote accepted manually by Samar</QuoteAcceptNote>
	                                        <QuoteAcceptDate>2017-12-31T00:00:00Z</QuoteAcceptDate>	  
                                            <DueDate>2017-12-31T00:00:00Z</DueDate>                                      
	                                        <Stage Id='1-439' Code='QTAC'>
		                                        <Name>Accepted</Name>		                                        
	                                        </Stage>
                                        </QuoteStatus>                
                                        ";
                                    break;
                                }

                        }

                        break;
                    }
                #endregion

                #region ARInvoice
                case 4:
                    {
                        textBox4.Visible = true;
                        lblParameter.Visible = true;
                        lblParameter.Text = "Id:";

                        txtParameter2.Visible = true;
                        lblParameter2.Visible = true;
                        lblParameter2.Text = "Number:";
                        apiName = "ARInvoice";
                        requestText = @"<ARInvoice xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
  <InvoiceType Code='IN'>

< Name > Invoice </ Name >
</ InvoiceType >
< BillTo  Code = '10009065' >
 

 < EnglishName > Ayman </ EnglishName >
 

 < LocalName > أيمن </ LocalName >


 -< MainAddress Id = '1-132527' >
  

  < Name > Ayman </ Name >
  

  < Address1 > adrr1 </ Address1 >


  < Country  Code = 'HK' >
   

   < EnglishName > Hong Kong </ EnglishName >
      

      < LocalName > Hong Kong </ LocalName >
         

         </ Country >
         

         < City > hong </ City >
         

         < ZipCode > 3333 </ ZipCode >
         

         < PhoneNumber > 23132132131 </ PhoneNumber >
         

         < FaxNumber > 5645646545646 </ FaxNumber >
         

         </ MainAddress >
         

         </ BillTo >
         



         < InvoiceDate > 2018 - 02 - 04T00: 00:00 </ InvoiceDate >
                 

                 < PrintDate xsi: nil = 'true' />
                   

                   < IsPrinted > false </ IsPrinted >
                   

                   < MainEntityReference > 1 </ MainEntityReference >
                   

                   < IsConstituentInvoice > false </ IsConstituentInvoice >
                   

                   < IsConsolidationInvoice > false </ IsConsolidationInvoice >


                   -< InvoiceCurrency  Code = 'USD' >
                    

                    </ InvoiceCurrency >
                    


                    < CancelledByARInvoice > 1 - 96 </ CancelledByARInvoice >


                    -< CreatedByUser  ExternalCode = 'BASEL' >
                     

                     < EnglishName > System </ EnglishName >
                     

                     < LocalName > System </ LocalName >
                     

                     </ CreatedByUser >


                     -< BillToAddress ExternalId = '2002395010' >
                      

                      < Name > Ayman </ Name >
                      

                      < Address1 > adrr1 </ Address1 >


                      -< Country  Code = 'HK' >
                       


                       </ Country >
                       

                       < City > hong </ City >
                       

                       < ZipCode > 3333 </ ZipCode >
                       

                       < PhoneNumber > 23132132131 </ PhoneNumber >
                       

                       < FaxNumber > 5645646545646 </ FaxNumber >
                       

                       </ BillToAddress >
                       

                       < IssuedByUser ExternalCode = 'BASEL' >
                        </ IssuedByUser >
                        

                        < InvoiceCurrencyExchangeRate > 4 </ InvoiceCurrencyExchangeRate >


                        -< ARInvoiceLines >


                        -< ARInvoiceLine >
                        



                        < LineNumber > 1 </ LineNumber >


                        -< ChargesType  Code = 'AFT' >
                         



                         </ ChargesType >
                         
                          < Quantity > 1 </ Quantity >
                         < UnitPriceInForeignCurrency > 1 </ UnitPriceInForeignCurrency >

                         -< ForeignCurrency  Code = 'EUR' >
                          



                          </ ForeignCurrency >
                          


                          < ForeignExchangeRate > 5 </ ForeignExchangeRate >
                          

                          < LocalCurrencyAmount > 5 </ LocalCurrencyAmount >
                          

                          < ForeignCurrencyAmount > 1 </ ForeignCurrencyAmount >
                          

                          < VatPercentage > 0 </ VatPercentage >
                          

                          < ValueDate xsi: nil = 'true' />
                            

                            < DateForInterest xsi: nil = 'true' />


                              -< VatType Code = 'STD' >
                               



                               </ VatType >
                               



                               < InvoiceCurrencyAmount > 1.25 </ InvoiceCurrencyAmount >
                               

                               < ProfitCurrencyAmount > 4000 </ ProfitCurrencyAmount >
                               

                               </ ARInvoiceLine >
                               

                               </ ARInvoiceLines >
                               

                               < DueDate > 2012 - 07 - 04T00: 00:00 </ DueDate >
                                       

                                       < SubTotalInInvoiceCurrency > 1.25 </ SubTotalInInvoiceCurrency >
                                       

                                       < SubTotalInLocalCurrency > 5 </ SubTotalInLocalCurrency >
                                       

                                       < AmountInInvoiceCurrency > 1.25 </ AmountInInvoiceCurrency >
                                       
                                        < AmountInLocalCurrency > 5 </ AmountInLocalCurrency >
                                       




                                       < ProfitCurrencyExchangeRate > 5 </ ProfitCurrencyExchangeRate >
                                       

                                       < AmountInProfitCurrency > 4000 </ AmountInProfitCurrency >


                                       -< TransferStatus Code = 'NR' >
                                        



                                        </ TransferStatus >


                                        -< Branch Code = 'TLV' >
                                         


                                         </ Branch >


                                         -< LocalCurrency  Code = 'NIS' >
                                          

                                          </ LocalCurrency >
                                          
                                           < IsDraft > true </ IsDraft >
                                          < Tenant > 989 </ Tenant >
                                          </ ARInvoice >


                                          ";
                        break;
                    }
                #endregion



                case 5:
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

                case 6:
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
                case 7:
                    {
                        apiName = "master";

                        requestText = @"<Master xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
               <Direction Code='E' />
               <TransportMode Code='A' />
               <Shipper Code='10009' />
               <ShipperReference1>SR1</ShipperReference1>
               <ShipperReference2>SR2</ShipperReference2>               
               <Agent Code='10009' />
               <FromPort Code='PFAAA' />
               <ToPort Code='DZAAE' />
               <GrossWeightUnit Code='KG'/>
               <ChargeableWeightUnit Code='KG'/>
               <VolumeUnit  Code='CBM' />
               <Commodity>653</Commodity>
               <Incoterm Code='CIF' />
               <MasterNumber>00001146</MasterNumber>
               <MainCarriageCarrier Code='ACLU'/>
               <MainCarriageCarrierNumber>CC</MainCarriageCarrierNumber>
               <Vessel Code='VD'/>
               <MainCarriageATD>2018-12-31T00:00:00Z</MainCarriageATD> 
               <Houses>
                  <House ShipmentNumber='EXP9207'>                   
                  </House>
               </Houses>
 <Receivables>
                    <Receivable>
                        <ChargesType Code='AFT'/>
                        <Measurement Code='GRWT'/>
                        <Currency Code='USD'/>
                        <Quantity>10</Quantity>
                        <UnitPrice>10</UnitPrice>
                    </Receivable>
                </Receivables>

                 <Payables>
                    <Payable>
                        <ChargesType Code='AFT'/>
                        <Measurement Code='GRWT'/>
                        <Currency Code='USD'/>
                        <Quantity>10</Quantity>
                        <UnitPrice>10</UnitPrice>
                    </Payable>
                  </Payables>
            </Master>
             ";
                        break;
                    }
                #endregion

                #region Customer
                case 8:
                    {
                        apiName = "customer";

                        requestText = @"<Customer>
                       <EnglishName>API Customer with GLAccount</EnglishName>
                       <LocalName>API Customer Local</LocalName>
                       <VatNumber>11001</VatNumber>
                       <Code>AASS88</Code>
                       <MainAddress>
                            <Name>API Adderss</Name>
                            <Address1>adrr1</Address1>
                            <Country Code='AF'/>
                            <City>
                                <Name>My City</Name>
                            </City> 
                            <ZipCode>3333</ZipCode>
                            <PhoneNumber>23132132131</PhoneNumber>
                       </MainAddress>
                       <BillingAddress>
                            <Name>API Billing Adderss</Name>
                            <Address1>Bill adrr1</Address1>
                            <Country Code='PS'/>
                            <City Code='NAB'></City> 
                            <ZipCode>3333</ZipCode>
                            <PhoneNumber>23132132131</PhoneNumber>
                            <ExternalId>55</ExternalId>
                       </BillingAddress>
                       <GLAccount>
                            <IsMultiCurrency>true</IsMultiCurrency>
                            <ChartOfAccount Code='TST'></ChartOfAccount>
                       </GLAccount>
                    </Customer>
                    ";
                        break;
                    }
                #endregion

                #region Vendor
                case 9:
                    {
                        txtParameter2.Visible = true;
                        apiName = "vendorpartner";

                        requestText = @"<Vendor xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                       <EnglishName>API Vendor</EnglishName>
                       <LocalName>API Vendor Local</LocalName>
                       <VatNumber>1100110011</VatNumber>
                       <MainAddress>
                            <Name>API Adderss</Name>
                            <Address1>adrr1</Address1>
                            <Country Code='PS'/>
                            <City Code='NAB'></City> 
                            <ZipCode>3333</ZipCode>
                            <PhoneNumber>23132132131</PhoneNumber>
                       </MainAddress>
                        <GLAccount>
                            <IsMultiCurrency>true</IsMultiCurrency>
                            <ChartOfAccount Code='TST'></ChartOfAccount>
                       </GLAccount>
                    </Vendor>
                    ";
                        break;
                    }
                #endregion

                #region ARPayment
                case 10:
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
                case 11:
                    {
                        apiName = "ARPaymentCancellation";
                        this.operationCombo.SelectedIndex = 1;
                        break;
                    }

                #endregion

                #region APInvoice
                case 12:
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
                case 13:
                    {
               
                        apiName = "ARInvoiceAdditionalData";

                        break;
                    }
                #endregion


                #region CustomerOpenFilesAmount
                case 14:
                    {

                        apiName = "CustomerOpenFilesAmount";

                        break;
                    }
                #endregion

                #region APInvoiceCancellation
                case 15:
                    {

                        lblParameter.Text = "External ID:";
                        lblParameter.Visible  =true;
                        textBox4.Visible = true;
                        apiName = "APInvoiceCancellation";
                        break;
                    }

                #endregion

                #region GLAccountMoreData
             
                case 16:
                    {

                        lblParameter.Text = "Internal NO.:";
                        lblParameter.Visible = true;
                        txtParameter.Visible = true;
                        apiName = "GLAccountMoreData";
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
                            else if (apiName == "ARInvoice" || apiName == "ARPayment" )
                            { 
                                response = await client.GetAsync(txtServerUrl.Text + "/" + api + "?id=" + textBox4.Text + "&number=" + txtParameter2.Text);
                            }
                            else if ( apiName == "APInvoice")
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
                            response = await client.GetAsync(txtServerUrl.Text + "/" + api +  "?externalId=" + textBox4.Text);

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
