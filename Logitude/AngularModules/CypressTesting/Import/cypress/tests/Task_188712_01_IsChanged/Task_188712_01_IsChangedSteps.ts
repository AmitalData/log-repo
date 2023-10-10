import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as IsChangedAction from '../../actions/IsChangedActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { IsChangedDetails } from '../../models/IsChangedDetails';
import * as Actions from '../../actions/Actions';

//#region Send declaration to customs
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});
debugger
Given("Filter for Correct Draft Declaration", (dataTable) => {
    IsChangedAction.NavigatesImportDeclarationWorkspace()
    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedAction.FillIsChanged(isChangedDetails)
    
});

// מדמה שליחה כדי לאפס את השדה IsChanged
When("send to customs", () => {
    //let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    Actions.SendToCustomsButtonSimulator()
});
     
     

Then("the declaration should reset", (dataTable) => {

    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    Actions.SendToCustomsButtonSimulator1(isChangedDetails)
});
  

//#endregion


//#region Change in Declaration Office Code

Given("the user change the Declaration Office Code",(dataTable) => {
    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedAction.ChangeInDeclarationOfficeCode(isChangedDetails)
});

 When("save the declaration",()  => {
    IsChangedAction.SaveDeclaretion()

});

Then ("the changes should saved successfully", () => {
    IsChangedAction.AssertSaveDeclaretion()
 });

//#endregion


//#region Declaration to Payment Screen

When("the screen of Declaration to Payment is open",()  => {
    IsChangedAction.SendDeclarationToPayment()

});

Then ("the Button Save in the Declaration Payment screen should be disabled successfully {string}", (condition) => {
    IsChangedAction.AssertSendDeclarationToPayment(condition)
 });

 

//#endregion

