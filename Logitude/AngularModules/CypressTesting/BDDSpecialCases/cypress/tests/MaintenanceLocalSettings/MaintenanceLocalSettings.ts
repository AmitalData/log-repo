import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as ShipmentActions from "../../../../Shipment/cypress/actions/Actions"
import * as Actions from "../../actions/Actions"
import * as MaintenanceActions from "../../../../Maintenance/cypress/actions/Actions"
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { MaintenanceSelectors } from "../../../../Maintenance/cypress/selectors/Selectors";
import { ShipmentSelectors } from "../../../../Shipment/cypress/selectors/Selectors";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { LocalSettingsDetails } from "../../models/LocalSettingsDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentDetails } from "../../../../Shipment/cypress/models/ShipmentDetails";
import {BDDSpecialCasesSelectors} from "../../selectors/Selectors"

let shipmentDetails: ShipmentDetails
let localSettingsDetails : LocalSettingsDetails

//#region Change time zone and date time format
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.LocalSettingsMaintenanceItem)
});

Given("the user open {string} in maintenance menu", (maintenanceItemName) => {
    cy.BackButton("Operations")
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.LocalSettingsMaintenanceItem)
});

Given("local settings with the following details", (dataTable) => {
    localSettingsDetails = Assists.CreateInstance<LocalSettingsDetails>(dataTable, true);
    Actions.FillLocalSettingsDetails(localSettingsDetails);
});

When("save local settings", () => {
    Actions.UpdateLocalSettings();
});

Then("the local settings should update successfully", () => {
    Actions.AssertUpdateLocalSettings();
});
//#endregion

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Date format
Given("open the direct shipment and navigate to general tab", () => {
    ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber)
    cy.Navigate(ShipmentSelectors.GeneralTab)
});

When("fill {string} as HAWB date", (date) => {
    cy.FillDate(BDDSpecialCasesSelectors.HAWBDate,date);
});

Then("the date format should be {string}", (dateFormat) => {
    Actions.ValidateDateFormat(dateFormat);
});
//#endregion

//#region Event time zone
Given("the user update the shipment", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    localSettingsDetails = MapTimeZoneToCountry(localSettingsDetails);
    LocalSettingsDetails.UpdateTime = new Date().toLocaleTimeString("en-US", { timeZone: localSettingsDetails.TimeZoneRegion })
});

When("navigate to event tab", () => {
    cy.Navigate(ShipmentSelectors.EventsTab)
});

Then("the {string} event should include the time of {string} timezone", (eventName , timezone) => {
    Actions.ValidateTimeInEventsTab(eventName,ShipmentSelectors.EventsTab)
});

//#endregion

function MapTimeZoneToCountry(localSettingsDetails:LocalSettingsDetails){
    if(localSettingsDetails.TimeZone == "(UTC+03:00)"){
        localSettingsDetails.TimeZoneRegion = "Asia/Jerusalem"
    }
    if(localSettingsDetails.TimeZone == "(UTC-06:00)"){
        localSettingsDetails.TimeZoneRegion = "America/Costa_Rica"
    }
    return localSettingsDetails ;
}