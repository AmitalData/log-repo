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
    cy.Click(BaseSelectors.Button,"New");
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
    cy.DefineRequestWait("POST",URLs.CRMDomain, "WaitPostTicketRequest")
    cy.Click(BaseSelectors.RedButton, "Create");
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostTicketRequest", 200).then((interception) => {
        TicketData.TicketNumber = interception.response.body.TicketNumber;
    })
});
//#endregion

//#region Actions 
Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("cancel", () => {
    cy.DefineRequestWait("PUT",URLs.Tickets, "WaitPutTicketRequest")
    Actions.CancelTicket();
});

Then("the ticket should cancel successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutTicketRequest", 200);
});

When("reactivate", () => {
    cy.DefineRequestWait("PUT",URLs.Tickets, "WaitPutTicketRequest");
    Actions.ReactivateTicket();
});

Then("the ticket should reactivate successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutTicketRequest", 200);
});

When("close without notifying",()=>{
    cy.DefineRequestWait("PUT",URLs.Tickets, "WaitPutTicketRequest");
    Actions.CloseTicket();
})

Then("the ticket should close successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPutTicketRequest", 200);
});
//#endregion