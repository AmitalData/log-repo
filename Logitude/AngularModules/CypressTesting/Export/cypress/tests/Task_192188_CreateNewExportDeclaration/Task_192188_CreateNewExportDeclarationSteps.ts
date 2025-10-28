import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CreateNewExportDeclarationActions from '../../actions/CreateNewExportDeclarationActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CreateNewExportDeclarationDetails } from '../../models/CreateNewExportDeclarationDetails';
import { CreateNewExportDeclarationSelectors } from '../../selectors/CreateNewExportDeclarationSelectors';

var createNewExportDeclarationDetails

//#region Create new Import Declaration
Given("the user logged in and navigates to Export workspace", () => {
    cy.Login();
    Actions.NavigatesExportsWizerd()
     });

Given("open new declaration", (dataTable) => {
    
    let createNewExportDeclarationDetails = Assists.CreateInstance<CreateNewExportDeclarationDetails>(dataTable, true);
    CreateNewExportDeclarationActions.CreateNewExportDeclaration(createNewExportDeclarationDetails)
});

When("approve the new declaration", () => {
    cy.Click(CreateNewExportDeclarationSelectors.OkButton, null)
        
});

Then("the declaration should save successfully", () => {
   
    CreateNewExportDeclarationActions.Compare(createNewExportDeclarationDetails)

});

Given("Declaration with the following details", (dataTable) => {
   
    let createNewExportDeclarationDetails = Assists.CreateInstance<CreateNewExportDeclarationDetails>(dataTable, true);
    CreateNewExportDeclarationActions.FillNewExportDeclaration(createNewExportDeclarationDetails)
    
});


When("save Declaration", () => {
   CreateNewExportDeclarationActions.SaveDeclaretion()
        
});

Then("the Declaration save successfully", () => {
    
    
CreateNewExportDeclarationActions.AssertSaveDeclaretion()
});

//#endregion








