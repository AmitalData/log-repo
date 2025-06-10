import { AddItemSelectors } from "../selectors/AddItemSelectors";
import { AddItemDetails } from "cypress/models/AddItemDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'

export function NavigatesExportWizerd() {
    cy.get(AddItemSelectors.AddButtonInvoice).click();
}


export function FillSearchField(addItemDetails: AddItemDetails) {
    cy.FillLogTextBox(AddItemSelectors.SearchField, addItemDetails.File, true);
    cy.get(AddItemSelectors.SearchField).focus();
    cy.wait(5000);
    cy.get(AddItemSelectors.FirstDeclaration).click();
    cy.get(BaseExportSelectors.ExporterInvoices).click();
 
}

export function CreateNewItem(addItemDetails: AddItemDetails) {
   cy.get(AddItemSelectors.EditButtonInvoice).click();
   cy.wait(10000);
   cy.get(AddItemSelectors.AddItemButton).click();
   
 }

 export function FillItem(addItemDetails: AddItemDetails) {
    
    cy.get(AddItemSelectors.ItemNo).type(addItemDetails.ItemNo);
     cy.get(AddItemSelectors.ItemDescription).type(addItemDetails.ItemDescription);
    // cy.get(AddItemSelectors.Item).type(addItemDetails.Item);
     FillMatchingDDL('div[index="6"]', addItemDetails.TradeAgreementCode);  
    FillMatchingDDL(AddItemSelectors.ProtocolCode, addItemDetails.ProtocolCode); 
    cy.get(AddItemSelectors.UnitsQuantity).type(addItemDetails.UnitsQuantity);
    FillMatchingDDL(AddItemSelectors.UnitType, addItemDetails.UnitType); 
    cy.get(AddItemSelectors.ValueInForeignCurrency).type(addItemDetails.ValueInForeignCurrency);
   // FillMatchingDDL(AddItemSelectors.OriginCountry, addItemDetails.OriginCountry); 
    cy.get('img.FlipImgHoriz:eq(42)').click();
    cy.get('#Add_6').click();
    cy.wait(5000);

 }

 export function FillProcessTypes(addItemDetails: AddItemDetails) {
   
        
        FillMatchingDDL('.LogCellTemplate:eq(45)', addItemDetails.ProcessTypeCode);
        cy.get('button.RedButton:eq(1)').focus();
        cy.contains('.RedButton', 'אישור').click();
        cy.wait(5000);
        cy.Click('#SaveSupplierInvoice.RedButton', null)
        cy.wait(5000);
        cy.get(AddItemSelectors.EditButtonInvoice).click();
        cy.get(AddItemSelectors.DeleteButton).click();

   }

   export function DeleteItem(addItemDetails: AddItemDetails) {

    
    cy.get(AddItemSelectors.EditButtonInvoice).click();
    cy.wait(5000);
    cy.get(AddItemSelectors.DeleteButton).click();
    
  }


    export function AssertDeleteRow() {
     BaseAssertion.AssertElementNotExist(AddItemSelectors.SupplierInvoiceFirstRow);
          
    } 
 

  export function SaveSupplierInvoice() {
    cy.DefineRequestWait(RestAPI.POST, URLs.SupplierInvoice, RequestAliases.SupplierInvoiceRequest)
    cy.Click(AddItemSelectors.ButtonSaveSupplierInvoice,null);
    
}


export function AssertSaveSupplierInvoice() {
    BaseAssertion.AssertElementExist(AddItemSelectors.SupplierInvoiceFirstRow);
    
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

 
 
