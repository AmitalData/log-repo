import * as Actions from '../../actions/Actions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { ShipmentContext } from '../../models/ShipmentContext';

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentDetails: ShipmentDetails;
//#endregion

//#region Create master export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details", (dataTable) => {
    const shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    MasterShipmentDetails = shipmentDetails;
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Assert house wizerd
Given("the user in the master's Shipment tab", () => {
    Actions.OpenShipment(ShipmentContext.MasterNumber);
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
});

When("press on new house button", () => {
    cy.Click(ShipmentSelectors.NewAttachedHouse, null);
});

Then("direction, transport and main carriage should be dim", () => {
    Actions.AssertHouseWizerdInsideMaster()
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
});
//#endregion

//#region Create house export air shipment inside the master
When("create house with {string} as Shipper", (Shipper) => {
    Actions.CreateNewAttachedHouse(Shipper);
});

Then("the house should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.HouseNumber = interception.response.body.House;
    })
});

Then("the house should connect successfully", () => {
    Actions.CheckBusyIndicator()
    Actions.ValidateCheckHouseCheckBox();
});
//#endregion

//#region Assert main carriage be dim from inside the connected house
When("the user goes to the main carriage leg of the created house", () => {
    Actions.CheckBusyIndicator()
    Actions.NavigateMainCarriageLegForConnectedHouse()
});

Then("all fields of the main carriage leg should be dim", () => {
    Actions.AssertMainCarriageLegFieldsDisable()
});
//#endregion

//#region Assert these actions are dim: convert the house to direct and cancel shipment
Given("open the house shipment", () => {
    cy.Click(ShipmentSelectors.Backbutton, null)
    Actions.OpenShipment(ShipmentContext.HouseNumber);
});

When("press on menu button", () => {
    cy.Click(BaseSelectors.MenuButtons_Number + BaseSelectors.LastElement, null)
});

Then("convert the house to direct and cancel shipment should be dim", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.ConvertToDirect + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.CancelShipmentButton + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
});

Then("convert the house to direct and cancel shipment should not be dim", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.ConvertToDirect + BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.CancelShipmentButton + BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
});
//#endregion

//#region Disconnect the house shipment
Given("the user goes back to the master shipment", () => {
    cy.get(ShipmentSelectors.Backbutton_1).click({ force: true })
});

When("disconnect shipment", () => {
    Actions.CheckBusyIndicator()
    Actions.UncheckHouseCheckBox();
});

Then("the shipment should disconnect successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
    Actions.CheckBusyIndicator()
});
//#endregion

//#region Create house export air shipment steps
Given("a house Shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    cy.BackButton(BaseSelectors.ContainsOperations)
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

When("create house shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the house should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.HouseNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Connect the house shipment to the master 
Given("the user in the master's Shipment tab", () => {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelectors.ShipmentTab, null)
    Actions.OpenShipment(ShipmentContext.MasterNumber);
    cy.Click(ShipmentSelectors.ShipmentsTab, null);
});

When("connect the house shipment", () => {
    Actions.CheckBusyIndicator()
    Actions.CheckHouseCheckBox();
});

Then("the shipment should connect successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    Actions.CheckBusyIndicator()
});
//#endregion

//#region Disconnect the house shipment
When("the user disconnect the house shipment", () => {
    Actions.CheckBusyIndicator()
    Actions.UncheckHouseCheckBox();
});

Then("the shipment should disconnect successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion