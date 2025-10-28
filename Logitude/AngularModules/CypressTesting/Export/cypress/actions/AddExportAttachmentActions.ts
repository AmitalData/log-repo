import { AddExportAttachmentSelectors } from "../selectors/AddExportAttachmentSelectors";
import { AddExportAttachmentDetails } from "cypress/models/AddExportAttachmentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';


export function NavigatesExportWizerd() {
    cy.Click(BaseExportSelectors.ExportDeclaration, null);
}



export function FillSearchField(addExportAttachmentDetails: AddExportAttachmentDetails) {
    cy.FillLogTextBox(AddExportAttachmentSelectors.SearchField, addExportAttachmentDetails.File, true);
    cy.get(AddExportAttachmentSelectors.SearchField).focus();
    cy.wait(2000);
    cy.get(AddExportAttachmentSelectors.FirstDeclaration).click();
    cy.get(BaseExportSelectors.CustomDocuments).click();
    
}

export function AddFSIE(addExportAttachmentDetails: AddExportAttachmentDetails) {
    cy.contains('button', 'שאילתא למסמכים').click();
    cy.wait(500);
    cy.get(AddExportAttachmentSelectors.RedX).click()
    cy.FillLogTextBox(AddExportAttachmentSelectors.CustomsDocId, addExportAttachmentDetails.CustomsDocId, true);
    cy.wait(1500);
    cy.Click(AddExportAttachmentSelectors.row0,null);
    cy.wait(1500);
    cy.Click(AddExportAttachmentSelectors.Approve, null);
    
}


export function Disconnect(addExportAttachmentDetails: AddExportAttachmentDetails) {
    cy.get('.ConnectedTicketItem iconbutton').invoke('removeAttr','class').invoke('removeProp','class');

}



    




