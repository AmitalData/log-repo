import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("User logged in successfully", () => {
  cy.Login()
});

Given("Test log message", () => {
  console.log("Test log message")
});