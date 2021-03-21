import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { QuoteDetails } from '../../models/QuoteDetails';
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region variables
let quoteDetails: QuoteDetails;
//#endregion

//#region Create export air quote
Given("the user logged in and navigates to quotes workspace", () => {
  cy.Login();
  QuotesActions.NavigatesToSQuotesWorkspace();
});

Given("a quote with the following details", (dataTable) => {
  quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
  QuotesActions.FillQuoteFields(quoteDetails);
});

Given("an expected order with the following details", (dataTable) => {
  let expectedOrderDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  QuotesActions.FillExpectedOrderDetailsDimensions(expectedOrderDetails);
});

When("create quote", () => {
  QuotesActions.CreateQuote();
});

Then("the quote should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200).then((interception) => {
    quoteDetails.QuoteNumber = interception.response.body.QuoteNumber;
  })
});
//#endregion

//#region Print quotation
Given("the user in quote quotation", () => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
  QuotesActions.OpenQuotation();
});

When("print the quotation", () => {
  QuotesActions.PrintQuotation();
});

Then("a new page should open successfully", () => {
  BaseAssertion.AssertWindowOpen(RequestAliases.PrintQuotationWindowOpen);
});
//#endregion

//#region Edit quotation
When("the user add {string} data field to quotation introduction", (dataField) => {
  QuotesActions.AddDataFieldToQuotationIntroduction(dataField);
  QuotesActions.UpdateQuotation();
});

Then("the quotation should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.UpdateQuotation, 200);
  QuotesActions.WaitQuotationLoading();
});
//#endregion

//#region Send quotation to customer
When("the user send quotation to the logged in user", () => {
  QuotesActions.SendQuotationToLoggedInUser();
});

Then("the quotation should send successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.SentToCustomer, 200);
  QuotesActions.WaitQuotationLoading();
});
//#endregion

//#region Assert stage status
Then("quote stage status should be {string}", (stageStatus) => {
  BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, stageStatus);
});
//#endregion