import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as LocatFileSearchFieldAction from '../../actions/LocatFileSearchFieldAction';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { SearchFieldDetails } from '../../models/SearchFieldDetails';
import * as Actions from '../../actions/Actions';
var searchFieldDetails;


//#region Filter for file
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Search for File", (dataTable) => {
    
    searchFieldDetails = Assists.CreateInstance<SearchFieldDetails>(dataTable, true);
    LocatFileSearchFieldAction.FillSearchField(searchFieldDetails)
    
});                                                                                                                                                                                                                               

When("compare to filed Customs File",()  => {
    
    LocatFileSearchFieldAction.Compare(searchFieldDetails)
});


Then ("the comparison succeeded", () => {
     LocatFileSearchFieldAction.AssertSaveCompare
    });
//#endregion


