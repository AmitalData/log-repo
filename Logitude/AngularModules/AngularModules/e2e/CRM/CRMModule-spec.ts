
import { browser, by, element } from 'protractor';
import { CRMComp } from './CRMModule'


describe('CRM Module', () => {
  let CRMPage: CRMComp;
  beforeEach(() => {
    CRMPage = new CRMComp();
  });
  afterEach(() => {
  })

  it('Operations Success', function () {
    browser.ignoreSynchronization = true;
    if (browser.params.CRM.CRMType == 'overview') {
      CRMPage.DoCRM('Overview');
    }
    else if (browser.params.CRM.CRMType == 'customer') {
      CRMPage.DoCRM('Customers');
    }
    else if (browser.params.CRM.CRMType == 'quote') {
      CRMPage.DoCRM('Quotes');
    }
    else if (browser.params.CRM.CRMType == 'activity') {
      CRMPage.DoCRM('Activities');
    }
    else if (browser.params.CRM.CRMType == 'opportunity') {
      CRMPage.DoCRM('Opportunities');
    }
  });
});
