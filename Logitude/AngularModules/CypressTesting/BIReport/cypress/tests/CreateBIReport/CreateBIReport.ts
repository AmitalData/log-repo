import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseActions from '../../../../Base/cypress/actions/Actions';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as BIActions from '../../actions/BIActions';
import { BIfolderDetails } from '../../models/BIfolderDetails';
import { BIReportDetails } from '../../models/BIReportDetails';
import * as gr from '../../../../Base/cypress/actions/GenerateRandoms';


let BifolderDetails: BIfolderDetails;
let BiReportDetails: BIReportDetails;
let BIReportFolderName: string;
//#region To Create BI folder
Given("the user logged in and navigates to BI Report workspace", () => {
    cy.Login();
    BIActions.NavigatesBIReportWorkspace()
});

Given("a BI folder with the following details", (dataTable) => {
    let BifolderDetails = Assists.CreateInstance<BIfolderDetails>(dataTable);
    var RandomBIReportFolderName = gr.GenerateRandomNumberAndString(4);
    BifolderDetails.Name = BifolderDetails.Name + RandomBIReportFolderName;
    BIReportFolderName = BifolderDetails.Name
    BIActions.FillBIFoldertDetails(BifolderDetails)
});

When("create BI folder", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the folder will created successfully", () => {
  //  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});


//#region to Create Bi Report
Given("User open the folder to add BI Report", () => {
    BIActions.SearchBIFolder(BIReportFolderName)
});

Given("a Shipment BI Report with the following details", (dataTable) => {
    let BiReportDetails = Assists.CreateInstance<BIReportDetails>(dataTable);
    var RandomBIReportName = gr.GenerateRandomNumberAndString(4);
    BiReportDetails.Name = BiReportDetails.Name + RandomBIReportName;
    BIActions.FillBIReportDetails(BiReportDetails)
});

When("Create BI Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the Report will created successfully", () => {
  //  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});


