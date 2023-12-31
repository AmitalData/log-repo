import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CreateNewImportInvoiceActions from '../../actions/CreateNewImportInvoiceActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CreateNewImportInvoiceDetails } from '../../models/CreateNewImportInvoiceDetails';
import * as Actions from '../../actions/Actions';
import { CreateNewImportInvoiceSelectors } from '../../selectors/CreateNewImportInvoiceSelectors';

//#region Create new Export Invoice
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace();
});

Given("Search for file and enter to Importer Invoices", (dataTable) => {
  
    let createNewImportInvoiceDetails = Assists.CreateInstance<CreateNewImportInvoiceDetails>(dataTable, true);
    CreateNewImportInvoiceActions.FillSearchField(createNewImportInvoiceDetails);
  
});

Given("Fill Importer Invoice with the following details", (dataTable) => {
    
let createNewImportInvoiceDetails = Assists.CreateInstance<CreateNewImportInvoiceDetails>(dataTable, true);
CreateNewImportInvoiceActions.CreateaAndFillNewExporterInvoice(createNewImportInvoiceDetails);
CreateNewImportInvoiceActions.Matching(createNewImportInvoiceDetails);
});


Given("Fill the New Item with the following details", (dataTable) => {
    
    let createNewImportInvoiceDetails = Assists.CreateInstance<CreateNewImportInvoiceDetails>(dataTable, true);
    CreateNewImportInvoiceActions.CreateNewItem(createNewImportInvoiceDetails);
    CreateNewImportInvoiceActions.FillItem(createNewImportInvoiceDetails);
    });
    


When("saveing the Invoice", () => {
cy.Click(CreateNewImportInvoiceSelectors.ButtonSaveSupplierInvoice, null);
        
});


Then("the Invoice should save successfully", () => {
    
    
    CreateNewImportInvoiceActions.AssertSaveSupplierInvoice();
   
 });

 Given("the user delete the row", () => { 
    CreateNewImportInvoiceActions.DeleteRow()

    });

    Then("there is no row in the grid", () => {
        CreateNewImportInvoiceActions.AssertDeleteRow()
    });   




//#endregion







