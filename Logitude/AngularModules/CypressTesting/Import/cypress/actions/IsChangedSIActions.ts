import { IsChangedSIDetails } from "cypress/models/IsChangedSIDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { IsChangedSISelectors } from "../selectors/IsChangedSISelectors";

export function NavigatesImportDeclarationWorkspace() 
{ 
    cy.Click(IsChangedSISelectors.GeneralMHDeclarationsTab, null)
    
}

export function FillIsChanged(isChangedSIDetails: IsChangedSIDetails)
{
    cy.Click(IsChangedSISelectors.IsChangedFilterOpen,null,true)
    cy.FillLogLov(IsChangedSISelectors.IsChangedDeclarationStatus,isChangedSIDetails.DeclarationStatus,true);
    cy.get(IsChangedSISelectors.SearchField).focus();
    cy.Click(IsChangedSISelectors.IsChangedFilterClose,null,true)
    cy.wait(2000);
    cy.Click(IsChangedSISelectors.IsChangedDeclaration1, null)
    cy.Click(IsChangedSISelectors.IsChangedGeneral, null)
    //cy.Click(IsChangedSISelectors.IsChangeSITHInvoices, null)
    //cy.Click(IsChangedSISelectors.Edit, null)
   

}
//לא היה צורך בפונקציה פנימית, כיוון שלא היה עוד כפתור של שמור, כי המסך בו נעשה השינוי נפתח אחרי - מירקרנו את השורות מעלה והעברנו אותם לפונקציה של השינוי
//תבוצע פניית API שתאפס את השדה IsChanged
// export function SendToCustomsButtonSimulator() {
  
//     cy.get('.x-button-drop').click();
//     cy.get('li:nth-child(5) > span').click();
    
// }

// export function SendToCustomsButtonSimulator1(isChangedSIDetails:IsChangedSIDetails)  {

//         cy.get('senddeclarationtastcasecomponent combobox .ComboBox table td img').type(isChangedSIDetails.Scen1);
//         cy.get('senddeclarationtastcasecomponent combobox .ComboBox .ComboBoxDropdown  div ul li:eq(0)').click({force: true});
//         cy.Click(IsChangedSISelectors.Save,null, true);
//         cy.wait(1000)
       


// let errorMessage = '';
// do {
//     cy.Click(IsChangedSISelectors.Save,null, true);
//   if (errorMessage) {
  
//   }
// } while (errorMessage);

// }



export function ChangeInTotalSI(isChangedSIDetails:IsChangedSIDetails) 
{
    cy.Click(IsChangedSISelectors.IsChangeSITHInvoices, null)
    cy.Click(IsChangedSISelectors.Edit, null)
    cy.FillLogTextBox(IsChangedSISelectors.IsChangedSISupplierInvoice, isChangedSIDetails.TotalSI,true);
    cy.Click(IsChangedSISelectors.SaveSupplierInvoic, null)
    
}

export function SaveSI()
{
cy.DefineRequestWait(RestAPI.PUT, URLs.supplierinvoices, RequestAliases.Savesupplierinvoice);
cy.Click(IsChangedSISelectors.SaveSupplierInvoic, null)
}


export function SendDeclarationToPayment()
{
cy.DefineRequestWait(RestAPI.GET, URLs.Pay, RequestAliases.IsChanged)
cy.Click(IsChangedSISelectors.IsChangedDeclarationPaymentButton, null);
}


export function AssertSendDeclarationToPayment(condition)
{
     
BaseAssertion.AssertElementDisabled(IsChangedSISelectors.IsChangedSendButtonD,condition);
BaseAssertion.AssertElementExist(IsChangedSISelectors.DisbleBox);
     
}



