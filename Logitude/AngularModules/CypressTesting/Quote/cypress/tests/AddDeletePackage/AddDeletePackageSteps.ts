import * as CommonActions from '../../../../Common/cypress/actions/Actions';
import * as QuotesActions from '../../actions/Actions';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import { PackagesDetails } from '../../../../Shipment/cypress/models/PackagesDetails';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { CustomerDetails } from '../../../../Common/cypress/models/CustomerDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import { QuoteDetails } from '../../models/QuoteDetails';
import { QuoteSelectors } from '../../selectors/Selectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region variables
let quoteDetails: QuoteDetails;
let packagesDetails: PackagesDetails[];
let customerCode: string;
//#endregion

//#region Create customer
Given("the user logged in and navigates to customers workspace", () => {
  cy.Login();
  CommonActions.NavigatesToCustomersWorkspace();
});

Given("a customer with the following details", (dataTable) => {
  let customerDetails = Assists.CreateInstance<CustomerDetails>(dataTable, true);
  CommonActions.AddNewCustomer(customerDetails);
});

When("create customer", () => {
  CommonActions.CreateCustomer();
});

Then("the customer should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.PartnersDomainRequest, 200).then((interception) => {
    customerCode = interception.response.body.Customer.Code;
  });;
});
//#endregion

//#region Create export ocean LCL quote
Given("the user in quotes workspace", () => {
  QuotesActions.NavigatesToSQuotesWorkspace();
});

Given("a quote with the following details", (dataTable) => {
  quoteDetails = Assists.CreateInstance<QuoteDetails>(dataTable, true);
  quoteDetails.Shipper = customerCode;
  QuotesActions.FillQuoteFields(quoteDetails);
});

When("create quote", () => {
  QuotesActions.CreateQuote();
});

Then("the quote should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200).then((interception) => {
    quoteDetails.QuoteNumber = interception.response.body.QuoteNumber;
  })
});
//#endregion

//#region Add/Delete package
Given("the user add a package with the following details", (dataTable) => {
  QuotesActions.OpenQuote(quoteDetails.QuoteNumber);
  packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
  QuotesActions.FillPackageTab(packagesDetails, quoteDetails.ShipmentType);
});

Given("the user delete the package", () => {
  cy.Click(QuoteSelectors.DeletePackage, null)
  cy.Click(BaseSelectors.RedButton,"Yes");
});

When("update quote", () => {
  QuotesActions.UpdateQuote(QuoteSelectors.QuoteSave);
});

Then("the quote should update successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.Quotes, 200);
});
//#endregion