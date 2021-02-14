import { TicketDetails } from "../models/TicketDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion"
import { TicketSelectors } from "../selectors/TicketSelectors";
import { URLs } from "../constants/URLs";

export function FillTicketFields(ticketDetails: TicketDetails) {
    cy.FillLogLov(TicketSelectors.TicketCompany, ticketDetails.Company, true);
    cy.FillLogLov(TicketSelectors.TicketContact, ticketDetails.Contact, true);
    cy.FillLogTextBox(TicketSelectors.TicketSubject, ticketDetails.Subject);
    cy.FillLogTextBox(TicketSelectors.TicketDescription, ticketDetails.Description);
    cy.FillLogLov(TicketSelectors.TicketClassification, ticketDetails.MainClassification, true)
}

export function OpenTicket(TicketNumber: string) {
    cy.SelectQuickSearchFirstElement2(TicketSelectors.TicketSearchBar, TicketNumber)
}

export function EditTheTicket() {
    cy.FillLogTextBox(TicketSelectors.TicketDescription, "Test Ticket Edited")
}

export function ReplyTicket() {
    cy.Click(TicketSelectors.Reply, null);
    cy.FillLogTextBox(TicketSelectors.CorrespondenceLine, "Send Reply");
    cy.Click(TicketSelectors.SendAsOpenButton, "Send and set as Open");
}

export function AddInternalNoteTicket() {
    cy.Click(TicketSelectors.InternalNote, null);
    cy.FillLogTextBox(TicketSelectors.CorrespondenceLine, "Send Internal Note");
    cy.Click(TicketSelectors.SendAsOpenButton, "Send and set as Open");//not done
}

export function CancelTicket() {
    cy.Click(TicketSelectors.MenuButtons, null);
    cy.Click(TicketSelectors.TicketCancel, null);
    cy.Click(BaseSelectors.RedButton, "Yes");
}

export function ReactivateTicket() {
    cy.Click(TicketSelectors.MenuButtons, null);
    cy.Click(TicketSelectors.TicektReactivate, null);
    cy.Click(BaseSelectors.RedButton, "Yes");
}

export function CloseTicket() {
    cy.Click(TicketSelectors.MenuButtons, null);
    cy.Click(TicketSelectors.TicektClosewithoutNotifying, null);
    cy.Click(BaseSelectors.RedButton, "Ok");
}

export function CreateActivity(ActivityTypeButton : string) {
    cy.Click(ActivityTypeButton, null);
    cy.FillLogTextBox(TicketSelectors.ActivitySubject, "Create Activity Test");
    cy.Click(BaseSelectors.RedButton, "Ok");
}

export function MarkActivitiesAsComplete(){
    for(var i=0;i<3;i++){
        cy.get('.HyperlinkButtonControl').children().eq(i).click({force:true});
        cy.Click(TicketSelectors.MarkAsComplete,null);
        cy.DefineRequestWait("PUT", URLs.Activity, "WaitPutActivityRequest");
        BaseAssertion.AssertStatusCode("WaitPutActivityRequest", 200);
        cy.Click(TicketSelectors.BackButton,null);
    }
}

