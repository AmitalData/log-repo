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
        let branchName = branchSettingsDetailsList[i].BranchName
        FillBranchINTTRAId(branchName, branchSettingsDetailsList[i].INTTRAID);
        FillBranchINTTRAAlias(branchName, branchSettingsDetailsList[i].PartyAlias);
        FillBranchINTTRAContact(branchName, branchSettingsDetailsList[i].Contact);
    }
}

export function FillINTTRARegistrationSettings(registrationSettingsDetailsList: RegistrationSettingsDetails[]) {
    for (let i = 0; i < registrationSettingsDetailsList.length; i++) {
        let branchName = registrationSettingsDetailsList[i].BranchName;
        let registrationCode = registrationSettingsDetailsList[i].RegistrationCode;
        let registrationCheckBoxSelector = ShipmentSelectors.INTTRARegistrationCheckBox(branchName, registrationCode);
        cy.get(registrationCheckBoxSelector).check({ force: true });
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
    cy.get(ShipmentSelectors.INTTRABookingStatus).should("have.text", status);
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
    let addButtonSelector = ShipmentSelectors.InOutSettingsHostAddButton(settingsType);
    let editButtonSelector = ShipmentSelectors.InOutSettingsHostEditButton(settingsType);
    cy.get(editButtonSelector)
        .then($editButton => {
            let isEditButtonDisabled = $editButton.is(":disabled");
            let requestType = isEditButtonDisabled ? RestAPI.POST : RestAPI.PUT;
            let openFTPDetailWizardSelector = isEditButtonDisabled ? addButtonSelector : editButtonSelector;
            cy.DefineRequestWait(requestType, URLs.FTPDetails, RequestAliases.SaveFTPDetails);
            cy.Click(openFTPDetailWizardSelector, null);
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
    let shipperContact = requiredToSendBookingDetails.ShipperContact;
    if (shipperContact) {
        cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetByFilters, RequestAliases.ContactLogLovLoad);
        cy.get(ShipmentSelectors.ShipmentShipperContact).type(shipperContact);
        BaseAssertion.AssertStatusCode(RequestAliases.ContactLogLovLoad, 200).then(interception => {
            if (interception.response.body.Result.filter(c => c.EnglishName === shipperContact).length === 0) {
                AddNewShipperContact(shipperContact);
            } else {
                cy.get(BaseSelectors.DropDownListItem).children().eq(0).click();
            }
        });
    }
    cy.Click(ShipmentSelectors.PartnerOKButton, null);
}

function AddNewShipperContact(shipperContact: string) {
    if(shipperContact){
        cy.Click(ShipmentSelectors.AddShipperContactButton, null);
        cy.FillLogTextBox(ShipmentSelectors.ContactEmail, shipperContact + "@test.com")
        cy.FillLogTextBox(ShipmentSelectors.ContactEnglishName, shipperContact);
        cy.DefineRequestWait(RestAPI.POST, BaseURLs.Contacts, RequestAliases.PostContact);
        cy.Click(ShipmentSelectors.SaveShipperContactButton, null);
        BaseAssertion.AssertStatusCode(RequestAliases.PostContact, 200);
    }
}

function FillRequiredToSendBookingInRoutingsTab(requiredToSendBookingDetails: RequiredToSendBookingDetails) {
    cy.Click(ShipmentSelectors.RoutingsTab, null);
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null);
    let ETDDate = requiredToSendBookingDetails.ETDDate;
    let ETDTime = requiredToSendBookingDetails.ETDTime;
    let vessel = requiredToSendBookingDetails.Vessel;
    if (ETDDate) {
        cy.FillDate(ShipmentSelectors.ShipmentMainCarriageETDDate, ETDDate);
    }
    if(ETDTime){
        cy.FillLogTextBox(ShipmentSelectors.ShipmentMainCarriageETDTime, ETDTime);
    }
    if (vessel) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentMainCarriageVessel, vessel);
    }
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}

function FillBranchINTTRAId(branchName: string, INTTRAId: string) {
    if (branchName && INTTRAId) {
        cy.FillLogTextBox(ShipmentSelectors.BranchINTTRAId(branchName), INTTRAId);
    }
}

function FillBranchINTTRAAlias(branchName: string, INTTRAAlias: string) {
    if (branchName && INTTRAAlias) {
        cy.FillLogTextBox(ShipmentSelectors.BranchINTTRAAlias(branchName), INTTRAAlias);
    }
}

function FillBranchINTTRAContact(branchName: string, INTTRAContact: string) {
    if (branchName && INTTRAContact) {
        cy.FillLogLov(ShipmentSelectors.BranchINTTRAContact(branchName), INTTRAContact, false, true);
    }
}