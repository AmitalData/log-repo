import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseActions from '../../../../Base/cypress/actions/Actions';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as BIActions from '../../actions/BIActions';
import { BIfolderDetails } from '../../models/BIfolderDetails';
import { BIReportDetails } from '../../models/BIReportDetails';


let BIfolderDetails: BIfolderDetails;
let BIReportDetails: BIReportDetails;
//#region To Create BI folder
Given("the user logged in and navigates to BI Report workspace", () => {
    cy.Login();
    BIActions.NavigatesBIReportWorkspace()
});

Given("a BI folder with the following details", (dataTable) => {
    const BIfolder = Assists.CreateSet<BIfolderDetails>(dataTable);
});

When("create BI folder", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the folder will created successfully", () => {
    BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});