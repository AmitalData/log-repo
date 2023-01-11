using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
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
        private string[] generalTenantsAPISNames = new[] { "House", "Direct", "Master", "Rates Update", "Get Shipments by References" };
        private string[] hypridTenantsAPISNames = new[] { "Customs", "Quote", "Customer", "Vendor", "Cargo Tracking Shipment Details" };
        private string[] oceanInsightAPISNames = new[] { "Container" };
        private string[] fullAccountingTenantsAPISNames = new[] { "Customer", "Vendor", "ARPayment", "Cancel ARPayment", "APInvoice Cancellation",
                                                                  "ARInvoice", "APInvoice", "ARInvoice Additional Data", "Journal", "GLAccount" , "ClosedMonthCheck" ,
                                                                  "Customer Open Files Amount", "GL Account More Data" };
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

                    if(FeatureToggleHelper.HasFeatureToggle("OIC", this.responseParameters.Tenant))                    
                        AddTenantAPIsNames(oceanInsightAPISNames);                    

                    if (FeatureToggleHelper.HasFeatureToggle("CTI", this.responseParameters.Tenant))                    
                        AddTenantAPIsNames(new[] { "Customer" });                    
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

                    if (FeatureToggleHelper.HasFeatureToggle("OIC", this.responseParameters.Tenant))                    
                        AddTenantAPIsNames(oceanInsightAPISNames);

                    if (FeatureToggleHelper.HasFeatureToggle("CTI", this.responseParameters.Tenant))
                        AddTenantAPIsNames(new[] { "Customer" });
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
            AddCustomerAPIsRequestText();
            AddHouseAPIsRequestText();
            AddDirectAPIsRequestText();
            AddMasterAPIsRequestText();
            AddRatesAPIsRequestText();
            AddGetShipmentsAPIsRequestText();
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
            if (!this.XMLRequestTexts.ContainsKey("PostHouse"))
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
            }

            if (!this.XMLRequestTexts.ContainsKey("PutHouse"))
            {
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
        }

        private void AddDirectAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("PostDirect"))
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
            if (!this.XMLRequestTexts.ContainsKey("PatchDirect"))
            {
                this.XMLRequestTexts.Add("PatchDirect", "[	\r\n" +
                    "  {\"op\": \"replace\",	\r\n" +
                    "	\"path\": \"/Customer\",	\r\n" +
                    "	\"value\": {\"Code\": 89086	}\r\n	},\r\n" +
                    "	{\"op\": \"replace\",\r\n" +
                    "	\"path\": \"/Shipper\",		\r\n" +
                    "   \"value\": {\"Code\": 89086	}\r\n	},\r\n" +
                    "	{\"op\": \"replace\",\r\n" +
                    "   \"path\": \"/Consignee\",	\r\n" +
                    "	\"value\": {\"Code\": 89088}	},\r\n" +
                    "	{\"op\": \"replace\",	\r\n" +
                    "	\"path\": \"/MainCarriageFromPort\",	\r\n" +
                    "	\"value\": {\"Code\": \"GBLHR\"	}\r\n	},	\r\n" +
                    "   {\"op\": \"replace\",\r\n" +
                    " 	\"path\": \"/MainCarriageToPort\",\r\n" +
                    "	\"value\": {\"Code\": \"GBLHR\"}\r\n	},\r\n" +
                    "	{\"op\": \"replace\",\r\n" +
                    "   \"path\": \"/CutoffDate\",	\r\n" +
                    "	\"value\": \"2021-12-21T00:00:00\"	},	\r\n" +
                    "   {\"op\": \"replace\",	\r\n" +
                    "	\"path\": \"/PickUps\",	\r\n" +
                    "	\"value\": [	\r\n" +
                    "		{ \r\n" +
                    "               \"Id\": \"1-1351597\",\r\n " +
                    "				\"Seal\": null,\r\n " +
                    "				\"Notes\": null,\r\n " +
                    "				\"Seal2\": null,\r\n " +
                    "				\"Width\": 2,\r\n " +
                    "				\"Height\": 3,\r\n " +
                    "				\"Length\": 1,\r\n " +
                    "				\"Pieces\": 11,\r\n " +
                    "				\"Volume\": 0,\r\n " +
                    "				\"IMDGCode\": null,\r\n " +
                    "				\"UnNumber\": null,\r\n " +
                    "				\"Harmonize\": null,\r\n " +
                    "				\"FlashPoint\": null,\r\n " +
                    "				\"Reference1\": null,\r\n " +
                    "				\"Reference2\": null,\r\n " +
                    "				\"Reference3\": null,\r\n " +
                    "				\"Reference4\": null,\r\n " +
                    "				\"ClassNumber\": null,\r\n " +
                    "				\"GrossWeight\": 44,\r\n " +
                    "				\"IsDangerous\": false,\r\n " +
                    "				\"PackageType\": \r\n " +
                    "                  {\"Code\": null,\r\n " +
                    "					\"LocalName\": null,\r\n " +
                    "					\"EnglishName\": null,\r\n " +
                    "					\"PartnerCode\": null,\r\n " +
                    "					\"ComputingPartnerCode\": null\r\n " +
                    "     				},\r\n " +
                    " 				\"Temperature\": null,\r\n " +
                    "				\"Ventilation\": null,\r\n " +
                    "				\"changeSetOp\": \"Update\",\r\n " +
                    "				\"InsidePackages\": null,\r\n " +
                    "				\"PackagingGroup\": null,\r\n " +
                    "				\"CommodityNumber\": null,\r\n " +
                    "				\"ContainerNumber\": null,\r\n " +
                    "				\"MaterialDescription\": null,\r\n " +
                    "				\"ComputingPartnerCode\": null}, \r\n " +
                    "               {\"Id\": \"1-1351598\",\r\n " +
                    "				\"Seal\": null,	\r\n " +
                    "   			\"Notes\": null,\r\n " +
                    "				\"Seal2\": null,\r\n " +
                    "				\"Width\": 8,\r\n " +
                    "				\"Height\": 9,\r\n " +
                    "				\"Length\": 7,\r\n " +
                    "				\"Pieces\": 14,	\r\n " +
                    "		    	\"Volume\": 0.007,\r\n " +
                    "				\"IMDGCode\": null,\r\n " +
                    "				\"UnNumber\": null,\r\n " +
                    "				\"Harmonize\": null,\r\n " +
                    "				\"FlashPoint\": null,\r\n " +
                    "				\"Reference1\": null,\r\n " +
                    "				\"Reference2\": null,\r\n " +
                    "				\"Reference3\": null,\r\n " +
                    "				\"Reference4\": null,\r\n " +
                    "				\"ClassNumber\": null,\r\n " +
                    "				\"GrossWeight\": 10,\r\n " +
                    "				\"IsDangerous\": false,\r\n " +
                    "				\"PackageType\": {	\r\n " +
                    "				    \"Code\": null,	\r\n " +
                    "				    \"LocalName\": null,\r\n " +
                    "				    \"EnglishName\": null,\r\n " +
                    "					\"PartnerCode\": null,\r\n " +
                    "					\"ComputingPartnerCode\": null},\r\n " +
                    "				\"Temperature\": null,\r\n " +
                    "				\"Ventilation\": null,\r\n " +
                    "				\"changeSetOp\": \"Update\",\r\n " +
                    "				\"InsidePackages\": null,\r\n " +
                    "				\"PackagingGroup\": null,\r\n " +
                    "				\"CommodityNumber\": null,\r\n " +
                    "				\"ContainerNumber\": null,\r\n " +
                    "				\"MaterialDescription\": null,\r\n " +
                    "				\"ComputingPartnerCode\": null},\r\n " +
                    "   			{\"Id\": \"null\",\r\n " +
                    "				\"Seal\": null,	\r\n " +
                    "   			\"Notes\": null,\r\n " +
                    "				\"Seal2\": null,\r\n " +
                    "				\"Width\": 13,\r\n " +
                    "				\"Height\": 14,\r\n " +
                    "				\"Length\": 12,\r\n " +
                    "				\"Pieces\": 15,\r\n " +
                    "				\"Volume\": 0.007,\r\n " +
                    "				\"IMDGCode\": null,\r\n " +
                    "				\"UnNumber\": null,\r\n " +
                    "				\"Harmonize\": null,\r\n " +
                    "				\"FlashPoint\": null,\r\n " +
                    "				\"Reference1\": null,\r\n " +
                    "				\"Reference2\": null,\r\n " +
                    "				\"Reference3\": null,\r\n " +
                    "				\"Reference4\": null,\r\n " +
                    "				\"ClassNumber\": null,\r\n " +
                    "				\"GrossWeight\": 100,\r\n " +
                    "				\"IsDangerous\": false,\r\n " +
                    "				\"PackageType\": \r\n " +
                    "                   {\"Code\": null,\r\n " +
                    "					\"LocalName\": null,\r\n " +
                    "					\"EnglishName\": null,\r\n " +
                    "					\"PartnerCode\": null,\r\n " +
                    "					\"ComputingPartnerCode\": null},\r\n " +
                    "				\"Temperature\": null,\r\n " +
                    "				\"Ventilation\": null,\r\n " +
                    "				\"changeSetOp\": \"Insert\",\r\n " +
                    "				\"InsidePackages\": null,\r\n " +
                    "				\"PackagingGroup\": null,\r\n " +
                    "				\"CommodityNumber\": null,\r\n " +
                    "				\"ContainerNumber\": null,\r\n " +
                    "				\"MaterialDescription\": null,\r\n " +
                    "				\"ComputingPartnerCode\": null},\r\n " +
                    "			    {\"Id\": \"1-1351651\",\r\n " +
                    "               \"Seal\": null,\r\n " +
                    "				\"Notes\": null,\r\n " +
                    "				\"Seal2\": null,\r\n " +
                    "				\"Width\": null,\r\n " +
                    "				\"Height\": null,\r\n " +
                    "				\"Length\": null,\r\n " +
                    "				\"Pieces\": null,\r\n " +
                    "				\"Volume\": null,\r\n " +
                    "				\"IMDGCode\": null,\r\n " +
                    "				\"UnNumber\": null,	\r\n " +
                    "   			\"Harmonize\": null,\r\n " +
                    "				\"FlashPoint\": null,\r\n " +
                    "				\"Reference1\": null,\r\n " +
                    "				\"Reference2\": null,\r\n " +
                    "				\"Reference3\": null,\r\n " +
                    "				\"Reference4\": null,\r\n " +
                    "				\"ClassNumber\": null,\r\n " +
                    "				\"GrossWeight\": null,\r\n " +
                    "				\"PackageType\": {\r\n " +
                    "					\"Code\": null,\r\n " +
                    "					\"LocalName\": null,\r\n " +
                    "					\"EnglishName\": null,\r\n " +
                    "					\"PartnerCode\": null,\r\n " +
                    "					\"ComputingPartnerCode\": null},\r\n " +
                    "				\"Temperature\": null,\r\n " +
                    "				\"Ventilation\": null,\r\n " +
                    "				\"changeSetOp\": \"Delete\",\r\n " +
                    "				\"InsidePackages\": null,\r\n " +
                    "				\"PackagingGroup\": null,\r\n " +
                    "				\"CommodityNumber\": null,\r\n " +
                    "				\"ContainerNumber\": null,\r\n " +
                    "				\"MaterialDescription\": null,\r\n " +
                    "				\"ComputingPartnerCode\": null}]\r\n " +
                    "	}]");
            }
        }

        private void AddMasterAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("PostMaster"))
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
        }

        private void AddRatesAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("RatesUpdate"))
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
        }

        private void AddGetShipmentsAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("GetShipmentsByReferences"))
            {
                this.XMLRequestTexts.Add("GetShipmentsByReferences", @"<Query xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                                            <ComputingPartnerCode>AMS</ComputingPartnerCode>
                                            <Agent Code='0000' PartnerCode='jjj' Reference1='' Reference2=''></Agent>
                                            <Shipper></Shipper>
                                            <Consignee></Consignee>
                                            <ShipperNotExporter></ShipperNotExporter>
                                            <ConsigneeNotImporter></ConsigneeNotImporter>
                                            <Forwarder></Forwarder>
                                            <House>123</House>
                                            <Master></Master>
                                        </Query>
              ");
            }
        }

        private void AddCustomsAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("Customs"))
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
        }

        private void AddQuotesStatusAPIsRequestText()
        {
            if (!this.XMLRequestTexts.ContainsKey("AcceptQuote"))
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
            }

            if (!this.XMLRequestTexts.ContainsKey("DeclineQuote"))
            {
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
            }

            if (!this.XMLRequestTexts.ContainsKey("CancleQuote"))
            {
                this.XMLRequestTexts.Add("CancleQuote", @"<QuoteStatus xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
	                                        <QuoteNumber>1289</QuoteNumber>
	                                        <IsQuoteCancel xsi:nil='true' />
	                                        <QuoteCancelNote>This quote cancelled by Samar</QuoteCancelNote>
	                                        <QuoteCancelDate>2017-12-31T00:00:00Z</QuoteCancelDate>
                                        </QuoteStatus>                
              ");
            }

            if (!this.XMLRequestTexts.ContainsKey("DefultQuoteStatus"))
            {
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
            if (!this.XMLRequestTexts.ContainsKey("ARInvoice"))
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
}