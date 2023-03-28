import * as Actions from '../../actions/Actions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { PackagesDetails } from '../../models/PackagesDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';

let MasterShipmentDetails: ShipmentDetails;

//#region Create master export air/Ocean shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("the user navigates to shipments workspace", () => {
    cy.Click(ShipmentSelectors.Backbutton, null)
});

Given("a master Shipment with following details", (dataTable) => {
    MasterShipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
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
    cy.wait(5000)
});
//#endregion

//#region Add Containers/ packages
Given("the user navigate to the packages workspace", () => {
    cy.get(ShipmentSelectors.HouseHyperLink).contains((ShipmentContext.HouseNumber).replace(/^0+/, '')).click({ force: true })
});

Given("the following details", (dataTable) => {
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
    cy.get(ShipmentSelectors.HouseHyperLink).contains("Build From 1 Shipments").click({ force: true })
});

When("save the shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the shipment should have two packages", () => {
    cy.get(ShipmentSelectors.Row).should('have.length', 2)
});
//#endregion