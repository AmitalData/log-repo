
import { browser, by, element } from 'protractor';
import {CRMComp} from './CRMModule'


describe('CRM Module', () => {
    let CRMPage : CRMComp;
  beforeEach(() => {
      CRMPage=new CRMComp();
  });
  afterEach(() => {
  })

  it('Operations Success', function () {
    browser.ignoreSynchronization = true;
    // CRMPage.DoCRM('Overview');
    CRMPage.DoCRM('Customers');
    // CRMPage.DoCRM('Quotes');
    // CRMPage.DoCRM('Activities');
    // CRMPage.DoCRM('Opportunities');
   
  });

});
