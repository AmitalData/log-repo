import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CreateNewExportInvoiceActions from '../../actions/CreateNewExportInvoiceActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CreateNewExportInvoiceDetails } from '../../models/CreateNewExportInvoiceDetails';
import * as Actions from '../../actions/Actions';
import { CreateNewExportInvoiceSelectors } from '../../selectors/CreateNewExportInvoiceSelectors';

//#region Create new Import Invoice
Given("the user logged in and navigates to Export workspace", () => {
    cy.Login();
    Actions.NavigatesExportsWizerd();
});

Given("Search for file and enter to ExporterInvoices", (dataTable) => {
  
    let createNewExportInvoiceDetails = Assists.CreateInstance<CreateNewExportInvoiceDetails>(dataTable, true);
    CreateNewExportInvoiceActions.FillSearchField(createNewExportInvoiceDetails);
  
});

Given("Fill Exporter Invoice with the following details", (dataTable) => {
    
let creatNewExportInvoiceDetails = Assists.CreateInstance<CreateNewExportInvoiceDetails>(dataTable, true);
CreateNewExportInvoiceActions.CreateaAndFillNewExporterInvoice(creatNewExportInvoiceDetails);
CreateNewExportInvoiceActions.Matching(creatNewExportInvoiceDetails);
});


When("saveing the Invoice", () => {
cy.Click(CreateNewExportInvoiceSelectors.ButtonSaveSupplierInvoice, null);
        
});


Then("the Invoice should save successfully", () => {
    
    
    CreateNewExportInvoiceActions.AssertSaveSupplierInvoice();
   
 });

 Given("the user delete the row", () => { 
    CreateNewExportInvoiceActions.DeleteRow()

    });

    Then("there is no row in the grid", () => {
        CreateNewExportInvoiceActions.AssertDeleteRow()
    });   




//#endregion







