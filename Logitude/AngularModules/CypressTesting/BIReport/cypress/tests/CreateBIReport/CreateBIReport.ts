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
    var RandomBIReportFolderName = gr.GenerateRandomNumberAndString(2);
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

//ShipmentsFact
//#region to Create Bi Report
Given("User open the folder to add Shipment BI Report", () => {
    cy.wait(1000)
    BIActions.SearchBIFolder(BIReportFolderName)
});

Given("a Shipment BI Report with the following details", (dataTable) => {
    let BiReportDetails = Assists.CreateInstance<BIReportDetails>(dataTable);
    var RandomBIReportName = gr.GenerateRandomNumberAndString(1);
    BiReportDetails.Name = BiReportDetails.Name + RandomBIReportName;
    BIActions.FillBIReportDetails(BiReportDetails)
});

When("Create BI Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the Shipment Report will created successfully", () => {
    //  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow);
});

//#region Add Columns and Filters to the Report
Given("User open QueryBuilder and add {string} column and filter to Shipment Report", (ShipmentNumber) => {
    BIActions.AddCoulmnAndFilter(ShipmentNumber)
});

When("add filter and cloumn user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Shipment Report will create successfully with all details", () => {
   
});
//#endregion

//#region Edit The Report
Given("User click on Edit Query to edit and add {string} column and filter to Shipment Report", (Customer) => {
    BIActions.EditCoulmnAndFilter(Customer)
});

When("add new filter and cloumn user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Shipment Report will edit successfully with all details", () => {

});
//#region Download the Report to Excel file
When("User Click On Download To Excel file Button", () => {
});

Then("The Shipment report will download successfully", () => {
});
//#endregion

//ShipmentChargesFact
//#region Add Shipment Charges BI Report to the folder
Given("User open the folder to add Shipment Charges BI Report", () => {
    BIActions.OpenBIFolder();
});

Given("a Shipment Charges BI Report with the following details", (dataTable) => {
    let BiReportDetails = Assists.CreateInstance<BIReportDetails>(dataTable);
    var RandomBIReportName = gr.GenerateRandomNumberAndString(2);
    BiReportDetails.Name = BiReportDetails.Name + RandomBIReportName;
    BIActions.FillBIReportDetails(BiReportDetails)
});

When("Create BI Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the Shipment Charges Report will created successfully", () => {
    
});

//#region Add Columns and Filters to ShipmentCharges Report
Given("User open QueryBuilder and add {string} column and filter to Shipment Charges", (branch) => {
    BIActions.AddCoulmnAndFiltershipmentcharge(branch)
});

When("add filter and cloumn to Shipment Charges user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Shipment Charges Report will create successfully with all details", () => {
   
});
//#endregion

//#region Edit ShipmentCharges Report
Given("User click on Edit Query to edit and add {string} column and filter to Shipment Charges", (Shipper) => {
    BIActions.EditCoulmnAndFiltershipmentcharge(Shipper);
});

When("add new filter and cloumn user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Shipment Charges Report will edit successfully with all details", () => {
  
});
//#endregion


//MasterChargesFact
//#region Add Master BI Report to the folder
Given("User open the folder to add Master BI Report", () => {
    BIActions.OpenMasterBIFolder();
});

Given("a Master BI Report with the following details", (dataTable) => {
    let BiReportDetails = Assists.CreateInstance<BIReportDetails>(dataTable);
    var RandomBIReportName = gr.GenerateRandomNumberAndString(2);
    BiReportDetails.Name = BiReportDetails.Name + RandomBIReportName;
    BIActions.FillBIReportDetails(BiReportDetails)
});

When("Create Master BI Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the Master Report will created successfully", () => {
   
});

//#region Add Columns and Filters to MasterCharges Report
Given("User open QueryBuilder and add {string} column and filter to Master", (Master) => {
    BIActions.AddCoulmnAndFilterMaster(Master)
});

When("add filter and cloumn to Master user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Master Report will create successfully with all details", () => {
   
});
//#endregion

//#region Edit MasterCharges Report
Given("User click on Edit Query to edit and add {string} column and filter to Master", (MasterShipmentNumber) => {
    BIActions.EditCoulmnAndFilterMaster(MasterShipmentNumber);
});

When("add new filter and cloumn user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Master Report will edit successfully with all details", () => {
    
});
//#endregion

//ARInvoiceFact
//#region Add ARInvoices BI Report to the folder
Given("User open the folder to add ARInvoices BI Report", () => {
    BIActions.OpenARInvoicesBIFolder();
});

Given("a ARInvoices BI Report with the following details", (dataTable) => {
    let BiReportDetails = Assists.CreateInstance<BIReportDetails>(dataTable);
    var RandomBIReportName = gr.GenerateRandomNumberAndString(2);
    BiReportDetails.Name = BiReportDetails.Name + RandomBIReportName;
    BIActions.FillBIReportDetails(BiReportDetails)
});

When("Create ARInvoices BI Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the ARInvoices Report will created successfully", () => {
   
});
//#region Add Columns and Filters to ARInvoice Report
Given("User open QueryBuilder and add {string} column and filter to ARInvoices", (ARInvoiceType) => {
    BIActions.AddCoulmnAndFilterARInvoices(ARInvoiceType)
});

When("add filter and cloumn to ARInvoices user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the ARInvoices Report will create successfully with all details", () => {
    //
});
//#endregion

//#region Edit ARInvoice Report
Given("User click on Edit Query to edit and add {string} column and filter to ARInvoices", (InvoiceBranch) => {
    BIActions.EditCoulmnAndFilterARInvoices(InvoiceBranch);
});

When("add new filter and cloumn user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the ARInvoices Report will edit successfully with all details", () => {
});

//QuoteFact
//#region Add Quote BI Report to the folder
Given("User open the folder to add Quote BI Report", () => {
    BIActions.OpenQuoteBIFolder();
});

Given("a Quote BI Report with the following details", (dataTable) => {
    let BiReportDetails = Assists.CreateInstance<BIReportDetails>(dataTable);
    var RandomBIReportName = gr.GenerateRandomNumberAndString(2);
    BiReportDetails.Name = BiReportDetails.Name + RandomBIReportName;
    BIActions.FillBIReportDetails(BiReportDetails)
});

When("Create Quote BI Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
});

Then("the Quote Report will created successfully", () => {
});

//#region Add Columns and Filters to Quote Report
Given("User open QueryBuilder and add {string} column and filter to Quote", (QuoteNumber) => {
    BIActions.AddCoulmnAndFilterQuote(QuoteNumber)
});

When("add filter and cloumn to Quote user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Quote Report will create successfully with all details", () => {
   
});
//#endregion

//#region Edit Quote Report
Given("User click on Edit Query to edit and add {string} column and filter to Quote", (IsQuoteDataExternal) => {
    BIActions.EditCoulmnAndFilterQuote(IsQuoteDataExternal);
});

When("add new filter and cloumn user save changes", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
});

Then("the Quote Report will edit successfully with all details", () => {
   
});
//#endregion