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


export function FillSearchField(isChangedDetails: IsChangedDetails) {
    cy.FillLogTextBox(IsChangedSelectors.SearchField, isChangedDetails.File, true);
    cy.Click(IsChangedSelectors.IsChangedDeclaration1, null);

}


//תבוצע פניית API שתאפס את השדה IsChanged


export function ChangeInCargoSerialData(isChangedDetails: IsChangedDetails)
{
  FillInRowTable('כמות',isChangedDetails.Quantity);
  //cy.get(IsChangedSelectors.IsChangedquantity).clear();
}


export function FillInRowTable(headerText: string, value: string) {
    cy.get(`.ag-header-cell div:contains("${headerText}")`).each(ele => {
        if(ele.text() != headerText)return;
        const id = ele.attr('id')  
            if (id?.indexOf('HeaderTemplateDiv') > -1) {
                let i = id.replace('HeaderTemplateDiv', '');
                FillGLAccount('[index="' + i + '"]', value,);
            }
    })
}

export function FillGLAccount(selector, value) {
    cy.get(selector).type(value);
    
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



