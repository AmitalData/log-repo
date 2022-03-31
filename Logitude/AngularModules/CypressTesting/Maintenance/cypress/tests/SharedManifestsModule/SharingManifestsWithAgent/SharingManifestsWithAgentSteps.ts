import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import * as SharedManifestsActions from "../../../actions/SharedManifestsActions/SharedManifestsActions";
import * as Actions from "../../../actions/Actions";
import { BaseSelectors } from "../../../../../Base/cypress/selectors/BaseSelectors";
import { MaintenanceSelectors } from "../../../selectors/Selectors";
import { DocumentsPermissionsDetails } from "../../../models/SharedManifestsDetails/DocumentsPermissionsDetails"
import * as ShipmentActions from "../../../../../Shipment/cypress/actions/Actions";
import { ShipmentSelectors } from "../../../../../Shipment/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentDetails } from "../../../../../Shipment/cypress/models/ShipmentDetails";
import { ContactDetails } from "../../../models/ContactDetails";
import { CardDetails } from "../../../models/CardDetails";
import { Constants } from "../../../constants/Constants";
import * as MaintenanceBaseActions from "../../../actions/BaseActions"
import { ShipmentContext } from '../../../../../Shipment/cypress/models/ShipmentContext';

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentDetails: ShipmentDetails;
let code = null
let Secondcode = null
//#endregion

//#region Create new Agent
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login(true)
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.AgentMaintenanceItem)
});

Given("an agent with the following details", (dataTable) => {
    let agentDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    Actions.OpenNewWizard("Agent");
    Actions.FillCardDetails(agentDetails, null)
});

Given("an agent contact with the following details", (dataTable) => {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    Actions.FillCardContactDetails(contactDetails)
});

When("create agent", () => {
    Actions.CreateCard();
});

Then("the agent should create successfully", () => {
    Actions.AssertCreateCard(Constants.Agent)

});
//#endregion

//#region Create master export air shipment
Given("the user navigates to shipments workspace", () => {
    code = Actions.getCardCode()
    cy.log(code+" Agent code")
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details", (dataTable) => {
    const shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    MasterShipmentDetails = shipmentDetails;
    MasterShipmentDetails.Agent=code;
    ShipmentActions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion
When ("the user try to sharing manifest in a shipment with no MAWB number", () => {
    ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
    SharedManifestsActions.ShareManifestAction()
});
Then ("the following validation appears {string}", (ErrorMessage) => {
    SharedManifestsActions.ValidateShareManifestErrorMessage(ErrorMessage)
});

Given ("the user fill {string} as master number for the shipment and remove the consignee", (masterNumber) => {
    SharedManifestsActions.UpdateShipment(masterNumber)
});
When ("the user save the shipment", () => {
    SharedManifestsActions.SaveMasterShipment()
});
Then ("the master should updated successfully", () => {
   SharedManifestsActions.AssertSaveMasterShipment()
});

When ("the user try to sharing manifest in a shipment with no consignee", () => {
    SharedManifestsActions.ShareManifestAction()
 });
 Given ("the user add consignee", () => {
    SharedManifestsActions.AddConsignee(code)
 });
 When ("the user try to sharing manifest in a shipment Agent who doesn't have sharing accept", () => {
    SharedManifestsActions.ShareManifestAction()

 });

 //#region Search for the agent by code
When("search agent", () => {
    SharedManifestsActions.ExitShipment()
    Actions.OpenMaintenanceItemFromMaintenanceMenu('Agent', MaintenanceSelectors.AgentMaintenanceItem)
    code = Actions.getCardCode()
    MaintenanceBaseActions.Search(code)
});

When("search the second agent", () => {
    Secondcode = Actions.getCardCode()
    MaintenanceBaseActions.Search(Secondcode)
});

Then("the agent should appear successfully", () => {
    MaintenanceBaseActions.AssertSearch(code);
});
Then("the second agent should appear successfully", () => {
    MaintenanceBaseActions.AssertSearch(Secondcode);
});
//#endregion

//#region Open the agent
When("open agent", () => {
    Actions.OpenCard(Constants.Agent)
});

Then("the agent should open successfully", () => {
    Actions.AssertOpenCard()
});
//#endregion


When("send Invitation from Shared Logistics to {string}", (email) => {
    SharedManifestsActions.FillInvitation(email)
    SharedManifestsActions.SendInvitation()
});
Then("the Invitation should sent successfully", () => {
    SharedManifestsActions.AssertSendInvitation()
    SharedManifestsActions.GetSharedKey()
     /** SHOULD includeed BEFORE MERGE TO MASTER  */
    SharedManifestsActions.SignOut()
    
});


 Given("the user logged in to another tenant and open {string} in maintenance menu", (maintenanceItemName) => {
     /** SHOULD CHANGED BEFORE MERGE TO MASTER  */
    SharedManifestsActions.LoginSecondTenant() 
    //cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.AgentMaintenanceItem)
});
When ("Accept Invitation from Shared Logistics with the shared key from the previouse agent", () => {
    SharedManifestsActions.FillSharedKey()
    SharedManifestsActions.AcceptInvitation()
});
Then ("the Invitation should accepted successfully", () => {
    SharedManifestsActions.AssertAcceptInvitation()
});

Given ("the user logged in to the first tenant and open the created shipment", () => {
     /** SHOULD CHANGED BEFORE MERGE TO MASTER  */
    cy.Login(true)
    //cy.Login()
    ShipmentActions.NavigatesToShipmentsWorkspace()
    ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
 });
When ("the user shares the manifest from a shipment", () => {
    SharedManifestsActions.ShareManifestAction()
    SharedManifestsActions.ShareManifest()
 });
Then ("the Manifest should shared successfully", () => {
    SharedManifestsActions.AssertShareManifest()
 });

 Given("the user logged in to the second tenant", () => { 
   /** SHOULD CHANGED BEFORE MERGE TO MASTER  */
    SharedManifestsActions.LoginSecondTenant() 
    //cy.Login()
});
Given("the user navigates to operations", () => { 
    SharedManifestsActions.ToOperations()
});
Given("chooses shared manifests tab", () => { 
    SharedManifestsActions.ToSharedManifests()
});







//#region Create direct export air shipment Given steps
Given("the user navigates to shipments workspace", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    shipmentDetails.Agent=code
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});
//#endregion

//#region Update general tab given step
Given("the user fill {string} as ValueOfGoods and {string} as a MoveType", (ValueOfGoods, MoveType) => {
    ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber);
    
});
//#endregion

           
//#region ShipmentActions steps
When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("update shipment", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion

//#region Assert steps
Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Then("the direct should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion