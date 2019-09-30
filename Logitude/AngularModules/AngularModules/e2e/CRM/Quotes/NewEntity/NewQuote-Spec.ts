import { NewQuote } from './NewQuote';
import { browser, by, element } from 'protractor';
import { LoginComp } from '../../../Login/Login.po';

describe('Quote Module', () => {
  let page: NewQuote = new NewQuote();
  // let login: LoginComp=new LoginComp();
  let count: number = 0;

  afterEach(() => {
    // browser.switchTo().alert().accept();
  })

  it('Quote Success', function () {
    browser.ignoreSynchronization = true;
    page.DoOperations();
  });

});
