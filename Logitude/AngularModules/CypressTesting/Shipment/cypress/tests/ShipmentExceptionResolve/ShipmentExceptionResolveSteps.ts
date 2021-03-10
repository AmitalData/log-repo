import * as ShipmentActions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { EventDetails } from 'cypress/models/EventDetails';
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { ShipmentSelectors } from "../../selectors/Selectors";
//#region variables
let shipmentDetails: ShipmentDetails;
let eventDetails: EventDetails
let shipmentNumber: string;
let EventType: string;
let EventNote: string;
//#endregion
//#region Create Direct Shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    ShipmentActions.NavigatesToShipmentsWorkspace()
});
Given("a direct shipment with the following details",
    (dataTable) => {
        shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
        ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
        ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
    });
When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    });
});
//#endregion
//#region Add exception
Given("the initial has exception status is {string}",
    (hasException) => {
        ShipmentActions.OpenShipment(shipmentNumber);
        ShipmentActions.CheckHasException(hasException)
    });
Given("exception event with the following details",
    (dataTable) => {
        eventDetails = dataTable.hashes()[0] as EventDetails;
        EventType = eventDetails.EventType
        EventNote = eventDetails.EventNotes
        ShipmentActions.FillEventDetails(eventDetails)
    });
When("add exception", () => {
    ShipmentActions.AddEvent()
});
Then("the exception should add successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentGetSingle, 200);
});
Then("the exception should appear in events tab", () => {
    ShipmentActions.AssertAddEvent(EventNote)
    ShipmentActions.AssertEventAppearInEventTab(EventType)
});
Then("has exception should change to yes", () => {
    ShipmentActions.RefreshEventTab()
    ShipmentActions.CheckHasException(BaseSelectors.ContainYes)
});
//#endregion
//#region resolve the exception
When("resolve the exception due to {string}", (ExceptionResolvedNote) => {
    EventNote = ExceptionResolvedNote
    ShipmentActions.ClickOnExceptionResolved(ExceptionResolvedNote)
});
Then("the exception should resolve successfully", () => {
    ShipmentActions.AssertExceptionResolved(EventNote)
});
Then("resolve the exception should appear in events tab", () => {
    ShipmentActions.AssertEventAppearInEventTab(ShipmentSelectors.ExceptionResolved)
});
Then("has exception should change to no", () => {
    ShipmentActions.RefreshEventTab()
    ShipmentActions.CheckHasException(BaseSelectors.ContainNo)
});
//#endregion