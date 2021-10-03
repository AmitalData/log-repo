import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { QuoteDetails } from '../../models/QuoteDetails';
import { QuoteSelectors } from '../../selectors/Selectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";

let quoteDetails: QuoteDetails;

//#region Create export ocean FCL quote
Given("the user logged in and navigates to quotes workspace", () => {
  cy.Login();
  QuotesActions.NavigatesToSQuotesWorkspace();
});

Given("a quote with the following details", (dataTable) => {
  quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
  QuotesActions.FillQuoteFields(quoteDetails);
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

//#region Add/Delete container
Given("the user add a container with the following details", (dataTable) => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
  let packagesDetails = Assists.CreateInstance<PackagesDetails>(dataTable, true);
  QuotesActions.AddContainer(packagesDetails);
});

Given("the user delete the container", () => {
  QuotesActions.DeleteContainer()
});

When("update quote", () => {
  QuotesActions.UpdateQuote(QuoteSelectors.QuoteSave);
});

Then("the quote should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
});
//#endregion