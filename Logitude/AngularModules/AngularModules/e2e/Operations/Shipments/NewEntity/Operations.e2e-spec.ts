import { OperationsComp } from './Operations.po';
import { browser, by, element } from 'protractor';
import { LoginComp } from '../../../Login/Login.po';
describe('Operations Module', () => {
  let page: OperationsComp;
  let login: LoginComp;
  let count: number = 0;
  beforeEach(() => {
    page = new OperationsComp();
    login = new LoginComp();
    // browser.driver.manage().window().maximize();

  });

  // it('Operations Success', function () {
  //       browser.ignoreSynchronization = true;
  //       page.DoOperations();

  //     });
  // afterEach(() => {
  //   console.log('After each spec print ');
  //   browser.pause();
  //   browser.switchTo().alert().accept();

  // })

  it('Operations Success', function () {
    // count++;
    browser.ignoreSynchronization = true;
    // login.navigateTo();
    // login.DoLogin();
    page.DoOperations();
  });

  // }

  // it('Operations 2', function () {
  //   browser.ignoreSynchronization = true;
  //   login.navigateTo();
  //   login.DoLogin();
  //   page.DoOperations();

  // });
  // it('Operations 3', function () {
  //   browser.ignoreSynchronization = true;
  //   login.navigateTo();
  //   login.DoLogin();
  //   page.DoOperations();

  // });
  // it('Operations 4', function () {
  //   browser.ignoreSynchronization = true;
  //   login.navigateTo();
  //   login.DoLogin();
  //   page.DoOperations();

  // });
  // it('Operations 5', function () {
  //   browser.ignoreSynchronization = true;
  //   login.navigateTo();
  //   login.DoLogin();
  //   page.DoOperations();

  // });

});
