var Helper = require('../../../../Helper.js');
var Helper = new Helper();
var shipper;

describe('Logitude Protractor Testing', function () {
    
    it('Login', function () {
        Login();
    });
    it('Edit shipment', function () {
        GoToOperation();
        GoToShipments();
        EditShipment();
        OverviewTab();
        //FillPartnersTab();
    });
    it('Shipment statuses', function () {
        //SaveShipment();
        //CloseShipmentWizard();
        //SendFWB();
        //CopyShipment();
        PreviewShipment();
    });
  
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
    function EditShipment() {
        Helper.waitByCss('.QueryLink', 4000);
        var allShipments = element(by.cssContainingText('.QueryLink', 'All Shipments')).click();
        browser.driver.sleep(3000);
        Helper.waitByCss('.Row',4000);
        //let editShipment = element(by.css('.GeneratedGridBody')).all(by.tagName('div'));
        //var editShipment = element.all(by.css('.GeneratedGridBody div'));
        var editShipment = element(by.css('.GeneratedGridBody'));
        var test = editShipment.all(by.tagName('div'));

        test.get(2).click();
        browser.driver.sleep(3000);
    };
    function OverviewTab() {
        var overview = element.all(by.css('.TabControlHead li')).get(2);
        expect(overview.getText()).toBe("Overview");

        overview.click();
        Helper.waitByCss('.TabControl', 4000);
        //browser.driver.sleep(2000);
    }
    function FillPartnersTab() {
        // **************************** I have to check this

        var partners = element(by.cssContainingText('.InnerChild', 'Partners'));
        partners.click();

        Helper.waitById('Shipment_ShipperId', 4000);
        //browser.driver.sleep(3000);

        shipper = element(by.id('Shipment_ShipperId'));
        expect(shipper.getAttribute('input')).toEqual('Razan');

     
        //expect(shipper.getAttribute('')).toEqual('Razan');

        //var elm = element(by.id('Shipment_ShipperId'));
        //elm.getAttribute('value').then(function (value) {
        //    console.log(value);
        //});
       // <div id="foo" class="bar"></div>

        //var foo = element(by.id('foo'));
        //expect(foo.getAttribute('class')).toEqual('bar');

        browser.driver.sleep(3000);

        //element(by.css('.RightCenter img')).click();
        //browser.driver.sleep(3000);


        //var shipper = element(by.id('Shipment_ShipperId')).sendKeys('raz');
        ////browser.driver.sleep(1000);

        //Helper.waitById('mydatalist_Shipment_ShipperId', 1000);
        //Helper.waitByCss('.DropDownListItem', 5000);
        //element.all(by.css('.DropDownListItem')).get(0).click();
    
        //Helper.waitById('Shipment_ConsigneeId', 4000);
        //var consignee = element(by.id('Shipment_ConsigneeId')).sendKeys('raz');

        //Helper.waitByCss('.DropDownListItem', 5000);
        //// element(by.cssContainingText('.DropDownListItem', 'RazanConsignee')).click();
        //element.all(by.css('.DropDownListItem')).get(1).click();
    }

    //****************************************** Status Buttons *************************************************************

    function SaveShipment() {
        var saveButton = element(by.cssContainingText('.Button', 'Save')).click();
        Helper.waitByCss('.Button', 1000);
        browser.driver.sleep(2000);
    }
    function CloseShipmentWizard() {
        var closeButton = element(by.cssContainingText('.Button', 'Close')).click();
        //Helper.waitByCss('.Button', 1000);
        browser.driver.sleep(3000);
    }
    function SendFWB() {
        var wizardType = element(by.cssContainingText('',''));

        element(by.cssContainingText('.Button', 'Send FWB')).click();
      
        Helper.waitByCss('.LogitudeWindow', 4000);
        browser.driver.sleep(3000);
        //element(by.cssContainingText('.Button', 'Close')).click();
        browser.driver.sleep(3000);


        //element(by.buttonText('Close')).click();
        //Helper.waitByCss('');
    }
    function CopyShipment() {
        var moreButton = element(by.cssContainingText('.ToggleButton', 'More'));
        moreButton.click();
        Helper.waitByCss('.ToggleButtonMenu', 4000);

        //var copyShipment = element(by.cssContainingText('.ToggleButtonMenu', 'Copy Shipment')).click();
        var copyShipment = element(by.buttonText('Copy Shipment')).click();
        Helper.waitById('Shipment_ShipperId',4000);
        browser.driver.sleep(2000);
        //Helper.waitById('EntityChangesButton', 4000);

        //var saveCopiedShipment = element.all(by.cssContainingText('.EntityChangesButton li')).get(1).click();
        var saveCopiedShipment = element(by.cssContainingText('.EntityChangesButton span', 'Save')).click();
        browser.driver.sleep(2000);

        var closeCopiedShipment = element(by.cssContainingText('.Button', 'Close'));
        closeCopiedShipment.click();
        browser.driver.sleep(2000);
    }
    function PreviewShipment() {
        var preview = element(by.cssContainingText('.Button', 'Preview')).click();
        Helper.waitByCss('.LogitudeWindow', 4000);
        browser.driver.sleep(6000);

    }

    //****************************************** End of Status Buttons *************************************************************


});