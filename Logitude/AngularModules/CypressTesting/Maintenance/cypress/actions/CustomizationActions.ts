import { CustomizationDetails } from "../models/CustomizationDetails";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { CustomizationSelectors } from "../selectors/CustomizationSelectors";

export function NavigatesToSCustomizationWorkspace() {
    cy.Click(CustomizationSelectors.SettingMenu, null, true);
    cy.Click(CustomizationSelectors.CustomizationTab, null, true);
}

export const SearchModule = () => {
    cy.wait(2000)
    cy.FillLogTextBox(CustomizationSelectors.SearchModule, "Shipment", true);
    cy.get(CustomizationSelectors.GridViewCell).contains('Shipment').click()
    cy.Click(CustomizationSelectors.CustomeFields, null, true);
}

export function AssertSearchModule() {
    BaseAssertion.AssertStatusCode(RequestAliases.EntityResource, 200);
}

export const DefineSearchAssert = () => {
    cy.DefineRequestWait(RestAPI.GET, Urls.EntityResource, RequestAliases.EntityResource);
}

export const AssertCreateField = () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ObjectFields, 200);
}

export const DefineCreateFieldAssert = () => {
    cy.DefineRequestWait(RestAPI.POST, Urls.ObjectFields, RequestAliases.ObjectFields);
}

const formatId = (id: string, appendNumber): string => {
    return id + (appendNumber !== null ? `_${appendNumber}` : "");
}

export function FillCustomFieldDetailes(customizationDetailes: CustomizationDetails, id: number, selectText, dropdownId, appendNumber) {
    cy.FillLogTextBox(formatId(CustomizationSelectors.CustomFieldLable, appendNumber), customizationDetailes.FieldLabel)
    cy.FillLogTextBox(formatId(CustomizationSelectors.CustomFieldsCode, appendNumber), customizationDetailes.Code + id)
    cy.Click(formatId("#ComboBox_0", dropdownId), null, true)
    cy.Click(".ComboBoxItem", selectText, true)
    if (customizationDetailes.MaxLength) {
        cy.FillLogTextBox(CustomizationSelectors.CustomMinLength, customizationDetailes.MinLength)
        cy.FillLogTextBox(CustomizationSelectors.CustomMaxLength, customizationDetailes.MaxLength)
    }
};

export function EditCustomFieldDetailes() {
    cy.Click('#edit', '', true);
    cy.FillLogTextBox(CustomizationSelectors.HelpText, "Cypress Test")

};

export function EditOrCreateCustomField(isEdit: Boolean, CutomFieldID: number, customizationDetailes: CustomizationDetails) {
    if (isEdit) {
        EditCustomFieldDetailes();
    }
    else {
        cy.Click(CustomizationSelectors.AddCustomField, null, true);
        FillCustomFieldDetailes(customizationDetailes, CutomFieldID, "Text", 3, null);
        DefineCreateFieldAssert();
    }
}