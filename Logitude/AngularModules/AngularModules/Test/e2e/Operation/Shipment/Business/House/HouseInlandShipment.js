var Helper = require('../../../../../Helper.js');
var Helper = new Helper();

describe('HouseInlandShipment', function () {
    //it('Login', function () {
    //    Login();
    //});
    it('ExportInlandFTL Shipment', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewExportInlandFTLShipment();
    });
    it('ExportInlandLTL Shipment', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewExportInlandLTLShipment();
    });
    it('ImportInlandFTL Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewImportInlandFTLShipment();
    });
    it('ImportInlandLTL Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewImportInlandLTLShipment();
    });
    it('DomesticInlandFTL Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewDomesticInlandFTLShipment();
    });
    it('DomesticInlandLTL Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewDomesticInlandLTLShipment();
    });
    it('DropInlandFTL Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewDropInlandFTLShipment();
    });
    it('DropInlandLTL Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenInlandWizard();
        NewDropInlandLTLShipment();
    });
    function Login() {
        browser.ignoreSynchronization = true;
        Helper.login();
        Helper.waitByCss('.DefaultMenuItem', 60000);
        //browser.driver.sleep(3000);
    }
    function GoToOperation() {
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Operations')).click();
        Helper.waitByCss('.PagesMenu', 4000);
    }
    function GoToShipments() {
        let shipment = element(by.css('.PagesMenu')).all(by.tagName('li'));
        expect(shipment.get(1).getText()).toBe("Shipments");
        shipment.get(1).click();
        //Helper.waitByCss('.FiltersMenu', 4000);
        browser.driver.sleep(2000);
    }
    function OpenDirectWizard() {
        var newButton = element(by.cssContainingText('.ToggleButton', 'New'));
        newButton.click();
        Helper.waitByCss('.ToggleButtonMenu', 4000);

        element(by.buttonText('Direct')).click();
        Helper.waitByCss('.WindowHeader', 4000);
    }

    function NewExportInlandFTLShipment() {

        //#region Export-Inland-FTL

        var Export = element(by.cssContainingText('.RadioButton', 'Export'));
        Export.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        ftl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper&Consignee 

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region IncludePickcup

        Helper.waitById('Shipment_IncludePickUp', 4000);
        var includePickup = element(by.css('.CheckBox')).click();

        //#endregion

        //#region Main Carriage 

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var gateway = element(by.id('Shipment_MainCarriageFromPortId'));
        gateway.sendKeys('eze');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'EZE'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'MVD')).click();

        //#endregion

        //#region Delivery

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        //#endregion

        //#region Fill Dimenssins  

        Helper.waitById('Shipment_Quantity1', 4000);
        var quantity1 = element(by.id('Shipment_Quantity1')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId1', 4000);
        var package1 = element(by.id('Shipment_PackageTypeId1')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage1 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_Quantity2', 4000);
        var quantity2 = element(by.id('Shipment_Quantity2')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId2', 4000);
        var package2 = element(by.id('Shipment_PackageTypeId2')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage2 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create

        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }
    function NewExportInlandLTLShipment() {

        //#region Export-Inland-LTL

        var Export = element(by.cssContainingText('.RadioButton', 'Export'));
        Export.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        Helper.waitById('Shipment_ShipperId', 4000);
        //#endregion

        //#region Shipper&Consignee 

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region IncludePickcup

        Helper.waitById('Shipment_IncludePickUp', 4000);
        var includePickup = element(by.css('.CheckBox')).click();

        //#endregion

        //#region Main Carriage 

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var gateway = element(by.id('Shipment_MainCarriageFromPortId'));
        gateway.sendKeys('eze');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'EZE'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'MVD')).click();

        //#endregion

        //#region Delivery

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        //#endregion

        //#region Fill Dimenssins  
        Helper.waitById('Shipment_OrderGrossWeight', 4000);
        var fillDimenssions = element(by.cssContainingText('.hyperlink', 'Fill Dimensions')).click();

        Helper.waitById('ShipmentOrderPackage_Quantity', 4000);
        var quantity = element(by.id('ShipmentOrderPackage_Quantity')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Length', 4000);
        var length = element(by.id('ShipmentOrderPackage_Length')).sendKeys('100');

        Helper.waitById('ShipmentOrderPackage_Width', 4000);
        var width = element(by.id('ShipmentOrderPackage_Width')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Height', 4000);
        var hight = element(by.id('ShipmentOrderPackage_Height')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_GrossWeight', 4000);
        var grossWeight = element(by.id('ShipmentOrderPackage_GrossWeight')).sendKeys('10');

        Helper.waitByCss('.RedButton', 4000);
        var ok = element(by.cssContainingText('.RedButton', 'Ok')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create

        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }

    function NewImportInlandFTLShipment() {

        //#region Import-Inland-FTL

        var Import = element(by.cssContainingText('.RadioButton', 'Import'));
        Import.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        ftl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper&Consignee 

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region IncludePickcup

        Helper.waitById('Shipment_IncludePickUp', 4000);
        var includePickup = element(by.css('.CheckBox')).click();

        //#endregion

        //#region Main Carriage 

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var gateway = element(by.id('Shipment_MainCarriageFromPortId'));
        gateway.sendKeys('eze');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'EZE'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'MVD')).click();

        //#endregion

        //#region Delivery

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        //#endregion

        //#region Fill Dimenssins  

        Helper.waitById('Shipment_Quantity1', 4000);
        var quantity1 = element(by.id('Shipment_Quantity1')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId1', 4000);
        var package1 = element(by.id('Shipment_PackageTypeId1')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage1 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_Quantity2', 4000);
        var quantity2 = element(by.id('Shipment_Quantity2')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId2', 4000);
        var package2 = element(by.id('Shipment_PackageTypeId2')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage2 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create

        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }
    function NewImportInlandLTLShipment() {

        //#region Import-Inland-LTL

        var Import = element(by.cssContainingText('.RadioButton', 'Import'));
        Import.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper&Consignee 

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region IncludePickcup

        Helper.waitById('Shipment_IncludePickUp', 4000);
        var includePickup = element(by.css('.CheckBox')).click();

        //#endregion

        //#region Main Carriage 

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var gateway = element(by.id('Shipment_MainCarriageFromPortId'));
        gateway.sendKeys('eze');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'EZE'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'MVD')).click();

        //#endregion

        //#region Delivery

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        //#endregion

        //#region Fill Dimenssins  
        Helper.waitById('Shipment_OrderGrossWeight', 4000);
        var fillDimenssions = element(by.cssContainingText('.hyperlink', 'Fill Dimensions')).click();

        Helper.waitById('ShipmentOrderPackage_Quantity', 4000);
        var quantity = element(by.id('ShipmentOrderPackage_Quantity')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Length', 4000);
        var length = element(by.id('ShipmentOrderPackage_Length')).sendKeys('100');

        Helper.waitById('ShipmentOrderPackage_Width', 4000);
        var width = element(by.id('ShipmentOrderPackage_Width')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Height', 4000);
        var hight = element(by.id('ShipmentOrderPackage_Height')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_GrossWeight', 4000);
        var grossWeight = element(by.id('ShipmentOrderPackage_GrossWeight')).sendKeys('10');

        Helper.waitByCss('.RedButton', 4000);
        var ok = element(by.cssContainingText('.RedButton', 'Ok')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create

        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }

    function NewDomesticInlandFTLShipment() {

        //#region Domestic-Inland-FTL 
        var Domestic = element(by.cssContainingText('.RadioButton', 'Domestic'));
        Domestic.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        ftl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper  &  Consignee 
        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region General Area

        Helper.waitById('Shipment_IncotermId', 4000);
        var incoterm = element(by.id('Shipment_IncotermId'));
        incoterm.sendKeys('daf');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var incotermValue = element(by.cssContainingText('.DropDownListItem', 'DAF'));
        incotermValue.click();

        //*************

        Helper.waitById('Shipment_MoveTypeId', 4000);
        var moveType = element(by.id('Shipment_MoveTypeId'));
        moveType.sendKeys('ttt');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var moveTypeValue = element(by.cssContainingText('.DropDownListItem', 'TTT')).click();

        //#endregion

        //#region Fill Dimenssins  

        Helper.waitById('Shipment_Quantity1', 4000);
        var quantity1 = element(by.id('Shipment_Quantity1')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId1', 4000);
        var package1 = element(by.id('Shipment_PackageTypeId1')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage1 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_Quantity2', 4000);
        var quantity2 = element(by.id('Shipment_Quantity2')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId2', 4000);
        var package2 = element(by.id('Shipment_PackageTypeId2')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage2 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create 
        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();


        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }
    function NewDomesticInlandLTLShipment() {

        //#region Domestic-Inland-LTL
        var Domestic = element(by.cssContainingText('.RadioButton', 'Domestic'));
        Domestic.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper&Consignee 

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region General Area

        Helper.waitById('Shipment_IncotermId', 4000);
        var incoterm = element(by.id('Shipment_IncotermId'));
        incoterm.sendKeys('daf');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var incotermValue = element(by.cssContainingText('.DropDownListItem', 'DAF'));
        incotermValue.click();

        //*************

        Helper.waitById('Shipment_MoveTypeId', 4000);
        var moveType = element(by.id('Shipment_MoveTypeId'));
        moveType.sendKeys('ttt');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var moveTypeValue = element(by.cssContainingText('.DropDownListItem', 'TTT')).click();

        //#endregion

        //#region Fill Dimenssins  
        Helper.waitById('Shipment_OrderGrossWeight', 4000);
        var fillDimenssions = element(by.cssContainingText('.hyperlink', 'Fill Dimensions')).click();

        Helper.waitById('ShipmentOrderPackage_Quantity', 4000);
        var quantity = element(by.id('ShipmentOrderPackage_Quantity')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Length', 4000);
        var length = element(by.id('ShipmentOrderPackage_Length')).sendKeys('100');

        Helper.waitById('ShipmentOrderPackage_Width', 4000);
        var width = element(by.id('ShipmentOrderPackage_Width')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Height', 4000);
        var hight = element(by.id('ShipmentOrderPackage_Height')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_GrossWeight', 4000);
        var grossWeight = element(by.id('ShipmentOrderPackage_GrossWeight')).sendKeys('10');

        Helper.waitByCss('.RedButton', 4000);
        var ok = element(by.cssContainingText('.RedButton', 'Ok')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create

        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }

    function NewDropInlandFTLShipment() {

        //#region Drop-Inland-FTL 
        var Drop = element(by.cssContainingText('.RadioButton', 'Drop'));
        Drop.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        ftl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper  &  Consignee 
        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region Include Pickup 
        Helper.waitById('Shipment_IncludePickUp', 4000);
        var includePickup = element(by.css('.CheckBox')).click();

        //#endregion


        //#region Main Carriage 

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var gateway = element(by.id('Shipment_MainCarriageFromPortId'));
        gateway.sendKeys('eze');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'EZE'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'MVD')).click();

        //#endregion

        //#region Delivery 

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        //#endregion

        //#region Fill Dimenssins  

        Helper.waitById('Shipment_Quantity1', 4000);
        var quantity1 = element(by.id('Shipment_Quantity1')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId1', 4000);
        var package1 = element(by.id('Shipment_PackageTypeId1')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage1 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_Quantity2', 4000);
        var quantity2 = element(by.id('Shipment_Quantity2')).sendKeys('10');

        Helper.waitById('Shipment_PackageTypeId2', 4000);
        var package2 = element(by.id('Shipment_PackageTypeId2')).sendKeys('standard');

        Helper.waitByCss('.DropDownListItem', 4000);
        var selectPackage2 = element(by.cssContainingText('.DropDownListItem', '20 Ft. Standard Container')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create 
        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }
    function NewDropInlandLTLShipment() {

        //#region Drop-Inland-LTL

        var Drop = element(by.cssContainingText('.RadioButton', 'Drop'));
        Drop.click();

        Helper.waitById('TransportModeRadio_0O');
        var inland = element(by.cssContainingText('.RadioButton', 'Inland'));
        inland.click();

        Helper.waitById('TransportModeRadio_0O');
        var ftl = element(by.cssContainingText('.RadioButton', 'FTL'));
        ftl.click();

        Helper.waitById('TransportModeRadio_0O');
        var ltl = element(by.cssContainingText('.RadioButton', 'LTL'));
        ltl.click();

        Helper.waitById('Shipment_ShipperId', 4000);

        //#endregion

        //#region Shipper&Consignee 

        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('compan');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element(by.cssContainingText('.DropDownListItem ', 'Company1')).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region IncludePickcup

        Helper.waitById('Shipment_IncludePickUp', 4000);
        var includePickup = element(by.css('.CheckBox')).click();

        //#endregion

        //#region Main Carriage 

        Helper.waitById('Shipment_MainCarriageFromPortId', 4000);
        var gateway = element(by.id('Shipment_MainCarriageFromPortId'));
        gateway.sendKeys('eze');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'EZE'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'MVD')).click();

        //#endregion

        //#region Delivery

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        //#endregion

        //#region Fill Dimenssins  
        Helper.waitById('Shipment_OrderGrossWeight', 4000);
        var fillDimenssions = element(by.cssContainingText('.hyperlink', 'Fill Dimensions')).click();

        Helper.waitById('ShipmentOrderPackage_Quantity', 4000);
        var quantity = element(by.id('ShipmentOrderPackage_Quantity')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Length', 4000);
        var length = element(by.id('ShipmentOrderPackage_Length')).sendKeys('100');

        Helper.waitById('ShipmentOrderPackage_Width', 4000);
        var width = element(by.id('ShipmentOrderPackage_Width')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_Height', 4000);
        var hight = element(by.id('ShipmentOrderPackage_Height')).sendKeys('10');

        Helper.waitById('ShipmentOrderPackage_GrossWeight', 4000);
        var grossWeight = element(by.id('ShipmentOrderPackage_GrossWeight')).sendKeys('10');

        Helper.waitByCss('.RedButton', 4000);
        var ok = element(by.cssContainingText('.RedButton', 'Ok')).click();

        Helper.waitById('Shipment_DescriptionOfGoods', 4000);
        var descriptionOfGoods = element(by.id('Shipment_DescriptionOfGoods')).sendKeys('logitudeTest1');

        //#endregion

        //#region Create

        Helper.waitByCss('.RedButton', 4000);
        var create = element(by.cssContainingText('.RedButton', 'Create')).click();

        Helper.waitByCss('.QueryLink', 4000);
        Helper.waitByCss('.ToggleButton', 4000);

        //#endregion

    }
});