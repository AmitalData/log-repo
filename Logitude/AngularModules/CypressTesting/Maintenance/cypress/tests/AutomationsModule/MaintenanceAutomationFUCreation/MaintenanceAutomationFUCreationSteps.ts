import { Given, When, Then, And} from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { AutomationsDetails } from "../../../models/AutomationsModuleDetails/AutomationsDetails"
import * as AutomationsActions from "../../../actions/AutomationsModuleActions/AutomationsActions";
import { AutomationsSelectors } from "../../../selectors/AutomationsModulesSelectors/AutomationsSelectors"
import { ConditionsDetails } from "cypress/models/AutomationsModuleDetails/ConditionsDetails";
import { AutomationsConstants } from "../../../constants/AutomationsConstants/AutomationsConstants";
import { SetFieldValueResultDetails } from "cypress/models/AutomationsModuleDetails/SetFieldValueResultDetails";
import { FollowUpCreationDetails } from "cypress/models/AutomationsModuleDetails/FollowUpCreationDetails";
import * as ShipmentActions from "../../../../../Shipment/cypress/actions/Actions";
import { ShipmentSelectors } from "../../../../../Shipment/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentDetails } from "../../../../../Shipment/cypress/models/ShipmentDetails";
import { BaseSelectors } from "../../../../../Base/cypress/selectors/BaseSelectors";

//#region variables
let automationsDetails: AutomationsDetails
let shipmentDetails: ShipmentDetails;
let conditionsDetails:ConditionsDetails[];
let followUpCreationDetails:FollowUpCreationDetails;
//#endregion

//#region Create new automation 
Given("the user logged in", () => {
    cy.Login()
});

Given("the user navigates to automations menu", () => {
    AutomationsActions.OpenAutomationMenu()
});

Given("the user select {string} as entity", (entity) => {
        cy.get("[data-cy='"+entity+"']").click()  
});
Given("the user add automation {string} with automation detailes as following", (onActionType,dataTable) => { 
    automationsDetails = Assists.CreateInstance<AutomationsDetails>(dataTable, true);
    AutomationsActions.AddNewAutomation(onActionType,automationsDetails)
});
Given("the user add the following condition", (dataTable) => {
    conditionsDetails  = Assists.CreateSet<ConditionsDetails>(dataTable); 
    AutomationsActions.AddConditions(conditionsDetails)

});

When("create automation", () => {
    AutomationsActions.addAutomation()
});

Then("the new automation should create successfully", () => {
    AutomationsActions.AssertAddAutomation()
});

Given("the user adds follow up creation result", (dataTable) => {
    followUpCreationDetails = Assists.CreateInstance<FollowUpCreationDetails>(dataTable, true);
    AutomationsActions.addResultFUCreation(followUpCreationDetails)
});


//#endregion

//#region Scenario: Edit an automation

Given("choose the first automation from {string}", (onActionType) => {
    AutomationsActions.chooseFirstAutomation(onActionType)
    
});

Given("inactive the automation", () => {
    AutomationsActions.inactivateAutomation()
});

When("update automation", () => {
    AutomationsActions.saveAutomation()
});

Then("the automation should updated successfully", () => {
    AutomationsActions.AssertsaveAutomation()
});
//#endregion

//#region Create direct export air shipment Given steps
Given("the user navigates to shipments workspace", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});
//#endregion
//#region Actions steps
When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});
//#region Update general tab given step
Given("the user fill {string} as ValueOfGoods and {string} as a MoveType", (ValueOfGoods, MoveType) => {
    ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber);
    ShipmentActions.FillGeneralTab(ValueOfGoods, MoveType)
});
//#endregion
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
Then("the automation with follow up creation result should executed successfully", () => {
    AutomationsActions.AssertFUCAutomationExecution(followUpCreationDetails,automationsDetails.Name)
});
Then("the automation shouldn't executed", () => {
   AutomationsActions.assertAutomationNotExecuted(automationsDetails.Name)
});

//#endregion

