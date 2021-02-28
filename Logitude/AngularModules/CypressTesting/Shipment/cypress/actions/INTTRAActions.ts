import { ShipmentSelectors } from '../selectors/Selectors';
import { GeneralSettingsDetails } from '../models/INTTRA/GeneralSettingsDetails';
import { InOutSettingsDetails } from '../models/INTTRA/InOutSettingsDetails';
import { BranchSettingsDetails } from '../models/INTTRA/BranchSettingsDetails';
import { RegistrationSettingsDetails } from '../models/INTTRA/RegistrationSettingsDetails';
import { RequiredToSendBookingDetails } from "../models/INTTRA/RequiredToSendBookingDetails";
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import { BaseURLs } from '../../../Base/cypress/constants/URLs';


export function NavigateToMaintenanceMenu() {
    cy.Click(BaseSelectors.MaintenanceMenu, null);
}

export function OpenINTTRASettingsWizard() {
    cy.Click(BaseSelectors.MaintenanceTransmissionsTab, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetINTTRASettings, RequestAliases.GetINTTRASettings);
    cy.Click(BaseSelectors.INTTRAMaintenanceItem, null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetINTTRASettings, 200);
}

export function FillINTTRAGeneralSettings(generalSettingsDetails: GeneralSettingsDetails) {
    FillINTTRAMode(generalSettingsDetails.Mode);
    FillINTTRAId(generalSettingsDetails.INTTRAID);
    FillINTTRAAlias(generalSettingsDetails.Alias);
}

export function FillINTTRAInOutSettings(inOutSettingsDetails: InOutSettingsDetails, settingsType: string) {
    OpenFTPDetailWizard(settingsType);
    FillFTPDetailUserName(inOutSettingsDetails.UserName);
    FillFTPDetailPassword(inOutSettingsDetails.Password);
    FillFTPDetailHost(inOutSettingsDetails.Host);
    FillFTPDetailFolder(inOutSettingsDetails.Folder);
    cy.Click(ShipmentSelectors.SaveFTPDetailButton, null);
    BaseAssertion.AssertStatusCode(RequestAliases.SaveFTPDetails, 200);
}

export function FillINTTRABranchesSettings(branchSettingsDetailsList: BranchSettingsDetails[]) {
    for (let i = 0; i < branchSettingsDetailsList.length; i++) {
        cy.contains(ShipmentSelectors.ContainsBranchesSettings)
            .parents(BaseSelectors.table)
            .find(BaseSelectors.SimpleGridViewRow)
            .contains(branchSettingsDetailsList[i].BranchName)
            .parents(BaseSelectors.SimpleGridViewRow)
            .within(() => {
                cy.FillLogTextBox(ShipmentSelectors.BranchINTTRAId, branchSettingsDetailsList[i].INTTRAID);
                cy.FillLogTextBox(ShipmentSelectors.BranchINTTRAAlias, branchSettingsDetailsList[i].PartyAlias);
                cy.FillLogLov(ShipmentSelectors.BranchINTTRAContact, branchSettingsDetailsList[i].Contact, false, true);
            });
    }
}

export function FillINTTRARegistrationSettings(registrationSettingsDetailsList: RegistrationSettingsDetails[]) {
    for (let i = 0; i < registrationSettingsDetailsList.length; i++) {
        cy.contains(ShipmentSelectors.ContainsRegistration)
            .parents(BaseSelectors.table)
            .find(BaseSelectors.SimpleGridViewHeaderDark)
            .contains(BaseSelectors.td, registrationSettingsDetailsList[i].BranchName)
            .invoke("index").then((index) => {
                cy.contains(ShipmentSelectors.ContainsRegistration)
                    .parents(BaseSelectors.table)
                    .find(BaseSelectors.SimpleGridViewRow)
                    .contains(registrationSettingsDetailsList[i].RegistrationCode)
                    .parents(BaseSelectors.SimpleGridViewRow)
                    .within(() => {
                        let checkBoxIndex = index - 2;
                        cy.get(BaseSelectors.CheckboxInput).eq(checkBoxIndex).check({ force: true });
                    });
            });
    }
}

export function SaveINTTRASettings() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.PutINTTRASettings, RequestAliases.PutINTTRASettings);
    cy.Click(BaseSelectors.RedButton, null);
}

export function ValidateSaveINTTRASettings() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutINTTRASettings, 200);
}

export function ValidateMessagesForSendingEBooking() {
    cy.get(BaseSelectors.ValidationSummaryBlock).eq(1).find(BaseSelectors.ul)
        .within(() => {
            cy.get(BaseSelectors.li).contains(ShipmentSelectors.ContainsMainCarriageCarrierRequired).should("exist");
            cy.get(BaseSelectors.li).contains(ShipmentSelectors.ContainsContractNumberRequired).should("exist");
            cy.get(BaseSelectors.li).contains(ShipmentSelectors.ContainsETDOrVesselAndVoyageMustProvided).should("exist");
            cy.get(BaseSelectors.li).contains(ShipmentSelectors.ContainsShipmentDescriptionOfGoodsRequired).should("exist");
            cy.get(BaseSelectors.li).contains(ShipmentSelectors.ContainsShipmentPackagesRequired).should("exist");
        });
}

export function OpenSendBookingWizard() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetBookingMessageResultValidate, RequestAliases.GetBookingMessageResultValidate);
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsSendEBooking);
    BaseAssertion.AssertStatusCode(RequestAliases.GetBookingMessageResultValidate, 200);
}

export function FillRequiredToSendBooking(requiredToSendBookingDetails: RequiredToSendBookingDetails) {
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsClose);
    FillRequiredToSendBookingInGeneralTab(requiredToSendBookingDetails);
    FillRequiredToSendBookingInOrdersTab(requiredToSendBookingDetails);
    FillRequiredToSendBookingInPartnersTab(requiredToSendBookingDetails);
    FillRequiredToSendBookingInRoutingsTab(requiredToSendBookingDetails);
}

export function SendBookingRequest() {
    cy.DefineRequestWait(RestAPI.GET, URLs.INTTRAWebServiceSendEBooking, RequestAliases.INTTRAWebServiceSendEBooking);
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsRequestBooking);
}

export function ValidateSendBookingRequest() {
    BaseAssertion.AssertStatusCode(RequestAliases.INTTRAWebServiceSendEBooking, 200);
}

export function ValidateBookingRequestStatus(status: string) {
    cy.get(BaseSelectors.Label)
        .contains(ShipmentSelectors.ContainsStatus)
        .parent()
        .find(BaseSelectors.Value)
        .should("have.text", status);
}

function FillINTTRAMode(mode: string) {
    if (mode) {
        cy.ClickRadio(ShipmentSelectors.INTTRASettingsModeRadio(mode));
    }
}

function FillINTTRAId(id: string) {
    if (id) {
        cy.FillLogTextBox(ShipmentSelectors.INTTRASettingId, id);
    }
}

function FillINTTRAAlias(alias: string) {
    if (alias) {
        cy.FillLogTextBox(ShipmentSelectors.INTTRASettingAlias, alias);
    }
}

function FillFTPDetailUserName(username: string) {
    if (username) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailUserName, username);
    }
}

function FillFTPDetailPassword(password: string) {
    if (password) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailPassword, password);
    }
}

function FillFTPDetailHost(host: string) {
    if (host) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailHost, host);
    }
}

function FillFTPDetailFolder(folder: string) {
    if (folder) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailFolder, folder);
    }
}

function OpenFTPDetailWizard(settingsType: string) {
    cy.get(BaseSelectors.Label)
        .contains(settingsType === "Out" ? ShipmentSelectors.ContainsOutSettings : ShipmentSelectors.ContainsInSettings)
        .parents(BaseSelectors.FormFieldRow)
        .find(BaseSelectors.StartsWithEditButton)
        .then($btn => {
            let isEditButtonDisabled = $btn.is(":disabled");
            let requestType: string = isEditButtonDisabled ? RestAPI.POST : RestAPI.PUT;
            let openFTPDetailWizardSelector: string = isEditButtonDisabled ? BaseSelectors.StartsWithAddButton : BaseSelectors.StartsWithEditButton;

            cy.DefineRequestWait(requestType, URLs.FTPDetails, RequestAliases.SaveFTPDetails);
            cy.get(BaseSelectors.Label)
                .contains(settingsType === "Out" ? ShipmentSelectors.ContainsOutSettings : ShipmentSelectors.ContainsInSettings)
                .parents(BaseSelectors.FormFieldRow)
                .find(openFTPDetailWizardSelector)
                .click();
        });
}

function FillRequiredToSendBookingInGeneralTab(requiredToSendBookingDetails: RequiredToSendBookingDetails) {
    cy.Click(ShipmentSelectors.GeneralTab, null);
    if (requiredToSendBookingDetails.BranchName) {
        cy.FillLogLov(ShipmentSelectors.ShipmentBranch, requiredToSendBookingDetails.BranchName, true);
    }
}

function FillRequiredToSendBookingInOrdersTab(requiredToSendBookingDetails: RequiredToSendBookingDetails) {
    cy.Click(ShipmentSelectors.OrdersTab, null);
    if (requiredToSendBookingDetails.ShippingLine) {
        cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, requiredToSendBookingDetails.ShippingLine, false);
    }
    if (requiredToSendBookingDetails.ContractNumber) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentINTTRAContractNumber, requiredToSendBookingDetails.ContractNumber);
    }
    if (requiredToSendBookingDetails.DescriptionOfGoods) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentDescriptionOfGoods, requiredToSendBookingDetails.DescriptionOfGoods);
    }
}

function FillRequiredToSendBookingInPartnersTab(requiredToSendBookingDetails: RequiredToSendBookingDetails) {
    cy.Click(ShipmentSelectors.PartnersTab, null);
    cy.Click(ShipmentSelectors.EditShipper, null);
    if (requiredToSendBookingDetails.ShipperContact) {
        cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetByFilters, RequestAliases.ContactLogLovLoad);
        cy.get(ShipmentSelectors.ShipmentShipperContact).type(requiredToSendBookingDetails.ShipperContact);
        BaseAssertion.AssertStatusCode(RequestAliases.ContactLogLovLoad, 200).then(interception => {
            if (interception.response.body.Result.filter(c => c.EnglishName === requiredToSendBookingDetails.ShipperContact).length === 0) {
                cy.get(ShipmentSelectors.AddEditPartnerComponent).find(BaseSelectors.StartsWithAddButton).eq(2).click();
                cy.FillLogTextBox(ShipmentSelectors.ContactEmail, requiredToSendBookingDetails.ShipperContact + "@test.com")
                cy.FillLogTextBox(ShipmentSelectors.ContactEnglishName, requiredToSendBookingDetails.ShipperContact);
                cy.DefineRequestWait(RestAPI.POST, BaseURLs.Contacts, RequestAliases.PostContact);
                cy.Click(BaseSelectors.RedButton + ":last", null);
                BaseAssertion.AssertStatusCode(RequestAliases.PostContact, 200);
            } else {
                cy.get(BaseSelectors.DropDownListItem).children().eq(0).click();
            }
        });
    }
    cy.Click(ShipmentSelectors.PartnerOKButton, null);
}

function FillRequiredToSendBookingInRoutingsTab(requiredToSendBookingDetails: RequiredToSendBookingDetails) {
    cy.Click(ShipmentSelectors.RoutingsTab, null);
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null);
    if (requiredToSendBookingDetails.ETDDate && requiredToSendBookingDetails.ETDTime) {
        cy.FillDate(ShipmentSelectors.ShipmentMainCarriageETDDate, requiredToSendBookingDetails.ETDDate);
        cy.FillLogTextBox(ShipmentSelectors.ShipmentMainCarriageETDTime, requiredToSendBookingDetails.ETDTime);
    }
    if (requiredToSendBookingDetails.Vessel) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentMainCarriageVessel, requiredToSendBookingDetails.Vessel);
    }
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}