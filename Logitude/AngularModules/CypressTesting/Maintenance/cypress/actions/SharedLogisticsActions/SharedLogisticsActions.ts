import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { CustomizationRuleDetails } from "../../models/CustomizationModuleDetails/CustomizationRuleDetails"
import { SharedLogisticsSelectors } from "../../selectors/SharedLogisticsSelectors/SharedLogisticsSelectors"
import { CustomizationConstants } from "../../constants/CustomizationConstants/CustomizationConstants";
import { SharedLogisticsURLs } from "../../constants/SharedLogisticsURLs/SharedLogisticsURLs";
import { SharedLogisticsRequestAliases } from "../../constants/SharedLogisticsURLs/SharedLogisticsRequestAliases";
import { SharedLogisticsSettingsDetails } from "../../models/SharedLogisticsDetails/SharedLogisticsSettingsDetails"
import { DocumentsPermissionsDetails } from "../../models/SharedLogisticsDetails/DocumentsPermissionsDetails"
import {PartnersPermissionsDetails} from "../../models/SharedLogisticsDetails/PartnersPermissionsDetails"
import {MoneyPermissionsDetails} from "../../models/SharedLogisticsDetails/MoneyPermissionsDetails"
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { And } from "cypress-cucumber-preprocessor/steps/index";
import * as Actions from "../Actions"
import { ContactContext } from "../../models/ContactContext";
import { ContactDetails } from "../../models/ContactDetails";

let CustomerCode:string

export function ChooseSharedLogistics() {
    
    cy.DefineRequestWait(RestAPI.POST, SharedLogisticsURLs.TotangoService, SharedLogisticsRequestAliases.TotangoService);
    cy.get(SharedLogisticsSelectors.SharedLogisticsTab).click()
}
export function ActivateSharedLogistics() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.TotangoService, 204).then((interception) => {
    });
    if (SharedLogisticsSelectors.ActivationWizard) {
        cy.get(SharedLogisticsSelectors.ActivationWizard).click()
        cy.get(SharedLogisticsSelectors.ActivateSharedLogistics).click()
    }
}
export function SaveActivateSharedLogistics() {
    DefinePutActivateSharedLogisticsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutActivateSharedLogisticsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedLogisticsURLs.tenants, SharedLogisticsRequestAliases.tenants);
}

export function AssertActivateSharedLogistics() {
    AssertPutActivateSharedLogistics();
}
function AssertPutActivateSharedLogistics() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.tenants, 200).then((interception) => {
    });
}

export function UpdateSharedLogisticsSetting(sharedLogisticsSettingsDetails: SharedLogisticsSettingsDetails) {
    cy.get(SharedLogisticsSelectors.SharedLogisticsSettings).click()

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.DisplayDocumentsAndEventsInMessagesLink,
        sharedLogisticsSettingsDetails.DisplayDocumentsAndEventsInMessagesLink)
    
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.ActivateWebAccess,
    sharedLogisticsSettingsDetails.ActivateWebAccess)

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.ActivateMobile,
    sharedLogisticsSettingsDetails.ActivateMobile)

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.DisplayDocumentsAndEventsInMessagesLink,
    sharedLogisticsSettingsDetails.DisplayDocumentsAndEventsInMessagesLink) 

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.SendLinkInEmailMessagesToYourPartners,
        sharedLogisticsSettingsDetails.SendLinkInEmailMessagesToYourPartners)

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.EnableMastersDocumentsLinkForAgents,
        sharedLogisticsSettingsDetails.EnableMastersDocumentsLinkForAgents)
}

export function UpdatedocumentsPermissions(documentsPermissionsDetails: DocumentsPermissionsDetails) {
    cy.get(SharedLogisticsSelectors.DocumentsPermissions).click()
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.AirManifestDoc,
        documentsPermissionsDetails.AirManifest)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.AirWaybillLabelsDoc,
        documentsPermissionsDetails.AirwaybillLabels)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.ArrivalNoticeDoc,
        documentsPermissionsDetails.ArrivalNotice)
}

export function SaveDocumentsPermissions() {
    DefinePutDocumentsPermissionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutDocumentsPermissionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedLogisticsURLs.putupdatedocumenttypepmlists, SharedLogisticsRequestAliases.putupdatedocumenttypepmlists);
}

export function AssertDocumentsPermissions() {
    AssertPutDocumentsPermissions();
}
function AssertPutDocumentsPermissions() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.putupdatedocumenttypepmlists, 200).then((interception) => {
    });
}
//PartnersPermissions
export function UpdatePartnersPermissions(partnersPermissionsDetails:PartnersPermissionsDetails){
    cy.get(SharedLogisticsSelectors.PartnersPermissions).click()

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.Shipper, partnersPermissionsDetails.Shipper)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.Consignee, partnersPermissionsDetails.Consignee)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.Agent, partnersPermissionsDetails.Agent)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.ShipperNotExporter,
        partnersPermissionsDetails.ShipperNotExporter)

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.ConsigneeNotImporter,
        partnersPermissionsDetails.ConsigneeNotImporter)

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.Notify1, partnersPermissionsDetails.Notify1)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.Notify2, partnersPermissionsDetails.Notify2)

}
export function SavePartnersPermissions() {
    DefinePutPartnersPermissionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutPartnersPermissionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedLogisticsURLs.putsharedlogisticssettings, SharedLogisticsRequestAliases.sharedlogisticssettings);
}

export function AssertPartnersPermissions() {
    AssertPutPartnersPermissions();
}
function AssertPutPartnersPermissions() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.sharedlogisticssettings, 200).then((interception) => {
    });
}
//Money Permissions
export function UpdateMoneyPermissions(moneyPermissionsDetails:MoneyPermissionsDetails){
    cy.get(SharedLogisticsSelectors.MoneyPermissions).click()

    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.InvoicesMenu, moneyPermissionsDetails.InvoicesMenu)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.MoneyTab, moneyPermissionsDetails.MoneyTab)
    Actions.FillInputCheckBoxProcess(SharedLogisticsSelectors.AmountInLocalCurrencyColumn,
         moneyPermissionsDetails.AmountInLocalCurrencyColumn)
}
export function SaveMoneyPermissions() {
    DefinePutMoneyPermissionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutMoneyPermissionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedLogisticsURLs.putsharedlogisticssettings, SharedLogisticsRequestAliases.sharedlogisticssettings);
}

export function AssertMoneyPermissions() {
    AssertPutMoneyPermissions();
}
function AssertPutMoneyPermissions() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.sharedlogisticssettings, 200).then((interception) => {
    });
}

export function GetFirstCustomer(){
    cy.get(SharedLogisticsSelectors.InviteCustomers).click()
    cy.get(SharedLogisticsSelectors.FirstCustomer).click()
    GetCustomerCode(SharedLogisticsSelectors.CustomerCode)
    cy.get(SharedLogisticsSelectors.NewContact).click()
    
}
export function GetCustomerCode(CustomerCodeSelector:string){
    cy.get(CustomerCodeSelector).invoke('text').then((Lable) => {
        cy.log(Lable.toString())
        CustomerCode = (Lable.toString())
    })
}
export function CreateContact(){
DefinePostContactRequest();

cy.Click(BaseSelectors.RedButton + ":last", null);
}

function DefinePostContactRequest() {
    cy.DefineRequestWait(RestAPI.POST,SharedLogisticsURLs.PartnersDomain, SharedLogisticsRequestAliases.PostPartnersDomain);
}


export function AssertPostContact() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.PostPartnersDomain, 200).then((interception) => {
        let responseBody = interception.response.body;
        ContactContext.Id = responseBody.Id;
        ContactContext.Email = responseBody.Email;
    });
}
export function InviteAddedContact(NewContactEmail:string){
cy.get('[data-cy="'+NewContactEmail+'_Contact"]').within(() => {
    DefinePutContactInvitationRequest();
    cy.get(SharedLogisticsSelectors.Invite).click()
});

}
function DefinePutContactInvitationRequest() {
    cy.DefineRequestWait(RestAPI.PUT,SharedLogisticsURLs.contactinvitation, SharedLogisticsRequestAliases.putcontactinternetaccessinvitation);
}
export function AssertPutContactInvitation() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.putcontactinternetaccessinvitation, 200).then((interception) => {
    });
}

export function ConfirmInviting(ContactEnglishName:string,MsgPart1:string,MsgPart2:string){

cy.get(SharedLogisticsSelectors.Message).contains(MsgPart1+'" '+ContactEnglishName+' "'+MsgPart2)
cy.Click(BaseSelectors.RedButton + ":last", null);
cy.Click(BaseSelectors.RedButton + ":last", null);
cy.get(SharedLogisticsSelectors.BackBottun).click()
}

export function AssertcustomerInInvitedList() {
    cy.get(SharedLogisticsSelectors.InvitedCustomersCount).click()
    cy.get(SharedLogisticsSelectors.Search).click().type(CustomerCode)
    cy.get(SharedLogisticsSelectors.row0).contains(CustomerCode)
    cy.get(SharedLogisticsSelectors.row0).within(() => {
        cy.get(SharedLogisticsSelectors.FirstCustomer).contains("Invited")
    });
    cy.get(SharedLogisticsSelectors.BackBottun).click()
}

export function GetFirstNotInvited() {
    cy.get(SharedLogisticsSelectors.NotInvitedCustomersCount).click()
    cy.get(SharedLogisticsSelectors.FirstCustomer).click()
    GetCustomerCode(SharedLogisticsSelectors.CustomerCode)
    cy.Click(BaseSelectors.RedButton + ":last", null);
    cy.get(SharedLogisticsSelectors.BackBottun).click()
}
export function SearchInvitedCustomers(){
    cy.get(SharedLogisticsSelectors.InvitedCustomersCount).click()
    cy.get(SharedLogisticsSelectors.Search).click().type(CustomerCode)
}
export function SearchInvitedNotCustomers(){
    cy.get(SharedLogisticsSelectors.NotInvitedCustomersCount).click()
    cy.get(SharedLogisticsSelectors.Search).click().type(CustomerCode)
}
export function AssertCustomerExist(){
    cy.get(SharedLogisticsSelectors.row0).contains(CustomerCode)
}

export function GetFirstInvited(){
    cy.get(SharedLogisticsSelectors.InvitedCustomersCount).click()
    DefineGetContactsRequest()
    cy.get(SharedLogisticsSelectors.FirstCustomer).click()
    GetCustomerCode(SharedLogisticsSelectors.CustomerCode)
    cy.Click(BaseSelectors.RedButton + ":last", null);
    cy.get(SharedLogisticsSelectors.BackBottun).click()
}

function DefineGetContactsRequest() {
    cy.DefineRequestWait(RestAPI.GET,SharedLogisticsURLs.GetContacts, SharedLogisticsRequestAliases.GetContacts);
}
export function AssertGetContacts() {
    BaseAssertion.AssertStatusCode(SharedLogisticsRequestAliases.GetContacts, 200).then((interception) => {
    });
}

export function AssertSearchIsNull(){
cy.get(SharedLogisticsSelectors.CellContent).contains(' No Data Found ')
cy.get(SharedLogisticsSelectors.BackBottun).click()
}