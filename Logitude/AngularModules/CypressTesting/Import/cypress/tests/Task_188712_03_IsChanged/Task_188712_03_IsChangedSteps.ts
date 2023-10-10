import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as IsChangedAction2 from '../../actions/IsChangedActions2';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { IsChangedDetails } from '../../models/IsChangedDetails';
import * as Actions from '../../actions/Actions';


//#region Send declaration to customs
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()

});

Given("Filter for Correct Draft Declaration", (dataTable) => {
   
    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedAction2.FillIsChanged(isChangedDetails)
});

When("send to customs", () => {
    
    Actions.SendToCustomsButtonSimulator()
});
     
     

Then("the declaration should reset", (dataTable) => {

    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    Actions.SendToCustomsButtonSimulator1(isChangedDetails)
});
//#endregion


//#region Change In Cargo Serial Data

Given("the user change the Cargo serial data",(dataTable) => {
    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedAction2.ChangeInCargoSerialData(isChangedDetails)
});

 When("save the declaration",()  => {
    IsChangedAction2.SaveDeclaretion()

});

Then ("the changes should saved successfully", () => {
    IsChangedAction2.AssertSaveDeclaretion()
 });

//#endregion


//#region Declaration to Payment 

When("the screen of Declaration to Payment is open",()  => {
    IsChangedAction2.SendDeclarationToPayment()

});

Then ("the Button Save in the Declaration Payment screen should be disabled successfully {string}", (condition) => {
    IsChangedAction2.AssertSendDeclarationToPayment(condition)
 });

 

//#endregion

