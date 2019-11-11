
import { browser, by, element } from 'protractor';
import { APPaymentComponent } from './NewFolder1/APPayment'

describe('Accounting Module', () => {

    let paymentTab: APPaymentComponent = new APPaymentComponent();

    beforeEach(() => {
    });
    afterEach(() => {
    });

    //if (browser.params.CRM.CRMType == 'overview') {
        it('Payment Success', function () {
            browser.ignoreSynchronization = true;
            paymentTab.Shipment();

            paymentTab.accounting();
        });
    //}
    //else if (browser.params.CRM.CRMType == 'customer') {
    //    it('Customers Success', function () {
    //        browser.ignoreSynchronization = true;

    //    });
    //}
    //else if (browser.params.CRM.CRMType == 'quote') {
    //    it('Quotes Success', function () {
    //        browser.ignoreSynchronization = true;

    //    });
    //}
  
 });
