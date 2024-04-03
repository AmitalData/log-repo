import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { QuoteDetails } from '../../models/QuoteDetails';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails"
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { QuoteSelectors } from "../../selectors/Selectors";

let quoteDetails: QuoteDetails;

//#region Create export air quote
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

//#region Accept quote
Given("the user open the quote", () => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
});

When("{string} action with {string} notes", (action, note) => {
  QuotesActions.QuoteActionAcceptDecline(action, note);
});

Then("quote stage status should be {string}", (stageStatus) => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
  BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, stageStatus);
});

Then("following event should appear in events tab", (dataTable) => {
  let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
  BaseActions.ValidateEventsTab(eventDetailsList, QuoteSelectors.QuoteEventsTab);
});

Then("the quote should appear in Accept quotes list", () => {
  QuotesActions.AssertQuoteExistInCorrectList(QuoteSelectors.AcceptQuery, quoteDetails.QuoteNumber);
});
//#endregion