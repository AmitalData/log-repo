import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as LocatDecSearchFieldAction from '../../actions/LocatDecSearchFieldAction';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { SearchFieldDetails } from '../../models/SearchFieldDetails';
import * as Actions from '../../actions/Actions';
var searchFieldDetails;
//#region Filter for Declaration
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Search for Declaration", (dataTable) => {
    
    searchFieldDetails = Assists.CreateInstance<SearchFieldDetails>(dataTable, true);
    LocatDecSearchFieldAction.FillSearchField(searchFieldDetails)
   // cy.wait(3000)
    
});                                                                                                                                                                                                                               

When("compare to filed Declaration",()  => {
    
    LocatDecSearchFieldAction.Compare(searchFieldDetails)
});


Then ("the comparison succeeded", () => {
    LocatDecSearchFieldAction.AssertSaveCompare
    });
//#endregion


