import { LoginComp } from './Login.po';
import { browser, by, element } from 'protractor';

describe('Login Module', () => {

  let page: LoginComp = new LoginComp();
  beforeEach(() => {
    browser.driver.manage().window().maximize();
  });


  it('Login Success', function () {
    browser.ignoreSynchronization = true;
      //page.navigateTo('https://system.logbox.co.il');
      page.navigateTo(' http://localhost:4200');
    page.DoLogin('ahmadb@test.com', 'ahmed!A123');
  });
});
