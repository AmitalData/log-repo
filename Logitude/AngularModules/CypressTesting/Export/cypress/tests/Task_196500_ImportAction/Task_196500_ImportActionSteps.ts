import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as ImportActionActions from '../../actions/ImportActionActions';

// Reuse Background steps from Task_SearchImportDeclarations
Given("the user is logged in", () => {
    cy.Login();
});

Given("navigates to Import Declarations workspace", () => {
    ImportActionActions.NavigateToImportDeclarationsWorkspace();
});

// Step definitions for the scenario
When("the user selects the first result from the grid", () => {
    ImportActionActions.SelectFirstResult();
});

When("clicks on {string} menu", (menuName: string) => {
    if (menuName === "Forms" || menuName === "טפסים") {
        ImportActionActions.ClickFormsMenu();
    } else {
        cy.log(`Unknown menu: ${menuName}`);
        throw new Error(`Menu "${menuName}" not implemented`);
    }
});

When("clicks on {string}", (itemName: string) => {
    if (itemName === "Declaration Form" || itemName === "טופס הצהרה") {
        ImportActionActions.ClickDeclarationForm();
    } else {
        cy.log(`Unknown item: ${itemName}`);
        throw new Error(`Item "${itemName}" not implemented`);
    }
});

Then("a popup dialog should appear", () => {
    ImportActionActions.WaitForPopup();
});

When(/^the user clicks OK\/Confirm in the popup$/, () => {
    ImportActionActions.ClickPopupConfirm();
});

Then("the action should complete successfully", () => {
    ImportActionActions.VerifyActionSuccess();
});

