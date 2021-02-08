import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { PayableDetails } from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { PackagesDetails } from "cypress/models/PackagesDetails";

//#region variables
let shipmentDetails: ShipmentDetails;
let packagesDetails: PackagesDetails[];
//#endregion

//#region Create direct export air shipment Given steps
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});
//#endregion

//#region Update general tab given step
Given("the user fill {string} as GrossWeight and {string} as a MoveType", (GrossWeight, MoveType) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    Actions.FillGeneralTab(GrossWeight, MoveType)
});
//#endregion

//#region Update orders tab given step
Given("the user add order package with the following details", (dataTable) => {
    packagesDetails = dataTable.hashes() as PackagesDetails[];
    Actions.FillOrdersTab(packagesDetails)
});
//#endregion

//#region Update partners tab given step
Given("the user add partners with following details", (dataTable) => {
    const partnersDetails = dataTable.hashes()[0] as PartnersDetails;
    Actions.FillPartnersTab(shipmentDetails.Direction, shipmentDetails.TransportMode, partnersDetails)
});
//#endregion

//#region Update packages tab given step
Given("the user add package with the following details", (dataTable) => {
    packagesDetails = dataTable.hashes() as PackagesDetails[];
    Actions.FillPackageTab(shipmentDetails.TransportMode, packagesDetails)
});
//#endregion

//#region Update receivables tab given step
Given("the user fill receivables with the following details", (dataTable) => {
    const ReceivableData = dataTable.hashes()[0] as ReceivableDetails;
    Actions.FillReceivablesTab(ReceivableData)
});
//#endregion

//#region Update routing tab given steps
Given("the user add new pickup", () => {
    Actions.FillPickupRouting()
});
Given("add delivery with {string} as a partner routing", (partner) => {
    Actions.FillDeliveryRouting(partner)
});
Given("add pre carriage and on carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting(shipmentDetails.TransportMode, fromPort, toPort)
    Actions.FillOnCarriageRouting(shipmentDetails.TransportMode, fromPort, toPort)
});
//#endregion

//#region  Update payables tab given step
Given("the user add payable with the following details", (dataTable) => {
    const PayableData = dataTable.hashes()[0] as PayableDetails;
    Actions.FillPayablesTab(PayableData)
});
//#endregion

//#region Actions steps
When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("update shipment", () => {
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});
//#endregion

//#region Assert steps
Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Then("the direct should update successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);
});
//#endregion