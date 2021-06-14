import { FBLStockSelectors } from "../selectors/FBLStockSelectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { FBLStockDetails } from 'cypress/models/FBLStockDetails';

let searchStartNumber = null

export function OpenAddWizard() {
    cy.Click(FBLStockSelectors.Button, FBLStockSelectors.Add);
}

function GenerateRandomNumber() {
    return (Math.floor(Math.random() * 100000))
}

export function FillFBLStockDetails(fblStockDetails: FBLStockDetails) {
    let startNumber = GenerateRandomNumber()
    searchStartNumber = startNumber
    let endNumber = startNumber + 1
    cy.FillLogTextBox(FBLStockSelectors.StartNumber, startNumber.toString())
    if (fblStockDetails.ByAmount) {
        cy.ClickRadio(FBLStockSelectors.ByAmount).then(() => {
            cy.FillLogTextBox(FBLStockSelectors.Amount, fblStockDetails.Amount)
        })
    }
    else {
        cy.FillLogTextBox(FBLStockSelectors.EndNumber, endNumber.toString())
    }
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
        let statusCode = interception.response.statusCode
        if (statusCode === 400) {
            ReCreateFBLStock();
        }
        else {
            assert.equal(statusCode, 200)
        }
    })
}

export function ReCreateFBLStock() {
    let startNumber = GenerateRandomNumber()
    searchStartNumber = startNumber
    cy.FillLogTextBox(FBLStockSelectors.StartNumber, startNumber.toString())
    CreateFBLStock()
    AssertCreateFBLStock()
}

export function AssertGetAllFBLStock() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetAllFBLStock, 200)
    BaseAssertion.AssertElementExist(FBLStockSelectors.GridBody)
}

export function RemoveFBLStock() {
    DefineDeleteFBLStockRequest();
    DefineGetFBLStockGetAll()
    cy.get(".GridViewCell").contains(searchStartNumber.toString()).click().then(() => {
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
    DefineDeleteFBLStockRequest()
    DefineGetFBLStockGetAll()
    cy.get(".GridViewCell").contains(searchStartNumber.toString()).click().then(() => {
        cy.Click(FBLStockSelectors.Button, FBLStockSelectors.RemoveSeries)
        cy.Click(FBLStockSelectors.ConfirmRemove, FBLStockSelectors.Delete)
    })
}

export function DefineGetFBLStockGetAll() {
    cy.DefineRequestWait(RestAPI.GET, Urls.FBLStocksGetAll, RequestAliases.GetAllFBLStock);
}