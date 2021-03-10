import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { EventTypeDetails } from "../../models/EventTypeDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentConversionContext } from "../../models/ShipmentConversionContext";
import * as Assists from "../../../../Base/cypress/assists/Assists";


Given("the user logged in and navigate to shipments workspace", () => {
    cy.Login();
    Actions.NavigatesToShipmentsWorkspace();
});

Given("a shipment with the following details", (dataTable) => {
    let shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentConversionContext.ShipmentLevel = shipmentDetails.ShipmentLevel;
    ShipmentConversionContext.ShipmentTransportMode = shipmentDetails.TransportMode;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

Given("the user open the shipment", () => {
    Actions.OpenShipment(ShipmentConversionContext.ShipmentNumber);
});

Given("open direction conversion wizard", () => {
    Actions.OpenDirectionConversionWizard();
});

Given("the user open direct to house conversion wizard", () => {
    Actions.OpenDirectAndHouseConversionWizard("House");
});

Given("the user open house to direct conversion wizard", () => {
    Actions.OpenDirectAndHouseConversionWizard("Direct");
});

Given("the user open FCL to LCL conversion wizard", () => {
    Actions.OpenFclAndLclConversionWizard("LCL");
});

Given("the user open LCL to FCL conversion wizard", () => {
    Actions.OpenFclAndLclConversionWizard("FCL");
});

Given("fill the following details for direction conversion", (dataTable) => {
    let shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    shipmentDetails.TransportMode = ShipmentConversionContext.ShipmentTransportMode;
    Actions.FillDirectionConversionWizard(shipmentDetails);
});

Given("fill notes {string}", (notes: string) => {
    Actions.FillEventNotesWizard(notes);
});

When("create shipment", () => {
    Actions.CreateShipment(ShipmentConversionContext.ShipmentLevel);
});

When("convert shipment direction", () => {
    Actions.ConvertShipmentDirection();
});

When("convert shipment type", () => {
    Actions.ConvertShipmentType();
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentConversionContext.ShipmentNumber = interception.response.body.ShipmentNumber;
    });
});

Then("direction should be {string}", (expectedDirection: string) => {
    Actions.ValidateShipmentDirectionInShortTitle(expectedDirection);
});

Then("type should be {string}", (expectedShipmentType: string) => {
    Actions.ValidateShipmentTypeInHeaderScreen(expectedShipmentType);
});

Then("following events should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    eventDetailsList = ShipmentConversionEventsMapping(eventDetailsList);
    Actions.ValidateEventsTab(eventDetailsList);
});

Then("the direction should convert successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShipment, 200).then((interception) => {
        ShipmentConversionContext.NewShipmentNumber = interception.response.body.ShipmentNumber;
    });
});

Then("the type should convert successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShipment, 200);
});

Then("the shipment should have a new number", () => {
    Actions.ValidateShipmentNumberInShortTitle(ShipmentConversionContext.NewShipmentNumber);
});

Then("the consignee {string} should appear in partners tab", (consignee: string) => {
    Actions.ValidatePartnerInPartnersTab("Consignee", consignee);
});

Then("the button {string} should appear in packages tab", (buttonContains: string) => {
    Actions.ValidateAddButtonInPackagesTab(buttonContains);
});


function ShipmentConversionEventsMapping(eventDetailsList: EventTypeDetails[]): EventTypeDetails[]{
    for (let i = 0; i < eventDetailsList.length; i++) {
        eventDetailsList[i].Notes = eventDetailsList[i].Notes.replace(/\"OldShipmentNumber\"/gi, ShipmentConversionContext.ShipmentNumber);
    }
    return eventDetailsList;
}