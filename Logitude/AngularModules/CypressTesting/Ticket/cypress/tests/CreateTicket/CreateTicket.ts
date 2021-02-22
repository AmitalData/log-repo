import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { TicketSelectors } from "../../selectors/TicketSelectors";

let TicketData: TicketDetails;

Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    TicketData = dataTable.hashes()[0] as TicketDetails;
    cy.Click(BaseSelectors.Button ,TicketSelectors.ContainsNew);
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
    Actions.CreateTicket();
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTicket, 200);
});
