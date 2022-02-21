import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
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
});

Given("adds new rule with the following details", (dataTable) => {
    customizationRuleDetails  = Assists.CreateInstance<CustomizationRuleDetails>(dataTable); 
    
});

When ("create rule", () => {
});
Then ("the rule should created successfully", () => {
});