
import { browser, by, element } from 'protractor';
import { AccountingComp } from './AccountingModule';
describe('Accounting Module', () => {
    let accountingComp: AccountingComp = new AccountingComp();

    if (browser.params.Accounting.AccountingType == 'APP') {
        it('AP Payment Success', function () {
            browser.ignoreSynchronization = true;
            accountingComp.DoAccounting(browser.params.Accounting.AccountingType);
            
        });
    }
    else if (browser.params.Accounting.AccountingType == 'ARP') {
        it('AR Payment Success', function () {
            browser.ignoreSynchronization = true;
            accountingComp.DoAccounting(browser.params.Accounting.AccountingType);

        });
    } 
});
