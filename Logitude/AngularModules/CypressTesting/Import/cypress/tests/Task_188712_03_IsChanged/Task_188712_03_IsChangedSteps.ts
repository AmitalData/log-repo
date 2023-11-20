import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as IsChangedActions2 from '../../actions/IsChangedActions2';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { IsChangedDetails } from '../../models/IsChangedDetails';
import * as Actions from '../../actions/Actions';

var isChangedDetails;

//#region Send declaration to customs
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()

});

Given("Search for File", (dataTable) => {
    
    isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    IsChangedActions2.FillSearchField(isChangedDetails)
    
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
    IsChangedActions2.ChangeInCargoSerialData(isChangedDetails);
    
});


 When("save the declaration",()  => {
    IsChangedActions2.SaveDeclaretion()

});

Then ("the changes should saved successfully", () => {
    IsChangedActions2.AssertSaveDeclaretion()
 });

//#endregion


//#region Declaration to Payment 

When("the screen of Declaration to Payment is open",()  => {
    IsChangedActions2.SendDeclarationToPayment()

});

Then ("the Button Save in the Declaration Payment screen should be disabled successfully {string}", (condition) => {
    IsChangedActions2.AssertSendDeclarationToPayment(condition)
 });

 

//#endregion

