import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { TicketDetails } from "../../models/TicketDetails"
import { ShipmentDetails } from "../../../../Shipment/cypress/models/ShipmentDetails"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { TicketSelectors } from "../../selectors/Selectors";
import { ShipmentSelector } from "../../../../Shipment/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions";
import * as ShipmentActions from "../../../../Shipment/cypress/actions/Actions"

let shipmentNumber: string;
let TicketData: TicketDetails;
let _ShipmentDetails: ShipmentDetails;

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelector.ShipmentTab, null)
});

Given("a direct shipment with the following details",
    (dataTable) => {
        const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
        _ShipmentDetails = shipmentDetails;
        ShipmentActions.OpenNewShipmentWizard(_ShipmentDetails.ShipmentLevel);
        ShipmentActions.FillShipmentWizardsFields(_ShipmentDetails);
    });

When("create shipment", () => {
    ShipmentActions.CreateShipment(_ShipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    });
});

Given("the user logged in and navigated to ticket workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.TicketsMenu, null)
});

Given("a ticket with the following details", (dataTable) => {
    let ticketDetails = dataTable.hashes()[0] as TicketDetails;
    TicketData = ticketDetails;
    Actions.FillTicketFields(TicketData);
});

When("create ticket", () => {
    cy.DefineRequestWait("POST", "**/tickets", "WaitPostTicketRequest")
    cy.Click(BaseSelectors.RedButton, "Create");
});

Then("the ticket should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostTicketRequest", 200).then((interception) => {
        TicketData.TicketNumber = interception.response.body.THENUMBER;
    })
});

Given("the user fill the shipment number as Entity Number", () => {
    Actions.OpenTicket(TicketData.TicketNumber);
    Actions.FillEntityNumber(shipmentNumber);
});

When("save as open", () => {
    cy.DefineRequestWait("PUT", "**/tickets", "WaitPutTicketRequest")
    cy.Click(TicketSelectors.SaveAsOpenButton, null);
});

Then("the ticket should update|save successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutTicketRequest", 200);
});
