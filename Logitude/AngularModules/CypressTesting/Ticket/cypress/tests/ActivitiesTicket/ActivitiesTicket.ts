import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import { URLs } from "../../constants/URLs";

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

Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("create phone call activity", () => {
    cy.DefineRequestWait("POST", URLs.Activity, "WaitPostActivityRequest");
    Actions.CreateCallActivity();
});

Then("the call activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostActivityRequest", 200);
});

When("create task activity", () => {
    cy.DefineRequestWait("POST", URLs.Activity, "WaitPostActivityRequest");
    Actions.CreateTaskActivity();
});

Then("the task activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostActivityRequest", 200);
});

When("create appointment activity", () => {
    cy.DefineRequestWait("POST", URLs.Activity, "WaitPostActivityRequest");
    Actions.CreateAppoimentActivity();
});

Then("the appointment activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostActivityRequest", 200);
});