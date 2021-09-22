import { CardSelectors } from "../selectors/CardSelectors";
import { CardDetails } from "cypress/models/CardDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import { Constants } from "../constants/Constants";

let CardCode = null;

export function getCardCode() {
    return CardCode;
}

export function NavigateCutomersWizerd() {
    cy.Click(CardSelectors.CustomersTab, null);
    cy.Click(CardSelectors.NewCustomerButton, null);
}

export function FillCardDetails(cardDetails: CardDetails) {
    let currentDateTime = gr.GenerateCurrentDatetimeString("")
    cy.FillLogTextBox(CardSelectors.CompanyName, currentDateTime)
    cy.FillLogTextBox(CardSelectors.Phone, cardDetails.Phone)
    cy.FillLogTextBox(CardSelectors.Address1, cardDetails.Address1)
    cy.FillLogTextBox(CardSelectors.City, cardDetails.City)
    cy.FillLogLov(CardSelectors.Country, cardDetails.Country, true)
}

export function CreateCard() {
    cy.DefineRequestWait(RestAPI.POST, URLs.PartnersDomain, RequestAliases.PostCard)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateCard(CardType: string) {
    BaseAssertion.AssertStatusCode(RequestAliases.PostCard, 200).then((interception) => {
        let responseBody = interception.response.body;
        if (CardType == Constants.Customer) {
            CardCode = responseBody.Address.Name;
        }
        else if (CardType == Constants.Vendor) {
            CardCode = responseBody.Vendor.Code;
        }
    });
}

export function OpenCard(CardType: string) {
    if (CardType == Constants.Customer) {
        cy.DefineRequestWait(RestAPI.GET, URLs.CustomerGetSingle, RequestAliases.GetSignle);
    }
    else if (CardType == Constants.Vendor) {
        cy.DefineRequestWait(RestAPI.GET, URLs.VendorsGetSingle, RequestAliases.GetSignle);
    }
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function NavigateGLAccountWizerdFromCustomer() {
    cy.Click(CardSelectors.CustomerAccountingTab, null)
    cy.Click(CardSelectors.ActivateHyperLink, null)
}

export function NavigateGLAccountWizerdFromVendor() {
    cy.Click(CardSelectors.VendorAccountingTab, null)
    cy.Click(CardSelectors.ActivateHyperLink, null)
}