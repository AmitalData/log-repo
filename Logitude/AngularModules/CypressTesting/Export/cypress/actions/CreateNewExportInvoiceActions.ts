import { CreateNewExportInvoiceSelectors } from "../selectors/CreateNewExportInvoiceSelectors";
import { CreateNewExportInvoiceDetails } from "cypress/models/CreateNewExportInvoiceDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesExportWizerd() {
    cy.get(CreateNewExportInvoiceSelectors.AddButtonInvoice).click();
}


export function FillSearchField(creatNewExportInvoiceDetails: CreateNewExportInvoiceDetails) {
    cy.FillLogTextBox(CreateNewExportInvoiceSelectors.SearchField, creatNewExportInvoiceDetails.File, true);
    cy.get(CreateNewExportInvoiceSelectors.SearchField).focus();
    cy.wait(2000);
    
    // Wait for grid to load and find first declaration row
    cy.get(CreateNewExportInvoiceSelectors.FirstDeclaration, { timeout: 30000 })
        .should('be.visible')
        .click();
    
    cy.get(BaseExportSelectors.ExporterInvoices).click();
    
}

export function CreateaAndFillNewExporterInvoice(createNewExportInvoiceDetails: CreateNewExportInvoiceDetails) {
    
   // Wait for Add button to be visible and clickable
   cy.get(CreateNewExportInvoiceSelectors.AddButtonInvoice, { timeout: 30000 })
       .should('be.visible')
       .click();
   cy.FillLogLov(CreateNewExportInvoiceSelectors.AccountTypeCode,createNewExportInvoiceDetails.AccountTypeCode, true);
   cy.FillLogLov(CreateNewExportInvoiceSelectors.InvoiceCurrencyTypeCode,createNewExportInvoiceDetails.InvoiceCurrencyTypeCode, true);
   cy.FillLogLov(CreateNewExportInvoiceSelectors.IncotermCode,createNewExportInvoiceDetails.IncotermCode, true);
   cy.get(CreateNewExportInvoiceSelectors.BuyerName).type(createNewExportInvoiceDetails.BuyerName);
   cy.get(CreateNewExportInvoiceSelectors.BuyerAddress).type(createNewExportInvoiceDetails.BuyerAddress);
   cy.get(CreateNewExportInvoiceSelectors.PartyRelationshipCode).type(createNewExportInvoiceDetails.PartyRelationshipCode);
  // cy.FillLogLov(CreateNewExportInvoiceSelectors.BuyerCountryCode,createNewExportInvoiceDetails.BuyerCountryCode,true);
   cy.FillLogLov(CreateNewExportInvoiceSelectors.BuyerRoleCode,createNewExportInvoiceDetails.BuyerRoleCode,true);
   cy.FillLogTextBox(CreateNewExportInvoiceSelectors.InvoiceNumber,createNewExportInvoiceDetails.InvoiceNumber,true);
   cy.get(CreateNewExportInvoiceSelectors.InvoiceAmount).type(createNewExportInvoiceDetails.InvoiceAmount);
   //cy.FillLogTextBox(CreateNewExportInvoiceSelectors.IsPreferencer,createNewExportInvoiceDetails.CheckBox,true);
   //cy.get('td:nth-child(5) .CheckBox > label').click();
   //cy.get(CreateNewExportInvoiceSelectors.CheckBox).click({force:true});
   cy.FillLogLov(CreateNewExportInvoiceSelectors.DutyRegimeProtocolCode,createNewExportInvoiceDetails.DutyRegimeProtocolCode,true);
  // cy.FillLogLov("#Customs.SupplierInvoiceModification_CurrencyTypeCode",createNewExportInvoiceDetails.BuyerRoleCode,true);

 }
    

 export function Matching(createNewExportInvoiceDetails: CreateNewExportInvoiceDetails)
 {
//   FillMatchingDDL(CreateNewExportInvoiceSelectors.ExportModificationCurrency, createNewExportInvoiceDetails.ExportModificationCurrency);  
//   FillMatchingDDL(CreateNewExportInvoiceSelectors.PreferenceDocumentTypeCode, createNewExportInvoiceDetails.PreferenceDocumentTypeCode);
//   FillMatchingDDL('#edit-log-grid_0_30_1_0', createNewExportInvoiceDetails.InsuranceCurrencyTypeCode);
//   FillMatching('#edit-log-grid_0_30_2_0', createNewExportInvoiceDetails.InsuranceAmount);
//   FillMatchingDDL('#edit-log-grid_0_30_1_1', createNewExportInvoiceDetails.TransportCurrencyTypeCode);
//   FillMatching('#edit-log-grid_0_30_2_1', createNewExportInvoiceDetails.TransportAmount);
//   FillMatchingDDL('#edit-log-grid_0_30_1_2', createNewExportInvoiceDetails.ExpenseCurrencyTypeCode);
//   FillMatching('#edit-log-grid_0_30_2_2', createNewExportInvoiceDetails.ExpenseAmount);
  cy.get(CreateNewExportInvoiceSelectors.ButtonSaveSupplierInvoice).focus();
   
 }
 
 export function FillMatching(selector, value) {
     cy.get(selector).type(value);
     
 }

 export function FillMatchingDDL(selector, value) {
    cy.get(selector).type(value)
    cy.get(BaseSelectors.DropDownList).contains(value).then(a => {
        a[0].click();
    });
}

 
 export function SaveSupplierInvoice() {
    cy.DefineRequestWait(RestAPI.POST, URLs.SupplierInvoice, RequestAliases.SupplierInvoiceRequest)
    cy.Click(CreateNewExportInvoiceSelectors.ButtonSaveSupplierInvoice,null);
    
}


export function AssertSaveSupplierInvoice() {
    BaseAssertion.AssertElementExist(CreateNewExportInvoiceSelectors.SupplierInvoiceFirstRow);
    
}



export function DeleteRow(){
    // Wait for delete button to exist and force click (handles overflow issues)
    cy.get(CreateNewExportInvoiceSelectors.ButtonDeleteSupplierInvoice, { timeout: 30000 })
        .should('exist')
        .click({ force: true });
    
    // Wait for confirmation dialog and click Yes with shorter timeout
    cy.get(CreateNewExportInvoiceSelectors.Yes, { timeout: 5000 })
        .should('be.visible')
        .click();
    
   }

   export function AssertDeleteRow() {
       // Wait a bit for the delete operation to complete with shorter wait
       cy.wait(1000);
       
       // Check if the specific supplier invoice grid is empty with shorter timeout
       cy.get('declarationsupplierinvoicetabcomponent [id*="LogGrid"] [id*="row"]', { timeout: 5000 })
           .should('not.exist');
       
   }
