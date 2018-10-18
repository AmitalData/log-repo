'use strict';

var LoginPage = require('./login.page.js');

describe('login page ', function () {
  var page;

  beforeEach(function () {
    page = new LoginPage();
  });

  it('forgit password', function () {
    page.typeUserName('razan@mail2.com');
	browser.driver.sleep(1000);

	page.typePassword('!A1234567');
	browser.driver.sleep(1000);
	
	page.clickLogin();
	browser.driver.sleep(1000);

	page.clickForgotPasswordBtn();
	browser.driver.sleep(1000);
	
	
	
	browser.driver.wait(function() {
            return browser.driver.isElementPresent(by.css('.DefaultMenuItemm'));
        }, 20000);
  });

});
