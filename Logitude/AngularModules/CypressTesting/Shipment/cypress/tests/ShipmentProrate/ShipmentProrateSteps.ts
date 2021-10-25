import * as Actions from '../../actions/Actions';
import * as ProrateActions from '../../actions/ProrateActions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { PackagesDetails } from '../../models/PackagesDetails';
import { PayableDetails } from "cypress/models/PayableDetails";

//#region variables
let MasterShipmentDetails: ShipmentDetails;
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

//#region Create house export air shipment inside the master
Given("the user in the master's Shipment tab", () => {
    Actions.OpenShipment(ShipmentContext.MasterNumber);
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
});

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
    Actions.CheckBusyIndicator()
});
//#endregion

//#region Add packages
Given("the user navigate to the first house workspace", () => {
    cy.get(ShipmentSelectors.HouseHyperLink).eq(0).click({ force: true })
});

Given("the user navigate to the seconed house workspace", () => {
    cy.get(ShipmentSelectors.HouseHyperLink).eq(1).click({ force: true })
});

Given("fill the following package details", (dataTable) => {
    let containerDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(MasterShipmentDetails.TransportMode, containerDetailsList, MasterShipmentDetails.ShipmentType);
});

When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveCloseButton + BaseSelectors.LastElement)
});

Then("the shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region generate the house shipment Packages in the master shipment
Given("the user navigate to packages workspace in master shipment", () => {
    cy.Click(ShipmentSelectors.PackagesTab + BaseSelectors.LastElement, null)
});

Given("bress on build from shipments", () => {
    cy.get(ShipmentSelectors.HouseHyperLink).contains("Build From 2 Shipments").click({ force: true })
});

When("save the shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton + BaseSelectors.LastElement)
});
//#endregion

//#region Add Payables
Given("a payable with the following details", (dataTable) => {
    let payableData = Assists.CreateInstance<PayableDetails>(dataTable, true);
    Actions.FillPayablesTab(payableData)
});
When("add payables", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

});
Then("the payables should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Assert prorate divided the values in the correct way
Given("press on the plus icon to see the prorate details", () => {
    cy.get(BaseSelectors.CustomTreeIcon).click()
});

Then("the prorate should be divided the values in the correct way", () => {
    ProrateActions.AssertProrate()
});
//#endregion

//#region Assert prorate divided the values in the correct way
Given("the user in the first house payable tab", () => {
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
    cy.get(ShipmentSelectors.HouseHyperLink).eq(0).click({ force: true })
    cy.Click(ShipmentSelectors.StartWithPayablesTab + BaseSelectors.LastElement, null)
});

When("press on edit payable button", () => {
    cy.Click(BaseSelectors.StartsWithEditButton + BaseSelectors.LastElement, null, true)
});

Then("this message {string} should be printed", (message) => {
    ProrateActions.AssertHouseHasNonEditablePayable(message)
});
//#endregion