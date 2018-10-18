

'use strict';

var LoginPage = require('./login.page.js');

describe('login page ', function () {
  var page;

  beforeEach(function () {
    page = new LoginPage();
  });

  it('Login', function () {
    page.typeUserName('razan@mail2.com');
	browser.driver.sleep(1000);

	page.typePassword('!A123456');
	browser.driver.sleep(1000);
	
	page.clickLogin();
	browser.driver.sleep(1000);

	page.clickAngular();
	  browser.driver.wait(function() {
            return browser.driver.isElementPresent(by.css('.DefaultMenuItem'));
        }, 20000);
  });

 

});


/* 


it('Login', function() {
    
	browser.ignoreSynchronization = true;
	browser.driver.get('http://192.168.1.32/main');

	browser.driver.findElement(by.id('Email')).sendKeys('razan@mail2.com');
	browser.driver.findElement(by.id('Password')).sendKeys('!A123456');

    expect(element(by.id('Email')).getAttribute('value')).toEqual('anasa@mail.com');
    expect(element(by.id('Password')).getAttribute('value')).toEqual('L@ans123');

	browser.driver.findElement(by.id('cmdLogin')).click();

    //LoginButton
	element(by.id('cmdLogin')).click();
	element(by.css('.promptButton')).click();
	  
	    browser.driver.wait(function() {
            return browser.driver.isElementPresent(by.css('.DefaultMenuItem'));
        }, 20000);
	
  }); */