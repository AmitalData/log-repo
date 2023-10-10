import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as LocatCustSearchFieldAction from '../../actions/LocatCustSearchFieldAction';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { SearchFieldDetails } from '../../models/SearchFieldDetails';
import * as Actions from '../../actions/Actions';
var searchFieldDetails;

//#region Filter for Customer
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Search for Customer", (dataTable) => {
    
    searchFieldDetails = Assists.CreateInstance<SearchFieldDetails>(dataTable, true);
    LocatCustSearchFieldAction.FillSearchField(searchFieldDetails)
    
});                                                                                                                                                                                                                               

When("compare to filed Customer",()  => {
    
    LocatCustSearchFieldAction.Compare(searchFieldDetails)
});


Then ("the comparison succeeded", () => {
    LocatCustSearchFieldAction.AssertSaveCompare
    });



//#endregion



                                                                                                                                                                                                                          

