import { BranchSelectors } from "../selectors/BranchSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BranchDetails } from 'cypress/models/BranchDetails';
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';
import * as GeneralActions from './BaseActions'
import * as Actions from './Actions'

let searchFieldValue = null;

export function FillBranchCode(code: string) {
    cy.FillLogTextBox(BranchSelectors.Code, code)
}

export function FillBranchCounterCode(counterCode: string) {
    cy.FillLogTextBox(BranchSelectors.CounterCode, counterCode)
}

export function FillBranchDetails(branchDetails: BranchDetails) {
    cy.FillLogTextBox(BranchSelectors.Name, gr.GenerateCurrentDatetimeString("_"))
    cy.FillLogTextBox(BranchSelectors.LocalName, branchDetails.LocalName)
    cy.FillLogTextBox(BranchSelectors.Code, gr.GenerateRandomNumberAndString(10))
    cy.FillLogTextBox(BranchSelectors.Signature, branchDetails.Signature)
    cy.FillLogTextBox(BranchSelectors.CounterCode, gr.GenerateRandomNumberAndString(5))
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
        let statusCode = interception.response.statusCode;
        assert.equal(statusCode, 200)
        searchFieldValue = interception.response.body.EnglishName
    })
}

export function SearchBranch() {
    GeneralActions.Search(searchFieldValue)
}

export function AssertSearchBranch() {
    GeneralActions.AssertSearch(searchFieldValue)
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

export function CreateAddress() {
    DefinePostAddressRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePostAddressRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.BrancheAddress, RequestAliases.PostBranchAddress)
}

export function AssertCreateAddress() {
    let intercept = cy.wait("@" + RequestAliases.PostBranchAddress);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        assert.equal(statusCode, 200)
    })
}

export function UpdateAddress() {
    DefinePutAddressRequest()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

function DefinePutAddressRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.BrancheAddress, RequestAliases.PutBranchAddress)
}

export function AssertUpdateAddress() {
    let intercept = cy.wait("@" + RequestAliases.PutBranchAddress);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        assert.equal(statusCode, 200)
    })
}

export function FillAccountingExternalID() {
    cy.FillLogTextBox(BranchSelectors.AccountingExternalID, gr.GenerateRandomNumberAndString(7))
}

export function CheckInactiveBox() {
    Actions.FillCheckBoxProcess(BranchSelectors.InActiveBranchCheckBox + BaseSelectors.LastElement, "Yes")
}

export function EditBranch() {
    DefinePutBranchRequest();
    cy.Click(BranchSelectors.SaveButton, null);
}

function DefinePutBranchRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Branches, RequestAliases.PutBranch);
}

export function AssertEditBranch() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutBranch, 200)
}

export function CloseSaveBranch() {
    DefineBranchViewGetSingleRequest()
    cy.Click(BranchSelectors.SaveCloseButton, null);
}

function DefineBranchViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BranchesviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveBranch() {
    AssertBranchGetSingle();
}