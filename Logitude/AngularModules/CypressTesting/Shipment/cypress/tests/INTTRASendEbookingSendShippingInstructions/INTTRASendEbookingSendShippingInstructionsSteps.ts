import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as ShipmentActions from "../../actions/Actions";
import * as INTTRAActions from "../../actions/INTTRAActions";
import { BranchSettingsDetails } from "../../models/INTTRA/BranchSettingsDetails";
import { GeneralSettingsDetails } from "../../models/INTTRA/GeneralSettingsDetails";
import { InOutSettingsDetails } from "../../models/INTTRA/InOutSettingsDetails";
import { RegistrationSettingsDetails } from "../../models/INTTRA/RegistrationSettingsDetails";
import { RequiredToSendBookingShippingInstructions } from "../../models/INTTRA/RequiredToSendBookingShippingInstructions";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { PackagesDetails } from "../../models/PackagesDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let ShipmentNumber: string;

Given("the customer care user logged in and navigate to maintenance menu", () => {
    cy.Login(true);
    INTTRAActions.NavigateToMaintenanceMenu();
});

Given("open INTTRA settings wizard", () => {
    INTTRAActions.OpenINTTRASettingsWizard();
});

Given("fill the following general settings", (dataTable) => {
    let generalSettingsDetails = Assists.CreateInstance<GeneralSettingsDetails>(dataTable, true);
    INTTRAActions.FillINTTRAGeneralSettings(generalSettingsDetails);
});

Given("fill the following out settings", (dataTable) => {
    let inOutSettingsDetails = Assists.CreateInstance<InOutSettingsDetails>(dataTable, true);
    INTTRAActions.FillINTTRAInOutSettings(inOutSettingsDetails, "Out");
});

Given("fill the following in settings", (dataTable) => {
    let inOutSettingsDetails = Assists.CreateInstance<InOutSettingsDetails>(dataTable, true);
    INTTRAActions.FillINTTRAInOutSettings(inOutSettingsDetails, "In");
});

Given("fill the following branches settings", (dataTable) => {
    let branchSettingsDetailsList = Assists.CreateSet<BranchSettingsDetails>(dataTable);
    INTTRAActions.FillINTTRABranchesSettings(branchSettingsDetailsList);
});

Given("fill the following registration settings", (dataTable) => {
    let registrationSettingsDetailsList = Assists.CreateSet<RegistrationSettingsDetails>(dataTable);
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
    let shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
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

When("open INTTRA shipping instructions wizard", () => {
    INTTRAActions.OpenSendShippingInstructionsWizard();
});

Then("validation messages for sending shipping instructions should appear", () => {
    INTTRAActions.ValidateMessagesForSendingShippingInstructions();
});

Given("the user fill the following information to send e-booking", (dataTable) => {
    let requiredToSendBookingDetails = Assists.CreateInstance<RequiredToSendBookingShippingInstructions>(dataTable, true);
    INTTRAActions.FillRequiredToSendBooking(requiredToSendBookingDetails);
});

Given("add the following package", (dataTable) => {
    let packagesDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    ShipmentActions.FillPackageTab("Ocean", packagesDetailsList, "FCL");
});

Given("the user fill the following information to send shipping instructions", (dataTable) => {
    let requiredToSendShippingInstructions = Assists.CreateInstance<RequiredToSendBookingShippingInstructions>(dataTable, true);
    INTTRAActions.FillRequiredToSendShippingInstructions(requiredToSendShippingInstructions);
});

Given("add an inside package with the following details", (dataTable) => {
    let insidePackagesDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    ShipmentActions.AddInsidePackage(insidePackagesDetailsList);
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

Given("the user in INTTRA shipping instructions wizard", () => {
    INTTRAActions.OpenSendShippingInstructionsWizard();
});

When("send booking request", () => {
    INTTRAActions.SendBookingRequest();
});

When("send shipping instructions request", () => {
    INTTRAActions.SendShippingInstructionsRequest();
});

Then("the request should send successfully", () => {
    INTTRAActions.ValidateSendBookingRequest();
});

Then("the instructions should send successfully", () => {
    INTTRAActions.ValidateSendInstructionsRequest();
});

Then("booking request status should be {string}", (status) => {
    INTTRAActions.ValidateBookingRequestStatus(status);
});