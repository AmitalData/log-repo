import { BranchSelectors } from "../selectors/BranchSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { constants } from "../../../Base/cypress/constants/constants"
import { BranchDetails } from 'cypress/models/BranchDetails';
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';
import * as GeneralActions from './GeneralActions'

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

export function FillBranchDetails(branchDetails: BranchDetails) {

    let CurrentDateName = GenerateCurrentDatetimeString("_")

    cy.FillLogTextBox(BranchSelectors.BranchName, CurrentDateName)
    cy.FillLogTextBox(BranchSelectors.BranchLocalName, branchDetails.LocalName)
    cy.FillRandomNumber(BranchSelectors.BranchCode, BranchSelectors.MinCodeRandomNumber, BranchSelectors.MaxCodeRandomNumber)
    cy.FillLogTextBox(BranchSelectors.BranchSignature, branchDetails.Signature)
    cy.FillRandomNumber(BranchSelectors.BranchCounterCode, BranchSelectors.MinCounterCodeRandomNumber, BranchSelectors.MaxCounterCodeRandomNumber)
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
    GeneralActions.Search(BranchName)
}

export function AssertSearchBranch() {
    GeneralActions.AssertSearch(BranchName)
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
    BaseAssertion.AssertStatusCode(RequestAliases.PutBranch, 200)
}

export function CloseSaveBranch() {
    DefineBranchViewGetSingleRequest()
    cy.Click(BranchSelectors.BranchSaveCloseButton, null);
}

export function AssertCloseSaveBranch() {
    AssertBranchGetSingle();
}

function DefineBranchViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.BranchesviewGetSingle, RequestAliases.GetSignle);
}
