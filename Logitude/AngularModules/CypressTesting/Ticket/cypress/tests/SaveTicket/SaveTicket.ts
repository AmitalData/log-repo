import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { TicketSelectors } from "../../selectors/TicketSelectors";

let TicketData: TicketDetails;

//#region create ticket 
Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login();
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    TicketData = dataTable.hashes()[0] as TicketDetails;
    cy.Click(BaseSelectors.Button,TicketSelectors.ContainsNew);
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
Actions.CreateTicket()
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTicket, 200).then((interception) => {
        TicketData.TicketNumber = interception.response.body.TicketNumber;
    })
});
//#endregion

Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("save as close", () => {
    Actions.SaveTicket(TicketSelectors.ContainsSaveAsClosed)
    cy.Click(BaseSelectors.RedButton , TicketSelectors.ContainsOk);
});

When("save as open", () => {
    Actions.SaveTicket(TicketSelectors.ContainsSaveAsOpen);
});

When("save as resolve", () => {
    Actions.SaveTicket(TicketSelectors.ContainsSaveAsResolved);
    cy.Click(BaseSelectors.RedButton , TicketSelectors.ContainsOk);
});

Then("the ticket should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicket, 200);
});
