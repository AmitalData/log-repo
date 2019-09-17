import { NewQuote } from './NewQuote';
import { browser, by, element } from 'protractor';
import { LoginComp } from '../../../Login/Login.po';

describe('Quote Module', () => {
    let page: NewQuote = new NewQuote();
    // let login: LoginComp=new LoginComp();

    var QuoteNumber;
    afterEach(() => {
        // browser.switchTo().alert().accept();
    })

    it('Create Quote .. ', function () {
        browser.ignoreSynchronization = true;
        QuoteNumber = page.DoQuoteActions();
        console.log(QuoteNumber);
    });
    it('Search For Quote ..', function () {
        console.log('inside the search it : ' + QuoteNumber);
        browser.ignoreSynchronization = true;
        page.SearchForQuote(QuoteNumber);
    });
    it('Edit Quote .. ', function () {
        browser.ignoreSynchronization = true;
        page.EditQuote(QuoteNumber);


    });

});
