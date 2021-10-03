import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { QuoteDetails } from '../../models/QuoteDetails';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { QuoteSelectors } from "../../selectors/Selectors";

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
  cy.ClickRadio(QuoteSelectors.RoutingRadioButton)
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

//#region Update and delete partners tab given step
Given("the user open the quote", () => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
});

Given("the user add pickup", () => {
  cy.Click(QuoteSelectors.QuoteRoutingsTab, null)
  QuotesActions.AddPickUp()
});

Given("the user add delivery with {string} as city and {string} as country", (city, country) => {
  QuotesActions.AddDelivery(city, country)
});

When("update quote", () => {
  QuotesActions.UpdateQuote(QuoteSelectors.QuoteSave);
});

Then("the quote should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
});
//#endregion