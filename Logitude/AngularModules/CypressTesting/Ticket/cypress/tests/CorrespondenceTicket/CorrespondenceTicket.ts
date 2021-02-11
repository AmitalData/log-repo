import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";

let TicketData: TicketDetails;

Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    let ticketDetails = dataTable.hashes()[0] as TicketDetails;
    TicketData = ticketDetails;
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
    cy.DefineRequestWait("POST", "**/tickets", "WaitPostTicketRequest")
    cy.Click(BaseSelectors.RedButton, "Create");
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostTicketRequest", 200);
});

Given("the user in the ticket's main page || the user open the ticket", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("reply || reply to the correspondence", () => {
    cy.DefineRequestWait("PUT", "**/tickets", "WaitPutTicketRequest")
    Actions.ReplyTicket();
});

Then("the reply should appear successfully || the ticket should update successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutTicketRequest", 200);
});

// Given("the user in the ticket's main page || the user open the ticket", () => {

// });

When("add an internal note", () => {
    cy.DefineRequestWait("PUT", "**/tickets", "WaitPutTicketRequest")
    Actions.AddInternalNoteTicket();
});

Then("the internal note should appear successfully || the ticket should update successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutTicketRequest", 200);
});