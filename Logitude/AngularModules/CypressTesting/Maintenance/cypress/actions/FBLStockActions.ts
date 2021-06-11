import { FBLStockSelectors } from "../selectors/FBLStockSelectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { constants } from "../../../Base/cypress/constants/constants"
import { FBLStockDetails } from 'cypress/models/FBLStockDetails';
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

let byAmount = null
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

function GenerateFourDigitkRandomNumber(MinNumber, MaxNumber) {
    let NewRandomCode = gr.GenerateRandomNumber(MinNumber, MaxNumber)
    return NewRandomCode;
}

export function OpenAddWizard() {
    cy.Click(FBLStockSelectors.Button, FBLStockSelectors.Add);
}

export function FillFBLStockDetails(fblStockDetails: FBLStockDetails) {

    let RandomNStartNumber = GenerateFourDigitkRandomNumber(FBLStockSelectors.MinRandomNumber, FBLStockSelectors.MaxRandomNumber)
    console.log(RandomNStartNumber + 1000)
    let RandomEndNumber = GenerateFourDigitkRandomNumber(RandomNStartNumber, RandomNStartNumber + 1000)
    let randomAmount = GenerateFourDigitkRandomNumber(FBLStockSelectors.MinRandomNumber, FBLStockSelectors.MaxAmountRandomNumber)
    byAmount = fblStockDetails.ByAmount

    if (fblStockDetails.ByEndNumber.toLowerCase() == 'yes') {
        cy.FillLogTextBox(FBLStockSelectors.FBLStockStartNumber, RandomNStartNumber.toString())
        cy.FillLogTextBox(FBLStockSelectors.FBLStockEndNumber, RandomEndNumber.toString())
        cy.ClickRadio(FBLStockSelectors.FBLStockByEndNumber)
    }
    else if (fblStockDetails.ByAmount.toLowerCase() == 'yes') {
        cy.FillLogTextBox(FBLStockSelectors.FBLStockStartNumber, RandomNStartNumber.toString())
        cy.ClickRadio(FBLStockSelectors.FBLStockByAmount).then(() => {
            cy.FillLogTextBox(FBLStockSelectors.FBLStockAmount, randomAmount.toString())
        })
    }
}

export function DefineGetFBLStockGetAll() {
    cy.DefineRequestWait(RestAPI.GET, Urls.FBLStocksGetAll, RequestAliases.GetAllFBLStock);
}

export function CreateFBLStock() {
    DefinePostFBLStockRequest()
    DefineGetFBLStockGetAll()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
    
}

export function DefinePostFBLStockRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.FBLStocks, RequestAliases.GetFBLStock)
}

export function AssertCreateFBLStock() {
    let intercept = cy.wait("@" + RequestAliases.GetFBLStock);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            ReCreateFBLStock();
        }
        else {
            AssertGetFBLStock(interception.response.statusCode, 200, interception.response.body)
        }
    })
}

export function ReCreateFBLStock() {

    let RandomNStartNumber = GenerateFourDigitkRandomNumber(FBLStockSelectors.MinRandomNumber, FBLStockSelectors.MaxRandomNumber)
    let RandomEndNumber = GenerateFourDigitkRandomNumber(RandomNStartNumber, RandomNStartNumber+ 1000)
    let randomAmount = GenerateFourDigitkRandomNumber(FBLStockSelectors.MinRandomNumber, FBLStockSelectors.MaxAmountRandomNumber)

    if (byAmount.toLowerCase() == 'yes') {
        cy.FillLogTextBox(FBLStockSelectors.FBLStockStartNumber, RandomNStartNumber.toString())
        cy.FillLogTextBox(FBLStockSelectors.FBLStockAmount, randomAmount.toString())
    }
    else {
        cy.FillLogTextBox(FBLStockSelectors.FBLStockStartNumber, RandomNStartNumber.toString())
        cy.FillLogTextBox(FBLStockSelectors.FBLStockEndNumber, RandomEndNumber.toString())
    }
    CreateFBLStock()
    AssertCreateFBLStock()
}

export function AssertGetAllFBLStock() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetAllFBLStock, 200)
    BaseAssertion.AssertElementExist(FBLStockSelectors.FBLStockGridBody)
}

export function AssertGetFBLStock(responseStatusCode: number, expectedStatusCode: number, responseBody: string) {
    assert.equal(responseStatusCode, expectedStatusCode)
}

export function RemoveFBLStock() {
    DefineDeleteFBLStockRequest();
    DefineGetFBLStockGetAll()
    cy.get(FBLStockSelectors.FBLStockGridBody).find(FBLStockSelectors.FBLStockGridRow).first().click().then(() => {
        cy.Click(FBLStockSelectors.Button, FBLStockSelectors.Remove)
        cy.Click(FBLStockSelectors.ConfirmRemove, FBLStockSelectors.Delete)
    })
}

function DefineDeleteFBLStockRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.FBLStocksDelete, RequestAliases.RemoveFBLStock);
}

export function AssertRemoveFBLStock() {
    BaseAssertion.AssertStatusCode(RequestAliases.RemoveFBLStock, 200)
}

export function RemoveFBLStockSeries() {
    DefineDeleteFBLStockRequest();
    DefineGetFBLStockGetAll()
    cy.get(FBLStockSelectors.FBLStockGridBody).find(FBLStockSelectors.FBLStockGridRow).first().click({force : true}).then(() => {
        cy.Click(FBLStockSelectors.Button, FBLStockSelectors.RemoveSeries)
        cy.Click(FBLStockSelectors.ConfirmRemove, FBLStockSelectors.Delete)
    })
}
