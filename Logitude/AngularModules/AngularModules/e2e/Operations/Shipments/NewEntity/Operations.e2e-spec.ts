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
  });
  afterEach(() => {
    console.log(' Cloose session');
    browser.pause();
    // browser.switchTo().alert().accept();

  })

  it('Operations Success', function () {
    browser.ignoreSynchronization = true;
    page.DoOperations();
  });

});
