import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { TicketSelectors } from "../../selectors/TicketSelectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let TicketData: TicketDetails;

//#region Create Ticket 
Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login();
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    TicketData = Assists.CreateInstance<TicketDetails>(dataTable, true);
    cy.Click(BaseSelectors.Button,TicketSelectors.ContainsNew);
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
    Actions.CreateTicket();
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTicket,200).then((interception) => {
        TicketData.TicketNumber = interception.response.body.TicketNumber;
    })
});
//#endregion

//#region Actions 
Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("cancel", () => {
    Actions.CancelTicket();
});

Then("the ticket should cancel successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicket, 200);
});

When("reactivate", () => {
    Actions.ReactivateTicket();
});

Then("the ticket should reactivate successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicket, 200);
});

When("close without notifying",()=>{
    Actions.CloseTicket();
})

Then("the ticket should close successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicket, 200);
});
//#endregion