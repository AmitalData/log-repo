var Helper = require('../../../../Helper.js');
var Helper = new Helper();

describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('New Direct AWB', function () {
        GoToOperation();
        GoToShipments();
        OpenDirectAWBWizard();
        NewDirectAWB();
    });
   
    //it('New House Wizard', function () {
    //    GoToOperation();
    //    GoToShipments();
    //    OpenHouseAWBWizard();
    //    NewHouseAWB();
    //});
    //it('New Master Wizard', function () {
    //    GoToOperation();
    //    GoToShipments();
    //    OpenMasterAWBWizard();
    //    NewMasterAWB();
    //});

    function Login() {
        browser.ignoreSynchronization = true;
        Helper.login();
        Helper.waitByCss('.DefaultMenuItem', 60000);
    }
    function GoToOperation() {
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Operations')).click();
        Helper.waitByCss('.PagesMenu', 4000);
    }
    function GoToShipments() {
        let shipment = element(by.css('.PagesMenu')).all(by.tagName('li'));
        expect(shipment.get(1).getText()).toBe("Shipments");
        shipment.get(1).click();
        Helper.waitByCss('.FiltersMenu', 4000);
      
    }
    function OpenDirectAWBWizard() {
        var newButton = element(by.cssContainingText('.ToggleButton', 'New'));
        newButton.click();
        Helper.waitByCss('.ToggleButtonMenu', 4000);

        element(by.buttonText('Direct AWB Wizard')).click();
        Helper.waitByCss('.AWBWizardHeader', 4000);
    }
    function NewDirectAWB() {

        FillPartnersTab();
        FillRoutingsTab_Direct();
        FillPackagesTab();
        FillFreightChargesTab();
        //FillOtherChargesTab();
        //FillGeneralDetailsTab();
        //FillOCITab();
      //  FillOtherPartnersTab();

        Statuses();
        //browser.driver.sleep(3000);
    }
    function OpenHouseAWBWizard() {
        var newButton = element(by.cssContainingText('.ToggleButton', 'New'));
        newButton.click();
        Helper.waitByCss('.ToggleButtonMenu', 1000);
 
        //var houseAWB = element(by.cssContainingText('.ToggleButtonMenu', 'House AWB Wizard'));
        //expect(houseAWB.getText()).toBe("House AWB Wizard");
        //houseAWB.click();
        element(by.buttonText('House AWB Wizard')).click();
        Helper.waitByCss('.TabControl', 5000);
    }
    function NewHouseAWB() {
        FillPartnersTab();
        FillRoutingsTab_House();
        FillPackagesTab();
        FillFreightChargesTab();
        FillOtherChargesTab();
        FillGeneralDetailsTab();
        FillOCITab();
        Statuses();
    }
    function OpenMasterAWBWizard() {
        var newButton = element(by.cssContainingText('.ToggleButton', 'New'));
        newButton.click();
        Helper.waitByCss('.ToggleButtonMenu', 4000);

        element(by.buttonText('Master AWB Wizard')).click();
        Helper.waitByCss('.AWBWizardHeader', 4000);
    }
    function NewMasterAWB() {
        FillPartnersTab_Master();
        FillRoutingsTab_Master();
        //FillHAWBTab_Master();
        FillPackagesTab_Master();
        FillFreightChargesTab_Master();
        FillOtherChargesTab();
        FillGeneralDetailsTab_Master();
        FillOCITab();
      //  FillOtherPartnersTab_Master();
        Statuses();
    }

   
    function FillPartnersTab() {
        var partners = element(by.cssContainingText('.InnerChild', 'Partners'));
        partners.click();

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId')).sendKeys('raz');
        //browser.driver.sleep(1000);
        
        Helper.waitById('mydatalist_Shipment_ShipperId', 1000);
        Helper.waitByCss('.DropDownListItem', 5000);
        //element(by.cssContainingText('.DropDownListItem', 'Razan')).click();
        element.all(by.css('.DropDownListItem')).get(0).click();
        //Helper.waitById('searchicon_Shipment_ShipperId', 4000);
        //var shipperSearchIcon = element(by.id('searchicon_Shipment_ShipperId')).click();
        //browser.driver.sleep(3000);
        //Helper.waitById('row3', 4000);
        //Element(by.id('row3')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId')).sendKeys('raz');

        Helper.waitByCss('.DropDownListItem', 5000);
       // element(by.cssContainingText('.DropDownListItem', 'RazanConsignee')).click();
        element.all(by.css('.DropDownListItem')).get(1).click();
    }
    function FillPartnersTab_Master() {
        var partners = element(by.cssContainingText('.InnerChild', 'Partners'));
        partners.click();

        Helper.waitById('Master_ConsigneeId', 4000);
        var consignee = element(by.id('Master_ConsigneeId')).sendKeys('con');

        Helper.waitByCss('.DropDownListItem', 5000);
        element.all(by.css('.DropDownListItem')).get(0).click();
    }
    function FillRoutingsTab_House() {
        var routings = element(by.cssContainingText('.InnerChild', 'Routings'));
        routings.click();
        
        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var departure = element(by.id('Shipment_MainCarriageFromPortId')).sendKeys('eze');

        Helper.waitByCss('.DropDownListItem', 4000);
        //element(by.cssContainingText('.DropDownListItem', 'EZE')).click();
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_MainCarriageToPortId', 4000)
        var destination = element(by.id('Shipment_MainCarriageToPortId')).sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem', 4000);
        //element(by.cssContainingText('.DropDownListItem', 'MVD')).click();
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_House', 4000);
        var house = element(by.id('Shipment_House')).sendKeys('HouseTest123');
    }
    function FillRoutingsTab_Direct() {
        var routings = element(by.cssContainingText('.InnerChild', 'Routings'));
        routings.click();

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var departure = element(by.id('Shipment_MainCarriageFromPortId')).sendKeys('eze');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_MainCarriageFinalDestinationPortId', 4000)
        var destination = element(by.id('Shipment_MainCarriageFinalDestinationPortId')).sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_MainCarriageCarrierId', 4000);
        var airline = element(by.id('Shipment_MainCarriageCarrierId')).sendKeys('aerolineas');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_MainCarriageCarrierNumber', 4000);
        var airlineNumber = element(by.id('Shipment_MainCarriageCarrierNumber')).sendKeys('1354');

        //Helper.waitById('Shipment_Master', 4000);
        //var MAWB = element(by.id('Shipment_Master')).sendKeys('98574523');
        var getFromStock = element(by.cssContainingText('.Button', 'Get from stock'));
        getFromStock.click();
        Helper.waitByCss('.SimpleGridViewBody', 4000);

        browser.driver.sleep(2000);
        let selectStock = element(by.css('.SimpleGridViewBody')).all(by.tagName('tr'));
        selectStock.get(1).click();

        var okButton = element(by.cssContainingText('.RedButton', 'Ok')).click();
        //Helper.waitByCss('.RedButton', 4000);
        //var yesButton = element(by.cssContainingText('.RedButton', 'Yes')).click();

        browser.driver.sleep(2000);


        Helper.waitById('calendarbutton_date_Shipment_MAWBOBLDate', 4000);
        var MAWBDate = element(by.id('calendarbutton_date_Shipment_MAWBOBLDate')).click();

        Helper.waitByCss('.DateTimePickerBoxHeader', 4000);
        element(by.cssContainingText('.DayCell', '9')).click();
       // var selectMAWBDate = element(by.id('calendarbutton_Shipment_MAWBOBLDate')).sendKeys('eze');

        Helper.waitById('calendarbutton_date_Shipment_MainCarriageETD', 4000);
        var ETD = element(by.id('calendarbutton_date_Shipment_MainCarriageETD')).click();

        Helper.waitByCss('.DateTimePickerBoxHeader', 4000);
        element(by.cssContainingText('.DayCell', '9')).click();
    }
    function FillRoutingsTab_Master() {
        var routings = element(by.cssContainingText('.InnerChild', 'Routings'));
        routings.click();

        Helper.waitById('Master_MainCarriageFromPortId', 4000);
        var departure = element(by.id('Master_MainCarriageFromPortId')).sendKeys('eze');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Master_MainCarriageFinalDestinationPortId', 4000)
        var destination = element(by.id('Master_MainCarriageFinalDestinationPortId')).sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Master_MainCarriageCarrierId', 4000);
        var airline = element(by.id('Master_MainCarriageCarrierId')).sendKeys('aerolineas');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Master_MainCarriageCarrierNumber', 4000);
        var airlineNumber = element(by.id('Master_MainCarriageCarrierNumber')).sendKeys('594');

        Helper.waitById('Master_Master', 4000);
        var MAWB = element(by.id('Master_Master')).sendKeys('85725205');

        Helper.waitById('calendarbutton_date_Master_MAWBOBLDate', 4000);
        var MAWBDate = element(by.id('calendarbutton_date_Master_MAWBOBLDate')).click();

        Helper.waitByCss('.DateTimePickerBoxHeader', 4000);
        element(by.cssContainingText('.DayCell', '9')).click();

        Helper.waitById('calendarbutton_date_Master_MainCarriageETD', 4000);
        var ETD = element(by.id('calendarbutton_date_Master_MainCarriageETD')).click();
        Helper.waitByCss('.DateTimePickerBoxHeader', 4000);
        element(by.cssContainingText('.DayCell', '9')).click();
        
    }
    function FillHAWBTab_Master() {
        var HAWBTab = element.all(by.css('.TabControlHead li')).get(4);
        HAWBTab.click();
 
        //var newHouseButton = element(by.cssContainingText('.Button', 'New house')).click();
        //Helper.waitByCss();
        //NewHouseAWB_House();
    }

    function FillPackagesTab() {
        var packages = element(by.cssContainingText('.InnerChild', 'Packages'));
        packages.click();

        Helper.waitById('ShipmentPackage_Quantity',4000);
        var Quantity = element(by.id('ShipmentPackage_Quantity')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Length', 4000);
        var length = element(by.id('ShipmentPackage_Length')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Width', 4000);
        var width = element(by.id('ShipmentPackage_Width')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Height', 4000);
        var hight = element(by.id('ShipmentPackage_Height')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Weight', 4000);
        var grosswieght = element(by.id('ShipmentPackage_Weight')).sendKeys('2.1');

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var DesOfGoodes = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitude test 1');

       // Helper.waitById('', 1000);

    }
    function FillPackagesTab_Master() {
        var packages = element(by.cssContainingText('.InnerChild', 'Packages'));
        packages.click();

        browser.driver.sleep(2000);
        //Helper.waitByCss('.Button', 4000);
        var addLine = element(by.cssContainingText('.Button', 'Add line')).click();

        Helper.waitById('ShipmentPackage_Quantity', 4000);
        var Quantity = element(by.id('ShipmentPackage_Quantity')).sendKeys('10');
        browser.driver.sleep(1000);
        Helper.waitById('ShipmentPackage_Length', 4000);
        var length = element(by.id('ShipmentPackage_Length')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Width', 4000);
        var width = element(by.id('ShipmentPackage_Width')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Height', 4000);
        var hight = element(by.id('ShipmentPackage_Height')).sendKeys('10');

        Helper.waitById('ShipmentPackage_Weight', 4000);
        var grosswieght = element(by.id('ShipmentPackage_Weight')).sendKeys('2.1');

        Helper.waitByCss('.RedButton', 4000);
        var okButton = element(by.cssContainingText('.RedButton', 'Ok')).click();
        //browser.driver.sleep(2000);
    }
    function FillFreightChargesTab() {
        var freightCharges = element(by.cssContainingText('.InnerChild', 'Freight Charges'));
        freightCharges.click();

        Helper.waitById('Shipment_AWBCurrencyId', 4000);
        var currency = element(by.id('Shipment_AWBCurrencyId')).sendKeys('us');
        
        Helper.waitByCss('.DropDownListItem', 4000);
        browser.driver.sleep(500);
       // element(by.cssContainingText('.DropDownListItem', 'USD')).click();
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_AWBChargeRate', 4000);
        var chargeRate = element(by.id('Shipment_AWBChargeRate')).sendKeys('10');
        Helper.waitById('textboxdiv_Shipment_AWBChargeAmount', 4000);
        browser.driver.sleep(1000);
    }
    function FillFreightChargesTab_Master() {
        var freightCharges = element(by.cssContainingText('.InnerChild', 'Freight Charges'));
        freightCharges.click();

        Helper.waitById('Master_AWBCurrencyId', 4000);
        var currency = element(by.id('Master_AWBCurrencyId')).sendKeys('us');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Master_AWBChargeRate', 4000);
        var chargeRate = element(by.id('Master_AWBChargeRate')).sendKeys('10');
    }
    function FillOtherChargesTab() {
        var otherCharges = element(by.cssContainingText('.InnerChild', 'Other Charges'));
        otherCharges.click();

        Helper.waitByCallerName('css', '.hyperlink', 4000);
        browser.driver.sleep(3000);
        var addCharge = element(by.cssContainingText('.Button', 'Add Charge')).click();

        Helper.waitById('LogLov_ShipmentAWBPrintOnly_IATACodeId', 4000);
        var IATACode = element(by.id('ShipmentAWBPrintOnly_IATACodeId')).sendKeys('aw');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('ShipmentAWBPrintOnly_DueTypeCode', 4000);
        var due = element(by.id('ShipmentAWBPrintOnly_DueTypeCode')).sendKeys('ag');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('ShipmentAWBPrintOnly_Quantity', 4000);
        var quantity = element(by.id('ShipmentAWBPrintOnly_Quantity')).sendKeys('10');

        Helper.waitById('ShipmentAWBPrintOnly_UnitPrice', 4000);
        var unitPrice = element(by.id('ShipmentAWBPrintOnly_UnitPrice')).sendKeys('10');

        //Helper.waitById('ShipmentAWBPrintOnly_CurrencyId', 4000);
        //var currency = element(by.id('ShipmentAWBPrintOnly_CurrencyId')).sendKeys('us');

        //Helper.waitByCss('.DropDownListItem', 4000);
        //element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('ShipmentAWBPrintOnly_MeasurementId', 4000);
        var measurement = element(by.id('ShipmentAWBPrintOnly_MeasurementId')).sendKeys('f');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        var okButton = element(by.cssContainingText('.RedButton', 'Ok')).click();
        Helper.waitByCss('.ToggleButton', 4000);
    }
    function FillGeneralDetailsTab() {
        var generalDetails = element(by.cssContainingText('.InnerChild', 'General Details'));
        generalDetails.click();

        Helper.waitById('Shipment_AWBAccountingInformation', 4000);
        var accountingInformation = element(by.id('Shipment_AWBAccountingInformation')).sendKeys('accounting information 123');

        Helper.waitById('Shipment_AWBHandlingInformation', 4000);
        var handlingInformation = element(by.id('Shipment_AWBHandlingInformation')).sendKeys('handling information 987');

        Helper.waitById('Shipment_AWBComments', 4000);
        var comments = element(by.id('Shipment_AWBComments')).sendKeys('Comments 452');

        Helper.waitById('Shipment_AWBSpecialHandlingCodeId1', 4000);
        var spatialHandlingCode1 = element(by.id('Shipment_AWBSpecialHandlingCodeId1')).sendKeys('av');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(2).click();


        //Helper.waitByCss('.FillParent', 4000);
        //var SCI = element(by.css('.FillParent')).click();

        //Helper.waitByCss('.DropDownListItem', 4000);
        //element.all(by.css('.DropDownListItem')).get(2).click();

        Helper.waitById('Shipment_MainHarmonize', 4000);
        var mainHarmonize = element(by.id('Shipment_MainHarmonize')).sendKeys('Main Harmonize 984984');


        Helper.waitById('Shipment_AWBCarrierTarrifReference', 4000);
        var carrierTariffReference = element(by.id('Shipment_AWBCarrierTarrifReference')).sendKeys('Carrier Tariff Reference 7545');
    }
    function FillGeneralDetailsTab_Master() {
        var generalDetails = element(by.cssContainingText('.InnerChild', 'General Details'));
        generalDetails.click();

        Helper.waitById('Master_AWBAccountingInformation', 4000);
        var accountingInformation = element(by.id('Master_AWBAccountingInformation')).sendKeys('accounting information 123');

        Helper.waitById('Master_AWBHandlingInformation', 4000);
        var handlingInformation = element(by.id('Master_AWBHandlingInformation')).sendKeys('handling information 987');

        Helper.waitById('Master_AWBComments', 4000);
        var comments = element(by.id('Master_AWBComments')).sendKeys('Comments 452');

        Helper.waitById('Master_AWBSpecialHandlingCodeId1', 4000);
        var spatialHandlingCode1 = element(by.id('Master_AWBSpecialHandlingCodeId1')).sendKeys('av');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(2).click();

        Helper.waitById('Master_MainHarmonize', 4000);
        var mainHarmonize = element(by.id('Master_MainHarmonize')).sendKeys('Main Harmonize 984984');

        Helper.waitById('Master_AWBCarrierTarrifReference', 4000);
        var carrierTariffReference = element(by.id('Master_AWBCarrierTarrifReference')).sendKeys('Carrier Tariff Reference 7545');
    }
    function FillOCITab() {
        var OCI = element(by.cssContainingText('.InnerChild', 'OCI'));
        OCI.click();

        Helper.waitById('AWBOCI_CountryId', 4000);
        var country = element(by.id('AWBOCI_CountryId')).sendKeys('us');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('AWBOCI_AWBInformationCode', 4000);
        var informationId = element(by.id('AWBOCI_AWBInformationCode')).sendKeys('abi');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('AWBOCI_AWBCustomsInformationCode', 4000);
        var customsInfoId = element(by.id('AWBOCI_AWBCustomsInformationCode')).sendKeys('ac');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('AWBOCI_SupplementaryCustomsInfo', 4000);
        var supplimentaryValue = element(by.id('AWBOCI_SupplementaryCustomsInfo')).sendKeys('supplimentary value 9875');

        //Helper.waitByCss('.IconButton', 4000);
        //var addLineButton = element(by.cssContainingText('.Button', 'Add line')).click();
        browser.driver.sleep(1000);
    }
    function FillOtherPartnersTab() {
        var otherPartners = element(by.cssContainingText('.InnerChild', 'Other Partners'));
        otherPartners.click();

        Helper.waitById('Shipment_NominatedHandlingPartyId', 4000);
        var nominatedHandlingParty = element(by.id('Shipment_NominatedHandlingPartyId')).sendKeys('aero');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_OtherParticipantIdCode1', 4000);
        var otherParticipantID = element(by.id('Shipment_OtherParticipantIdCode1')).sendKeys('ag');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Shipment_OtherParticipantInformationCode1', 4000);
        var otherParticipantCode = element(by.id('Shipment_OtherParticipantInformationCode1')).sendKeys('100');

        Helper.waitById('Shipment_OtherParticipantInformationName1', 4000);
        var otherParticipantName = element(by.id('Shipment_OtherParticipantInformationName1')).sendKeys('razan92');

        Helper.waitById('Shipment_OtherParticipantInformationPortCode1', 4000);
        var otherParticipantPort = element(by.id('Shipment_OtherParticipantInformationPortCode1')).sendKeys('951');

        Helper.waitById('Shipment_OtherParticipantInformationReference1', 4000);
        var otherParticipantReference = element(by.id('Shipment_OtherParticipantInformationReference1')).sendKeys('reference154');

    }
    function FillOtherPartnersTab_Master() {
        var otherPartners = element(by.cssContainingText('.InnerChild', 'Other Partners'));
        otherPartners.click();

        Helper.waitById('Master_NominatedHandlingPartyId', 4000);
        var nominatedHandlingParty = element(by.id('Master_NominatedHandlingPartyId')).sendKeys('aero');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Master_OtherParticipantIdCode1', 4000);
        var otherParticipantID = element(by.id('Master_OtherParticipantIdCode1')).sendKeys('ag');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();
        
        Helper.waitById('Master_OtherParticipantInformationCode1', 4000);
        var otherParticipantCode = element(by.id('Master_OtherParticipantInformationCode1')).sendKeys('100');

        Helper.waitById('Master_OtherParticipantInformationName1', 4000);
        var otherParticipantName = element(by.id('Master_OtherParticipantInformationName1')).sendKeys('razan92');

        Helper.waitById('Master_OtherParticipantInformationPortCode1', 4000);
        var otherParticipantPort = element(by.id('Master_OtherParticipantInformationPortCode1')).sendKeys('951');

        Helper.waitById('Master_OtherParticipantInformationReference1', 4000);
        var otherParticipantReference = element(by.id('Master_OtherParticipantInformationReference1')).sendKeys('reference154');

    }
    function Statuses() {
        var saveButton = element(by.css('.EntityChangesButton')).click();
        Helper.waitByCss('.Button', 1000);
        browser.driver.sleep(1000);

        var closeButton = element(by.cssContainingText('.Button', 'Close')).click();
        //Helper.waitByCss('.Button', 1000);
        browser.driver.sleep(1000);

    }
});