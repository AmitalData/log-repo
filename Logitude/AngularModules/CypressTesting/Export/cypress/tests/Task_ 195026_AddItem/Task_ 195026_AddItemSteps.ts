import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as AddItemActions from '../../actions/AddItemActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { AddItemDetails } from '../../models/AddItemDetails';
import * as Actions from '../../actions/Actions';
import { AddItemSelectors } from '../../selectors/AddItemSelectors';

//#region Create new Export Invoice
Given("the user logged in and navigates to Export workspace", () => {
    cy.Login();
    Actions.NavigatesExportsWizerd();
});

Given("Search for file and enter to ExporterInvoices", (dataTable) => {
  
    let addItemDetails = Assists.CreateInstance<AddItemDetails>(dataTable, true);
    AddItemActions.FillSearchField(addItemDetails);
    AddItemActions.CreateNewItem(addItemDetails);
   

});

Given("Fill the New Item with the following details", (dataTable) => {
    
let addItemDetails = Assists.CreateInstance<AddItemDetails>(dataTable, true);
AddItemActions.FillItem(addItemDetails);
AddItemActions.FillProcessTypes(addItemDetails);
});


When("saveing the Invoice", () => {
 cy.Click('#SaveSupplierInvoice.RedButton', null)
 
});


Then("the Invoice should save successfully", () => {
    
 AddItemActions.AssertSaveSupplierInvoice();
   
});

Given("the user delete the Item",() => {
    cy.get(AddItemSelectors.DeleteButton).click();
    
});

// When("deleting the Item", () => {

//     cy.get(AddItemSelectors.DeleteButton).click();
            
//     });


// Then("there is no Item in the grid", () => {
//    AddItemActions.AssertDeleteRow()
//    });   


// //#endregion








