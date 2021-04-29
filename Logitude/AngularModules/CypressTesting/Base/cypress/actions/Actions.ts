import { BaseSelectors } from '../selectors/BaseSelectors';
import * as BaseAssertion from '../../cypress/actions/Assertion';
import { AccountingSelectors } from '../../../Accounting/cypress/selectors/Selectors';
import { RestAPI } from '../constants/RestAPI';
import { AccountingURLs } from '../../../Accounting/cypress/constants/URLs';
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import {BaseURLs}from "../constants/URLs"
import { WarehouseStorage } from "../../../Shipment/cypress/models/WarehouseStorage";
import * as gr from "../../../Base/cypress/actions/GenerateRandoms";
import { Datepicker } from "../models/Datepicker";
import { EventTypeDetails } from 'cypress/models/EventTypeDetails';

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
    cy.FillLogTextBox(BaseSelectors.NullSearch, BaseSelectors.ContainWarehouse);
    cy.Click(BaseSelectors.Warehouse, null);
}

export function FillWarehouseStorageDetails(warehouseDetails : WarehouseStorage){
    cy.Click(BaseSelectors.Hyperlink, BaseSelectors.WarehouseStorageDefaults);
    cy.SelectCheckBox(BaseSelectors.WarehouseChargeStorage);
    cy.FillLogLov(BaseSelectors.WarehouseCurrency, warehouseDetails.Currency, true);
    cy.FillLogTextBox(BaseSelectors.WarehouseStorageFreeDays, warehouseDetails.StorageFreeDays.toString());
}

export function FillWarehouseStorageWeightDetails(warehouseDetails : WarehouseStorage , transportmode:string){
    cy.FillLogLov(BaseSelectors.WarehouseStorageMeasurement(transportmode), warehouseDetails.Measurement, true)
    cy.FillLogLov(BaseSelectors.WarehouseStorageRounding(transportmode), warehouseDetails.Rounding, true)
}

export function FillWarehouseStoragePricing(warehousePricingList: WarehouseStorage[]) {
        if (Cypress.$(BaseSelectors.LogitudeIconButton).length > 2) {  
            DeletePricingDefaults()
        }
    AddPricingDefaults(warehousePricingList);
}
function DeletePricingDefaults(){
    for (let i = 0; i < BaseSelectors.DeleteButton.length; i++) {
        cy.get(BaseSelectors.DeleteButton).first().click();
        cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, BaseSelectors.ContainYes)
    }
}	
function AddPricingDefaults(warehousePricingList: WarehouseStorage[]){
    for (let i = 0; i < warehousePricingList.length; i++) {
        cy.Click(BaseSelectors.AddButton, null)
        FillCell(BaseSelectors.StepFromColumn, i, BaseSelectors.WarehouseStoragePricingStepFrom, warehousePricingList[i].StepFrom)
        FillCell(BaseSelectors.DaysColumn, i, BaseSelectors.WarehouseStoragePricingDays, warehousePricingList[i].NumberOfDays)
        FillCell(BaseSelectors.StepToColumn, i, BaseSelectors.WarehouseStoragePricingStepTo, warehousePricingList[i].StepTo)
        FillCell(BaseSelectors.SalePriceColumn, i, BaseSelectors.WarehouseStoragePricingSalePrice, warehousePricingList[i].SalePrice)
    }
}

export function OpenWarehouse(warehouseName:string){
    cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetFilterSearch(warehouseName), RequestAliases.GetByFilter)
    cy.FillLogTextBox(BaseSelectors.SearchField, warehouseName);
    BaseAssertion.AssertStatusCode(RequestAliases.GetByFilter, 200)
    cy.Click(BaseSelectors.GridFitstRow(),null,true)
}

export function UpdateWarehouse(){
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
    cy.DefineRequestWait(RestAPI.PUT, BaseURLs.Warehouses, RequestAliases.PutWarehouses)
    cy.Click(BaseSelectors.WarehouseSaveCloseBtn, null)
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
export function FormatDate(date: string): string{
    var currentDateArray = date.split("/");
    return currentDateArray[0] + " " + GetMonth(currentDateArray[1]) + " " + currentDateArray[2];
}

function GetMonth(monthNum: string) {
    switch (monthNum) {
      case "01": return "Jan"; 
      case "02": return "Feb"; 
      case "03": return "Mar"; 
      case "04": return "Apr"; 
      case "05": return "May"; 
      case "06": return "June"; 
      case "07": return "July"; 
      case "08": return "Aug"; 
      case "09": return "Sept"; 
      case "10": return "Oct"; 
      case "11": return "Nov"; 
      case "12": return "Dec"; 
    }
  }
export function AssertDateOneOf(daySelector:string){
    cy.get(daySelector).then(($day) => {
        const day = $day.text()
        expect(day).to.be.oneOf([FormatDate(GetTodayDate()),FormatDate(GetYesterdayDate()),FormatDate(GetTomorrowDate())] ) 
})
   
  }
  export function GetTodayDate() {
    var todayDate = new Date
    return FormateTheDate(todayDate)
}
export function GetYesterdayDate() {
    var yesterdayDate = new Date();
    yesterdayDate.setDate(yesterdayDate.getDate() - 1);
    return FormateTheDate(yesterdayDate)
}
export function GetTomorrowDate() {
    var tomorrowDate = new Date();
    tomorrowDate.setDate(tomorrowDate.getDate() + 1);
    return FormateTheDate(tomorrowDate)
}
export function SubstractDaysFromDate(Days: number) {
    var todayDate = new Date
    var pastDate = new Date

    pastDate.setDate(todayDate.getDate() - Days);
    return FormateTheDate(pastDate)
}

export function AddDaysToTodayDate(days: number) {
    var todayDate = new Date().toLocaleDateString("en-US", { timeZone: "Asia/Jerusalem"})
    var todayDateList = todayDate.split("/")

    todayDateList[1] = (Number(todayDateList[1]) + days).toString();
    return FormateTheDateString(todayDateList)
}

export function GetDatepicker(dateString: string): Datepicker{
    let currentDate = new Date();
    let dateDay: number;
    let dateMonth: number;
    let dateYear: number;

    if (dateString.toLowerCase() == "today") {
        dateDay = currentDate.getDate();
        dateMonth = currentDate.getMonth() + 1;
        dateYear = currentDate.getFullYear();
    }
    else if (dateString.toLowerCase() == "random") {
        dateMonth = gr.GenerateRandomNumber(1, 12);
        dateYear = gr.GenerateRandomNumber(1950, currentDate.getFullYear());
        dateDay = GetRandomDay(dateMonth, dateYear);
    }
    else{
        let date = new Date(dateString);
        dateDay = date.getDate();
        dateMonth = date.getMonth() + 1;
        dateYear = date.getFullYear();
    }

    let datepicker = new Datepicker();
    datepicker.Day = dateDay;
    datepicker.Month = dateMonth;
    datepicker.Year = dateYear;
    return datepicker;
}

function GetRandomDay(month: number, year: number){
    let maxDay = 0;
    if(month == 2){
        if(year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)){
            maxDay = 29;
        }
        else{
            maxDay = 28;
        }
    }
    else if([1, 3, 5, 7, 8, 10, 12].indexOf(month) != -1){
        maxDay = 31;
    }
    else if([4, 6, 9, 11].indexOf(month) != -1){
        maxDay = 30;
    }

    return gr.GenerateRandomNumber(1, maxDay);
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

function FormateTheDateString(dateList: string[]) {
    var DateFormat
    var dd = dateList[1].toString();
    var mm = dateList[0].toString();
    var yyyy = dateList[2].toString();

    if (Number(dd) < 10) {
        dd = "0" + dd;
    }
    if (Number(mm) < 10) {
        mm = "0" + mm;
    }

    DateFormat = dd + '/' + mm + '/' + yyyy;
    return DateFormat;
}

function FormateTheDate(date: Date) {
    var DateFormat
    var dd = date.getUTCDate().toString();
    var mm = (date.getUTCMonth() + 1).toString();
    var yyyy = date.getFullYear().toString();

    if (Number(dd) < 10) {
        dd = "0" + dd;
    }
    if (Number(mm) < 10) {
        mm = "0" + mm;
    }

    DateFormat = dd + '/' + mm + '/' + yyyy;
    return DateFormat;
}

function FillCell(columnNumber: string, rowNumber: number, pricingCellselector: string, value: number) {
    cy.get(BaseSelectors.CellWithRowAndCol(columnNumber, rowNumber.toString())).last().click({ force: true })
    cy.FillLogTextBox(pricingCellselector, value.toString())
}

export function ValidateEventsTab(expectedEventDetailsList: EventTypeDetails[] , eventTabSelector:string) {
    cy.get(eventTabSelector).then(($eventTab) => {
        cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetTraceEventsForEntity, RequestAliases.GetTraceEventsForEntity);
        if ($eventTab.hasClass("SelectedMenuItem")) {
            cy.Click(BaseSelectors.RefreshImg+BaseSelectors.LastElement, null,true);
        } else {
            cy.Click(eventTabSelector, null,true);
        }
        BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEventsForEntity, 200);
        cy.Click(BaseSelectors.RefreshImg+BaseSelectors.LastElement, null,true);
        for (let i = 0; i < expectedEventDetailsList.length; i++) {
            let expectedEvent = expectedEventDetailsList[i].Event;
            let expectedNotes = expectedEventDetailsList[i].Notes;
    
            if (expectedEvent) {
                cy.contains(expectedEvent).eq(0).should("exist");
            }
    
            if (expectedEvent && expectedNotes) {
                cy.get(BaseSelectors.EventItemBox).contains(expectedEvent).eq(0).parents(BaseSelectors.EventItemBox).within(() => {
                    cy.get(BaseSelectors.textarea).should("have.value", expectedNotes);
                });
            }
        }
    });
}