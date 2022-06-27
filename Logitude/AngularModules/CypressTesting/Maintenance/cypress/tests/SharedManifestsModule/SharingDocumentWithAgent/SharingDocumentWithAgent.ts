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
import { MainCarriageLegDetails } from "cypress/models/SharedManifestsDetails/MainCarriageLegDetails";

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentDetails: ShipmentDetails;
let MAWB: string;
let TransportMode: string;
let code = null
let Secondcode = null
let DocumentName: string;
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
    cy.log(code + " Agent code")
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("the user navigates to shipments", () => {
    cy.log(code + " Agent code")
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details", (dataTable) => {
    const shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    MasterShipmentDetails = shipmentDetails;
    MasterShipmentDetails.Agent = code;
    TransportMode = shipmentDetails.TransportMode
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
When("the user try to sharing manifest in a shipment with no MAWB number", () => {
    ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
    SharedManifestsActions.ShareManifestAction()
});
Then("the following validation appears {string}", (ErrorMessage) => {
    SharedManifestsActions.ValidateShareManifestErrorMessage(ErrorMessage)
});
Given("the user fill master number for the shipment, MainCarriageCarrier,and remove the consignee", (dataTable) => {
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLegDetails>(dataTable, true);
    MAWB = mainCarriageLeg.masterNumber;
    SharedManifestsActions.UpdateShipment(mainCarriageLeg);
});
When("the user save the shipment", () => {
    SharedManifestsActions.SaveMasterShipment()
});
Then("the master should updated successfully", () => {
    SharedManifestsActions.AssertSaveMasterShipment()
});

Given("the user add consignee", () => {
    SharedManifestsActions.AddConsignee(code)
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
    SharedManifestsActions.SignOut()

});


Given("the user logged in to another tenant and open {string} in maintenance menu", (maintenanceItemName) => {
    SharedManifestsActions.LoginSecondTenant()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.AgentMaintenanceItem)
});
When("Accept Invitation from Shared Logistics with the shared key from the previouse agent", () => {
    SharedManifestsActions.FillSharedKey()
    SharedManifestsActions.AcceptInvitation()
});
Then("the Invitation should accepted successfully", () => {
    SharedManifestsActions.AssertAcceptInvitation()
    SharedManifestsActions.SignOut()
});

Given("the user logged in to the first tenant and open the created shipment", () => {
    cy.Login(true)
    ShipmentActions.NavigatesToShipmentsWorkspace()
    ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
});
When("the user shares the manifest from a shipment", () => {
    SharedManifestsActions.ShareManifestAction()
    SharedManifestsActions.ShareManifest()
});
Then("the Manifest should shared successfully", () => {
    SharedManifestsActions.AssertShareManifest()
});

Given("the user logged in to the second tenant", () => {

    SharedManifestsActions.LoginSecondTenant()

});
Given("the user navigates to operations", () => {
    SharedManifestsActions.ToOperations()
});
Given("chooses shared manifests tab", () => {
    SharedManifestsActions.ToSharedManifests()
});

When("chooses Air Manifests and search by the manifest number", () => {
    SharedManifestsActions.SearchManifest(TransportMode, ShipmentContext.MasterNumber)
});
Then("the shared shipment should exist with same details as we send from the first agent side", () => {
    SharedManifestsActions.AssertManifestExist(MAWB)
});

When("the user create the shipment with {string} as carrier", (MasterMainCarriageCarrier) => {
    SharedManifestsActions.CreateShipment(ShipmentContext.MasterNumber, MasterMainCarriageCarrier)
});
Then("an import shipment should be created", () => {
    SharedManifestsActions.AssertCreateShipment()
    SharedManifestsActions.SignOut()
});


//#region check shared manifests from the second tenant
Given("the user logged in to the First tenant", () => {
    cy.Login(true)
});

Given("Open Shared Logistics Module and open Documents Permissions", () => {
    SharedManifestsActions.OpenSharedLogistic()
});
When("Open Agent View and search for {string}", (documentName) => {
    DocumentName = documentName;
    SharedManifestsActions.SearchForAgentDocument(DocumentName)
});

Then("check master checkbox as true and Save", () => {
    SharedManifestsActions.EditPermissions()
});

//#endregion

//#region User Build Document "Air Manifest"

Given("the user enter the shipment", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
    ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
});

When("Open Document Out tab and search for {string}", (documentName) => {
    SharedManifestsActions.OpenDocTab(DocumentName)
});
Then("Build the document", () => {
    SharedManifestsActions.BuildDocument()

});
//#endregion

//#region Share Document With Agent
When("the user click on share Document Option",() => {
    SharedManifestsActions.ShareDocument()
});
Then("The document will Shared successfully", () => {
    SharedManifestsActions.ClickShare()

});
//#endregion

//#region Share Document from shipment successfully
Given("the user logged in to the Second tenant and open the shipment", () => {
    SharedManifestsActions.LoginSecondTenant()
    ShipmentActions.NavigatesToShipmentsWorkspace()

});

When("the user open DocIn Tab {string}",(documentName) => {
    ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
    SharedManifestsActions.OpenDocInTab(DocumentName)

});
Then("the Document should be shared successfully", () => {
    SharedManifestsActions.UploadDoc()

});
//#endregion

Given("the user logged in to the first tenant and open the created shipment", () => {
    SharedManifestsActions.SignOut()

});
Then("delete Master number", () => {
 //   cy.Login(true)
   // ShipmentActions.NavigatesToShipmentsWorkspace()
   // ShipmentActions.OpenShipment(ShipmentContext.MasterNumber);
    SharedManifestsActions.clearMasterNumber()
    cy.get('#Shipment-Save').click({ force: true })
});