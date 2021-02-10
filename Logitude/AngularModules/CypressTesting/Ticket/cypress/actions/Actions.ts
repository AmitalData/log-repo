import { TicketDetails } from "../models/TicketDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TicketSelectors } from "../selectors/Selectors";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion"


export function FillTicketFields(ticketDetails : TicketDetails){
    cy.FillLogLov(TicketSelectors.TicketEntityType,ticketDetails.EntityType,false);
    cy.FillLogLov(TicketSelectors.TicketCompany,ticketDetails.Company,false);
    cy.FillLogLov(TicketSelectors.TicketContact,ticketDetails.Contact,false);
    cy.FillLogTextBox(TicketSelectors.TicketSubject,ticketDetails.Subject);
    cy.FillLogTextBox(TicketSelectors.TicketDescription,ticketDetails.Description);
    cy.FillLogLov(TicketSelectors.TicketClassification,ticketDetails.MainClassification,false)
}

export function OpenTicket(TicketNumber: string) {
    cy.SelectQuickSearchFirstElement(TicketSelectors.TicketSearchBar, TicketNumber)
}

export function FillEntityNumber(EntityNumber:string){
    cy.FillLogLov(TicketSelectors.TicketShipmentEntityNumber,EntityNumber,false);
}


