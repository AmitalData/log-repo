import { AddImportAttachmentSelectors } from "../selectors/AddImportAttachmentSelectors";
import { AddImportAttachmentDetails } from "cypress/models/AddImportAttachmentDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseImportSelectors } from "../selectors/BaseImportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';


export function NavigatesImporttWizerd() {
    cy.Click(BaseImportSelectors.CustomDocuments, null);
}



export function FillSearchField(addImportAttachmentDetails: AddImportAttachmentDetails) {
    cy.FillLogTextBox(AddImportAttachmentSelectors.SearchField, addImportAttachmentDetails.File, true);
    cy.get(AddImportAttachmentSelectors.SearchField).focus();
    cy.wait(5000);
    cy.get(AddImportAttachmentSelectors.FirstDeclaration).click();
    cy.get(BaseImportSelectors.CustomDocuments).click();
    
}

export function AddFSIE(addImportAttachmentDetails: AddImportAttachmentDetails) {
    cy.contains('button', 'שאילתא למסמכים').click();
    cy.wait(3000);
    cy.Click(AddImportAttachmentSelectors.row0,null);
    cy.wait(3000);
    cy.Click(AddImportAttachmentSelectors.Approve, null);
    
}


export function Disconnect(addImportAttachmentDetails: AddImportAttachmentDetails) {
    cy.get('.ConnectedTicketItem iconbutton').invoke('removeAttr','class').invoke('removeProp','class');

}



    




