import { Given } from "cypress-cucumber-preprocessor/steps";

Given("the user visit Google", () => {
  cy.visit("https://www.google.com");
});