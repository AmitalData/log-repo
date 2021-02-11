import { TicketDetails } from "../models/TicketDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TicketSelectors } from "../selectors/Selectors";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion"
import { selectors } from "sizzle";


export function FillTicketFields(ticketDetails : TicketDetails){
    cy.FillLogLov(TicketSelectors.TicketCompany,ticketDetails.Company,true);
    cy.FillLogLov(TicketSelectors.TicketContact,ticketDetails.Contact,true);
    cy.FillLogTextBox(TicketSelectors.TicketSubject,ticketDetails.Subject);
    cy.FillLogTextBox(TicketSelectors.TicketDescription,ticketDetails.Description);
    cy.FillLogLov(TicketSelectors.TicketClassification,ticketDetails.MainClassification,true)
}

export function OpenTicket(TicketNumber: string) {
    cy.SelectQuickSearchFirstElement2(TicketSelectors.TicketSearchBar, TicketNumber)
}

export function EditTheTicket(){
    cy.FillLogTextBox(TicketSelectors.TicketDescription,"Test Ticket Edited")
}


