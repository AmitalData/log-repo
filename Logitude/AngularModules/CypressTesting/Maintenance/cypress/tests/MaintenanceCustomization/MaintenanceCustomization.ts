import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { CustomizationSelectors } from "../../selectors/CustomizationSelectors";
import * as CustomizationActions from "../../actions/CustomizationActions";
import { CustomizationDetails} from "../../models//CustomizationDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as GeneralActions from "../../actions/BaseActions";


let customizationDetailes : CustomizationDetails
let CutomFieldID=Math.random()*100
let currentlyCreated = 0;
const LIMIT = 10; // TODO change when switching to Shipment Custom Fields

//#region open Customization and search for Shipment Module
Given("the user logged in and open Customization in setting menu", (Customization) => {
    cy.Login(true);
    CustomizationActions.NavigatesToSCustomizationWorkspace();
});

When("search for shipment module",() =>{
    CustomizationActions.DefineSearchAssert();
    CustomizationActions.SearchModule();
});

Then("shipment module will appears successfully",() =>{
   CustomizationActions.AssertSearchModule()
});

//#endregion

//#region Create text custom filed 
Given("the user click on Add button to add Text field", () => {
    cy.wait(1000)
    cy.Click(CustomizationSelectors.AddCustomField,null,true);
   // cy.get('.EditableGrid').find('.LogCellTemplate').then((matching) => {
      //  currentlyCreated = matching.length / 4;
    });
});

Given("a text field with the following details",(dataTable) =>{
    customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);

    if (currentlyCreated === LIMIT) {
        CustomizationActions.FillCustomFieldDetailes(customizationDetailes,CutomFieldID, "Text", 0, null)
        // edit
    } else {
        CustomizationActions.FillCustomFieldDetailes(customizationDetailes,CutomFieldID, "Text", 0, null)
    }
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
    CustomizationActions.DefineCreateFieldAssert();
   
});

When("create text custome field",() =>{

});

Then("the text custom field should create successfully",() =>{
    CustomizationActions.AssertCreateField();
});
//#endregion

//#region open Add new custom fields with Code already exist

Given("the user click on Add button to add new field with Code already exist",()=> {
    cy.wait(1000)
    cy.Click(CustomizationSelectors.AddCustomField,null,true);

});


Given("a field with the following details already exists",(dataTable)=> {
  customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
  CustomizationActions.FillCustomFieldDetailes(customizationDetailes,CutomFieldID, "Text", 1, null)

});

When("create custome field",() =>{
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("a validation error message with {string} should appear",(validationMessage)=> {
    BaseAssertion.AssertElementContain(BaseSelectors.ValidationSummary,validationMessage);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel,true);
    

});
//#endregion

//#region Add new custom fields with Boolean Type

Given("the user click on Add button to add boolean field", () => {
    cy.wait(1000)
    cy.Click(CustomizationSelectors.AddCustomField,null,true);
});

Given("a boolean field with the following details",(dataTable) =>{
    customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
    CustomizationActions.FillCustomFieldDetailes(customizationDetailes,CutomFieldID,"Boolean",2,null)
    
});

When("create boolean custome field",() =>{
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK,true);
    // CustomizationActions.DefineCreateFieldAssert();


});

Then("the boolean custom field should create successfully",() =>{
    // CustomizationActions.AssertCreateField();


});
//#endregion

//#region Add new custom fields with Decimal Type
Given("the user click on Add button to add Decimal field", () => {
    cy.wait(1000)
    cy.Click(CustomizationSelectors.AddCustomField,null,true);
});

Given("a decimal with the following details",(dataTable) =>{
    customizationDetailes = Assists.CreateInstance<CustomizationDetails>(dataTable, true);
    CustomizationActions.FillCustomFieldDetailes(customizationDetailes,CutomFieldID,"Decimal",3, null)
    
});

When("create Decimal custome field",() =>{
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK,true);
    // CustomizationActions.DefineCreateFieldAssert();


});

Then("the Decimal custom field should create successfully",() =>{
    // CustomizationActions.AssertCreateField();
});
//#endregion

//#region Add custom filed to shipment general screen 
Given("the user click on screen layout button", () => {
    cy.wait(1000)
    cy.Click(CustomizationSelectors.ScreenLayout,null,true);
});

Given("search for custom field to add ",(dataTable) =>{
  //  GeneralActions.AssertSearch();
    
});

When("click on custom field",() =>{
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK,true);

});

Then("it should add successfully to the screen ",() =>{
   
});

//#endregion