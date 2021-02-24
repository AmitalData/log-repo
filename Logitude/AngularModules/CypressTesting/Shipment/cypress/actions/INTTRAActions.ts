import { ShipmentSelectors } from '../selectors/Selectors';
import { GeneralSettingsDetails } from '../models/INTTRA/GeneralSettingsDetails';
import { InOutSettingsDetails } from '../models/INTTRA/InOutSettingsDetails';
import { BranchSettingsDetails } from '../models/INTTRA/BranchSettingsDetails';
import { RegistrationSettingsDetails } from '../models/INTTRA/RegistrationSettingsDetails';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';


export function LoginAsCustomerCare() {
    cy.Login(true);
}

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

export function FillINTTRAInOutSettings(inOutSettingsDetails: InOutSettingsDetails, isForOutSettings: boolean) {
    OpenFTPDetailWizard(isForOutSettings);
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
                let checkBoxLabelIndex = index - 2;
                cy.get(BaseSelectors.CheckBoxLabel).eq(checkBoxLabelIndex).then($label => {
                    if ($label.css("background").indexOf("CheckBoxIcon.png") === -1) {
                        cy.get(BaseSelectors.CheckBoxLabel).eq(checkBoxLabelIndex).click();
                    }
                });
            });
        });
    }
}

export function SaveINTTRASettings(){
    cy.DefineRequestWait(RestAPI.PUT, URLs.PutINTTRASettings, RequestAliases.PutINTTRASettings);
    cy.Click(BaseSelectors.RedButton, null);
}

export function ValidateSaveINTTRASettings(){
    BaseAssertion.AssertStatusCode(RequestAliases.PutINTTRASettings, 200);
}

export function OpenSendBookingWizard(){
    cy.DefineRequestWait(RestAPI.GET, URLs.GetSingleShipment, RequestAliases.GetSingleShipment);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetBookingMessageResultValidate, RequestAliases.GetBookingMessageResultValidate);
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsSendEBooking);
    BaseAssertion.AssertStatusCode(RequestAliases.GetSingleShipment, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.GetBookingMessageResultValidate, 200);
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

function OpenFTPDetailWizard(isForOutSettings: boolean) {
    cy.get(BaseSelectors.Label)
    .contains(isForOutSettings ? ShipmentSelectors.ContainsOutSettings : ShipmentSelectors.ContainsInSettings)
    .parents(BaseSelectors.FormFieldRow)
    .find(BaseSelectors.StartsWithEditButton)
    .then($btn => {
        if ($btn.is(":disabled")) {
            cy.DefineRequestWait(RestAPI.POST, URLs.FTPDetails, RequestAliases.SaveFTPDetails);
            OpenAddFTPDetailWizard(isForOutSettings);
        }else{
            cy.DefineRequestWait(RestAPI.PUT, URLs.FTPDetails, RequestAliases.SaveFTPDetails);
            OpenEditFTPDetailWizard(isForOutSettings);
        }
    });
}

function OpenAddFTPDetailWizard(isForOutSettings: boolean){
    cy.get(BaseSelectors.Label)
    .contains(isForOutSettings ? ShipmentSelectors.ContainsOutSettings : ShipmentSelectors.ContainsInSettings)
    .parents(BaseSelectors.FormFieldRow)
    .find(BaseSelectors.StartsWithAddButton)
    .click();
}

function OpenEditFTPDetailWizard(isForOutSettings: boolean){
    cy.get(BaseSelectors.Label)
    .contains(isForOutSettings ? ShipmentSelectors.ContainsOutSettings : ShipmentSelectors.ContainsInSettings)
    .parents(BaseSelectors.FormFieldRow)
    .find(BaseSelectors.StartsWithEditButton)
    .click();
}