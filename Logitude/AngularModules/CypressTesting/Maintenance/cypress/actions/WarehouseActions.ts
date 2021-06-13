import { WarehousesSelectors } from "../selectors/WarehousesSelectors";
import { MaintenanceSelectors } from "../selectors/Selectors";
import * as Actions from "./Actions";
import { CardDetails } from "../models/CardDetails";
import { ContactDetails } from "../models/ContactDetails";
import { CardGeneralTabDetails } from "../models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "../models/CardBillingTabDetails";
import { Constants } from "../constants/Constants";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

export function FillWarehouseDetails(warehouseDetails: CardDetails) {
    Actions.FillCardDetails(warehouseDetails, 4)
}

export function FillWarehouseContactDetails(conatactDetails: ContactDetails) {
    Actions.FillCardContactDetails(conatactDetails)
}

export function FillWarehouseGeneralTab(warehouseGeneralTabDetails: CardGeneralTabDetails) {
    cy.FillLogTextBox(WarehousesSelectors.Notes, warehouseGeneralTabDetails.Notes)
}

export function FillWarehouseBillingTab(warehouseBillingTabDetails: CardBillingTabDetails) {
    cy.FillLogTextBox(WarehousesSelectors.VatNumber, warehouseBillingTabDetails.VatNumber.toString())
    cy.FillLogTextBox(WarehousesSelectors.BankName, warehouseBillingTabDetails.BankName)
    cy.FillLogTextBox(WarehousesSelectors.IBANNumber, warehouseBillingTabDetails.IBANNo)
}

export function CreateWarehouse() {
    Actions.CreateCard()
}

export function UpdateWarehouse() {
    DefinePutWarehouseRequest()
    cy.Click(WarehousesSelectors.SaveButton, null)
}

function DefinePutWarehouseRequest() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Warehouses, RequestAliases.PutWarehouses);
}

export function AssertCreateWarehouse() {
    Actions.AssertCreateCard(Constants.Warehouse)
}

export function AssertUpdateWarehouse() {
    AssertPutWarehouse()
}

function AssertPutWarehouse() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutWarehouses, 200);
}

export function SearchWarehouse() {
    Actions.SearchCard()
}

export function AssertSearchWarehouse(Code: string) {
    Actions.AssertSearchCard(Code)
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

export function CloseSaveWarehouse() {
    DefineWarehouseViewGetSingleRequest()
    cy.Click(WarehousesSelectors.SaveCloseButton, null);
}

function DefineWarehouseViewGetSingleRequest() {
    cy.DefineRequestWait(RestAPI.GET, Urls.WarehouseviewGetSingle, RequestAliases.GetSignle);
}

export function AssertCloseSaveWarehouse() {
    AssertWarehouseGetSingle();
}

function AssertWarehouseGetSingle() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSignle, 200);
}