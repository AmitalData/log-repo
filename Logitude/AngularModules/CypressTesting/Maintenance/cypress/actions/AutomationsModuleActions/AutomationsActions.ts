import { AutomationsDetails } from "../../models/AutomationsModuleDetails/AutomationsDetails"
import { AutomationsSelectors } from "../../selectors/AutomationsModulesSelectors/AutomationsSelectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { ConditionsDetails } from "../../models/AutomationsModuleDetails/ConditionsDetails"
import { AutomationsConstants } from "../../constants/AutomationsConstants/AutomationsConstants";
import { SetFieldValueResultDetails } from "../../models/AutomationsModuleDetails/SetFieldValueResultDetails"
import { EmailResultDetails } from "../../models/AutomationsModuleDetails/EmailResultDetails"
import { FollowUpCreationDetails } from "../../models/AutomationsModuleDetails/FollowUpCreationDetails"
import { AutomationsURLs } from "../../constants/AutomationsURLs/AutomationsURLs";
import { AutomationsRequestAliases } from "../../constants/AutomationsURLs/AutomationsRequestAliases";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import { contains } from "cypress/types/jquery";
import { values } from "cypress/types/lodash";

export function OpenAutomationMenu() {
    cy.Click(AutomationsSelectors.AutomationsMenu, null)
}

export function AddNewAutomation(onActionType: string, automationDetails: AutomationsDetails) {
    if (onActionType.toUpperCase() == AutomationsConstants.OnUpdate) {
        cy.get(AutomationsSelectors.OnUpdateAutomationTabTitle).click()
        cy.get(AutomationsSelectors.AddAutomationOnUpdate).click()
    }
    else if (onActionType.toUpperCase() == AutomationsConstants.OnCreate)
        cy.get(AutomationsSelectors.AddAutomationOnCreate).click()
    cy.FillLogTextBox(AutomationsSelectors.AutomationName, automationDetails.Name)
    cy.FillLogTextBox(AutomationsSelectors.AutomationDescription, automationDetails.Description)
}

export function AddCondition(conditionsDetails: ConditionsDetails, conditionNo: number) {
    chooseArea(conditionsDetails.Area)
    chooseEntity(conditionsDetails.Entity, conditionNo)
    if (conditionNo > 0) {
        cy.get(AutomationsSelectors.ConditionField).eq(1).click().type(conditionsDetails.Field);
        cy.contains(conditionsDetails.Field).click()
        cy.get(AutomationsSelectors.OperatorsList).eq(1).click()
    }
    else {
        cy.SelectDropDownListItem(AutomationsSelectors.ConditionField, conditionsDetails.Field)
        cy.get(AutomationsSelectors.OperatorsList).click()
    }
    chooseOperator(conditionsDetails, conditionNo)
}

function chooseArea(area: string) {
    if (area.toUpperCase() == AutomationsConstants.AndArea) {
        cy.get(AutomationsSelectors.AddAndConditionArea).click({ force: true })
    }
    else {
        if (area.toUpperCase() == AutomationsConstants.OrArea)
            cy.get(AutomationsSelectors.AddOrConditionArea).click({ force: true })
    }
}

function chooseEntity(Entity: string, conditionNumber: number) {

    if (Entity == 'Master') {
        clickEntity(AutomationsSelectors.MasterEntity,conditionNumber)
    }
    else if (Entity == 'Agent'){
        clickEntity(AutomationsSelectors.AgentEntity,conditionNumber)  
    }
    else if (Entity == 'Ticket'){
        clickEntity(AutomationsSelectors.TicketEntity,conditionNumber) 
    }
    else if (Entity =='Shipment'){
        clickEntity(AutomationsSelectors.ShipmentEntity,conditionNumber) 
    }
}
 function clickEntity(EntitySelector : string, conditionNumber){
    if (conditionNumber > 0) {
        cy.get(AutomationsSelectors.EntityList).eq(1).click()
        cy.get(EntitySelector).eq(conditionNumber).click()
    }
    else {
        cy.get(AutomationsSelectors.EntityList).eq(0).click()
        cy.get(EntitySelector).click()
    }
 }
function chooseOperator(conditionsDetails: ConditionsDetails, conditionNo: number) {

    if (conditionsDetails.Type.toUpperCase() == AutomationsConstants.boolean) {
        chooseBooleanOperator(conditionsDetails.Operator, conditionsDetails.ConditionValue,)
    }
    else if (conditionsDetails.Type.toUpperCase() == AutomationsConstants.strings)
        chooseStringOperator(conditionsDetails.Operator, conditionsDetails.ConditionValue)
    else if (conditionsDetails.Type.toUpperCase() == AutomationsConstants.date || conditionsDetails.Type.toUpperCase() == AutomationsConstants.number)
        chooseDateOperator(conditionsDetails, conditionNo)
}

function chooseBooleanOperator(Operator: string, conditionValue: string) {

    if (Operator.toUpperCase() == AutomationsConstants.Equals) {
        cy.get(AutomationsSelectors.BooleanEquals).click()
        chooseBooleanConditionValue(conditionValue)
    }
    else if (Operator.toUpperCase() == AutomationsConstants.IsEmpty)
        cy.get(AutomationsSelectors.BooleanIsEmpty).click()
    else if (Operator.toUpperCase() == AutomationsConstants.IsnotEmpty)
        cy.get(AutomationsSelectors.BooleanIsnotEmpty).click()
    else if (Operator.toUpperCase() == AutomationsConstants.Changed)
        cy.get(AutomationsSelectors.Changed).click()
    else if (Operator.toUpperCase() == AutomationsConstants.ChangedTo) {
        cy.get(AutomationsSelectors.ChangedTo).click()
        chooseBooleanConditionValue(conditionValue)
    }
}

function chooseStringOperator(Operator: string, conditionValue: string) {
    if (Operator.toUpperCase() == AutomationsConstants.Equals) {
        cy.get(AutomationsSelectors.StringsEquals).click()
        cy.get("[type='text']").eq(2).type(conditionValue)
    }
    else if (Operator.toUpperCase() == AutomationsConstants.DoesNotEqual)

        cy.get(AutomationsSelectors.StringsDoesNotEqual).click()
    else if (Operator.toUpperCase() == AutomationsConstants.Contains)
        cy.get(AutomationsSelectors.StringsContains).click()
    else if (Operator.toUpperCase() == AutomationsConstants.DoesNotContain)
        cy.get(AutomationsSelectors.StringsDoesNotContain).click()
    else if (Operator.toUpperCase() == AutomationsConstants.EqualsField)
        cy.get(AutomationsSelectors.StringsEqualField).click()
    else if (Operator.toUpperCase() == AutomationsConstants.DoesNotEqualField)
        cy.get(AutomationsSelectors.StringsDoesNotEqualField).click()
    else if (Operator.toUpperCase() == AutomationsConstants.DoesNotContainField)
        cy.get(AutomationsSelectors.StringsDoesNotContainField).click()
    else if (Operator.toUpperCase() == AutomationsConstants.IsEmpty)
        cy.get(AutomationsSelectors.StringsIsEmpty).click()
    else if (Operator.toUpperCase() == AutomationsConstants.IsnotEmpty)
        cy.get(AutomationsSelectors.StringsIsnotEmpty).click()
}
function chooseDateOperator(conditionsDetails: ConditionsDetails, conditionNo: number) {

    if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.Equals)//=
    {
        if (conditionNo > 0) cy.get(AutomationsSelectors.DateEquals).eq(1).click()
        else cy.get(AutomationsSelectors.DateEquals).click()
    }
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.DoesNotEqualField)//<>
        cy.get(AutomationsSelectors.DateDoesNotEqual).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThan)//>
        cy.get(AutomationsSelectors.DateGreaterThan).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThan)//<
        cy.get(AutomationsSelectors.DateLessThan).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThanOrEquals)//>=
        cy.get(AutomationsSelectors.DateGreaterThanOrEquals).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThanOrEquals)//<=
        cy.get(AutomationsSelectors.DateLessThanOrEquals).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.EqualsField)//=[Field]
        cy.get(AutomationsSelectors.DateEqualsField).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThanField)//> [Field]
        cy.get(AutomationsSelectors.DateGreaterThanField).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThanField)//> [Field]
        cy.get(AutomationsSelectors.DateLessThanField).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.DoesNotEqualField)//<> [Field]
        cy.get(AutomationsSelectors.DateDoesNotEqualField).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThanOrEqualsField)//<= [Field]
        cy.get(AutomationsSelectors.DateLessThanOrEqualsField).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThanOrEqualsField)//>= [Field]
        cy.get(AutomationsSelectors.DateGreaterThanOrEqualsField).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.Changed)
        cy.get(AutomationsSelectors.Changed).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.IsEmpty)
        cy.get(AutomationsSelectors.DateIsEmpty).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.IsnotEmpty)
        cy.get(AutomationsSelectors.DateIsnotEmpty).click()

    if (conditionsDetails.OperatorValue != null) {
        if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.TodayMinuse)//@Today-
        { setDateOperatorAndValue(AutomationsSelectors.TodayMinuse, conditionsDetails.ConditionValue, conditionNo) }
        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.TodayPlus)//@Today+
            setDateOperatorAndValue(AutomationsSelectors.TodayPluse, conditionsDetails.ConditionValue, conditionNo)

        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.OldValueMinuse)//@Old Value-
            setDateOperatorAndValue(AutomationsSelectors.OldValueMinuse, conditionsDetails.ConditionValue, conditionNo)

        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.OldValuePlus)//@Old Value+
            setDateOperatorAndValue(AutomationsSelectors.OldValuePluse, conditionsDetails.ConditionValue, conditionNo)

        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.Date)//Date
        {
            cy.get(AutomationsSelectors.Date).click()
            cy.get("[placeholder='Enter Date']").type(conditionsDetails.ConditionValue)
        }
    }
}
function setDateOperatorAndValue(valueOperator: string, value: string, conditionNo: number) {
    if (conditionNo > 0) {//AutomationsSelectors.DateOperatorsList
        cy.get('comboBox').eq(5).click()
        cy.get(valueOperator).eq(1).click()
        cy.get("[type='number']").eq(1).type(value)
    }
    else {//cy.SelectDropDownListItem(AutomationsSelectors.DateOperatorsList,valueOperator)
        //AutomationsSelectors.DateOperatorsList
        cy.get('comboBox').eq(2).click()
        cy.get(valueOperator).click()
        cy.get("[type='number']").type(value) //"[data-cy='Value']"
    }
}

function chooseBooleanConditionValue(conditionValue: string) {
    cy.get(AutomationsSelectors.EntityList).eq(2).click()
    if (conditionValue.toUpperCase() == 'TRUE')
        cy.contains('true').click()
    else if (conditionValue.toUpperCase() == 'FALSE')
        cy.contains('false').click()
}

export function addResultSetFieldsValue(setFieldValueResultDetails: SetFieldValueResultDetails) {
    cy.get(AutomationsSelectors.AutomationResultList).click()
    cy.get(AutomationsSelectors.SetFieldsValue).click()
    cy.get(AutomationsSelectors.AddFieldinSetValueArea).click({ force: true })
    let selector = cy.get("[class='LogLovInputDiv']").eq(1)
    selector.click().type(setFieldValueResultDetails.Field);
    cy.contains(setFieldValueResultDetails.Field).click()
    chooeResultOperator(setFieldValueResultDetails)
}
function chooeResultOperator(setFieldValueResultDetails: SetFieldValueResultDetails) {
    cy.get("[class='TextTrimming']").eq(3).click()

    if (setFieldValueResultDetails.Operator.toUpperCase() == AutomationsConstants.SetConstantValue) {
        cy.get(AutomationsSelectors.SetConstantValue).click({ force: true })

        if (setFieldValueResultDetails.Fieldtype.toUpperCase() == AutomationsConstants.date &&
            setFieldValueResultDetails.Operatorvalue.toUpperCase() == AutomationsConstants.date)//@Today-
        {
            cy.get("[class='ComboBox']").eq(4).click()
            cy.get(AutomationsSelectors.Date).click()
            cy.get("[placeholder='Enter Date']").type(setFieldValueResultDetails.Value)
        }
        else {
            cy.get("[type='text']").eq(2).click().type(setFieldValueResultDetails.Value);
        }
    }
    else if (setFieldValueResultDetails.Operator.toUpperCase() == AutomationsConstants.SetValueFromField) {
        cy.get(AutomationsSelectors.SetValueFromField).click({ force: true })
        cy.get("[class='LogLovInputDiv']").eq(2).type(setFieldValueResultDetails.Value)
        cy.contains(setFieldValueResultDetails.Value).click()
    }
}

export function addResultEmail(emailResultDetails: EmailResultDetails) {
    cy.get(AutomationsSelectors.AutomationResultList).click()
    cy.get(AutomationsSelectors.Email).click()

    if (emailResultDetails.Type.toUpperCase() == AutomationsConstants.Immediately)
        cy.get(AutomationsSelectors.Immediately).click()
    else if (emailResultDetails.Type.toUpperCase() == AutomationsConstants.Delayed)
        cy.get(AutomationsSelectors.Delayed).click()

    cy.get(AutomationsSelectors.DocumentTypeList).click()
    if (emailResultDetails.Document.toUpperCase() == AutomationsConstants.ShippingDeclaration)
        cy.get(AutomationsSelectors.ShippingDeclaration).click()
    else if (emailResultDetails.Document.toUpperCase() == AutomationsConstants.DeliveryNoteDoc)
        cy.get(AutomationsSelectors.DeliveryNoteDoc).click()

    cy.get("[class='ComboBox']").eq(6).click()//    Document Template
    cy.contains('Salesman').click()
}

export function addResultFUCreation(followUpCreationDetails: FollowUpCreationDetails) {
    cy.get(AutomationsSelectors.AutomationResultList).click()
    cy.get(AutomationsSelectors.FUCreation).click()

    if (followUpCreationDetails.Type.toUpperCase() == AutomationsConstants.Immediately)
        cy.get(AutomationsSelectors.Immediately).click()
    else if (followUpCreationDetails.Type.toUpperCase() == AutomationsConstants.Delayed)
        cy.get(AutomationsSelectors.Delayed).click()

    cy.get(AutomationsSelectors.AutomationFollowUpList).click()
    cy.get(AutomationsSelectors.AutomationFollowUpList).within(() => {
        cy.get("ul > li").contains(followUpCreationDetails.FollowUpType).click()
    })
    cy.get(AutomationsSelectors.FollowUpOwnerList).click()
    cy.get(AutomationsSelectors.FollowUpOwnerList).within(() => {
        cy.get("ul > li").contains(followUpCreationDetails.Owner).click()
    })
    cy.get(AutomationsSelectors.FollowUpDateList).click()
    cy.get(AutomationsSelectors.FollowUpDateList).within(() => {
        cy.get("ul > li").contains(followUpCreationDetails.Date).click()
    })
    cy.get(AutomationsSelectors.FollowUpNote).type(followUpCreationDetails.Notes)

}

export function addAutomation() {
    DefinePostAutomationRequest();
    cy.Click(BaseSelectors.RedButton + ":last", null);
}
function DefinePostAutomationRequest() {
    cy.DefineRequestWait(RestAPI.POST, AutomationsURLs.Automation, AutomationsRequestAliases.PostNewAutomation);
}

export function AssertAddAutomation() {
    AssertPostAutomation();
    cy.Click(BaseSelectors.RedButton + ":last", null);
}
function AssertPostAutomation() {
    BaseAssertion.AssertStatusCode(AutomationsRequestAliases.PostNewAutomation, 200).then((interception) => {
    });
}

export function saveAutomation() {
    DefinePutAutomationRequest();
    cy.Click(BaseSelectors.RedButton + ":last", null);
}
function DefinePutAutomationRequest() {
    cy.DefineRequestWait(RestAPI.PUT, AutomationsURLs.EditAutomation, AutomationsRequestAliases.PutNewAutomation);
}

export function AssertsaveAutomation() {
    AssertPutAutomation();
    cy.Click(BaseSelectors.RedButton + ":last", null);
}
function AssertPutAutomation() {
    BaseAssertion.AssertStatusCode(AutomationsRequestAliases.PutNewAutomation, 200).then((interception) => {
    });
}
export function inactivateAutomation() {
    cy.get(AutomationsSelectors.Inactive).click()

}
export function assertAutomationExcution(Field: string, Value: string,AutomaionName:string) {
cy.get(AutomationsSelectors.ShipmentAudit).click()
cy.wait(10000)
cy.get("[data-cy='Refresh']").dblclick()  
BaseAssertion.AssertElementExist("img[src='./Images/Icons/MiniGreenTick.png']")
cy.contains(Field)
//cy.contains(Value)
//BaseAssertion.AssertElementHaveValue("[data-cy='"+Field+"']",Field)
//BaseAssertion.AssertElementHaveValue("[data-cy='"+Value+"']",Value)
cy.get("[data-cy='Automation']").click()
cy.get("[class='TabControlBody']").eq(1).within(() => {
    cy.get("img[src='./Images/Icons/MiniGreenTick.png']").should("exist");
//BaseAssertion.AssertElementExist("img[src='./Images/Icons/MiniGreenTick.png']")
cy.contains(AutomaionName)
//BaseAssertion.AssertElementHaveValue("[title='"+AutomaionName+"']",AutomaionName)
})
cy.get("[id='Shipment-SaveClose']").click()
}
export function chooseFirstAutomation(onActionType:string){
    if (onActionType.toUpperCase() == AutomationsConstants.OnUpdate) {
        cy.get(AutomationsSelectors.OnUpdateAutomationTabTitle).click()
        cy.get("[class='SimpleGridViewBody']").eq(1).within(()=>{
        cy.get("img[src='./Images/Buttons/Edit.png']").eq(0).click()
    })
    }
    else if (onActionType.toUpperCase() == AutomationsConstants.OnCreate)
        cy.get(AutomationsSelectors.EditAutomation).eq(0).click()
}