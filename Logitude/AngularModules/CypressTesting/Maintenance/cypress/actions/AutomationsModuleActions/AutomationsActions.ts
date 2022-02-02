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
import * as BaseActions from "../../../../Base/cypress/actions/Actions";



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

export function AddConditions(conditionsDetails: ConditionsDetails[]) {
    let i = 0, j = 0;
    conditionsDetails.forEach(element => {
        if (element.Area == AutomationsConstants.AndArea)
            AddCondition(element, i++)
        else if (element.Area == AutomationsConstants.OrArea)
            AddCondition(element, j++)
    });
}

export function AddCondition(conditionsDetails: ConditionsDetails, conditionNo: number) {
    chooseArea(conditionsDetails.Area)
    chooseEntity(conditionsDetails.Entity, conditionsDetails.Area, conditionNo)

    cy.get("[data-cy=" + conditionsDetails.Area + AutomationsSelectors.ObjectFieldLov + conditionNo + "]").click().type(conditionsDetails.Field);
    cy.contains(conditionsDetails.Field).click()
    cy.get("[data-cy=" + conditionsDetails.Area + AutomationsSelectors.OperatorsList + "]").eq(conditionNo).click()
    chooseOperator(conditionsDetails, conditionNo)
}

function chooseArea(area: string) {
    if (area == AutomationsConstants.AndArea) {
        cy.get(AutomationsSelectors.AddAndConditionArea).click({ force: true })
    }
    else
        if (area == AutomationsConstants.OrArea) {
            cy.get(AutomationsSelectors.AddOrConditionArea).click({ force: true })
        }
}

function chooseEntity(Entity: string, Area: string, conditionNumber: number) {
    if (Entity == 'Master') {
        clickEntity(Area, AutomationsSelectors.MasterEntity, conditionNumber)
    }
    else if (Entity == 'Agent') {
        clickEntity(Area, AutomationsSelectors.AgentEntity, conditionNumber)
    }
    else if (Entity == 'Ticket') {
        clickEntity(Area, AutomationsSelectors.TicketEntity, conditionNumber)
    }
    else if (Entity == 'Shipment') {
        clickEntity(Area, AutomationsSelectors.ShipmentEntity, conditionNumber)
    }
 
}

function clickEntity(Area: string, EntitySelector: string, conditionNumber: number) {

    cy.get("[data-cy='" + Area + AutomationsSelectors.EntityList + "']").eq(conditionNumber).click()
    cy.get("[data-cy='" + Area + EntitySelector + "']").eq(conditionNumber).click()
}

function chooseOperator(conditionsDetails: ConditionsDetails, conditionNo: number) {

    if (conditionsDetails.Type.toUpperCase() == AutomationsConstants.boolean) {
        chooseBooleanOperator(conditionsDetails, conditionNo)
    }
    else if (conditionsDetails.Type.toUpperCase() == AutomationsConstants.strings) {
        chooseStringOperator(conditionsDetails.Operator, conditionsDetails.ConditionValue)
    }
    else if (conditionsDetails.Type.toUpperCase() == AutomationsConstants.date
        || conditionsDetails.Type.toUpperCase() == AutomationsConstants.number) {
        chooseDateOperator(conditionsDetails, conditionNo)
    }
}

function chooseBooleanOperator(conditionsDetails: ConditionsDetails, conditionNo: number) {

    if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.Equals) {
        cy.get(AutomationsConstants.DataCy + AutomationsSelectors.BooleanEquals + AutomationsConstants.EndDataCy).eq(conditionNo).click()
        chooseBooleanConditionValue(conditionsDetails.ConditionValue)
    }
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.IsEmpty)
        cy.get(AutomationsConstants.DataCy + conditionsDetails.Area
            + AutomationsSelectors.BooleanIsEmpty + AutomationsConstants.EndDataCy).eq(conditionNo).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.IsnotEmpty)
        cy.get(AutomationsConstants.DataCy + conditionsDetails.Area
            + AutomationsSelectors.BooleanIsnotEmpty + AutomationsConstants.EndDataCy).eq(conditionNo).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.Changed)
        cy.get(AutomationsConstants.DataCy + conditionsDetails.Area
            + AutomationsSelectors.Changed + AutomationsConstants.EndDataCy).eq(conditionNo).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.ChangedTo) {
        cy.get(AutomationsConstants.DataCy + conditionsDetails.Area
            + AutomationsSelectors.ChangedTo + AutomationsConstants.EndDataCy).eq(conditionNo).click()
        chooseBooleanConditionValue(conditionsDetails.ConditionValue)
    }
}

function chooseStringOperator(Operator: string, conditionValue: string) {
    if (Operator.toUpperCase() == AutomationsConstants.Equals) {
        cy.get(AutomationsConstants.DataCy + AutomationsSelectors.StringsEquals + AutomationsConstants.EndDataCy).click()
        cy.get(AutomationsSelectors.typeText).eq(2).type(conditionValue)
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
    let j = conditionNo
    if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.Equals)//=
    {
        cy.get("[data-cy='" + conditionsDetails.Area + AutomationsSelectors.DateEquals + "']").eq(j).click()
    }
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.DoesNotEqualField)//<>
        cy.get(AutomationsSelectors.DateDoesNotEqual).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThan)//>
        cy.get(AutomationsSelectors.DateGreaterThan).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThan)//<
        cy.get(AutomationsSelectors.DateLessThan).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThanOrEquals)//>=
        cy.get(AutomationsSelectors.DateGreaterThanOrEquals).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThanOrEquals)//<=
        cy.get(AutomationsSelectors.DateLessThanOrEquals).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.EqualsField)//=[Field]
        cy.get(AutomationsSelectors.DateEqualsField).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThanField)//> [Field]
        cy.get(AutomationsSelectors.DateGreaterThanField).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThanField)//> [Field]
        cy.get(AutomationsSelectors.DateLessThanField).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.DoesNotEqualField)//<> [Field]
        cy.get(AutomationsSelectors.DateDoesNotEqualField).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.LessThanOrEqualsField)//<= [Field]
        cy.get(AutomationsSelectors.DateLessThanOrEqualsField).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.GreaterThanOrEqualsField)//>= [Field]
        cy.get(AutomationsSelectors.DateGreaterThanOrEqualsField).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.Changed)
        cy.get(AutomationsSelectors.Changed).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.IsEmpty)
        cy.get(AutomationsSelectors.DateIsEmpty).eq(j).click()
    else if (conditionsDetails.Operator.toUpperCase() == AutomationsConstants.IsnotEmpty)
        cy.get(AutomationsSelectors.DateIsnotEmpty).eq(j).click()

    if (conditionsDetails.OperatorValue != null) { /// to sepatrate function 
        if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.TodayMinuse)//@Today-
        { setDateOperatorAndValue(AutomationsSelectors.TodayMinuse, conditionsDetails, conditionNo) }
        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.TodayPlus)//@Today+
            setDateOperatorAndValue(AutomationsSelectors.TodayPluse,
                conditionsDetails, conditionNo)

        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.OldValueMinuse)//@Old Value-
            setDateOperatorAndValue(AutomationsSelectors.OldValueMinuse, conditionsDetails, conditionNo)

        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.OldValuePlus)//@Old Value+
            setDateOperatorAndValue(AutomationsSelectors.OldValuePluse, conditionsDetails, conditionNo)

        else if (conditionsDetails.OperatorValue.toUpperCase() == AutomationsConstants.Date)//Date
        {
            cy.get(AutomationsSelectors.Date).click()
            cy.get(AutomationsSelectors.EnterDate).type(conditionsDetails.ConditionValue)
        }
    }
}
function setDateOperatorAndValue(OperatorSelector: string, conditionsDetails: ConditionsDetails, conditionNo: number) {

    cy.get("[data-cy='" + conditionsDetails.Area + AutomationsSelectors.DateOperatorsList + "']").eq(conditionNo).click()

    cy.get("[data-cy='" + conditionsDetails.Area + OperatorSelector + "']").eq(conditionNo).click()

    cy.get("[data-cy='" + conditionsDetails.Area + AutomationsSelectors.Value + "']").
        eq(conditionNo).click().type(conditionsDetails.ConditionValue)
}

function chooseBooleanConditionValue(conditionValue: string) {
    cy.get(AutomationsSelectors.EntityList).eq(2).click()
    if (conditionValue.toUpperCase() == AutomationsConstants.True)
        cy.contains('true').click()
    else if (conditionValue.toUpperCase() == AutomationsConstants.False)
        cy.contains('false').click()
}

export function addResultSetFieldsValue(setFieldValueResultDetails: SetFieldValueResultDetails) {
    cy.get(AutomationsSelectors.AutomationResultList).click()
    cy.get(AutomationsSelectors.SetFieldsValue).click()
    cy.get(AutomationsSelectors.AddFieldinSetValueArea).click({ force: true })
    cy.get(AutomationsSelectors.SetAutomationObjectFieldLov0).click().type(setFieldValueResultDetails.Field);
    cy.contains(setFieldValueResultDetails.Field).click()
    chooeResultOperator(setFieldValueResultDetails)
}
function chooeResultOperator(setFieldValueResultDetails: SetFieldValueResultDetails) {
    cy.get(AutomationsSelectors.SetOperatorsList).click()

    if (setFieldValueResultDetails.Operator.toUpperCase() == AutomationsConstants.SetConstantValue) {
        cy.get(AutomationsSelectors.SetConstantValue).click({ force: true })

        if (setFieldValueResultDetails.Fieldtype.toUpperCase() == AutomationsConstants.date &&
            setFieldValueResultDetails.Operatorvalue.toUpperCase() == AutomationsConstants.date)//@Today+
        {
            cy.get(AutomationsSelectors.SetDateOperatorsList).click()
            cy.get(AutomationsSelectors.SetDate).click()
            cy.get(AutomationsSelectors.EnterDate).type(setFieldValueResultDetails.Value)
        }
        else { cy.get(AutomationsSelectors.SetValue).click().type(setFieldValueResultDetails.Value); }
    }
    else if (setFieldValueResultDetails.Operator.toUpperCase() == AutomationsConstants.SetValueFromField) {
        cy.get(AutomationsSelectors.SetValueFromField).click({ force: true })
        cy.get(AutomationsSelectors.SetAutomationObjectFieldLov0).eq(1).type(setFieldValueResultDetails.Value)
        cy.contains(setFieldValueResultDetails.Value).click()
    }
}

export function addResultEmail(emailResultDetails: EmailResultDetails) { // FUNCTION NAMES ARE CAPITAL LETTERS
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

    cy.get("[data-cy='DocumentTypeTemplatesList']").click()//    Document Template
    cy.contains(AutomationsConstants.Salesman).click()
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
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePostAutomationRequest() {
    cy.DefineRequestWait(RestAPI.POST, AutomationsURLs.Automation, AutomationsRequestAliases.PostNewAutomation);
}

export function AssertAddAutomation() {
    AssertPostAutomation();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function AssertPostAutomation() {
    BaseAssertion.AssertStatusCode(AutomationsRequestAliases.PostNewAutomation, 200).then((interception) => {
    });
}

export function saveAutomation() {
    DefinePutAutomationRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutAutomationRequest() {
    cy.DefineRequestWait(RestAPI.PUT, AutomationsURLs.EditAutomation, AutomationsRequestAliases.PutNewAutomation);
}

export function AssertsaveAutomation() {
    AssertPutAutomation();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function AssertPutAutomation() {
    BaseAssertion.AssertStatusCode(AutomationsRequestAliases.PutNewAutomation, 200).then((interception) => {
    });
}
export function inactivateAutomation() {
    cy.get(AutomationsSelectors.Inactive).click()

}
export function AssertSFVAutomationExecution(setFieldValueResultDetailes: SetFieldValueResultDetails, AutomaionName: string) {

    cy.get(AutomationsSelectors.ShipmentAudit).click()
    cy.wait(10000)
    cy.get(AutomationsSelectors.Refresh).dblclick()
    BaseAssertion.AssertElementExist(AutomationsSelectors.MiniGreenTick)

    cy.contains(setFieldValueResultDetailes.Field)
    cy.get(AutomationsSelectors.TabControlBody).eq(1).within(() => {
        cy.get(AutomationsSelectors.SimpleGridViewRowRowHover).eq(1).within(() => {
            cy.contains(AddDaysToTodayDateMonthName(setFieldValueResultDetailes.Value, setFieldValueResultDetailes.Operatorvalue))
        })
    })
    cy.get(AutomationsSelectors.AutomationTab).click()
    cy.get(AutomationsSelectors.TabControlBody).eq(1).within(() => {
        cy.get(AutomationsSelectors.MiniGreenTick).should("exist");
        cy.contains(AutomaionName)
    })

    cy.get("[id='ShipmentTHCustoms']").click()
    BaseAssertion.AssertElementHaveValue("[id='date_Shipment_CustomsClearanceDate']",
        AddDaysToTodayDate(setFieldValueResultDetailes.Value, setFieldValueResultDetailes.Operatorvalue))
    cy.get(AutomationsSelectors.ShipmentSaveClose).click()
}

export function chooseFirstAutomation(onActionType: string) {
    if (onActionType.toUpperCase() == AutomationsConstants.OnUpdate) {
        cy.get(AutomationsSelectors.OnUpdateAutomationTabTitle).click()
        cy.get(AutomationsSelectors.SimpleGridViewBody).eq(1).within(() => {
            cy.get(AutomationsSelectors.Edit).eq(0).click()
        })
    }
    else if (onActionType.toUpperCase() == AutomationsConstants.OnCreate)
        cy.get(AutomationsSelectors.EditAutomation).eq(0).click()
}

export function AddDaysToTodayDate(days: string, Operatorvalue: string) {

    var date = new Date();
    if (Operatorvalue.toUpperCase() == AutomationsConstants.date)
        date.setDate(date.getDate());
    else
        date.setDate(date.getDate() + parseInt(days));

    return FormateTheDateString(date.toDateString().split(" "))

}

function FormateTheDateString(dateList: string[]) {

    var dd = dateList[2];

    var mm = GetMonth(dateList[1])

    var yyyy = dateList[3];

    var DateFormat = dd + '/' + mm + '/' + yyyy;

    return DateFormat;

}

export function AddDaysToTodayDateMonthName(days: string, Operatorvalue: string) {

    var date = new Date();
    if (Operatorvalue.toUpperCase() == AutomationsConstants.date)
        date.setDate(date.getDate());
    else
        date.setDate(date.getDate() + parseInt(days));

    return FormateTheDateStringMonthName(date.toDateString().split(" "))

}

function FormateTheDateStringMonthName(dateList: string[]) {

    var dd = dateList[2];

    var mm = dateList[1];

    var yyyy = dateList[3];

    var DateFormat = dd + ' ' + mm + ' ' + yyyy;

    return DateFormat;

}
function GetMonth(monthNum: string) {

    switch (monthNum) {

        case "Jan": return "01";

        case "Feb": return "02";

        case "Mar": return "03";

        case "Apr": return "04";

        case "May": return "05";

        case "Jun": return "06";

        case "Jul": return "07";

        case "Aug": return "08";

        case "Sep": return "09";

        case "Oct": return "10";

        case "Nov": return "11";

        case "Dec": return "12";

    }

}

export function assertAutomationNotExecuted(AutomaionName: string) {
    cy.get(AutomationsSelectors.ShipmentAudit).click()
    cy.wait(10000)
    cy.get(AutomationsSelectors.Refresh).dblclick()
    BaseAssertion.AssertElementNotExist(AutomationsSelectors.MiniGreenTick)

    cy.get(AutomationsSelectors.AutomationTab).click()
    cy.get(AutomationsSelectors.TabControlBody).eq(1).within(() => {
        cy.get(AutomationsSelectors.SimpleGridViewRowRowHover).within(() => {
            cy.get(AutomationsSelectors.RedX).should("exist");
            cy.contains(AutomaionName)
        })
    })
    cy.get(AutomationsSelectors.ShipmentSaveClose).click()
}

export function AssertFUCAutomationExecution(followUpCreationDetails: FollowUpCreationDetails, AutomaionName: string) {

    cy.get(AutomationsSelectors.ShipmentAudit).click()
    cy.wait(10000)
    cy.get(AutomationsSelectors.Refresh).dblclick()
    BaseAssertion.AssertElementExist(AutomationsSelectors.MiniGreenTick)

    cy.get(AutomationsSelectors.AutomationTab).click()
    cy.get(AutomationsSelectors.TabControlBody).eq(1).within(() => {
        cy.get(AutomationsSelectors.MiniGreenTick).should("exist");
        cy.contains(AutomaionName)
    })

    cy.get("img[src='./_Resources/Images/Icons/Followups/Followup_Black.png']").eq(1).click()
    cy.get("[class='LogitudeHelperFollowupsBody']").within(() => {
        //cy.contains(followUpCreationDetails.FollowUpType)
        cy.contains(followUpCreationDetails.Notes)
    })
    cy.get(AutomationsSelectors.ShipmentSaveClose).click()

}