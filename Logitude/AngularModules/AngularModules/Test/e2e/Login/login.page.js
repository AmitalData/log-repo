'use strict';

var LoginPage = function () {
  browser.driver.get('http://192.168.1.32/main');

};

LoginPage.prototype = Object.create({}, {
	userName: { get : function(){return browser.driver.findElement(by.id('Email')) }},
	password: { get : function(){return browser.driver.findElement(by.id('Password')) }},
	loginBtn: { get : function(){return browser.driver.findElement(by.id('cmdLogin')) }},
	angularBtn: { get : function(){return browser.driver.findElement(by.css('.promptButton')) }},
	forgotPasswordBtn: { get : function(){return browser.driver.findElement(by.css('[onclick="resetpasswordclick()"]')) }},
	backToLoginBtn: { get : function(){return browser.driver.findElement(by.css('[onclick="backToLoginClick()"]')) }},
	
	
	
	typeUserName: { value: function (keys) { return this.userName.sendKeys(keys); }},
	typePassword: { value: function (keys) { return this.password.sendKeys(keys); }},
	clickLogin: { value: function () { return this.loginBtn.click(); }},
	clickAngular: { value: function () { return this.angularBtn.click(); }},
	clickForgotPasswordBtn: { value: function () { return this.forgotPasswordBtn.click(); }},
	clickBackToLoginBtn: { value: function () { return this.backToLoginBtn.click(); }},

	//element( by.css('[ng-click="submit()"]') ).click();
});

module.exports = LoginPage;
