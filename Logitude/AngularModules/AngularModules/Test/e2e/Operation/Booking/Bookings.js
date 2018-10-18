var Helper = require('../../../Helper.js');
var Helper = new Helper();

describe('Logitude Protractor Testing', function () {
    it('Login', function () {
        Login();
    });
    it('New booking', function () {
        GoToOperation();
        GoToBookings();
        OpenBookingsWizard();
        NewBooking();


        //FillPartnersTab();
        //FillPackagesTab();

        //SaveBooking();
        //RequestBooking();
        //CloseBookingWizard()
    });
    //it('Fill packages tab', function () {
        
    //})
    function Login() {
        browser.ignoreSynchronization = true;
        Helper.login();
        Helper.waitByCss('.DefaultMenuItem', 60000);
    }
    function GoToOperation() {
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Operations')).click();
        Helper.waitByCss('.PagesMenu', 4000);
    }
    function GoToBookings() {
        let booking = element(by.css('.PagesMenu')).all(by.tagName('li'));
        expect(booking.get(0).getText()).toBe("Bookings");
        booking.get(0).click();
        Helper.waitByCss('.Button', 4000);
    }
    function OpenBookingsWizard() {
        var newButton = element(by.cssContainingText('.Button', 'New'));
        newButton.click();
        Helper.waitById('Booking_MainCarriageCarrierId', 4000);
    }

    function NewBooking() {
        FillBookingDetailsTab();
        FillPartnersTab();
        FillPackagesTab();
        FillGeneralDetailsTab();
        SaveBooking();
        CloseBookingWizard();
    }
  
    function FillBookingDetailsTab() {
        CheckAirline();
        var stock = element(by.cssContainingText('.Button', 'Stock'));
        stock.click();
        Helper.waitByCss('.SimpleGridViewBody', 4000);
        
        browser.driver.sleep(2000);
        let selectStock = element(by.css('.SimpleGridViewBody')).all(by.tagName('tr'));
        selectStock.get(1).click();

        var okButton = element(by.cssContainingText('.RedButton', 'Ok')).click();
        Helper.waitByCss('.RedButton', 4000);
        var yesButton = element(by.cssContainingText('.RedButton', 'Yes')).click();
       

        Helper.waitById('Booking_MainCarriageFromPortId', 4000);
        var departure = element(by.id('Booking_MainCarriageFromPortId')).sendKeys('eze');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

        Helper.waitById('Booking_MainCarriageFinalDestinationPortId', 4000)
        var destination = element(by.id('Booking_MainCarriageFinalDestinationPortId')).sendKeys('mvd');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();
    }
    function CheckAirline() {
        var airline = element(by.id('Booking_MainCarriageCarrierId'));
        if (airline.getText() == '') {
            // you don't have to do changes to this field.
            expect(airline.getText()).toBe('Aerolineas Argentinas');
            browser.driver.sleep(2000);
        }
        else {
            expect(airline.getText()).toBe('');
            airline.sendKeys('aeroli');
            Helper.waitByCss('.DropDownListItem', 4000);
            element.all(by.css('.DropDownListItem')).get(0).click();
        }
    }
    function FindFlight() {
       
        var findFlight = element(by.css('.GreenButton'));
        browser.driver.sleep(1000);
        findFlight.click();
        browser.driver.sleep(2000);

        // this wizard has to take the values of airline, departure, destination
        var departureDate = element(by.css('.RightCenter'));
        departureDate.click();
        browser.driver.sleep(1000);
        element(by.cssContainingText('.DayCellDev', '30')).click();
        browser.driver.sleep(1000);

        var okFindFlight = element(by.cssContainingText('.GreenButton', 'Find Flights'));
        okFindFlight.click();
        browser.driver.sleep(1000);
    }
    function FillPartnersTab() {
        var partners = element(by.cssContainingText('.InnerChild', 'Partners'));
        partners.click();
       
        Helper.waitById('Booking_ShipperId', 4000);
        var shipper = element(by.id('Booking_ShipperId')).sendKeys('raz');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(0).click();

       
        var consignee = element(by.id('Booking_ConsigneeId')).sendKeys('raz');
        
        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(1).click();

        element(by.id('searchicon_Booking_ShipperId')).click();

        Helper.waitByCss('.GeneratedGridBody', 4000);
        browser.driver.sleep(1000);

        var selectShipper = element.all(by.css('.GeneratedGridBody div'));
        selectShipper.get(5).click();

    }
    function FillPackagesTab() {
        var packages = element(by.cssContainingText('.InnerChild', 'Packages'));
        packages.click();

        browser.driver.sleep(1000);
        Helper.waitById('BookingPackage_Quantity', 4000);
        var Quantity = element(by.id('BookingPackage_Quantity')).sendKeys('10');
        browser.driver.sleep(1000);

        Helper.waitById('BookingPackage_Length', 4000);
        var length = element(by.id('BookingPackage_Length')).sendKeys('10');

        Helper.waitById('BookingPackage_Width', 4000);
        var width = element(by.id('BookingPackage_Width')).sendKeys('10');

        Helper.waitById('BookingPackage_Height', 4000);
        var hight = element(by.id('BookingPackage_Height')).sendKeys('10');

        Helper.waitById('BookingPackage_Weight', 4000);
        var grosswieght = element(by.id('BookingPackage_Weight')).sendKeys('2.1');

        Helper.waitById('Booking_DescriptionOfGoods', 4000);
        var DesOfGoodes = element(by.id('Booking_DescriptionOfGoods')).sendKeys('logitude test 1');

    }
    function FillGeneralDetailsTab() {
        var generalDetails = element(by.cssContainingText('.InnerChild', 'General Details'));
        generalDetails.click();

        Helper.waitById('Booking_AWBCarrierTarrifReference', 4000);
        var AWBCarrierTarrifRef = element(by.id('Booking_AWBCarrierTarrifReference')).sendKeys('AWB Carrier Tarrif 111');

        Helper.waitById('Booking_AWBSpecialHandlingCodeId1', 4000);
        var spatialHandlingCode1 = element(by.id('Booking_AWBSpecialHandlingCodeId1')).sendKeys('av');

        Helper.waitByCss('.DropDownListItem', 4000);
        element.all(by.css('.DropDownListItem')).get(2).click();

        Helper.waitById('Booking_SpecialServicesRequest', 4000);
        var specialServicesRequist = element(by.id('Booking_SpecialServicesRequest')).sendKeys('special services request 222');

        Helper.waitById('Booking_OtherServicesInformation', 4000);
        var otherServicesInformation = element(by.id('Booking_OtherServicesInformation')).sendKeys('Other Services Information 333');
    }
    function SaveBooking() {
        var saveButton = element(by.css('.EntityChangesButton')).click();
        Helper.waitByCss('.Button', 1000);
        browser.driver.sleep(2000);
    }
    function CloseBookingWizard() {
        var closeButton = element(by.cssContainingText('.Button', 'Close')).click();
        //Helper.waitByCss('.Button', 1000);
        browser.driver.sleep(3000);
    }
    function RequestBooking() {
        element(by.cssContainingText('.Button', 'Request Booking')).click();
        browser.driver.sleep(1000);
    }
    function CancelBooking() {

    }


});