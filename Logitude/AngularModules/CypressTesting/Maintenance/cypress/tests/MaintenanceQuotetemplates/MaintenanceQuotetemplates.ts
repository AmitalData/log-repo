import '@4tw/cypress-drag-drop'
import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { QuoteTemplateDetails } from '../../models/QuoteTemplateDetails';

//#region Create new Quote Template
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenTabInMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.QuoteTemplatesMaintenanceItem)
});

Given("a quote template with {string} name", (quoteTemplateName) => {
    Actions.OpenNewWizard("QuoteTemplate");
    Actions.FillQuoteTemplateName(quoteTemplateName)
});

When("create quote template", () => {
    Actions.CreateQuoteTemplate();
});

Then("the quote template should create successfully", () => {
    Actions.AssertCreateQuoteTemplate();
});
//#endregion

//#region Edit the quote template's Page Header & Footer
Given("fill the {string} settings with the following details", (section, dataTable) => {
    let coulmnWidthDetails = Assists.CreateInstance<QuoteTemplateDetails>(dataTable, true);
    Actions.OpenQuoteTemplateSection(section);
    Actions.FillQuoteHeaderFooterColumnWidth(coulmnWidthDetails);
    Actions.ValidateWidthErrorMesseage();
});

When("save quote template", () => {
    Actions.UpdateQuoteTemplate();
});

Then("the quote template should update successfully", () => {
   Actions.AssertUpdateQuoteTemplate();
});
//#endregion

//#region Edit the quote template's Quote Header & Details
Given("drag and drop the following details in {string} settings", (section, dataTable) => {
    let fieldDetails = Assists.CreateSet<QuoteTemplateDetails>(dataTable);
   Actions.OpenQuoteTemplateSection(section);
   Actions.DragAndDropFields(fieldDetails);
});

Given("edit {string} field in label tab to {string}", (labelToEdit, newFieldValue) => {
    Actions.EditLabelField(labelToEdit, newFieldValue)
});

When("save quote header", () => {
    Actions.UpdateQuoteHeaderTemplate();
});

Then("the quote header template should update successfully", () => {
    Actions.AssertUpdateQuoteHeaderTemplate();
});

//#endregion

//#region Edit the quote template's Quote Introduction
Given("the user open the quote template", () => {
    Actions.OpenQuoteTemplate();
});

Given("add {string} field in {string} settings", (fieldToBeAdd: string, section,) => {
    Actions.OpenQuoteTemplateSection(section);
    Actions.AddDataFieldToIntroduction(fieldToBeAdd)
});

When("save quote introduction template", () => {
    Actions.UpdateQuoteIntroductionTemplate();
});

//#endregion

//#region Edit the quote template's Pricing Packages and Containers
Given("the user reopen the quote template", () => {
    Actions.ReopenQuoteTemplate();
});

Given("add the following columns fields in {string} settings", (section, dataTable) => {
    let coulmnsList = Assists.CreateSet<QuoteTemplateDetails>(dataTable);
    Actions.OpenQuoteTemplateSection(section)
    Actions.AddColumnsToPricingTable(coulmnsList)
});

When("save quote pricing template", () => {
    Actions.UpdateQuotePricingTemplate();
});
//#endregion


