import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentDetails: ShipmentDetails;
let EventNote ; 
let shipmentNumber: string;

//#endregion

//#region Create master export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details",(dataTable)=>{
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
Given("the user in the master's Shipment tab",()=>{
    Actions.OpenShipment(ShipmentContext.MasterNumber);
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
});

When("create house with {string} as Shipper",(Shipper)=>{
    Actions.CreateNewAttachedHouse(Shipper);
});

Then("the house should create successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.HouseNumber= interception.response.body.House;
    })
});

Then("the house should connect successfully",()=>{
    Actions.CheckBusyIndicator()
    Actions.ValidateCheckHouseCheckBox();
});
//#endregion
When("cancel the master shipment with {string} Note", (note) => {
    EventNote = note
    Actions.CancelShipment(note);
  });
  Then("the house should Cancel successfully", () => {
    let HouseNumber = Actions.GetHouseNumber().toString();
    cy.get('.HyperlinkButtonControl').contains(HouseNumber).Click
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    Actions.ValidateCancelIconExist(true);
    Actions.ValidateShipmentEventActions(ShipmentSelectors.EventsTab,EventNote);
    Actions.ValidateShipmentFields(true);
  }); 
  When("reactivate the shipment with {string} Note",(note)=>{
  EventNote = note
  Actions.ReactiveShipment(note);
})

Then("the house should Reactivate successfully",()=>{
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  Actions.ValidateCancelIconExist(false);
  Actions.ValidateShipmentEventActions(ShipmentSelectors.EventsTab,EventNote);
  Actions.ValidateShipmentFields(false);

})
