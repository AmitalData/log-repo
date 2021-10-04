import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as OpportunityActions from "../../actions/OpportunityActions";
import * as QuotesActions from '../../../../Quote/cypress/actions/Actions';
import { QuoteDetails } from '../../../../Quote/cypress/models/QuoteDetails';
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { OpportunityDetails } from "cypress/models/OpportunityDetails";
import { ActivitiesDetails } from "cypress/models/ActivitiesDetails";
import { OpportunitySelectors } from "../../selectors/OpportunitySelectors"
import * as ActivitiesActions from "../../actions/ActivitiesActions";

//#region Create export air quote
Given("the user logged in and navigates to quotes workspace", () => {
    cy.Login();
    QuotesActions.NavigatesToSQuotesWorkspace();
});

Given("a quote with the following details", (dataTable) => {
    let quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
    QuotesActions.FillQuoteFields(quoteDetails);
});

When("create quote", () => {
    QuotesActions.CreateQuote();
});

Then("the quote should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200)
});
//#endregion 

//#region Create new Opportunity
Given("the user navigate Opportunity workspace and fill the following details", (dataTable) => {
    OpportunityActions.NavigatesOpportunityWorkSpace();
    let opportunityDetails = Assists.CreateInstance<OpportunityDetails>(dataTable, true);
    OpportunityActions.FillOpportunityWizardsFields(opportunityDetails);
});

When("create Opportunity", () => {
    OpportunityActions.CreateOpportunity();
});

Then("the Opportunity should create successfully", () => {
    OpportunityActions.AssertCreateOpportunity()
});
//#endregion

//#region Search for the Opportunity by subject
When("search Opportunity", () => {
    OpportunityActions.SearchOpportunity()
});

Then("the Opportunity should appear successfully", () => {
    OpportunityActions.AssertSearchOpportunity()
});
//#endregion

//#region Open the Opportunity
When("open Opportunity", () => {
    OpportunityActions.OpenOpportunity();
});

Then("the Opportunity should open successfully", () => {
    OpportunityActions.AssertOpenOpportunity();
});
//#endregion

//#region add new task
Given("navigate task wizerd and fill the following details", (dataTable) => {
    cy.Click(OpportunitySelectors.AddTask, null)
    let taskDetails = Assists.CreateInstance<ActivitiesDetails>(dataTable, true);
    ActivitiesActions.FillTaskWizardsFields(taskDetails);
});

When("create task", () => {
    ActivitiesActions.CreateActivity();
});

Then("the task should create successfully", () => {
    ActivitiesActions.AssertCreateActivity()
});
//#endregion

//#region mark the task as complete
When("press on Complete button", () => {
    OpportunityActions.MarkTaskAsComplete();
});

Then("the task should get complete", () => {
    OpportunityActions.AssertCompleteActivity();
});
//#endregion

//#region add Competitors
Given("add Competitor", () => {
    OpportunityActions.AddCompetitor()
});

When("save Opportunity", () => {
    OpportunityActions.UpdateOpportunity();
});

Then("the Opportunity should update successfully", () => {
    OpportunityActions.AssertUpdateOpportunity()
});
//#endregion

//#region remove Competitors
Given("remove Competitor", () => {
    OpportunityActions.RemoveCompetitor()
});
//#endregion

//#region add Additional Services
Given("add Additional Service", () => {
    OpportunityActions.AddAdditionalService()
});
//#endregion

//#region remove Additional Services
Given("remove Additional Service", () => {
    OpportunityActions.RemoveAdditionalService()
});
//#endregion

//#region Cadd export air quote from inside the Opportunity
Given("the user navigates to quotes workspace from inside the Opportunity", () => {
    cy.Click(OpportunitySelectors.AddQuote, null)
});

Given("the user fill a quote with the following details", (dataTable) => {
    let quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
    OpportunityActions.FillQuoteWizerdFields(quoteDetails);
});

When("create quote from inside the Opportunity", () => {
    OpportunityActions.CreateQuote();
});

Then("the quote should connect successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuotesByOpportunityId, 200)
});
//#endregion

//#region connect Quote
Given("the user choose a quote to connect it to the Opportunity", () => {
    OpportunityActions.ChooseQuote()
});

When("connect quote", () => {
    OpportunityActions.ConnectQuote();
});
//#endregion

//#region Edit general tab
Given("fill {string} as description", (description) => {
    OpportunityActions.FillDescription(description)
});
//#endregion