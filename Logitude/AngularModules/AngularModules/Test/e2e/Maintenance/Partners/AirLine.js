

// spec.js
describe('Logitude Protractor Testing', function() {
    it('Login', function () {
        Login();
    });
    it('Fill RequiredField in ariline scenario', function () {
        GoToMaintenance();
        ClickAirline();
        NewAirline();
        FillAirlineRequire();
    });
    it('Fill All field in airline scenario', function () {
        browser.driver.sleep(2000);
        NewAirline();
        FillAllAirlineField();
    });

    function Login() {
        browser.ignoreSynchronization = true;
        browser.driver.get('https://system.logitudeworld.com/login.aspx');
        browser.driver.sleep(1000);

        browser.driver.findElement(by.id('Email')).sendKeys('razan@fnarsoft.com');
        browser.driver.findElement(by.id('Password')).sendKeys('!R123456');

        browser.driver.findElement(by.id('cmdLogin')).click();
        browser.driver.sleep(1000);

        browser.driver.findElement(by.css('.promptButton')).click();
        browser.driver.sleep(1000);

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.DefaultMenuItem'));
        }, 60000);
    } 

    function GoToMaintenance() {
        browser.driver.sleep(1000);
        var menuItem = element(by.cssContainingText('.DefaultMenuItem', 'Maintenance')).click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.PagesMenu'));
        }, 5000);
    }
    function ClickAirline() {
        var Airlines = element(by.cssContainingText('.Title', 'Airlines'));
        Airlines.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.IconButton'));
        }, 20000);
    }
    function NewAirline() {
        var newAirline = element(by.cssContainingText('.Button', 'New Airline'));
        newAirline.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.RedButton'));
        }, 20000);
    }

    function FillAirlineRequire() {
        browser.driver.sleep(1000);
        var Code = element(by.id('textbox_1')).sendKeys('57');
        browser.driver.sleep(1000);
        var name = element(by.id('textbox_3')).sendKeys('Razan32');
        browser.driver.sleep(1000);
        var prefix = element(by.id('textbox_4')).sendKeys('A74');
        browser.driver.sleep(1000);
        
        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);
    }

    function FillAllAirlineField() {
        browser.driver.sleep(1000);
        var Code = element(by.id('textbox_8')).sendKeys('43');
        browser.driver.sleep(1000);
        var ICAO = element(by.id('textbox_9')).sendKeys('67');
        browser.driver.sleep(1000);
        var name = element(by.id('textbox_10')).sendKeys('Razan32');
        browser.driver.sleep(1000);
        var prefix = element(by.id('textbox_11')).sendKeys('A74');
        browser.driver.sleep(1000);
        var localName = element(by.id('textbox_12')).sendKeys('Razan32');
        browser.driver.sleep(1000);
        var website = element(by.id('textbox_13')).sendKeys('Razan32');
        browser.driver.sleep(1000);

        var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
        okBtn.click();

        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css('.Label'));
        }, 4000);

    }
   
	
//	/* it('click CancelBtn',function(){
		
//	    var okBtn = element(by.cssContainingText('.Button', 'Cancel'));
//	    okBtn.click(); 
		
//		 browser.driver.wait(function() {
//            return browser.driver.isElementPresent(by.css('.Counter'));
//        }, 20000);		
//	}); 

//	*/
//	/* it('Chick code validation, must fill name and prefix fields in New Airline',function(){
		
//		var code=element(by.id('textbox_1')).sendKeys('123');
//		var LocalName=element(by.id('textbox_5')).sendKeys('abcd');
//	    var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
//	    okBtn.click(); 
//		browser.driver.wait(function() {
//            return browser.driver.isElementPresent(by.css('.Labela'));
//        }, 20000);			
		
//	}); */
	
//  	it('Fill New Airline',function(){
//		/* var code=element(by.cssContainingText());
//		code.sendKeys('12'); */
//		 //driver.findElement(By.xpath("//div[text()='60']"));
//       /*  var code=element(by.xpath("//input[@id='textbox_1']"));
//		code.sendKeys('12');
//		var Name=element(by.xpath("//input[@id='textbox_3']"));
//		Name.sendKeys('abcd'); */
		
//		var code=element(by.id('textbox_1')).sendKeys('88');
//		browser.driver.sleep(2000);
//        var ICAO=element(by.id('textbox_2')).sendKeys('333');
//		browser.driver.sleep(2000);
		
//		var ICAO=element(by.id('textbox_2')).click();
	
//		ICAO.sendKeys('abcd');
//		ICAO.sendKeys('abcd');


//		var Name=element(by.id('textbox_3')).sendKeys('Razan');
//		var Prefix=element(by.id('textbox_4')).sendKeys('Fan');
//		var LocalName=element(by.id('textbox_5')).sendKeys('abcd');
//		var Website=element(by.id('textbox_6')).sendKeys('abcd');
		
//	    var okBtn = element(by.cssContainingText('.RedButton', 'Ok'));
//	    okBtn.click(); 
		
//		 browser.driver.wait(function() {
//            return browser.driver.isElementPresent(by.css('.Labela'));
//        }, 20000);		
//	});
   
  
///* 	it('go to Location',function(){
		
//		element(by.css('.PagesMenu')).click();
//		//locationTab.click();
//		browser.driver.wait(function(){
//			return browser.driver.isElementPresent(by.css('.BoxItemm'));
//		},20000);
//	});	 */
///* 	
//	it('go to branch',function(){
		
//		var branchBox=element(by.cssContainingText('.Title','!Branches'));
//		branchBox.click();
//		browser.driver.wait(function(){
//			return browser.driver.isElementPresent(by.css('.Buntton'));
//		},20000);
		
//	});  */
	
//	/* it('go to branch',function(){
		
//		var branchBox=element(by.cssContainingText('.Title','!Branches'));
//		branchBox.click();
//		browser.driver.wait(function(){
//			return browser.driver.isElementPresent(by.css('.Button'));
//		},20000);
		
//	}); 
//	it('add new branch',function(){
		
//		var addBranch=element(by.cssContainingText('.Button','New !Branch'));
//		addBranch.click();
//		browser.driver.wait(function(){
//			return browser.driver.isElementPresent(by.css('.FillParent'));
//		},20000);
		
//	}); 
//   */
//  /////
});