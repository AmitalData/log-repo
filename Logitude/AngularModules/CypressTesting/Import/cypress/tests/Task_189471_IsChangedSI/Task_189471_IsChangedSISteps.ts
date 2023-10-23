import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as IsChangedSIActions from '../../actions/IsChangedSIActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { IsChangedSIDetails } from '../../models/IsChangedSIDetails';
import { IsChangedDetails } from '../../models/IsChangedDetails';
import * as Actions from '../../actions/Actions';
//var isChangedSIDetails

//#region Send declaration to customs
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace()
});

Given("Filter for Correct Draft Declaration", (dataTable) => {
    
    let isChangedSIDetails = Assists.CreateInstance<IsChangedSIDetails>(dataTable, true);
    IsChangedSIActions.FillIsChanged(isChangedSIDetails)
});

When("send to customs", () => {
    
    Actions.SendToCustomsButtonSimulator()
});
     
     

Then("the declaration should reset", (dataTable) => {

    let isChangedDetails = Assists.CreateInstance<IsChangedDetails>(dataTable, true);
    Actions.SendToCustomsButtonSimulator1(isChangedDetails)
    
}); 

//#endregion


//#region Change in Total SI

Given("the user change the Total SI",(dataTable) => {


    let isChangedSIDetails = Assists.CreateInstance<IsChangedSIDetails>(dataTable, true);
    IsChangedSIActions.ChangeInTotalSI(isChangedSIDetails);
    //IsChangedSIActions.SaveSI()
});


//#endregion


//#region Declaration to Payment Screen

When("the screen of Declaration to Payment is open",()  => {
    IsChangedSIActions.SendDeclarationToPayment()

});

Then ("the Button Save in the Declaration Payment screen should be disabled successfully {string}", (condition) => {
    IsChangedSIActions.AssertSendDeclarationToPayment(condition)
 });

 

//#endregion

