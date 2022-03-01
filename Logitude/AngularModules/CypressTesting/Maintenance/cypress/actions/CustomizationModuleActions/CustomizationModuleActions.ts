import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { CustomizationRuleDetails } from "../../models/CustomizationModuleDetails/CustomizationRuleDetails"
import { CustomizationSelectors } from "../../selectors/CustomizationModuleSelectors/CustomizationSelectors"
import { CustomizationConstants } from "../../constants/CustomizationConstants/CustomizationConstants";
import { CustomizationURLs } from "../../constants/CustomizationURLs/CustomizationURLs";
import { CustomizationRequestAliases } from "../../constants/CustomizationURLs/CustomizationRequestAliases";

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
        chooseitemcomboBox(CustomizationSelectors.NotificationType, "Notification Type "+customizationRuleDetails.NotificationType)
    }
}

function ChooseRuleCondition(customizationRuleDetails: CustomizationRuleDetails) {
    if (customizationRuleDetails.RuleCondition != null && customizationRuleDetails.TriggerType == "Condition") {
        cy.get(CustomizationSelectors.NewRuleFiltersSearchFields).click().type(customizationRuleDetails.RuleCondition)

        cy.get("[data-cy='" + customizationRuleDetails.RuleCondition + "']").within(() => {
            cy.get("label").click()
        })
        cy.get(CustomizationSelectors.Card).click().type(customizationRuleDetails.ConditionFieldValue)
        cy.contains(customizationRuleDetails.ConditionFieldValue).click()
    }
}
function addNewRulemoreDetails(customizationRuleDetails: CustomizationRuleDetails){
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
    return String(new Date()).substring(0,25)
}

export function SearchRule(code:string){
    cy.get(CustomizationSelectors.SearchRulesMainComponent).click().type(code)
}

export function AssertRuleExection(ValidationMessage:string){
        cy.get(CustomizationSelectors.ValidationSummary).should("contain.text", ValidationMessage)
        cy.get(CustomizationSelectors.ShipmentCancel).click()
        cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function InactivateRule(){
    cy.get(CustomizationSelectors.Edit).click({force:true})
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