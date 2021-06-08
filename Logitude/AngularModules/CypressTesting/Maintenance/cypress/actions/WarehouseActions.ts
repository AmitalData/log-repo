import { WarehousesSelectors } from "../selectors/WarehousesSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import { CardDetails } from "../models/CardDetails";
import { ContactDetails } from "../models/ContactDetails";
import { CardGeneralTabDetails } from "../models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "../models/CardBillingTabDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

let WarehouseCode = null

export function FillWarehouseDetails(warehouseDetails: CardDetails) {
    Actions.FillCardDetails(warehouseDetails, 4)
}

export function FillWarehouseContactDetails(conatactDetails: ContactDetails) {
    Actions.FillCardContactDetails(conatactDetails)
}

export function FillWarehouseGeneralTab(warehouseGeneralTabDetails: CardGeneralTabDetails) {
    cy.FillLogTextBox(WarehousesSelectors.Notes, warehouseGeneralTabDetails.Notes)
    cy.ClickCheckBox("#Warehouse_InActive")
}

export function FillWarehouseBillingTab(warehouseBillingTabDetails: CardBillingTabDetails) {
    cy.FillLogTextBox(WarehousesSelectors.VatNumber, warehouseBillingTabDetails.VatNumber.toString())
    cy.FillLogTextBox(WarehousesSelectors.BankName, warehouseBillingTabDetails.BankName)
    cy.FillLogTextBox(WarehousesSelectors.IBANNumber, warehouseBillingTabDetails.IBANNo)
}

export function CreateWarehouse() {
    DefinePostWarehouseRequest()
    Actions.DefineGetByFilterRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePostWarehouseRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Warehouses, RequestAliases.PostWarehouses);
}

export function AssertCreateWarehouse() {
    AssertPostWarehouse()
    Actions.AssertGetByFilters()
}

function AssertPostWarehouse() {
    let intercept = cy.wait("@" + RequestAliases.PostWarehouses);
    intercept.then((interception) => {
        let statusCode = interception.response.statusCode;
        if (statusCode === 400) {
            ReCreateWarehouse();
        }
        else {
            assert.equal(statusCode, 200)
            WarehouseCode = interception.response.body.Warehouse.Code;
        }
    })
}

function ReCreateWarehouse() {
    let RandomCode = Actions.GenerateRandomNumber(4);
    cy.FillLogTextBox(MaintenanceSelectors.CardCode, RandomCode)
    CreateWarehouse();
    AssertCreateWarehouse();
}

export function SearchWarehouse() {
    Actions.SearchCardByValue(WarehouseCode)
}

export function AssertSearchWarehouse() {
    Actions.AssertSearchCard(WarehouseCode)
}

export function OpenWarehouse() {
    DefineWarehousesGetSingleRequest();
    cy.get(BaseSelectors.RowClass).eq(0).click();
}

function DefineWarehousesGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.WarehousesGetSingle, RequestAliases.GetSignle);
}

export function AssertOpenWarehouse() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
    BaseAssertion.AssertElementExist(MaintenanceSelectors.GeneralEditScreen);
}

export function UpdateWarehouse() {
    DefinePutWarehouseRequest()
    cy.Click(WarehousesSelectors.SaveButton, null)
}

function DefinePutWarehouseRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.PUTWarehouses, RequestAliases.PutWarehouses);
}

export function AssertUpdateWarehouse() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWarehouses, 200)
}

export function CloseSaveWarehouse() {
    DefineWarehouseViewGetSingleRequest()
    cy.Click(WarehousesSelectors.SaveCloseButton, null);
}

function DefineWarehouseViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.WarehouseviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveWarehouse() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}