import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../../actions/Actions";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import * as ShipmentActions from "../../../../../Shipment/cypress/actions/Actions";
import { ShipmentSelectors } from "../../../../../Shipment/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentDetails } from "../../../../../Shipment/cypress/models/ShipmentDetails";
import { BaseSelectors } from "../../../../../Base/cypress/selectors/BaseSelectors";
import { CustomizationRuleDetails } from "cypress/models/CustomizationModuleDetails/CustomizationRuleDetails";
import * as CustomizationActions from "../../../actions/CustomizationModuleActions/CustomizationModuleActions";
import { CustomizationConstants } from "../../../constants/CustomizationConstants/CustomizationConstants";


//#region variables
let customizationRuleDetails:CustomizationRuleDetails;
let shipmentDetails: ShipmentDetails;
//#endregion

//#region Create new Rules 
Given("the user logged in and choose customization", () => {
    cy.Login()
    CustomizationActions.ChooseCustomization();
});

Given("the user choose {string} Object", (object) => {
    CustomizationActions.ChooseObjectTable(object)
});

Given("the user clicks on Rules", () => {
    CustomizationActions.DisplyRules()
});

Given("adds new rule with the following details", (dataTable) => {
    customizationRuleDetails  = Assists.CreateInstance<CustomizationRuleDetails>(dataTable, true); 
    customizationRuleDetails.Code=CustomizationActions.GetTodayDateTime()+customizationRuleDetails.Code
    CustomizationActions.AddRule(customizationRuleDetails)
    
});

When ("create rule", () => {
    CustomizationActions.CreateNewRule()
});
Then ("the rule should created successfully", () => {
    CustomizationActions.AssertCreateNewRule()
});

//Scenario area: Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
    ShipmentActions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

//Scenario area:Update shipment to verify the rule exection
Given("the user fill {string} as ValueOfGoods and {string} as a MoveType", (ValueOfGoods, MoveType) => {
    ShipmentActions.OpenShipment(shipmentDetails.ShipmentNumber);
    ShipmentActions.FillGeneralTab(ValueOfGoods, MoveType)
});
When("update shipment", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the rule with required field result should executed successfully and show the {string}", (ValidationMessage) => {
    CustomizationActions.AssertRuleExection(ValidationMessage)
     
});

//Scenario area:Search and edit the created New Rule From Customization
Given ("the user navigate to customization", () => {
    CustomizationActions.ChooseCustomization();
});

And ("search the rule already created", () => {
    CustomizationActions.SearchRule(customizationRuleDetails.Code)
    
});
When ("inactivate the rule and save", () => {
    CustomizationActions.InactivateRule()
    CustomizationActions.SaveRule()
});
Then ("the rule should saved successfully", () => {
    CustomizationActions.AssertSaveRule()
});