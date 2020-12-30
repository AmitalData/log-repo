import * as sh from "../../actions/ShipmentActions"
import {Selectors} from "../../selectors/Selectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";

Given("User logged in successfully", () => {
  cy.Login()
});

Given("Go to shipments workspace", () => {
  cy.Click(Selectors.GeneralMHOperations, null)
  cy.Click("#SHIP", null)
});

Given("Test log message", () => {
  console.log("Test log message")
});