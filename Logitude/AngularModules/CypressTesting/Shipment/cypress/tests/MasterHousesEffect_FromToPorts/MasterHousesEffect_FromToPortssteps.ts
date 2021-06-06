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
Given("the user in the master's rounting tab",()=>{
    //Actions.OpenShipment(ShipmentContext.MasterNumber);
    cy.Navigate(ShipmentSelectors.RoutingsTab);
})