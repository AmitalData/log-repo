import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { PayableDetails } from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { CreatedShipment } from "cypress/models/CreatedShipment";
import { PackagesDetails } from "cypress/models/PackagesDetails";
let shipmentDetails: ShipmentDetails;
let packagesDetails:PackagesDetails

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
});
Given("a direct shipment with the following details",
    (dataTable) => {
        shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
        Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
        Actions.FillShipmentWizardsFields(shipmentDetails);
    });
When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});
Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Given("the user fills {string} as GrossWeight and {string} as a MoveType", (GrossWeight, MoveType) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click(Selectors.GeneralTab, null)
    Actions.FillGeneralTab(GrossWeight, MoveType)
});

Given("the user add order package with the following details",
    (dataTable) => {
        packagesDetails = dataTable.hashes() as PackagesDetails;
        
        cy.Click(Selectors.OrdersTab, null)
        Actions.FillOrderTab(packagesDetails)
    });



Given("the user adds partners with following details", (dataTable) => {
    cy.Click(Selectors.PartnersTab, null)
    const partnersDetails = dataTable.hashes()[0] as PartnersDetails;
    Actions.FillPartnersTab(shipmentDetails.Direction, shipmentDetails.TransportMode, partnersDetails)
});

Given("the user add package with the following details", (dataTable) => {
    packagesDetails = dataTable.hashes()[0] as PackagesDetails;
    cy.Click(Selectors.PackagesTab, null)
    Actions.FillPackagesTab(packagesDetails)

});

Given("the user fill receivables with the following details", (dataTable) => {
    const ReceivableData = dataTable.hashes()[0] as ReceivableDetails;
    cy.Click(Selectors.ReceivablesTab, null)
    Actions.FillReceivablesTab(ReceivableData)
});

Given("the user add new pickup", () => {
    cy.Click(Selectors.RoutingsTab, null)
    Actions.FillPickupRouting()

});
Given("add delivery with {string} as a partner routing", (partner) => {
    Actions.FillDeliveryRouting(partner)
});
Given("add pre carriage and on carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting(shipmentDetails.TransportMode, fromPort, toPort)
    Actions.FillOnCarriageRouting(shipmentDetails.TransportMode, fromPort, toPort)
});


Given("the user add payable with the following details", 
    (dataTable) => {
        const PayableData = dataTable.hashes()[0] as PayableDetails;
        cy.Click(Selectors.PayablesTab, null)
        Actions.FillPayablesTab(PayableData)
});

When("update shipment", () => {
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)
});

Then("the direct should update successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);
});