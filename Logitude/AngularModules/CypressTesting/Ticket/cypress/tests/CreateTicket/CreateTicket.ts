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
