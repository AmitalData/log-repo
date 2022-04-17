import * as Actions from "../../actions/Actions";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentDetails: ShipmentDetails;
let EventNote;
let shipmentNumber: string;
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

When("create master", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion
//#region Create house export air shipment inside the master
Given("the user in the master's rounting tab", () => {
    Actions.OpenShipment(ShipmentContext.MasterNumber);
    cy.Navigate(ShipmentSelectors.RoutingsTab);
});


Given("edit main carriage leg with the following details", (dataTable) => {
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
    cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
});
When("update master", () => {
   Actions.UpdateMaster()
    // Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});


Then("the master should update successfully", () => {
   Actions.AsserationMasterUpdateRoutind()
      
});
//#region Create house export air shipment inside the master

Given("the user in the master's Shipment tab", () => {

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
});
//#endregion
When("close the master shipment Operationally", () => {

    Actions.OperationalCloseShipment();
});



Then("the master should close operationally successfully", () => {
    
    Actions.ValidateCloseShipmentFields(true);
});
Then("the connected house should close operationally successfully",()=>{
    Actions.openHouseShipment()
   // Actions.ValidateShipmentHouseFields(true)
   Actions.ValidateShipmentHouseCloseoperationally(true)
})
When("reopen master operationally with {string} Note",(note)=>{
    EventNote = note
    Actions.RepoenShipment(note);
});
Then('the master should reopen successfully',()=>{
    cy.BackButton('Shipment')
    Actions.ValidateCloseShipmentFields(false);
})
Then('the connected house should reopen successfully',()=>{
    Actions.openHouseShipment()
   Actions.ValidateShipmentHouseFieldsReactive(false)
   //Actions.ValidateShipmentHouseCloseoperationally(false)

})