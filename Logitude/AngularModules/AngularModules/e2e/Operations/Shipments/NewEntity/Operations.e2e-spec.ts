import { OperationsComp } from './Operations.po';
import { browser, by, element } from 'protractor';
import { LoginComp } from '../../../Login/Login.po';

describe('Operations Module', () => {
  let page: OperationsComp = new OperationsComp();
  // let login: LoginComp=new LoginComp();
  let count: number = 0;

  afterEach(() => {
    // browser.switchTo().alert().accept();
  })

  it('Operations Success', function () {
    browser.ignoreSynchronization = true;
    page.DoOperations();
  });

});
