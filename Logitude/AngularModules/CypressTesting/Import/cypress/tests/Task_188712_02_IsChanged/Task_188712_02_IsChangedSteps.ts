import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as IsChangedActions1 from '../../actions/IsChangedActions1';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { IsChangedDetails } from '../../models/IsChangedDetails';
import * as Actions from '../../actions/Actions';

//#region Send declaration to customs
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Filter for Correct Draft Declaration", (dataTable) => {
    IsChangedActions1.NavigatesImportDeclarationWorkspace()
    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedActions1.FillIsChanged(isChangedDetails)
});

When("send to customs", () => {
    //let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    Actions.SendToCustomsButtonSimulator()
});
     
     

Then("the declaration should reset", (dataTable) => {

    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    Actions.SendToCustomsButtonSimulator1(isChangedDetails)
});

//#endregion


//#region Change in Cargo Description

Given("the user change the Cargo Description",(dataTable) => {
    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedActions1.ChangeInCargoDescription(isChangedDetails)
});

 When("save the declaration",()  => {
    IsChangedActions1.SaveDeclaretion()

});

Then ("the changes should saved successfully", () => {
    IsChangedActions1.AssertSaveDeclaretion()
 });

//#endregion


//#region Declaration to Payment Screen

When("the screen of Declaration to Payment is open",()  => {
    IsChangedActions1.SendDeclarationToPayment()

});

Then ("the Button Save in the Declaration Payment screen should be disabled successfully {string}", (condition) => {
    IsChangedActions1.AssertSendDeclarationToPayment(condition)
 });

 

//#endregion

