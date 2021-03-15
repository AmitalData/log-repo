import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { QuoteDetails } from '../../models/QuoteDetails';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import { QuoteURLs } from "../../constants/URLs";
import {EventTypeDetails} from "../../../../Base/cypress/models/EventTypeDetails"
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
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

Given("the user open the quote", () => {
    QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
});

//#region Set as Sent , Return to Draft , Cancel and Reactivate
When("{string} action with {string} note", (action, note) => {
    QuotesActions.OpenQuoteAction(action,note);
});

Then("quote stage status should be {string}", (stageStatus) => {
    BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, stageStatus);
});

Then("following events should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList,QuoteSelectors.QuoteEventsTab);
});
//#endregion

//#region Copy quote 
When("Copy the quote", () => {
  QuotesActions.CopyQuote("Copy Quote")
});

Then("following events should appear in copied events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    eventDetailsList=QuotesActions.QuoteConversionEventsMapping(eventDetailsList,quoteDetails.QuoteNumber)
    BaseActions.ValidateEventsTab(eventDetailsList,QuoteSelectors.CopiedQuoteEventsTab);
});

Then("partners tab contains {string} as shipper", (shipperName) => {
    cy.Navigate(QuoteSelectors.CopiedQuotePartnersTab)
    BaseAssertion.AssertElementContain(BaseSelectors.Title,shipperName)
});

Then("packages tab contains the following", (dataTable) => {
    let expectedOrderDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    cy.Navigate(QuoteSelectors.CopiedQuotePackagesTab)
    QuotesActions.ValidatePackageCells(expectedOrderDetails);
});
//#endregion 


