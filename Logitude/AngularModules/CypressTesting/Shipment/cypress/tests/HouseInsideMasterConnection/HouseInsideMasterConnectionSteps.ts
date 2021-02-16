import * as Actions from '../../actions/Actions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentNumber: string;
//#endregion

//#region Create master export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details",(dataTable)=>{
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    MasterShipmentDetails = shipmentDetails;
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
  });
  
Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Create house export air shipment inside the master
Given("the user in the master's Shipment tab",()=>{
    Actions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.ShipmentsTab, null);
});

When("create house with {string} as Shipper",(Shipper)=>{
    cy.Click(ShipmentSelectors.NewAttachedHouse,null);
    Actions.FillHouseInShipmentsTab(Shipper);
    Actions.CreateShipment("House");
});

Then("the house should create and connect successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Disconnect the house shipment
When("disconnect shipment",()=>{
    cy.get('#EditComponentBusyIndicator_0').should('not.exist');
    cy.Click("#EditComponentCellId_0_0 > div.MediaFill > table > tr:nth-child(3) > td > div > div.MediaFillAbsolute.CurvedEditArea > table > tr > td:nth-child(2) > div > ng-component:nth-child(3) > div > scrollviewer > div > div > div > ng-component > table > tr:nth-child(1) > td > div > table > tr:nth-child(3) > td > div > div > div.SimpleGridViewBody > table > tr:nth-child(1) > td > table > tr:nth-child(2) > td:nth-child(2) > table > tr > td:nth-child(2) > button",null);
    Actions.ConnectOrDisconnectShipment();
});

Then("the shipment should disconnect successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion