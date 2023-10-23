import { DeclarationFormDetails } from "cypress/models/DeclarationFormDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { DeclarationFormSelectors } from "../selectors/DeclarationFormSelectors";

export function NavigatesImportDeclarationWorkspace() 
{ 
    cy.Click(DeclarationFormSelectors.GeneralMHDeclarationsTab, null)
    
}

export function FillSearchField(declarationFormDetails:DeclarationFormDetails) {
    cy.FillLogTextBox(DeclarationFormSelectors.SearchField, declarationFormDetails.File, true);
    
}

export function InterToFile() {

    cy.get(DeclarationFormSelectors.InterToFile).eq(0).click();
    cy.wait(1000);

}

export function GetDeclarationForm() {

   
    cy.get(DeclarationFormSelectors.Forms).click();

}

export function DisplayDeclaration() {

    cy.get(DeclarationFormSelectors.DeclarationForm).click({force:true});
    cy.wait(50);
   
}







