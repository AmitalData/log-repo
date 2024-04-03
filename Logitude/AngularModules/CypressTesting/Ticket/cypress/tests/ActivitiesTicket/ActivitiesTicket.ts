import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import { TicketSelectors } from "../../selectors/TicketSelectors";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import { URLs } from "../../constants/URLs";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let TicketData: TicketDetails;
let TicketActivitySubject : string ;

//#region Create Ticket 
Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login();
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    TicketData = Assists.CreateInstance<TicketDetails>(dataTable, true);
    cy.Click(BaseSelectors.Button, TicketSelectors.ContainsNew);
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

//#region Create Activities
Given("the user in the ticket's main page", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
});

Given("a subject as {string}", (subject) => {
    TicketActivitySubject =subject
});

When("create phone call activity", () => {
    Actions.CreateActivity(TicketSelectors.AddCall , TicketActivitySubject);
});

Then("the call activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTicketActivity,200);
});

When("create task activity", () => {
    Actions.CreateActivity(TicketSelectors.AddTask,TicketActivitySubject);
});

Then("the task activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTicketActivity,200);
});

When("create appointment activity", () => {
    cy.DefineRequestWait(RestAPI.POST, URLs.Performancelogs,RequestAliases.WailAllLoad);
    Actions.CreateActivity(TicketSelectors.AddAppoinment,TicketActivitySubject);
});

Then("the appointment activity should appear successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTicketActivity,200);
});
//#endregion

//#region Complete Activities 
When("complete phone call activity", () => {
   Actions.MarkActivitiesAsComplete(TicketSelectors.CLImg);
});

Then("the call activity should complete successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicketActivity,200);
    cy.Click(TicketSelectors.BackButton, null); 
});

When("complete task activity", () => {
    Actions.MarkActivitiesAsComplete(TicketSelectors.TSImg);
});

Then("the task activity should complete successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicketActivity,200);
    cy.Click(TicketSelectors.BackButton, null);
});

When("complete appointment activity", () => {
    Actions.MarkActivitiesAsComplete(TicketSelectors.APImg);
});

Then("the appointment activity should complete successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTicketActivity,200);
    cy.Click(TicketSelectors.BackButton, null);
});
//#endregion