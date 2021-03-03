import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { EventDetails } from "../../models/EventDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentConversionContext } from "../../models/ShipmentConversionContext";


Given("the user logged in and navigate to shipments workspace", () => {
    cy.Login();
    Actions.NavigatesToShipmentsWorkspace();
});

Given("a shipment with the following details", (dataTable) => {
    let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentConversionContext.ShipmentLevel = shipmentDetails.ShipmentLevel;
    ShipmentConversionContext.ShipmentTransportMode = shipmentDetails.TransportMode;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

Given("the user open direction conversion wizard", () => {
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
    let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
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
    Actions.ValidateShipmentDirection(expectedDirection);
});

Then("type should be {string}", (expectedShipmentType: string) => {
    Actions.ValidateShipmentType(expectedShipmentType);
});

Then("following events should appear in events tab", (dataTable) => {
    let eventDetailsList = dataTable.hashes() as EventDetails[];
    eventDetailsList = ShipmentConversionEventsMapping(eventDetailsList);
    Actions.ValidateEventTypes(eventDetailsList);
});

Then("the direction should convert successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShipment, 200).then((interception) => {
        ShipmentConversionContext.NewShipmentNumber = interception.response.body.ShipmentNumber;
    });
});

Then("the type should convert successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PutShipment, 200);
});


function ShipmentConversionEventsMapping(eventDetailsList: EventDetails[]): EventDetails[]{
    let oldShipmentNumberRegex = "\"OldShipmentNumber\"";
    for (let i = 0; i < eventDetailsList.length; i++) {
        if(eventDetailsList[i].Notes.indexOf(oldShipmentNumberRegex) !== -1){
            eventDetailsList[i].Notes = eventDetailsList[i].Notes.replace(oldShipmentNumberRegex, ShipmentConversionContext.ShipmentNumber);
        }
    }
    return eventDetailsList;
}