import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../../actions/Actions";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import * as ShipmentActions from "../../../../../Shipment/cypress/actions/Actions";
import { ShipmentSelectors } from "../../../../../Shipment/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentDetails } from "../../../../../Shipment/cypress/models/ShipmentDetails";
import { BaseSelectors } from "../../../../../Base/cypress/selectors/BaseSelectors";
import * as CustomizationActions from "../../../actions/CustomizationModuleActions/CustomizationModuleActions";
import { CustomizationConstants } from "../../../constants/CustomizationConstants/CustomizationConstants";
import { CustomizationScreenLayoutDetails } from "cypress/models/CustomizationModuleDetails/CustomizationScreenLayoutDetails";
import '@4tw/cypress-drag-drop'

//#region variables

let shipmentDetails: ShipmentDetails;
let objectTable: string
//#endregion

//#region Create new Rules 
Given("the user logged in and choose customization", () => {
    cy.Login()
    CustomizationActions.ChooseCustomization();
});

Given("the user choose {string} Object", (object) => {
    CustomizationActions.ChooseObjectTable(object)
    objectTable=object
});

Given("the user clicks on Custom Fields", () => {
    CustomizationActions.DisplyCustomFields()
});
Given("get first two fields", () => {
    CustomizationActions.GetCustomFields(objectTable)

});

When ("drag and drop the following details in shipment general tab and save", (dataTable) => {
    CustomizationActions.GoToScreenLayout()
    let fieldDetails = Assists.CreateSet<CustomizationScreenLayoutDetails>(dataTable);
    CustomizationActions.DragAndDropFields(fieldDetails);
    CustomizationActions.SaveScreenLayout() 
});

Then ("the fields should saved successfully to the screen layout", () => {
    CustomizationActions.AssertSaveScreenLayout()
});


//#region Create direct export air shipment Given steps
Given("the user navigates to shipments workspace", () => {
    cy.Login()
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
Given("the user open the shipment and go to the general tab", (field1, field2) => {
    ShipmentActions.OpenShipment("7545"/*shipmentDetails.ShipmentNumber*/);
    cy.Click(ShipmentSelectors.GeneralTab, null)
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


Then("the custom fields should be added successfully", () => {
   CustomizationActions.assertCustomFieldsExist()
});

Given("the user choose customization", () => {

    CustomizationActions.ChooseCustomization();
});

When ("delete the following fields details from shipment general tab and save", (dataTable) => {
    CustomizationActions.GoToScreenLayout()
    let fieldDetails = Assists.CreateSet<CustomizationScreenLayoutDetails>(dataTable);
    CustomizationActions.RemoveFromScreenLayout(fieldDetails);
    CustomizationActions.SaveScreenLayout() 
});

Then ("the fields should removed successfully from the screen layout", () => {
    CustomizationActions.AssertSaveScreenLayout()
});