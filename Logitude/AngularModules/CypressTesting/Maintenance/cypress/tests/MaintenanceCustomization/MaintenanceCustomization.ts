import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { CustomizationSelectors } from "../../selectors/CustomizationSelectors";
import * as CustomizationActions from "../../actions/CustomizationActions";
import { CustomizationDetails } from "../../models//CustomizationDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

let customizationDetailes: CustomizationDetails
let CutomFieldID = Math.random() * 100
let currentlyCreated = 0;
const LIMIT = 4; // TODO change when switching to Shipment Custom Fields
const CELLS_PER_ROW = 10;


//#region open Customization and search for Shipment Module
Given("the user logged in and open Customization in setting menu", (Customization) => {
    cy.Login(true);
    CustomizationActions.NavigatesToSCustomizationWorkspace();
});

When("search for shipment module", () => {
    CustomizationActions.DefineSearchAssert();
    CustomizationActions.SearchModule();
});

Then("shipment module will appears successfully", () => {
    CustomizationActions.AssertSearchModule()
});
//#endregion

//#region Create text custom filed 
Given("the user click on Add button to add Text field", () => {
    cy.wait(1000);
    let documentResult = null;
    cy.document().then(($document) => {
        documentResult = $document.getElementsByClassName('LogCellTemplate').length
        currentlyCreated = documentResult / CELLS_PER_ROW;
    });
});

Given("a text field with the following details", (dataTable) => {
    customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
    CustomizationActions.EditOrCreateCustomField(currentlyCreated >= LIMIT, CutomFieldID, customizationDetailes)
});

When("create text custome field", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK, true);
});

Then("the text custom field should create successfully", () => {
    if (currentlyCreated < LIMIT)
        CustomizationActions.AssertCreateField();
});
//#endregion

//#region open Add new custom fields with Code already exist

Given("the user click on Add button to add new field with Code already exist", () => {
    if (currentlyCreated < LIMIT) {
        cy.wait(1000)
        cy.Click(CustomizationSelectors.AddCustomField, null, true);
    }
});


Given("a field with the following details already exists", (dataTable) => {
    if (currentlyCreated < LIMIT) {
        customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
        CustomizationActions.FillCustomFieldDetailes(customizationDetailes, CutomFieldID, "Text", 1, null)
    }
});

When("create custome field", () => {
    if (currentlyCreated < LIMIT) {
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
    }
});

Then("a validation error message with {string} should appear", (validationMessage) => {
    if (currentlyCreated < LIMIT) {
        BaseAssertion.AssertElementContain(BaseSelectors.ValidationSummary, validationMessage);
        cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel, true);
    }
});
//#endregion

//#region Add new custom fields with Boolean Type

Given("the user click on Add button to add boolean field", () => {
    if (currentlyCreated < LIMIT) {
        cy.wait(1000)
        cy.Click(CustomizationSelectors.AddCustomField, null, true);
    }
});

Given("a boolean field with the following details", (dataTable) => {
    if (currentlyCreated < LIMIT) {
        customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
        CustomizationActions.FillCustomFieldDetailes(customizationDetailes, CutomFieldID, "Boolean", 2, null)
    }
});

When("create boolean custome field,it will create successfully", () => {
    if (currentlyCreated < LIMIT) {
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK, true);
        CustomizationActions.DefineCreateFieldAssert();
    }
});
//#endregion

//#region Add new custom fields with Decimal Type
Given("the user click on Add button to add Decimal field", () => {
    if (currentlyCreated < LIMIT) {
        cy.wait(1000)
        cy.Click(CustomizationSelectors.AddCustomField, null, true);
    }
});

Given("a decimal with the following details", (dataTable) => {
    if (currentlyCreated < LIMIT) {
        customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
        CustomizationActions.FillCustomFieldDetailes(customizationDetailes, CutomFieldID, "Decimal", 6, null)
    }
});

When("create Decimal custome field,it will create successfully", () => {
    if (currentlyCreated < LIMIT) {
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK, true);
        CustomizationActions.DefineCreateFieldAssert();
    }
});

//#endregion
