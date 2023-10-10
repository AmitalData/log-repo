import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as DeclarationFormActions from '../../actions/DeclarationFormActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { DeclarationFormDetails } from '../../models/DeclarationFormDetails';
import * as Actions from '../../actions/Actions';

//#region Filter for Form
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Search for File", (dataTable) => {
    let declarationFormDetails = Assists.CreateInstance<DeclarationFormDetails>(dataTable, true);
    DeclarationFormActions.FillSearchField(declarationFormDetails);
    DeclarationFormActions.InterToFile();
    
});


When("the user click on Forms > Declaration Form",() => {

      DeclarationFormActions.GetDeclarationForm()

    });


Then("the system should display the declaration that already exist",() => {

    DeclarationFormActions.DisplayDeclaration()

  });


//#endregion


    



                                                                                                                                                                                                                          

