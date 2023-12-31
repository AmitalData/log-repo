import { CreateNewImportInvoiceSelectors } from "../selectors/CreateNewImportInvoiceSelectors";
import { CreateNewImportInvoiceDetails } from "cypress/models/CreateNewImportInvoiceDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseImportSelectors } from "../selectors/BaseImportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesExportWizerd() {
    cy.get(CreateNewImportInvoiceSelectors.AddButtonInvoice).click();
}


export function FillSearchField(createNewImportInvoiceDetails: CreateNewImportInvoiceDetails) {
    cy.FillLogTextBox(CreateNewImportInvoiceSelectors.SearchField, createNewImportInvoiceDetails.File, true);
    cy.get(CreateNewImportInvoiceSelectors.SearchField).focus();
    cy.wait(5000);
    cy.get(CreateNewImportInvoiceSelectors.FirstDeclaration).click();
    cy.get(CreateNewImportInvoiceSelectors.ImporterInvoices).click();
    
}

export function CreateaAndFillNewExporterInvoice(createNewImportInvoiceDetails: CreateNewImportInvoiceDetails) {
    
   cy.get(CreateNewImportInvoiceSelectors.AddButtonInvoice).click();
   cy.FillLogLov(CreateNewImportInvoiceSelectors.AccountTypeCode,createNewImportInvoiceDetails.AccountTypeCode, true);
   cy.FillLogLov(CreateNewImportInvoiceSelectors.InvoiceCurrencyTypeCode,createNewImportInvoiceDetails.InvoiceCurrencyTypeCode, true);
   cy.FillLogLov(CreateNewImportInvoiceSelectors.IncotermCode,createNewImportInvoiceDetails.IncotermCode, true);
   cy.FillLogTextBox(CreateNewImportInvoiceSelectors.InvoiceNumber,createNewImportInvoiceDetails.InvoiceNumber,true);
   cy.get(CreateNewImportInvoiceSelectors.InvoiceAmount).type(createNewImportInvoiceDetails.InvoiceAmount);
   //cy.FillLogTextBox(CreateNewImportInvoiceSelectors.IsPreferencer,createNewImportInvoiceDetails.CheckBox,true);
   //cy.get('td:nth-child(5) .CheckBox > label').click();
   cy.get(CreateNewImportInvoiceSelectors.IsPreferencer).click({force: true});
   cy.FillLogLov(CreateNewImportInvoiceSelectors.PreferenceDocumentTypeCode,createNewImportInvoiceDetails.PreferenceDocumentTypeCode,true);
   cy.FillLogLov(CreateNewImportInvoiceSelectors.VendorId,createNewImportInvoiceDetails.VendorId,true);
   //cy.FillLogLov(CreateNewImportInvoiceSelectors.FreightAmountCurrencyTypeCode,createNewImportInvoiceDetails.FreightAmountCurrencyTypeCode,true);
   //cy.FillLogLov(CreateNewImportInvoiceSelectors.FreightAmount,createNewImportInvoiceDetails.FreightAmount,true);

 }
    


 export function Matching(createNewImportInvoiceDetails: CreateNewImportInvoiceDetails)
 {
  FillMatchingDDL(CreateNewImportInvoiceSelectors.FreightAmountCurrencyTypeCode, createNewImportInvoiceDetails.FreightAmountCurrencyTypeCode);  
  FillMatching(CreateNewImportInvoiceSelectors.FreightAmount, createNewImportInvoiceDetails.FreightAmount);
  cy.get(CreateNewImportInvoiceSelectors.ButtonSaveSupplierInvoice).focus();
   
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

export function CreateNewItem(createNewImportInvoiceDetails: CreateNewImportInvoiceDetails) {
    //cy.get(CreateNewImportInvoiceSelectors.EditButtonInvoice).click();
    cy.wait(5000);
    cy.get(CreateNewImportInvoiceSelectors.AddItemButton).click();
    
  }


export function FillItem(createNewImportInvoiceDetails: CreateNewImportInvoiceDetails) {
    cy.get(CreateNewImportInvoiceSelectors.ItemNo).type(createNewImportInvoiceDetails.ItemNo);
    cy.get(CreateNewImportInvoiceSelectors.ItemDescription).type(createNewImportInvoiceDetails.ItemDescription);
    cy.get(CreateNewImportInvoiceSelectors.Item).type(createNewImportInvoiceDetails.Item);
    FillMatchingDDL(CreateNewImportInvoiceSelectors.TradeAgreementCode, createNewImportInvoiceDetails.TradeAgreementCode);  
    FillMatching(CreateNewImportInvoiceSelectors.ProtocolCode, createNewImportInvoiceDetails.ProtocolCode); 
    //cy.get(CreateNewImportInvoiceSelectors.UnitsQuantity).type(createNewImportInvoiceDetails.UnitsQuantity);
    //FillMatchingDDL(CreateNewImportInvoiceSelectors.UnitType, createNewImportInvoiceDetails.UnitType); 
    cy.get(CreateNewImportInvoiceSelectors.ValueInForeignCurrency).type(createNewImportInvoiceDetails.ValueInForeignCurrency);
    cy.get(CreateNewImportInvoiceSelectors.ValueInForeignCurrency).type(createNewImportInvoiceDetails.ValueInForeignCurrency);
    FillMatchingDDL(CreateNewImportInvoiceSelectors.OriginCountryCode, createNewImportInvoiceDetails.OriginCountryCode); 
    cy.get(CreateNewImportInvoiceSelectors.ValueInForeignCurrency).type(createNewImportInvoiceDetails.ValueInForeignCurrency);
   // FillMatchingDDL(AddItemSelectors.OriginCountry, addItemDetails.OriginCountry); 
    cy.wait(5000);

 }


export function AssertSaveSupplierInvoice() {
    BaseAssertion.AssertElementExist(CreateNewImportInvoiceSelectors.SupplierInvoiceFirstRow);
    
}



export function DeleteRow(){
    cy.Click(CreateNewImportInvoiceSelectors.ButtonDeleteSupplierInvoice,null);
    cy.Click(CreateNewImportInvoiceSelectors.Yes,null);

    
   }

   export function AssertDeleteRow() {
       BaseAssertion.AssertElementNotExist(CreateNewImportInvoiceSelectors.SupplierInvoiceFirstRow);
       
   }


