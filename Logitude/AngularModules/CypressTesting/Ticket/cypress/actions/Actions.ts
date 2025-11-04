import { TicketDetails } from "../models/TicketDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TicketSelectors } from "../selectors/TicketSelectors";
import { URLs } from "../constants/URLs";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { QuickSearchDetails } from "../../../Base/cypress/models/QuickSearchDetails";

export function FillTicketFields(ticketDetails: TicketDetails) {
    cy.FillLogLov(TicketSelectors.TicketCompany, ticketDetails.Company, true);
    cy.FillLogLov(TicketSelectors.TicketContact, ticketDetails.Contact, true);
    cy.FillLogTextBox(TicketSelectors.TicketSubject, ticketDetails.Subject);
    cy.FillLogTextBox(TicketSelectors.TicketDescription, ticketDetails.Description);
    cy.FillLogLov(TicketSelectors.TicketClassification, ticketDetails.MainClassification, true)
}

export function CreateTicket(){
    cy.DefineRequestWait(RestAPI.POST,URLs.PostTicket,RequestAliases.PostTicket)
    cy.Click(BaseSelectors.RedButton,TicketSelectors.ContainsCreate);
}

export function UpdateTicket(){
    cy.DefineRequestWait(RestAPI.PUT,URLs.Tickets,RequestAliases.PutTicket)
    cy.Click(BaseSelectors.SaveAsOpenButton, null);
}

export function OpenTicket(TicketNumber: string) {
    var quickSearchDetails = {
        Selector: TicketSelectors.TicketSearchBar,
        Parent: TicketSelectors.TicketParent,
        ParentClass: TicketSelectors.TicketParentClass,
        WaitURL: URLs.TicketSearchURL,
        Value: TicketNumber,
        RequestAliase: RequestAliases.QuickSearchDataLoaded
    } as QuickSearchDetails;
    cy.SelectQuickSearchFirstElement(quickSearchDetails)
}

export function EditTheTicket(description:string) {
    cy.FillLogTextBox(TicketSelectors.TicketDescription,description)
}

export function ReplyTicket() {
    cy.DefineRequestWait(RestAPI.POST,URLs.Correspondences,RequestAliases.PostTicketCorrespondences)
    cy.Click(TicketSelectors.Reply, null);
    cy.FillLogTextBox(TicketSelectors.CorrespondenceLine,TicketSelectors.ContainsSendReply);
    cy.Click(TicketSelectors.SendAsOpenButton,TicketSelectors.ContainsSendAndSetAsOpen);
}

export function AddInternalNoteTicket() {
    cy.DefineRequestWait(RestAPI.POST,URLs.Correspondences,RequestAliases.PostTicketCorrespondences)
    cy.Click(TicketSelectors.InternalNote, null);
    cy.FillLogTextBox(TicketSelectors.CorrespondenceLine,TicketSelectors.ContainsSendInternalNote);
    cy.get(TicketSelectors.SendAsOpenButton).eq(1).click();
}

export function CancelTicket() {
    cy.DefineRequestWait(RestAPI.PUT,URLs.Tickets,RequestAliases.PutTicket)
    // Wait for MenuButtons to exist and use force click since it may be covered
    cy.get(TicketSelectors.MenuButtons, { timeout: 5000 }).should('exist').click({ force: true });
    cy.wait(500); // Small wait for menu to appear
    cy.Click(TicketSelectors.TicketCancel, null, true);
    // Wait for confirmation dialog to appear, then click the 'כן' button directly by ID
    cy.get(BaseSelectors.ConfirmWindow, { timeout: 5000 }).should('be.visible');
    cy.get(BaseSelectors.ConfirmWindowButton, { timeout: 5000 }).should('exist').click({ force: true });
}

export function ReactivateTicket() {
    cy.DefineRequestWait(RestAPI.PUT,URLs.Tickets,RequestAliases.PutTicket)
    // Wait for MenuButtons to exist and use force click since it may be covered
    cy.get(TicketSelectors.MenuButtons, { timeout: 10000 }).should('exist').click({ force: true });
    cy.wait(1000); // Wait for menu to appear
    // Click Reactivate button with force
    cy.get(TicketSelectors.TicektReactivate, { timeout: 5000 }).should('exist').click({ force: true });
    // Wait for confirmation dialog and click Yes button
    cy.get(BaseSelectors.ConfirmWindow, { timeout: 5000 }).should('be.visible');
    cy.get(BaseSelectors.ConfirmWindowButton, { timeout: 5000 }).should('exist').click({ force: true });
}

export function CloseTicket() {
    cy.DefineRequestWait(RestAPI.PUT,URLs.Tickets,RequestAliases.PutTicket)
    // Wait for MenuButtons to exist and use force click since it may be covered
    cy.get(TicketSelectors.MenuButtons, { timeout: 10000 }).should('exist').click({ force: true });
    cy.wait(1000); // Wait for menu to appear
    // Click Close Without Notifying button with force
    cy.get(TicketSelectors.TicektClosewithoutNotifying, { timeout: 5000 }).should('exist').click({ force: true });
    // Wait for confirmation dialog and click Ok button
    cy.get(BaseSelectors.ConfirmWindow, { timeout: 5000 }).should('be.visible');
    cy.contains('button', TicketSelectors.ContainsOk, { timeout: 5000 }).should('be.visible').click({ force: true });
}

export function CreateActivity(ActivityTypeButton: string ,ticketActivitySubject:string) {
    cy.DefineRequestWait(RestAPI.POST, URLs.Activity,RequestAliases.PostTicketActivity);
    cy.Click(ActivityTypeButton, null);
    cy.FillLogTextBox(TicketSelectors.ActivitySubject,ticketActivitySubject);
    cy.Click(BaseSelectors.RedButton,TicketSelectors.ContainsOk);
}

export function MarkActivitiesAsComplete(ActivityImg: string) {
    cy.Click(ActivityImg, null);
    cy.DefineRequestWait(RestAPI.PUT, URLs.Activity,RequestAliases.PutTicketActivity);
    cy.Click(TicketSelectors.MarkAsComplete, null);
}

export function SaveTicket(SaveType: string) {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Tickets,RequestAliases.PutTicket)
    cy.Click(TicketSelectors.SaveMenuDropButton, null);
    cy.Click(TicketSelectors.SaveMenuButton, SaveType);
}