var Helper = require('../../../../../Helper.js');
var Helper = new Helper();

describe('DirectAirShipment', function () {
    it('Login', function () {
        Login();
    });
    it('ExportAir Shipment', function () {
        GoToOperation();
        GoToShipments();
        OpenDirectWizard();
        NewExportAirShipment();
    });
    it('ImportAir Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenDirectWizard();
        NewImportAirShipment();

    });
    it('DomesticAir Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenDirectWizard();
        NewDomesticAirShipment();
    });
    it('DropAir Shipment ', function () {
        GoToOperation();
        GoToShipments();
        OpenDirectWizard();
        NewDropAirShipment();
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
    function NewExportAirShipment() {

        //#region Export-Air 
        var Export = element(by.cssContainingText('.RadioButton', 'Export'));
        Export.click();

        Helper.waitById('TransportModeRadio_0A',4000);
        var air = element(by.id('TransportModeRadio_0A'));
        air.click();

        Helper.waitById('TransportModeRadio_0O');
        var ocean = element(by.cssContainingText('.RadioButton', 'Ocean'));

        ocean.click();

        air.click();

        //#endregion

        //#region Shipper  &  Consignee 
        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('export');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element.all(by.id('mydatalist_Shipment_ShipperId')).get(0).click();

        Helper.waitById('Shipment_ConsigneeId', 4000);
        var consignee = element(by.id('Shipment_ConsigneeId'));
        consignee.sendKeys('consig');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var consigneeValue = element(by.cssContainingText('.DropDownListItem', '70028')).click();

        //#endregion

        //#region Include Pickup 

        Helper.waitById('Shipment_IncludePickUp', 4000);
        //var includePickup = element(by.id('Shipment_IncludePickUp'));
        var includePickup = element(by.css('.CheckBox')).click();


        ////#region Notes, test element is not clickable 
        //var EC = protractor.ExpectedConditions;
        //var test = element(by.id('Shipment_IncludePickUp'));
        //var isClickable = EC.elementToBeClickable(test);
        //browser.wait(isClickable, 5000);
        //test.click();





        //browser.actions().mouseMove(includePickup).click();

        //Helper.waitById('Shipment_FromAddressZipCode', 4000);
        //var zipCode = element(by.id('Shipment_FromAddressZipCode')).click();

        //Helper.waitById('Shipment_FromAddressCity', 4000);
        //var selectCity = element(by.cssContainingText('.hyperlink', 'Select'));

        //Helper.waitByCss('.SimpleGridViewRow ', 4000);
        //var city = element(by.cssContainingText('.SimpleGridViewRow ', 'Ramallah'));
        ////#endregion

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

        ////#region Delivery 

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        ////#endregion

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
        Helper.waitByCss('.ToggleButton',4000);

        //#endregion

    }
    function NewImportAirShipment() {

        //#region Import-Air 
        var Import = element(by.cssContainingText('.RadioButton', 'Import'));
        Import.click();

        Helper.waitById('TransportModeRadio_0A', 4000);
        var air = element(by.id('TransportModeRadio_0A'));
        air.click();

        Helper.waitById('TransportModeRadio_0O');
        var ocean = element(by.cssContainingText('.RadioButton', 'Ocean'));

        ocean.click();

        air.click();

        //#endregion

        //#region Shipper  &  Consignee 
        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('export');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element.all(by.id('mydatalist_Shipment_ShipperId')).get(0).click();

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

        ////#region Delivery 

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        ////#endregion

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
    function NewDomesticAirShipment() {

        //#region Domestic-Air 
        var Domestic = element(by.cssContainingText('.RadioButton', 'Domestic'));
        Domestic.click();

        Helper.waitById('TransportModeRadio_0A', 4000);
        var air = element(by.id('TransportModeRadio_0A'));
        air.click();

        Helper.waitById('TransportModeRadio_0O');
        var ocean = element(by.cssContainingText('.RadioButton', 'Ocean'));

        ocean.click();

        air.click();

        //#endregion

        //#region Shipper  &  Consignee 
        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('export');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element.all(by.id('mydatalist_Shipment_ShipperId')).get(0).click();

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
        gateway.sendKeys('bos');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var gatewayValue = element(by.cssContainingText('.DropDownListItem', 'BOS'));
        gatewayValue.click();

        //*************

        Helper.waitById('Shipment_MainCarriageToPortId', 4000);
        var destination = element(by.id('Shipment_MainCarriageToPortId'));
        destination.sendKeys('ith');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var destinationValue = element(by.cssContainingText('.DropDownListItem', 'ITH')).click();

        //#endregion

        ////#region Delivery 

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        ////#endregion

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
    function NewDropAirShipment() {

        //#region Drop-Air 
        var Drop = element(by.cssContainingText('.RadioButton', 'Drop'));
        Drop.click();

        Helper.waitById('TransportModeRadio_0A', 4000);
        var air = element(by.id('TransportModeRadio_0A'));
        air.click();

        Helper.waitById('TransportModeRadio_0O');
        var ocean = element(by.cssContainingText('.RadioButton', 'Ocean'));

        ocean.click();

        air.click();

        //#endregion

        //#region Shipper  &  Consignee 
        Helper.waitById('Shipment_ShipperId', 4000);
        var shipper = element(by.id('Shipment_ShipperId'));
        shipper.sendKeys('export');

        Helper.waitByCss('.DropDownListItem ', 4000);
        var shipperValue = element.all(by.id('mydatalist_Shipment_ShipperId')).get(0).click();

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

        ////#region Delivery 

        //var includeDelivery = element(by.css('for="Shipment_IncludeDelivery"')).click();

        //browser.driver.sleep(2000);

        ////#endregion

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