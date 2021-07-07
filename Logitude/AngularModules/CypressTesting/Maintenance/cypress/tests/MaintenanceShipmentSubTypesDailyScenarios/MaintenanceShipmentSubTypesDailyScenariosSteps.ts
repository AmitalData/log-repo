import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { ShipmentSubTypeDetails } from "../../../cypress/models/ShipmentSubTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { Constants } from "../../constants/Constants";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

//#region variable
let shipmentSubTypesDetails: ShipmentSubTypeDetails
//#endregion

//#region Add Shipment Sub Type Code with lenght more than 6
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShipmentSubType)
});

When("add {string} as shipment sub type code", (ShipmentSubTypeCode) => {
    MaintenanceActions.OpenNewWizard(Constants.ShipmentSubType);
    MaintenanceActions.FillShipmentSubTypeCode(ShipmentSubTypeCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion
//#region  Create a new shipment sub type/Create a new shipment with already exists code
Given("a shipment sub type with the following details", (dataTable) => {
    shipmentSubTypesDetails = Assists.CreateInstance<ShipmentSubTypeDetails>(dataTable, true);
    MaintenanceActions.FillShipmentSubTypeDetails(shipmentSubTypesDetails)
});

When("create shipment sub type", () => {
    MaintenanceActions.CreateShipmentSubTypeMockCreate();
});
When("click create shipment sub type", () => {
    MaintenanceActions.CreateShipmentSubType();
});
Then("the shipment sub type should create successfully", () => {
    MaintenanceActions.AssertCreateShipmentSubTypeMockCreate();
});
Then("the shipment sub type should not create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostShipmentSubType, 400)
});
Then("a validation error with {string} message should appear", (ValidationMessage) => {
    BaseAssertion.AssertElementContain(BaseSelectors.SingleError, ValidationMessage)
});
//#endregion
//#region Search for the shipment sub type by code
When("search for {string} shipment sub type", (ShipmentSubTypeCode) => {
    MaintenanceActions.SearchShipmentSubTypeByCode(ShipmentSubTypeCode)
});

Then("the {string} shipment sub type should appear successfully", (ShipmentSubTypeCode) => {
    MaintenanceActions.AssertSearchShipmentSubTypeByCode(ShipmentSubTypeCode)
});

//#endregion
//#region Open the shipment sub type
When("open shipment sub type", () => {
    MaintenanceActions.OpenShipmentSubType()
});

Then("the shipment sub type should open successfully", () => {
    MaintenanceActions.AssertOpenShipmentSubType()
});
//#endregion
//#region  Edit the shipment sub type
Given("{string} as shipment sub type name", (name) => {
    MaintenanceActions.FillShipmentSubTypeName(name)
});

When("update shipment sub type", () => {
    MaintenanceActions.UpdateShipmentSubType()
});

Then("the shipment sub type should update successfully", () => {
    MaintenanceActions.AssertUpdateShipmentSubType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.ShipmentSubTypeEventsTab);
});
//#endregion