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

//#region Delete all charges
Given("the user open the quote", () => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
});

Given("the user delete all charges", () => {
  cy.Click(QuoteSelectors.QuoteCharges, null)
  QuotesActions.DeleteAllCharges()
});

When("update quote", () => {
  QuotesActions.UpdateQuote(QuoteSelectors.QuoteSave);
});

Then("the quote should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
});
//#endregion

//#region Add charge with fixed sale currency mode
Given("the user add charge with {string} as charge type", (chargeType) => {
  QuotesActions.AddCharge(chargeType)
});

Then("sale currency should have {string} value", (currencyValue) => {
  QuotesActions.AssertSaleCurrencyValue(currencyValue);
});
//#endregion

//#region Change currency mode to same as cost 
Given("the user change currency mode to same as cost currency", () => {
  QuotesActions.ChangeSaleCurrencyModeValueToSameAsCost()
});
//#endregion