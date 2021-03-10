import { BaseSelectors } from '../selectors/BaseSelectors';
import * as BaseAssertion from '../../cypress/actions/Assertion';
import { AccountingSelectors } from '../../../Accounting/cypress/selectors/Selectors';
import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { RestAPI } from '../constants/RestAPI';
import { AccountingURLs } from '../../../Accounting/cypress/constants/URLs';
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import {BaseURLs}from "../constants/URLs"
import { WarehouseStorage } from "../../../Shipment/cypress/models/WarehouseStorage";
import { multiply } from 'cypress/types/lodash';

export function NavigatesToMaintenanceMenu() {
    cy.Click(BaseSelectors.MaintenanceMenu, null)
}

export function NavigatesToCustomsSettings() {
    NavigatesToMaintenanceMenu();
    cy.Click(BaseSelectors.SystemSettings, null)
    cy.Click(BaseSelectors.CustomsSettings, null)
}

export function NavigatesToWarehouse(){
    cy.Click(BaseSelectors.MaintenanceMenu, null);
    cy.FillLogTextBox(BaseSelectors.NullSearch, "Warehouse");
    cy.Click(BaseSelectors.Warehouse, null);
}

export function FillWarehouseStorageDetails(warehouseDetails : WarehouseStorage){
    cy.get(BaseSelectors.Hyperlink).contains(BaseSelectors.WarehouseStorageDefaults).click();
    cy.SelectCheckBox(BaseSelectors.WarehouseChargeStorage)
    cy.FillLogLov(BaseSelectors.WarehouseCurrencyId, warehouseDetails.Currency, true)
    cy.FillLogTextBox(BaseSelectors.WarehouseStorageFreeDays, warehouseDetails.StorageFreeDays.toString());
}

export function FillWarehouseStorageWeightDetails(warehouseDetails : WarehouseStorage , transportmode:string){
    cy.FillLogLov(BaseSelectors.WarehouseStorageMeasurement(transportmode), warehouseDetails.Measurement, true)
    cy.FillLogLov(BaseSelectors.WarehouseStorageRounding(transportmode), warehouseDetails.Rounding, true)
}

export function FillWarehouseStoragePricing(warehousePricingList: WarehouseStorage[]) {

    cy.get("button[id^=Delete]").its('length').then(deleteButtons => {
        for(let i = 0; i < deleteButtons; i++){
            cy.get("button[id^=Delete]").first().click();
            cy.Click(BaseSelectors.RedButton+BaseSelectors.LastElement,BaseSelectors.ContainYes)
        }
    });

    for (let i = 0; i < warehousePricingList.length; i++) {
        cy.Click(BaseSelectors.AddButton, null)
        getCellAndFill(BaseSelectors.StepFromColumn, i, BaseSelectors.WarehouseStoragePricingStepFrom, warehousePricingList[i].StepFrom)
        getCellAndFill(BaseSelectors.DaysColumn, i, BaseSelectors.WarehouseStoragePricingDays, warehousePricingList[i].NumberOfDays)
        getCellAndFill(BaseSelectors.StepToColumn, i, BaseSelectors.WarehouseStoragePricingStepTo, warehousePricingList[i].StepTo)
        getCellAndFill(BaseSelectors.SalePriceColumn, i, BaseSelectors.WarehouseStoragePricingSalePrice, warehousePricingList[i].SalePrice)
    }
}

export function OpenWarehouseWithName(warehouseName:string){
    cy.FillLogTextBox(BaseSelectors.SearchField, warehouseName);
    cy.wait(3000)
    cy.Click(BaseSelectors.GridFitstRow(),null,true)
}

export function ActivateCustomsManagementInShipments() {
    NavigatesToCustomsSettings()
    cy.get(BaseSelectors.typeCheckbox).check({ force: true })
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)

}

export function ClearExternalIDFromShipmentLevel(ExternalIDName: string) {
    let ExternalIdSelectorTextBox: string;
    let EntitySelector = AccountingSelectors.LogLovId(ExternalIDName)
    let SaveCloseButton = BaseSelectors.SaveClose(ExternalIDName)
    let AccountingSelector = BaseSelectors.AccountTab(ExternalIDName)

    if (ExternalIDName == BaseSelectors.ChargesType) {
        ExternalIdSelectorTextBox = BaseSelectors.ChargesTypeReceivableCreditAccount
    }
    else if (ExternalIDName == BaseSelectors.Currency) {
        ExternalIdSelectorTextBox = BaseSelectors.CurrencyAccountingExternalCode
    }
    else if (ExternalIDName == BaseSelectors.Partner) {
        ExternalIdSelectorTextBox = BaseSelectors.CustomerReceivablesAccountingCard;
    }

    ClickOnMaintenanceButton(EntitySelector)
    ClickOnEditInMaintenanceButton();
    NavigateToAccountTab(ExternalIDName, AccountingSelector)
    cy.get(ExternalIdSelectorTextBox).clear();
    cy.Click(SaveCloseButton, null);
    BaseAssertion.AssertElementNotExist(SaveCloseButton)
}

function ClickOnMaintenanceButton(LogLovSelector: string) {
    cy.ClickingAfterHovering(LogLovSelector, BaseSelectors.MaintenanceButton)
}

function ClickOnEditInMaintenanceButton() {
    cy.Click(BaseSelectors.MTCPopup, BaseSelectors.ContainsEdit, true)
}

function NavigateToAccountTab(ExternalIDName: string, AccountingSelector: string) {
    cy.Navigate(AccountingSelector);
    if (ExternalIDName == BaseSelectors.ChargesType) {
        cy.DefineRequestWait(RestAPI.GET, AccountingURLs.EntityResourceAccountingPeriod, RequestAliases.EntityResourceAccountingPeriod)
        BaseAssertion.AssertStatusCode(RequestAliases.EntityResourceAccountingPeriod, 200)
    }
}

export function GetstringWithoutLastCharacter(text: string) {
    let textWithoutLastCharacter = text.substring(0, (text.length - 1)).toString();
    return textWithoutLastCharacter;
}

export function GetLastCharacter(text: string) {
    let lastCharacterOfText = text.slice(text.length - 1).toString();
    return lastCharacterOfText
}

export function ClickOnRowDependingOnValue(value: string) {
    cy.get(BaseSelectors.RowCellClass).find(BaseSelectors.TextTrimming).contains(value)
        .parents(BaseSelectors.ListItem).click({ force: true })
}
export function CloseWindow() {
    cy.Click(BaseSelectors.button, BaseSelectors.ContainClose)
}

export function GetTodayDate() {
    var todayDate = new Date
    return FormateTheDate(todayDate)
}

export function SubstractDaysFromDate(Days: number) {
    var todayDate = new Date
    var pastDate = new Date

    pastDate.setDate(todayDate.getDate() - 8);
    return FormateTheDate(pastDate)
}

function FormateTheDate(date: Date) {
    var dd = date.getUTCDate();
    var mm = date.getUTCMonth() + 1
    var yyyy = date.getFullYear();

    let DateFormat = "0" + dd + '/' + "0" + mm + '/' + yyyy;
    return DateFormat
}

function getCellAndFill(columnNumber: string, rowNumber: number, pricingCellselector: string, value: number) {
    cy.get(BaseSelectors.CellWithRowAndCol(columnNumber, rowNumber.toString())).last().click({ force: true })
    cy.FillLogTextBox(pricingCellselector, value.toString())
}