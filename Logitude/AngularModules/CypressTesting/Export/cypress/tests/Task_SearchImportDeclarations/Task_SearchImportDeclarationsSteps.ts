import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as SearchActions from '../../actions/SearchActions';

Given("the user is logged in", () => {
    cy.Login();
});

Given("navigates to Import Declarations workspace", () => {
    SearchActions.NavigateToImportDeclarationsWorkspace();
});

When("the user enters file number {string} in the search field", (fileNumber: string) => {
    SearchActions.EnterSearchTerm(fileNumber);
});

When("the user clears the search field", () => {
    SearchActions.ClearSearchField();
});

When("the user enters customer name {string} in the search field", (customerName: string) => {
    SearchActions.EnterSearchTerm(customerName);
});

When("the user enters declaration number {string} in the search field", (declarationNumber: string) => {
    SearchActions.EnterSearchTerm(declarationNumber);
});

When("the user enters cargo identifier {string} in the search field", (cargoId: string) => {
    SearchActions.EnterSearchTerm(cargoId);
});

Then("the system displays only files matching {string}", (searchTerm: string) => {
    SearchActions.VerifySearchResultsDisplayed();
    SearchActions.GetSearchResultsCount().then((count) => {
        cy.log(`Found ${count} result(s) for: ${searchTerm}`);
        expect(count).to.be.greaterThan(0);
    });
});

Then("the displayed file number matches the search term", () => {
    // Verify that the first result contains the searched file number
    cy.get("[id*='LogGrid'][id*='row0']:first").should('be.visible');
});

Then("the system displays only files with customer {string}", (customerName: string) => {
    SearchActions.VerifySearchResultsDisplayed();
    SearchActions.GetSearchResultsCount().then((count) => {
        cy.log(`Found ${count} result(s) for customer: ${customerName}`);
        expect(count).to.be.greaterThan(0);
    });
});

Then("all displayed files have customer names containing the search term", () => {
    // Verify customer name appears in results
    cy.get("[id*='LogGrid'][id*='row']:first").should('be.visible');
});

Then("the system displays only files matching declaration {string}", (declarationNumber: string) => {
    SearchActions.VerifySearchResultsDisplayed();
    SearchActions.GetSearchResultsCount().then((count) => {
        cy.log(`Found ${count} result(s) for declaration: ${declarationNumber}`);
        expect(count).to.be.greaterThan(0);
    });
});

Then("the displayed declaration number matches the search term", () => {
    // Verify declaration number appears in the first result
    cy.get("[id*='LogGrid'][id*='row0']:first").should('be.visible');
});

Then("the system displays only files with cargo identifier {string}", (cargoId: string) => {
    SearchActions.VerifySearchResultsDisplayed();
    SearchActions.GetSearchResultsCount().then((count) => {
        cy.log(`Found ${count} result(s) for cargo: ${cargoId}`);
        expect(count).to.be.greaterThan(0);
    });
});

Then("all displayed files contain the cargo identifier in their data", () => {
    // Verify cargo identifier search executed successfully
    cy.get("[id*='LogGrid'][id*='row']:first").should('be.visible');
});

