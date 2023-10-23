import { IsChangedDetails } from "cypress/models/IsChangedDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { IsChangedSelectors } from "../selectors/IsChangedSelectors";

export function NavigatesImportDeclarationWorkspace() 
{ 
    cy.Click(IsChangedSelectors.GeneralMHDeclarationsTab, null)
    
}


export function FillIsChanged(isChangedDetails: IsChangedDetails)
{
    
    cy.Click(IsChangedSelectors.IsChangedFilterOpen,null,true)
    cy.FillLogLov(IsChangedSelectors.IsChangedDeclarationStatus,isChangedDetails.DeclarationStatus,true);
    cy.get(IsChangedSelectors.SearchField).focus();
    cy.Click(IsChangedSelectors.IsChangedFilterClose,null,true)
    cy.wait(2000);
    cy.Click(IsChangedSelectors.IsChangedDeclaration1, null)

}

//תבוצע פניית API שתאפס את השדה IsChanged




debugger
export function ChangeInCargoSerialData(isChangedDetails: IsChangedDetails)
{

  //cy.get('IsChangedquantity').clear();

  cy.get(IsChangedSelectors.IsChangedquantity).clear();

  //cy.FillLogLov(IsChangedSelectors.IsChangedquantity,isChangedDetails["Quantity"],true)

}


export function SaveDeclaretion()
{
    cy.DefineRequestWait(RestAPI.PUT, URLs.IsChanged, RequestAliases.IsChanged)
    cy.Click(IsChangedSelectors.IsChangedSaveButton, null);
}

export function AssertSaveDeclaretion()
{
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
    BaseAssertion.AssertStatusCode(RequestAliases.IsChanged, 200)
}


export function SendDeclarationToPayment()
{
    cy.DefineRequestWait(RestAPI.GET, URLs.Pay, RequestAliases.IsChanged)
    cy.Click(IsChangedSelectors.IsChangedDeclarationPaymentButton, null);
}

 export function AssertSendDeclarationToPayment(condition)
 {;
    // BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);     
     BaseAssertion.AssertElementDisabled(IsChangedSelectors.IsChangedSendButtonD,condition);
     BaseAssertion.AssertElementExist(IsChangedSelectors.DisbleBox);
     //BaseAssertion.AssertStatusCode(RequestAliases.IsChanged, 200)
 }



