
import { browser, by, element } from 'protractor';
import { CRMComp } from './CRMModule'


describe('CRM Module', () => {
    let CRMPage: CRMComp;
    beforeEach(() => {
        CRMPage = new CRMComp();
    });
    afterEach(() => {
    })

    //it('Operations Success', function () {
    //    browser.ignoreSynchronization = true;
    //    if (browser.params.CRM.CRMType == 'overview') {
    //        CRMPage.DoCRM('Overview');
    //    }
    //    else if (browser.params.CRM.CRMType == 'customer') {
    //        CRMPage.DoCRM('Customers');
    //    }
    //    else if (browser.params.CRM.CRMType == 'quote') {
    //        CRMPage.DoCRM('Quotes');
    //    }
    //    else if (browser.params.CRM.CRMType == 'activity') {
    //        CRMPage.DoCRM('Activities');
    //    }
    //    else if (browser.params.CRM.CRMType == 'opportunity') {
    //        CRMPage.DoCRM('Opportunities');
    //    }
    //});

    if (browser.params.CRM.CRMType == 'overview') {
        it('Overview Success', function () {
            browser.ignoreSynchronization = true;
            CRMPage.DoCRM('Overview');
        });
    }
    else if (browser.params.CRM.CRMType == 'customer') {
        it('Customers Success', function () {
            browser.ignoreSynchronization = true;
            CRMPage.DoCRM('Customers');
        });
    }
    else if (browser.params.CRM.CRMType == 'quote') {
        it('Quotes Success', function () {
            browser.ignoreSynchronization = true;
            CRMPage.DoCRM('Quotes');
        });
    }
    else if (browser.params.CRM.CRMType == 'activity') {
        it('Activities Success', function () {
            browser.ignoreSynchronization = true;
            CRMPage.DoCRM('Activities');
        });
    }
    else if (browser.params.CRM.CRMType == 'opportunity') {
        it('Opportunities Success', function () {
            browser.ignoreSynchronization = true;
            CRMPage.DoCRM('Opportunities');
        });
    }
});
