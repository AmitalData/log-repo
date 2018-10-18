var Helper = function () {

    var userName;
    var password;
    var loginbtn;

    this.get = function () {
        browser.driver.get('http://test.logitudeworld.com/test');
        //browser.driver.get('https://system.logitudeworld.com');
        //browser.driver.get('http://192.168.1.70/main/');
    };

    this.setUserName = function (name) {
        userName = browser.driver.findElement(by.id('Email'));
        userName.sendKeys(name);
    };

    this.setPassword = function (pass) {
        password = browser.driver.findElement(by.id('Password'));
        password.sendKeys(pass);
    };

    this.login = function () {
        this.get();
        var params = browser.params; 
        this.setUserName(params.login.user);
        this.setPassword(params.login.password);
        loginbtn = browser.driver.findElement(by.id('cmdLogin'));
        loginbtn.click();
    };

    this.waittime = function (waittime) { 
        browser.driver.wait(waittime);
    };

    this.waitByCss = function (name, waittime) {
        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.css(name));
        }, waittime);
    };

    this.waitById = function (name, waittime) {
        browser.driver.wait(function () {
            return browser.driver.isElementPresent(by.id(name));
        }, waittime);
    };
    this.waitByCallerName = function (callName, name, waittime) {
        if (callName == 'css') {
            browser.driver.wait(function () {
                return browser.driver.isElementPresent(by.css(name));
            }, waittime);
        }
        else if (callName == 'id') {
            browser.driver.wait(function () {
                return browser.driver.isElementPresent(by.id(name));
            }, waittime);
        }
    };
 

    this.waitVisibilityByCss = function (name, waittime) {

        var EC = protractor.ExpectedConditions;
        browser.wait(EC.visibilityOf(name), waittime);

    };

    this.saveBrowserLogs = function () {
        browser.manage().logs()
    .get('browser').then(function (browserLog) {
        console.log('log: ' +
          require('util').inspect(browserLog));
    });
    };

    this.log = function (element) {
        element.getText().then(function (value) {
            console.log(value)
        });
    };

};

module.exports = Helper;