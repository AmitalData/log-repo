import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as ShipmentActions from "../../actions/Actions";
import * as INTTRAActions from "../../actions/INTTRAActions";
import { BranchSettingsDetails } from "../../models/INTTRA/BranchSettingsDetails";
import { GeneralSettingsDetails } from "../../models/INTTRA/GeneralSettingsDetails";
import { InOutSettingsDetails } from "../../models/INTTRA/InOutSettingsDetails";
import { RegistrationSettingsDetails } from "../../models/INTTRA/RegistrationSettingsDetails";
import { RequiredToSendBookingDetails } from "../../models/INTTRA/RequiredToSendBookingDetails";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { PackagesDetails } from "../../models/PackagesDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";

let ShipmentNumber: string;

Given("the customer care user logged in and navigate to maintenance menu", () => {
    cy.Login(true);
    INTTRAActions.NavigateToMaintenanceMenu();
});

Given("open INTTRA settings wizard", () => {
    INTTRAActions.OpenINTTRASettingsWizard();
});

Given("fill the following general settings", (dataTable) => {
    let generalSettingsDetails = dataTable.hashes()[0] as GeneralSettingsDetails;
    INTTRAActions.FillINTTRAGeneralSettings(generalSettingsDetails);
});

Given("fill the following out settings", (dataTable) => {
    let inOutSettingsDetails = dataTable.hashes()[0] as InOutSettingsDetails;
    INTTRAActions.FillINTTRAInOutSettings(inOutSettingsDetails, "Out");
});

Given("fill the following in settings", (dataTable) => {
    let inOutSettingsDetails = dataTable.hashes()[0] as InOutSettingsDetails;
    INTTRAActions.FillINTTRAInOutSettings(inOutSettingsDetails, "In");
});

Given("fill the following branches settings", (dataTable) => {
    let branchSettingsDetailsList = dataTable.hashes() as BranchSettingsDetails[];
    INTTRAActions.FillINTTRABranchesSettings(branchSettingsDetailsList);
});

Given("fill the following registration settings", (dataTable) => {
    let registrationSettingsDetailsList = dataTable.hashes() as RegistrationSettingsDetails[];
    INTTRAActions.FillINTTRARegistrationSettings(registrationSettingsDetailsList);
});

When("save settings", () => {
    INTTRAActions.SaveINTTRASettings();
});

Then("the settings should save successfully", () => {
    INTTRAActions.ValidateSaveINTTRASettings();
});

Given("the user logged in and navigate to shipments workspace", () => {
    cy.LogoutThenLogin();
    ShipmentActions.NavigatesToShipmentsWorkspace();
});

Given("a master shipment with the following details", (dataTable) => {
    let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment("Master");
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentNumber = interception.response.body.ShipmentNumber;
    });
});

Given("the user open the master shipment", () => {
    ShipmentActions.OpenShipment(ShipmentNumber);
});

When("open INTTRA e-booking wizard", () => {
    INTTRAActions.OpenSendBookingWizard();
});

Then("validation messages for sending e-booking should appear", () => {
    INTTRAActions.ValidateMessagesForSendingEBooking();
});

Given("the user fill the following information to send e-booking", (dataTable) => {
    let requiredToSendBookingDetails = dataTable.hashes()[0] as RequiredToSendBookingDetails;
    INTTRAActions.FillRequiredToSendBooking(requiredToSendBookingDetails);
});

Given("add the following package", (dataTable) => {
    let packagesDetailsList = dataTable.hashes() as PackagesDetails[];
    ShipmentActions.FillPackageTab("Ocean", packagesDetailsList, "FCL");
});

When("save the shipment", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton);
});

Then("the shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});

Given("the user in INTTRA e-booking wizard", () => {
    INTTRAActions.OpenSendBookingWizard();
});

When("send booking request", () => {
    INTTRAActions.SendBookingRequest();
});

Then("the request should send successfully", () => {
    INTTRAActions.ValidateSendBookingRequest();
});

Then("booking request status should be {string}", (status) => {
    INTTRAActions.ValidateBookingRequestStatus(status);
});