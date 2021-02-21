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

//#region Correspondence
Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

When("reply to the correspondence", () => {
    cy.DefineRequestWait("POST",URLs.Correspondences, "WaitPostCorrespondenceRequest")
    Actions.ReplyTicket();
});

Then("the reply should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostCorrespondenceRequest", 200);
});

When("add an internal note", () => {
    cy.DefineRequestWait("POST",URLs.Correspondences, "WaitPostCorrespondenceRequest")
    Actions.AddInternalNoteTicket();
});

Then("the internal note should appear successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostCorrespondenceRequest", 200);
});
//#endregion