import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as LocactSecondCargoIDSearchFieldAction from '../../actions/LocactSecondCargoIDSearchFieldAction';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { SearchFieldDetails } from '../../models/SearchFieldDetails';
import * as Actions from '../../actions/Actions';
var searchFieldDetails;

//#region Filter for Second Cargo ID
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Search for Second Cargo ID", (dataTable) => {
    
    searchFieldDetails = Assists.CreateInstance<SearchFieldDetails>(dataTable, true);
    LocactSecondCargoIDSearchFieldAction.FillSearchField(searchFieldDetails)
    
});                                                                                                                                                                                                                               

When("compare to filed Second Cargo ID",()  => {
    
    LocactSecondCargoIDSearchFieldAction.Compare(searchFieldDetails)
});


Then ("the comparison succeeded", () => {
    LocactSecondCargoIDSearchFieldAction.AssertSaveCompare
    });



//#endregion



                                                                                                                                                                                                                          

