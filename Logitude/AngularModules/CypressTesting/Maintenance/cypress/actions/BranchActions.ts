import { BranchSelectors } from "../selectors/BranchSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { constants } from "../../../Base/cypress/constants/constants"
import { BranchDetails } from 'cypress/models/BranchDetails';
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let BranchName = null;
let inActiveBranch = false;

export function FillCheckBoxProcess(CheckBoxSelector: string, IsCheck: string) {
    if (IsCheck) {
        if (IsCheck.toUpperCase() == constants.YES) {
            cy.get(CheckBoxSelector).check({ force: true })
        }
        else {
            cy.get(CheckBoxSelector).find(BaseSelectors.input).uncheck({ force: true })
        }
    }
}

function GenerateRandomNumber(NumberLength: number) {
    let NewRandomCode = gr.GenerateRandomNumberAndString(NumberLength)
    return NewRandomCode;
}

export function FillBranchDetails(branchDetails: BranchDetails) {

    let RandomBranchName = GenerateRandomNumber(8);
    cy.FillLogTextBox(BranchSelectors.BranchName, branchDetails.Name.toLowerCase() == "random" ? RandomBranchName : branchDetails.Name)
    cy.FillLogTextBox(BranchSelectors.BranchLocalName, branchDetails.LocalName)
    if (branchDetails.Code.toLowerCase() == "random") {
        cy.FillRandomNumber(BranchSelectors.BranchCode, BranchSelectors.MinRandomNumber, BranchSelectors.MaxRandomNumber)
    }
    else {
        cy.FillLogTextBox(BranchSelectors.BranchCode, branchDetails.Code)
    }

    cy.FillLogTextBox(BranchSelectors.BranchSignature, branchDetails.Signature)
    if (branchDetails.CounterCode.toLowerCase() == "random") {
        cy.FillRandomNumber(BranchSelectors.BranchCounterCode, BranchSelectors.MinRandomNumber, BranchSelectors.MaxRandomNumber)

    }
    else {
        cy.FillLogTextBox(BranchSelectors.BranchCounterCode, branchDetails.Code)

    }
}

export function CreateBranch() {
    DefinePostBranchRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostBranchRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Branches, RequestAliases.PostBranch)
}

export function AssertCreateBranch() {
    let intercept = cy.wait("@" + RequestAliases.PostBranch);
    intercept.then((interception) => {
            AssertPostBranch(interception.response.statusCode, 200, interception.response.body.EnglishName)
        
    })
}

export function AssertPostBranch(responseStatusCode: number, expectedStatusCode: number, branchName: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
    BranchName = branchName
}

export function SearchBranch() {
    DefineBranchViewsGetByFiltersRequest(BranchName);
    cy.FillLogTextBox(BaseSelectors.SearchTextboxInput, BranchName);
    AssertBranchViewsGetByFilters();
}

export function DefineBranchViewsGetByFiltersRequest(BranchName: string) {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetFilterSearch(BranchName + "&GetCount=false"), RequestAliases.GetFilterSearch);
}
export function AssertBranchViewsGetByFilters() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetFilterSearch, 200);
}

export function AssertSearchBranch() {
    cy.get(BaseSelectors.RowClass).eq(0).invoke(BaseSelectors.TextElement).then((text) => {
        expect(text).to.contain(BranchName);
    });
}

export function OpenBranch() {
    DefineBranchesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineBranchesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BranchesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenBranch() {
    AssertBranchGetSingle();
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen)
}

function AssertBranchGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}

export function FillBranchLocalName(LocalName: string) {
    cy.FillLogTextBox(BranchSelectors.BranchLocalName, LocalName)
}

export function EditBranch() {
    DefinePutBranchRequest();
    cy.Click(BranchSelectors.BranchSaveButton, null);
}

function DefinePutBranchRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Branches, RequestAliases.PutBranch);
}

export function AssertEditBranch() {
    AssertPutBranch();
}

export function AssertPutBranch() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutBranch, 200).
        then((interception) => {
            inActiveBranch = interception.response.body.InActive;
        });
}

export function CloseSaveBranch() {
    DefineBranchViewGetSingleRequest()
    cy.Click(BranchSelectors.BranchSaveCloseButton, null);
}

function DefineBranchViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BranchesGetSingle, RequestAliases.GetSignle);
}
