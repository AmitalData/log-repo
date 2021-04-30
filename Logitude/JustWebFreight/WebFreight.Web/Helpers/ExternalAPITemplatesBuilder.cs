using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ExternalAPITemplatesBuilder
    {
        private ExternalAPIResponseParameters responseParameters;
        private TenantPM tenantPM;
        private TenantQuery tenantQuery;
        private string[] generalTenantsAPISNames= new[] { "House", "Direct", "Master", "Rates Update" };
        private string[] hypridTenantsAPISNames = new[] { "Customs", "Quote", "Customer", "Vendor" };
        private string[] fullAccountingTenantsAPISNames = new[] { "Customer", "Vendor", "ARPayment", "Cancel ARPayment", "APInvoice Cancellation",
                                                                  "ARInvoice", "APInvoice", "ARInvoice Additional Data", "Journal", "GLAccount" , 
                                                                  "Customer Open Files Amount", "GL Account More Data","CargoTrackingShipmentDetails" };
        private Dictionary<string,string> XMLRequestTexts;

        public ExternalAPITemplatesBuilder(int tenant)
        {
            this.responseParameters = new ExternalAPIResponseParameters();
            this.responseParameters.Tenant = tenant;
            this.tenantQuery = new TenantQuery(tenant);
            this.tenantPM = tenantQuery.GetTenantFromDB(tenant);
        }

        public ExternalAPIResponseParameters GetExternalAPIResponseParameters()
        {
            GetTanentType();
            GetTanentAPIsNames();
            AddAPIsXMLRequestText();
            return (this.responseParameters);
        }

        private void GetTanentType()
        {
            if (tenantPM != null)
            {
                string tenantType = "";  
                if (this.tenantPM.IsHybrid && !this.tenantPM.AccountingActivated)
                {
                    tenantType = "Hybrid";
                }
                else if (this.tenantPM.AccountingActivated && !this.tenantPM.IsHybrid)
                {
                    tenantType = "FullAccounting";
                }
                else if (this.tenantPM.AccountingActivated && this.tenantPM.IsHybrid)
                {
                    tenantType = "All";
                }
                else
                {
                    tenantType = "General";
                }
                this.responseParameters.ApiTanentType = tenantType;
            }
        }

        private void GetTanentAPIsNames()
        {
            if (this.responseParameters != null)
            {   
                this.responseParameters.APIsNames = new List<string>();
                if (this.responseParameters.ApiTanentType == "General")
                {
                    AddTenantAPIsNames(generalTenantsAPISNames);
                }
                else if (this.responseParameters.ApiTanentType == "FullAccounting")
                {
                    AddTenantAPIsNames(generalTenantsAPISNames);
                    AddTenantAPIsNames(fullAccountingTenantsAPISNames);
                }
                else if(this.responseParameters.ApiTanentType == "Hybrid")
                {
                    AddTenantAPIsNames(generalTenantsAPISNames);
                    AddTenantAPIsNames(hypridTenantsAPISNames);
                }
                else if (this.responseParameters.ApiTanentType == "All")
                {
                    AddTenantAPIsNames(generalTenantsAPISNames);
                    AddTenantAPIsNames(hypridTenantsAPISNames);
                    AddTenantAPIsNames(fullAccountingTenantsAPISNames);
                }
            }
        }

        private void AddTenantAPIsNames(string[] apisNames)
        {
            foreach(string name in apisNames)
            {
                if (!this.responseParameters.APIsNames.Contains(name))
                {
                    this.responseParameters.APIsNames.Add(name);
                }
            }
        }

        private void AddAPIsXMLRequestText()
        {
            this.XMLRequestTexts = new Dictionary<string, string>();
            if (this.responseParameters.ApiTanentType == "General")
            {
                AddGeneralTenantsAPISRequestText();
            }
            else if (this.responseParameters.ApiTanentType == "FullAccounting")
            {
                AddGeneralTenantsAPISRequestText();
                AddFullAccountingTenantsAPIsRequestText();
            }
            else if (this.responseParameters.ApiTanentType == "Hybrid")
            {
                AddGeneralTenantsAPISRequestText();
                AddHypridTenantsAPIsRequestText();
            }
            else if (this.responseParameters.ApiTanentType == "All")
            {
                AddGeneralTenantsAPISRequestText();
                AddHypridTenantsAPIsRequestText();
                AddFullAccountingTenantsAPIsRequestText();
            }
            this.responseParameters.XMLRequestText = this.XMLRequestTexts;
        }

        private void AddGeneralTenantsAPISRequestText()
        {
            AddHouseAPIsRequestText();
            AddDirectAPIsRequestText();
            AddMasterAPIsRequestText();
            AddRatesAPIsRequestText();
        }

        private void AddFullAccountingTenantsAPIsRequestText()
        {
            AddCustomerAPIsRequestText();
            AddVendorAPIsRequestText();
            AddARInvoiceAPIsRequestText();
        }

        private void AddHypridTenantsAPIsRequestText()
        {
            AddCustomsAPIsRequestText();
            AddQuotesStatusAPIsRequestText();
            AddCustomerAPIsRequestText();
            AddVendorAPIsRequestText();
        }

        private void AddHouseAPIsRequestText()
        {
            this.XMLRequestTexts.Add("PostHouse", @" < House >
                                                       < Direction Code = 'E' />
                                                       < TransportMode Code = 'A' />
                                                       < Shipper Code = '70002' />
                                                       < ShipperReference1 > SR1 </ ShipperReference1 >
                                                       < ShipperReference2 > SR2 </ ShipperReference2 >
                                                       < Consignee Code = '70003' />
                                                       < ConsigneeReference1 > CR1 </ ConsigneeReference1 >
                                                       < ConsigneeReference2 > CR2 </ ConsigneeReference2 >
                                                       < Customer Code = '70002' />
                                                       < FromPort Code = 'DE222' />
                                                       < FromPort Code = 'PFAAA' />
                                                       < ToPort Code = 'DZAAE' />
                                                       < GrossWeightUnit Code = 'KG' />
                                                       < ChargeableWeightUnit Code = 'KG' />
                                                       < VolumeUnit  Code = 'CBM' />
                                                       < Commodity > 653 </ Commodity >
                                                       < Incoterm Code = 'CIF' />
                                                       < HouseNo > 00001146 </ HouseNo >
                                                       < HouseDate > 2017 - 10 - 24T00: 00:00 </ HouseDate >



                                                            < Receivable >
                                                                < ChargesType Code = 'AFT' />
                                                                < Measurement Code = 'GRWT' />
                                                                < Currency Code = 'USD' />
                                                                < Quantity > 10 </ Quantity >
                                                                < UnitPrice > 10 </ UnitPrice >
                                                            </ Receivable >
                                                        </ Receivables >

                                                        < Payables >
                                                            < Payable >
                                                                < ChargesType Code = 'AFT' />
                                                                < Measurement Code = 'GRWT' />
                                                                < Currency Code = 'USD' />
                                                                < Quantity > 10 </ Quantity >
                                                                < UnitPrice > 10 </ UnitPrice >
                                                            </ Payable >
                                                        </ Payables >
                                                    < AirPackages >



                                                    </ House >
            ");

            this.XMLRequestTexts.Add("PutHouse", @"<House ShipmentNumber='EXP9115'>
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
            ");
        }

        private void AddDirectAPIsRequestText()
        {
            this.XMLRequestTexts.Add("PostDirect", @"<Direct xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
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
            ");
        }

        private void AddMasterAPIsRequestText()
        {
            this.XMLRequestTexts.Add("PostMaster", @"<Master xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
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
            ");
        }

        private void AddRatesAPIsRequestText()
        {
            this.XMLRequestTexts.Add("RatesUpdate", @"<RatesUpdate xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <ComputingPartnerCode>AMS</ComputingPartnerCode>
	                                        <RateUpdate>
		                                        <Currency Code='USD' PartnerCode='USD'></Currency>
                                                <RateDate>2021-11-29</RateDate>
                                                <Rate>5</Rate>
                                            </RateUpdate>
                                            </RatesUpdate>
            ");
        }

        private void AddCustomsAPIsRequestText()
        {
            this.XMLRequestTexts.Add("Customs", @"<Customs>
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
                                        </Customs>
            ");
        }

        private void AddQuotesStatusAPIsRequestText()
        {
            this.XMLRequestTexts.Add("AcceptQuote", @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1289</QuoteNumber>
	                                        <QuoteAcceptNote>This quote accepted manually by Samar</QuoteAcceptNote>
	                                        <QuoteAcceptDate>2017-12-31T00:00:00Z</QuoteAcceptDate>
                                            <DueDate>2017-12-31T00:00:00Z</DueDate>
	                                        <Stage Id='1-439' Code='QTAC'>
		                                        <Name>Accepted</Name>		                                        
	                                        </Stage>
                                        </QuoteStatus>                
            ");

            this.XMLRequestTexts.Add("DeclineQuote", @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
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
            ");

            this.XMLRequestTexts.Add("CancleQuote", @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1289</QuoteNumber>
	                                        <IsQuoteCancel xsi:nil='true' />
	                                        <QuoteCancelNote>This quote cancelled by Samar</QuoteCancelNote>
	                                        <QuoteCancelDate>2017-12-31T00:00:00Z</QuoteCancelDate>
                                        </QuoteStatus>                
            ");

            this.XMLRequestTexts.Add("DefultQuoteStatus", @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1288</QuoteNumber>	                                        <
	                                        <QuoteAcceptNote>This quote accepted manually by Samar</QuoteAcceptNote>
	                                        <QuoteAcceptDate>2017-12-31T00:00:00Z</QuoteAcceptDate>	  
                                            <DueDate>2017-12-31T00:00:00Z</DueDate>                                      
	                                        <Stage Id='1-439' Code='QTAC'>
		                                        <Name>Accepted</Name>		                                        
	                                        </Stage>
                                        </QuoteStatus>                
                                        
            ");
        }

        private void AddCustomerAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("Customer"))
            {
                this.XMLRequestTexts.Add("Customer", @"<Customer>
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
               ");
            }
        }

        private void AddVendorAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("Vendor"))
            {
                this.XMLRequestTexts.Add("Vendor", @"<Vendor xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
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
               ");
            }
        }

        private void AddARInvoiceAPIsRequestText()
        {
            this.XMLRequestTexts.Add("ARInvoice", @"<ARInvoice xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
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


            ");
        }

    }
}