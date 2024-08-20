import { ChartOfAccountsSelectors } from "../selectors/ChartOfAccountsSelectors";
import { ChartOfAccountsDetails } from "cypress/models/ChartOfAccountsDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { GenerateRandomNumberAndString } from '../../../Base/cypress/actions/GenerateRandoms';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';

let searchFieldValue = null;

export function GetSearchFieldValue() {
    return searchFieldValue;
}
export function FillChartOfAccountDetails(chartOfAccountsDetails: ChartOfAccountsDetails, codeDigits: number) {
    cy.FillLogTextBox(ChartOfAccountsSelectors.Code, GenerateRandomNumberAndString(codeDigits));
    cy.FillLogTextBox(ChartOfAccountsSelectors.EnglishName, chartOfAccountsDetails.EnglishName);
    cy.FillLogTextBox(ChartOfAccountsSelectors.LocalName, chartOfAccountsDetails.LocalName);
    cy.FillLogLov(ChartOfAccountsSelectors.Type, chartOfAccountsDetails.Type, true);
}

export function CreateChartOfAccount() {
    cy.DefineRequestWait(RestAPI.POST, URLs.ChartOfAccounts, RequestAliases.PostChartOfAccount);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateChartOfAccount() {
    let intercept = cy.wait("@" + RequestAliases.PostChartOfAccount);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        if (statusCode === 400) {
            ReCreateChartOfAccount();
        }
        else {
            assert.equal(statusCode, 200)
            searchFieldValue = interception.response.body.Code;
        }
    })
}

function ReCreateChartOfAccount() {
    let RandomCode = GenerateRandomNumberAndString(5);
    cy.FillLogTextBox(ChartOfAccountsSelectors.Code, RandomCode)
    CreateChartOfAccount();
    AssertCreateChartOfAccount();
}

export function OpenChartOfAccount() {
    cy.DefineRequestWait(RestAPI.GET, URLs.ChartOfAccountsGetSingle, RequestAliases.GetSignle);
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

export function EditChartOfAccount() {
    cy.FillLogTextBox(ChartOfAccountsSelectors.EnglishName, "New English Name");
}

export function SaveChartOfAccount() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.ChartOfAccounts, RequestAliases.PutChartOfAccount);
    cy.Click(ChartOfAccountsSelectors.SaveButton, null)
}

export function AssertSaveChartOfAccount() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutChartOfAccount, 200);
}