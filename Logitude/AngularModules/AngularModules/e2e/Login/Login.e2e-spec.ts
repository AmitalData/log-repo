import { LoginComp } from './Login.po';
import { browser, by, element } from 'protractor';

describe('Login Module', () => {

  let page: LoginComp = new LoginComp();
  beforeEach(() => {
    browser.driver.manage().window().maximize();
  });


  it('Login Success', function () {
    browser.ignoreSynchronization = true;
    page.navigateTo('https://system.logitudeworld.com');
    page.DoLogin('razantest@protractor.com', '!R123j456');
  });
});
