import { TicketDetails } from "../models/TicketDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
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
    cy.Click(TicketSelectors.SendAsOpenButton, "Send and set as Open");
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

export function SaveTicket(SaveType: string) {
    cy.DefineRequestWait("PUT", URLs.Tickets, "WaitPutTicketRequest")
    cy.Click(TicketSelectors.SaveMenuDropButton, null);
    cy.Click(TicketSelectors.SaveMenuButton, SaveType);
}

