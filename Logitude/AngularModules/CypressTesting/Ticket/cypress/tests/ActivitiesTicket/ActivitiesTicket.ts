import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import { URLs } from "../../constants/URLs";
import { TicketSelectors } from "../../selectors/TicketSelectors";

let TicketData: TicketDetails;

//#region Create Ticket 
Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login();
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    let ticketDetails = dataTable.hashes()[0] as TicketDetails;
    TicketData = ticketDetails;
    cy.Click(BaseSelectors.Button, "New");
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
    cy.DefineRequestWait("POST", URLs.CRMDomain, "WaitPostTicketRequest")
    cy.Click(BaseSelectors.RedButton, "Create");
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostTicketRequest", 200).then((interception) => {
        TicketData.TicketNumber = interception.response.body.TicketNumber;
    })
});
//#endregion

//#region Create Activities
Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("create phone call activity", () => {
    cy.DefineRequestWait("POST", URLs.Activity, "WaitPostActivityRequest");
    Actions.CreateActivity(TicketSelectors.TdCall);
});

Then("the call activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostActivityRequest", 200);
});

When("create task activity", () => {
    cy.DefineRequestWait("POST", URLs.Activity, "WaitPostActivityRequest");
    Actions.CreateActivity(TicketSelectors.TdTask);
});

Then("the task activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostActivityRequest", 200);
});

When("create appointment activity", () => {
    cy.DefineRequestWait("POST", URLs.Activity, "WaitPostActivityRequest");
    cy.DefineRequestWait("POST", "performancelogs", "WAITGET");
    Actions.CreateActivity(TicketSelectors.TdAppoinment);
});

Then("the appointment activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostActivityRequest", 200);
    BaseAssertion.AssertStatusCode("WAITGET", 200);
});
//#endregion

//#region Complete Activities 
When("complete phone call activity", () => {
   Actions.MarkActivitiesAsComplete(TicketSelectors.CLImg);
});

Then("the call activity should complete successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutActivityRequest", 200);
    cy.Click(TicketSelectors.BackButton, null);
});

When("complete task activity", () => {
    Actions.MarkActivitiesAsComplete(TicketSelectors.TSImg);
});

Then("the task activity should complete successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutActivityRequest", 200);
    cy.Click(TicketSelectors.BackButton, null);
});

When("complete appointment activity", () => {
    Actions.MarkActivitiesAsComplete(TicketSelectors.APImg);
});

Then("the appointment activity should complete successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutActivityRequest", 200);
    cy.Click(TicketSelectors.BackButton, null);
});
//#endregion