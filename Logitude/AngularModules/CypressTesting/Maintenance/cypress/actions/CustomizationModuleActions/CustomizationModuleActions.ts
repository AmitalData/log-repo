import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { CustomizationRuleDetails } from "../../models/CustomizationModuleDetails/CustomizationRuleDetails"
import { CustomizationSelectors } from "../../selectors/CustomizationModuleSelectors/CustomizationSelectors"
import { CustomizationConstants } from "../../constants/CustomizationConstants/CustomizationConstants";
import { CustomizationURLs } from "../../constants/CustomizationURLs/CustomizationURLs";
import { CustomizationRequestAliases } from "../../constants/CustomizationURLs/CustomizationRequestAliases";
import { CustomizationScreenLayoutDetails } from "../../models/CustomizationModuleDetails/CustomizationScreenLayoutDetails"
import { MaintenanceSelectors } from "../../selectors/Selectors";


let Field: string[]=["",""]


export function ChooseCustomization() {

    cy.get(CustomizationSelectors.Settings).click()
    cy.DefineRequestWait(RestAPI.GET, CustomizationURLs.GetTranslationsByParam,
        CustomizationRequestAliases.Customization)
    cy.get(CustomizationSelectors.Customization).click()

}

export function ChooseObjectTable(objecttable: string) {

    cy.get(CustomizationSelectors.Search).type(objecttable)
    cy.get("[data-cy='" + objecttable + "']").click()

}
export function DisplyRules() {
    cy.get(CustomizationSelectors.Rules).click()
}

export function AddRule(customizationRuleDetails: CustomizationRuleDetails) {

    cy.get(CustomizationSelectors.AddRule).click()
    cy.get(CustomizationSelectors.RuleCode).click().type(customizationRuleDetails.Code)
    cy.get(CustomizationSelectors.RuleName).click().type(customizationRuleDetails.Name)
    chooseitemcomboBox(CustomizationSelectors.RuleType, "Rule Type " + customizationRuleDetails.RuleType)
    chooseitemcomboBox(CustomizationSelectors.TriggerType, "Trigger Type " + customizationRuleDetails.TriggerType)
    chooseTriggerField(customizationRuleDetails)
    ChooseNotificationType(customizationRuleDetails)

    if (customizationRuleDetails.ActiveForNew)
        cy.get(CustomizationSelectors.ActiveForNew).click()

    if (customizationRuleDetails.ActiveForUpdate)
        cy.get(CustomizationSelectors.ActiveForUpdate).click()

    ChooseRuleCondition(customizationRuleDetails)

    cy.get(CustomizationSelectors.Next).click()
    addNewRulemoreDetails(customizationRuleDetails)
}

function chooseitemcomboBox(comboBox: string, itemSelector: string) {
    cy.get(comboBox).click()
    cy.get("[data-cy='" + itemSelector + "']").click()

}
function chooseTriggerField(customizationRuleDetails: CustomizationRuleDetails) {
    if (customizationRuleDetails.RuleType != CustomizationConstants.FieldDuplication &&
        customizationRuleDetails.TriggerField != null) {
        cy.get(CustomizationSelectors.TriggerField).click().type(customizationRuleDetails.TriggerField);
        cy.contains(customizationRuleDetails.TriggerField).click()
    }
}
function ChooseNotificationType(customizationRuleDetails: CustomizationRuleDetails) {
    if (customizationRuleDetails.RuleType != CustomizationConstants.BlockField &&
        customizationRuleDetails.RuleType != CustomizationConstants.SetFieldValue &&
        customizationRuleDetails.NotificationType != null) {
        chooseitemcomboBox(CustomizationSelectors.NotificationType, "Notification Type " + customizationRuleDetails.NotificationType)
    }
}

function ChooseRuleCondition(customizationRuleDetails: CustomizationRuleDetails) {
    if (customizationRuleDetails.RuleCondition != null && customizationRuleDetails.TriggerType == "Condition") {
        cy.get(CustomizationSelectors.NewRuleFiltersSearchFields).click().type(customizationRuleDetails.RuleCondition)

        cy.get("[data-cy='" + customizationRuleDetails.RuleCondition + "']").within(() => {
            cy.get("label").click()
        })
        cy.SelectDropDownListItem2(CustomizationSelectors.Card,customizationRuleDetails.ConditionFieldValue)
    }
}
function addNewRulemoreDetails(customizationRuleDetails: CustomizationRuleDetails) {
    cy.get(CustomizationSelectors.AddField).click()
    cy.get(CustomizationSelectors.SearchField).click().type(customizationRuleDetails.RuleField)
    cy.get("ul > li").contains(customizationRuleDetails.RuleField).click()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function CreateNewRule() {
    DefinePostNewRuleRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePostNewRuleRequest() {
    cy.DefineRequestWait(RestAPI.POST, CustomizationURLs.ObjectTableRules, CustomizationRequestAliases.PostNewRule);
}

export function AssertCreateNewRule() {
    AssertPostNewRule();
    cy.get(CustomizationSelectors.Close).click()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function AssertPostNewRule() {
    BaseAssertion.AssertStatusCode(CustomizationRequestAliases.PostNewRule, 200).then((interception) => {
    });
}

export function GetTodayDateTime() {
    return String(new Date()).substring(0, 25)
}

export function SearchRule(code: string) {
    cy.get(CustomizationSelectors.SearchRulesMainComponent).click().type(code)
}

export function AssertRuleExection(ValidationMessage: string) {
    cy.get(CustomizationSelectors.ValidationSummary).should("contain.text", ValidationMessage)
    cy.get(CustomizationSelectors.ShipmentCancel).click()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function InactivateRule() {
    cy.get(CustomizationSelectors.Edit).click({ force: true })
    cy.get(CustomizationSelectors.InActive).click()
}


export function SaveRule() {
    DefinePutRuleRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutRuleRequest() {
    cy.DefineRequestWait(RestAPI.PUT, CustomizationURLs.ObjectTableRules, CustomizationRequestAliases.PutNewRule);
}

export function AssertSaveRule() {
    AssertPutRule();
    cy.get(CustomizationSelectors.Close).click()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function AssertPutRule() {
    BaseAssertion.AssertStatusCode(CustomizationRequestAliases.PutNewRule, 200).then((interception) => {
    });
}

export function DisplyCustomFields() {
    cy.get(CustomizationSelectors.CustomFields).click()
}

export function GetCustomFields(objectTable: string) {
    let row = 1
    cy.get("[data-cy='" + objectTable + ".Field" + row + "']").click()

    cy.get('input[id="FieldLable"]').invoke('val').then((Lable) => {
        cy.log(Lable.toString())
        Field[0] = (Lable.toString())
    });
    row = 2
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
    cy.get("[data-cy='" + objectTable + ".Field" + row + "']").click()

    cy.get('input[id="FieldLable"]').invoke('val').then((Lable) => {
        cy.log(Lable.toString())
        Field[1] = (Lable.toString())
    })
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
    cy.get(CustomizationSelectors.CustomFieldsClose).click()
}
export function GoToScreenLayout() {
    cy.get(CustomizationSelectors.ScreenLayout).click()
    DefineGetGeneralTabScreen()
    cy.get(CustomizationSelectors.GeneralTabScreen).click()
    AssertGetGeneralTabScreen()
}

export function DragAndDropFields(fieldDetails: CustomizationScreenLayoutDetails[]) {
    cy.get(CustomizationSelectors.FieldsSearch).click().type("" + Field[0])
    for (let i = 0; i < fieldDetails.length; i++) {
        if (i > 0) {
            cy.get(CustomizationSelectors.FieldsSearch).within(() => {
                cy.get(CustomizationSelectors.CloseX).click()
            })
            cy.get(CustomizationSelectors.FieldsSearch).type("" + Field[1])
        }
        cy.get(MaintenanceSelectors.AvaliableColumnsFields(fieldDetails[i].Field)).drag(MaintenanceSelectors.ColumnDropArea(fieldDetails[i].Column))
    }

}

export function SaveScreenLayout() {
    DefinePutScreenLayout();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutScreenLayout() {
    cy.DefineRequestWait(RestAPI.PUT, CustomizationURLs.PutScreenFields, CustomizationRequestAliases.PutScreenFields);
}

export function AssertSaveScreenLayout() {
    AssertPutScreenLayout();

    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function AssertPutScreenLayout() {
    BaseAssertion.AssertStatusCode(CustomizationRequestAliases.PutScreenFields, 200).then((interception) => {
    });
}

export function assertCustomFieldsExist() {
    cy.get("[class='TabHolder']").within(() => {
        cy.contains(Field[0])
        cy.contains(Field[1])
    });
}

export function RemoveFromScreenLayout(fieldDetails: CustomizationScreenLayoutDetails[]) {
    for (let i = 0; i < fieldDetails.length; i++) {

        cy.get(MaintenanceSelectors.ColumnDropArea(fieldDetails[i].Column)).within(() => {
            cy.get("[data-cy='"+Field[i]+"']").within(() => {
                cy.get(CustomizationSelectors.RedX).click()
            });

        });
    }

}

function DefineGetGeneralTabScreen() {
    cy.DefineRequestWait(RestAPI.GET, CustomizationURLs.GetGeneralTabScreen, CustomizationRequestAliases.GetGeneralTabScreen);
}

function AssertGetGeneralTabScreen() {
    BaseAssertion.AssertStatusCode(CustomizationRequestAliases.GetGeneralTabScreen, 200).then((interception) => {
    });
}