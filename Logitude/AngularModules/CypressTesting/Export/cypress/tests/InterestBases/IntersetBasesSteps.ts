import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as IntersetBasesAction from '../../actions/IntersetBasesAction';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { InterestBasesDetails } from '../../models/InterestBasesDetails';

//#region Create new Interest Base
Given("the user logged in and navigates to Full Accounting workspace", () => {
    cy.Login();
    Actions.NavigatesFullAccounting()
});

Given("an interest base with the following details", (dataTable) => {
    IntersetBasesAction.NavigatesInterestWizerd()
    let interestBasesDetails = Assists.CreateInstance<InterestBasesDetails>(dataTable, true);
    IntersetBasesAction.FillInterestBasesDetails(interestBasesDetails)
});

When("create interest base", () => {
    IntersetBasesAction.CreateInterestBases()
});

Then("the interest base should create successfully", () => {
    IntersetBasesAction.AssertCreateInterestBases()
});
//#endregion

//#region Add new Interest Period
Given("interest period with the following details", (dataTable) => {
    let interestBasesDetails = Assists.CreateInstance<InterestBasesDetails>(dataTable, true);
    IntersetBasesAction.AddInterestPeriods(interestBasesDetails)
});

Then("save interest base", () => {
    IntersetBasesAction.SaveInterestBases()
});

Then("the interest base should update successfully", () => {
    IntersetBasesAction.ASsertSaveInterestBases()
});
//#endregion